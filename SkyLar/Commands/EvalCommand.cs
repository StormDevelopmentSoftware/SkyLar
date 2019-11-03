using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SkyLar.Attributes;

namespace SkyLar.Commands
{
    public class EvalCommand : BaseCommandModule
    {
        static readonly Regex PASTEBIN_REGEX = new Regex(@"(http|https)\:\/\/pastebin\.com\/(raw\/)?(.+)", RegexOptions.ECMAScript);

        public static Lazy<string> IMPORTS_LAZY => new Lazy<string>(() =>
        {
            return string.Join("\n", new[]
            {
                "System",
                "System.Linq",
                "System.Text",
                "System.Collections.Generic",
                "DSharpPlus",
                "DSharpPlus.Entities",
                "DSharpPlus.CommandsNext",
                "SkyLar",
                "Utilities = SkyLar.Utilities"
            }.Select(x => $"using {x};"));
        });

        public static string IMPORTS => IMPORTS_LAZY.Value;

        [Command]
        [CommandCategory(Category.Developer)]
        [DeveloperCommand]
        public async Task EvalAsync(CommandContext ctx, [RemainingText] string query = null)
        {
            var watch = Stopwatch.StartNew();

            var bdeb = new DiscordEmbedBuilder()
                .WithAuthor("SkyLar: Eval", iconUrl: ctx.Client.CurrentUser.GetAvatarUrl(ImageFormat.Png))
                .WithFooter($"Requested by {ctx.User.Username}#{ctx.User.Discriminator}", ctx.User.GetAvatarUrl(ImageFormat.Png))
                .WithTimestamp(DateTime.UtcNow)
                .Build();

            var message = await ctx.RespondAsync(embed: new DiscordEmbedBuilder(bdeb)
                .WithColor(DiscordColor.Purple)
                .AddField("Elapsed Time", watch.Elapsed.ToString(@"m\:ss"))
                .AddField("Status", "Validating")
                .Build());

            if (string.IsNullOrEmpty(query))
            {
                if (ctx.Message.Attachments?.Any() == false)
                    goto error;
                else
                {
                    var attachment = ctx.Message.Attachments.First();
                    var (success, raw) = await Utilities.DownloadStringAsync(attachment.Url);

                    if (!success || string.IsNullOrEmpty(raw))
                        goto error;

                    query = raw;
                }
            }
            else if (PASTEBIN_REGEX.HasMatch(query, out var match))
            {
                var (success, raw) = await Utilities.DownloadStringAsync($"https://pastebin.com/raw/{match.Groups[3].Value}");

                if (!success || string.IsNullOrEmpty(raw))
                    goto error;

                query = raw;
            }
            else if (Uri.TryCreate(query, UriKind.Absolute, out var uri))
            {
                var (success, raw) = await Utilities.DownloadStringAsync(uri.ToString());

                if (!success || string.IsNullOrEmpty(raw))
                    goto error;

                query = raw;
            }

            if (string.IsNullOrEmpty(query))
                goto error;
            else
                goto eval;

            error:
            {
                await message.ModifyAsync(embed: new DiscordEmbedBuilder(bdeb)
                    .WithColor(DiscordColor.Red)
                    .WithDescription(":x: You need provide an source code, url or attached file to evulate.")
                    .Build());
                return;
            }

        eval:
            {
                // forçar a engine do C# entender que agora estamos na "linha 1" do script
                // (é bom pra ajudar para retorno de exceções).

                query = string.Concat(IMPORTS, "\n", "#line 1 \"Evulation\"", "\n", query);

                await message.ModifyAsync(embed: new DiscordEmbedBuilder(bdeb)
                    .WithColor(DiscordColor.Cyan)
                    .AddField("Elapsed Time", watch.Elapsed.ToString(@"m\:ss"))
                    .AddField("Status", "Building")
                    .Build());

                var options = ScriptOptions.Default
                    .WithAllowUnsafe(true)
                    .WithCheckOverflow(true)
                    .WithEmitDebugInformation(false)
                    .WithLanguageVersion(LanguageVersion.CSharp8)
                    .WithOptimizationLevel(OptimizationLevel.Release)
                    .WithReferences(AppDomain.CurrentDomain.GetAssemblies()
                        .Where(xa => !xa.IsDynamic && !string.IsNullOrEmpty(xa.Location)));

                var env = new SkyLarEvulationEnvironment
                {
                    Context = ctx,
                    Shard = SkyLarBot.GetShard(ctx.Client.ShardId)
                };

                var script = CSharpScript.Create(query, options, typeof(SkyLarEvulationEnvironment));

                await Task.Delay(500); // previnir rate-limit.

                await message.ModifyAsync(embed: new DiscordEmbedBuilder(bdeb)
                    .WithColor(DiscordColor.Orange)
                    .AddField("Elapsed Time", watch.Elapsed.ToString(@"m\:ss"))
                    .AddField("Status", "Linking")
                    .Build());

                await Task.Delay(500); // previnir rate-limit.

                var diagnostics = script.Compile();

                if(diagnostics.Any(x => x.Severity == DiagnosticSeverity.Error))
                {
                    var text = string.Empty;

                    foreach(var dg in diagnostics)
                    {
                        if(text.Length < 2000)
                        {
                            var ls = dg.Location.GetMappedLineSpan();
                            var line = $"- [**`{ls.StartLinePosition.Line};{ls.StartLinePosition.Character}`**]: {dg.GetMessage()}";
                            text += line + "\n";
                        }
                    }

                    var xerror = await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
                        .WithAuthor("SkyLar: Linking Failed", iconUrl: ctx.Client.CurrentUser.GetAvatarUrl(ImageFormat.Png))
                        .WithColor(DiscordColor.Red)
                        .WithFooter($"Requested by {ctx.User.Username}#{ctx.User.Discriminator}", ctx.User.GetAvatarUrl(ImageFormat.Png))
                        .WithDescription(text));

                    GC.Collect();
                    diagnostics = default;
                    goto gc;
                }

                await message.ModifyAsync(embed: new DiscordEmbedBuilder(bdeb)
                    .WithColor(DiscordColor.Yellow)
                    .AddField("Elapsed Time", watch.Elapsed.ToString(@"m\:ss"))
                    .AddField("Status", "Executing")
                    .Build());

                await Task.Delay(500); // previnir rate-limit.;

                try
                {
                    var result = await script.RunAsync(env);

                    if (result.ReturnValue is null)
                    {
                        await ctx.RespondAsync(ctx.User.Mention, embed: new DiscordEmbedBuilder(bdeb)
                            .WithColor(DiscordColor.Green)
                            .AddField("Elapsed Time", watch.Elapsed.ToString(@"m\:ss"))
                            .AddField("Status", "Success")
                            .AddField("Return Value", "Nothing was return.")
                            .Build());
                    }
                    else
                    {
                        var value = result.ReturnValue.ToString();
                        var extra = "\n*[...]*";

                        if (value.Length > 1000)
                            value = Formatter.BlockCode(value.Substring(0, ((1000 - extra.Length) - 6) - 8), "cs") + extra;
                        else
                            value = Formatter.BlockCode(value, "cs");

                        await ctx.RespondAsync(ctx.User.Mention, embed: new DiscordEmbedBuilder(bdeb)
                            .WithColor(DiscordColor.Green)
                            .AddField("Elapsed Time", watch.Elapsed.ToString(@"m\:ss"))
                            .AddField("Status", "Success")
                            .AddField("Return Value", value)
                            .Build());
                    }

                    goto gc;
                }
                catch (Exception ex)
                {
                    var value = ex.ToString();

                    if (value.Length > 1000)
                        value = value.Substring(0, 10000);
                    else
                        value = Formatter.BlockCode(value, "cs");

                    await ctx.RespondAsync(ctx.User.Mention, embed: new DiscordEmbedBuilder(bdeb)
                            .WithColor(DiscordColor.Red)
                            .AddField("Elapsed Time", watch.Elapsed.ToString(@"m\:ss"))
                            .AddField("Status", "Failed")
                            .AddField("Exception", value)
                            .Build());

                    goto gc;
                }

            gc:
                {
                    // isso faz uma limpesa na memória do bot
                    // preveindo que restos do eval que acabou de executar fiquem persistentes
                    // e assim evitando o memory leak.

                    GC.Collect();

                    try { await message.DeleteAsync(); } catch { }

                    options = null;
                    script = null;
                    env = null;

                    GC.Collect();
                }
            }
        }

        public class SkyLarEvulationEnvironment
        {
            public CommandContext Context { get; set; }
            public SkyLarBot Shard { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using SkyLar.Attributes;
using SkyLar.Utilities;

namespace SkyLar
{
    public class HelpFormatter : BaseHelpFormatter
    {
        private CommandContext _ctx;
        public DiscordEmbedBuilder EmbedBuilder { get; }
        private Command command { get; set; }
        public List<Command> InfoCommands = new List<Command>();
        public List<Command> DeveloperCommands = new List<Command>();
        public List<Command> UtilityCommands = new List<Command>();
        public List<Command> FunCommands = new List<Command>();

        public Dictionary<Category, List<Command>> dict = new Dictionary<Category, List<Command>>();
        public Dictionary<List<Command>, string> evalText = new Dictionary<List<Command>, string>();
        public Dictionary<Command, string[]> examples = new Dictionary<Command, string[]>();

        public HelpFormatter(CommandContext ctx) : base(ctx)
        {

            EmbedBuilder = new DiscordEmbedBuilder()
            {
                Title = ":question: **Help**",
                Color = DiscordColor.Blurple
            };


            _ctx = ctx;

            dict = new Dictionary<Category, List<Command>> { { Category.Info, InfoCommands }, { Category.Developer, DeveloperCommands }, { Category.Utility, UtilityCommands }, { Category.Fun, FunCommands } };

            evalText = new Dictionary<List<Command>, string> { { InfoCommands, ":information_source: • Information" }, { DeveloperCommands, "<:netcore:378151776320487424> • Developer" }, { UtilityCommands, ":tools: • Utility" }, { FunCommands, ":balloon: • Fun" } };

            foreach (var cmd in CommandsNext.RegisteredCommands)
                foreach (Attribute att in cmd.Value.CustomAttributes)
                {
                    if (att is CommandCategory)
                    {
                        var check = cmd.Value.RunChecksAsync(ctx, true).GetAwaiter().GetResult().Any();
                        if (!check)
                            dict[((CommandCategory)att).Category].Add(cmd.Value);
                    }
                    if (att is CommandExamples)
                    {
                        examples.Add(cmd.Value, ((CommandExamples)att).Examples);
                    }
                }
        }

        public override CommandHelpMessage Build()
        {

            if (command == null)
            {
                Console.WriteLine(DeveloperCommands.HelpCommandFormat());
                EmbedBuilder.Description = $"Listing all commands you have permission to execute.\nFor more information, use **`{_ctx.Prefix}help [command]`**.";

                foreach (var abc in evalText.Keys)
                {
                    if (abc.Any())
                        EmbedBuilder.AddField(evalText[abc], abc.HelpCommandFormat());
                }

            }

            return new CommandHelpMessage(embed: EmbedBuilder);
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            this.command = command;

            var desc = command.Description.IsWhiteSpace() ? "No description available at this moment." : command.Description;

            EmbedBuilder.WithDescription(Formatter.InlineCode(command.Name) + ": " + desc + "\n");

            if (command.Aliases.Any())
            {
                EmbedBuilder.AddField(":repeat: Aliases", string.Join(", ", command.Aliases.Select(Formatter.InlineCode)));
            }

            if (command.Overloads?.Any() == true)
            {
                var sb = new StringBuilder();

                foreach (var ovl in command.Overloads.OrderByDescending(x => x.Priority))
                {
                    sb.Append('`').Append(_ctx.Prefix).Append(command.QualifiedName);

                    foreach (var arg in ovl.Arguments)
                        sb.Append(arg.IsOptional || arg.IsCatchAll ? " [" : " <").Append(arg.Name).Append(arg.IsCatchAll ? "..." : "").Append(arg.IsOptional || arg.IsCatchAll ? ']' : '>');

                    sb.Append("`\n");
                }

                EmbedBuilder.AddField(":pen_ballpoint: Arguments", sb.ToString().Trim(), false);
            }
            if (examples.ContainsKey(command))
                EmbedBuilder.AddField(":mag_right: Examples", string.Join('\n', examples[command].Select(x => Formatter.InlineCode(_ctx.Prefix + x))));

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            if (command != null)
            {
                EmbedBuilder.AddField("Commands", string.Join(", ", subcommands.Select(x => Formatter.InlineCode(x.Name))));
            }

            return this;
        }
    }
}

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Humanizer;
using SkyLar.Attributes;

namespace SkyLar.Commands
{
    public partial class DeveloperCommands : BaseCommandModule
    {
        [Command("devstatus")]
        [CommandCategory(Category.Developer)]
        [DeveloperCommand]
        public async Task DevStatusCommand(CommandContext ctx)
        {
            GC.Collect();
            
            var os = Environment.OSVersion.VersionString;
            var ram = Process.GetCurrentProcess().WorkingSet64;

            await ctx.RespondWithEmbedAsync(ctx.GetBaseEmbed()
                .AddField("OS", os, true)
                .AddField("RAM", ram.Bytes().Humanize("#.## MB"), true)
                .AddField("Ping", ctx.Client.Ping + "ms", true)
                .AddField("DSharpPlus", ctx.Client.VersionString, true)
                .AddField("Guilds", ctx.Client.Guilds.Count.ToString(), true));
        }

    }

}

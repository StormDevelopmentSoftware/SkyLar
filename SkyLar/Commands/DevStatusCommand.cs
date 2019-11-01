// File DevStatusCommand.cs created by Animadoria (me@animadoria.cf) at 11/1/2019 8:38 PM.
// (C) Animadoria 2019 - All Rights Reserved
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Humanizer;
using SkyLar.Attributes;
using SkyLar.Utilities;

namespace SkyLar.Commands
{
    public partial class DeveloperCommands : BaseCommandModule
    {
        [Command("devstatus")]
        [CommandCategory(Categories.Developer)]
        [DeveloperCommand]
        public async Task DevStatusCommand(CommandContext ctx)
        {
            var os = Environment.OSVersion.VersionString;
            var ram = Process.GetCurrentProcess().WorkingSet64;
            var memory = GC.GetTotalMemory(true);
            await ctx.RespondWithEmbedAsync(new DiscordEmbedBuilder()
            {
                Title = "SkyLar status"
            }.AddField("OS", os, true).AddField("RAM", ram.Bytes().Humanize("#.## MB"), true));
        }
    }
}

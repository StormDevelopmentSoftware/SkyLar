// File DevStatusCommand.cs for the SkyLar Discord bot at 11/1/2019 8:38 PM.
// (C) Storm Development Software - 2019. All Rights Reserved
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Humanizer;
using SkyLar.Attributes;
using SkyLar.Localization;
using SkyLar.Utilities;

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
            await ctx.RespondWithEmbedAsync(new DiscordEmbedBuilder(ctx.BaseEmbed())
            {
                Title = "SkyLar status"
            }.AddField("OS", os, true)
            .AddField("RAM", ram.Bytes().Humanize("#.## MB"), true)
            .AddField("Ping", ctx.Client.Ping + "ms", true)
            .AddField("DSharpPlus", ctx.Client.VersionString, true)
            .AddField("Guilds", ctx.Client.Guilds.Count.ToString(), true));
        }

    }

}

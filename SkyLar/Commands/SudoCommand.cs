// File SudoCommand.cs created by Animadoria (me@animadoria.cf) at 11/1/2019 7:53 PM.
// (C) Animadoria 2019 - All Rights Reserved
using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SkyLar.Attributes;

namespace SkyLar.Commands
{
    public partial class DeveloperCommands : BaseCommandModule
    {
        [Command("sudo")]
        [CommandCategory(Categories.Developer)]
        [DeveloperCommand]
        public async Task SudoCommand(CommandContext ctx, DiscordMember member, string content)
        {
            var cmd = ctx.CommandsNext.FindCommand(content, out var args);
            var fctx = ctx.CommandsNext.CreateFakeContext(member, ctx.Channel, content, ctx.Prefix, cmd, args);
            await ctx.CommandsNext.ExecuteCommandAsync(fctx);
        }
    }
}

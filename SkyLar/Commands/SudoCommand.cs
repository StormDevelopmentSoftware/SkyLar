// File SudoCommand.cs for the SkyLar Discord bot at 11/1/2019 7:53 PM.
// (C) Storm Development Software - 2019. All Rights Reserved
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
        [CommandCategory(Category.Developer)]
        [DeveloperCommand]
        public async Task SudoCommand(CommandContext ctx, DiscordMember member, string content)
        {
            var cmd = ctx.CommandsNext.FindCommand(content, out var args);
            var fctx = ctx.CommandsNext.CreateFakeContext(member, ctx.Channel, content, ctx.Prefix, cmd, args);
            await ctx.CommandsNext.ExecuteCommandAsync(fctx);
        }
    }
}

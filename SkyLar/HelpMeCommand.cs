// File HelpMeCommand.cs created by Animadoria (me@animadoria.cf) at 27/10/2019 21:12.
// (C) Animadoria 2019 - All Rights Reserved
using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SkyLar.Attributes;

namespace SkyLar
{
    public class HelpMeCommand : BaseCommandModule
    {
        [Command("aaaa")]
        [CommandCategory(Categories.Info)]
        public async Task AaaaCommand(CommandContext ctx)
        {
         
            await ctx.RespondAsync("yay");
        }

        [Command("comandoparadevs")]
        [CommandCategory(Categories.Developer)]
        public async Task BbbbCommand(CommandContext ctx)
        {
            await ctx.RespondAsync("yay");
        }

        
        [Command("maisumdedevs")]
        [DeveloperCommand]
        [CommandCategory(Categories.Developer)]
        public async Task varioscommand(CommandContext ctx)
        {
            await ctx.RespondAsync("yay");
        }
    }
}

using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using NCalc;
using SkyLar.Attributes;

namespace SkyLar.Commands
{
    public partial class InfoCommands : BaseCommandModule
    {
        [Command("math")]
        [CommandCategory(Category.Info)]
        [Description("Evaluates a math expression.\nValid functions: [Functions](https://github.com/ncalc/ncalc/wiki/Functions)")]
        [Example("math 1+1", "math Cos(Pi)", "math 6*3")]
        public async Task MathCommand(CommandContext ctx, [RemainingText] string expression)
        {
            try
            {
                var exp = new Expression(expression);
                exp.Parameters["Pi"] = Math.PI;
                exp.Parameters["E"] = Math.E;
                var result = exp.Evaluate();
                await ctx.RespondAsync($"Result: {Formatter.InlineCode(result.ToString())}");
            }
            catch (Exception)
            {
                await ctx.RespondAsync($"{DiscordEmoji.FromGuildEmote(ctx.Client, 451145098470752286)} The expression was not valid! Use help for more information.");
            }
        }
    }
}

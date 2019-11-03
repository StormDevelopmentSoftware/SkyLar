using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SkyLar.Attributes
{
    public class DeveloperCommand : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (ctx.User.Id == 163324170556538880 || ctx.User.Id == 143466929615667201)
                return true;

            if (!help)
                await ctx.RespondAsync("no perm haha");

            return false;
        }
    }
}

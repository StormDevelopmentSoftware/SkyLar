using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SkyLar.Attributes;

namespace SkyLar.Commands
{
    public partial class UtilityCommands : BaseCommandModule
    {
        [Command("avatar")]
        [CommandCategory(Category.Utility)]
        [Example("avatar", "avatar Animadoria", "avatar @FRNathan13#7402", "avatar 155774074885242880")]
        [Description("Displays someone's avatar.")]
        public async Task AvatarCommand(CommandContext ctx, DiscordUser user = null)
        {
            if (user == null)
                user = ctx.User;

            await ctx.RespondWithEmbedAsync(ctx.GetBaseEmbed()
                .WithAuthor($"{user.Username}'s avatar", iconUrl: user.AvatarUrl)
                .WithImageUrl(user.AvatarUrl));
        }
    }
}

// File AvatarCommand.cs created by Animadoria (me@animadoria.cf) at 11/1/2019 8:11 PM.
// (C) Animadoria 2019 - All Rights Reserved
using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SkyLar.Attributes;
using SkyLar.Utilities;

namespace SkyLar.Commands
{
    public partial class UtilityCommands : BaseCommandModule
    {
        [Command("avatar")]
        [CommandCategory(Categories.Utility)]
        public async Task AvatarCommand(CommandContext ctx, DiscordUser user)
        {
            await ctx.RespondWithEmbedAsync(new DiscordEmbedBuilder()
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor()
                {
                    Name = $"{user.Username}'s avatar",
                    IconUrl = user.AvatarUrl,
                },
                Color = DiscordColor.Blurple,
                ImageUrl = user.AvatarUrl,
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = "Requested by " + ctx.User.Username + "#" + ctx.User.Discriminator,
                    IconUrl = ctx.User.AvatarUrl
                }
            });
        }
    }
}

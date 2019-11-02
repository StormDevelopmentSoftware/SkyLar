// File AvatarCommand.cs created for the SkyLar Discord bot at 11/1/2019 8:11 PM.
// (C) Storm Development Software - 2019. All Rights Reserved
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
        [CommandCategory(Category.Utility)]
        [CommandExamples("avatar", "avatar Animadoria", "avatar @FRNathan13#7402", "avatar 155774074885242880")]
        [Description("Displays someone's avatar.")]
        public async Task AvatarCommand(CommandContext ctx, DiscordUser user = null)
        {
            if (user == null)
                user = ctx.User;

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

using System;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace SkyLar.Utilities
{
    public static class EmbedUtilities
    {
        public static DiscordEmbed BaseEmbed(this CommandContext ctx)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
            builder
            .WithFooter($"Requested by {ctx.User.Username}#{ctx.User.Discriminator}", ctx.User.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .WithColor(DiscordColor.Blurple);
            return builder.Build();
        }
    }
}
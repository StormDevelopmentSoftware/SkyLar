// File DSharpPlusExtensions.cs created by Animadoria (me@animadoria.cf) at 11/1/2019 8:15 PM.
// (C) Animadoria 2019 - All Rights Reserved
using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace SkyLar.Utilities
{
    public static class DSharpPlusExtensions
    {
        public static async Task<DiscordMessage> RespondWithEmbedAsync(this CommandContext context, DiscordEmbed embed)
        {
            return await context.RespondAsync(embed: embed);
        }

        public static async Task<DiscordMessage> RespondAsync(this CommandContext context, string message)
        {
            return await context.RespondAsync(message);
        }

        public static async Task<DiscordMessage> ModifyAsync(this DiscordMessage message, DiscordEmbed embed)
        {
            return await message.ModifyAsync(embed: embed);
        }

        public static async Task<DiscordMessage> RespondReactedAsync(this CommandContext context, string message, params DiscordEmoji[] emojis)
        {
            var _ = await context.RespondAsync(message);
            foreach (var emoji in emojis)
            {
                await _.CreateReactionAsync(emoji);
            }
            return _;
        }

        public static async Task<DiscordMessage> RespondReactedAsync(this CommandContext context, DiscordEmbed embed, params DiscordEmoji[] emojis)
        {
            var _ = await context.RespondAsync(embed: embed);
            foreach (var emoji in emojis)
            {
                await _.CreateReactionAsync(emoji);
                await Task.Delay(24);
            }
            return _;
        }
    }
}

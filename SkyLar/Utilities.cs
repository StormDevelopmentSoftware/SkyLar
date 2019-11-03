using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace SkyLar
{
    public static class Utilities
    {
        public static readonly JsonSerializerSettings DEFAULT_JSON_SETTINGS = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy(true, true) } };
        public static readonly HttpClient DEFAULT_HTTP_CLIENT = new HttpClient();

        public static async Task<int> GetShardCountAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            var req = new HttpRequestMessage(HttpMethod.Get, "https://discordapp.com/api/gateway/bot");
            req.Headers.TryAddWithoutValidation("Authorization", $"Bot {token}");
            req.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            using (req)
            using (var res = await DEFAULT_HTTP_CLIENT.SendAsync(req))
            {
                var json = "{}";

                if (res.IsSuccessStatusCode)
                    json = await res.Content.ReadAsStringAsync();
                else
                    throw new HttpRequestException(string.Format("Cannot fetch shard count from gateway. [{0}] {1}", (int)res.StatusCode, res.StatusCode));

                var jo = JObject.Parse(json);

                if (jo != null && jo["shards"] != null)
                    return jo["shards"].Value<int>();
                else
                    throw new InvalidOperationException("Cannot fetch shard count from gateway.");
            }
        }

        public static DiscordEmbedBuilder GetBaseEmbed(this CommandContext ctx)
            => new DiscordEmbedBuilder()
                .WithFooter($"Requested by {ctx.User.Username}#{ctx.User.Discriminator}", ctx.User.GetAvatarUrl(ImageFormat.Png))
                .WithTimestamp(DateTime.Now)
                .WithColor(DiscordColor.Blurple);

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

        public static async Task<DiscordMessage> RespondReactedAsync(this CommandContext context, string content, params DiscordEmoji[] emojis)
        {
            var message = await context.RespondAsync(content);
            foreach (var emoji in emojis)
            {
                await message.CreateReactionAsync(emoji);
                await Task.Delay(250);
            }
            return message;
        }

        public static async Task<DiscordMessage> RespondReactedAsync(this CommandContext context, DiscordEmbed embed, params DiscordEmoji[] emojis)
        {
            var message = await context.RespondAsync(embed: embed);
            foreach (var emoji in emojis)
            {
                await message.CreateReactionAsync(emoji);
                await Task.Delay(250);
            }
            return message;
        }

        public static bool NotNull(this string value) =>
            !string.IsNullOrEmpty(value);

        public static bool IsNull(this string value) =>
            string.IsNullOrEmpty(value);

        public static bool NotWhiteSpace(this string value) =>
            !string.IsNullOrWhiteSpace(value);

        public static bool IsWhiteSpace(this string value) =>
            string.IsNullOrWhiteSpace(value);

        public static byte[] GetBytes(this string value) =>
            Encoding.UTF8.GetBytes(value);

        public static string GetString(this byte[] bytes) =>
            Encoding.UTF8.GetString(bytes);

        public static async Task<(bool success, string raw)> DownloadStringAsync(string url)
        {
            using(var res = await DEFAULT_HTTP_CLIENT.GetAsync(url))
            {
                if (!res.IsSuccessStatusCode)
                    return (false, string.Empty);

                try
                {
                    var str = await res.Content.ReadAsStringAsync();
                    return (true, str);
                }
                catch { return (false, string.Empty); }
            }
        }

        public static bool HasMatch(this Regex regex, string input, out Match match)
        {
            if (!regex.IsMatch(input))
            {
                match = null;
                return false;
            }
            else
            {
                match = regex.Match(input);
                return true;
            }
        }

        public static bool HasMatches(this Regex regex, string input, out MatchCollection matches)
        {
            if (!regex.IsMatch(input))
            {
                matches = null;
                return false;
            }
            else
            {
                matches = regex.Matches(input);
                return true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SkyLar.Entities;

namespace SkyLar
{
    public static class Utilities
    {
        internal static async Task InitializeShardsAsync()
        {
            var settings = SkyLarSettings.GetInstance();
            var count = await GetShardsAsync(settings);
            var shards = new Dictionary<int, SkyLarBot>();

            for (var i = 0; i < count; i++)
                shards[i] = new SkyLarBot(i, count);

            SkyLarBot.Shards = shards;

            settings.Discord.Reset();
        }

        internal static async Task<int> GetShardsAsync(SkyLarSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Discord.Token))
                throw new InvalidOperationException("Token cannot be null or empty.");

            if (settings.Discord.ShardCount != default)
                return settings.Discord.ShardCount;

            using (var http = new HttpClient())
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, "https://discordapp.com/api/gateway/bot"))
                {
                    req.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                    req.Headers.TryAddWithoutValidation("Authorization", $"Bot {settings.Discord.Token}");
                    using (var res = await http.SendAsync(req))
                    {
                        var json = "{}";

                        if (res.IsSuccessStatusCode)
                            json = await res.Content.ReadAsStringAsync();

                        var result = JObject.Parse(json);

                        if (result["shards"] != null)
                            return result.Value<int>("shards");
                    }
                }
            }

            return 1;
        }
    }
}

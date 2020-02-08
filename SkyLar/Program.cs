using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SkyLar.Entities;

namespace SkyLar
{
    static class Program
    {
        static readonly CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            var settings = SkyLarSettings.GetInstance();
            
            int count;

            if (settings.Discord.ShardCount == default) count = await GetShardsAsync(settings);
            else count = settings.Discord.ShardCount;


            var shards = new Dictionary<int, SkyLarBot>();

            for (var i = 0; i < count; i++)
                shards[i] = new SkyLarBot(i, count);

            SkyLarBot.Shards = shards;

            settings.Discord.Reset();

            await Task.WhenAll(SkyLarBot.Shards.Select(x => x.Value)
                .Select(x => x.InitializeAsync()));

            GC.Collect();

            while (!cts.IsCancellationRequested)
                await Task.Delay(1);

            await Task.WhenAll(SkyLarBot.Shards.Select(x => x.Value)
                .Select(x => x.ShutdownAsync()));
        }

        static async Task<int> GetShardsAsync(SkyLarSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Discord.Token))
                throw new InvalidOperationException("Token cannot be null or empty.");

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

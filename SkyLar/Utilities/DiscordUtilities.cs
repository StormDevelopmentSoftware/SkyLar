using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SkyLar.Utilities
{
    public static class DiscordUtilities
    {
        public static async Task<int> GetShardCountAsync(string token)
        {
            using (var http = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Get, "https://discordapp.com/api/gateway/bot");
                req.Headers.TryAddWithoutValidation("Authorization", $"Bot {token}");
                req.Headers.TryAddWithoutValidation("Content-Type", "application/json");

                using (req)
                using (var res = await http.SendAsync(req))
                {
                    var json = "{}";

                    if (res.IsSuccessStatusCode)
                        json = await res.Content.ReadAsStringAsync();

                    var jo = FileUtilities.ParseJson(json);

                    if (jo != null && jo["shards"] != null)
                        return jo["shards"].Value<int>();
                    else
                        return -1;
                }
            }
        }
    }
}

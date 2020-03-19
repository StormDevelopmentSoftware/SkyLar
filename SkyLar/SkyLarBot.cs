using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SkyLar.Entities;

namespace SkyLar
{
    public class SkyLarBot
    {
        /// <summary>
        /// Ctor estático, registra um servidor HttpClient global.
        /// </summary>
        static SkyLarBot()
        {
            Singleton<HttpClient>.Instance = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.All,
                UseCookies = true
            });
        }

        /// <summary>
        /// Configuração global da skylar.
        /// </summary>
        public SkyLarConfiguration Config { get; private set; }

        /// <summary>
        /// Contém as shards registradas.
        /// </summary>
        public IReadOnlyDictionary<int, SkyLarShard> Shards { get; private set; }

        public SkyLarBot(SkyLarConfiguration config)
        {
            Singleton<SkyLarBot>.Instance = this;
            this.Config = config;
        }

        /// <summary>
        /// Obtém a shard pelo ID.
        /// </summary>
        /// <param name="i">ID da shard.</param>
        /// <returns></returns>
        public SkyLarShard this[int i]
        {
            get
            {
                if (this.Shards.TryGetValue(i, out var shard))
                    return shard;

                return default;
            }
        }

        /// <summary>
        /// Iniciar o bot.
        /// </summary>
        public async Task InitializeAsync()
        {
            await this.SetupShardsAsync();

            var tasks = new List<Task>();

            foreach (var shard in this.Shards.Select(x => x.Value))
                tasks.Add(shard.InitializeAsync());

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Finaliza o bot.
        /// </summary>
        public async Task ShutdownAsync()
        {
            var tasks = new List<Task>();

            foreach (var shard in this.Shards.Select(x => x.Value))
                tasks.Add(shard.ShutdownAsync());

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Configura cada shard imediatamente.
        /// </summary>
        protected async Task SetupShardsAsync()
        {
            var http = Singleton<HttpClient>.Get();

            var tokenInternal = this.Config.Discord.GetType()
                .GetField("TokenInternal", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(this.Config.Discord)
                .ToString();

            using var request = new HttpRequestMessage(HttpMethod.Get, "https://discordapp.com/api/gateway/bot");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bot {tokenInternal}");

            using var response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Cannot initialize shards. {(int)response.StatusCode}: {response.StatusCode}");

            var raw = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(raw);

            if (json["shards"] == null)
                throw new InvalidOperationException("Cannot initialize shards. Unknown payload received from gateway.");

            var temp = new Dictionary<int, SkyLarShard>();
            var count = json.Value<int>("shards");

            for (var i = 0; i < count; i++)
                temp[i] = new SkyLarShard(this, this.Config.Discord.Build(i, count));

            this.Shards = temp;
        }
    }
}

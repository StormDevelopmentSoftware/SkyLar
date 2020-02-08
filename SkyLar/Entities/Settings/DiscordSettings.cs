using System;
using System.Security.Cryptography;
using DSharpPlus;
using Newtonsoft.Json;

namespace SkyLar.Entities.Settings
{
    public class DiscordSettings
    {
        [JsonProperty]
        public string Token { get; protected set; }

        [JsonProperty]
        public bool AutoReconnect { get; protected set; } = true;

        [JsonProperty]
        public bool ReconnectIndefinitely { get; protected set; } = false;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ShardCount { get; protected set; } = default;

        [JsonProperty]
        public TimeSpan HttpTimeout { get; protected set; } = TimeSpan.FromMinutes(1.5d);

        [JsonProperty]
        public GatewayCompressionLevel GatewayCompressionLevel { get; protected set; } = GatewayCompressionLevel.Stream;

        [JsonProperty]
        public int MessageCacheSize { get; private set; } = 1024;

        [JsonProperty]
        public int LargeThreshold { get; private set; } = 250;

        public DiscordConfiguration Build(int id, int count)
        {
            var szTokenChars = new char[this.Token.Length];
            this.Token.CopyTo(0, szTokenChars, 0, szTokenChars.Length);

            return new DiscordConfiguration
            {
                Token = new string(szTokenChars),
                TokenType = TokenType.Bot,
                AutoReconnect = this.AutoReconnect,
                ReconnectIndefinitely = this.ReconnectIndefinitely,
                GatewayCompressionLevel = this.GatewayCompressionLevel,
                HttpTimeout = this.HttpTimeout,
                MessageCacheSize = this.MessageCacheSize,
                LargeThreshold = this.LargeThreshold,
                ShardId = id,
                ShardCount = count
            };
        }

        public void Reset()
            => this.Token = default;
    }
}

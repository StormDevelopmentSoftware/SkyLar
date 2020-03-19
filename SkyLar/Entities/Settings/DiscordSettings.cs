using System;
using DSharpPlus;
using Newtonsoft.Json;

namespace SkyLar.Entities.Settings
{
    public class DiscordSettings
    {
        [JsonIgnore]
        protected string TokenInternal;

        [JsonProperty]
        public string Token
        {
            get => string.Empty;
            set => this.TokenInternal = value;
        }

        [JsonIgnore]
        public bool HasInvalidToken => string.IsNullOrEmpty(this.TokenInternal);

        [JsonProperty]
        public GatewayCompressionLevel GatewayCompressionLevel { get; private set; } = GatewayCompressionLevel.Stream;

        [JsonProperty]
        public bool AutoReconnect { get; private set; } = true;

        [JsonProperty]
        public bool ReconnectIndefinitely { get; private set; } = false;

        [JsonProperty]
        public TimeSpan HttpTimeout { get; private set; } = TimeSpan.FromSeconds(30d);

        public DiscordConfiguration Build(int shardId, int shardCount)
        {
            return new DiscordConfiguration
            {
                Token = this.TokenInternal,
                TokenType = TokenType.Bot,
                AutoReconnect = this.AutoReconnect,
                GatewayCompressionLevel = this.GatewayCompressionLevel,
                HttpTimeout = this.HttpTimeout,
                ReconnectIndefinitely = this.ReconnectIndefinitely,
                ShardId = shardId,
                ShardCount = shardCount,

#if USING_INTERNAL_LOGGER
				DateTimeFormat = "HH:mm:ss.fff",
				LogLevel = LogLevel.Debug,
				UseInternalLogHandler = true
#endif
            };
        }
    }
}

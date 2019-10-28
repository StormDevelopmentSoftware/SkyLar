using System;
using DSharpPlus;
using Newtonsoft.Json;

namespace SkyLar.Entities.Config
{
    public class SkyLarDiscordConfig
    {
        [JsonProperty]
        public bool AutoReconnect { get; private set; } = true;

        [JsonProperty]
        public bool ReconnectIndefinitely { get; private set; } = false;

        [JsonProperty]
        public string Token { get; private set; } = null;

        [JsonProperty]
        public int MessageCacheSize { get; private set; } = 1024;

        [JsonProperty]
        public LogLevel LogLevel { get; private set; } = LogLevel.Info;

        [JsonProperty]
        public int LargeThreshold { get; private set; } = 250;

        [JsonProperty]
        public GatewayCompressionLevel GatewayCompressionLevel { get; private set; } = GatewayCompressionLevel.Stream;

        [JsonProperty]
        public TimeSpan HttpTimeout { get; private set; } = TimeSpan.FromSeconds(45d);

        public DiscordConfiguration Build()
        {
            return new DiscordConfiguration
            {
                Token = this.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = this.AutoReconnect,
                ReconnectIndefinitely = this.ReconnectIndefinitely,
                GatewayCompressionLevel = this.GatewayCompressionLevel,
                HttpTimeout = this.HttpTimeout,
                MessageCacheSize = this.MessageCacheSize,
                LogLevel = LogLevel.Debug,
                LargeThreshold = this.LargeThreshold,
                UseInternalLogHandler = true
            };
        }
    }
}

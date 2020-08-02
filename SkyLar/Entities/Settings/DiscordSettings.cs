using System;
using System.Threading;
using DSharpPlus;
using Newtonsoft.Json;

namespace SkyLar.Entities.Settings
{
	public class DiscordSettings
	{
		[JsonProperty]
		public bool AutoReconnect { get; private set; } = true;

		[JsonProperty]
		public GatewayCompressionLevel GatewayCompressionLevel { get; private set; } = GatewayCompressionLevel.Stream;

		[JsonProperty]
		public TimeSpan HttpTimeout { get; private set; } = Timeout.InfiniteTimeSpan;

		[JsonProperty]
		public int LargeThreshold { get; private set; } = 250;

		[JsonProperty]
		public int MessageCacheSize { get; private set; } = 1024;

		[JsonProperty]
		public int ShardCount { get; private set; } = 0;

		[JsonProperty]
		public string Token { get; private set; } = string.Empty;

		[JsonProperty]
		public bool UseRelativeRatelimit { get; private set; } = true;

		public DiscordConfiguration Build(int shard_id, int shard_count) => new DiscordConfiguration
		{
			AutoReconnect = this.AutoReconnect,
			GatewayCompressionLevel = this.GatewayCompressionLevel,
			HttpTimeout = this.HttpTimeout,
			LargeThreshold = this.LargeThreshold,
#if DEBUG
			LogLevel = LogLevel.Debug,
#else
			LogLevel = LogLevel.Warning,
#endif
			MessageCacheSize = this.MessageCacheSize,
			ReconnectIndefinitely = false,
			ShardId = shard_id,
			ShardCount = shard_count,
			Token = this.Token,
			TokenType = TokenType.Bot,
			UseRelativeRatelimit = this.UseRelativeRatelimit,
		};
	}
}

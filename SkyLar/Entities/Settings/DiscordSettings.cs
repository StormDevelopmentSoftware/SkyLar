using System;
using System.Threading;
using DSharpPlus;

namespace SkyLar.Entities.Settings
{
	public class DiscordSettings
	{
		public bool AutoReconnect { get; private set; } = true;
		public GatewayCompressionLevel GatewayCompressionLevel { get; private set; } = GatewayCompressionLevel.Stream;
		public TimeSpan HttpTimeout { get; private set; } = Timeout.InfiniteTimeSpan;
		public int LargeThreshold { get; private set; } = 250;
		public int MessageCacheSize { get; private set; } = 1024;
		public int ShardCount { get; private set; } = 0;
		public string Token { get; private set; } = string.Empty;
		public bool UseRelativeRatelimit { get; private set; } = true;

		public DiscordConfiguration Build(int shard_id, int shard_count) => new DiscordConfiguration
		{
			AutoReconnect = this.AutoReconnect,
			GatewayCompressionLevel = this.GatewayCompressionLevel,
			HttpTimeout = this.HttpTimeout,
			LargeThreshold = this.LargeThreshold,
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

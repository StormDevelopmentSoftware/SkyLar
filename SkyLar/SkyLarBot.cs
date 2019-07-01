using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using SkyLar.Entities;

namespace SkyLar
{
    public class SkyLarBot
    {
        public int ShardId { get; }
        public int ShardCount { get; }
        public SkyLarConfig Config { get; }
        public DiscordClient Discord { get; }

        public SkyLarBot(int shard_id, int shard_count, SkyLarConfig config)
        {
            this.ShardId = shard_id;
            this.ShardCount = shard_count;
            this.Config = config;

            this.Discord = new DiscordClient(new DiscordConfiguration(this.Config.Discord.Build())
            {
                ShardId = this.ShardId,
                ShardCount = this.ShardCount
            });

            this.Discord.Ready += async e =>
            {
                await e.Client.UpdateStatusAsync(new DiscordActivity
                {
                    ActivityType = ActivityType.Streaming,
                    Name = "skylar help | " + e.Client.Ping + "ms | shard " + (this.ShardId + 1),
                    StreamUrl = "https://twitch.tv/#"
                });
            };

            this.Discord.Heartbeated += async e =>
            {
                await e.Client.UpdateStatusAsync(new DiscordActivity
                {
                    ActivityType = ActivityType.Streaming,
                    Name = "skylar help | " + e.Ping + "ms | shard " + (this.ShardId + 1),
                    StreamUrl = "https://twitch.tv/#"
                });
            };
        }

        public async Task InitializeAsync()
        {
            await this.Discord.ConnectAsync();
        }

        public async Task ShutdownAsync()
        {
            await this.Discord.DisconnectAsync();
        }
    }
}
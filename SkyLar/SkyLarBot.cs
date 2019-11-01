using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Lavalink;
using Microsoft.Extensions.DependencyInjection;
using SkyLar.Commands;
using SkyLar.Entities;

namespace SkyLar
{
    public class SkyLarBot
    {
        public int ShardId { get; private set; }
        public int ShardCount { get; private set; }
        public SkyLarConfig Config { get; private set; }
        public DiscordClient Discord { get; private set; }
        public CommandsNextExtension CommandsNext { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public LavalinkExtension Lavalink { get; private set; }
        public IServiceProvider Services { get; private set; }

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
                    Name = $"skylar help | {e.Client.Ping}ms",
                    StreamUrl = "https://twitch.tv/#"
                });
            };

            this.Discord.Heartbeated += async e =>
            {
                await e.Client.UpdateStatusAsync(new DiscordActivity
                {
                    ActivityType = ActivityType.Streaming,
                    Name = $"skylar help | {e.Ping}ms",
                    StreamUrl = "https://twitch.tv/#"
                });
            };

            this.Services = new ServiceCollection()
                .AddSingleton(this)
                .AddSingleton(this.Discord)
                .BuildServiceProvider();

            this.Lavalink = this.Discord.UseLavalink();
            this.Interactivity = this.Discord.UseInteractivity(this.Config.Interactivity.Build());
            this.CommandsNext = this.Discord.UseCommandsNext(this.Config.CommandsNext.Build(this.Services));

            CommandsNext.RegisterCommands<InfoCommands>();
            CommandsNext.RegisterCommands<DeveloperCommands>();
            CommandsNext.RegisterCommands<UtilityCommands>();
            CommandsNext.SetHelpFormatter<HelpFormatter>();
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
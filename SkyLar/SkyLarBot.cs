using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Lavalink;
using Microsoft.Extensions.DependencyInjection;
using SkyLar.Entities;

namespace SkyLar
{
    public sealed class SkyLarBot
    {
        public static IReadOnlyDictionary<int, SkyLarBot> Shards { get; internal set; }
        public DiscordClient Discord { get; private set; }
        public CommandsNextExtension CommandsNext { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public LavalinkExtension Lavalink { get; private set; }
        public IServiceProvider Services { get; private set; }

        public SkyLarBot(int shard_id, int shard_count)
        {
            var config = SkyLarSettings.GetInstance();

            this.Discord = new DiscordClient(new DiscordConfiguration(config.Discord.Build(shard_id, shard_count)));
            this.Lavalink = this.Discord.UseLavalink();
            this.Interactivity = this.Discord.UseInteractivity(config.Interactivity.Build());

            this.Services = new ServiceCollection()
                .AddSingleton(this)
                //.AddSingleton<DatabaseService>()
                //.AddSingleton<CommandService>()
                .BuildServiceProvider();

            this.CommandsNext = this.Discord.UseCommandsNext(new CommandsNextConfiguration
            {
                UseDefaultCommandHandler = false,
                Services = this.Services
            });
        }

        public static SkyLarBot GetShard(int id)
        {
            if (Shards.TryGetValue(id, out var shard))
                return shard;

            return default;
        }

        public Task InitializeAsync()
            => this.Discord.ConnectAsync();

        public Task ShutdownAsync()
            => this.Discord.DisconnectAsync();
    }
}

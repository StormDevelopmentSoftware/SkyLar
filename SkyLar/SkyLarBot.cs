using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SkyLar.Entities;

namespace SkyLar
{
	public class SkyLarBot
	{
		public static IReadOnlyDictionary<int, SkyLarBot> Shards
		{
			get;
			protected internal set;
		}

		public int ShardId { get; }
		public SkyLarConfiguration Configuration { get; }
		public DiscordClient Discord { get; }
		public InteractivityExtension Interactivity { get; }
		public CommandsNextExtension CommandsNext { get; }
		public IServiceProvider Services { get; }
		public int Latency { get; private set; }
		public DateTimeOffset StartTime { get; private set; }

		public SkyLarBot(SkyLarConfiguration configuration, int shard_id, int shard_count)
		{
			this.Configuration = configuration;
			this.ShardId = shard_id;

			this.Discord = new DiscordClient(this.Configuration.Discord.Build(shard_id, shard_count));
			this.Discord.Ready += this.Discord_Ready;
			this.Discord.DebugLogger.LogMessageReceived += this.DebugLogger_LogMessageReceived;
			this.Discord.ClientErrored += this.Discord_ClientErrored;
			this.Discord.Heartbeated += this.Discord_Heartbeated;
			this.Interactivity = this.Discord.UseInteractivity(this.Configuration.Interactivity.Build());

			this.Services = new ServiceCollection()
				.BuildServiceProvider(true);

			this.CommandsNext = this.Discord.UseCommandsNext(new CommandsNextConfiguration
			{
				UseDefaultCommandHandler = false,
				EnableDefaultHelp = false,
				EnableMentionPrefix = true,
				Services = this.Services
			});

			this.CommandsNext.RegisterCommands(typeof(SkyLarBot).Assembly);
		}

		Task Discord_Ready(ReadyEventArgs e)
		{
			this.Latency = this.Discord.Ping;

			if (this.StartTime == DateTimeOffset.MinValue)
			{
				this.StartTime = DateTimeOffset.UtcNow;
				Log.Information("Shard {ShardId} initialized.", this.ShardId);
			}

			return Task.CompletedTask;
		}

		Task Discord_Heartbeated(HeartbeatEventArgs e)
		{
			this.Latency = e.Ping;
			return Task.CompletedTask;
		}

		Task Discord_ClientErrored(ClientErrorEventArgs e)
		{
			this.Discord.DebugLogger.LogMessage(LogLevel.Warning, "AsyncEventHandler", e.Exception.Message, DateTime.Now, e.Exception);
			return Task.CompletedTask;
		}

		void DebugLogger_LogMessageReceived(object sender, DebugLogMessageEventArgs e)
		{
			if (e.Application.ToLowerInvariant().Contains("websocket"))
				return;

			if (e.Message.Contains("Default command handler is not attached. If this was intentional, you can ignore this message."))
				return;

			Log.Verbose(e.Exception, $"[{{Context}}] #{{ShardId}}: {e.Message}", e.Application, this.ShardId, e.Message);
		}

		public Task StartAsync()
			=> this.Discord.ConnectAsync();

		public Task StopAsync()
			=> this.Discord.DisconnectAsync();
	}
}

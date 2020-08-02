using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using SkyLar.Entities;

namespace SkyLar
{
	class Program
	{
		static readonly CancellationTokenSource TokenSource = new CancellationTokenSource();

		static async Task Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(theme: AnsiConsoleTheme.Code)
				.MinimumLevel.Verbose()
				.Enrich.FromLogContext()
				.CreateLogger();

			var config = SkyLarConfiguration.GetOrCreateDefault();

			if (string.IsNullOrEmpty(config.Discord.Token))
			{
				Log.Warning("Cannot initialize skylar. {ArgName} cannot be null or empty.", "Token");
				return;
			}

			try
			{
				int result;

				if (config.Discord.ShardCount > 0)
				{
					result = config.Discord.ShardCount;
					Log.Warning("Manual sharding enabled by configuration.");
				}
				else
				{
					var tpl = await Utilities.GetShardCountAsync(config.Discord.Token);

					if (!tpl.Success)
					{
						Log.Warning("Cannot fetch shards from gateway. (code: {HttpCode})", tpl.Result);
						return;
					}

					result = tpl.Result;
				}

				Log.Information("Should initialize {Count} shards.", result);

				var shards = new Dictionary<int, SkyLarBot>();

				for (var i = 0; i < result; i++)
					shards[i] = new SkyLarBot(config, i, result);

				SkyLarBot.Shards = shards;

				var tasks = new List<Task>();

				foreach (var (i, shard) in shards)
					tasks.Add(shard.StartAsync());

				await Task.WhenAll(tasks);

				while (!TokenSource.IsCancellationRequested)
					await Task.Delay(100);

				tasks.Clear();

				foreach (var (i, shard) in shards)
					tasks.Add(shard.StopAsync());

				await Task.WhenAll(tasks);
			}
			catch (Exception ex)
			{
				Log.Fatal(ex.ToString());
			}
		}
	}
}

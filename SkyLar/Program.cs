using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SkyLar.Entities;
using SkyLar.Localization;

namespace SkyLar
{
    internal static class Program
    {
        static CancellationTokenSource Cts { get; } = new CancellationTokenSource();

        private static async Task Main(string[] _)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                if (!Cts.IsCancellationRequested)
                    Cts.Cancel();

                e.Cancel = true;
            };

            var config = await SkyLarConfig.InitializeConfigurationAsync();

            await InitializeShardsAsync(config);

            while (!Cts.IsCancellationRequested)
                await Task.Delay(1);

            await ShutdownShardsAsync();
        }

        private static async Task InitializeShardsAsync(SkyLarConfig config)
        {
            LocalizationManager.Load();

            var shards = new ConcurrentDictionary<int, SkyLarBot>();
            var shard_count = await Utilities.GetShardCountAsync(config.Discord.Token);

            for (var shard_id = 0; shard_id < shard_count; shard_id++)
            {
                var bot = new SkyLarBot(shard_id, shard_count, config);
                shards.AddOrUpdate(shard_id, bot, (key, old) => bot);

                await bot.InitializeAsync();

                if (shard_count > 1)
                    await Task.Delay(500);
            }

            SkyLarBot.Shards = shards;
        }

        private static async Task ShutdownShardsAsync()
        {
            foreach (var bot in SkyLarBot.Shards.Select(x => x.Value))
                await bot.ShutdownAsync();
        }
    }
}
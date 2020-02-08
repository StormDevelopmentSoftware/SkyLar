using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SkyLar.Entities;

namespace SkyLar
{
    static class Program
    {
        static readonly CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            await Utilities.InitializeShardsAsync();

            var tasks = new List<Task>();

            foreach(var shard in SkyLarBot.Shards.Select(x => x.Value))
                tasks.Add(shard.InitializeAsync().ContinueWith(_ => Task.Delay(1500)));

            await Task.WhenAll(tasks);

            GC.Collect();

            while (!cts.IsCancellationRequested)
                await Task.Delay(1);

            tasks = new List<Task>();

            foreach (var bot in SkyLarBot.Shards.Select(x => x.Value))
                tasks.Add(bot.ShutdownAsync().ContinueWith(_ => Task.Delay(1500)));

            await Task.WhenAll(tasks);
        }
    }
}

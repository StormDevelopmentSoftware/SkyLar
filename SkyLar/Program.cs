using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SkyLar.Entities;
using SkyLar.Utilities;

namespace SkyLar
{
    public class Program
    {
        public const int DISCORD_INITIALIZATION_INTERVAL = 5000;

        public IReadOnlyList<string> Arguments { get; }
        public static Program Instance { get; private set; }

        private SkyLarConfig _config;
        private CancellationTokenSource _cts;
        private ConcurrentDictionary<int, SkyLarBot> _shards;

        static async Task Main(string[] args)
        {
            if(args.Contains("--configure"))
            {
                FileUtilities.WriteJson(SkyLarConfig.Empty, "Config.json");
                return;
            }

            try
            {
                var app = new Program(args);
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        public Program(string[] args)
        {
            Instance = this;

            this.Arguments = args.ToList().AsReadOnly();

            _shards = new ConcurrentDictionary<int, SkyLarBot>();
            _cts = new CancellationTokenSource();

            Console.CancelKeyPress += (sender, e) =>
            {
                if (!_cts.IsCancellationRequested)
                    _cts.Cancel();

                e.Cancel = true;
            };
        }

        public async Task RunAsync()
        {
            _config = SkyLarConfig.GetOrCreateDefault();

            await InitializeShardsAsync();

            while (!_cts.IsCancellationRequested)
                await Task.Delay(1);

            await ShutdownShardsAsync();
        }

        public SkyLarBot GetShard(int id = default)
        {
            if (_shards.TryGetValue(id, out var bot))
                return bot;

            return null;
        }

        async Task InitializeShardsAsync()
        {
            var count = await DiscordUtilities.GetShardCountAsync(_config.Discord.Token);

            if (count <= 0)
                throw new ApplicationException("Cannot initialize shards.");

            for (var i = 0; i < count; i++)
            {
                var bot = new SkyLarBot(i, count, _config);
                _shards.AddOrUpdate(i, bot, (key, old) => bot);

                await bot.InitializeAsync();

                if (count > 1)
                    await Task.Delay(DISCORD_INITIALIZATION_INTERVAL);
            }
        }

        async Task ShutdownShardsAsync()
        {
            foreach (var (i, bot) in _shards)
            {
                await bot.ShutdownAsync();
            }
        }
    }
}
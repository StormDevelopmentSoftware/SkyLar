using System;
using System.Threading;
using System.Threading.Tasks;
using SkyLar.Entities;

namespace SkyLar
{
	static class Program
	{
		static readonly CancellationTokenSource Cts
			= new CancellationTokenSource();

		static async Task Main(string[] args)
		{
			var config = await SkyLarConfiguration.LoadAsync();
			Singleton<SkyLarConfiguration>.Instance = config;

			if (!config.Discord.HasInvalidToken)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Token cannot be null, empty or invalid.");
				Console.ResetColor();
				return;
			}

			var bot = new SkyLarBot(config);
			await bot.InitializeAsync();

			while (!Cts.IsCancellationRequested)
				await Task.Delay(1);

			await bot.ShutdownAsync();
		}
	}
}

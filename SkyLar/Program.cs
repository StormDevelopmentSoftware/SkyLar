using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using SkyLar.Entities;

namespace SkyLar
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console(theme: AnsiConsoleTheme.Code)
				.MinimumLevel.Verbose()
				.CreateLogger();

			var config = SkyLarConfiguration.GetOrCreateDefault();

			if (string.IsNullOrEmpty(config.Discord.Token))
			{
				Log.Warning("Cannot initialize skylar. {ArgName} cannot be null or empty.", "Token");
				return;
			}

			try
			{
				// TODO
			}
			catch (Exception ex)
			{
				Log.Fatal(ex.ToString());
			}
		}
	}
}

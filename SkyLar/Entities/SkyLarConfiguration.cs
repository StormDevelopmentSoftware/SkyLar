using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SkyLar.Entities.Settings;

namespace SkyLar.Entities
{
	public class SkyLarConfiguration
	{
		[JsonProperty]
		public DiscordSettings Discord { get; private set; } = new DiscordSettings();

		[JsonProperty]
		public InteractivitySettings Interactivity { get; private set; } = new InteractivitySettings();

		[JsonProperty]
		public LavalinkSettings Lavalink { get; private set; } = new LavalinkSettings();

		public static async Task<SkyLarConfiguration> LoadAsync()
		{
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new DefaultContractResolver
				{
					NamingStrategy = new SnakeCaseNamingStrategy()
				}
			};

			var config = new SkyLarConfiguration();
			var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Config.json"));

			string json;

			if (!file.Exists)
			{
				json = JsonConvert.SerializeObject(config, Formatting.Indented, settings);
				using var sw = file.CreateText();
				await sw.WriteLineAsync(json);
				await sw.FlushAsync();
			}
			else
			{
				using var sr = file.OpenText();
				json = await sr.ReadToEndAsync();
				config = JsonConvert.DeserializeObject<SkyLarConfiguration>(json, settings);
			}

			return config;
		}
	}
}

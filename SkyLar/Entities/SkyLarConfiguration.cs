﻿using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SkyLar.Entities.Settings;

namespace SkyLar.Entities
{
	public class SkyLarConfiguration
	{
		public DiscordSettings Discord { get; private set; } = new DiscordSettings();
		public InteractivitySettings Interactivity { get; private set; } = new InteractivitySettings();

		[JsonIgnore]
		internal static readonly JsonSerializerSettings DefaultJsonSettings = new JsonSerializerSettings
		{
			ContractResolver = new DefaultContractResolver
			{
				NamingStrategy = new CamelCaseNamingStrategy(true, true)
			}
		};

		public static SkyLarConfiguration GetOrCreateDefault()
		{
			var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Config.json"));

			if (!file.Exists)
			{
				var temp = new SkyLarConfiguration();

				using (var fs = file.Open(FileMode.Create))
				using (var sw = new StreamWriter(fs))
				{
					sw.WriteLine(JsonConvert.SerializeObject(temp, Formatting.Indented, DefaultJsonSettings));
					sw.Flush();
				}

				return temp;
			}
			else
			{
				string json;

				using (var fs = file.Open(FileMode.Open))
				using (var sr = new StreamReader(fs))
					json = sr.ReadToEnd();

				return JsonConvert.DeserializeObject<SkyLarConfiguration>(json, DefaultJsonSettings);
			}
		}
	}
}

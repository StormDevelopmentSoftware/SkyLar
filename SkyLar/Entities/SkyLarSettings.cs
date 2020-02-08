using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SkyLar.Entities.Settings;

namespace SkyLar.Entities
{
    public class SkyLarSettings
    {
        private static Lazy<SkyLarSettings> settingsLazy = new Lazy<SkyLarSettings>(() =>
        {
            var settings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
            };

            var config = new SkyLarSettings();
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Config.json"));

            if (!file.Exists)
                using (var sw = file.CreateText())
                    sw.WriteLine(JsonConvert.SerializeObject(config, settings));
            else
                using (var sr = file.OpenText())
                    config = JsonConvert.DeserializeObject<SkyLarSettings>(sr.ReadToEnd(), settings);

            return config;
        });

        public static SkyLarSettings GetInstance()
            => settingsLazy.Value;

        [JsonProperty]
        public DiscordSettings Discord { get; protected set; } = new DiscordSettings();

        [JsonProperty]
        public InteractivitySettings Interactivity { get; protected set; } = new InteractivitySettings();

        [JsonProperty]
        public DatabaseSettings Database { get; protected set; } = new DatabaseSettings();
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SkyLar.Entities.Config;

namespace SkyLar.Entities
{
    public class SkyLarConfig
    {
        [JsonProperty]
        public SkyLarDiscordConfig Discord { get; private set; } = new SkyLarDiscordConfig();

        [JsonProperty]
        public SkyLarCommandsNextConfig CommandsNext { get; private set; } = new SkyLarCommandsNextConfig();

        [JsonProperty]
        public SkyLarInteractivityConfig Interactivity { get; private set; } = new SkyLarInteractivityConfig();

        [JsonProperty]
        public SkylarDatabaseConfig Database { get; private set; } = new SkylarDatabaseConfig();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int Version { get; set; } = 1;

        public static async Task<SkyLarConfig> InitializeConfigurationAsync()
        {
            var config = new SkyLarConfig();
            var file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Config.json"));

            if (!file.Exists)
            {
                using(var writer = file.CreateText())
                {
                    await writer.WriteAsync(JsonConvert.SerializeObject(config, Formatting.Indented, Utilities.DEFAULT_JSON_SETTINGS));
                    await writer.FlushAsync();
                    return config;
                }
            }
            else
            {
                using(var reader = file.OpenText())
                {
                    var json = await reader.ReadToEndAsync();

                    if (string.IsNullOrEmpty(json))
                        return config;

                    try
                    {
                        config = JsonConvert.DeserializeObject<SkyLarConfig>(json, Utilities.DEFAULT_JSON_SETTINGS);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Cannot initialize bot configuration.", ex);
                    }

                    return config;
                }
            }
        }
    }
}

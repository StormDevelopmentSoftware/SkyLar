using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SkyLar.Entities.Settings;

namespace SkyLar.Entities
{
    /// <summary>
    /// Representa as configurações da skylar.
    /// </summary>
    public class SkyLarConfiguration
    {
        /// <summary>
        /// Determina as configurações do cliente do discord.
        /// </summary>
        [JsonProperty]
        public DiscordSettings Discord { get; private set; } = new DiscordSettings();

        /// <summary>
        /// Determina as configurações do módulo interactivity.
        /// </summary>
        [JsonProperty]
        public InteractivitySettings Interactivity { get; private set; } = new InteractivitySettings();

        /// <summary>
        /// Determina as configurações do módulo lavalink.
        /// </summary>
        [JsonProperty]
        public LavalinkSettings Lavalink { get; private set; } = new LavalinkSettings();

        /// <summary>
        /// Inicializa a configuração da skylar.
        /// </summary>
        /// <returns>Instância da configuração importada do arquivo ou criada do zero.</returns>
        public static async Task<SkyLarConfiguration> InitializeAsync()
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

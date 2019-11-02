using Newtonsoft.Json;

namespace SkyLar.Entities.Config
{
    public class SkylarDatabaseConfig
    {
        [JsonProperty]
        public string ConnectionString { get; private set; } = "mongodb://127.0.0.1";
    }
}

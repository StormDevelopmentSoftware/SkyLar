using System;
using Newtonsoft.Json;

namespace SkyLar.Entities.Config
{
    public class SkylarDatabaseConfig
    {
        [JsonProperty]
        public string ConnectionString { get; private set; } = string.Empty;
    }
}

﻿using Newtonsoft.Json;
using SkyLar.Entities.Config;
using SkyLar.Utilities;

namespace SkyLar.Entities
{
    public class SkyLarConfig
    {
        [JsonIgnore]
        public static readonly SkyLarConfig Empty = new SkyLarConfig();

        [JsonProperty]
        public SkyLarDiscordConfig Discord { get; private set; } = new SkyLarDiscordConfig();

        [JsonProperty]
        public SkyLarCommandsNextConfig CommandsNext { get; private set; } = new SkyLarCommandsNextConfig();

        [JsonProperty]
        public SkyLarInteractivityConfig Interactivity { get; private set; } = new SkyLarInteractivityConfig();

        [JsonProperty]
        public SkylarDatabaseConfig Database { get; private set; } = new SkylarDatabaseConfig();

        public static SkyLarConfig GetOrCreateDefault()
        {
            if (!FileUtilities.Exists("Config.json"))
                FileUtilities.WriteJson(Empty, "Config.json");

            return FileUtilities.ReadJson<SkyLarConfig>("Config.json");
        }
    }
}

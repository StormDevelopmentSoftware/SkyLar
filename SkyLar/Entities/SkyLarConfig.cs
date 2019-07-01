﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
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

        public static SkyLarConfig GetOrCreateDefault()
        {
            if (!FileUtilities.Exists("Config.json"))
                FileUtilities.WriteJson(Empty, "Config.json");

            return FileUtilities.ReadJson<SkyLarConfig>("Config.json");
        }
    }
}

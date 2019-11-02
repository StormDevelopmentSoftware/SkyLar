// File SkylarDatabaseConfig.cs created for the SkyLar Discord bot at 11/2/2019 7:11 PM.
// (C) Storm Development Software - 2019. All Rights Reserved
using System;
using Newtonsoft.Json;

namespace SkyLar.Entities.Config
{
        public class SkylarDatabaseConfig
        {
            [JsonProperty]
            public string ConnectionString { get; private set; }
        }
}

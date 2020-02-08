using System;
using System.Collections.Immutable;
using MongoDB.Driver;
using Newtonsoft.Json;
using SkyLar.Entities.Net;

namespace SkyLar.Entities.Settings
{
    public class DatabaseSettings
    {
        [JsonProperty]
        public ConnectionMode ConnectionMode { get; protected set; } = ConnectionMode.Direct;

        [JsonProperty]
        public TimeSpan ConnectionTimeout { get; protected set; } = TimeSpan.FromSeconds(30d);

        [JsonProperty]
        public TimeSpan HeartbeatTimeout { get; protected set; } = TimeSpan.FromSeconds(15d);

        [JsonProperty]
        public TimeSpan HeartbeatInterval { get; protected set; } = TimeSpan.FromSeconds(30d);

        [JsonProperty]
        public NetworkEndpoint Endpoint { get; protected set; } = new NetworkEndpoint("127.0.0.1", 27017);
    }
}

using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace SkyLar.Entities.Net
{
    [DebuggerDisplay("Endpoint = {" + nameof(Host) + ",nq}:{" + nameof(Port) + "}")]
    public class NetworkEndpoint : IEquatable<NetworkEndpoint>
    {
        [JsonConstructor]
        protected NetworkEndpoint()
        {

        }

        public NetworkEndpoint(string host, int port = default)
        {
            this.Host = host;
            this.Port = port;
        }

        [JsonProperty(Required = Required.Always)]
        public string Host { get; protected set; } = "127.0.0.1";

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Port { get; protected set; } = default;

        [JsonIgnore]
        public bool IsDefaultPort => this.Port == 0;

        public override int GetHashCode()
        {
            return Tuple.Create(this.Host, this.Port).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as NetworkEndpoint);
        }

        public bool Equals(NetworkEndpoint other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(other, this))
                return true;

            return this.Host == other.Host
                && this.Port == other.Port;
        }

        public override string ToString()
        {
            return $"{this.Host}:{this.Port}";
        }

        public static bool operator ==(NetworkEndpoint e1, NetworkEndpoint e2)
            => Equals(e1, e2);

        public static bool operator !=(NetworkEndpoint e1, NetworkEndpoint e2)
            => !(e1 == e2);
    }
}

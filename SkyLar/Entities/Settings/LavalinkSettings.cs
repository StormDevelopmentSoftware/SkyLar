using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using Newtonsoft.Json;

namespace SkyLar.Entities.Settings
{
    /// <summary>
    /// Representa as configurações do módulo lavalink (música) do discord.
    /// </summary>
    public class LavalinkSettings
    {
        [JsonIgnore]
        protected string PasswordInternal = "youshallnotpass";

        /// <summary>
        /// Determina a versão do lavalink que será utilizada.
        /// </summary>
        [JsonProperty]
        public LavalinkVersion Version { get; private set; } = LavalinkVersion.V3;

        /// <summary>
        /// Determina o endpoint de conexão do cliente http.
        /// <para>Na versão três do lavalink, é utilizado apenas um endpoint para ambos http e websocket.</para>
        /// </summary>
        [JsonProperty]
        public ConnectionEndpoint RestEndpoint { get; private set; } = new ConnectionEndpoint("127.0.0.1", 2333);

        /// <summary>
        /// Determina o endpoint de conexão do cliente websocket.
        /// <para>Na versão três do lavalink, é utilizado apenas um endpoint para ambos http e websocket.</para>
        /// </summary>
        [JsonProperty]
        public ConnectionEndpoint SocketEndpoint { get; private set; } = new ConnectionEndpoint("127.0.0.1", 2333);

        /// <summary>
        /// Determina a senha de conexão do lavalink.
        /// </summary>
        [JsonProperty]
        public string Password
        {
            get => string.Empty;
            protected set => this.PasswordInternal = value;
        }

        /// <summary>
        /// Cria a configuração do lavalink.
        /// </summary>
        /// <returns>Configuração do lavalink com os valores definidos.</returns>
        public LavalinkConfiguration Build()
        {
            return new LavalinkConfiguration
            {
                Password = this.PasswordInternal,
                RestEndpoint = this.RestEndpoint,
                SocketEndpoint = this.SocketEndpoint,
            };
        }

        public enum LavalinkVersion : byte
        {
            V2,
            V3
        }
    }
}
using System;
using DSharpPlus;
using Newtonsoft.Json;

namespace SkyLar.Entities.Settings
{
    /// <summary>
    /// Representa as configurações do cliente do discord.
    /// </summary>
    public class DiscordSettings
    {
        [JsonIgnore]
        protected string TokenInternal;

        /// <summary>
        /// Determina a token segura de acesso ao bot.
        /// </summary>
        [JsonProperty]
        public string Token
        {
            get => string.Empty;
            set => this.TokenInternal = value;
        }

        /// <summary>
        /// Determina se a configuração do discord possui uma token válida.
        /// </summary>
        [JsonIgnore]
        public bool HasInvalidToken => string.IsNullOrEmpty(this.TokenInternal);

        /// <summary>
        /// Determina o tipo de compressão que será utilizado durante a troca de mensagens no websocket.
        /// </summary>
        [JsonProperty]
        public GatewayCompressionLevel GatewayCompressionLevel { get; private set; } = GatewayCompressionLevel.Stream;

        /// <summary>
        /// Determina se o bot irá reconectar automaticamente durante uma queda de conexão.
        /// </summary>
        [JsonProperty]
        public bool AutoReconnect { get; private set; } = true;

        /// <summary>
        /// Determina se o bot irá tentar reconnectar indefinidamente enquanto não puder iniciar.
        /// </summary>
        [JsonProperty]
        public bool ReconnectIndefinitely { get; private set; } = false;

        /// <summary>
        /// Determina o tempo limite de resposta para as solicitações HTTP do discord.
        /// </summary>
        [JsonProperty]
        public TimeSpan HttpTimeout { get; private set; } = TimeSpan.FromSeconds(30d);

        /// <summary>
        /// Cria a configuração do cliente do discord.
        /// </summary>
        /// <param name="shardId">Identificador da shard atual.</param>
        /// <param name="shardCount">Quantidade de shards disponível.</param>
        /// <returns>Configuração para o cliente do discord com os valores definidos.</returns>
        public DiscordConfiguration Build(int shardId, int shardCount)
        {
            return new DiscordConfiguration
            {
                Token = this.TokenInternal,
                TokenType = TokenType.Bot,
                AutoReconnect = this.AutoReconnect,
                GatewayCompressionLevel = this.GatewayCompressionLevel,
                HttpTimeout = this.HttpTimeout,
                ReconnectIndefinitely = this.ReconnectIndefinitely,
                ShardId = shardId,
                ShardCount = shardCount,

#if USING_INTERNAL_LOGGER
				DateTimeFormat = "HH:mm:ss.fff",
				LogLevel = LogLevel.Debug,
				UseInternalLogHandler = true
#endif
            };
        }
    }
}

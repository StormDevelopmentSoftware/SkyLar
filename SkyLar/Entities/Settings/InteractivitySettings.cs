using System;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Newtonsoft.Json;

namespace SkyLar.Entities.Settings
{
    /// <summary>
    /// Representa as configurações do módulo de interatividade do discord.
    /// </summary>
    public class InteractivitySettings
    {
        /// <summary>
        /// Determina o comportamento da paginação.
        /// </summary>
        [JsonProperty]
        public PaginationBehaviour PaginationBehaviour { get; private set; } = PaginationBehaviour.Ignore;

        /// <summary>
        /// Determina o comportamento após finalizar a paginação.
        /// </summary>
        [JsonProperty]
        public PaginationDeletion PaginationDeletion { get; private set; } = PaginationDeletion.DeleteMessage;

        /// <summary>
        /// Determina o comportamento de votações.
        /// </summary>
        [JsonProperty]
        public PollBehaviour PollBehaviour { get; private set; } = PollBehaviour.DeleteEmojis;

        /// <summary>
        /// Determina o tempo limite para interação com o usuário.
        /// </summary>
        [JsonProperty]
        public TimeSpan Timeout { get; private set; } = TimeSpan.FromMinutes(3d);

        /// <summary>
        /// Cria a configuração do interactivity.
        /// </summary>
        /// <returns>Configuração do interactivity com os valores definidos.</returns>
        public InteractivityConfiguration Build()
        {
            return new InteractivityConfiguration
            {
                PaginationBehaviour = this.PaginationBehaviour,
                PaginationDeletion = this.PaginationDeletion,
                PollBehaviour = this.PollBehaviour,
                Timeout = this.Timeout
            };
        }
    }
}

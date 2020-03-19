using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Lavalink;
using SkyLar.Extensions;

namespace SkyLar
{
    /// <summary>
    /// Representa a classe da shard da skylar.
    /// </summary>
    public class SkyLarShard
    {
        /// <summary>
        /// Referência de acesso ao bot.
        /// </summary>
        public SkyLarBot Bot { get; internal set; }

        /// <summary>
        /// Provedor de serviços exclusívo da shard.
        /// </summary>
        public IServiceProvider Services { get; internal set; }

        /// <summary>
        /// Cliente do discord.
        /// </summary>
        public DiscordClient Discord { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">Instância atual do bot.</param>
        /// <param name="config">Configuração pré definida do discord.</param>
        public SkyLarShard(SkyLarBot bot, DiscordConfiguration config)
        {
            this.Bot = bot;
            this.Discord = new DiscordClient(config);
            this.SetupShard();
        }

        /// <summary>
        /// Configurar a shard internalmente.
        /// </summary>
        protected void SetupShard()
        {
            var cnext = this.Discord.UseCommandsNext(new CommandsNextConfiguration
            {
                UseDefaultCommandHandler = false
            });

            cnext.RegisterCommands(typeof(SkyLarShard).Assembly);
            cnext.RegisterArgumentConverters();

            this.Discord.UseInteractivity(this.Bot.Config.Interactivity.Build());
            this.Discord.UseLavalink();
        }

        /// <summary>
        /// Inicia a shard.
        /// </summary>
        public Task InitializeAsync() =>
            Task.CompletedTask;

        /// <summary>
        /// Finaliza a shard.
        /// </summary>
        public Task ShutdownAsync() =>
            Task.CompletedTask;
    }
}

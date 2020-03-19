using System;
using System.Threading.Tasks;
using DSharpPlus;

namespace SkyLar
{
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

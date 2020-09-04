using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.EntityFrameworkCore;
using SkyLar.Database;
using SkyLar.Entities.Discord;

namespace SkyLar.Commands
{
	public class TestCommand : BaseCommandModule
	{
		SkyLarContext db;

		public TestCommand(SkyLarContext context)
		{
			db = context;
		}

		[Command]
		public async Task Test(CommandContext ctx)
		{
			if (!db.Guilds.Any(x => x.GuildID == ctx.Guild.Id))
			{
				await ctx.RespondAsync("Não tenho esta guild na base de dados, inserindo com o prefixo `s!`");
				await db.AddAsync(new SkyLarGuild
				{
					GuildID = ctx.Guild.Id,
					Prefixes = new string[] { "s!" }
				});
				await db.SaveChangesAsync();
			}
			SkyLarGuild guild = db.Guilds.Where(x => x.GuildID == ctx.Guild.Id).First();
			await ctx.RespondAsync("Hi! Número pessoas aqui: ID interno " + guild.ID + " | " + (await guild.GetDiscordGuildAsync(ctx.Client)).Name );
		}
	}
}

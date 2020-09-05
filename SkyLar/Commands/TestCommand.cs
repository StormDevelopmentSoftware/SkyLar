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
			var guild = db.Guilds.Where(x => x.GuildID == ctx.Guild.Id).First();
			await ctx.RespondAsync($"Esta guild é {ctx.Guild.Name}, com ID interno `{guild.ID}`. Os prefixos são `{string.Join(", ", guild.Prefixes)}` -- e você usou o prefixo {ctx.Prefix}.");
		}
	}
}

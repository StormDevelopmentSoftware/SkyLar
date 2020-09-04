using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;

namespace SkyLar.Entities.Discord
{
	[Table("Guilds")]
	public class SkyLarGuild
	{
		[Column, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
		public int ID { get; private set; }

		[Column, Required]
		public ulong GuildID { get; set; }

		[Column("Prefixes")]
		public string[] Prefixes { get; set; } = new string[] { "s!" };

		public async Task<DiscordGuild> GetDiscordGuildAsync(DiscordClient client)
		{
			return await client.GetGuildAsync(GuildID);
		}
	}
}

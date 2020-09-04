using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SkyLar.Entities.Discord;

namespace SkyLar.Database
{
	public class SkyLarContext : DbContext
	{
		private readonly string connectionString;
		public DbSet<SkyLarGuild> Guilds { get; set; }

		public SkyLarContext()
		{

		}

		public SkyLarContext(string connectionString)
		{
			this.connectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SkyLarGuild>()
				.Property(g => g.Prefixes)
				.HasConversion(
						v => string.Join("|-;", v),
						v => v.Split("|-;", StringSplitOptions.RemoveEmptyEntries));
		}
	}
}

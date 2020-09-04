using System;
using Newtonsoft.Json;

namespace SkyLar.Entities.Settings
{
	public class DatabaseSettings
	{
		[JsonProperty]
		public string Address { get; private set; }

		[JsonProperty]
		public string Username { get; private set; }

		[JsonProperty]
		public string Password { get; private set; }

		[JsonProperty]
		public string Database { get; private set; }

		public override string ToString()
		{
			return $"Server={Address};Database={Database};Uid={Username};Pwd={Password};";
		}
	}
}

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SkyLar
{
	public static class Utilities
	{
		internal static readonly HttpClient Http = new HttpClient();

		public static async Task<(bool Success, int Result)> GetShardCountAsync(string token)
		{
			using var req = new HttpRequestMessage(HttpMethod.Get, "https://discordapp.com/api/gateway/bot");
			req.Headers.TryAddWithoutValidation("Authorization", $"Bot {token}");

			using var res = await Http.SendAsync(req);

			if (!res.IsSuccessStatusCode)
				return (false, (int)res.StatusCode);
			else
			{
				var str = await res.Content.ReadAsStringAsync();
				var json = JObject.Parse(str);
				return (true, json.Value<int>("shards"));
			}
		}
	}
}

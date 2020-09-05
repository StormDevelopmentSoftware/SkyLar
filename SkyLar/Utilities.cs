using System.Collections.Generic;
using System.Linq;
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

		// Taken from DSharpPlus.CommandsNext/CommandsNextUtilities.cs
		public static string ExtractNextArgument(this string str, ref int startPos)
		{
			if (string.IsNullOrWhiteSpace(str))
				return null;

			var inBacktick = false;
			var inTripleBacktick = false;
			var inQuote = false;
			var inEscape = false;
			var removeIndices = new List<int>(str.Length - startPos);

			var i = startPos;
			for (; i < str.Length; i++)
				if (!char.IsWhiteSpace(str[i]))
					break;
			startPos = i;

			var endPosition = -1;
			var startPosition = startPos;
			for (i = startPosition; i < str.Length; i++)
			{
				if (char.IsWhiteSpace(str[i]) && !inQuote && !inTripleBacktick && !inBacktick && !inEscape)
					endPosition = i;

				if (str[i] == '\\' && str.Length > i + 1)
				{
					if (!inEscape && !inBacktick && !inTripleBacktick)
					{
						inEscape = true;
						if (str.IndexOf("\\`", i) == i || str.IndexOf("\\\"", i) == i || str.IndexOf("\\\\", i) == i || (str.Length >= i && char.IsWhiteSpace(str[i + 1])))
							removeIndices.Add(i - startPosition);
						i++;
					}
					else if ((inBacktick || inTripleBacktick) && str.IndexOf("\\`", i) == i)
					{
						inEscape = true;
						removeIndices.Add(i - startPosition);
						i++;
					}
				}

				if (str[i] == '`' && !inEscape)
				{
					var tripleBacktick = str.IndexOf("```", i) == i;
					if (inTripleBacktick && tripleBacktick)
					{
						inTripleBacktick = false;
						i += 2;
					}
					else if (!inBacktick && tripleBacktick)
					{
						inTripleBacktick = true;
						i += 2;
					}

					if (inBacktick && !tripleBacktick)
						inBacktick = false;
					else if (!inTripleBacktick && tripleBacktick)
						inBacktick = true;
				}

				if (str[i] == '"' && !inEscape && !inBacktick && !inTripleBacktick)
				{
					removeIndices.Add(i - startPosition);

					if (!inQuote)
						inQuote = true;
					else
						inQuote = false;
				}

				if (inEscape)
					inEscape = false;

				if (endPosition != -1)
				{
					startPos = endPosition;
					if (startPosition != endPosition)
						return str.Substring(startPosition, endPosition - startPosition).CleanupString(removeIndices);
					return null;
				}
			}

			startPos = str.Length;
			if (startPos != startPosition)
				return str.Substring(startPosition).CleanupString(removeIndices);
			return null;
		}

		public static string CleanupString(this string s, IList<int> indices)
		{
			if (!indices.Any())
				return s;

			var li = indices.Last();
			var ll = 1;
			for (var x = indices.Count - 2; x >= 0; x--)
			{
				if (li - indices[x] == ll)
				{
					ll++;
					continue;
				}

				s = s.Remove(li - ll + 1, ll);
				li = indices[x];
				ll = 1;
			}

			return s.Remove(li - ll + 1, ll);
		}
	}
}

using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json.Linq;
using SkyLar.Attributes;

namespace SkyLar.Commands
{
    public partial class FunCommands : BaseCommandModule
    {
        [Command("randomdog")]
        [CommandCategory(Category.Fun)]
        public async Task RandomDogCommand(CommandContext ctx)
        {
            using (var http = new HttpClient())
            {
                var raw = await http.GetStringAsync("https://dog.ceo/api/breeds/image/random");
                var json = JObject.Parse(raw);

                if (json["status"].ToString() == "success")
                {
                    var url = json["message"].ToString();

                    await ctx.RespondWithEmbedAsync(ctx.GetBaseEmbed()
                        .WithImageUrl(url)
                        .WithTitle(":dog: Random Dog"));
                }
                else
                {
                    await ctx.RespondAsync("<:ptr_err:451145098470752286> An error occurred while contacting the Random Dog API. Please try again later.");
                }
            }
        }
    }
}

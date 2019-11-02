// File RandomDogCommand.cs for the SkyLar Discord bot at 11/2/2019 2:28 PM.
// (C) Storm Development Software - 2019. All Rights Reserved
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SkyLar.Attributes;
using SkyLar.Utilities;

namespace SkyLar.Commands
{
    public partial class FunCommands : BaseCommandModule
    {
        [Command("randomdog")]
        [CommandCategory(Category.Fun)]
        public async Task RandomDogCommand(CommandContext ctx)
        {
            HttpClient http = new HttpClient();
            var jsonString = await http.GetStringAsync("https://dog.ceo/api/breeds/image/random");

            var json = FileUtilities.FromJson(jsonString);

            if (json["status"].ToString() == "success")
            {
                var url = json["message"].ToString();
                await ctx.RespondWithEmbedAsync(new DiscordEmbedBuilder(ctx.BaseEmbed())
                {
                    ImageUrl = url,
                    Title = ":dog: Random Dog",
                });
            }
            else
            {
                await ctx.RespondAsync("<:ptr_err:451145098470752286> An error occurred while contacting the Random Dog API. Please try again later.");
            }
        }
    }
}

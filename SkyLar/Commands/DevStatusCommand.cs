using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Humanizer;
using Humanizer.Bytes;
using SkyLar.Attributes;

namespace SkyLar.Commands
{
    public partial class DeveloperCommands : BaseCommandModule
    {
        [Command("devstatus")]
        [CommandCategory(Category.Developer)]
        [DeveloperCommand]
        public async Task DevStatusCommand(CommandContext ctx)
        {
            var os = Environment.OSVersion.VersionString;

            await ctx.RespondWithEmbedAsync(ctx.GetBaseEmbed()
                .AddField("OS", os, true)
                .AddField("RAM", GetMemoryUsage().Bytes().Humanize("#.## MB"), true)
                .AddField("Ping", ctx.Client.Ping + "ms", true)
                .AddField("DSharpPlus", ctx.Client.VersionString, true)
                .AddField("Guilds", ctx.Client.Guilds.Count.ToString(), true));
        }

        // TODO

        public static long GetMemoryUsage()
        {
            var process = Process.GetCurrentProcess();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return process.WorkingSet64;
            else //if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                /*var file = new FileInfo($"/proc/{process.Id}/stat"));
                
                if (!file.Exists)
                    return 0;

                using(var reader = file.OpenText())
                {
                    string line;

                    while((line = reader.ReadLine()) != null)
                    {

                    }
                }*/

                return 0;
            }
        }
    }
}

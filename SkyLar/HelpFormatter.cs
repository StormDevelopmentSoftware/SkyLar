using System;
using System.Collections.Generic;
using System.Linq;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using SkyLar.Attributes;

namespace SkyLar
{
    public class HelpFormatter : BaseHelpFormatter
    {
        private CommandContext _ctx;
        public DiscordEmbedBuilder EmbedBuilder { get; }
        private Command command { get; set; }
        public List<Command> InfoCommands = new List<Command>();
        public List<Command> DeveloperCommands = new List<Command>();

        public Dictionary<Categories, List<Command>> dict = new Dictionary<Categories, List<Command>>();

        public HelpFormatter(CommandContext ctx) : base(ctx)
        {
            EmbedBuilder = new DiscordEmbedBuilder()
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = "Help"
                },
                Color = DiscordColor.Blurple
            };

            _ctx = ctx;

            dict = new Dictionary<Categories, List<Command>> { { Categories.Info, InfoCommands }, { Categories.Developer, DeveloperCommands } };

            foreach (var cmd in ctx.CommandsNext.RegisteredCommands)
            {
                foreach (Attribute att in cmd.Value.CustomAttributes)
                {
                    if (att is CommandCategory)
                    { 
                        var check = cmd.Value.RunChecksAsync(ctx, true).GetAwaiter().GetResult().Any();
                        if (!check)
                        {
                            dict[((CommandCategory)att).Category].Add(cmd.Value);
                        }
                        
                    }
                }
            }
        }

        public override CommandHelpMessage Build()
        {

            if (command == null)
            {
                EmbedBuilder.Description = $"Listing all commands you have permission to execute.\nFor more information, use `{_ctx.Prefix}help [command]`.";
                EmbedBuilder.AddField("**:information_source: | Information**", string.Join(", ", InfoCommands.Distinct().Select(x => Formatter.InlineCode(x.Name))));
                EmbedBuilder.AddField("**:tools: | Developer**", string.Join(", ", DeveloperCommands.Distinct().Select(x => Formatter.InlineCode(x.Name))));
            }

            return new CommandHelpMessage(embed: EmbedBuilder);
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            this.command = command;

            EmbedBuilder.Description = Formatter.InlineCode(command.Name) + ": " + command.Description ?? "No description available at this moment.";
            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {

            return this;
        }
    }
}

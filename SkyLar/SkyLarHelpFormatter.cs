using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;
using SkyLar.Attributes;

namespace SkyLar
{
    public class SkyLarHelpFormatter : BaseHelpFormatter
    {
        private static string BuildRawCommandString(List<Command> list) => 
            string.Join(", ", list.Distinct().Select(x => Formatter.InlineCode(x.Name)));

        public DiscordEmbedBuilder EmbedBuilder { get; }
        private Command command { get; set; }
        public List<Command> InfoCommands = new List<Command>();
        public List<Command> DeveloperCommands = new List<Command>();
        public List<Command> UtilityCommands = new List<Command>();
        public List<Command> FunCommands = new List<Command>();

        public Dictionary<Category, List<Command>> commands_with_categories = new Dictionary<Category, List<Command>>();
        public Dictionary<List<Command>, string> text = new Dictionary<List<Command>, string>();
        public Dictionary<Command, string[]> examples = new Dictionary<Command, string[]>();

        public SkyLarHelpFormatter(CommandContext ctx) : base(ctx)
        {

            this.EmbedBuilder = new DiscordEmbedBuilder(ctx.GetBaseEmbed())
            {
                Title = ":question: **Help**",
            };

            this.commands_with_categories = new Dictionary<Category, List<Command>> { { Category.Info, this.InfoCommands }, { Category.Developer, this.DeveloperCommands }, { Category.Utility, this.UtilityCommands }, { Category.Fun, this.FunCommands } };
            this.text = new Dictionary<List<Command>, string> { { this.InfoCommands, ":information_source: • Information" }, { this.DeveloperCommands, "<:netcore:378151776320487424> • Developer" }, { this.UtilityCommands, ":tools: • Utility" }, { this.FunCommands, ":balloon: • Fun" } };

            this.InitializeCommandsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task InitializeCommandsAsync()
        {
            foreach (var command in this.CommandsNext.RegisteredCommands.Select(x => x.Value))
            {
                if (command.CustomAttributes.Any(x => x is CommandCategoryAttribute))
                {
                    var attrib = command.CustomAttributes.First(x => x is CommandCategoryAttribute) as CommandCategoryAttribute;

                    if (attrib != null)
                    {
                        var checks = await command.RunChecksAsync(this.Context, true);

                        if (!checks.Any())
                            this.commands_with_categories[attrib.Category].Add(command);
                    }
                }

                if (command.CustomAttributes.Any(x => x is ExampleAttribute))
                {
                    var attrib = command.CustomAttributes.FirstOrDefault(x => x is ExampleAttribute) as ExampleAttribute;

                    if(attrib != null)
                        this.examples.Add(command, attrib.Examples);
                }
            }
        }

        public override CommandHelpMessage Build()
        {

            if (this.command == null)
            {
                this.EmbedBuilder.Description = $"Listing all commands you have permission to execute.\nFor more information, use **`{this.Context.Prefix}help [command]`**.";

                foreach (var abc in this.text.Keys)
                {
                    if (abc.Any())
                        this.EmbedBuilder.AddField(this.text[abc], BuildRawCommandString(abc));
                }
            }

            return new CommandHelpMessage(embed: this.EmbedBuilder);
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            this.command = command;

            var desc = command.Description.IsWhiteSpace() ? "No description available at this moment." : command.Description;

            this.EmbedBuilder.WithDescription(Formatter.InlineCode(command.Name) + ": " + desc + "\n");

            if (command.Aliases.Any())
            {
                this.EmbedBuilder.AddField(":repeat: Aliases", string.Join(", ", command.Aliases.Select(Formatter.InlineCode)));
            }

            if (command.Overloads?.Any() == true)
            {
                var sb = new StringBuilder();

                foreach (var ovl in command.Overloads.OrderByDescending(x => x.Priority))
                {
                    sb.Append('`').Append(this.Context.Prefix).Append(command.QualifiedName);

                    foreach (var arg in ovl.Arguments)
                        sb.Append(arg.IsOptional || arg.IsCatchAll ? " [" : " <").Append(arg.Name).Append(arg.IsCatchAll ? "..." : "").Append(arg.IsOptional || arg.IsCatchAll ? ']' : '>');

                    sb.Append("`\n");
                }

                this.EmbedBuilder.AddField(":pen_ballpoint: Arguments", sb.ToString().Trim(), false);
            }
            if (this.examples.ContainsKey(command))
                this.EmbedBuilder.AddField(":mag_right: Examples", string.Join('\n', this.examples[command].Select(x => Formatter.InlineCode(this.Context.Prefix + x))));

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            if (this.command != null)
            {
                this.EmbedBuilder.AddField("Commands", string.Join(", ", subcommands.Select(x => Formatter.InlineCode(x.Name))));
            }

            return this;
        }
    }
}

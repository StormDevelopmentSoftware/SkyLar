using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using DSharpPlus.CommandsNext;
using Newtonsoft.Json;

namespace SkyLar.Entities.Config
{
    public class SkyLarCommandsNextConfig
    {
        [JsonProperty]
        public ImmutableArray<string> StringPrefixes { get; private set; } = ImmutableArray.Create("skylar ", "s!");

        [JsonProperty]
        public bool DmHelp { get; private set; } = false;

        [JsonProperty]
        public bool EnableDms { get; private set; } = true;

        [JsonProperty]
        public bool EnableMentionPrefix { get; private set; } = false;

        public CommandsNextConfiguration Build(IServiceProvider services)
        {
            return new CommandsNextConfiguration
            {
                StringPrefixes = this.StringPrefixes,
                DmHelp = this.DmHelp,
                EnableDms = this.EnableDms,
                EnableMentionPrefix = this.EnableMentionPrefix,
                Services = services,
                EnableDefaultHelp = true
            };
        }
    }
}

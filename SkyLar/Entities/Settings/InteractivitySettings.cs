using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Newtonsoft.Json;

namespace SkyLar.Entities.Settings
{
	public class InteractivitySettings
	{
		[JsonProperty]
		public PaginationBehaviour PaginationBehaviour { get; private set; } = PaginationBehaviour.Ignore;

		[JsonProperty]
		public PaginationDeletion PaginationDeletion { get; private set; } = PaginationDeletion.DeleteMessage;

		[JsonProperty]
		public PollBehaviour PollBehaviour { get; private set; } = PollBehaviour.DeleteEmojis;

		[JsonProperty]
		public TimeSpan Timeout { get; private set; } = TimeSpan.FromMinutes(3d);

		public InteractivityConfiguration Build()
		{
			return new InteractivityConfiguration
			{
				PaginationBehaviour = this.PaginationBehaviour,
				PaginationDeletion = this.PaginationDeletion,
				PollBehaviour = this.PollBehaviour,
				Timeout = this.Timeout
			};
		}
	}
}

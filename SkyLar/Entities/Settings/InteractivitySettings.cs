﻿using System;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;

namespace SkyLar.Entities.Settings
{
	public class InteractivitySettings
	{
		public PaginationBehaviour PaginationBehaviour { get; private set; } = PaginationBehaviour.WrapAround;
		public PaginationDeletion PaginationDeletion { get; private set; } = PaginationDeletion.DeleteMessage;
		public PollBehaviour PollBehaviour { get; private set; } = PollBehaviour.KeepEmojis;
		public TimeSpan Timeout { get; private set; } = TimeSpan.FromMinutes(2.5d);

		public InteractivityConfiguration Build() => new InteractivityConfiguration
		{
			PaginationBehaviour = this.PaginationBehaviour,
			PaginationDeletion = this.PaginationDeletion,
			PollBehaviour = this.PollBehaviour,
			Timeout = this.Timeout
		};
	}
}

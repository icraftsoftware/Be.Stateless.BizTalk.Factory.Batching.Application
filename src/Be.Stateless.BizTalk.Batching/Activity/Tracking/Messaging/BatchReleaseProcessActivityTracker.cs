#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Be.Stateless.BizTalk.Component.Extensions;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Message.Extensions;
using Be.Stateless.Extensions;
using log4net;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace Be.Stateless.BizTalk.Activity.Tracking.Messaging
{
	[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global", Justification = "virtual members for mocking purposes.")]
	internal class BatchReleaseProcessActivityTracker
	{
		internal static BatchReleaseProcessActivityTracker Create(IPipelineContext pipelineContext, IBaseMessage message)
		{
			return Factory(pipelineContext, message);
		}

		#region Mock's Factory Hook Point

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "internal for testing purposes.")]
		internal static Func<IPipelineContext, IBaseMessage, BatchReleaseProcessActivityTracker> Factory { get; set; } =
			(pipelineContext, message) => new BatchReleaseProcessActivityTracker(pipelineContext, message);

		#endregion

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "protected for mocking purposes.")]
		protected BatchReleaseProcessActivityTracker(IPipelineContext pipelineContext, IBaseMessage message)
		{
			_pipelineContext = pipelineContext;
			_message = message;
		}

		internal virtual void TrackActivity(BatchTrackingContext batchTrackingContext)
		{
			if (batchTrackingContext?.MessagingStepActivityIdList != null && batchTrackingContext.MessagingStepActivityIdList.Length > 0)
			{
				if (_logger.IsInfoEnabled) _logger.Debug("Associating the batch being released with its parts.");
				var activityFactory = _pipelineContext.GetBatchActivityFactory();
				var process = batchTrackingContext.ProcessActivityId.IsNullOrEmpty()
					? activityFactory.CreateProcess(_message, BizTalk.Factory.Areas.Batch.Processes.Release)
					: activityFactory.FindProcess(batchTrackingContext.ProcessActivityId);

				process.TrackActivity();
				process.AddSteps(batchTrackingContext.MessagingStepActivityIdList.Append(_message.GetProperty(TrackingProperties.MessagingStepActivityId)));
			}
			else
			{
				if (_logger.IsInfoEnabled) _logger.Debug("The batch being released cannot be associated with its parts because their ActivityIDs have not been captured.");
			}
		}

		private static readonly ILog _logger = LogManager.GetLogger(typeof(BatchReleaseProcessActivityTracker));
		private readonly IBaseMessage _message;
		private readonly IPipelineContext _pipelineContext;
	}
}

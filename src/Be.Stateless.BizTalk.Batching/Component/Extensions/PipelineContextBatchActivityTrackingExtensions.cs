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
using Be.Stateless.BizTalk.Activity.Tracking.Messaging;
using Microsoft.BizTalk.Component.Interop;

namespace Be.Stateless.BizTalk.Component.Extensions
{
	internal static class PipelineContextBatchActivityTrackingExtensions
	{
		#region Mock's Factory Hook Point

		internal static Func<IPipelineContext, IBatchReleaseProcessActivityFactory> ActivityFactoryFactory { get; set; } =
			pipelineContext => new BatchReleaseProcessActivityFactory(pipelineContext);

		#endregion

		/// <summary>
		/// Batch tracking-activity factory for messaging-only activities.
		/// </summary>
		/// <param name="pipelineContext">
		/// The pipeline context from which messaging-only activities can be created.
		/// </param>
		/// <returns>
		/// The activity factory.
		/// </returns>
		/// <remarks>
		/// The purpose of this factory is to make <see cref="IPipelineContext"/> extension methods amenable to mocking, <see
		/// href="http://blogs.clariusconsulting.net/kzu/how-to-mock-extension-methods/"/>.
		/// </remarks>
		/// <seealso href="http://blogs.clariusconsulting.net/kzu/how-extension-methods-ruined-unit-testing-and-oop-and-a-way-forward/"/>
		/// <seealso href="http://blogs.clariusconsulting.net/kzu/making-extension-methods-amenable-to-mocking/"/>
		public static IBatchReleaseProcessActivityFactory GetBatchActivityFactory(this IPipelineContext pipelineContext)
		{
			return ActivityFactoryFactory(pipelineContext);
		}
	}
}

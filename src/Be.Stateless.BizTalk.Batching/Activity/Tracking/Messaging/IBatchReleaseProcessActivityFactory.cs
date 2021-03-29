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

using Be.Stateless.BizTalk.Schemas.Xml;
using Microsoft.BizTalk.Bam.EventObservation;
using Microsoft.BizTalk.Message.Interop;

namespace Be.Stateless.BizTalk.Activity.Tracking.Messaging
{
	/// <summary>
	/// Batch-release process tracking-activity factory.
	/// </summary>
	/// <remarks>
	/// Notice that this is by design a messaging-only activity, i.e. <see cref="EventStream"/>-based activities.
	/// </remarks>
	internal interface IBatchReleaseProcessActivityFactory
	{
		/// <summary>
		/// Creates a batch release <see cref="BatchReleaseProcess"/> tracking activity.
		/// </summary>
		/// <param name="message">
		/// The <see cref="IBaseMessage"/> denoting the <see cref="Batch.Content"/> being released.
		/// </param>
		/// <param name="name">
		/// The name of the process to create.
		/// </param>
		/// <returns>
		/// A <see cref="Process"/> instance that will be used to feed the BAM Process activity.
		/// </returns>
		BatchReleaseProcess CreateProcess(IBaseMessage message, string name);

		/// <summary>
		/// Creates a reference to the batch release <see cref="BatchReleaseProcessReference"/> tracking activity that was
		/// initiated by a <see cref="Batch.Release"/> control message.
		/// </summary>
		/// <param name="processActivityId">
		/// The <see cref="Process.ActivityId"/> of the batch release process that was initiated by a <see cref="Batch.Release"/>
		/// control message.
		/// </param>
		/// <returns>
		/// A <see cref="ProcessReference"/> instance that will be used to feed the BAM Process activity.
		/// </returns>
		BatchReleaseProcess FindProcess(string processActivityId);
	}
}

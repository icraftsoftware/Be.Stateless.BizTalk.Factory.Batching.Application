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

using Be.Stateless.BizTalk.Activity.Tracking;
using Be.Stateless.BizTalk.Schemas.Xml;

namespace Be.Stateless.BizTalk.Stream.Extensions
{
	/// <summary>
	/// Probes the current <see cref="Batch.Content"/> <see cref="System.IO.Stream"/>.
	/// </summary>
	internal interface IProbeBatchContentStream
	{
		/// <summary>
		/// Probes the current <see cref="Batch.Content"/> <see cref="System.IO.Stream"/> for a <see cref="BatchDescriptor"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="BatchDescriptor"/> if probing is successful, <c>null</c> otherwise.
		/// </returns>
		/// <remarks>
		/// Probing will be successful only if the <see cref="System.IO.Stream"/>'s content is an instance of the <see
		/// cref="Batch.Content"/> schema &#8212; only the <see cref="BTS.MessageType"/> is verified,&#8212; and its
		/// <c>EnvelopeSpecName</c> element is not null nor empty.
		/// </remarks>
		BatchDescriptor BatchDescriptor { get; }

		/// <summary>
		/// Probes the current <see cref="Batch.Content"/> <see cref="System.IO.Stream"/> for a <see
		/// cref="BatchTrackingContext"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="BatchTrackingContext"/> if probing is successful, <c>null</c> otherwise.
		/// </returns>
		/// <remarks>
		/// Probing will be successful only if the <see cref="System.IO.Stream"/>'s content is an instance of the <see
		/// cref="Batch.Content"/> schema &#8212; only the <see cref="BTS.MessageType"/> is verified,&#8212; and its
		/// <c>EnvelopeSpecName</c> element is not null nor empty.
		/// </remarks>
		BatchTrackingContext BatchTrackingContext { get; }
	}
}

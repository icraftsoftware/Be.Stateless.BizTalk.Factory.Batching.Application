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
using Microsoft.BizTalk.Streaming;

namespace Be.Stateless.BizTalk.Stream.Extensions
{
	internal static class BatchContentStreamExtensions
	{
		/// <summary>
		/// Support for <see cref="Batch.Content"/> <see cref="System.IO.Stream"/> probing.
		/// </summary>
		/// <param name="stream">
		/// The current <see cref="System.IO.Stream"/>.
		/// </param>
		/// <returns>
		/// The <see cref="IProbeBatchContentStream"/> instance that will probe the current <see cref="System.IO.Stream"/>s.
		/// </returns>
		/// <remarks>
		/// The <paramref name="stream"/> is expected to be markable, that is to say, it has to be of type <see
		/// cref="MarkableForwardOnlyEventingReadStream"/>.
		/// </remarks>
		/// <seealso cref="StreamExtensions.Probe"/>
		internal static IProbeBatchContentStream ProbeBatchContent(this System.IO.Stream stream)
		{
			return new BatchContentProber(stream.Probe());
		}
	}
}

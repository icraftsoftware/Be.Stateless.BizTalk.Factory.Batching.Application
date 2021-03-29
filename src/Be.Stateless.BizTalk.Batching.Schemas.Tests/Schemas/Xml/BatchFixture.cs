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

using Be.Stateless.BizTalk.Message;
using Be.Stateless.BizTalk.Resources;
using Be.Stateless.BizTalk.Unit.Schema;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Be.Stateless.BizTalk.Schemas.Xml
{
	public class BatchFixture : SchemaFixture<Batch>
	{
		[Theory]
		[InlineData("BatchContent.xml")]
		[InlineData("BatchContentWithEnvelopeSpecName.xml")]
		[InlineData("BatchContentWithEnvironmentTag.xml")]
		[InlineData("BatchContentWithEnvironmentTagAndPartition.xml")]
		[InlineData("BatchContentWithPartition.xml")]
		public void ValidateBatchContent(string name)
		{
			Invoking(() => MessageBodyFactory.Create<Batch.Content>(MessageBody.Samples.LoadString($"Message.{name}")))
				.Should().NotThrow();
		}

		[Theory]
		[InlineData("ReleaseBatch.xml")]
		[InlineData("ReleaseBatchWithEnvironmentTag.xml")]
		[InlineData("ReleaseBatchWithEnvironmentTagAndPartition.xml")]
		[InlineData("ReleaseBatchWithPartition.xml")]
		public void ValidateReleaseBatch(string name)
		{
			Invoking(() => MessageBodyFactory.Create<Batch.Release>(MessageBody.Samples.LoadString($"Message.{name}")))
				.Should().NotThrow();
		}
	}
}

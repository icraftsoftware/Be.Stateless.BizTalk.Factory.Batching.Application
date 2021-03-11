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

using Be.Stateless.BizTalk.Resources;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Schemas.Xml;
using Be.Stateless.IO;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.Stream.Extensions
{
	public class BatchContentProberFixture
	{
		[Fact]
		public void BatchDescriptor()
		{
			using (var stream = MessageBody.Samples.Load("Message.BatchContent.xml").AsMarkable())
			{
				var batchDescriptor = stream.ProbeBatchContent().BatchDescriptor;
				batchDescriptor.EnvelopeSpecName.Should().Be(SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName);
				batchDescriptor.Partition.Should().BeNull();
			}
		}

		[Fact]
		public void BatchDescriptorHasEnvironmentTag()
		{
			const string batchContent = @"<ns:BatchContent xmlns:ns=""urn:schemas.stateless.be:biztalk:batch:2012:12"">
  <ns:EnvelopeSpecName>envelope-spec-name</ns:EnvelopeSpecName>
  <ns:EnvironmentTag>environment-tag</ns:EnvironmentTag>
  <ns:MessagingStepActivityIds>013684EE620E4A0BB6D6F7355B26D21B</ns:MessagingStepActivityIds>
  <ns:Parts />
</BatchContent>";
			using (var stream = new StringStream(batchContent).AsMarkable())
			{
				var batchDescriptor = stream.ProbeBatchContent().BatchDescriptor;
				batchDescriptor.EnvelopeSpecName.Should().Be("envelope-spec-name");
				batchDescriptor.EnvironmentTag.Should().Be("environment-tag");
			}
		}

		[Fact]
		public void BatchDescriptorHasEnvironmentTagAndPartition()
		{
			const string batchContent = @"<ns:BatchContent xmlns:ns=""urn:schemas.stateless.be:biztalk:batch:2012:12"">
  <ns:EnvelopeSpecName>envelope-spec-name</ns:EnvelopeSpecName>
  <ns:EnvironmentTag>environment-tag</ns:EnvironmentTag>
  <ns:Partition>p-one</ns:Partition>
  <ns:MessagingStepActivityIds>013684EE620E4A0BB6D6F7355B26D21B</ns:MessagingStepActivityIds>
  <ns:Parts />
</BatchContent>";
			using (var stream = new StringStream(batchContent).AsMarkable())
			{
				var batchDescriptor = stream.ProbeBatchContent().BatchDescriptor;
				batchDescriptor.EnvelopeSpecName.Should().Be("envelope-spec-name");
				batchDescriptor.EnvironmentTag.Should().Be("environment-tag");
				batchDescriptor.Partition.Should().Be("p-one");
			}
		}

		[Fact]
		public void BatchDescriptorHasPartition()
		{
			const string batchContent = @"<ns:BatchContent xmlns:ns=""urn:schemas.stateless.be:biztalk:batch:2012:12"">
  <ns:EnvelopeSpecName>envelope-spec-name</ns:EnvelopeSpecName>
  <ns:Partition>p-one</ns:Partition>
  <ns:MessagingStepActivityIds>013684EE620E4A0BB6D6F7355B26D21B</ns:MessagingStepActivityIds>
  <ns:Parts />
</BatchContent>";
			using (var stream = new StringStream(batchContent).AsMarkable())
			{
				var batchDescriptor = stream.ProbeBatchContent().BatchDescriptor;
				batchDescriptor.EnvelopeSpecName.Should().Be("envelope-spec-name");
				batchDescriptor.Partition.Should().Be("p-one");
			}
		}

		[Fact]
		public void BatchDescriptorIsNullIfIncompleteBatchContent()
		{
			const string batchContent = @"<ns:BatchContent xmlns:ns=""urn:schemas.stateless.be:biztalk:batch:2012:12"" />";
			using (var stream = new StringStream(batchContent).AsMarkable())
			{
				stream.ProbeBatchContent().BatchDescriptor.Should().BeNull();
			}
		}

		[Fact]
		public void BatchDescriptorIsNullIfInvalidBatchContent()
		{
			const string batchContent = @"<ns:BatchContent xmlns:ns=""urn:schemas.stateless.be:biztalk:batch:2012:12"">
  <ns:MessagingStepActivityIds />
  <ns:EnvelopeSpecName>Be.Stateless.BizTalk.Schemas.Xml.BatchControl+ReleaseBatches</ns:EnvelopeSpecName>
  <ns:Parts />
</BatchContent>";
			using (var stream = new StringStream(batchContent).AsMarkable())
			{
				stream.ProbeBatchContent().BatchDescriptor.Should().BeNull();
			}
		}

		[Fact]
		public void BatchDescriptorIsNullIfNotXml()
		{
			using (var stream = new StringStream("invalid xml content").AsMarkable())
			{
				stream.ProbeBatchContent().BatchDescriptor.Should().BeNull();
			}
		}

		[Fact]
		public void BatchTrackingContext()
		{
			using (var stream = MessageBody.Samples.Load("Message.BatchContent.xml").AsMarkable())
			{
				var batchTrackingContext = stream.ProbeBatchContent().BatchTrackingContext;
				batchTrackingContext.MessagingStepActivityIdList.Should().BeEquivalentTo(
					"013684EE620E4A0BB6D6F7355B26D21B",
					"08FCB363E00F4BD78D15D8EB2E80B411",
					"0B12CC6AE51740F6ABF672E3B32B496D");
				batchTrackingContext.ProcessActivityId.Should().Be("A800441B209E46A087A16833661590C2");
			}
		}

		[Fact]
		public void BatchTrackingContextIsEmptyIfIncompleteBatchContent()
		{
			const string batchContent = @"<ns:BatchContent xmlns:ns=""urn:schemas.stateless.be:biztalk:batch:2012:12"" />";
			using (var stream = new StringStream(batchContent).AsMarkable())
			{
				var batchTrackingContext = stream.ProbeBatchContent().BatchTrackingContext;
				batchTrackingContext.ProcessActivityId.Should().BeNull();
				batchTrackingContext.MessagingStepActivityIdList.Should().BeNull();
			}
		}

		[Fact]
		public void BatchTrackingContextIsEmptyIfInvalidBatchContent()
		{
			const string batchContent = @"<ns:BatchContent xmlns:ns=""urn:schemas.stateless.be:biztalk:batch:2012:12"">
  <ns:MessagingStepActivityIds />
  <ns:EnvelopeSpecName>Be.Stateless.BizTalk.Schemas.Xml.BatchControl+ReleaseBatches</ns:EnvelopeSpecName>
  <ns:Parts />
</BatchContent>";
			using (var stream = new StringStream(batchContent).AsMarkable())
			{
				var batchTrackingContext = stream.ProbeBatchContent().BatchTrackingContext;
				batchTrackingContext.ProcessActivityId.Should().BeNull();
				batchTrackingContext.MessagingStepActivityIdList.Should().BeNull();
			}
		}

		[Fact]
		public void BatchTrackingContextIsNullIfNotXml()
		{
			using (var stream = new StringStream("invalid xml content").AsMarkable())
			{
				stream.ProbeBatchContent().BatchTrackingContext.Should().BeNull();
			}
		}

		[Fact]
		public void BatchTrackingContextOnlyHasMessagingStepActivityIds()
		{
			const string batchContent = @"<ns:BatchContent xmlns:ns=""urn:schemas.stateless.be:biztalk:batch:2012:12"">
  <ns:EnvelopeSpecName>Be.Stateless.BizTalk.Schemas.Xml.BatchControl+ReleaseBatches</ns:EnvelopeSpecName>
  <ns:MessagingStepActivityIds>013684EE620E4A0BB6D6F7355B26D21B,08FCB363E00F4BD78D15D8EB2E80B411,0B12CC6AE51740F6ABF672E3B32B496D</ns:MessagingStepActivityIds>
  <ns:Parts />
</BatchContent>";
			using (var stream = new StringStream(batchContent).AsMarkable())
			{
				var batchTrackingContext = stream.ProbeBatchContent().BatchTrackingContext;
				batchTrackingContext.ProcessActivityId.Should().BeNull();
				batchTrackingContext.MessagingStepActivityIdList.Should().BeEquivalentTo(
					"013684EE620E4A0BB6D6F7355B26D21B",
					"08FCB363E00F4BD78D15D8EB2E80B411",
					"0B12CC6AE51740F6ABF672E3B32B496D");
			}
		}

		[Fact]
		public void BatchTrackingContextOnlyHasProcessActivityId()
		{
			const string batchContent = @"<ns:BatchContent xmlns:ns=""urn:schemas.stateless.be:biztalk:batch:2012:12"">
  <ns:EnvelopeSpecName>Be.Stateless.BizTalk.Schemas.Xml.BatchControl+ReleaseBatches</ns:EnvelopeSpecName>
  <ns:Partition>partition</ns:Partition>
  <ns:ProcessActivityId>A800441B209E46A087A16833661590C2</ns:ProcessActivityId>
  <ns:Parts />
</BatchContent>";
			using (var stream = new StringStream(batchContent).AsMarkable())
			{
				var batchTrackingContext = stream.ProbeBatchContent().BatchTrackingContext;
				batchTrackingContext.ProcessActivityId.Should().Be("A800441B209E46A087A16833661590C2");
				batchTrackingContext.MessagingStepActivityIdList.Should().BeNull();
			}
		}
	}
}

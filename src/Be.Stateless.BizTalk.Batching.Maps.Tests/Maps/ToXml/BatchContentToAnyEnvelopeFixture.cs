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

using System.Linq;
using Be.Stateless.BizTalk.Message;
using Be.Stateless.BizTalk.Resources;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Schemas.Xml;
using Be.Stateless.BizTalk.Unit.Transform;
using Be.Stateless.Xml.Extensions;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.Maps.ToXml
{
	public class BatchContentToAnyEnvelopeFixture : TransformFixture<BatchContentToAnyEnvelope>
	{
		[Fact]
		public void TransformBatchContent()
		{
			var setup = Given(
					input => input
						.Message(EnvelopeFactory.Create<Envelope>().AsStream())
						.Message(MessageBodyFactory.Create<Batch.Content>(MessageBody.Samples.LoadString("Message.BatchContent.xml")).AsStream()))
				.Transform
				.OutputsXml(
					output => output
						.ConformingTo<Envelope>()
						.ConformingTo<Batch.Release>()
						.WithStrictConformanceLevel());
			var result = setup.Validate();
			result.NamespaceManager.AddNamespace("env", SchemaMetadata.For<Envelope>().TargetNamespace);
			result.NamespaceManager.AddNamespace("tns", SchemaMetadata.For<Batch.Release>().TargetNamespace);

			result.SelectSingleNode("/*")!.LocalName.Should().Be("Envelope");
			result.Select("/env:Envelope/tns:ReleaseBatch").Cast<object>().Should().HaveCount(3);
			result.Select("/env:Envelope/*").Cast<object>().Should().HaveCount(3);
		}

		[Fact]
		public void TransformBatchContentWithEnvironmentTagAndPartition()
		{
			var setup = Given(
					input => input
						.Message(EnvelopeFactory.Create<Envelope>().AsStream())
						.Message(MessageBodyFactory.Create<Batch.Content>(MessageBody.Samples.LoadString("Message.BatchContentWithEnvironmentTagAndPartition.xml")).AsStream()))
				.Transform
				.OutputsXml(
					output => output
						.ConformingTo<Envelope>()
						.ConformingTo<Batch.Release>()
						.WithStrictConformanceLevel());
			var result = setup.Validate();
			result.NamespaceManager.AddNamespace("env", SchemaMetadata.For<Envelope>().TargetNamespace);
			result.NamespaceManager.AddNamespace("tns", SchemaMetadata.For<Batch.Release>().TargetNamespace);

			result.SelectSingleNode("/*")!.LocalName.Should().Be("Envelope");
			result.Select("/env:Envelope/tns:ReleaseBatch").Cast<object>().Should().HaveCount(3);
			result.Select("/env:Envelope/*").Cast<object>().Should().HaveCount(3);

			var part = result.SelectSingleNode("/env:Envelope/tns:ReleaseBatch[3]");
			part!.SelectSingleNode("tns:EnvelopeSpecName")!.Value.Should().Be(SchemaMetadata.For<Batch.Release>().DocumentSpec.DocSpecName);
			part!.SelectSingleNode("tns:EnvironmentTag")!.Value.Should().Be("graffiti");
			part!.SelectSingleNode("tns:Partition")!.Value.Should().Be("A");
		}
	}
}

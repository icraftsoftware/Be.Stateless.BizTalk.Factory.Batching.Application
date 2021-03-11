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

using System.Diagnostics.CodeAnalysis;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Message;
using Be.Stateless.BizTalk.Resources;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Schemas.Sql.Procedures.Batch;
using Be.Stateless.BizTalk.Schemas.Xml;
using Be.Stateless.BizTalk.Unit.Transform;
using Be.Stateless.Xml.Extensions;
using FluentAssertions;
using Microsoft.BizTalk.Message.Interop;
using Moq;
using Xunit;

namespace Be.Stateless.BizTalk.Maps.ToSql.Procedures.Batch
{
	public class ReleaseToQueueControlledReleaseFixture : TransformFixture<ReleaseToQueueControlledRelease>
	{
		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransform()
		{
			var instance = MessageBodyFactory.Create<Schemas.Xml.Batch.Release>(MessageBody.Samples.LoadString("Message.ReleaseBatch.xml"));
			using (var stream = instance.AsStream())
			{
				var setup = Given(input => input.Message(stream).Context(new Mock<IBaseMessageContext>().Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<QueueControlledRelease>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be(SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName);
				result.Select("//usp:partition").Should().BeEmpty();
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransformWithEnvironmentTag()
		{
			var instance = MessageBodyFactory.Create<Schemas.Xml.Batch.Release>(MessageBody.Samples.LoadString("Message.ReleaseBatchWithEnvironmentTag.xml"));
			using (var stream = instance.AsStream())
			{
				var setup = Given(input => input.Message(stream).Context(new Mock<IBaseMessageContext>().Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<QueueControlledRelease>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be(SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName);
				result.SelectSingleNode("//usp:environmentTag")!.Value.Should().Be("graffiti");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransformWithEnvironmentTagAndPartition()
		{
			var instance = MessageBodyFactory.Create<Schemas.Xml.Batch.Release>(MessageBody.Samples.LoadString("Message.ReleaseBatchWithEnvironmentTagAndPartition.xml"));
			using (var stream = instance.AsStream())
			{
				var setup = Given(input => input.Message(stream).Context(new Mock<IBaseMessageContext>().Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<QueueControlledRelease>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be(SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName);
				result.SelectSingleNode("//usp:environmentTag")!.Value.Should().Be("graffiti");
				result.SelectSingleNode("//usp:partition")!.Value.Should().Be("A");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransformWithPartition()
		{
			var instance = MessageBodyFactory.Create<Schemas.Xml.Batch.Release>(MessageBody.Samples.LoadString("Message.ReleaseBatchWithPartition.xml"));
			using (var stream = instance.AsStream())
			{
				var setup = Given(input => input.Message(stream).Context(new Mock<IBaseMessageContext>().Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<QueueControlledRelease>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be(SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName);
				result.SelectSingleNode("//usp:partition")!.Value.Should().Be("A");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransformWithProcessActivityId()
		{
			var contextMock = new Mock<IBaseMessageContext>();
			contextMock
				.Setup(c => c.Read(TrackingProperties.ProcessActivityId.Name, TrackingProperties.ProcessActivityId.Namespace))
				.Returns("D4D3A8E583024BAC9D35EC98C5422E82");

			var instance = MessageBodyFactory.Create<Schemas.Xml.Batch.Release>(MessageBody.Samples.LoadString("Message.ReleaseBatch.xml"));
			using (var stream = instance.AsStream())
			{
				var setup = Given(input => input.Message(stream).Context(contextMock.Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<QueueControlledRelease>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be(SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName);
				result.Select("//usp:partition").Should().BeEmpty();
				result.SelectSingleNode("//usp:processActivityId")!.Value.Should().Be("D4D3A8E583024BAC9D35EC98C5422E82");
			}
		}
	}
}

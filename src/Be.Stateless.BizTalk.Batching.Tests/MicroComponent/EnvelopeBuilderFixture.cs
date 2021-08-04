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
using System.IO;
using System.Text;
using System.Xml;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Dummies.Maps;
using Be.Stateless.BizTalk.Maps.ToXml;
using Be.Stateless.BizTalk.Message;
using Be.Stateless.BizTalk.Message.Extensions;
using Be.Stateless.BizTalk.Resources;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Schemas.Sql.Procedures.Batch;
using Be.Stateless.BizTalk.Schemas.Xml;
using Be.Stateless.BizTalk.Stream;
using Be.Stateless.BizTalk.Stream.Extensions;
using Be.Stateless.BizTalk.Unit.MicroComponent;
using Be.Stateless.BizTalk.Unit.Stream;
using Be.Stateless.IO;
using Be.Stateless.Reflection;
using Be.Stateless.Xml.Extensions;
using FluentAssertions;
using Microsoft.BizTalk.Streaming;
using Microsoft.XLANGs.BaseTypes;
using Moq;
using Xunit;
using static FluentAssertions.FluentActions;
using Any = Be.Stateless.BizTalk.Schemas.Xml.Any;

namespace Be.Stateless.BizTalk.MicroComponent
{
	public class EnvelopeBuilderFixture : MicroComponentFixture<EnvelopeBuilder>
	{
		[Fact]
		public void BatchContentIsTransformedToSpecificEnvelope()
		{
			string expectedEnvelopeContent;
			var envelopeStream = new StringStream(EnvelopeFactory.Create<Envelope>().OuterXml);
			var batchContentStream = MessageBodyFactory.Create<Batch.Content>(MessageBody.Samples.LoadString("Message.BatchContent.xml")).AsStream();
			var transformedStream = new[] { envelopeStream, batchContentStream }.Transform().Apply(typeof(BatchContentToAnyEnvelope), new UTF8Encoding(false));
			using (var expectedReader = XmlReader.Create(transformedStream, new() { CloseInput = true }))
			{
				expectedReader.Read();
				expectedEnvelopeContent = expectedReader.ReadOuterXml();
			}

			using (var stream = MessageBody.Samples.Load("Message.BatchContent.xml"))
			{
				MessageMock.Object.BodyPart.Data = stream;
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType(SchemaMetadata.For<Envelope>().MessageType))
					.Returns(SchemaMetadata.For<Envelope>().DocumentSpec);

				var sut = new EnvelopeBuilder();
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);
				using (var actualReader = XmlReader.Create(MessageMock.Object.BodyPart.Data, new() { CloseInput = true, IgnoreWhitespace = true }))
				{
					actualReader.Read();
					var actualEnvelopeContent = actualReader.ReadOuterXml();
					actualEnvelopeContent.Should().Be(expectedEnvelopeContent);
				}
			}
		}

		[Fact]
		public void EncodingDefaultsToUtf8WithoutSignature()
		{
			new EnvelopeBuilder().Encoding.Should().Be(new UTF8Encoding(false));
		}

		[Fact]
		public void FailsWhenMessageIsNotBatchContent()
		{
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("non xml payload")))
			{
				MessageMock.Object.BodyPart.Data = stream;

				var sut = new EnvelopeBuilder();

				Invoking(() => sut.Execute(PipelineContextMock.Object, MessageMock.Object))
					.Should().Throw<InvalidOperationException>()
					.WithMessage($"No EnvelopeSpecName has been found in {nameof(Batch.Content)} message and no envelope can be applied.");
			}
		}

		[Fact]
		public void MapDefaultsToBatchContentToAnyEnvelope()
		{
			new EnvelopeBuilder().MapType.Should().Be<BatchContentToAnyEnvelope>();
		}

		[Fact]
		public void MessageIsProbedForBatchDescriptor()
		{
			using (var stream = MessageBody.Samples.Load("Message.BatchContent.xml"))
			using (var probeBatchContentStreamMockInjectionScope = new ProbeBatchContentStreamMockInjectionScope())
			{
				MessageMock.Object.BodyPart.Data = stream;
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType(SchemaMetadata.For<Envelope>().MessageType))
					.Returns(SchemaMetadata.For<Envelope>().DocumentSpec);

				probeBatchContentStreamMockInjectionScope.Mock
					.Setup(ps => ps.BatchDescriptor)
					.Returns(new BatchDescriptor { EnvelopeSpecName = SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName })
					.Verifiable();

				var sut = new EnvelopeBuilder();
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				probeBatchContentStreamMockInjectionScope.Mock.VerifyAll();
			}
		}

		[Fact]
		public void MessageIsTransformedToEnvelope()
		{
			using (var stream = MessageBody.Samples.Load("Message.BatchContent.xml"))
			using (var transformedStream = new StringStream("<root xmlns='urn:ns'></root>"))
			using (var transformStreamMockInjectionScope = new TransformStreamMockInjectionScope())
			{
				MessageMock.Object.BodyPart.Data = stream;
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType("urn:ns#root"))
					.Returns(SchemaMetadata.For<Any>().DocumentSpec);

				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.ExtendWith(MessageMock.Object.Context))
					.Returns(transformStreamMockInjectionScope.Mock.Object)
					.Verifiable();
				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.Apply(typeof(BatchContentToAnyEnvelope), new UTF8Encoding(false)))
					.Returns(transformedStream)
					.Verifiable();

				var sut = new EnvelopeBuilder();
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				transformStreamMockInjectionScope.Mock.VerifyAll();
			}
		}

		[Fact]
		public void MessageIsTransformedToEnvelopeViaEnvelopeSpecNameAnnotation()
		{
			using (var stream = MessageBody.Samples.Load("Message.BatchContentWithEnvelopeSpecName.xml"))
			using (var transformedStream = new StringStream("<root xmlns='urn:ns'></root>"))
			using (var transformStreamMockInjectionScope = new TransformStreamMockInjectionScope())
			{
				MessageMock.Object.BodyPart.Data = stream;
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType("urn:ns#root"))
					.Returns(SchemaMetadata.For<Any>().DocumentSpec);

				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.ExtendWith(MessageMock.Object.Context))
					.Returns(transformStreamMockInjectionScope.Mock.Object);
				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.Apply(typeof(IdentityTransform), new UTF8Encoding(false)))
					.Returns(transformedStream)
					.Verifiable();

				var sut = new EnvelopeBuilder();
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				transformStreamMockInjectionScope.Mock.VerifyAll();
			}
		}

		[Fact]
		public void PartitionIsPromoted()
		{
			using (var stream = MessageBody.Samples.Load("Message.BatchContentWithPartition.xml"))
			{
				MessageMock.Object.BodyPart.Data = stream;
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType(SchemaMetadata.For<Batch.Content>().MessageType))
					.Returns(SchemaMetadata.For<Batch.Content>().DocumentSpec);
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType(SchemaMetadata.For<Envelope>().MessageType))
					.Returns(SchemaMetadata.For<Envelope>().DocumentSpec);

				var sut = new EnvelopeBuilder();
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				MessageMock.Verify(m => m.Promote(BatchProperties.EnvelopePartition, "p-one"));
			}
		}

		[Fact]
		public void ReplacesMessageOriginalDataStreamWithTransformResult()
		{
			using (var stream = new StringStream("<root xmlns='urn:ns'></root>"))
			using (var transformedStream = stream.Transform().Apply(typeof(IdentityTransform)))
			using (var probeBatchContentStreamMockInjectionScope = new ProbeBatchContentStreamMockInjectionScope())
			using (var transformStreamMockInjectionScope = new TransformStreamMockInjectionScope())
			{
				MessageMock.Object.BodyPart.Data = stream;
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType("urn:ns#root"))
					.Returns(SchemaMetadata.For<Any>().DocumentSpec);

				probeBatchContentStreamMockInjectionScope.Mock
					.Setup(ps => ps.BatchDescriptor)
					.Returns(new BatchDescriptor { EnvelopeSpecName = SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName });

				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.ExtendWith(MessageMock.Object.Context))
					.Returns(transformStreamMockInjectionScope.Mock.Object);
				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.Apply(typeof(IdentityTransform), Encoding.UTF8))
					.Returns(transformedStream)
					.Verifiable();

				var sut = new EnvelopeBuilder { Encoding = Encoding.UTF8, MapType = typeof(IdentityTransform) };
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				transformStreamMockInjectionScope.Mock.VerifyAll();

				Reflector.GetField(MessageMock.Object.BodyPart.Data.AsMarkable(), "m_data").Should().BeSameAs(transformedStream);
				MessageMock.Object.BodyPart.Data.Should().BeOfType<MarkableForwardOnlyEventingReadStream>();
			}
		}

		[Fact]
		public void XsltEntailsMessageTypeIsPromoted()
		{
			using (var stream = new StringStream("<root xmlns='urn:ns'></root>"))
			using (var transformedStream = stream.Transform().Apply(typeof(IdentityTransform)))
			using (var probeStreamMockInjectionScope = new ProbeStreamMockInjectionScope())
			using (var probeBatchContentStreamMockInjectionScope = new ProbeBatchContentStreamMockInjectionScope())
			using (var transformStreamMockInjectionScope = new TransformStreamMockInjectionScope())
			{
				PipelineContextMock
					.Setup(m => m.GetDocumentSpecByType(SchemaMetadata.For<Envelope>().MessageType))
					.Returns(SchemaMetadata.For<AddPart>().DocumentSpec);

				MessageMock.Object.BodyPart.Data = stream;

				probeStreamMockInjectionScope.Mock
					.Setup(ps => ps.MessageType)
					.Returns(SchemaMetadata.For<Envelope>().MessageType);
				probeBatchContentStreamMockInjectionScope.Mock
					.Setup(ps => ps.BatchDescriptor)
					.Returns(new BatchDescriptor { EnvelopeSpecName = SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName });

				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.ExtendWith(MessageMock.Object.Context))
					.Returns(transformStreamMockInjectionScope.Mock.Object);
				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.Apply(typeof(IdentityTransform), Encoding.UTF8))
					.Returns(transformedStream);

				var sut = new EnvelopeBuilder { Encoding = Encoding.UTF8, MapType = typeof(IdentityTransform) };
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				MessageMock.Verify(m => m.Promote(BtsProperties.MessageType, SchemaMetadata.For<AddPart>().MessageType), Times.Once);
				MessageMock.Verify(m => m.Promote(BtsProperties.MessageType, It.IsAny<string>()), Times.Once);
				MessageMock.Verify(m => m.Promote(BtsProperties.SchemaStrongName, SchemaMetadata.For<AddPart>().DocumentSpec.DocSpecStrongName), Times.Once);
				MessageMock.Verify(m => m.Promote(BtsProperties.SchemaStrongName, It.IsAny<string>()), Times.Once);
			}
		}

		[Fact]
		public void XsltEntailsMessageTypeIsPromotedOnlyIfOutputMethodIsXml()
		{
			using (var dataStream = new StringStream("<root xmlns='urn:ns'></root>"))
			using (var stream = new MarkableForwardOnlyEventingReadStream(dataStream))
			using (var transformedStream = dataStream.Transform().Apply(typeof(IdentityTransform)))
			using (var probeBatchContentStreamMockInjectionScope = new ProbeBatchContentStreamMockInjectionScope())
			using (var transformStreamMockInjectionScope = new TransformStreamMockInjectionScope())
			{
				PipelineContextMock
					.Setup(m => m.GetDocumentSpecByType("urn:ns#root"))
					.Returns(SchemaMetadata.For<Batch.Content>().DocumentSpec);

				MessageMock.Object.BodyPart.Data = stream;

				probeBatchContentStreamMockInjectionScope.Mock
					.Setup(ps => ps.BatchDescriptor)
					.Returns(new BatchDescriptor { EnvelopeSpecName = SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName });

				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.ExtendWith(MessageMock.Object.Context))
					.Returns(transformStreamMockInjectionScope.Mock.Object);
				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.Apply(typeof(AnyToText), Encoding.UTF8))
					.Returns(transformedStream);

				var sut = new EnvelopeBuilder { Encoding = Encoding.UTF8, MapType = typeof(AnyToText) };
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				MessageMock.Verify(m => m.Promote(BtsProperties.MessageType, It.IsAny<string>()), Times.Never);
				MessageMock.Verify(m => m.Promote(BtsProperties.SchemaStrongName, It.IsAny<string>()), Times.Never);
			}
		}

		[Fact]
		public void XsltFromContextHasPrecedenceOverConfiguredOne()
		{
			using (var stream = new StringStream("<root xmlns='urn:ns'></root>"))
			using (var transformedStream = stream.Transform().Apply(typeof(IdentityTransform)))
			using (var transformStreamMockInjectionScope = new TransformStreamMockInjectionScope())
			using (var probeBatchContentStreamMockInjectionScope = new ProbeBatchContentStreamMockInjectionScope())
			{
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType("urn:ns#root"))
					.Returns(SchemaMetadata.For<Any>().DocumentSpec);

				MessageMock.Object.BodyPart.Data = stream;
				MessageMock
					.Setup(m => m.GetProperty(BizTalkFactoryProperties.MapTypeName))
					.Returns(typeof(IdentityTransform).AssemblyQualifiedName);

				probeBatchContentStreamMockInjectionScope.Mock
					.Setup(ps => ps.BatchDescriptor)
					.Returns(new BatchDescriptor { EnvelopeSpecName = SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName });

				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.ExtendWith(MessageMock.Object.Context))
					.Returns(transformStreamMockInjectionScope.Mock.Object);
				transformStreamMockInjectionScope.Mock
					.Setup(ts => ts.Apply(typeof(IdentityTransform), Encoding.UTF8))
					.Returns(transformedStream)
					.Verifiable();

				var sut = new EnvelopeBuilder { Encoding = Encoding.UTF8, MapType = typeof(TransformBase) };
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				transformStreamMockInjectionScope.Mock.Verify(ts => ts.Apply(sut.MapType, sut.Encoding), Times.Never);
				transformStreamMockInjectionScope.Mock.VerifyAll();
			}
		}
	}
}

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
using Be.Stateless.BizTalk.Activity.Tracking;
using Be.Stateless.BizTalk.Activity.Tracking.Messaging;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Message.Extensions;
using Be.Stateless.BizTalk.Resources;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Schemas.Xml;
using Be.Stateless.BizTalk.Unit.MicroComponent;
using Be.Stateless.IO;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Moq;
using Xunit;

namespace Be.Stateless.BizTalk.MicroComponent
{
	public class BatchContentReleaseActivityTrackerFixture : MicroComponentFixture<BatchContentReleaseActivityTracker>, IDisposable
	{
		#region Setup/Teardown

		public BatchContentReleaseActivityTrackerFixture()
		{
			_batchReleaseProcessActivityTrackerFactory = BatchReleaseProcessActivityTracker.Factory;
			BatchReleaseProcessActivityTrackerMock = new Mock<BatchReleaseProcessActivityTracker>(PipelineContextMock.Object, MessageMock.Object);
			BatchReleaseProcessActivityTracker.Factory = (_, _) => BatchReleaseProcessActivityTrackerMock.Object;
		}

		public void Dispose()
		{
			BatchReleaseProcessActivityTracker.Factory = _batchReleaseProcessActivityTrackerFactory;
		}

		#endregion

		[Fact]
		public void DoesNothingWhenNotBatchContent()
		{
			using (var stream = new StringStream("<root xmlns='ns'></root>"))
			{
				MessageMock.Object.BodyPart.Data = stream;
				MessageMock.Setup(m => m.GetProperty(BtsProperties.InboundTransportLocation)).Returns("inbound-transport-location");

				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType(SchemaMetadata.For<Batch.Content>().MessageType))
					.Returns(SchemaMetadata.For<Batch.Content>().DocumentSpec);
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType(SchemaMetadata.For<Envelope>().MessageType))
					.Returns(SchemaMetadata.For<Envelope>().DocumentSpec);

				var sut = new BatchContentReleaseActivityTracker();
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				BatchReleaseProcessActivityTrackerMock.Verify(tracker => tracker.TrackActivity(It.IsAny<BatchTrackingContext>()), Times.Never);
			}
		}

		[Fact]
		public void TrackBatchContent()
		{
			using (var stream = MessageBody.Samples.Load("Message.BatchContentWithEnvironmentTagAndPartition.xml"))
			{
				MessageMock.Object.BodyPart.Data = stream;
				MessageMock.Setup(m => m.GetProperty(BtsProperties.InboundTransportLocation)).Returns("inbound-transport-location");

				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType(SchemaMetadata.For<Batch.Content>().MessageType))
					.Returns(SchemaMetadata.For<Batch.Content>().DocumentSpec);
				PipelineContextMock
					.Setup(pc => pc.GetDocumentSpecByType(SchemaMetadata.For<Envelope>().MessageType))
					.Returns(SchemaMetadata.For<Envelope>().DocumentSpec);

				var sut = new BatchContentReleaseActivityTracker();
				sut.Execute(PipelineContextMock.Object, MessageMock.Object);

				BatchReleaseProcessActivityTrackerMock.Verify(tracker => tracker.TrackActivity(It.IsAny<BatchTrackingContext>()), Times.Once);

				MessageMock.Verify(m => m.SetProperty(TrackingProperties.Value1, SchemaMetadata.For<Envelope>().DocumentSpec.DocSpecStrongName));
				MessageMock.Verify(m => m.SetProperty(TrackingProperties.Value2, "graffiti"));
				MessageMock.Verify(m => m.SetProperty(TrackingProperties.Value3, "A"));
			}
		}

		private Mock<BatchReleaseProcessActivityTracker> BatchReleaseProcessActivityTrackerMock { get; }

		private readonly Func<IPipelineContext, IBaseMessage, BatchReleaseProcessActivityTracker> _batchReleaseProcessActivityTrackerFactory;
	}
}

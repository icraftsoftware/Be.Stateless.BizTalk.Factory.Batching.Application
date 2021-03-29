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

using Be.Stateless.BizTalk.Unit;
using FluentAssertions;
using Microsoft.BizTalk.Bam.EventObservation;
using Microsoft.BizTalk.Component.Interop;
using Moq;
using Xunit;

namespace Be.Stateless.BizTalk.Activity.Tracking.Messaging
{
	public class BatchReleaseProcessActivityFactoryFixture
	{
		#region Setup/Teardown

		public BatchReleaseProcessActivityFactoryFixture()
		{
			PipelineContextMock = new Mock<IPipelineContext>();
			PipelineContextMock.Setup(pc => pc.GetEventStream()).Returns(new Mock<EventStream>().Object);
		}

		#endregion

		[Fact]
		public void CreateProcessReturnsRegularBatchReleaseProcess()
		{
			var messageMock = new MessageMock();
			var factory = (IBatchReleaseProcessActivityFactory) new BatchReleaseProcessActivityFactory(PipelineContextMock.Object);
			factory.CreateProcess(messageMock.Object, "name").Should().BeOfType<BatchReleaseProcess>();
		}

		[Fact]
		public void FindProcessReturnsBatchReleaseProcessReference()
		{
			var factory = (IBatchReleaseProcessActivityFactory) new BatchReleaseProcessActivityFactory(PipelineContextMock.Object);
			factory.FindProcess("pseudo-activity-id").Should().BeOfType<BatchReleaseProcessReference>();
		}

		private Mock<IPipelineContext> PipelineContextMock { get; }
	}
}

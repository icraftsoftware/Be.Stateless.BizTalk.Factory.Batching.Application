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
using System.Linq;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Message.Extensions;
using Be.Stateless.BizTalk.Schemas.Sql.Procedures.Batch;
using Be.Stateless.BizTalk.Unit;
using Be.Stateless.BizTalk.Unit.Transform;
using Be.Stateless.IO;
using FluentAssertions;
using Xunit;
using BizTalkFactoryProperties = Be.Stateless.BizTalk.ContextProperties.Subscribable.BizTalkFactoryProperties;

namespace Be.Stateless.BizTalk.Maps.ToSql.Procedures.Batch
{
	public class AnyToAddPartFixture : TransformFixture<AnyToAddPart>
	{
		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransform()
		{
			var contextMock = new MessageContextMock();
			contextMock
				.Setup(c => c.GetProperty(BatchProperties.EnvelopeSpecName))
				.Returns("envelope-name");

			using (var stream = new StringStream("<?xml version=\"1.0\" encoding=\"utf-16\" ?><root>content of a part is irrelevant here</root>"))
			{
				var setup = Given(input => input.Message(stream).Context(contextMock.Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<AddPart>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be("envelope-name");
				result.Select("//usp:partition").Cast<object>().Should().BeEmpty();
				result.Select("//usp:messagingStepActivityId").Cast<object>().Should().BeEmpty();
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransformWithEnvironmentTag()
		{
			var contextMock = new MessageContextMock();
			contextMock
				.Setup(c => c.GetProperty(BatchProperties.EnvelopeSpecName))
				.Returns("envelope-name");
			contextMock
				.Setup(c => c.GetProperty(BizTalkFactoryProperties.EnvironmentTag))
				.Returns("Tag");
			contextMock
				.Setup(c => c.GetProperty(TrackingProperties.MessagingStepActivityId))
				.Returns("D4D3A8E583024BAC9D35EC98C5422E82");

			using (var stream = new StringStream("<?xml version=\"1.0\" encoding=\"utf-16\" ?><root>content of a part is irrelevant here</root>"))
			{
				var setup = Given(input => input.Message(stream).Context(contextMock.Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<AddPart>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be("envelope-name");
				result.SelectSingleNode("//usp:environmentTag")!.Value.Should().Be("Tag");
				result.SelectSingleNode("//usp:messagingStepActivityId")!.Value.Should().Be("D4D3A8E583024BAC9D35EC98C5422E82");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransformWithEnvironmentTagAndPartition()
		{
			var contextMock = new MessageContextMock();
			contextMock
				.Setup(c => c.GetProperty(BatchProperties.EnvelopeSpecName))
				.Returns("envelope-name");
			contextMock
				.Setup(c => c.GetProperty(BizTalkFactoryProperties.EnvironmentTag))
				.Returns("Tag");
			contextMock
				.Setup(c => c.GetProperty(BatchProperties.EnvelopePartition))
				.Returns("A");
			contextMock
				.Setup(c => c.GetProperty(TrackingProperties.MessagingStepActivityId))
				.Returns("D4D3A8E583024BAC9D35EC98C5422E82");

			using (var stream = new StringStream("<?xml version=\"1.0\" encoding=\"utf-16\" ?><root>content of a part is irrelevant here</root>"))
			{
				var setup = Given(input => input.Message(stream).Context(contextMock.Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<AddPart>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be("envelope-name");
				result.SelectSingleNode("//usp:environmentTag")!.Value.Should().Be("Tag");
				result.SelectSingleNode("//usp:partition")!.Value.Should().Be("A");
				result.SelectSingleNode("//usp:messagingStepActivityId")!.Value.Should().Be("D4D3A8E583024BAC9D35EC98C5422E82");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransformWithMessagingStepActivityId()
		{
			var contextMock = new MessageContextMock();
			contextMock
				.Setup(c => c.GetProperty(BatchProperties.EnvelopeSpecName))
				.Returns("envelope-name");
			contextMock
				.Setup(c => c.GetProperty(TrackingProperties.MessagingStepActivityId))
				.Returns("D4D3A8E583024BAC9D35EC98C5422E82");

			using (var stream = new StringStream("<?xml version=\"1.0\" encoding=\"utf-16\" ?><root>content of a part is irrelevant here</root>"))
			{
				var setup = Given(input => input.Message(stream).Context(contextMock.Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<AddPart>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be("envelope-name");
				result.Select("//usp:partition").Cast<object>().Should().BeEmpty();
				result.SelectSingleNode("//usp:messagingStepActivityId")!.Value.Should().Be("D4D3A8E583024BAC9D35EC98C5422E82");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidateTransformWithPartition()
		{
			var contextMock = new MessageContextMock();
			contextMock
				.Setup(c => c.GetProperty(BatchProperties.EnvelopeSpecName))
				.Returns("envelope-name");
			contextMock
				.Setup(c => c.GetProperty(BatchProperties.EnvelopePartition))
				.Returns("A");
			contextMock
				.Setup(c => c.GetProperty(TrackingProperties.MessagingStepActivityId))
				.Returns("D4D3A8E583024BAC9D35EC98C5422E82");

			using (var stream = new StringStream("<?xml version=\"1.0\" encoding=\"utf-16\" ?><root>content of a part is irrelevant here</root>"))
			{
				var setup = Given(input => input.Message(stream).Context(contextMock.Object))
					.Transform
					.OutputsXml(output => output.ConformingTo<AddPart>().WithStrictConformanceLevel());
				var result = setup.Validate();
				result.SelectSingleNode("//usp:envelopeSpecName")!.Value.Should().Be("envelope-name");
				result.SelectSingleNode("//usp:partition")!.Value.Should().Be("A");
				result.SelectSingleNode("//usp:messagingStepActivityId")!.Value.Should().Be("D4D3A8E583024BAC9D35EC98C5422E82");
			}
		}
	}
}

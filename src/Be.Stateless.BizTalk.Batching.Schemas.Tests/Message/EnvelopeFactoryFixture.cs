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
using Be.Stateless.BizTalk.Schemas.Xml;
using FluentAssertions;
using Microsoft.BizTalk.Edi.BaseArtifacts;
using Microsoft.XLANGs.BaseTypes;
using Xunit;
using static FluentAssertions.FluentActions;
using Any = Be.Stateless.BizTalk.Schemas.Xml.Any;

namespace Be.Stateless.BizTalk.Message
{
	public class EnvelopeFactoryFixture
	{
		[Fact]
		public void CreateDoesNotThrowForEnvelopeSchema()
		{
			Invoking(EnvelopeFactory.Create<Envelope>).Should().NotThrow();
		}

		[Fact]
		public void CreateThrowsForNonEnvelopeSchema()
		{
			Invoking(EnvelopeFactory.Create<Any>)
				.Should().Throw<ArgumentException>()
				.WithMessage($"Either {typeof(Any).FullName} is not an envelope schema or does not derive from {typeof(SchemaBase).FullName}.*");
		}

		[Fact]
		public void EnvelopeTemplateContainsPartsPlaceHolder()
		{
			EnvelopeFactory.Create<ResendControlEnvelope>().OuterXml.Should().Be(
				"<ns0:ControlMessage xmlns:ns0=\"http://schemas.microsoft.com/BizTalk/2006/reliability-properties\">" +
				"<ns:parts-here xmlns:ns=\"urn:schemas.stateless.be:biztalk:batch:2012:12\" />" +
				"</ns0:ControlMessage>");

			EnvelopeFactory.Create<Envelope>().OuterXml.Should().Be(
				"<ns0:Envelope xmlns:ns0=\"urn:schemas.stateless.be:biztalk:envelope:2013:07\">" +
				"<ns:parts-here xmlns:ns=\"urn:schemas.stateless.be:biztalk:batch:2012:12\" />" +
				"</ns0:Envelope>");
		}
	}
}

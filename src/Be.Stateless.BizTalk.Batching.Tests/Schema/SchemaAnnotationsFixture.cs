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
using Be.Stateless.BizTalk.Dummies.Maps;
using Be.Stateless.BizTalk.Dummies.Schemas;
using Be.Stateless.BizTalk.Schema.Annotation;
using Be.Stateless.BizTalk.Schema.Extensions;
using Be.Stateless.BizTalk.Schemas.Xml;
using FluentAssertions;
using Xunit;
using BizTalkFactoryProperties = Be.Stateless.BizTalk.ContextProperties.Subscribable.BizTalkFactoryProperties;

namespace Be.Stateless.BizTalk.Schema
{
	public class SchemaAnnotationsFixture
	{
		[SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
		[Fact]
		public void ResolveEnvelopeMapAnnotationFromEnvelopeSchemaWithAnnotations()
		{
			var schemaMetadata = SchemaMetadata.For<AnnotatedSchema>();
			schemaMetadata.GetEnvelopeMap().Should().Be<IdentityTransform>();
		}

		[SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
		[Fact]
		public void ResolveEnvelopeMapAnnotationFromEnvelopeSchemaWithoutAnnotations()
		{
			var schemaMetadata = SchemaMetadata.For<Batch.Content>();
			schemaMetadata.GetEnvelopeMap().Should().BeNull();
			schemaMetadata.GetPropertyExtractors().Should().BeEquivalentTo(
				new XPathExtractor(BizTalkFactoryProperties.EnvironmentTag.QName, "/*/*[local-name()='EnvironmentTag']", ExtractionMode.Promote),
				new XPathExtractor(TrackingProperties.Value1.QName, "/*/*[local-name()='EnvelopeSpecName']", ExtractionMode.Write),
				new XPathExtractor(TrackingProperties.Value2.QName, "/*/*[local-name()='EnvironmentTag']", ExtractionMode.Write),
				new XPathExtractor(TrackingProperties.Value3.QName, "/*/*[local-name()='Partition']", ExtractionMode.Write));
		}

		[Fact]
		public void ResolveEnvelopeMapAnnotationFromRegularSchemaWithoutAnnotation()
		{
			var schemaMetadata = SchemaMetadata.For<UnannotatedSchema>();
			schemaMetadata.GetEnvelopeMap().Should().BeNull();
		}
	}
}

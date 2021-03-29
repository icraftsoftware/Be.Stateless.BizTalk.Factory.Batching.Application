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
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Dummies.Schemas
{
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
	[BodyXPath(@"/*[local-name()='Annotated' and namespace-uri()='urn:schemas.stateless.be:biztalk:tests:annotated:2013:01']")]
	[Schema(@"urn:schemas.stateless.be:biztalk:tests:annotated:2013:01", @"Annotated")]
	[SchemaRoots(new[] { @"Annotated" })]
	internal class AnnotatedSchema : SchemaBase
	{
		#region Base Class Member Overrides

		protected override object RawSchema { get; set; }

		public override string XmlContent => XML_CONTENT;

		#endregion

		[SuppressMessage("ReSharper", "StringLiteralTypo")]
		private const string XML_CONTENT =
			@"<xs:schema targetNamespace='urn:schemas.stateless.be:biztalk:tests:annotated:2013:01' xmlns:san='urn:schemas.stateless.be:biztalk:annotations:2013:01' xmlns:xs='http://www.w3.org/2001/XMLSchema'>
  <xs:element name='Annotated'>
    <xs:annotation>
      <xs:appinfo>
        <san:EnvelopeMapSpecName>Be.Stateless.BizTalk.Dummies.Maps.IdentityTransform, Be.Stateless.BizTalk.Batching.Tests, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3707daa0b119fc14</san:EnvelopeMapSpecName>
        <san:Properties xmlns:s0='urn:schemas.stateless.be:biztalk:properties:tracking:2012:04'>
          <s0:Value1 xpath=""/*[local-name()='Send']/*[local-name()='Message']/*[local-name()='Id']"" />
        </san:Properties>
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType />
  </xs:element>
</xs:schema>";
	}
}

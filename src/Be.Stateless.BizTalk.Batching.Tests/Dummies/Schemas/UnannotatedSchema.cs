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
	[Schema(@"urn:schemas.stateless.be:biztalk:tests:unannotated:2013:01", @"Unannotated")]
	[SchemaRoots(new[] { @"Unannotated" })]
	internal class UnannotatedSchema : SchemaBase
	{
		#region Base Class Member Overrides

		protected override object RawSchema { get; set; }

		public override string XmlContent => XML_CONTENT;

		#endregion

		private const string XML_CONTENT = @"<xs:schema targetNamespace='urn:schemas.stateless.be:biztalk:tests:unannotated:2013:01' xmlns:xs='http://www.w3.org/2001/XMLSchema'>
  <xs:element name='Unannotated'>
    <xs:complexType />
  </xs:element>
</xs:schema>";
	}
}

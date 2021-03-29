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
using Be.Stateless.Extensions;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Schema
{
	/// <summary>
	/// Provides access to annotations embedded in <see cref="SchemaBase"/>-derived schema definitions.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Only annotations declared in the schema annotation namespace defined by BizTalk Factory, i.e.
	/// <c>urn:schemas.stateless.be:biztalk:annotations:2013:01</c> are considered; all other annotations are discarded.
	/// </para>
	/// <para>
	/// Notice that there is no transitive discovery of the annotations across the XSD type definitions and that only
	/// annotations embedded directly underneath the root node of the relevant <see cref="SchemaBase"/>-derived schema are
	/// loaded.
	/// </para>
	/// </remarks>
	/// <example>
	/// The following example illustrates how to embed BizTalk Factory annotations. Note that there other annotations specific
	/// to Microsoft BizTalk Server might coexist but are not illustrated.
	/// <code>
	/// <![CDATA[
	/// <xs:schema targetNamespace='urn:schemas.stateless.be:biztalk:tests:annotated:2013:01'
	///            xmlns:san='urn:schemas.stateless.be:biztalk:annotations:2013:01'
	///            xmlns:xs='http://www.w3.org/2001/XMLSchema'>
	///   <xs:element name='Root'>
	///     <xs:annotation>
	///       <xs:appinfo>
	///         ...
	///         <san:EnvelopeMapSpecName>
	///           Be.Stateless.BizTalk.Maps.ToXml.BatchContentToAnyEnvelope, Be.Stateless.BizTalk.Batching.Maps, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3707daa0b119fc14
	///         </san:EnvelopeMapSpecName>
	///         ...
	///       </xs:appinfo>
	///     </xs:annotation>
	///     <xs:complexType />
	///   </xs:element>
	/// </xs:schema>
	/// ]]>
	/// </code>
	/// </example>
	public class EnvelopeMapAnnotation : ISchemaAnnotation<EnvelopeMapAnnotation>
	{
		#region ISchemaAnnotation<EnvelopeMapAnnotation> Members

		public EnvelopeMapAnnotation Build(ISchemaAnnotationReader schemaAnnotationReader)
		{
			if (schemaAnnotationReader == null) throw new ArgumentNullException(nameof(schemaAnnotationReader));
			EnvelopeMapType = schemaAnnotationReader.SchemaMetadata.IsEnvelopeSchema
				? schemaAnnotationReader.GetAnnotationElement("EnvelopeMapSpecName")?.Value.IfNotNull(n => Type.GetType(n, true))
				: null;
			return this;
		}

		#endregion

		/// <summary>
		/// The <see cref="Type"/> of the <see cref="TransformBase"/>-derived XSLT transform that has to be applied when
		/// transforming from a <see cref="Batch.Content"/> payload to the <see cref="SchemaBase"/>-derived envelope schema which
		/// embeds this annotation.
		/// </summary>
		public Type EnvelopeMapType { get; private set; }
	}
}

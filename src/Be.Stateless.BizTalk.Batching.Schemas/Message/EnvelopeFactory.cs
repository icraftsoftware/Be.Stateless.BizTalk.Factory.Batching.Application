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
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Schemas.Xml;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Message
{
	public static class EnvelopeFactory
	{
		/// <summary>
		/// Creates an envelope template document for a given <see cref="SchemaBase"/>-derived envelope schema type <typeparamref
		/// name="T"/>.
		/// </summary>
		/// <typeparam name="T">
		/// The <see cref="SchemaBase"/>-derived envelope schema type.
		/// </typeparam>
		/// <returns>
		/// The envelope template document as an <see cref="XmlDocument"/>.
		/// </returns>
		/// <remarks>
		/// Notice the that the <c>&lt;ns:parts-here xmlns:ns="urn:schemas.stateless.be:biztalk:batch:2012:12" /&gt;</c> XML
		/// placeholder element will be inserted where the content of the envelope's body must be located; this provides an easy
		/// way for XSLT maps to latter insert the envelope's body content by overriding this placeholder.
		/// </remarks>
		public static XmlDocument Create<T>() where T : SchemaBase, new()
		{
			return Create(typeof(T));
		}

		/// <summary>
		/// Creates an envelope template document for a given <see cref="SchemaBase"/>-derived envelope schema type.
		/// </summary>
		/// <param name="schema">
		/// The <see cref="SchemaBase"/>-derived envelope schema type.
		/// </param>
		/// <returns>
		/// The envelope template document as an <see cref="XmlDocument"/>.
		/// </returns>
		/// <remarks>
		/// Notice the that the <c>&lt;ns:parts-here xmlns:ns="urn:schemas.stateless.be:biztalk:batch:2012:12" /&gt;</c> XML
		/// placeholder element will be inserted where the content of the envelope's body must be located; this provides an easy
		/// way for XSLT maps to latter insert the envelope's body content by overriding this placeholder.
		/// </remarks>
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
		public static XmlDocument Create(Type schema)
		{
			if (!SchemaMetadata.For(schema).IsEnvelopeSchema)
				throw new ArgumentException(
					$"Either {schema.FullName} is not an envelope schema or does not derive from {typeof(SchemaBase).FullName}.",
					nameof(schema));

			var envelope = MessageBodyFactory.Create(schema);
			var xpath = SchemaMetadata.For(schema).BodyXPath;
			var body = envelope.SelectSingleNode(xpath);
			if (body == null) throw new InvalidOperationException($"Body element cannot be found for envelope schema '{schema.FullName}'.");
			// overwrite the whole body's dummy/default content with the parts' placeholder
			body.InnerXml = $"<ns:parts-here xmlns:ns=\"{SchemaMetadata.For<Batch.Content>().TargetNamespace}\" />";
			return envelope;
		}
	}
}

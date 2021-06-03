﻿#region Copyright & License

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

using System.Text;
using System.Xml;
using Be.Stateless.BizTalk.Message;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Stream;
using Be.Stateless.Extensions;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Unit.Message
{
	public class BatchFactory
	{
		public static XmlDocument CreateReleaseFor<TSchema>(string partition = null)
			where TSchema : SchemaBase
		{
			var builder = new StringBuilder();
			using (var writer = XmlWriter.Create(builder, new XmlWriterSettings { Indent = false, OmitXmlDeclaration = true, Encoding = Encoding.UTF8 }))
			{
				const string prefix = "ns";
				var schemaMetadata = SchemaMetadata.For<Schemas.Xml.Batch.Release>();
				var ns = schemaMetadata.TargetNamespace;
				writer.WriteStartElement(prefix, schemaMetadata.RootElementName, ns);
				{
					writer.WriteElementString(prefix, nameof(BatchDescriptor.EnvelopeSpecName), ns, SchemaMetadata.For<TSchema>().DocumentSpec.DocSpecStrongName);
					if (!partition.IsNullOrWhiteSpace()) writer.WriteElementString(prefix, nameof(BatchDescriptor.Partition), ns, partition);
				}
				writer.WriteEndElement();
			}
			return MessageBodyFactory.Create<Schemas.Xml.Batch.Release>(builder.ToString());
		}
	}
}

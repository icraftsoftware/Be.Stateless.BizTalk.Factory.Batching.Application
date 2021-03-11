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
using System.Xml;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Schemas.Xml;
using Be.Stateless.BizTalk.Tracking;
using Be.Stateless.Extensions;
using Be.Stateless.Xml.Extensions;

namespace Be.Stateless.BizTalk.Stream.Extensions
{
	/// <summary>
	/// Provides streams with probing extensions.
	/// </summary>
	/// <seealso cref="StreamExtensions.Probe"/>
	internal class BatchContentProber : IProbeBatchContentStream
	{
		public BatchContentProber(IProbeStream probe)
		{
			_probe = probe;
		}

		#region IProbeBatchContentStream Members

		[SuppressMessage("ReSharper", "InvertIf")]
		public BatchDescriptor BatchDescriptor
		{
			get
			{
				try
				{
					_probe.Stream.MarkPosition();
					var batchDescriptor = new BatchDescriptor();
					using (var xmlReader = XmlReader.Create(_probe.Stream))
					{
						var schemaMetadata = SchemaMetadata.For<Batch.Content>();
						// Ensure to read <BatchContent> root element at the right place and in the right namespace.
						// ReadStartElement() calls IsStartElement() and Read() in sequence; IsStartElement() calls
						// MoveToContent() and tests if the current content node's LocalName and NamespaceURI match
						// the given arguments.
						xmlReader.ReadStartElement(schemaMetadata.RootElementName, schemaMetadata.TargetNamespace);
						// Ensure to read <EnvelopeSpecName> element at the right place and in the right namespace.
						// ReadElementString() calls MoveToContent() to find the next content node and then parses its
						// value as a simple string if the element LocalName and NamespaceURI match the given arguments.
						batchDescriptor.EnvelopeSpecName = xmlReader.ReadElementString(BatchProperties.EnvelopeSpecName.Name, schemaMetadata.TargetNamespace);
						if (xmlReader.IsStartElement("EnvironmentTag", schemaMetadata.TargetNamespace))
						{
							batchDescriptor.EnvironmentTag = xmlReader.ReadString();
							xmlReader.ReadEndElement("EnvironmentTag", schemaMetadata.TargetNamespace);
						}
						if (xmlReader.IsStartElement("Partition", schemaMetadata.TargetNamespace))
						{
							batchDescriptor.Partition = xmlReader.ReadString();
							xmlReader.ReadEndElement("Partition", schemaMetadata.TargetNamespace);
						}
						return batchDescriptor;
					}
				}
				catch (XmlException)
				{
					return null;
				}
				finally
				{
					_probe.Stream.ResetPosition();
				}
			}
		}

		BatchTrackingContext IProbeBatchContentStream.BatchTrackingContext
		{
			get
			{
				try
				{
					_probe.Stream.MarkPosition();
					var batchTrackingContext = new BatchTrackingContext();
					using (var xmlReader = XmlReader.Create(_probe.Stream))
					{
						var schemaMetadata = SchemaMetadata.For<Batch.Content>();
						// Ensure to read <BatchContent> root element at the right place and in the right namespace.
						// ReadStartElement() calls IsStartElement() and Read() in sequence; IsStartElement() calls
						// MoveToContent() and tests if the current content node's LocalName and NamespaceURI match
						// the given arguments.
						xmlReader.ReadStartElement(schemaMetadata.RootElementName, schemaMetadata.TargetNamespace);
						while (!xmlReader.EOF && !xmlReader.IsStartElement("Parts", schemaMetadata.TargetNamespace))
						{
							if (xmlReader.IsStartElement("ProcessActivityId", schemaMetadata.TargetNamespace))
							{
								batchTrackingContext.ProcessActivityId = xmlReader.ReadString();
							}
							if (xmlReader.IsStartElement("MessagingStepActivityIds", schemaMetadata.TargetNamespace))
							{
								batchTrackingContext.MessagingStepActivityIdList = xmlReader.ReadString().IfNotNullOrEmpty(ids => ids.Split(','));
								break;
							}
							xmlReader.Skip();
						}
						return batchTrackingContext;
					}
				}
				catch (XmlException)
				{
					return null;
				}
				finally
				{
					_probe.Stream.ResetPosition();
				}
			}
		}

		#endregion

		private readonly IProbeStream _probe;
	}
}

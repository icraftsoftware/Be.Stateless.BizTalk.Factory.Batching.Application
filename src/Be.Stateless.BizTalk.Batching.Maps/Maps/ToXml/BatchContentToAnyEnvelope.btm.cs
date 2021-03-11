namespace Be.Stateless.BizTalk.Maps.ToXml {
    
    
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"Be.Stateless.BizTalk.Schemas.Xml.Batch+Content", typeof(global::Be.Stateless.BizTalk.Schemas.Xml.Batch.Content))]
    [Microsoft.XLANGs.BaseTypes.SchemaReference(@"Be.Stateless.BizTalk.Schemas.Xml.Any", typeof(global::Be.Stateless.BizTalk.Schemas.Xml.Any))]
    public sealed class BatchContentToAnyEnvelope : global::Microsoft.XLANGs.BaseTypes.TransformBase {
        
        private const string _strMap = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!--
  Copyright © 2012 - 2021 François Chabot

  Licensed under the Apache License, Version 2.0 (the ""License"");
  you may not use this file except in compliance with the License.
  You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an ""AS IS"" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
-->
<xsl:stylesheet xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" version=""1.0""
                xmlns:agg=""http://schemas.microsoft.com/BizTalk/2003/aggschema""
                xmlns:bat=""urn:schemas.stateless.be:biztalk:batch:2012:12""
                xmlns:msxsl=""urn:schemas-microsoft-com:xslt""
                exclude-result-prefixes=""agg bat msxsl"">
  <xsl:output omit-xml-declaration=""yes"" method=""xml"" version=""1.0"" />

  <xsl:template match=""/"">
    <xsl:apply-templates select=""/*/agg:InputMessagePart_0/*"" />
  </xsl:template>

  <xsl:template match=""bat:parts-here"">
    <xsl:apply-templates select=""/*/agg:InputMessagePart_1/bat:BatchContent/bat:Parts/*"" />
  </xsl:template>

  <!-- this is somehow the identity transform but swallows unwanted xml namespaces -->
  <xsl:template match=""*"">
    <xsl:element name=""{name(.)}"" namespace=""{namespace-uri(.)}"">
      <xsl:copy-of select=""namespace::*[. != 'urn:schemas.stateless.be:biztalk:batch:2012:12' and . != 'http://schemas.microsoft.com/BizTalk/2003/aggschema']"" />
      <xsl:apply-templates select=""@* | node()"" />
    </xsl:element>
  </xsl:template>

  <xsl:template match=""@* | comment() | processing-instruction() | text()"">
    <xsl:copy />
  </xsl:template>

</xsl:stylesheet>";
        
        private const string _xsltEngine = @"";
        
        private const int _useXSLTransform = 0;
        
        private const string _strArgList = @"<ExtensionObjects />";
        
        private const string _strSrcSchemasList0 = @"Be.Stateless.BizTalk.Schemas.Xml.Batch+Content";
        
        private const global::Be.Stateless.BizTalk.Schemas.Xml.Batch.Content _srcSchemaTypeReference0 = null;
        
        private const string _strTrgSchemasList0 = @"Be.Stateless.BizTalk.Schemas.Xml.Any";
        
        private const global::Be.Stateless.BizTalk.Schemas.Xml.Any _trgSchemaTypeReference0 = null;
        
        public override string XmlContent {
            get {
                return _strMap;
            }
        }
        
        public override string XsltEngine {
            get {
                return _xsltEngine;
            }
        }
        
        public override int UseXSLTransform {
            get {
                return _useXSLTransform;
            }
        }
        
        public override string XsltArgumentListContent {
            get {
                return _strArgList;
            }
        }
        
        public override string[] SourceSchemas {
            get {
                string[] _SrcSchemas = new string [1];
                _SrcSchemas[0] = @"Be.Stateless.BizTalk.Schemas.Xml.Batch+Content";
                return _SrcSchemas;
            }
        }
        
        public override string[] TargetSchemas {
            get {
                string[] _TrgSchemas = new string [1];
                _TrgSchemas[0] = @"Be.Stateless.BizTalk.Schemas.Xml.Any";
                return _TrgSchemas;
            }
        }
    }
}

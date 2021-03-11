namespace Be.Stateless.BizTalk.Schemas.Xml {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"BatchContent", @"ReleaseBatch"})]
    public sealed class Batch : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""urn:schemas.stateless.be:biztalk:batch:2012:12"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" elementFormDefault=""qualified"" targetNamespace=""urn:schemas.stateless.be:biztalk:batch:2012:12"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:annotation>
    <xs:documentation><![CDATA[
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
]]></xs:documentation>
    <xs:appinfo>
      <fileNameHint xmlns=""http://schemas.microsoft.com/servicemodel/adapters/metadata/xsd"">Batch</fileNameHint>
    </xs:appinfo>
  </xs:annotation>
  <xs:element name=""BatchContent"">
    <xs:annotation>
      <xs:appinfo>
        <san:Properties xmlns:bf=""urn:schemas.stateless.be:biztalk:properties:system:2012:04"" xmlns:tp=""urn:schemas.stateless.be:biztalk:properties:tracking:2012:04"" xmlns:san=""urn:schemas.stateless.be:biztalk:annotations:2013:01"">
          <bf:EnvironmentTag mode=""promote"" xpath=""/*/*[local-name()='EnvironmentTag']"" />
          <tp:Value1 xpath=""/*/*[local-name()='EnvelopeSpecName']"" />
          <tp:Value2 xpath=""/*/*[local-name()='EnvironmentTag']"" />
          <tp:Value3 xpath=""/*/*[local-name()='Partition']"" />
        </san:Properties>
        <b:recordInfo rootTypeName=""Content"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" />
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""EnvelopeSpecName"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""EnvironmentTag"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""Partition"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""ProcessActivityId"">
          <xs:simpleType>
            <xs:restriction base=""xs:string"">
              <xs:length value=""32"" />
            </xs:restriction>
          </xs:simpleType>
        </xs:element>
        <xs:element minOccurs=""0"" name=""MessagingStepActivityIds"" type=""xs:string"" />
        <xs:element name=""Parts"">
          <xs:complexType>
            <xs:sequence>
              <xs:any maxOccurs=""unbounded"" processContents=""lax"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
  <xs:element name=""ReleaseBatch"">
    <xs:annotation>
      <xs:appinfo>
        <san:Properties xmlns:tp=""urn:schemas.stateless.be:biztalk:properties:tracking:2012:04"" xmlns:san=""urn:schemas.stateless.be:biztalk:annotations:2013:01"">
          <tp:Value1 xpath=""/*/*[local-name()='EnvelopeSpecName']"" />
          <tp:Value2 xpath=""/*/*[local-name()='EnvironmentTag']"" />
          <tp:Value3 xpath=""/*/*[local-name()='Partition']"" />
        </san:Properties>
        <b:recordInfo rootTypeName=""Release"" />
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""EnvelopeSpecName"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""EnvironmentTag"" type=""xs:string"" />
        <xs:element minOccurs=""0"" name=""Partition"" type=""xs:string"" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public Batch() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [2];
                _RootElements[0] = "BatchContent";
                _RootElements[1] = "ReleaseBatch";
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
        
        [Schema(@"urn:schemas.stateless.be:biztalk:batch:2012:12",@"BatchContent")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"BatchContent"})]
        public sealed class Content : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public Content() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "BatchContent";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
        
        [Schema(@"urn:schemas.stateless.be:biztalk:batch:2012:12",@"ReleaseBatch")]
        [System.SerializableAttribute()]
        [SchemaRoots(new string[] {@"ReleaseBatch"})]
        public sealed class Release : Microsoft.XLANGs.BaseTypes.SchemaBase {
            
            [System.NonSerializedAttribute()]
            private static object _rawSchema;
            
            public Release() {
            }
            
            public override string XmlContent {
                get {
                    return _strSchema;
                }
            }
            
            public override string[] RootNodes {
                get {
                    string[] _RootElements = new string [1];
                    _RootElements[0] = "ReleaseBatch";
                    return _RootElements;
                }
            }
            
            protected override object RawSchema {
                get {
                    return _rawSchema;
                }
                set {
                    _rawSchema = value;
                }
            }
        }
    }
}

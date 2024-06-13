// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.32989
//    <NameSpace>NPOI.OpenXmlFormats.Encryption</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>True</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net40</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><OrderXMLAttrib>True</OrderXMLAttrib><EnableEncoding>True</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>True</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace NPOI.OpenXmlFormats.Encryption
{
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.Xml;
    using System.Collections.Generic;
    using NPOI.OpenXml4Net.Util;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/office/2006/keyEncryptor/password")]
    [System.Xml.Serialization.XmlRootAttribute("encryptedKey", Namespace = "http://schemas.microsoft.com/office/2006/keyEncryptor/password", IsNullable = false)]
    public partial class CT_PasswordKeyEncryptor
    {

        private uint saltSizeField;

        private uint blockSizeField;

        private uint keyBitsField;

        private uint hashSizeField;

        private ST_CipherAlgorithm cipherAlgorithmField;

        private ST_CipherChaining cipherChainingField;

        private ST_HashAlgorithm hashAlgorithmField;

        private byte[] saltValueField;

        private uint spinCountField;

        private byte[] encryptedVerifierHashInputField;

        private byte[] encryptedVerifierHashValueField;

        private byte[] encryptedKeyValueField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint saltSize
        {
            get
            {
                return this.saltSizeField;
            }
            set
            {
                this.saltSizeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint blockSize
        {
            get
            {
                return this.blockSizeField;
            }
            set
            {
                this.blockSizeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint keyBits
        {
            get
            {
                return this.keyBitsField;
            }
            set
            {
                this.keyBitsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint hashSize
        {
            get
            {
                return this.hashSizeField;
            }
            set
            {
                this.hashSizeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ST_CipherAlgorithm cipherAlgorithm
        {
            get
            {
                return this.cipherAlgorithmField;
            }
            set
            {
                this.cipherAlgorithmField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ST_CipherChaining cipherChaining
        {
            get
            {
                return this.cipherChainingField;
            }
            set
            {
                this.cipherChainingField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ST_HashAlgorithm hashAlgorithm
        {
            get
            {
                return this.hashAlgorithmField;
            }
            set
            {
                this.hashAlgorithmField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "base64Binary")]
        public byte[] saltValue
        {
            get
            {
                return this.saltValueField;
            }
            set
            {
                this.saltValueField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint spinCount
        {
            get
            {
                return this.spinCountField;
            }
            set
            {
                this.spinCountField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "base64Binary")]
        public byte[] encryptedVerifierHashInput
        {
            get
            {
                return this.encryptedVerifierHashInputField;
            }
            set
            {
                this.encryptedVerifierHashInputField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "base64Binary")]
        public byte[] encryptedVerifierHashValue
        {
            get
            {
                return this.encryptedVerifierHashValueField;
            }
            set
            {
                this.encryptedVerifierHashValueField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "base64Binary")]
        public byte[] encryptedKeyValue
        {
            get
            {
                return this.encryptedKeyValueField;
            }
            set
            {
                this.encryptedKeyValueField = value;
            }
        }



        public static CT_PasswordKeyEncryptor Parse(XmlNode node, XmlNamespaceManager nameSpaceManager)
        {
            if (node == null)
                return null;
            CT_PasswordKeyEncryptor ctObj = new CT_PasswordKeyEncryptor();
            ctObj.spinCount = XmlHelper.ReadUInt(node.Attributes["spinCount"]);
            ctObj.saltSize = XmlHelper.ReadUInt(node.Attributes["saltSize"]);
            ctObj.blockSize = XmlHelper.ReadUInt(node.Attributes["blockSize"]);
            ctObj.keyBits = XmlHelper.ReadUInt(node.Attributes["keyBits"]);
            ctObj.hashSize = XmlHelper.ReadUInt(node.Attributes["hashSize"]);
            ctObj.cipherAlgorithm = XmlHelper.ReadEnum<ST_CipherAlgorithm>(node.Attributes["cipherAlgorithm"]);
            ctObj.cipherChaining = XmlHelper.ReadEnum<ST_CipherChaining>(node.Attributes["cipherChaining"]);
            ctObj.hashAlgorithm = XmlHelper.ReadEnum<ST_HashAlgorithm>(node.Attributes["hashAlgorithm"]);

            if (node.Attributes["saltValue"] != null)
                ctObj.saltValue = Convert.FromBase64String(node.Attributes["saltValue"].Value);
            if (node.Attributes["encryptedVerifierHashInput"] != null)
                ctObj.encryptedVerifierHashInput = Convert.FromBase64String(node.Attributes["encryptedVerifierHashInput"].Value);
            if (node.Attributes["encryptedVerifierHashValue"] != null)
                ctObj.encryptedVerifierHashValue = Convert.FromBase64String(node.Attributes["encryptedVerifierHashValue"].Value);
            if (node.Attributes["encryptedKeyValue"] != null)
                ctObj.encryptedKeyValue = Convert.FromBase64String(node.Attributes["encryptedKeyValue"].Value);
            return ctObj;
           
        }
    }
}

// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.32989
//    <NameSpace>NPOI.OpenXmlFormats.Encryption</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>True</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>True</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net40</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><OrderXMLAttrib>True</OrderXMLAttrib><EnableEncoding>True</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>True</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
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
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Collections.Generic;
    using NPOI.OpenXml4Net.Util;
    using EnumsNET;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption")]
    [System.Xml.Serialization.XmlRootAttribute("encryption", Namespace = "http://schemas.microsoft.com/office/2006/encryption", IsNullable = false)]
    public partial class CT_Encryption
    {

        private CT_KeyData keyDataField;

        private CT_DataIntegrity dataIntegrityField;

        private CT_KeyEncryptors keyEncryptorsField;

        /// <summary>
        /// CT_Encryption class constructor
        /// </summary>
        public CT_Encryption()
        {
            //this.keyEncryptorsField = new List<CT_KeyEncryptor>();
            //this.dataIntegrityField = new CT_DataIntegrity();
            //this.keyDataField = new CT_KeyData();
        }

        [XmlElement(Order = 0)]
        public CT_KeyData keyData
        {
            get
            {
                return this.keyDataField;
            }
            set
            {
                this.keyDataField = value;
            }
        }

        [XmlElement(Order = 1)]
        public CT_DataIntegrity dataIntegrity
        {
            get
            {
                return this.dataIntegrityField;
            }
            set
            {
                this.dataIntegrityField = value;
            }
        }

        [XmlElement(Order = 2)]
        public CT_KeyEncryptors keyEncryptors
        {
            get
            {
                return this.keyEncryptorsField;
            }
            set
            {
                this.keyEncryptorsField = value;
            }
        }




        public CT_KeyData AddNewKeyData()
        {
            throw new NotImplementedException();
        }

        public CT_KeyEncryptors AddNewKeyEncryptors()
        {
            throw new NotImplementedException();
        }

        public CT_DataIntegrity AddNewDataIntegrity()
        {
            throw new NotImplementedException();
        }

        internal static CT_Encryption Parse(XmlNode node, XmlNamespaceManager nameSpaceManager)
        {
            if (node == null)
                return null;
            CT_Encryption encryption = new CT_Encryption();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.LocalName == "keyData")
                    encryption.keyData = CT_KeyData.Parse(childNode, nameSpaceManager);
                else if (childNode.LocalName == "dataIntegrity")
                    encryption.dataIntegrity = CT_DataIntegrity.Parse(childNode, nameSpaceManager);
                else if (childNode.LocalName == "keyEncryptors")
                {
                    encryption.keyEncryptorsField = CT_KeyEncryptors.Parse(childNode, nameSpaceManager);
                }
                    
            }
            return encryption;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption", IsNullable = true)]
    public partial class CT_KeyData
    {

        private uint saltSizeField;

        private uint blockSizeField;

        private uint keyBitsField;

        private uint hashSizeField;

        private ST_CipherAlgorithm cipherAlgorithmField;

        private ST_CipherChaining cipherChainingField;

        private ST_HashAlgorithm hashAlgorithmField;

        private byte[] saltValueField;

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

        internal static CT_KeyData Parse(XmlNode node, XmlNamespaceManager nameSpaceManager)
        {
            if (node == null)
                return null;
            CT_KeyData ctObj = new CT_KeyData();
            ctObj.saltSize = XmlHelper.ReadUInt(node.Attributes["saltSize"]);
            ctObj.blockSize = XmlHelper.ReadUInt(node.Attributes["blockSize"]);
            ctObj.keyBits = XmlHelper.ReadUInt(node.Attributes["keyBits"]);
            ctObj.hashSize = XmlHelper.ReadUInt(node.Attributes["hashSize"]);
            ctObj.cipherAlgorithm = XmlHelper.ReadEnum<ST_CipherAlgorithm>(node.Attributes["cipherAlgorithm"]);
            ctObj.cipherChaining = XmlHelper.ReadEnum<ST_CipherChaining>(node.Attributes["cipherChaining"]);
            ctObj.hashAlgorithm = XmlHelper.ReadEnum<ST_HashAlgorithm>(node.Attributes["hashAlgorithm"]);

            if (node.Attributes["saltValue"] != null)
                ctObj.saltValue = Convert.FromBase64String(node.Attributes["saltValue"].Value);
            
            return ctObj;
        }
        
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption", IsNullable = false)]
    public enum ST_CipherAlgorithm
    {

        /// <remarks/>
        AES,

        /// <remarks/>
        RC2,

        /// <remarks/>
        RC4,

        /// <remarks/>
        DES,

        /// <remarks/>
        DESX,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3DES")]
        Item3DES,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3DES_112")]
        Item3DES_112,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption", IsNullable = false)]
    public enum ST_CipherChaining
    {

        /// <remarks/>
        ChainingModeCBC,

        /// <remarks/>
        ChainingModeCFB,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption", IsNullable = false)]
    public enum ST_HashAlgorithm
    {

        /// <remarks/>
        SHA1,

        /// <remarks/>
        SHA256,

        /// <remarks/>
        SHA384,

        /// <remarks/>
        SHA512,

        /// <remarks/>
        MD5,

        /// <remarks/>
        MD4,

        /// <remarks/>
        MD2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("RIPEMD-128")]
        RIPEMD128,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("RIPEMD-160")]
        RIPEMD160,

        /// <remarks/>
        WHIRLPOOL,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption", IsNullable = true)]
    public partial class CT_KeyEncryptor
    {

        private object itemField;

        private CT_KeyEncryptorUri uriField;

        private bool uriFieldSpecified;

        [System.Xml.Serialization.XmlElementAttribute("encryptedKey", typeof(CT_CertificateKeyEncryptor), Namespace = "http://schemas.microsoft.com/office/2006/keyEncryptor/certificate", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("encryptedKey", typeof(CT_PasswordKeyEncryptor), Namespace = "http://schemas.microsoft.com/office/2006/keyEncryptor/password", Order = 0)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public CT_KeyEncryptorUri uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool uriSpecified
        {
            get
            {
                return this.uriFieldSpecified;
            }
            set
            {
                this.uriFieldSpecified = value;
            }
        }




        public CT_PasswordKeyEncryptor AddNewEncryptedPasswordKey()
        {
            CT_PasswordKeyEncryptor t = new CT_PasswordKeyEncryptor();
            this.itemField = t;
            return t;
        }

        public CT_CertificateKeyEncryptor AddNewEncryptedCertificateKey()
        {
            CT_CertificateKeyEncryptor t = new CT_CertificateKeyEncryptor();
            this.itemField = t;
            return t;
        }

        public static CT_KeyEncryptor Parse(XmlNode node, XmlNamespaceManager nameSpaceManager)
        {
            if (node == null)
                return null;
            CT_KeyEncryptor ctObj = new CT_KeyEncryptor();
            if (node.Attributes["uri"] != null)
            {
                ctObj.uriFieldSpecified = true;
                ctObj.uriField = Enums.Parse<CT_KeyEncryptorUri>(node.Attributes["uri"].Value, false, EnumFormat.Description);
            }
            foreach(XmlNode child in node.ChildNodes)
            {
                if(ctObj.uriField == CT_KeyEncryptorUri.httpschemasmicrosoftcomoffice2006keyEncryptorcertificate)
                {
                    ctObj.itemField = CT_CertificateKeyEncryptor.Parse(child, nameSpaceManager);
                }
                else
                {
                    ctObj.itemField = CT_PasswordKeyEncryptor.Parse(child, nameSpaceManager);
                }
                
            }
            return ctObj;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/office/2006/encryption")]
    public enum CT_KeyEncryptorUri
    {
        /// <remarks/>
        [Description("http://schemas.microsoft.com/office/2006/keyEncryptor/password")]
        httpschemasmicrosoftcomoffice2006keyEncryptorpassword,

        /// <remarks/>
        [Description("http://schemas.microsoft.com/office/2006/keyEncryptor/certificate")]
        httpschemasmicrosoftcomoffice2006keyEncryptorcertificate,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption", IsNullable = true)]
    public partial class CT_DataIntegrity
    {

        private byte[] encryptedHmacKeyField;

        private byte[] encryptedHmacValueField;


        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "base64Binary")]
        public byte[] encryptedHmacKey
        {
            get
            {
                return this.encryptedHmacKeyField;
            }
            set
            {
                this.encryptedHmacKeyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "base64Binary")]
        public byte[] encryptedHmacValue
        {
            get
            {
                return this.encryptedHmacValueField;
            }
            set
            {
                this.encryptedHmacValueField = value;
            }
        }



        internal static CT_DataIntegrity Parse(XmlNode node, XmlNamespaceManager nameSpaceManager)
        {
            if (node == null)
                return null;
            CT_DataIntegrity ctObj = new CT_DataIntegrity();

            if (node.Attributes["encryptedHmacKey"] != null)
                ctObj.encryptedHmacKey = Convert.FromBase64String(node.Attributes["encryptedHmacKey"].Value);
            if (node.Attributes["encryptedHmacValue"] != null)
                ctObj.encryptedHmacValue = Convert.FromBase64String(node.Attributes["encryptedHmacValue"].Value);
            return ctObj;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/office/2006/encryption", IsNullable = true)]
    public partial class CT_KeyEncryptors
    {

        private List<CT_KeyEncryptor> keyEncryptorField;

        /// <summary>
        /// CT_KeyEncryptors class constructor
        /// </summary>
        public CT_KeyEncryptors()
        {
            this.keyEncryptorField = new List<CT_KeyEncryptor>();
        }

        [System.Xml.Serialization.XmlElementAttribute("keyEncryptor", Order = 0)]
        public List<CT_KeyEncryptor> keyEncryptor
        {
            get
            {
                return this.keyEncryptorField;
            }
            set
            {
                this.keyEncryptorField = value;
            }
        }

        public CT_KeyEncryptor AddNewKeyEncryptor()
        {
            CT_KeyEncryptor item = new CT_KeyEncryptor();
            this.keyEncryptorField.Add(item);
            return item;
        }

        internal static CT_KeyEncryptors Parse(XmlNode node, XmlNamespaceManager nameSpaceManager)
        {
            if (node == null)
                return null;
            CT_KeyEncryptors ctObj = new CT_KeyEncryptors();
            foreach (XmlNode childNode in node.ChildNodes)
                ctObj.keyEncryptorField.Add(CT_KeyEncryptor.Parse(childNode, nameSpaceManager));
            return ctObj;
        }
    }
}

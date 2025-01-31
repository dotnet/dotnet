// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.Xml")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides classes to support the creation and validation of XML digital signatures. The classes in this namespace implement the World Wide Web Consortium Recommendation, \"XML-Signature Syntax and Processing\", described at http://www.w3.org/TR/xmldsig-core/.\r\n\r\nCommonly Used Types:\r\nSystem.Security.Cryptography.Xml.CipherData\r\nSystem.Security.Cryptography.Xml.CipherReference\r\nSystem.Security.Cryptography.Xml.DataObject\r\nSystem.Security.Cryptography.Xml.DataReference\r\nSystem.Security.Cryptography.Xml.DSAKeyValue\r\nSystem.Security.Cryptography.Xml.EncryptedData\r\nSystem.Security.Cryptography.Xml.EncryptedKey\r\nSystem.Security.Cryptography.Xml.EncryptedReference\r\nSystem.Security.Cryptography.Xml.EncryptedType\r\nSystem.Security.Cryptography.Xml.EncryptedXml\r\nSystem.Security.Cryptography.Xml.EncryptionMethod\r\nSystem.Security.Cryptography.Xml.EncryptionProperty\r\nSystem.Security.Cryptography.Xml.EncryptionPropertyCollection\r\nSystem.Security.Cryptography.Xml.KeyInfo\r\nSystem.Security.Cryptography.Xml.KeyInfoClause\r\nSystem.Security.Cryptography.Xml.KeyInfoEncryptedKey\r\nSystem.Security.Cryptography.Xml.KeyInfoName\r\nSystem.Security.Cryptography.Xml.KeyInfoNode\r\nSystem.Security.Cryptography.Xml.KeyInfoRetrievalMethod\r\nSystem.Security.Cryptography.Xml.KeyInfoX509Data\r\nSystem.Security.Cryptography.Xml.KeyReference\r\nSystem.Security.Cryptography.Xml.Reference\r\nSystem.Security.Cryptography.Xml.ReferenceList\r\nSystem.Security.Cryptography.Xml.RSAKeyValue\r\nSystem.Security.Cryptography.Xml.Signature\r\nSystem.Security.Cryptography.Xml.SignedInfo\r\nSystem.Security.Cryptography.Xml.SignedXml\r\nSystem.Security.Cryptography.Xml.Transform\r\nSystem.Security.Cryptography.Xml.TransformChain\r\nSystem.Security.Cryptography.Xml.XmlDecryptionTransform\r\nSystem.Security.Cryptography.Xml.XmlDsigBase64Transform\r\nSystem.Security.Cryptography.Xml.XmlDsigC14NTransform\r\nSystem.Security.Cryptography.Xml.XmlDsigC14NWithCommentsTransform\r\nSystem.Security.Cryptography.Xml.XmlDsigEnvelopedSignatureTransform\r\nSystem.Security.Cryptography.Xml.XmlDsigExcC14NTransform\r\nSystem.Security.Cryptography.Xml.XmlDsigExcC14NWithCommentsTransform\r\nSystem.Security.Cryptography.Xml.XmlDsigXPathTransform\r\nSystem.Security.Cryptography.Xml.XmlDsigXsltTransform\r\nSystem.Security.Cryptography.Xml.XmlLicenseTransform")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.Xml")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Security.Cryptography.Xml
{
    public sealed partial class CipherData
    {
        public CipherData() { }

        public CipherData(byte[] cipherValue) { }

        public CipherData(CipherReference cipherReference) { }

        public CipherReference CipherReference { get { throw null; } set { } }

        public byte[] CipherValue { get { throw null; } set { } }

        public System.Xml.XmlElement GetXml() { throw null; }

        public void LoadXml(System.Xml.XmlElement value) { }
    }

    public sealed partial class CipherReference : EncryptedReference
    {
        public CipherReference() { }

        public CipherReference(string uri, TransformChain transformChain) { }

        public CipherReference(string uri) { }

        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement value) { }
    }

    public partial class CryptoSignedXmlRecursionException : System.Xml.XmlException
    {
        public CryptoSignedXmlRecursionException() { }

        protected CryptoSignedXmlRecursionException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public CryptoSignedXmlRecursionException(string message, Exception inner) { }

        public CryptoSignedXmlRecursionException(string message) { }
    }

    public partial class DataObject
    {
        public DataObject() { }

        public DataObject(string id, string mimeType, string encoding, System.Xml.XmlElement data) { }

        public System.Xml.XmlNodeList Data { get { throw null; } set { } }

        public string Encoding { get { throw null; } set { } }

        public string Id { get { throw null; } set { } }

        public string MimeType { get { throw null; } set { } }

        public System.Xml.XmlElement GetXml() { throw null; }

        public void LoadXml(System.Xml.XmlElement value) { }
    }

    public sealed partial class DataReference : EncryptedReference
    {
        public DataReference() { }

        public DataReference(string uri, TransformChain transformChain) { }

        public DataReference(string uri) { }
    }

    public partial class DSAKeyValue : KeyInfoClause
    {
        [Runtime.Versioning.UnsupportedOSPlatform("ios")]
        [Runtime.Versioning.UnsupportedOSPlatform("tvos")]
        public DSAKeyValue() { }

        public DSAKeyValue(DSA key) { }

        public DSA Key { get { throw null; } set { } }

        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement value) { }
    }

    public sealed partial class EncryptedData : EncryptedType
    {
        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement value) { }
    }

    public sealed partial class EncryptedKey : EncryptedType
    {
        public string CarriedKeyName { get { throw null; } set { } }

        public string Recipient { get { throw null; } set { } }

        public ReferenceList ReferenceList { get { throw null; } }

        public void AddReference(DataReference dataReference) { }

        public void AddReference(KeyReference keyReference) { }

        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement value) { }
    }

    public abstract partial class EncryptedReference
    {
        protected EncryptedReference() { }

        protected EncryptedReference(string uri, TransformChain transformChain) { }

        protected EncryptedReference(string uri) { }

        protected internal bool CacheValid { get { throw null; } }

        protected string ReferenceType { get { throw null; } set { } }

        public TransformChain TransformChain { get { throw null; } set { } }

        public string Uri { get { throw null; } set { } }

        public void AddTransform(Transform transform) { }

        public virtual System.Xml.XmlElement GetXml() { throw null; }

        public virtual void LoadXml(System.Xml.XmlElement value) { }
    }

    public abstract partial class EncryptedType
    {
        public virtual CipherData CipherData { get { throw null; } set { } }

        public virtual string Encoding { get { throw null; } set { } }

        public virtual EncryptionMethod EncryptionMethod { get { throw null; } set { } }

        public virtual EncryptionPropertyCollection EncryptionProperties { get { throw null; } }

        public virtual string Id { get { throw null; } set { } }

        public KeyInfo KeyInfo { get { throw null; } set { } }

        public virtual string MimeType { get { throw null; } set { } }

        public virtual string Type { get { throw null; } set { } }

        public void AddProperty(EncryptionProperty ep) { }

        public abstract System.Xml.XmlElement GetXml();
        public abstract void LoadXml(System.Xml.XmlElement value);
    }

    public partial class EncryptedXml
    {
        public const string XmlEncAES128KeyWrapUrl = "http://www.w3.org/2001/04/xmlenc#kw-aes128";
        public const string XmlEncAES128Url = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
        public const string XmlEncAES192KeyWrapUrl = "http://www.w3.org/2001/04/xmlenc#kw-aes192";
        public const string XmlEncAES192Url = "http://www.w3.org/2001/04/xmlenc#aes192-cbc";
        public const string XmlEncAES256KeyWrapUrl = "http://www.w3.org/2001/04/xmlenc#kw-aes256";
        public const string XmlEncAES256Url = "http://www.w3.org/2001/04/xmlenc#aes256-cbc";
        public const string XmlEncDESUrl = "http://www.w3.org/2001/04/xmlenc#des-cbc";
        public const string XmlEncElementContentUrl = "http://www.w3.org/2001/04/xmlenc#Content";
        public const string XmlEncElementUrl = "http://www.w3.org/2001/04/xmlenc#Element";
        public const string XmlEncEncryptedKeyUrl = "http://www.w3.org/2001/04/xmlenc#EncryptedKey";
        public const string XmlEncNamespaceUrl = "http://www.w3.org/2001/04/xmlenc#";
        public const string XmlEncRSA15Url = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";
        public const string XmlEncRSAOAEPUrl = "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p";
        public const string XmlEncSHA256Url = "http://www.w3.org/2001/04/xmlenc#sha256";
        public const string XmlEncSHA512Url = "http://www.w3.org/2001/04/xmlenc#sha512";
        public const string XmlEncTripleDESKeyWrapUrl = "http://www.w3.org/2001/04/xmlenc#kw-tripledes";
        public const string XmlEncTripleDESUrl = "http://www.w3.org/2001/04/xmlenc#tripledes-cbc";
        public EncryptedXml() { }

        public EncryptedXml(System.Xml.XmlDocument document, Policy.Evidence evidence) { }

        public EncryptedXml(System.Xml.XmlDocument document) { }

        public Policy.Evidence DocumentEvidence { get { throw null; } set { } }

        public Text.Encoding Encoding { get { throw null; } set { } }

        public CipherMode Mode { get { throw null; } set { } }

        public PaddingMode Padding { get { throw null; } set { } }

        public string Recipient { get { throw null; } set { } }

        public System.Xml.XmlResolver Resolver { get { throw null; } set { } }

        public int XmlDSigSearchDepth { get { throw null; } set { } }

        public void AddKeyNameMapping(string keyName, object keyObject) { }

        public void ClearKeyNameMappings() { }

        public byte[] DecryptData(EncryptedData encryptedData, SymmetricAlgorithm symmetricAlgorithm) { throw null; }

        public void DecryptDocument() { }

        public virtual byte[] DecryptEncryptedKey(EncryptedKey encryptedKey) { throw null; }

        public static byte[] DecryptKey(byte[] keyData, RSA rsa, bool useOAEP) { throw null; }

        public static byte[] DecryptKey(byte[] keyData, SymmetricAlgorithm symmetricAlgorithm) { throw null; }

        public EncryptedData Encrypt(System.Xml.XmlElement inputElement, X509Certificates.X509Certificate2 certificate) { throw null; }

        public EncryptedData Encrypt(System.Xml.XmlElement inputElement, string keyName) { throw null; }

        public byte[] EncryptData(byte[] plaintext, SymmetricAlgorithm symmetricAlgorithm) { throw null; }

        public byte[] EncryptData(System.Xml.XmlElement inputElement, SymmetricAlgorithm symmetricAlgorithm, bool content) { throw null; }

        public static byte[] EncryptKey(byte[] keyData, RSA rsa, bool useOAEP) { throw null; }

        public static byte[] EncryptKey(byte[] keyData, SymmetricAlgorithm symmetricAlgorithm) { throw null; }

        public virtual byte[] GetDecryptionIV(EncryptedData encryptedData, string symmetricAlgorithmUri) { throw null; }

        public virtual SymmetricAlgorithm GetDecryptionKey(EncryptedData encryptedData, string symmetricAlgorithmUri) { throw null; }

        public virtual System.Xml.XmlElement GetIdElement(System.Xml.XmlDocument document, string idValue) { throw null; }

        public void ReplaceData(System.Xml.XmlElement inputElement, byte[] decryptedData) { }

        public static void ReplaceElement(System.Xml.XmlElement inputElement, EncryptedData encryptedData, bool content) { }
    }

    public partial class EncryptionMethod
    {
        public EncryptionMethod() { }

        public EncryptionMethod(string algorithm) { }

        public string KeyAlgorithm { get { throw null; } set { } }

        public int KeySize { get { throw null; } set { } }

        public System.Xml.XmlElement GetXml() { throw null; }

        public void LoadXml(System.Xml.XmlElement value) { }
    }

    public sealed partial class EncryptionProperty
    {
        public EncryptionProperty() { }

        public EncryptionProperty(System.Xml.XmlElement elementProperty) { }

        public string Id { get { throw null; } }

        public System.Xml.XmlElement PropertyElement { get { throw null; } set { } }

        public string Target { get { throw null; } }

        public System.Xml.XmlElement GetXml() { throw null; }

        public void LoadXml(System.Xml.XmlElement value) { }
    }

    public sealed partial class EncryptionPropertyCollection : Collections.IList, Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public bool IsFixedSize { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        [System.Runtime.CompilerServices.IndexerName("ItemOf")]
        public EncryptionProperty this[int index] { get { throw null; } set { } }

        public object SyncRoot { get { throw null; } }

        object Collections.IList.this[int index] { get { throw null; } set { } }

        public int Add(EncryptionProperty value) { throw null; }

        public void Clear() { }

        public bool Contains(EncryptionProperty value) { throw null; }

        public void CopyTo(Array array, int index) { }

        public void CopyTo(EncryptionProperty[] array, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public int IndexOf(EncryptionProperty value) { throw null; }

        public void Insert(int index, EncryptionProperty value) { }

        public EncryptionProperty Item(int index) { throw null; }

        public void Remove(EncryptionProperty value) { }

        public void RemoveAt(int index) { }

        int Collections.IList.Add(object value) { throw null; }

        bool Collections.IList.Contains(object value) { throw null; }

        int Collections.IList.IndexOf(object value) { throw null; }

        void Collections.IList.Insert(int index, object value) { }

        void Collections.IList.Remove(object value) { }
    }

    public partial interface IRelDecryptor
    {
        IO.Stream Decrypt(EncryptionMethod encryptionMethod, KeyInfo keyInfo, IO.Stream toDecrypt);
    }

    public partial class KeyInfo : Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public string Id { get { throw null; } set { } }

        public void AddClause(KeyInfoClause clause) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public Collections.IEnumerator GetEnumerator(Type requestedObjectType) { throw null; }

        public System.Xml.XmlElement GetXml() { throw null; }

        public void LoadXml(System.Xml.XmlElement value) { }
    }

    public abstract partial class KeyInfoClause
    {
        public abstract System.Xml.XmlElement GetXml();
        public abstract void LoadXml(System.Xml.XmlElement element);
    }

    public partial class KeyInfoEncryptedKey : KeyInfoClause
    {
        public KeyInfoEncryptedKey() { }

        public KeyInfoEncryptedKey(EncryptedKey encryptedKey) { }

        public EncryptedKey EncryptedKey { get { throw null; } set { } }

        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement value) { }
    }

    public partial class KeyInfoName : KeyInfoClause
    {
        public KeyInfoName() { }

        public KeyInfoName(string keyName) { }

        public string Value { get { throw null; } set { } }

        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement value) { }
    }

    public partial class KeyInfoNode : KeyInfoClause
    {
        public KeyInfoNode() { }

        public KeyInfoNode(System.Xml.XmlElement node) { }

        public System.Xml.XmlElement Value { get { throw null; } set { } }

        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement value) { }
    }

    public partial class KeyInfoRetrievalMethod : KeyInfoClause
    {
        public KeyInfoRetrievalMethod() { }

        public KeyInfoRetrievalMethod(string strUri, string typeName) { }

        public KeyInfoRetrievalMethod(string strUri) { }

        public string Type { get { throw null; } set { } }

        public string Uri { get { throw null; } set { } }

        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement value) { }
    }

    public partial class KeyInfoX509Data : KeyInfoClause
    {
        public KeyInfoX509Data() { }

        public KeyInfoX509Data(byte[] rgbCert) { }

        public KeyInfoX509Data(X509Certificates.X509Certificate cert, X509Certificates.X509IncludeOption includeOption) { }

        public KeyInfoX509Data(X509Certificates.X509Certificate cert) { }

        public Collections.ArrayList Certificates { get { throw null; } }

        public byte[] CRL { get { throw null; } set { } }

        public Collections.ArrayList IssuerSerials { get { throw null; } }

        public Collections.ArrayList SubjectKeyIds { get { throw null; } }

        public Collections.ArrayList SubjectNames { get { throw null; } }

        public void AddCertificate(X509Certificates.X509Certificate certificate) { }

        public void AddIssuerSerial(string issuerName, string serialNumber) { }

        public void AddSubjectKeyId(byte[] subjectKeyId) { }

        public void AddSubjectKeyId(string subjectKeyId) { }

        public void AddSubjectName(string subjectName) { }

        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement element) { }
    }

    public sealed partial class KeyReference : EncryptedReference
    {
        public KeyReference() { }

        public KeyReference(string uri, TransformChain transformChain) { }

        public KeyReference(string uri) { }
    }

    public partial class Reference
    {
        public Reference() { }

        public Reference(IO.Stream stream) { }

        public Reference(string uri) { }

        public string DigestMethod { get { throw null; } set { } }

        public byte[] DigestValue { get { throw null; } set { } }

        public string Id { get { throw null; } set { } }

        public TransformChain TransformChain { get { throw null; } set { } }

        public string Type { get { throw null; } set { } }

        public string Uri { get { throw null; } set { } }

        public void AddTransform(Transform transform) { }

        public System.Xml.XmlElement GetXml() { throw null; }

        public void LoadXml(System.Xml.XmlElement value) { }
    }

    public sealed partial class ReferenceList : Collections.IList, Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        [System.Runtime.CompilerServices.IndexerName("ItemOf")]
        public EncryptedReference this[int index] { get { throw null; } set { } }

        public object SyncRoot { get { throw null; } }

        bool Collections.IList.IsFixedSize { get { throw null; } }

        bool Collections.IList.IsReadOnly { get { throw null; } }

        object Collections.IList.this[int index] { get { throw null; } set { } }

        public int Add(object value) { throw null; }

        public void Clear() { }

        public bool Contains(object value) { throw null; }

        public void CopyTo(Array array, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public int IndexOf(object value) { throw null; }

        public void Insert(int index, object value) { }

        public EncryptedReference Item(int index) { throw null; }

        public void Remove(object value) { }

        public void RemoveAt(int index) { }
    }

    public partial class RSAKeyValue : KeyInfoClause
    {
        public RSAKeyValue() { }

        public RSAKeyValue(RSA key) { }

        public RSA Key { get { throw null; } set { } }

        public override System.Xml.XmlElement GetXml() { throw null; }

        public override void LoadXml(System.Xml.XmlElement value) { }
    }

    public partial class Signature
    {
        public string Id { get { throw null; } set { } }

        public KeyInfo KeyInfo { get { throw null; } set { } }

        public Collections.IList ObjectList { get { throw null; } set { } }

        public byte[] SignatureValue { get { throw null; } set { } }

        public SignedInfo SignedInfo { get { throw null; } set { } }

        public void AddObject(DataObject dataObject) { }

        public System.Xml.XmlElement GetXml() { throw null; }

        public void LoadXml(System.Xml.XmlElement value) { }
    }

    public partial class SignedInfo : Collections.ICollection, Collections.IEnumerable
    {
        public string CanonicalizationMethod { get { throw null; } set { } }

        public Transform CanonicalizationMethodObject { get { throw null; } }

        public int Count { get { throw null; } }

        public string Id { get { throw null; } set { } }

        public bool IsReadOnly { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public Collections.ArrayList References { get { throw null; } }

        public string SignatureLength { get { throw null; } set { } }

        public string SignatureMethod { get { throw null; } set { } }

        public object SyncRoot { get { throw null; } }

        public void AddReference(Reference reference) { }

        public void CopyTo(Array array, int index) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public System.Xml.XmlElement GetXml() { throw null; }

        public void LoadXml(System.Xml.XmlElement value) { }
    }

    public partial class SignedXml
    {
        protected Signature m_signature;
        protected string m_strSigningKeyName;
        public const string XmlDecryptionTransformUrl = "http://www.w3.org/2002/07/decrypt#XML";
        public const string XmlDsigBase64TransformUrl = "http://www.w3.org/2000/09/xmldsig#base64";
        public const string XmlDsigC14NTransformUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
        public const string XmlDsigC14NWithCommentsTransformUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments";
        public const string XmlDsigCanonicalizationUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
        public const string XmlDsigCanonicalizationWithCommentsUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments";
        public const string XmlDsigDSAUrl = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";
        public const string XmlDsigEnvelopedSignatureTransformUrl = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";
        public const string XmlDsigExcC14NTransformUrl = "http://www.w3.org/2001/10/xml-exc-c14n#";
        public const string XmlDsigExcC14NWithCommentsTransformUrl = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";
        public const string XmlDsigHMACSHA1Url = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";
        public const string XmlDsigMinimalCanonicalizationUrl = "http://www.w3.org/2000/09/xmldsig#minimal";
        public const string XmlDsigNamespaceUrl = "http://www.w3.org/2000/09/xmldsig#";
        public const string XmlDsigRSASHA1Url = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
        public const string XmlDsigRSASHA256Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        public const string XmlDsigRSASHA384Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384";
        public const string XmlDsigRSASHA512Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";
        public const string XmlDsigSHA1Url = "http://www.w3.org/2000/09/xmldsig#sha1";
        public const string XmlDsigSHA256Url = "http://www.w3.org/2001/04/xmlenc#sha256";
        public const string XmlDsigSHA384Url = "http://www.w3.org/2001/04/xmldsig-more#sha384";
        public const string XmlDsigSHA512Url = "http://www.w3.org/2001/04/xmlenc#sha512";
        public const string XmlDsigXPathTransformUrl = "http://www.w3.org/TR/1999/REC-xpath-19991116";
        public const string XmlDsigXsltTransformUrl = "http://www.w3.org/TR/1999/REC-xslt-19991116";
        public const string XmlLicenseTransformUrl = "urn:mpeg:mpeg21:2003:01-REL-R-NS:licenseTransform";
        public SignedXml() { }

        public SignedXml(System.Xml.XmlDocument document) { }

        public SignedXml(System.Xml.XmlElement elem) { }

        public EncryptedXml EncryptedXml { get { throw null; } set { } }

        public KeyInfo KeyInfo { get { throw null; } set { } }

        public System.Xml.XmlResolver Resolver { set { } }

        public Collections.ObjectModel.Collection<string> SafeCanonicalizationMethods { get { throw null; } }

        public Signature Signature { get { throw null; } }

        public Func<SignedXml, bool> SignatureFormatValidator { get { throw null; } set { } }

        public string SignatureLength { get { throw null; } }

        public string SignatureMethod { get { throw null; } }

        public byte[] SignatureValue { get { throw null; } }

        public SignedInfo SignedInfo { get { throw null; } }

        public AsymmetricAlgorithm SigningKey { get { throw null; } set { } }

        public string SigningKeyName { get { throw null; } set { } }

        public void AddObject(DataObject dataObject) { }

        public void AddReference(Reference reference) { }

        public bool CheckSignature() { throw null; }

        public bool CheckSignature(AsymmetricAlgorithm key) { throw null; }

        public bool CheckSignature(KeyedHashAlgorithm macAlg) { throw null; }

        public bool CheckSignature(X509Certificates.X509Certificate2 certificate, bool verifySignatureOnly) { throw null; }

        public bool CheckSignatureReturningKey(out AsymmetricAlgorithm signingKey) { throw null; }

        public void ComputeSignature() { }

        public void ComputeSignature(KeyedHashAlgorithm macAlg) { }

        public virtual System.Xml.XmlElement GetIdElement(System.Xml.XmlDocument document, string idValue) { throw null; }

        protected virtual AsymmetricAlgorithm GetPublicKey() { throw null; }

        public System.Xml.XmlElement GetXml() { throw null; }

        public void LoadXml(System.Xml.XmlElement value) { }
    }

    public abstract partial class Transform
    {
        public string Algorithm { get { throw null; } set { } }

        public System.Xml.XmlElement Context { get { throw null; } set { } }

        public abstract Type[] InputTypes { get; }
        public abstract Type[] OutputTypes { get; }

        public Collections.Hashtable PropagatedNamespaces { get { throw null; } }

        public System.Xml.XmlResolver Resolver { set { } }

        public virtual byte[] GetDigestedOutput(HashAlgorithm hash) { throw null; }

        protected abstract System.Xml.XmlNodeList GetInnerXml();
        public abstract object GetOutput();
        public abstract object GetOutput(Type type);
        public System.Xml.XmlElement GetXml() { throw null; }

        public abstract void LoadInnerXml(System.Xml.XmlNodeList nodeList);
        public abstract void LoadInput(object obj);
    }

    public partial class TransformChain
    {
        public int Count { get { throw null; } }

        public Transform this[int index] { get { throw null; } }

        public void Add(Transform transform) { }

        public Collections.IEnumerator GetEnumerator() { throw null; }
    }

    public partial class XmlDecryptionTransform : Transform
    {
        public EncryptedXml EncryptedXml { get { throw null; } set { } }

        public override Type[] InputTypes { get { throw null; } }

        public override Type[] OutputTypes { get { throw null; } }

        public void AddExceptUri(string uri) { }

        protected override System.Xml.XmlNodeList GetInnerXml() { throw null; }

        public override object GetOutput() { throw null; }

        public override object GetOutput(Type type) { throw null; }

        protected virtual bool IsTargetElement(System.Xml.XmlElement inputElement, string idValue) { throw null; }

        public override void LoadInnerXml(System.Xml.XmlNodeList nodeList) { }

        public override void LoadInput(object obj) { }
    }

    public partial class XmlDsigBase64Transform : Transform
    {
        public override Type[] InputTypes { get { throw null; } }

        public override Type[] OutputTypes { get { throw null; } }

        protected override System.Xml.XmlNodeList GetInnerXml() { throw null; }

        public override object GetOutput() { throw null; }

        public override object GetOutput(Type type) { throw null; }

        public override void LoadInnerXml(System.Xml.XmlNodeList nodeList) { }

        public override void LoadInput(object obj) { }
    }

    public partial class XmlDsigC14NTransform : Transform
    {
        public XmlDsigC14NTransform() { }

        public XmlDsigC14NTransform(bool includeComments) { }

        public override Type[] InputTypes { get { throw null; } }

        public override Type[] OutputTypes { get { throw null; } }

        public override byte[] GetDigestedOutput(HashAlgorithm hash) { throw null; }

        protected override System.Xml.XmlNodeList GetInnerXml() { throw null; }

        public override object GetOutput() { throw null; }

        public override object GetOutput(Type type) { throw null; }

        public override void LoadInnerXml(System.Xml.XmlNodeList nodeList) { }

        public override void LoadInput(object obj) { }
    }

    public partial class XmlDsigC14NWithCommentsTransform : XmlDsigC14NTransform
    {
    }

    public partial class XmlDsigEnvelopedSignatureTransform : Transform
    {
        public XmlDsigEnvelopedSignatureTransform() { }

        public XmlDsigEnvelopedSignatureTransform(bool includeComments) { }

        public override Type[] InputTypes { get { throw null; } }

        public override Type[] OutputTypes { get { throw null; } }

        protected override System.Xml.XmlNodeList GetInnerXml() { throw null; }

        public override object GetOutput() { throw null; }

        public override object GetOutput(Type type) { throw null; }

        public override void LoadInnerXml(System.Xml.XmlNodeList nodeList) { }

        public override void LoadInput(object obj) { }
    }

    public partial class XmlDsigExcC14NTransform : Transform
    {
        public XmlDsigExcC14NTransform() { }

        public XmlDsigExcC14NTransform(bool includeComments, string inclusiveNamespacesPrefixList) { }

        public XmlDsigExcC14NTransform(bool includeComments) { }

        public XmlDsigExcC14NTransform(string inclusiveNamespacesPrefixList) { }

        public string InclusiveNamespacesPrefixList { get { throw null; } set { } }

        public override Type[] InputTypes { get { throw null; } }

        public override Type[] OutputTypes { get { throw null; } }

        public override byte[] GetDigestedOutput(HashAlgorithm hash) { throw null; }

        protected override System.Xml.XmlNodeList GetInnerXml() { throw null; }

        public override object GetOutput() { throw null; }

        public override object GetOutput(Type type) { throw null; }

        public override void LoadInnerXml(System.Xml.XmlNodeList nodeList) { }

        public override void LoadInput(object obj) { }
    }

    public partial class XmlDsigExcC14NWithCommentsTransform : XmlDsigExcC14NTransform
    {
        public XmlDsigExcC14NWithCommentsTransform() { }

        public XmlDsigExcC14NWithCommentsTransform(string inclusiveNamespacesPrefixList) { }
    }

    public partial class XmlDsigXPathTransform : Transform
    {
        public override Type[] InputTypes { get { throw null; } }

        public override Type[] OutputTypes { get { throw null; } }

        protected override System.Xml.XmlNodeList GetInnerXml() { throw null; }

        public override object GetOutput() { throw null; }

        public override object GetOutput(Type type) { throw null; }

        public override void LoadInnerXml(System.Xml.XmlNodeList nodeList) { }

        public override void LoadInput(object obj) { }
    }

    public partial class XmlDsigXsltTransform : Transform
    {
        public XmlDsigXsltTransform() { }

        public XmlDsigXsltTransform(bool includeComments) { }

        public override Type[] InputTypes { get { throw null; } }

        public override Type[] OutputTypes { get { throw null; } }

        protected override System.Xml.XmlNodeList GetInnerXml() { throw null; }

        public override object GetOutput() { throw null; }

        public override object GetOutput(Type type) { throw null; }

        public override void LoadInnerXml(System.Xml.XmlNodeList nodeList) { }

        public override void LoadInput(object obj) { }
    }

    public partial class XmlLicenseTransform : Transform
    {
        public IRelDecryptor Decryptor { get { throw null; } set { } }

        public override Type[] InputTypes { get { throw null; } }

        public override Type[] OutputTypes { get { throw null; } }

        protected override System.Xml.XmlNodeList GetInnerXml() { throw null; }

        public override object GetOutput() { throw null; }

        public override object GetOutput(Type type) { throw null; }

        public override void LoadInnerXml(System.Xml.XmlNodeList nodeList) { }

        public override void LoadInput(object obj) { }
    }
}
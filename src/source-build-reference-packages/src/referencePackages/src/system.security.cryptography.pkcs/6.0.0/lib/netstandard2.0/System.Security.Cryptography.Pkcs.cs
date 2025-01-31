// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.Pkcs")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides support for PKCS and CMS algorithms.\r\n\r\nCommonly Used Types:\r\nSystem.Security.Cryptography.Pkcs.EnvelopedCms")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.Pkcs")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Security.Cryptography
{
    public sealed partial class CryptographicAttributeObject
    {
        public CryptographicAttributeObject(Oid oid, AsnEncodedDataCollection? values) { }

        public CryptographicAttributeObject(Oid oid) { }

        public Oid Oid { get { throw null; } }

        public AsnEncodedDataCollection Values { get { throw null; } }
    }

    public sealed partial class CryptographicAttributeObjectCollection : Collections.ICollection, Collections.IEnumerable
    {
        public CryptographicAttributeObjectCollection() { }

        public CryptographicAttributeObjectCollection(CryptographicAttributeObject attribute) { }

        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public CryptographicAttributeObject this[int index] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public int Add(AsnEncodedData asnEncodedData) { throw null; }

        public int Add(CryptographicAttributeObject attribute) { throw null; }

        public void CopyTo(CryptographicAttributeObject[] array, int index) { }

        public CryptographicAttributeObjectEnumerator GetEnumerator() { throw null; }

        public void Remove(CryptographicAttributeObject attribute) { }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class CryptographicAttributeObjectEnumerator : Collections.IEnumerator
    {
        internal CryptographicAttributeObjectEnumerator() { }

        public CryptographicAttributeObject Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }
}

namespace System.Security.Cryptography.Pkcs
{
    public sealed partial class AlgorithmIdentifier
    {
        public AlgorithmIdentifier() { }

        public AlgorithmIdentifier(Oid oid, int keyLength) { }

        public AlgorithmIdentifier(Oid oid) { }

        public int KeyLength { get { throw null; } set { } }

        public Oid Oid { get { throw null; } set { } }

        public byte[] Parameters { get { throw null; } set { } }
    }

    public sealed partial class CmsRecipient
    {
        public CmsRecipient(SubjectIdentifierType recipientIdentifierType, X509Certificates.X509Certificate2 certificate) { }

        public CmsRecipient(X509Certificates.X509Certificate2 certificate) { }

        public X509Certificates.X509Certificate2 Certificate { get { throw null; } }

        public SubjectIdentifierType RecipientIdentifierType { get { throw null; } }
    }

    public sealed partial class CmsRecipientCollection : Collections.ICollection, Collections.IEnumerable
    {
        public CmsRecipientCollection() { }

        public CmsRecipientCollection(CmsRecipient recipient) { }

        public CmsRecipientCollection(SubjectIdentifierType recipientIdentifierType, X509Certificates.X509Certificate2Collection certificates) { }

        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public CmsRecipient this[int index] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public int Add(CmsRecipient recipient) { throw null; }

        public void CopyTo(Array array, int index) { }

        public void CopyTo(CmsRecipient[] array, int index) { }

        public CmsRecipientEnumerator GetEnumerator() { throw null; }

        public void Remove(CmsRecipient recipient) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class CmsRecipientEnumerator : Collections.IEnumerator
    {
        internal CmsRecipientEnumerator() { }

        public CmsRecipient Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    public sealed partial class CmsSigner
    {
        public CmsSigner() { }

        public CmsSigner(CspParameters parameters) { }

        public CmsSigner(SubjectIdentifierType signerIdentifierType, X509Certificates.X509Certificate2? certificate, AsymmetricAlgorithm? privateKey) { }

        public CmsSigner(SubjectIdentifierType signerIdentifierType, X509Certificates.X509Certificate2? certificate) { }

        public CmsSigner(SubjectIdentifierType signerIdentifierType) { }

        public CmsSigner(X509Certificates.X509Certificate2? certificate) { }

        public X509Certificates.X509Certificate2? Certificate { get { throw null; } set { } }

        public X509Certificates.X509Certificate2Collection Certificates { get { throw null; } }

        public Oid DigestAlgorithm { get { throw null; } set { } }

        public X509Certificates.X509IncludeOption IncludeOption { get { throw null; } set { } }

        public AsymmetricAlgorithm? PrivateKey { get { throw null; } set { } }

        public CryptographicAttributeObjectCollection SignedAttributes { get { throw null; } }

        public SubjectIdentifierType SignerIdentifierType { get { throw null; } set { } }

        public CryptographicAttributeObjectCollection UnsignedAttributes { get { throw null; } }
    }

    public sealed partial class ContentInfo
    {
        public ContentInfo(byte[] content) { }

        public ContentInfo(Oid contentType, byte[] content) { }

        public byte[] Content { get { throw null; } }

        public Oid ContentType { get { throw null; } }

        public static Oid GetContentType(byte[] encodedMessage) { throw null; }

        public static Oid GetContentType(ReadOnlySpan<byte> encodedMessage) { throw null; }
    }

    public sealed partial class EnvelopedCms
    {
        public EnvelopedCms() { }

        public EnvelopedCms(ContentInfo contentInfo, AlgorithmIdentifier encryptionAlgorithm) { }

        public EnvelopedCms(ContentInfo contentInfo) { }

        public X509Certificates.X509Certificate2Collection Certificates { get { throw null; } }

        public AlgorithmIdentifier ContentEncryptionAlgorithm { get { throw null; } }

        public ContentInfo ContentInfo { get { throw null; } }

        public RecipientInfoCollection RecipientInfos { get { throw null; } }

        public CryptographicAttributeObjectCollection UnprotectedAttributes { get { throw null; } }

        public int Version { get { throw null; } }

        public void Decode(byte[] encodedMessage) { }

        public void Decode(ReadOnlySpan<byte> encodedMessage) { }

        public void Decrypt() { }

        public void Decrypt(RecipientInfo recipientInfo, AsymmetricAlgorithm? privateKey) { }

        public void Decrypt(RecipientInfo recipientInfo, X509Certificates.X509Certificate2Collection extraStore) { }

        public void Decrypt(RecipientInfo recipientInfo) { }

        public void Decrypt(X509Certificates.X509Certificate2Collection extraStore) { }

        public byte[] Encode() { throw null; }

        public void Encrypt(CmsRecipient recipient) { }

        public void Encrypt(CmsRecipientCollection recipients) { }
    }

    public sealed partial class KeyAgreeRecipientInfo : RecipientInfo
    {
        internal KeyAgreeRecipientInfo() { }

        public DateTime Date { get { throw null; } }

        public override byte[] EncryptedKey { get { throw null; } }

        public override AlgorithmIdentifier KeyEncryptionAlgorithm { get { throw null; } }

        public SubjectIdentifierOrKey OriginatorIdentifierOrKey { get { throw null; } }

        public CryptographicAttributeObject? OtherKeyAttribute { get { throw null; } }

        public override SubjectIdentifier RecipientIdentifier { get { throw null; } }

        public override int Version { get { throw null; } }
    }

    public sealed partial class KeyTransRecipientInfo : RecipientInfo
    {
        internal KeyTransRecipientInfo() { }

        public override byte[] EncryptedKey { get { throw null; } }

        public override AlgorithmIdentifier KeyEncryptionAlgorithm { get { throw null; } }

        public override SubjectIdentifier RecipientIdentifier { get { throw null; } }

        public override int Version { get { throw null; } }
    }

    public partial class Pkcs9AttributeObject : AsnEncodedData
    {
        public Pkcs9AttributeObject() { }

        public Pkcs9AttributeObject(AsnEncodedData asnEncodedData) { }

        public Pkcs9AttributeObject(Oid oid, byte[] encodedData) { }

        public Pkcs9AttributeObject(string oid, byte[] encodedData) { }

        public new Oid? Oid { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public sealed partial class Pkcs9ContentType : Pkcs9AttributeObject
    {
        public Oid ContentType { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public sealed partial class Pkcs9DocumentDescription : Pkcs9AttributeObject
    {
        public Pkcs9DocumentDescription() { }

        public Pkcs9DocumentDescription(byte[] encodedDocumentDescription) { }

        public Pkcs9DocumentDescription(string documentDescription) { }

        public string DocumentDescription { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public sealed partial class Pkcs9DocumentName : Pkcs9AttributeObject
    {
        public Pkcs9DocumentName() { }

        public Pkcs9DocumentName(byte[] encodedDocumentName) { }

        public Pkcs9DocumentName(string documentName) { }

        public string DocumentName { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public sealed partial class Pkcs9MessageDigest : Pkcs9AttributeObject
    {
        public byte[] MessageDigest { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public sealed partial class Pkcs9SigningTime : Pkcs9AttributeObject
    {
        public Pkcs9SigningTime() { }

        public Pkcs9SigningTime(byte[] encodedSigningTime) { }

        public Pkcs9SigningTime(DateTime signingTime) { }

        public DateTime SigningTime { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public sealed partial class PublicKeyInfo
    {
        internal PublicKeyInfo() { }

        public AlgorithmIdentifier Algorithm { get { throw null; } }

        public byte[] KeyValue { get { throw null; } }
    }

    public abstract partial class RecipientInfo
    {
        internal RecipientInfo() { }

        public abstract byte[] EncryptedKey { get; }
        public abstract AlgorithmIdentifier KeyEncryptionAlgorithm { get; }
        public abstract SubjectIdentifier RecipientIdentifier { get; }

        public RecipientInfoType Type { get { throw null; } }

        public abstract int Version { get; }
    }

    public sealed partial class RecipientInfoCollection : Collections.ICollection, Collections.IEnumerable
    {
        internal RecipientInfoCollection() { }

        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public RecipientInfo this[int index] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public void CopyTo(Array array, int index) { }

        public void CopyTo(RecipientInfo[] array, int index) { }

        public RecipientInfoEnumerator GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class RecipientInfoEnumerator : Collections.IEnumerator
    {
        internal RecipientInfoEnumerator() { }

        public RecipientInfo Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    public enum RecipientInfoType
    {
        Unknown = 0,
        KeyTransport = 1,
        KeyAgreement = 2
    }

    public sealed partial class SignedCms
    {
        public SignedCms() { }

        public SignedCms(ContentInfo contentInfo, bool detached) { }

        public SignedCms(ContentInfo contentInfo) { }

        public SignedCms(SubjectIdentifierType signerIdentifierType, ContentInfo contentInfo, bool detached) { }

        public SignedCms(SubjectIdentifierType signerIdentifierType, ContentInfo contentInfo) { }

        public SignedCms(SubjectIdentifierType signerIdentifierType) { }

        public X509Certificates.X509Certificate2Collection Certificates { get { throw null; } }

        public ContentInfo ContentInfo { get { throw null; } }

        public bool Detached { get { throw null; } }

        public SignerInfoCollection SignerInfos { get { throw null; } }

        public int Version { get { throw null; } }

        public void AddCertificate(X509Certificates.X509Certificate2 certificate) { }

        public void CheckHash() { }

        public void CheckSignature(bool verifySignatureOnly) { }

        public void CheckSignature(X509Certificates.X509Certificate2Collection extraStore, bool verifySignatureOnly) { }

        public void ComputeSignature() { }

        public void ComputeSignature(CmsSigner signer, bool silent) { }

        public void ComputeSignature(CmsSigner signer) { }

        public void Decode(byte[] encodedMessage) { }

        public void Decode(ReadOnlySpan<byte> encodedMessage) { }

        public byte[] Encode() { throw null; }

        public void RemoveCertificate(X509Certificates.X509Certificate2 certificate) { }

        public void RemoveSignature(int index) { }

        public void RemoveSignature(SignerInfo signerInfo) { }
    }

    public sealed partial class SignerInfo
    {
        internal SignerInfo() { }

        public X509Certificates.X509Certificate2? Certificate { get { throw null; } }

        public SignerInfoCollection CounterSignerInfos { get { throw null; } }

        public Oid DigestAlgorithm { get { throw null; } }

        public Oid SignatureAlgorithm { get { throw null; } }

        public CryptographicAttributeObjectCollection SignedAttributes { get { throw null; } }

        public SubjectIdentifier SignerIdentifier { get { throw null; } }

        public CryptographicAttributeObjectCollection UnsignedAttributes { get { throw null; } }

        public int Version { get { throw null; } }

        public void AddUnsignedAttribute(AsnEncodedData unsignedAttribute) { }

        public void CheckHash() { }

        public void CheckSignature(bool verifySignatureOnly) { }

        public void CheckSignature(X509Certificates.X509Certificate2Collection extraStore, bool verifySignatureOnly) { }

        public void ComputeCounterSignature() { }

        public void ComputeCounterSignature(CmsSigner signer) { }

        public byte[] GetSignature() { throw null; }

        public void RemoveCounterSignature(int index) { }

        public void RemoveCounterSignature(SignerInfo counterSignerInfo) { }

        public void RemoveUnsignedAttribute(AsnEncodedData unsignedAttribute) { }
    }

    public sealed partial class SignerInfoCollection : Collections.ICollection, Collections.IEnumerable
    {
        internal SignerInfoCollection() { }

        public int Count { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public SignerInfo this[int index] { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public void CopyTo(Array array, int index) { }

        public void CopyTo(SignerInfo[] array, int index) { }

        public SignerInfoEnumerator GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class SignerInfoEnumerator : Collections.IEnumerator
    {
        internal SignerInfoEnumerator() { }

        public SignerInfo Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    public sealed partial class SubjectIdentifier
    {
        internal SubjectIdentifier() { }

        public SubjectIdentifierType Type { get { throw null; } }

        public object? Value { get { throw null; } }

        public bool MatchesCertificate(X509Certificates.X509Certificate2 certificate) { throw null; }
    }

    public sealed partial class SubjectIdentifierOrKey
    {
        internal SubjectIdentifierOrKey() { }

        public SubjectIdentifierOrKeyType Type { get { throw null; } }

        public object Value { get { throw null; } }
    }

    public enum SubjectIdentifierOrKeyType
    {
        Unknown = 0,
        IssuerAndSerialNumber = 1,
        SubjectKeyIdentifier = 2,
        PublicKeyInfo = 3
    }

    public enum SubjectIdentifierType
    {
        Unknown = 0,
        IssuerAndSerialNumber = 1,
        SubjectKeyIdentifier = 2,
        NoSignature = 3
    }
}

namespace System.Security.Cryptography.Xml
{
    public partial struct X509IssuerSerial
    {
        private object _dummy;
        private int _dummyPrimitive;
        public string IssuerName { get { throw null; } set { } }

        public string SerialNumber { get { throw null; } set { } }
    }
}
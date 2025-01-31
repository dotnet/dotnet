// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName = "")]
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
        public CmsRecipient(SubjectIdentifierType recipientIdentifierType, X509Certificates.X509Certificate2 certificate, RSAEncryptionPadding rsaEncryptionPadding) { }

        public CmsRecipient(SubjectIdentifierType recipientIdentifierType, X509Certificates.X509Certificate2 certificate) { }

        public CmsRecipient(X509Certificates.X509Certificate2 certificate, RSAEncryptionPadding rsaEncryptionPadding) { }

        public CmsRecipient(X509Certificates.X509Certificate2 certificate) { }

        public X509Certificates.X509Certificate2 Certificate { get { throw null; } }

        public SubjectIdentifierType RecipientIdentifierType { get { throw null; } }

        public RSAEncryptionPadding? RSAEncryptionPadding { get { throw null; } }
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

    public sealed partial class Pkcs12Builder
    {
        public bool IsSealed { get { throw null; } }

        public void AddSafeContentsEncrypted(Pkcs12SafeContents safeContents, byte[]? passwordBytes, PbeParameters pbeParameters) { }

        public void AddSafeContentsEncrypted(Pkcs12SafeContents safeContents, ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { }

        public void AddSafeContentsEncrypted(Pkcs12SafeContents safeContents, ReadOnlySpan<char> password, PbeParameters pbeParameters) { }

        public void AddSafeContentsEncrypted(Pkcs12SafeContents safeContents, string? password, PbeParameters pbeParameters) { }

        public void AddSafeContentsUnencrypted(Pkcs12SafeContents safeContents) { }

        public byte[] Encode() { throw null; }

        public void SealWithMac(ReadOnlySpan<char> password, HashAlgorithmName hashAlgorithm, int iterationCount) { }

        public void SealWithMac(string? password, HashAlgorithmName hashAlgorithm, int iterationCount) { }

        public void SealWithoutIntegrity() { }

        public bool TryEncode(Span<byte> destination, out int bytesWritten) { throw null; }
    }

    public sealed partial class Pkcs12CertBag : Pkcs12SafeBag
    {
        public Pkcs12CertBag(Oid certificateType, ReadOnlyMemory<byte> encodedCertificate) : base(default!, default, default) { }

        public ReadOnlyMemory<byte> EncodedCertificate { get { throw null; } }

        public bool IsX509Certificate { get { throw null; } }

        public X509Certificates.X509Certificate2 GetCertificate() { throw null; }

        public Oid GetCertificateType() { throw null; }
    }

    public enum Pkcs12ConfidentialityMode
    {
        Unknown = 0,
        None = 1,
        Password = 2,
        PublicKey = 3
    }

    public sealed partial class Pkcs12Info
    {
        internal Pkcs12Info() { }

        public Collections.ObjectModel.ReadOnlyCollection<Pkcs12SafeContents> AuthenticatedSafe { get { throw null; } }

        public Pkcs12IntegrityMode IntegrityMode { get { throw null; } }

        public static Pkcs12Info Decode(ReadOnlyMemory<byte> encodedBytes, out int bytesConsumed, bool skipCopy = false) { throw null; }

        public bool VerifyMac(ReadOnlySpan<char> password) { throw null; }

        public bool VerifyMac(string? password) { throw null; }
    }

    public enum Pkcs12IntegrityMode
    {
        Unknown = 0,
        None = 1,
        Password = 2,
        PublicKey = 3
    }

    public sealed partial class Pkcs12KeyBag : Pkcs12SafeBag
    {
        public Pkcs12KeyBag(ReadOnlyMemory<byte> pkcs8PrivateKey, bool skipCopy = false) : base(default!, default, default) { }

        public ReadOnlyMemory<byte> Pkcs8PrivateKey { get { throw null; } }
    }

    public abstract partial class Pkcs12SafeBag
    {
        protected Pkcs12SafeBag(string bagIdValue, ReadOnlyMemory<byte> encodedBagValue, bool skipCopy = false) { }

        public CryptographicAttributeObjectCollection Attributes { get { throw null; } }

        public ReadOnlyMemory<byte> EncodedBagValue { get { throw null; } }

        public byte[] Encode() { throw null; }

        public Oid GetBagId() { throw null; }

        public bool TryEncode(Span<byte> destination, out int bytesWritten) { throw null; }
    }

    public sealed partial class Pkcs12SafeContents
    {
        public Pkcs12ConfidentialityMode ConfidentialityMode { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public Pkcs12CertBag AddCertificate(X509Certificates.X509Certificate2 certificate) { throw null; }

        public Pkcs12KeyBag AddKeyUnencrypted(AsymmetricAlgorithm key) { throw null; }

        public Pkcs12SafeContentsBag AddNestedContents(Pkcs12SafeContents safeContents) { throw null; }

        public void AddSafeBag(Pkcs12SafeBag safeBag) { }

        public Pkcs12SecretBag AddSecret(Oid secretType, ReadOnlyMemory<byte> secretValue) { throw null; }

        public Pkcs12ShroudedKeyBag AddShroudedKey(AsymmetricAlgorithm key, byte[]? passwordBytes, PbeParameters pbeParameters) { throw null; }

        public Pkcs12ShroudedKeyBag AddShroudedKey(AsymmetricAlgorithm key, ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }

        public Pkcs12ShroudedKeyBag AddShroudedKey(AsymmetricAlgorithm key, ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }

        public Pkcs12ShroudedKeyBag AddShroudedKey(AsymmetricAlgorithm key, string? password, PbeParameters pbeParameters) { throw null; }

        public void Decrypt(byte[]? passwordBytes) { }

        public void Decrypt(ReadOnlySpan<byte> passwordBytes) { }

        public void Decrypt(ReadOnlySpan<char> password) { }

        public void Decrypt(string? password) { }

        public Collections.Generic.IEnumerable<Pkcs12SafeBag> GetBags() { throw null; }
    }

    public sealed partial class Pkcs12SafeContentsBag : Pkcs12SafeBag
    {
        internal Pkcs12SafeContentsBag() : base(default!, default, default) { }

        public Pkcs12SafeContents? SafeContents { get { throw null; } }
    }

    public sealed partial class Pkcs12SecretBag : Pkcs12SafeBag
    {
        internal Pkcs12SecretBag() : base(default!, default, default) { }

        public ReadOnlyMemory<byte> SecretValue { get { throw null; } }

        public Oid GetSecretType() { throw null; }
    }

    public sealed partial class Pkcs12ShroudedKeyBag : Pkcs12SafeBag
    {
        public Pkcs12ShroudedKeyBag(ReadOnlyMemory<byte> encryptedPkcs8PrivateKey, bool skipCopy = false) : base(default!, default, default) { }

        public ReadOnlyMemory<byte> EncryptedPkcs8PrivateKey { get { throw null; } }
    }

    public sealed partial class Pkcs8PrivateKeyInfo
    {
        public Pkcs8PrivateKeyInfo(Oid algorithmId, ReadOnlyMemory<byte>? algorithmParameters, ReadOnlyMemory<byte> privateKey, bool skipCopies = false) { }

        public Oid AlgorithmId { get { throw null; } }

        public ReadOnlyMemory<byte>? AlgorithmParameters { get { throw null; } }

        public CryptographicAttributeObjectCollection Attributes { get { throw null; } }

        public ReadOnlyMemory<byte> PrivateKeyBytes { get { throw null; } }

        public static Pkcs8PrivateKeyInfo Create(AsymmetricAlgorithm privateKey) { throw null; }

        public static Pkcs8PrivateKeyInfo Decode(ReadOnlyMemory<byte> source, out int bytesRead, bool skipCopy = false) { throw null; }

        public static Pkcs8PrivateKeyInfo DecryptAndDecode(ReadOnlySpan<byte> passwordBytes, ReadOnlyMemory<byte> source, out int bytesRead) { throw null; }

        public static Pkcs8PrivateKeyInfo DecryptAndDecode(ReadOnlySpan<char> password, ReadOnlyMemory<byte> source, out int bytesRead) { throw null; }

        public byte[] Encode() { throw null; }

        public byte[] Encrypt(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }

        public byte[] Encrypt(ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }

        public bool TryEncode(Span<byte> destination, out int bytesWritten) { throw null; }

        public bool TryEncrypt(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }

        public bool TryEncrypt(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
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

    public sealed partial class Pkcs9LocalKeyId : Pkcs9AttributeObject
    {
        public Pkcs9LocalKeyId() { }

        public Pkcs9LocalKeyId(byte[] keyId) { }

        public Pkcs9LocalKeyId(ReadOnlySpan<byte> keyId) { }

        public ReadOnlyMemory<byte> KeyId { get { throw null; } }

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

    public sealed partial class Rfc3161TimestampRequest
    {
        internal Rfc3161TimestampRequest() { }

        public bool HasExtensions { get { throw null; } }

        public Oid HashAlgorithmId { get { throw null; } }

        public Oid? RequestedPolicyId { get { throw null; } }

        public bool RequestSignerCertificate { get { throw null; } }

        public int Version { get { throw null; } }

        public static Rfc3161TimestampRequest CreateFromData(ReadOnlySpan<byte> data, HashAlgorithmName hashAlgorithm, Oid? requestedPolicyId = null, ReadOnlyMemory<byte>? nonce = null, bool requestSignerCertificates = false, X509Certificates.X509ExtensionCollection? extensions = null) { throw null; }

        public static Rfc3161TimestampRequest CreateFromHash(ReadOnlyMemory<byte> hash, HashAlgorithmName hashAlgorithm, Oid? requestedPolicyId = null, ReadOnlyMemory<byte>? nonce = null, bool requestSignerCertificates = false, X509Certificates.X509ExtensionCollection? extensions = null) { throw null; }

        public static Rfc3161TimestampRequest CreateFromHash(ReadOnlyMemory<byte> hash, Oid hashAlgorithmId, Oid? requestedPolicyId = null, ReadOnlyMemory<byte>? nonce = null, bool requestSignerCertificates = false, X509Certificates.X509ExtensionCollection? extensions = null) { throw null; }

        public static Rfc3161TimestampRequest CreateFromSignerInfo(SignerInfo signerInfo, HashAlgorithmName hashAlgorithm, Oid? requestedPolicyId = null, ReadOnlyMemory<byte>? nonce = null, bool requestSignerCertificates = false, X509Certificates.X509ExtensionCollection? extensions = null) { throw null; }

        public byte[] Encode() { throw null; }

        public X509Certificates.X509ExtensionCollection GetExtensions() { throw null; }

        public ReadOnlyMemory<byte> GetMessageHash() { throw null; }

        public ReadOnlyMemory<byte>? GetNonce() { throw null; }

        public Rfc3161TimestampToken ProcessResponse(ReadOnlyMemory<byte> responseBytes, out int bytesConsumed) { throw null; }

        public static bool TryDecode(ReadOnlyMemory<byte> encodedBytes, out Rfc3161TimestampRequest? request, out int bytesConsumed) { throw null; }

        public bool TryEncode(Span<byte> destination, out int bytesWritten) { throw null; }
    }

    public sealed partial class Rfc3161TimestampToken
    {
        internal Rfc3161TimestampToken() { }

        public Rfc3161TimestampTokenInfo TokenInfo { get { throw null; } }

        public SignedCms AsSignedCms() { throw null; }

        public static bool TryDecode(ReadOnlyMemory<byte> encodedBytes, out Rfc3161TimestampToken? token, out int bytesConsumed) { throw null; }

        public bool VerifySignatureForData(ReadOnlySpan<byte> data, out X509Certificates.X509Certificate2? signerCertificate, X509Certificates.X509Certificate2Collection? extraCandidates = null) { throw null; }

        public bool VerifySignatureForHash(ReadOnlySpan<byte> hash, HashAlgorithmName hashAlgorithm, out X509Certificates.X509Certificate2? signerCertificate, X509Certificates.X509Certificate2Collection? extraCandidates = null) { throw null; }

        public bool VerifySignatureForHash(ReadOnlySpan<byte> hash, Oid hashAlgorithmId, out X509Certificates.X509Certificate2? signerCertificate, X509Certificates.X509Certificate2Collection? extraCandidates = null) { throw null; }

        public bool VerifySignatureForSignerInfo(SignerInfo signerInfo, out X509Certificates.X509Certificate2? signerCertificate, X509Certificates.X509Certificate2Collection? extraCandidates = null) { throw null; }
    }

    public sealed partial class Rfc3161TimestampTokenInfo
    {
        public Rfc3161TimestampTokenInfo(Oid policyId, Oid hashAlgorithmId, ReadOnlyMemory<byte> messageHash, ReadOnlyMemory<byte> serialNumber, DateTimeOffset timestamp, long? accuracyInMicroseconds = null, bool isOrdering = false, ReadOnlyMemory<byte>? nonce = null, ReadOnlyMemory<byte>? timestampAuthorityName = null, X509Certificates.X509ExtensionCollection? extensions = null) { }

        public long? AccuracyInMicroseconds { get { throw null; } }

        public bool HasExtensions { get { throw null; } }

        public Oid HashAlgorithmId { get { throw null; } }

        public bool IsOrdering { get { throw null; } }

        public Oid PolicyId { get { throw null; } }

        public DateTimeOffset Timestamp { get { throw null; } }

        public int Version { get { throw null; } }

        public byte[] Encode() { throw null; }

        public X509Certificates.X509ExtensionCollection GetExtensions() { throw null; }

        public ReadOnlyMemory<byte> GetMessageHash() { throw null; }

        public ReadOnlyMemory<byte>? GetNonce() { throw null; }

        public ReadOnlyMemory<byte> GetSerialNumber() { throw null; }

        public ReadOnlyMemory<byte>? GetTimestampAuthorityName() { throw null; }

        public static bool TryDecode(ReadOnlyMemory<byte> encodedBytes, out Rfc3161TimestampTokenInfo? timestampTokenInfo, out int bytesConsumed) { throw null; }

        public bool TryEncode(Span<byte> destination, out int bytesWritten) { throw null; }
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
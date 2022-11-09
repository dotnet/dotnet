// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Security.Cryptography.Pkcs")]
[assembly: AssemblyDescription("System.Security.Cryptography.Pkcs")]
[assembly: AssemblyDefaultAlias("System.Security.Cryptography.Pkcs")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.27129.04")]
[assembly: AssemblyInformationalVersion("4.6.27129.04 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.3.2")]




namespace System.Security.Cryptography
{
    public sealed partial class CryptographicAttributeObject
    {
        public CryptographicAttributeObject(System.Security.Cryptography.Oid oid) { }
        public CryptographicAttributeObject(System.Security.Cryptography.Oid oid, System.Security.Cryptography.AsnEncodedDataCollection values) { }
        public System.Security.Cryptography.Oid Oid { get { throw null; } }
        public System.Security.Cryptography.AsnEncodedDataCollection Values { get { throw null; } }
    }
    public sealed partial class CryptographicAttributeObjectCollection : System.Collections.ICollection, System.Collections.IEnumerable
    {
        public CryptographicAttributeObjectCollection() { }
        public CryptographicAttributeObjectCollection(System.Security.Cryptography.CryptographicAttributeObject attribute) { }
        public int Count { get { throw null; } }
        public System.Security.Cryptography.CryptographicAttributeObject this[int index] { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        public int Add(System.Security.Cryptography.AsnEncodedData asnEncodedData) { throw null; }
        public int Add(System.Security.Cryptography.CryptographicAttributeObject attribute) { throw null; }
        public void CopyTo(System.Security.Cryptography.CryptographicAttributeObject[] array, int index) { }
        public System.Security.Cryptography.CryptographicAttributeObjectEnumerator GetEnumerator() { throw null; }
        public void Remove(System.Security.Cryptography.CryptographicAttributeObject attribute) { }
        void System.Collections.ICollection.CopyTo(System.Array array, int index) { }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }
    public sealed partial class CryptographicAttributeObjectEnumerator : System.Collections.IEnumerator
    {
        internal CryptographicAttributeObjectEnumerator() { }
        public System.Security.Cryptography.CryptographicAttributeObject Current { get { throw null; } }
        object System.Collections.IEnumerator.Current { get { throw null; } }
        public bool MoveNext() { throw null; }
        public void Reset() { }
    }
}
namespace System.Security.Cryptography.Pkcs
{
    public sealed partial class AlgorithmIdentifier
    {
        public AlgorithmIdentifier() { }
        public AlgorithmIdentifier(System.Security.Cryptography.Oid oid) { }
        public AlgorithmIdentifier(System.Security.Cryptography.Oid oid, int keyLength) { }
        public int KeyLength { get { throw null; } set { } }
        public System.Security.Cryptography.Oid Oid { get { throw null; } set { } }
    }
    public sealed partial class CmsRecipient
    {
        public CmsRecipient(System.Security.Cryptography.Pkcs.SubjectIdentifierType recipientIdentifierType, System.Security.Cryptography.X509Certificates.X509Certificate2 certificate) { }
        public CmsRecipient(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate) { }
        public System.Security.Cryptography.X509Certificates.X509Certificate2 Certificate { get { throw null; } }
        public System.Security.Cryptography.Pkcs.SubjectIdentifierType RecipientIdentifierType { get { throw null; } }
    }
    public sealed partial class CmsRecipientCollection : System.Collections.ICollection, System.Collections.IEnumerable
    {
        public CmsRecipientCollection() { }
        public CmsRecipientCollection(System.Security.Cryptography.Pkcs.CmsRecipient recipient) { }
        public CmsRecipientCollection(System.Security.Cryptography.Pkcs.SubjectIdentifierType recipientIdentifierType, System.Security.Cryptography.X509Certificates.X509Certificate2Collection certificates) { }
        public int Count { get { throw null; } }
        public System.Security.Cryptography.Pkcs.CmsRecipient this[int index] { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        public int Add(System.Security.Cryptography.Pkcs.CmsRecipient recipient) { throw null; }
        public void CopyTo(System.Array array, int index) { }
        public void CopyTo(System.Security.Cryptography.Pkcs.CmsRecipient[] array, int index) { }
        public System.Security.Cryptography.Pkcs.CmsRecipientEnumerator GetEnumerator() { throw null; }
        public void Remove(System.Security.Cryptography.Pkcs.CmsRecipient recipient) { }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }
    public sealed partial class CmsRecipientEnumerator : System.Collections.IEnumerator
    {
        internal CmsRecipientEnumerator() { }
        public System.Security.Cryptography.Pkcs.CmsRecipient Current { get { throw null; } }
        object System.Collections.IEnumerator.Current { get { throw null; } }
        public bool MoveNext() { throw null; }
        public void Reset() { }
    }
    public sealed partial class CmsSigner
    {
        public CmsSigner() { }
        public CmsSigner(System.Security.Cryptography.CspParameters parameters) { }
        public CmsSigner(System.Security.Cryptography.Pkcs.SubjectIdentifierType signerIdentifierType) { }
        public CmsSigner(System.Security.Cryptography.Pkcs.SubjectIdentifierType signerIdentifierType, System.Security.Cryptography.X509Certificates.X509Certificate2 certificate) { }
        public CmsSigner(System.Security.Cryptography.X509Certificates.X509Certificate2 certificate) { }
        public System.Security.Cryptography.X509Certificates.X509Certificate2 Certificate { get { throw null; } set { } }
        public System.Security.Cryptography.X509Certificates.X509Certificate2Collection Certificates { get { throw null; } }
        public System.Security.Cryptography.Oid DigestAlgorithm { get { throw null; } set { } }
        public System.Security.Cryptography.X509Certificates.X509IncludeOption IncludeOption { get { throw null; } set { } }
        public System.Security.Cryptography.CryptographicAttributeObjectCollection SignedAttributes { get { throw null; } }
        public System.Security.Cryptography.Pkcs.SubjectIdentifierType SignerIdentifierType { get { throw null; } set { } }
        public System.Security.Cryptography.CryptographicAttributeObjectCollection UnsignedAttributes { get { throw null; } }
    }
    public sealed partial class ContentInfo
    {
        public ContentInfo(byte[] content) { }
        public ContentInfo(System.Security.Cryptography.Oid contentType, byte[] content) { }
        public byte[] Content { get { throw null; } }
        public System.Security.Cryptography.Oid ContentType { get { throw null; } }
        public static System.Security.Cryptography.Oid GetContentType(byte[] encodedMessage) { throw null; }
    }
    public sealed partial class EnvelopedCms
    {
        public EnvelopedCms() { }
        public EnvelopedCms(System.Security.Cryptography.Pkcs.ContentInfo contentInfo) { }
        public EnvelopedCms(System.Security.Cryptography.Pkcs.ContentInfo contentInfo, System.Security.Cryptography.Pkcs.AlgorithmIdentifier encryptionAlgorithm) { }
        public System.Security.Cryptography.X509Certificates.X509Certificate2Collection Certificates { get { throw null; } }
        public System.Security.Cryptography.Pkcs.AlgorithmIdentifier ContentEncryptionAlgorithm { get { throw null; } }
        public System.Security.Cryptography.Pkcs.ContentInfo ContentInfo { get { throw null; } }
        public System.Security.Cryptography.Pkcs.RecipientInfoCollection RecipientInfos { get { throw null; } }
        public System.Security.Cryptography.CryptographicAttributeObjectCollection UnprotectedAttributes { get { throw null; } }
        public int Version { get { throw null; } }
        public void Decode(byte[] encodedMessage) { }
        public void Decrypt() { }
        public void Decrypt(System.Security.Cryptography.Pkcs.RecipientInfo recipientInfo) { }
        public void Decrypt(System.Security.Cryptography.Pkcs.RecipientInfo recipientInfo, System.Security.Cryptography.X509Certificates.X509Certificate2Collection extraStore) { }
        public void Decrypt(System.Security.Cryptography.X509Certificates.X509Certificate2Collection extraStore) { }
        public byte[] Encode() { throw null; }
        public void Encrypt(System.Security.Cryptography.Pkcs.CmsRecipient recipient) { }
        public void Encrypt(System.Security.Cryptography.Pkcs.CmsRecipientCollection recipients) { }
    }
    public sealed partial class KeyAgreeRecipientInfo : System.Security.Cryptography.Pkcs.RecipientInfo
    {
        internal KeyAgreeRecipientInfo() { }
        public System.DateTime Date { get { throw null; } }
        public override byte[] EncryptedKey { get { throw null; } }
        public override System.Security.Cryptography.Pkcs.AlgorithmIdentifier KeyEncryptionAlgorithm { get { throw null; } }
        public System.Security.Cryptography.Pkcs.SubjectIdentifierOrKey OriginatorIdentifierOrKey { get { throw null; } }
        public System.Security.Cryptography.CryptographicAttributeObject OtherKeyAttribute { get { throw null; } }
        public override System.Security.Cryptography.Pkcs.SubjectIdentifier RecipientIdentifier { get { throw null; } }
        public override int Version { get { throw null; } }
    }
    public sealed partial class KeyTransRecipientInfo : System.Security.Cryptography.Pkcs.RecipientInfo
    {
        internal KeyTransRecipientInfo() { }
        public override byte[] EncryptedKey { get { throw null; } }
        public override System.Security.Cryptography.Pkcs.AlgorithmIdentifier KeyEncryptionAlgorithm { get { throw null; } }
        public override System.Security.Cryptography.Pkcs.SubjectIdentifier RecipientIdentifier { get { throw null; } }
        public override int Version { get { throw null; } }
    }
    public partial class Pkcs9AttributeObject : System.Security.Cryptography.AsnEncodedData
    {
        public Pkcs9AttributeObject() { }
        public Pkcs9AttributeObject(System.Security.Cryptography.AsnEncodedData asnEncodedData) { }
        public Pkcs9AttributeObject(System.Security.Cryptography.Oid oid, byte[] encodedData) { }
        public Pkcs9AttributeObject(string oid, byte[] encodedData) { }
        public new System.Security.Cryptography.Oid Oid { get { throw null; } }
        public override void CopyFrom(System.Security.Cryptography.AsnEncodedData asnEncodedData) { }
    }
    public sealed partial class Pkcs9ContentType : System.Security.Cryptography.Pkcs.Pkcs9AttributeObject
    {
        public Pkcs9ContentType() { }
        public System.Security.Cryptography.Oid ContentType { get { throw null; } }
        public override void CopyFrom(System.Security.Cryptography.AsnEncodedData asnEncodedData) { }
    }
    public sealed partial class Pkcs9DocumentDescription : System.Security.Cryptography.Pkcs.Pkcs9AttributeObject
    {
        public Pkcs9DocumentDescription() { }
        public Pkcs9DocumentDescription(byte[] encodedDocumentDescription) { }
        public Pkcs9DocumentDescription(string documentDescription) { }
        public string DocumentDescription { get { throw null; } }
        public override void CopyFrom(System.Security.Cryptography.AsnEncodedData asnEncodedData) { }
    }
    public sealed partial class Pkcs9DocumentName : System.Security.Cryptography.Pkcs.Pkcs9AttributeObject
    {
        public Pkcs9DocumentName() { }
        public Pkcs9DocumentName(byte[] encodedDocumentName) { }
        public Pkcs9DocumentName(string documentName) { }
        public string DocumentName { get { throw null; } }
        public override void CopyFrom(System.Security.Cryptography.AsnEncodedData asnEncodedData) { }
    }
    public sealed partial class Pkcs9MessageDigest : System.Security.Cryptography.Pkcs.Pkcs9AttributeObject
    {
        public Pkcs9MessageDigest() { }
        public byte[] MessageDigest { get { throw null; } }
        public override void CopyFrom(System.Security.Cryptography.AsnEncodedData asnEncodedData) { }
    }
    public sealed partial class Pkcs9SigningTime : System.Security.Cryptography.Pkcs.Pkcs9AttributeObject
    {
        public Pkcs9SigningTime() { }
        public Pkcs9SigningTime(byte[] encodedSigningTime) { }
        public Pkcs9SigningTime(System.DateTime signingTime) { }
        public System.DateTime SigningTime { get { throw null; } }
        public override void CopyFrom(System.Security.Cryptography.AsnEncodedData asnEncodedData) { }
    }
    public sealed partial class PublicKeyInfo
    {
        internal PublicKeyInfo() { }
        public System.Security.Cryptography.Pkcs.AlgorithmIdentifier Algorithm { get { throw null; } }
        public byte[] KeyValue { get { throw null; } }
    }
    public abstract partial class RecipientInfo
    {
        internal RecipientInfo() { }
        public abstract byte[] EncryptedKey { get; }
        public abstract System.Security.Cryptography.Pkcs.AlgorithmIdentifier KeyEncryptionAlgorithm { get; }
        public abstract System.Security.Cryptography.Pkcs.SubjectIdentifier RecipientIdentifier { get; }
        public System.Security.Cryptography.Pkcs.RecipientInfoType Type { get { throw null; } }
        public abstract int Version { get; }
    }
    public sealed partial class RecipientInfoCollection : System.Collections.ICollection, System.Collections.IEnumerable
    {
        internal RecipientInfoCollection() { }
        public int Count { get { throw null; } }
        public System.Security.Cryptography.Pkcs.RecipientInfo this[int index] { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        public void CopyTo(System.Array array, int index) { }
        public void CopyTo(System.Security.Cryptography.Pkcs.RecipientInfo[] array, int index) { }
        public System.Security.Cryptography.Pkcs.RecipientInfoEnumerator GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }
    public sealed partial class RecipientInfoEnumerator : System.Collections.IEnumerator
    {
        internal RecipientInfoEnumerator() { }
        public System.Security.Cryptography.Pkcs.RecipientInfo Current { get { throw null; } }
        object System.Collections.IEnumerator.Current { get { throw null; } }
        public bool MoveNext() { throw null; }
        public void Reset() { }
    }
    public enum RecipientInfoType
    {
        Unknown = 0,
        KeyTransport = 1,
        KeyAgreement = 2,
    }
    public sealed partial class Rfc3161TimestampRequest
    {
        internal Rfc3161TimestampRequest() { }
        public bool HasExtensions { get { throw null; } }
        public System.Security.Cryptography.Oid HashAlgorithmId { get { throw null; } }
        public System.Security.Cryptography.Oid RequestedPolicyId { get { throw null; } }
        public bool RequestSignerCertificate { get { throw null; } }
        public int Version { get { throw null; } }
        public static System.Security.Cryptography.Pkcs.Rfc3161TimestampRequest CreateFromData(System.ReadOnlySpan<byte> data, System.Security.Cryptography.HashAlgorithmName hashAlgorithm, System.Security.Cryptography.Oid requestedPolicyId = null, System.ReadOnlyMemory<byte>? nonce = default(System.ReadOnlyMemory<byte>?), bool requestSignerCertificates = false, System.Security.Cryptography.X509Certificates.X509ExtensionCollection extensions = null) { throw null; }
        public static System.Security.Cryptography.Pkcs.Rfc3161TimestampRequest CreateFromHash(System.ReadOnlyMemory<byte> hash, System.Security.Cryptography.HashAlgorithmName hashAlgorithm, System.Security.Cryptography.Oid requestedPolicyId = null, System.ReadOnlyMemory<byte>? nonce = default(System.ReadOnlyMemory<byte>?), bool requestSignerCertificates = false, System.Security.Cryptography.X509Certificates.X509ExtensionCollection extensions = null) { throw null; }
        public static System.Security.Cryptography.Pkcs.Rfc3161TimestampRequest CreateFromHash(System.ReadOnlyMemory<byte> hash, System.Security.Cryptography.Oid hashAlgorithmId, System.Security.Cryptography.Oid requestedPolicyId = null, System.ReadOnlyMemory<byte>? nonce = default(System.ReadOnlyMemory<byte>?), bool requestSignerCertificates = false, System.Security.Cryptography.X509Certificates.X509ExtensionCollection extensions = null) { throw null; }
        public static System.Security.Cryptography.Pkcs.Rfc3161TimestampRequest CreateFromSignerInfo(System.Security.Cryptography.Pkcs.SignerInfo signerInfo, System.Security.Cryptography.HashAlgorithmName hashAlgorithm, System.Security.Cryptography.Oid requestedPolicyId = null, System.ReadOnlyMemory<byte>? nonce = default(System.ReadOnlyMemory<byte>?), bool requestSignerCertificates = false, System.Security.Cryptography.X509Certificates.X509ExtensionCollection extensions = null) { throw null; }
        public byte[] Encode() { throw null; }
        public System.Security.Cryptography.X509Certificates.X509ExtensionCollection GetExtensions() { throw null; }
        public System.ReadOnlyMemory<byte> GetMessageHash() { throw null; }
        public System.ReadOnlyMemory<byte>? GetNonce() { throw null; }
        public System.Security.Cryptography.Pkcs.Rfc3161TimestampToken ProcessResponse(System.ReadOnlyMemory<byte> responseBytes, out int bytesConsumed) { throw null; }
        public static bool TryDecode(System.ReadOnlyMemory<byte> encodedBytes, out System.Security.Cryptography.Pkcs.Rfc3161TimestampRequest request, out int bytesConsumed) { throw null; }
        public bool TryEncode(System.Span<byte> destination, out int bytesWritten) { throw null; }
    }
    public sealed partial class Rfc3161TimestampToken
    {
        internal Rfc3161TimestampToken() { }
        public System.Security.Cryptography.Pkcs.Rfc3161TimestampTokenInfo TokenInfo { get { throw null; } }
        public System.Security.Cryptography.Pkcs.SignedCms AsSignedCms() { throw null; }
        public static bool TryDecode(System.ReadOnlyMemory<byte> encodedBytes, out System.Security.Cryptography.Pkcs.Rfc3161TimestampToken token, out int bytesConsumed) { throw null; }
        public bool VerifySignatureForData(System.ReadOnlySpan<byte> data, out System.Security.Cryptography.X509Certificates.X509Certificate2 signerCertificate, System.Security.Cryptography.X509Certificates.X509Certificate2Collection extraCandidates = null) { throw null; }
        public bool VerifySignatureForHash(System.ReadOnlySpan<byte> hash, System.Security.Cryptography.HashAlgorithmName hashAlgorithm, out System.Security.Cryptography.X509Certificates.X509Certificate2 signerCertificate, System.Security.Cryptography.X509Certificates.X509Certificate2Collection extraCandidates = null) { throw null; }
        public bool VerifySignatureForHash(System.ReadOnlySpan<byte> hash, System.Security.Cryptography.Oid hashAlgorithmId, out System.Security.Cryptography.X509Certificates.X509Certificate2 signerCertificate, System.Security.Cryptography.X509Certificates.X509Certificate2Collection extraCandidates = null) { throw null; }
        public bool VerifySignatureForSignerInfo(System.Security.Cryptography.Pkcs.SignerInfo signerInfo, out System.Security.Cryptography.X509Certificates.X509Certificate2 signerCertificate, System.Security.Cryptography.X509Certificates.X509Certificate2Collection extraCandidates = null) { throw null; }
    }
    public sealed partial class Rfc3161TimestampTokenInfo
    {
        public Rfc3161TimestampTokenInfo(System.Security.Cryptography.Oid policyId, System.Security.Cryptography.Oid hashAlgorithmId, System.ReadOnlyMemory<byte> messageHash, System.ReadOnlyMemory<byte> serialNumber, System.DateTimeOffset timestamp, long? accuracyInMicroseconds = default(long?), bool isOrdering = false, System.ReadOnlyMemory<byte>? nonce = default(System.ReadOnlyMemory<byte>?), System.ReadOnlyMemory<byte>? timestampAuthorityName = default(System.ReadOnlyMemory<byte>?), System.Security.Cryptography.X509Certificates.X509ExtensionCollection extensions = null) { }
        public long? AccuracyInMicroseconds { get { throw null; } }
        public bool HasExtensions { get { throw null; } }
        public System.Security.Cryptography.Oid HashAlgorithmId { get { throw null; } }
        public bool IsOrdering { get { throw null; } }
        public System.Security.Cryptography.Oid PolicyId { get { throw null; } }
        public System.DateTimeOffset Timestamp { get { throw null; } }
        public int Version { get { throw null; } }
        public byte[] Encode() { throw null; }
        public System.Security.Cryptography.X509Certificates.X509ExtensionCollection GetExtensions() { throw null; }
        public System.ReadOnlyMemory<byte> GetMessageHash() { throw null; }
        public System.ReadOnlyMemory<byte>? GetNonce() { throw null; }
        public System.ReadOnlyMemory<byte> GetSerialNumber() { throw null; }
        public System.ReadOnlyMemory<byte>? GetTimestampAuthorityName() { throw null; }
        public static bool TryDecode(System.ReadOnlyMemory<byte> encodedBytes, out System.Security.Cryptography.Pkcs.Rfc3161TimestampTokenInfo timestampTokenInfo, out int bytesConsumed) { throw null; }
        public bool TryEncode(System.Span<byte> destination, out int bytesWritten) { throw null; }
    }
    public sealed partial class SignedCms
    {
        public SignedCms() { }
        public SignedCms(System.Security.Cryptography.Pkcs.ContentInfo contentInfo) { }
        public SignedCms(System.Security.Cryptography.Pkcs.ContentInfo contentInfo, bool detached) { }
        public SignedCms(System.Security.Cryptography.Pkcs.SubjectIdentifierType signerIdentifierType) { }
        public SignedCms(System.Security.Cryptography.Pkcs.SubjectIdentifierType signerIdentifierType, System.Security.Cryptography.Pkcs.ContentInfo contentInfo) { }
        public SignedCms(System.Security.Cryptography.Pkcs.SubjectIdentifierType signerIdentifierType, System.Security.Cryptography.Pkcs.ContentInfo contentInfo, bool detached) { }
        public System.Security.Cryptography.X509Certificates.X509Certificate2Collection Certificates { get { throw null; } }
        public System.Security.Cryptography.Pkcs.ContentInfo ContentInfo { get { throw null; } }
        public bool Detached { get { throw null; } }
        public System.Security.Cryptography.Pkcs.SignerInfoCollection SignerInfos { get { throw null; } }
        public int Version { get { throw null; } }
        public void CheckHash() { }
        public void CheckSignature(bool verifySignatureOnly) { }
        public void CheckSignature(System.Security.Cryptography.X509Certificates.X509Certificate2Collection extraStore, bool verifySignatureOnly) { }
        public void ComputeSignature() { }
        public void ComputeSignature(System.Security.Cryptography.Pkcs.CmsSigner signer) { }
        public void ComputeSignature(System.Security.Cryptography.Pkcs.CmsSigner signer, bool silent) { }
        public void Decode(byte[] encodedMessage) { }
        public byte[] Encode() { throw null; }
        public void RemoveSignature(int index) { }
        public void RemoveSignature(System.Security.Cryptography.Pkcs.SignerInfo signerInfo) { }
    }
    public sealed partial class SignerInfo
    {
        internal SignerInfo() { }
        public System.Security.Cryptography.X509Certificates.X509Certificate2 Certificate { get { throw null; } }
        public System.Security.Cryptography.Pkcs.SignerInfoCollection CounterSignerInfos { get { throw null; } }
        public System.Security.Cryptography.Oid DigestAlgorithm { get { throw null; } }
        public System.Security.Cryptography.Oid SignatureAlgorithm { get { throw null; } }
        public System.Security.Cryptography.CryptographicAttributeObjectCollection SignedAttributes { get { throw null; } }
        public System.Security.Cryptography.Pkcs.SubjectIdentifier SignerIdentifier { get { throw null; } }
        public System.Security.Cryptography.CryptographicAttributeObjectCollection UnsignedAttributes { get { throw null; } }
        public int Version { get { throw null; } }
        public void CheckHash() { }
        public void CheckSignature(bool verifySignatureOnly) { }
        public void CheckSignature(System.Security.Cryptography.X509Certificates.X509Certificate2Collection extraStore, bool verifySignatureOnly) { }
        public void ComputeCounterSignature() { }
        public void ComputeCounterSignature(System.Security.Cryptography.Pkcs.CmsSigner signer) { }
        public byte[] GetSignature() { throw null; }
        public void RemoveCounterSignature(int index) { }
        public void RemoveCounterSignature(System.Security.Cryptography.Pkcs.SignerInfo counterSignerInfo) { }
    }
    public sealed partial class SignerInfoCollection : System.Collections.ICollection, System.Collections.IEnumerable
    {
        internal SignerInfoCollection() { }
        public int Count { get { throw null; } }
        public bool IsSynchronized { get { throw null; } }
        public System.Security.Cryptography.Pkcs.SignerInfo this[int index] { get { throw null; } }
        public object SyncRoot { get { throw null; } }
        public void CopyTo(System.Array array, int index) { }
        public void CopyTo(System.Security.Cryptography.Pkcs.SignerInfo[] array, int index) { }
        public System.Security.Cryptography.Pkcs.SignerInfoEnumerator GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }
    public sealed partial class SignerInfoEnumerator : System.Collections.IEnumerator
    {
        internal SignerInfoEnumerator() { }
        public System.Security.Cryptography.Pkcs.SignerInfo Current { get { throw null; } }
        object System.Collections.IEnumerator.Current { get { throw null; } }
        public bool MoveNext() { throw null; }
        public void Reset() { }
    }
    public sealed partial class SubjectIdentifier
    {
        internal SubjectIdentifier() { }
        public System.Security.Cryptography.Pkcs.SubjectIdentifierType Type { get { throw null; } }
        public object Value { get { throw null; } }
    }
    public sealed partial class SubjectIdentifierOrKey
    {
        internal SubjectIdentifierOrKey() { }
        public System.Security.Cryptography.Pkcs.SubjectIdentifierOrKeyType Type { get { throw null; } }
        public object Value { get { throw null; } }
    }
    public enum SubjectIdentifierOrKeyType
    {
        Unknown = 0,
        IssuerAndSerialNumber = 1,
        SubjectKeyIdentifier = 2,
        PublicKeyInfo = 3,
    }
    public enum SubjectIdentifierType
    {
        Unknown = 0,
        IssuerAndSerialNumber = 1,
        SubjectKeyIdentifier = 2,
        NoSignature = 3,
    }
}
namespace System.Security.Cryptography.Xml
{
    public partial struct X509IssuerSerial
    {
        private object _dummy;
        public string IssuerName { get { throw null; } set { } }
        public string SerialNumber { get { throw null; } set { } }
    }
}

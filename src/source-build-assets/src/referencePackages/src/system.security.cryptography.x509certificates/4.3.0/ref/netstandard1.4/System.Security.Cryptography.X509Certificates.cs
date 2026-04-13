// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.X509Certificates")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Cryptography.X509Certificates")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.X509Certificates")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Win32.SafeHandles
{
    public sealed partial class SafeX509ChainHandle : System.Runtime.InteropServices.SafeHandle
    {
        internal SafeX509ChainHandle() : base(default, default) { }

        public override bool IsInvalid { get { throw null; } }

        protected override bool ReleaseHandle() { throw null; }
    }
}

namespace System.Security.Cryptography.X509Certificates
{
    public static partial class ECDsaCertificateExtensions
    {
        public static ECDsa GetECDsaPrivateKey(this X509Certificate2 certificate) { throw null; }

        public static ECDsa GetECDsaPublicKey(this X509Certificate2 certificate) { throw null; }
    }

    [Flags]
    public enum OpenFlags
    {
        ReadOnly = 0,
        ReadWrite = 1,
        MaxAllowed = 2,
        OpenExistingOnly = 4,
        IncludeArchived = 8
    }

    public sealed partial class PublicKey
    {
        public PublicKey(Oid oid, AsnEncodedData parameters, AsnEncodedData keyValue) { }

        public AsnEncodedData EncodedKeyValue { get { throw null; } }

        public AsnEncodedData EncodedParameters { get { throw null; } }

        public Oid Oid { get { throw null; } }
    }

    public static partial class RSACertificateExtensions
    {
        public static RSA GetRSAPrivateKey(this X509Certificate2 certificate) { throw null; }

        public static RSA GetRSAPublicKey(this X509Certificate2 certificate) { throw null; }
    }

    public enum StoreLocation
    {
        CurrentUser = 1,
        LocalMachine = 2
    }

    public enum StoreName
    {
        AddressBook = 1,
        AuthRoot = 2,
        CertificateAuthority = 3,
        Disallowed = 4,
        My = 5,
        Root = 6,
        TrustedPeople = 7,
        TrustedPublisher = 8
    }

    public sealed partial class X500DistinguishedName : AsnEncodedData
    {
        public X500DistinguishedName(byte[] encodedDistinguishedName) { }

        public X500DistinguishedName(AsnEncodedData encodedDistinguishedName) { }

        public X500DistinguishedName(X500DistinguishedName distinguishedName) { }

        public X500DistinguishedName(string distinguishedName, X500DistinguishedNameFlags flag) { }

        public X500DistinguishedName(string distinguishedName) { }

        public string Name { get { throw null; } }

        public string Decode(X500DistinguishedNameFlags flag) { throw null; }

        public override string Format(bool multiLine) { throw null; }
    }

    [Flags]
    public enum X500DistinguishedNameFlags
    {
        None = 0,
        Reversed = 1,
        UseSemicolons = 16,
        DoNotUsePlusSign = 32,
        DoNotUseQuotes = 64,
        UseCommas = 128,
        UseNewLines = 256,
        UseUTF8Encoding = 4096,
        UseT61Encoding = 8192,
        ForceUTF8Encoding = 16384
    }

    public sealed partial class X509BasicConstraintsExtension : X509Extension
    {
        public X509BasicConstraintsExtension() { }

        public X509BasicConstraintsExtension(bool certificateAuthority, bool hasPathLengthConstraint, int pathLengthConstraint, bool critical) { }

        public X509BasicConstraintsExtension(AsnEncodedData encodedBasicConstraints, bool critical) { }

        public bool CertificateAuthority { get { throw null; } }

        public bool HasPathLengthConstraint { get { throw null; } }

        public int PathLengthConstraint { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public partial class X509Certificate : IDisposable
    {
        public X509Certificate() { }

        public X509Certificate(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags) { }

        public X509Certificate(byte[] rawData, string password) { }

        public X509Certificate(byte[] data) { }

        public X509Certificate(IntPtr handle) { }

        public X509Certificate(string fileName, string password, X509KeyStorageFlags keyStorageFlags) { }

        public X509Certificate(string fileName, string password) { }

        public X509Certificate(string fileName) { }

        public IntPtr Handle { get { throw null; } }

        public string Issuer { get { throw null; } }

        public string Subject { get { throw null; } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public override bool Equals(object obj) { throw null; }

        public virtual bool Equals(X509Certificate other) { throw null; }

        public virtual byte[] Export(X509ContentType contentType, string password) { throw null; }

        public virtual byte[] Export(X509ContentType contentType) { throw null; }

        public virtual byte[] GetCertHash() { throw null; }

        public virtual string GetFormat() { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual string GetKeyAlgorithm() { throw null; }

        public virtual byte[] GetKeyAlgorithmParameters() { throw null; }

        public virtual string GetKeyAlgorithmParametersString() { throw null; }

        public virtual byte[] GetPublicKey() { throw null; }

        public virtual byte[] GetSerialNumber() { throw null; }

        public override string ToString() { throw null; }

        public virtual string ToString(bool fVerbose) { throw null; }
    }

    public partial class X509Certificate2 : X509Certificate
    {
        public X509Certificate2() { }

        public X509Certificate2(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags) { }

        public X509Certificate2(byte[] rawData, string password) { }

        public X509Certificate2(byte[] rawData) { }

        public X509Certificate2(IntPtr handle) { }

        public X509Certificate2(string fileName, string password, X509KeyStorageFlags keyStorageFlags) { }

        public X509Certificate2(string fileName, string password) { }

        public X509Certificate2(string fileName) { }

        public bool Archived { get { throw null; } set { } }

        public X509ExtensionCollection Extensions { get { throw null; } }

        public string FriendlyName { get { throw null; } set { } }

        public bool HasPrivateKey { get { throw null; } }

        public X500DistinguishedName IssuerName { get { throw null; } }

        public DateTime NotAfter { get { throw null; } }

        public DateTime NotBefore { get { throw null; } }

        public PublicKey PublicKey { get { throw null; } }

        public byte[] RawData { get { throw null; } }

        public string SerialNumber { get { throw null; } }

        public Oid SignatureAlgorithm { get { throw null; } }

        public X500DistinguishedName SubjectName { get { throw null; } }

        public string Thumbprint { get { throw null; } }

        public int Version { get { throw null; } }

        public static X509ContentType GetCertContentType(byte[] rawData) { throw null; }

        public static X509ContentType GetCertContentType(string fileName) { throw null; }

        public string GetNameInfo(X509NameType nameType, bool forIssuer) { throw null; }

        public override string ToString() { throw null; }

        public override string ToString(bool verbose) { throw null; }
    }

    public partial class X509Certificate2Collection : X509CertificateCollection
    {
        public X509Certificate2Collection() { }

        public X509Certificate2Collection(X509Certificate2 certificate) { }

        public X509Certificate2Collection(X509Certificate2[] certificates) { }

        public X509Certificate2Collection(X509Certificate2Collection certificates) { }

        public new X509Certificate2 this[int index] { get { throw null; } set { } }

        public int Add(X509Certificate2 certificate) { throw null; }

        public void AddRange(X509Certificate2[] certificates) { }

        public void AddRange(X509Certificate2Collection certificates) { }

        public bool Contains(X509Certificate2 certificate) { throw null; }

        public byte[] Export(X509ContentType contentType, string password) { throw null; }

        public byte[] Export(X509ContentType contentType) { throw null; }

        public X509Certificate2Collection Find(X509FindType findType, object findValue, bool validOnly) { throw null; }

        public new X509Certificate2Enumerator GetEnumerator() { throw null; }

        public void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags) { }

        public void Import(byte[] rawData) { }

        public void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags) { }

        public void Import(string fileName) { }

        public void Insert(int index, X509Certificate2 certificate) { }

        public void Remove(X509Certificate2 certificate) { }

        public void RemoveRange(X509Certificate2[] certificates) { }

        public void RemoveRange(X509Certificate2Collection certificates) { }
    }

    public sealed partial class X509Certificate2Enumerator : Collections.IEnumerator
    {
        internal X509Certificate2Enumerator() { }

        public X509Certificate2 Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }

        bool Collections.IEnumerator.MoveNext() { throw null; }

        void Collections.IEnumerator.Reset() { }
    }

    public partial class X509CertificateCollection : Collections.ICollection, Collections.IEnumerable, Collections.IList
    {
        public X509CertificateCollection() { }

        public X509CertificateCollection(X509Certificate[] value) { }

        public X509CertificateCollection(X509CertificateCollection value) { }

        public int Count { get { throw null; } }

        public X509Certificate this[int index] { get { throw null; } set { } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        bool Collections.IList.IsFixedSize { get { throw null; } }

        bool Collections.IList.IsReadOnly { get { throw null; } }

        object System.Collections.IList.this[int index] { get { throw null; } set { } }

        public int Add(X509Certificate value) { throw null; }

        public void AddRange(X509Certificate[] value) { }

        public void AddRange(X509CertificateCollection value) { }

        public void Clear() { }

        public bool Contains(X509Certificate value) { throw null; }

        public void CopyTo(X509Certificate[] array, int index) { }

        public X509CertificateEnumerator GetEnumerator() { throw null; }

        public override int GetHashCode() { throw null; }

        public int IndexOf(X509Certificate value) { throw null; }

        public void Insert(int index, X509Certificate value) { }

        public void Remove(X509Certificate value) { }

        public void RemoveAt(int index) { }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        int Collections.IList.Add(object value) { throw null; }

        bool Collections.IList.Contains(object value) { throw null; }

        int Collections.IList.IndexOf(object value) { throw null; }

        void Collections.IList.Insert(int index, object value) { }

        void Collections.IList.Remove(object value) { }

        public partial class X509CertificateEnumerator : Collections.IEnumerator
        {
            public X509CertificateEnumerator(X509CertificateCollection mappings) { }

            public X509Certificate Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            public void Reset() { }

            bool Collections.IEnumerator.MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }
        }
    }

    public partial class X509Chain : IDisposable
    {
        public X509ChainElementCollection ChainElements { get { throw null; } }

        public X509ChainPolicy ChainPolicy { get { throw null; } set { } }

        public X509ChainStatus[] ChainStatus { get { throw null; } }

        public Microsoft.Win32.SafeHandles.SafeX509ChainHandle SafeHandle { get { throw null; } }

        public bool Build(X509Certificate2 certificate) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }
    }

    public partial class X509ChainElement
    {
        internal X509ChainElement() { }

        public X509Certificate2 Certificate { get { throw null; } }

        public X509ChainStatus[] ChainElementStatus { get { throw null; } }

        public string Information { get { throw null; } }
    }

    public sealed partial class X509ChainElementCollection : Collections.ICollection, Collections.IEnumerable
    {
        internal X509ChainElementCollection() { }

        public int Count { get { throw null; } }

        public X509ChainElement this[int index] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public void CopyTo(X509ChainElement[] array, int index) { }

        public X509ChainElementEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class X509ChainElementEnumerator : Collections.IEnumerator
    {
        internal X509ChainElementEnumerator() { }

        public X509ChainElement Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    public sealed partial class X509ChainPolicy
    {
        public OidCollection ApplicationPolicy { get { throw null; } }

        public OidCollection CertificatePolicy { get { throw null; } }

        public X509Certificate2Collection ExtraStore { get { throw null; } }

        public X509RevocationFlag RevocationFlag { get { throw null; } set { } }

        public X509RevocationMode RevocationMode { get { throw null; } set { } }

        public TimeSpan UrlRetrievalTimeout { get { throw null; } set { } }

        public X509VerificationFlags VerificationFlags { get { throw null; } set { } }

        public DateTime VerificationTime { get { throw null; } set { } }

        public void Reset() { }
    }

    public partial struct X509ChainStatus
    {
        public X509ChainStatusFlags Status { get { throw null; } set { } }

        public string StatusInformation { get { throw null; } set { } }
    }

    [Flags]
    public enum X509ChainStatusFlags
    {
        NoError = 0,
        NotTimeValid = 1,
        NotTimeNested = 2,
        Revoked = 4,
        NotSignatureValid = 8,
        NotValidForUsage = 16,
        UntrustedRoot = 32,
        RevocationStatusUnknown = 64,
        Cyclic = 128,
        InvalidExtension = 256,
        InvalidPolicyConstraints = 512,
        InvalidBasicConstraints = 1024,
        InvalidNameConstraints = 2048,
        HasNotSupportedNameConstraint = 4096,
        HasNotDefinedNameConstraint = 8192,
        HasNotPermittedNameConstraint = 16384,
        HasExcludedNameConstraint = 32768,
        PartialChain = 65536,
        CtlNotTimeValid = 131072,
        CtlNotSignatureValid = 262144,
        CtlNotValidForUsage = 524288,
        HasWeakSignature = 1048576,
        OfflineRevocation = 16777216,
        NoIssuanceChainPolicy = 33554432,
        ExplicitDistrust = 67108864,
        HasNotSupportedCriticalExtension = 134217728
    }

    public enum X509ContentType
    {
        Unknown = 0,
        Cert = 1,
        SerializedCert = 2,
        Pfx = 3,
        Pkcs12 = 3,
        SerializedStore = 4,
        Pkcs7 = 5,
        Authenticode = 6
    }

    public sealed partial class X509EnhancedKeyUsageExtension : X509Extension
    {
        public X509EnhancedKeyUsageExtension() { }

        public X509EnhancedKeyUsageExtension(AsnEncodedData encodedEnhancedKeyUsages, bool critical) { }

        public X509EnhancedKeyUsageExtension(OidCollection enhancedKeyUsages, bool critical) { }

        public OidCollection EnhancedKeyUsages { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public partial class X509Extension : AsnEncodedData
    {
        protected X509Extension() { }

        public X509Extension(AsnEncodedData encodedExtension, bool critical) { }

        public X509Extension(Oid oid, byte[] rawData, bool critical) { }

        public X509Extension(string oid, byte[] rawData, bool critical) { }

        public bool Critical { get { throw null; } set { } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public sealed partial class X509ExtensionCollection : Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public X509Extension this[int index] { get { throw null; } }

        public X509Extension this[string oid] { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public int Add(X509Extension extension) { throw null; }

        public void CopyTo(X509Extension[] array, int index) { }

        public X509ExtensionEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public sealed partial class X509ExtensionEnumerator : Collections.IEnumerator
    {
        internal X509ExtensionEnumerator() { }

        public X509Extension Current { get { throw null; } }

        object Collections.IEnumerator.Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }

    public enum X509FindType
    {
        FindByThumbprint = 0,
        FindBySubjectName = 1,
        FindBySubjectDistinguishedName = 2,
        FindByIssuerName = 3,
        FindByIssuerDistinguishedName = 4,
        FindBySerialNumber = 5,
        FindByTimeValid = 6,
        FindByTimeNotYetValid = 7,
        FindByTimeExpired = 8,
        FindByTemplateName = 9,
        FindByApplicationPolicy = 10,
        FindByCertificatePolicy = 11,
        FindByExtension = 12,
        FindByKeyUsage = 13,
        FindBySubjectKeyIdentifier = 14
    }

    [Flags]
    public enum X509KeyStorageFlags
    {
        DefaultKeySet = 0,
        UserKeySet = 1,
        MachineKeySet = 2,
        Exportable = 4,
        UserProtected = 8,
        PersistKeySet = 16
    }

    public sealed partial class X509KeyUsageExtension : X509Extension
    {
        public X509KeyUsageExtension() { }

        public X509KeyUsageExtension(AsnEncodedData encodedKeyUsage, bool critical) { }

        public X509KeyUsageExtension(X509KeyUsageFlags keyUsages, bool critical) { }

        public X509KeyUsageFlags KeyUsages { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    [Flags]
    public enum X509KeyUsageFlags
    {
        None = 0,
        EncipherOnly = 1,
        CrlSign = 2,
        KeyCertSign = 4,
        KeyAgreement = 8,
        DataEncipherment = 16,
        KeyEncipherment = 32,
        NonRepudiation = 64,
        DigitalSignature = 128,
        DecipherOnly = 32768
    }

    public enum X509NameType
    {
        SimpleName = 0,
        EmailName = 1,
        UpnName = 2,
        DnsName = 3,
        DnsFromAlternativeName = 4,
        UrlName = 5
    }

    public enum X509RevocationFlag
    {
        EndCertificateOnly = 0,
        EntireChain = 1,
        ExcludeRoot = 2
    }

    public enum X509RevocationMode
    {
        NoCheck = 0,
        Online = 1,
        Offline = 2
    }

    public sealed partial class X509Store : IDisposable
    {
        public X509Store() { }

        public X509Store(StoreName storeName, StoreLocation storeLocation) { }

        public X509Store(string storeName, StoreLocation storeLocation) { }

        public X509Certificate2Collection Certificates { get { throw null; } }

        public StoreLocation Location { get { throw null; } }

        public string Name { get { throw null; } }

        public void Add(X509Certificate2 certificate) { }

        public void Dispose() { }

        public void Open(OpenFlags flags) { }

        public void Remove(X509Certificate2 certificate) { }
    }

    public sealed partial class X509SubjectKeyIdentifierExtension : X509Extension
    {
        public X509SubjectKeyIdentifierExtension() { }

        public X509SubjectKeyIdentifierExtension(byte[] subjectKeyIdentifier, bool critical) { }

        public X509SubjectKeyIdentifierExtension(AsnEncodedData encodedSubjectKeyIdentifier, bool critical) { }

        public X509SubjectKeyIdentifierExtension(PublicKey key, bool critical) { }

        public X509SubjectKeyIdentifierExtension(PublicKey key, X509SubjectKeyIdentifierHashAlgorithm algorithm, bool critical) { }

        public X509SubjectKeyIdentifierExtension(string subjectKeyIdentifier, bool critical) { }

        public string SubjectKeyIdentifier { get { throw null; } }

        public override void CopyFrom(AsnEncodedData asnEncodedData) { }
    }

    public enum X509SubjectKeyIdentifierHashAlgorithm
    {
        Sha1 = 0,
        ShortSha1 = 1,
        CapiSha1 = 2
    }

    [Flags]
    public enum X509VerificationFlags
    {
        NoFlag = 0,
        IgnoreNotTimeValid = 1,
        IgnoreCtlNotTimeValid = 2,
        IgnoreNotTimeNested = 4,
        IgnoreInvalidBasicConstraints = 8,
        AllowUnknownCertificateAuthority = 16,
        IgnoreWrongUsage = 32,
        IgnoreInvalidName = 64,
        IgnoreInvalidPolicy = 128,
        IgnoreEndRevocationUnknown = 256,
        IgnoreCtlSignerRevocationUnknown = 512,
        IgnoreCertificateAuthorityRevocationUnknown = 1024,
        IgnoreRootRevocationUnknown = 2048,
        AllFlags = 4095
    }
}
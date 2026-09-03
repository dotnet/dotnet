// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Bcl.Cryptography")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsAotCompatible", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides support for some cryptographic primitives for .NET Framework and .NET Standard.")]
[assembly: System.Reflection.AssemblyFileVersion("10.0.326.7603")]
[assembly: System.Reflection.AssemblyInformationalVersion("10.0.3+c2435c3e0f46de784341ac3ed62863ce77e117b4")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Bcl.Cryptography")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/dotnet")]
[assembly: System.Reflection.AssemblyVersionAttribute("10.0.0.3")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.AesGcm))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.AuthenticationTagMismatchException))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.PbeEncryptionAlgorithm))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.PbeParameters))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.SP800108HmacCounterKdf))]
namespace System.Security.Cryptography
{
    [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public abstract partial class CompositeMLDsa : IDisposable
    {
        protected CompositeMLDsa(CompositeMLDsaAlgorithm algorithm) { }
        public CompositeMLDsaAlgorithm Algorithm { get { throw null; } }
        public static bool IsSupported { get { throw null; } }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public byte[] ExportCompositeMLDsaPrivateKey() { throw null; }
        public int ExportCompositeMLDsaPrivateKey(Span<byte> destination) { throw null; }
        protected abstract int ExportCompositeMLDsaPrivateKeyCore(Span<byte> destination);
        public byte[] ExportCompositeMLDsaPublicKey() { throw null; }
        public int ExportCompositeMLDsaPublicKey(Span<byte> destination) { throw null; }
        protected abstract int ExportCompositeMLDsaPublicKeyCore(Span<byte> destination);
        public byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }
        public byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }
        public byte[] ExportEncryptedPkcs8PrivateKey(string password, PbeParameters pbeParameters) { throw null; }
        public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }
        public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }
        public string ExportEncryptedPkcs8PrivateKeyPem(string password, PbeParameters pbeParameters) { throw null; }
        public byte[] ExportPkcs8PrivateKey() { throw null; }
        public string ExportPkcs8PrivateKeyPem() { throw null; }
        public byte[] ExportSubjectPublicKeyInfo() { throw null; }
        public string ExportSubjectPublicKeyInfoPem() { throw null; }
        public static CompositeMLDsa GenerateKey(CompositeMLDsaAlgorithm algorithm) { throw null; }
        public static CompositeMLDsa ImportCompositeMLDsaPrivateKey(CompositeMLDsaAlgorithm algorithm, byte[] source) { throw null; }
        public static CompositeMLDsa ImportCompositeMLDsaPrivateKey(CompositeMLDsaAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        public static CompositeMLDsa ImportCompositeMLDsaPublicKey(CompositeMLDsaAlgorithm algorithm, byte[] source) { throw null; }
        public static CompositeMLDsa ImportCompositeMLDsaPublicKey(CompositeMLDsaAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        public static CompositeMLDsa ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source) { throw null; }
        public static CompositeMLDsa ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source) { throw null; }
        public static CompositeMLDsa ImportEncryptedPkcs8PrivateKey(string password, byte[] source) { throw null; }
        public static CompositeMLDsa ImportFromEncryptedPem(ReadOnlySpan<char> source, ReadOnlySpan<byte> passwordBytes) { throw null; }
        public static CompositeMLDsa ImportFromEncryptedPem(ReadOnlySpan<char> source, ReadOnlySpan<char> password) { throw null; }
        public static CompositeMLDsa ImportFromEncryptedPem(string source, byte[] passwordBytes) { throw null; }
        public static CompositeMLDsa ImportFromEncryptedPem(string source, string password) { throw null; }
        public static CompositeMLDsa ImportFromPem(ReadOnlySpan<char> source) { throw null; }
        public static CompositeMLDsa ImportFromPem(string source) { throw null; }
        public static CompositeMLDsa ImportPkcs8PrivateKey(byte[] source) { throw null; }
        public static CompositeMLDsa ImportPkcs8PrivateKey(ReadOnlySpan<byte> source) { throw null; }
        public static CompositeMLDsa ImportSubjectPublicKeyInfo(byte[] source) { throw null; }
        public static CompositeMLDsa ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source) { throw null; }
        public static bool IsAlgorithmSupported(CompositeMLDsaAlgorithm algorithm) { throw null; }
        public byte[] SignData(byte[] data, byte[]? context = null) { throw null; }
        public int SignData(ReadOnlySpan<byte> data, Span<byte> destination, ReadOnlySpan<byte> context = default) { throw null; }
        protected abstract int SignDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, Span<byte> destination);
        public bool TryExportCompositeMLDsaPrivateKey(Span<byte> destination, out int bytesWritten) { throw null; }
        public bool TryExportCompositeMLDsaPublicKey(Span<byte> destination, out int bytesWritten) { throw null; }
        public bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        public bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        public bool TryExportEncryptedPkcs8PrivateKey(string password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        public bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten) { throw null; }
        protected abstract bool TryExportPkcs8PrivateKeyCore(Span<byte> destination, out int bytesWritten);
        public bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten) { throw null; }
        public bool VerifyData(byte[] data, byte[] signature, byte[]? context = null) { throw null; }
        public bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> context = default) { throw null; }
        protected abstract bool VerifyDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, ReadOnlySpan<byte> signature);
    }

    [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public sealed partial class CompositeMLDsaAlgorithm : IEquatable<CompositeMLDsaAlgorithm>
    {
        internal CompositeMLDsaAlgorithm() { }
        public int MaxSignatureSizeInBytes { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa44WithECDsaP256 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa44WithEd25519 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa44WithRSA2048Pkcs15 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa44WithRSA2048Pss { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa65WithECDsaBrainpoolP256r1 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa65WithECDsaP256 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa65WithECDsaP384 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa65WithEd25519 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa65WithRSA3072Pkcs15 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa65WithRSA3072Pss { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa65WithRSA4096Pkcs15 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa65WithRSA4096Pss { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa87WithECDsaBrainpoolP384r1 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa87WithECDsaP384 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa87WithECDsaP521 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa87WithEd448 { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa87WithRSA3072Pss { get { throw null; } }
        public static CompositeMLDsaAlgorithm MLDsa87WithRSA4096Pss { get { throw null; } }
        public string Name { get { throw null; } }
        public override bool Equals(object? obj) { throw null; }
        public bool Equals(CompositeMLDsaAlgorithm? other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(CompositeMLDsaAlgorithm? left, CompositeMLDsaAlgorithm? right) { throw null; }
        public static bool operator !=(CompositeMLDsaAlgorithm? left, CompositeMLDsaAlgorithm? right) { throw null; }
        public override string ToString() { throw null; }
    }

    [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public sealed partial class CompositeMLDsaCng : CompositeMLDsa
    {
        [Runtime.Versioning.SupportedOSPlatform("windows")]
        public CompositeMLDsaCng(CngKey key) : base(default!) { }
        protected override int ExportCompositeMLDsaPrivateKeyCore(Span<byte> destination) { throw null; }
        protected override int ExportCompositeMLDsaPublicKeyCore(Span<byte> destination) { throw null; }
        public CngKey GetKey() { throw null; }
        protected override int SignDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, Span<byte> destination) { throw null; }
        protected override bool TryExportPkcs8PrivateKeyCore(Span<byte> destination, out int bytesWritten) { throw null; }
        protected override bool VerifyDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, ReadOnlySpan<byte> signature) { throw null; }
    }

    public abstract partial class MLDsa : IDisposable
    {
        protected MLDsa(MLDsaAlgorithm algorithm) { }
        public MLDsaAlgorithm Algorithm { get { throw null; } }
        public static bool IsSupported { get { throw null; } }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportEncryptedPkcs8PrivateKey(string password, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportEncryptedPkcs8PrivateKeyPem(string password, PbeParameters pbeParameters) { throw null; }
        public byte[] ExportMLDsaPrivateKey() { throw null; }
        public void ExportMLDsaPrivateKey(Span<byte> destination) { }
        protected abstract void ExportMLDsaPrivateKeyCore(Span<byte> destination);
        public byte[] ExportMLDsaPrivateSeed() { throw null; }
        public void ExportMLDsaPrivateSeed(Span<byte> destination) { }
        protected abstract void ExportMLDsaPrivateSeedCore(Span<byte> destination);
        public byte[] ExportMLDsaPublicKey() { throw null; }
        public void ExportMLDsaPublicKey(Span<byte> destination) { }
        protected abstract void ExportMLDsaPublicKeyCore(Span<byte> destination);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportPkcs8PrivateKey() { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportPkcs8PrivateKeyPem() { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportSubjectPublicKeyInfo() { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportSubjectPublicKeyInfoPem() { throw null; }
        public static MLDsa GenerateKey(MLDsaAlgorithm algorithm) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportEncryptedPkcs8PrivateKey(string password, byte[] source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportFromEncryptedPem(ReadOnlySpan<char> source, ReadOnlySpan<byte> passwordBytes) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportFromEncryptedPem(ReadOnlySpan<char> source, ReadOnlySpan<char> password) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportFromEncryptedPem(string source, byte[] passwordBytes) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportFromEncryptedPem(string source, string password) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportFromPem(ReadOnlySpan<char> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportFromPem(string source) { throw null; }
        public static MLDsa ImportMLDsaPrivateKey(MLDsaAlgorithm algorithm, byte[] source) { throw null; }
        public static MLDsa ImportMLDsaPrivateKey(MLDsaAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        public static MLDsa ImportMLDsaPrivateSeed(MLDsaAlgorithm algorithm, byte[] source) { throw null; }
        public static MLDsa ImportMLDsaPrivateSeed(MLDsaAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        public static MLDsa ImportMLDsaPublicKey(MLDsaAlgorithm algorithm, byte[] source) { throw null; }
        public static MLDsa ImportMLDsaPublicKey(MLDsaAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportPkcs8PrivateKey(byte[] source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportPkcs8PrivateKey(ReadOnlySpan<byte> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportSubjectPublicKeyInfo(byte[] source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source) { throw null; }
        public byte[] SignData(byte[] data, byte[]? context = null) { throw null; }
        public void SignData(ReadOnlySpan<byte> data, Span<byte> destination, ReadOnlySpan<byte> context = default) { }
        protected abstract void SignDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, Span<byte> destination);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] SignMu(byte[] externalMu) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public void SignMu(ReadOnlySpan<byte> externalMu, Span<byte> destination) { }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] SignMu(ReadOnlySpan<byte> externalMu) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        protected abstract void SignMuCore(ReadOnlySpan<byte> externalMu, Span<byte> destination);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] SignPreHash(byte[] hash, string hashAlgorithmOid, byte[]? context = null) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public void SignPreHash(ReadOnlySpan<byte> hash, Span<byte> destination, string hashAlgorithmOid, ReadOnlySpan<byte> context = default) { }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        protected abstract void SignPreHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> context, string hashAlgorithmOid, Span<byte> destination);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportEncryptedPkcs8PrivateKey(string password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        protected abstract bool TryExportPkcs8PrivateKeyCore(Span<byte> destination, out int bytesWritten);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten) { throw null; }
        public bool VerifyData(byte[] data, byte[] signature, byte[]? context = null) { throw null; }
        public bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> context = default) { throw null; }
        protected abstract bool VerifyDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, ReadOnlySpan<byte> signature);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool VerifyMu(byte[] externalMu, byte[] signature) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool VerifyMu(ReadOnlySpan<byte> externalMu, ReadOnlySpan<byte> signature) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        protected abstract bool VerifyMuCore(ReadOnlySpan<byte> externalMu, ReadOnlySpan<byte> signature);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool VerifyPreHash(byte[] hash, byte[] signature, string hashAlgorithmOid, byte[]? context = null) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool VerifyPreHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, string hashAlgorithmOid, ReadOnlySpan<byte> context = default) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        protected abstract bool VerifyPreHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> context, string hashAlgorithmOid, ReadOnlySpan<byte> signature);
    }

    public sealed partial class MLDsaAlgorithm : IEquatable<MLDsaAlgorithm>
    {
        internal MLDsaAlgorithm() { }
        public static MLDsaAlgorithm MLDsa44 { get { throw null; } }
        public static MLDsaAlgorithm MLDsa65 { get { throw null; } }
        public static MLDsaAlgorithm MLDsa87 { get { throw null; } }
        public int MuSizeInBytes { get { throw null; } }
        public string Name { get { throw null; } }
        public int PrivateKeySizeInBytes { get { throw null; } }
        public int PrivateSeedSizeInBytes { get { throw null; } }
        public int PublicKeySizeInBytes { get { throw null; } }
        public int SignatureSizeInBytes { get { throw null; } }
        public override bool Equals(object? obj) { throw null; }
        public bool Equals(MLDsaAlgorithm? other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(MLDsaAlgorithm? left, MLDsaAlgorithm? right) { throw null; }
        public static bool operator !=(MLDsaAlgorithm? left, MLDsaAlgorithm? right) { throw null; }
        public override string ToString() { throw null; }
    }

    public sealed partial class MLDsaCng : MLDsa
    {
        [Runtime.Versioning.SupportedOSPlatform("windows")]
        public MLDsaCng(CngKey key) : base(default!) { }
        protected override void Dispose(bool disposing) { }
        protected override void ExportMLDsaPrivateKeyCore(Span<byte> destination) { }
        protected override void ExportMLDsaPrivateSeedCore(Span<byte> destination) { }
        protected override void ExportMLDsaPublicKeyCore(Span<byte> destination) { }
        public CngKey GetKey() { throw null; }
        protected override void SignDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, Span<byte> destination) { }
        protected override void SignMuCore(ReadOnlySpan<byte> mu, Span<byte> destination) { }
        protected override void SignPreHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> context, string hashAlgorithmOid, Span<byte> destination) { }
        protected override bool TryExportPkcs8PrivateKeyCore(Span<byte> destination, out int bytesWritten) { throw null; }
        protected override bool VerifyDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, ReadOnlySpan<byte> signature) { throw null; }
        protected override bool VerifyMuCore(ReadOnlySpan<byte> mu, ReadOnlySpan<byte> signature) { throw null; }
        protected override bool VerifyPreHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> context, string hashAlgorithmOid, ReadOnlySpan<byte> signature) { throw null; }
    }

    public abstract partial class MLKem : IDisposable
    {
        protected MLKem(MLKemAlgorithm algorithm) { }
        public MLKemAlgorithm Algorithm { get { throw null; } }
        public static bool IsSupported { get { throw null; } }
        public byte[] Decapsulate(byte[] ciphertext) { throw null; }
        public void Decapsulate(ReadOnlySpan<byte> ciphertext, Span<byte> sharedSecret) { }
        protected abstract void DecapsulateCore(ReadOnlySpan<byte> ciphertext, Span<byte> sharedSecret);
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public void Encapsulate(out byte[] ciphertext, out byte[] sharedSecret) { throw null; }
        public void Encapsulate(Span<byte> ciphertext, Span<byte> sharedSecret) { }
        protected abstract void EncapsulateCore(Span<byte> ciphertext, Span<byte> sharedSecret);
        public byte[] ExportDecapsulationKey() { throw null; }
        public void ExportDecapsulationKey(Span<byte> destination) { }
        protected abstract void ExportDecapsulationKeyCore(Span<byte> destination);
        public byte[] ExportEncapsulationKey() { throw null; }
        public void ExportEncapsulationKey(Span<byte> destination) { }
        protected abstract void ExportEncapsulationKeyCore(Span<byte> destination);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportEncryptedPkcs8PrivateKey(string password, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportEncryptedPkcs8PrivateKeyPem(string password, PbeParameters pbeParameters) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportPkcs8PrivateKey() { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportPkcs8PrivateKeyPem() { throw null; }
        public byte[] ExportPrivateSeed() { throw null; }
        public void ExportPrivateSeed(Span<byte> destination) { }
        protected abstract void ExportPrivateSeedCore(Span<byte> destination);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public byte[] ExportSubjectPublicKeyInfo() { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public string ExportSubjectPublicKeyInfoPem() { throw null; }
        public static MLKem GenerateKey(MLKemAlgorithm algorithm) { throw null; }
        public static MLKem ImportDecapsulationKey(MLKemAlgorithm algorithm, byte[] source) { throw null; }
        public static MLKem ImportDecapsulationKey(MLKemAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        public static MLKem ImportEncapsulationKey(MLKemAlgorithm algorithm, byte[] source) { throw null; }
        public static MLKem ImportEncapsulationKey(MLKemAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportEncryptedPkcs8PrivateKey(string password, byte[] source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportFromEncryptedPem(ReadOnlySpan<char> source, ReadOnlySpan<byte> passwordBytes) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportFromEncryptedPem(ReadOnlySpan<char> source, ReadOnlySpan<char> password) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportFromEncryptedPem(string source, byte[] passwordBytes) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportFromEncryptedPem(string source, string password) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportFromPem(ReadOnlySpan<char> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportFromPem(string source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportPkcs8PrivateKey(byte[] source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportPkcs8PrivateKey(ReadOnlySpan<byte> source) { throw null; }
        public static MLKem ImportPrivateSeed(MLKemAlgorithm algorithm, byte[] source) { throw null; }
        public static MLKem ImportPrivateSeed(MLKemAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportSubjectPublicKeyInfo(byte[] source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportEncryptedPkcs8PrivateKey(string password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        protected abstract bool TryExportPkcs8PrivateKeyCore(Span<byte> destination, out int bytesWritten);
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten) { throw null; }
    }

    public sealed partial class MLKemAlgorithm : IEquatable<MLKemAlgorithm>
    {
        internal MLKemAlgorithm() { }
        public int CiphertextSizeInBytes { get { throw null; } }
        public int DecapsulationKeySizeInBytes { get { throw null; } }
        public int EncapsulationKeySizeInBytes { get { throw null; } }
        public static MLKemAlgorithm MLKem1024 { get { throw null; } }
        public static MLKemAlgorithm MLKem512 { get { throw null; } }
        public static MLKemAlgorithm MLKem768 { get { throw null; } }
        public string Name { get { throw null; } }
        public int PrivateSeedSizeInBytes { get { throw null; } }
        public int SharedSecretSizeInBytes { get { throw null; } }
        public override bool Equals(object? obj) { throw null; }
        public bool Equals(MLKemAlgorithm? other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(MLKemAlgorithm? left, MLKemAlgorithm? right) { throw null; }
        public static bool operator !=(MLKemAlgorithm? left, MLKemAlgorithm? right) { throw null; }
        public override string ToString() { throw null; }
    }

    public sealed partial class MLKemCng : MLKem
    {
        [Runtime.Versioning.SupportedOSPlatform("windows")]
        public MLKemCng(CngKey key) : base(default!) { }
        protected override void DecapsulateCore(ReadOnlySpan<byte> ciphertext, Span<byte> sharedSecret) { }
        protected override void Dispose(bool disposing) { }
        protected override void EncapsulateCore(Span<byte> ciphertext, Span<byte> sharedSecret) { }
        protected override void ExportDecapsulationKeyCore(Span<byte> destination) { }
        protected override void ExportEncapsulationKeyCore(Span<byte> destination) { }
        protected override void ExportPrivateSeedCore(Span<byte> destination) { }
        public CngKey GetKey() { throw null; }
        protected override bool TryExportPkcs8PrivateKeyCore(Span<byte> destination, out int bytesWritten) { throw null; }
    }

    [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public abstract partial class SlhDsa : IDisposable
    {
        protected SlhDsa(SlhDsaAlgorithm algorithm) { }
        public SlhDsaAlgorithm Algorithm { get { throw null; } }
        public static bool IsSupported { get { throw null; } }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }
        public byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }
        public byte[] ExportEncryptedPkcs8PrivateKey(string password, PbeParameters pbeParameters) { throw null; }
        public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters) { throw null; }
        public string ExportEncryptedPkcs8PrivateKeyPem(ReadOnlySpan<char> password, PbeParameters pbeParameters) { throw null; }
        public string ExportEncryptedPkcs8PrivateKeyPem(string password, PbeParameters pbeParameters) { throw null; }
        public byte[] ExportPkcs8PrivateKey() { throw null; }
        public string ExportPkcs8PrivateKeyPem() { throw null; }
        public byte[] ExportSlhDsaPrivateKey() { throw null; }
        public void ExportSlhDsaPrivateKey(Span<byte> destination) { }
        protected abstract void ExportSlhDsaPrivateKeyCore(Span<byte> destination);
        public byte[] ExportSlhDsaPublicKey() { throw null; }
        public void ExportSlhDsaPublicKey(Span<byte> destination) { }
        protected abstract void ExportSlhDsaPublicKeyCore(Span<byte> destination);
        public byte[] ExportSubjectPublicKeyInfo() { throw null; }
        public string ExportSubjectPublicKeyInfoPem() { throw null; }
        public static SlhDsa GenerateKey(SlhDsaAlgorithm algorithm) { throw null; }
        public static SlhDsa ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source) { throw null; }
        public static SlhDsa ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source) { throw null; }
        public static SlhDsa ImportEncryptedPkcs8PrivateKey(string password, byte[] source) { throw null; }
        public static SlhDsa ImportFromEncryptedPem(ReadOnlySpan<char> source, ReadOnlySpan<byte> passwordBytes) { throw null; }
        public static SlhDsa ImportFromEncryptedPem(ReadOnlySpan<char> source, ReadOnlySpan<char> password) { throw null; }
        public static SlhDsa ImportFromEncryptedPem(string source, byte[] passwordBytes) { throw null; }
        public static SlhDsa ImportFromEncryptedPem(string source, string password) { throw null; }
        public static SlhDsa ImportFromPem(ReadOnlySpan<char> source) { throw null; }
        public static SlhDsa ImportFromPem(string source) { throw null; }
        public static SlhDsa ImportPkcs8PrivateKey(byte[] source) { throw null; }
        public static SlhDsa ImportPkcs8PrivateKey(ReadOnlySpan<byte> source) { throw null; }
        public static SlhDsa ImportSlhDsaPrivateKey(SlhDsaAlgorithm algorithm, byte[] source) { throw null; }
        public static SlhDsa ImportSlhDsaPrivateKey(SlhDsaAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        public static SlhDsa ImportSlhDsaPublicKey(SlhDsaAlgorithm algorithm, byte[] source) { throw null; }
        public static SlhDsa ImportSlhDsaPublicKey(SlhDsaAlgorithm algorithm, ReadOnlySpan<byte> source) { throw null; }
        public static SlhDsa ImportSubjectPublicKeyInfo(byte[] source) { throw null; }
        public static SlhDsa ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source) { throw null; }
        public byte[] SignData(byte[] data, byte[]? context = null) { throw null; }
        public void SignData(ReadOnlySpan<byte> data, Span<byte> destination, ReadOnlySpan<byte> context = default) { }
        protected abstract void SignDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, Span<byte> destination);
        public byte[] SignPreHash(byte[] hash, string hashAlgorithmOid, byte[]? context = null) { throw null; }
        public void SignPreHash(ReadOnlySpan<byte> hash, Span<byte> destination, string hashAlgorithmOid, ReadOnlySpan<byte> context = default) { }
        protected abstract void SignPreHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> context, string hashAlgorithmOid, Span<byte> destination);
        public bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        public bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        public bool TryExportEncryptedPkcs8PrivateKey(string password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten) { throw null; }
        public bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten) { throw null; }
        protected virtual bool TryExportPkcs8PrivateKeyCore(Span<byte> destination, out int bytesWritten) { throw null; }
        public bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten) { throw null; }
        public bool VerifyData(byte[] data, byte[] signature, byte[]? context = null) { throw null; }
        public bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> context = default) { throw null; }
        protected abstract bool VerifyDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, ReadOnlySpan<byte> signature);
        public bool VerifyPreHash(byte[] hash, byte[] signature, string hashAlgorithmOid, byte[]? context = null) { throw null; }
        public bool VerifyPreHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature, string hashAlgorithmOid, ReadOnlySpan<byte> context = default) { throw null; }
        protected abstract bool VerifyPreHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> context, string hashAlgorithmOid, ReadOnlySpan<byte> signature);
    }

    [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public sealed partial class SlhDsaAlgorithm : IEquatable<SlhDsaAlgorithm>
    {
        internal SlhDsaAlgorithm() { }
        public string Name { get { throw null; } }
        public int PrivateKeySizeInBytes { get { throw null; } }
        public int PublicKeySizeInBytes { get { throw null; } }
        public int SignatureSizeInBytes { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaSha2_128f { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaSha2_128s { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaSha2_192f { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaSha2_192s { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaSha2_256f { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaSha2_256s { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaShake128f { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaShake128s { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaShake192f { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaShake192s { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaShake256f { get { throw null; } }
        public static SlhDsaAlgorithm SlhDsaShake256s { get { throw null; } }
        public override bool Equals(object? obj) { throw null; }
        public bool Equals(SlhDsaAlgorithm? other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(SlhDsaAlgorithm? left, SlhDsaAlgorithm? right) { throw null; }
        public static bool operator !=(SlhDsaAlgorithm? left, SlhDsaAlgorithm? right) { throw null; }
        public override string ToString() { throw null; }
    }

    [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public sealed partial class SlhDsaCng : SlhDsa
    {
        [Runtime.Versioning.SupportedOSPlatform("windows")]
        public SlhDsaCng(CngKey key) : base(default!) { }
        protected override void ExportSlhDsaPrivateKeyCore(Span<byte> destination) { }
        protected override void ExportSlhDsaPublicKeyCore(Span<byte> destination) { }
        public CngKey GetKey() { throw null; }
        protected override void SignDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, Span<byte> destination) { }
        protected override void SignPreHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> context, string hashAlgorithmOid, Span<byte> destination) { }
        protected override bool TryExportPkcs8PrivateKeyCore(Span<byte> destination, out int bytesWritten) { throw null; }
        protected override bool VerifyDataCore(ReadOnlySpan<byte> data, ReadOnlySpan<byte> context, ReadOnlySpan<byte> signature) { throw null; }
        protected override bool VerifyPreHashCore(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> context, string hashAlgorithmOid, ReadOnlySpan<byte> signature) { throw null; }
    }
}

namespace System.Security.Cryptography.X509Certificates
{
    public sealed partial class Pkcs12LoaderLimits
    {
        public Pkcs12LoaderLimits() { }
        public Pkcs12LoaderLimits(Pkcs12LoaderLimits copyFrom) { }
        public static Pkcs12LoaderLimits DangerousNoLimits { get { throw null; } }
        public static Pkcs12LoaderLimits Defaults { get { throw null; } }
        public bool IgnoreEncryptedAuthSafes { get { throw null; } set { } }
        public bool IgnorePrivateKeys { get { throw null; } set { } }
        public int? IndividualKdfIterationLimit { get { throw null; } set { } }
        public bool IsReadOnly { get { throw null; } }
        public int? MacIterationLimit { get { throw null; } set { } }
        public int? MaxCertificates { get { throw null; } set { } }
        public int? MaxKeys { get { throw null; } set { } }
        public bool PreserveCertificateAlias { get { throw null; } set { } }
        public bool PreserveKeyName { get { throw null; } set { } }
        public bool PreserveStorageProvider { get { throw null; } set { } }
        public bool PreserveUnknownAttributes { get { throw null; } set { } }
        public int? TotalKdfIterationLimit { get { throw null; } set { } }
        public void MakeReadOnly() { }
    }

    public sealed partial class Pkcs12LoadLimitExceededException : CryptographicException
    {
        public Pkcs12LoadLimitExceededException(string propertyName) { }
    }

    public static partial class X509CertificateKeyAccessors
    {
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static X509Certificate2 CopyWithPrivateKey(this X509Certificate2 certificate, CompositeMLDsa privateKey) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static X509Certificate2 CopyWithPrivateKey(this X509Certificate2 certificate, MLDsa privateKey) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static X509Certificate2 CopyWithPrivateKey(this X509Certificate2 certificate, MLKem privateKey) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static X509Certificate2 CopyWithPrivateKey(this X509Certificate2 certificate, SlhDsa privateKey) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static CompositeMLDsa? GetCompositeMLDsaPrivateKey(this X509Certificate2 certificate) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static CompositeMLDsa? GetCompositeMLDsaPublicKey(this X509Certificate2 certificate) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa? GetMLDsaPrivateKey(this X509Certificate2 certificate) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLDsa? GetMLDsaPublicKey(this X509Certificate2 certificate) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem? GetMLKemPrivateKey(this X509Certificate2 certificate) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static MLKem? GetMLKemPublicKey(this X509Certificate2 certificate) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static SlhDsa? GetSlhDsaPrivateKey(this X509Certificate2 certificate) { throw null; }
        [Diagnostics.CodeAnalysis.Experimental("SYSLIB5006", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
        public static SlhDsa? GetSlhDsaPublicKey(this X509Certificate2 certificate) { throw null; }
    }

    [Runtime.Versioning.UnsupportedOSPlatform("browser")]
    public static partial class X509CertificateLoader
    {
        public static X509Certificate2 LoadCertificate(byte[] data) { throw null; }
        public static X509Certificate2 LoadCertificate(ReadOnlySpan<byte> data) { throw null; }
        public static X509Certificate2 LoadCertificateFromFile(string path) { throw null; }
        public static X509Certificate2 LoadPkcs12(byte[] data, string? password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet, Pkcs12LoaderLimits? loaderLimits = null) { throw null; }
        public static X509Certificate2 LoadPkcs12(ReadOnlySpan<byte> data, ReadOnlySpan<char> password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet, Pkcs12LoaderLimits? loaderLimits = null) { throw null; }
        public static X509Certificate2Collection LoadPkcs12Collection(byte[] data, string? password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet, Pkcs12LoaderLimits? loaderLimits = null) { throw null; }
        public static X509Certificate2Collection LoadPkcs12Collection(ReadOnlySpan<byte> data, ReadOnlySpan<char> password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet, Pkcs12LoaderLimits? loaderLimits = null) { throw null; }
        public static X509Certificate2Collection LoadPkcs12CollectionFromFile(string path, ReadOnlySpan<char> password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet, Pkcs12LoaderLimits? loaderLimits = null) { throw null; }
        public static X509Certificate2Collection LoadPkcs12CollectionFromFile(string path, string? password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet, Pkcs12LoaderLimits? loaderLimits = null) { throw null; }
        public static X509Certificate2 LoadPkcs12FromFile(string path, ReadOnlySpan<char> password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet, Pkcs12LoaderLimits? loaderLimits = null) { throw null; }
        public static X509Certificate2 LoadPkcs12FromFile(string path, string? password, X509KeyStorageFlags keyStorageFlags = X509KeyStorageFlags.DefaultKeySet, Pkcs12LoaderLimits? loaderLimits = null) { throw null; }
    }
}
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
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides support for some cryptographic primitives for .NET Framework and .NET Standard.")]
[assembly: System.Reflection.AssemblyFileVersion("9.0.24.52809")]
[assembly: System.Reflection.AssemblyInformationalVersion("9.0.0+9d5a6a9aa463d6d10b0b0ba6d5982cc82f363dc3")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Bcl.Cryptography")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("9.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.SP800108HmacCounterKdf))]
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
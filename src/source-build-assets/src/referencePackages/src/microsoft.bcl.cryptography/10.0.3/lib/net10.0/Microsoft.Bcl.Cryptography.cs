// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v10.0", FrameworkDisplayName = ".NET 10.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Bcl.Cryptography")]
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
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.CompositeMLDsa))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.CompositeMLDsaAlgorithm))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.CompositeMLDsaCng))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.MLDsa))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.MLDsaAlgorithm))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.MLDsaCng))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.MLKem))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.MLKemAlgorithm))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.MLKemCng))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.PbeEncryptionAlgorithm))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.PbeParameters))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.SlhDsa))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.SlhDsaAlgorithm))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.SlhDsaCng))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.SP800108HmacCounterKdf))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.Pkcs12LoaderLimits))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.Pkcs12LoadLimitExceededException))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509CertificateLoader))]
namespace System.Security.Cryptography.X509Certificates
{
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
}
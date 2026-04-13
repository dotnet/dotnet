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
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.Csp")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Cryptography.Csp")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.Csp")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Security.Cryptography
{
    public sealed partial class CspKeyContainerInfo
    {
        public CspKeyContainerInfo(CspParameters parameters) { }

        public bool Accessible { get { throw null; } }

        public bool Exportable { get { throw null; } }

        public bool HardwareDevice { get { throw null; } }

        public string KeyContainerName { get { throw null; } }

        public KeyNumber KeyNumber { get { throw null; } }

        public bool MachineKeyStore { get { throw null; } }

        public bool Protected { get { throw null; } }

        public string ProviderName { get { throw null; } }

        public int ProviderType { get { throw null; } }

        public bool RandomlyGenerated { get { throw null; } }

        public bool Removable { get { throw null; } }

        public string UniqueKeyContainerName { get { throw null; } }
    }

    public sealed partial class CspParameters
    {
        public string KeyContainerName;
        public int KeyNumber;
        public string ProviderName;
        public int ProviderType;
        public CspParameters() { }

        public CspParameters(int dwTypeIn, string strProviderNameIn, string strContainerNameIn) { }

        public CspParameters(int dwTypeIn, string strProviderNameIn) { }

        public CspParameters(int dwTypeIn) { }

        public CspProviderFlags Flags { get { throw null; } set { } }

        public IntPtr ParentWindowHandle { get { throw null; } set { } }
    }

    [Flags]
    public enum CspProviderFlags
    {
        NoFlags = 0,
        UseMachineKeyStore = 1,
        UseDefaultKeyContainer = 2,
        UseNonExportableKey = 4,
        UseExistingKey = 8,
        UseArchivableKey = 16,
        UseUserProtectedKey = 32,
        NoPrompt = 64,
        CreateEphemeralKey = 128
    }

    public partial interface ICspAsymmetricAlgorithm
    {
        CspKeyContainerInfo CspKeyContainerInfo { get; }

        byte[] ExportCspBlob(bool includePrivateParameters);
        void ImportCspBlob(byte[] rawData);
    }

    public enum KeyNumber
    {
        Exchange = 1,
        Signature = 2
    }

    public sealed partial class RSACryptoServiceProvider : RSA, ICspAsymmetricAlgorithm
    {
        public RSACryptoServiceProvider() { }

        public RSACryptoServiceProvider(int dwKeySize, CspParameters parameters) { }

        public RSACryptoServiceProvider(int dwKeySize) { }

        public RSACryptoServiceProvider(CspParameters parameters) { }

        public CspKeyContainerInfo CspKeyContainerInfo { get { throw null; } }

        public override int KeySize { get { throw null; } }

        public override KeySizes[] LegalKeySizes { get { throw null; } }

        public bool PersistKeyInCsp { get { throw null; } set { } }

        public bool PublicOnly { get { throw null; } }

        public static bool UseMachineKeyStore { get { throw null; } set { } }

        public byte[] Decrypt(byte[] rgb, bool fOAEP) { throw null; }

        public override byte[] Decrypt(byte[] data, RSAEncryptionPadding padding) { throw null; }

        protected override void Dispose(bool disposing) { }

        public byte[] Encrypt(byte[] rgb, bool fOAEP) { throw null; }

        public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding) { throw null; }

        public byte[] ExportCspBlob(bool includePrivateParameters) { throw null; }

        public override RSAParameters ExportParameters(bool includePrivateParameters) { throw null; }

        protected override byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm) { throw null; }

        protected override byte[] HashData(IO.Stream data, HashAlgorithmName hashAlgorithm) { throw null; }

        public void ImportCspBlob(byte[] keyBlob) { }

        public override void ImportParameters(RSAParameters parameters) { }

        public byte[] SignData(byte[] buffer, int offset, int count, object halg) { throw null; }

        public byte[] SignData(byte[] buffer, object halg) { throw null; }

        public byte[] SignData(IO.Stream inputStream, object halg) { throw null; }

        public override byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public byte[] SignHash(byte[] rgbHash, string str) { throw null; }

        public bool VerifyData(byte[] buffer, object halg, byte[] signature) { throw null; }

        public override bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature) { throw null; }
    }
}
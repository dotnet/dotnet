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
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.OpenSsl")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Cryptography.OpenSsl")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.OpenSsl")]
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
    public sealed partial class ECDsaOpenSsl : ECDsa
    {
        public ECDsaOpenSsl() { }

        public ECDsaOpenSsl(int keySize) { }

        public ECDsaOpenSsl(IntPtr handle) { }

        public ECDsaOpenSsl(ECCurve curve) { }

        public ECDsaOpenSsl(SafeEvpPKeyHandle pkeyHandle) { }

        public override int KeySize { get { throw null; } set { } }

        public override KeySizes[] LegalKeySizes { get { throw null; } }

        protected override void Dispose(bool disposing) { }

        public SafeEvpPKeyHandle DuplicateKeyHandle() { throw null; }

        public override ECParameters ExportExplicitParameters(bool includePrivateParameters) { throw null; }

        public override ECParameters ExportParameters(bool includePrivateParameters) { throw null; }

        public override void GenerateKey(ECCurve curve) { }

        protected override byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm) { throw null; }

        protected override byte[] HashData(IO.Stream data, HashAlgorithmName hashAlgorithm) { throw null; }

        public override void ImportParameters(ECParameters parameters) { }

        public override byte[] SignHash(byte[] hash) { throw null; }

        public override bool VerifyHash(byte[] hash, byte[] signature) { throw null; }
    }

    public sealed partial class RSAOpenSsl : RSA
    {
        public RSAOpenSsl() { }

        public RSAOpenSsl(int keySize) { }

        public RSAOpenSsl(IntPtr handle) { }

        public RSAOpenSsl(RSAParameters parameters) { }

        public RSAOpenSsl(SafeEvpPKeyHandle pkeyHandle) { }

        public override int KeySize { set { } }

        public override KeySizes[] LegalKeySizes { get { throw null; } }

        public override byte[] Decrypt(byte[] data, RSAEncryptionPadding padding) { throw null; }

        protected override void Dispose(bool disposing) { }

        public SafeEvpPKeyHandle DuplicateKeyHandle() { throw null; }

        public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding) { throw null; }

        public override RSAParameters ExportParameters(bool includePrivateParameters) { throw null; }

        protected override byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm) { throw null; }

        protected override byte[] HashData(IO.Stream data, HashAlgorithmName hashAlgorithm) { throw null; }

        public override void ImportParameters(RSAParameters parameters) { }

        public override byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public override bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }
    }

    public sealed partial class SafeEvpPKeyHandle : Runtime.InteropServices.SafeHandle
    {
        public SafeEvpPKeyHandle(IntPtr handle, bool ownsHandle) : base(default, default) { }

        public override bool IsInvalid { get { throw null; } }

        public SafeEvpPKeyHandle DuplicateHandle() { throw null; }

        protected override bool ReleaseHandle() { throw null; }
    }
}
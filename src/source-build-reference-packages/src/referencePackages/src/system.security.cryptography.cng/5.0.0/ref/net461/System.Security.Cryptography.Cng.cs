// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Security.Cryptography.Cng")]
[assembly: AssemblyDescription("System.Security.Cryptography.Cng")]
[assembly: AssemblyDefaultAlias("System.Security.Cryptography.Cng")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("5.0.20.51904")]
[assembly: AssemblyInformationalVersion("5.0.20.51904 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("5.0.0.0")]

[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngAlgorithm))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngAlgorithmGroup))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngExportPolicies))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKey))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyBlobFormat))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyCreationOptions))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyCreationParameters))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyHandleOpenOptions))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyOpenOptions))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngKeyUsages))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngProperty))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngPropertyCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngPropertyOptions))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngProvider))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngUIPolicy))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CngUIProtectionLevels))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.ECDiffieHellmanCng))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.ECDiffieHellmanCngPublicKey))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.ECDiffieHellmanKeyDerivationFunction))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.ECDsaCng))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.ECKeyXmlFormat))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.RSACng))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeNCryptHandle))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeNCryptKeyHandle))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeNCryptProviderHandle))]
[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeNCryptSecretHandle))]



namespace System.Security.Cryptography
{
    public sealed partial class AesCng : System.Security.Cryptography.Aes
    {
        public AesCng() { }
        public AesCng(string keyName) { }
        public AesCng(string keyName, System.Security.Cryptography.CngProvider provider) { }
        public AesCng(string keyName, System.Security.Cryptography.CngProvider provider, System.Security.Cryptography.CngKeyOpenOptions openOptions) { }
        public override byte[] Key { get { throw null; } set { } }
        public override int KeySize { get { throw null; } set { } }
        public override System.Security.Cryptography.ICryptoTransform CreateDecryptor() { throw null; }
        public override System.Security.Cryptography.ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV) { throw null; }
        public override System.Security.Cryptography.ICryptoTransform CreateEncryptor() { throw null; }
        public override System.Security.Cryptography.ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV) { throw null; }
        protected override void Dispose(bool disposing) { }
        public override void GenerateIV() { }
        public override void GenerateKey() { }
    }
    public sealed partial class DSACng : System.Security.Cryptography.DSA
    {
        public DSACng() { }
        public DSACng(int keySize) { }
        public DSACng(System.Security.Cryptography.CngKey key) { }
        public System.Security.Cryptography.CngKey Key { get { throw null; } }
        public override string KeyExchangeAlgorithm { get { throw null; } }
        public override System.Security.Cryptography.KeySizes[] LegalKeySizes { get { throw null; } }
        public override string SignatureAlgorithm { get { throw null; } }
        public override byte[] CreateSignature(byte[] rgbHash) { throw null; }
        protected override void Dispose(bool disposing) { }
        public override System.Security.Cryptography.DSAParameters ExportParameters(bool includePrivateParameters) { throw null; }
        public override void ImportParameters(System.Security.Cryptography.DSAParameters parameters) { }
        public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature) { throw null; }
    }
    public sealed partial class TripleDESCng : System.Security.Cryptography.TripleDES
    {
        public TripleDESCng() { }
        public TripleDESCng(string keyName) { }
        public TripleDESCng(string keyName, System.Security.Cryptography.CngProvider provider) { }
        public TripleDESCng(string keyName, System.Security.Cryptography.CngProvider provider, System.Security.Cryptography.CngKeyOpenOptions openOptions) { }
        public override byte[] Key { get { throw null; } set { } }
        public override int KeySize { get { throw null; } set { } }
        public override System.Security.Cryptography.ICryptoTransform CreateDecryptor() { throw null; }
        public override System.Security.Cryptography.ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV) { throw null; }
        public override System.Security.Cryptography.ICryptoTransform CreateEncryptor() { throw null; }
        public override System.Security.Cryptography.ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV) { throw null; }
        protected override void Dispose(bool disposing) { }
        public override void GenerateIV() { }
        public override void GenerateKey() { }
    }
}

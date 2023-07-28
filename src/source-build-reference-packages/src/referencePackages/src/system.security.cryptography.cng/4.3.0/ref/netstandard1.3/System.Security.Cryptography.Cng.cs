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
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.Cng")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Cryptography.Cng")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.Cng")]
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
namespace Microsoft.Win32.SafeHandles
{
    public abstract partial class SafeNCryptHandle : System.Runtime.InteropServices.SafeHandle
    {
        protected SafeNCryptHandle() : base(default, default) { }

        public override bool IsInvalid { get { throw null; } }

        protected override bool ReleaseHandle() { throw null; }

        protected abstract bool ReleaseNativeHandle();
    }

    public sealed partial class SafeNCryptKeyHandle : SafeNCryptHandle
    {
        protected override bool ReleaseNativeHandle() { throw null; }
    }

    public sealed partial class SafeNCryptProviderHandle : SafeNCryptHandle
    {
        protected override bool ReleaseNativeHandle() { throw null; }
    }

    public sealed partial class SafeNCryptSecretHandle : SafeNCryptHandle
    {
        protected override bool ReleaseNativeHandle() { throw null; }
    }
}

namespace System.Security.Cryptography
{
    public sealed partial class CngAlgorithm : IEquatable<CngAlgorithm>
    {
        public CngAlgorithm(string algorithm) { }

        public string Algorithm { get { throw null; } }

        public static CngAlgorithm ECDiffieHellmanP256 { get { throw null; } }

        public static CngAlgorithm ECDiffieHellmanP384 { get { throw null; } }

        public static CngAlgorithm ECDiffieHellmanP521 { get { throw null; } }

        public static CngAlgorithm ECDsaP256 { get { throw null; } }

        public static CngAlgorithm ECDsaP384 { get { throw null; } }

        public static CngAlgorithm ECDsaP521 { get { throw null; } }

        public static CngAlgorithm MD5 { get { throw null; } }

        public static CngAlgorithm Rsa { get { throw null; } }

        public static CngAlgorithm Sha1 { get { throw null; } }

        public static CngAlgorithm Sha256 { get { throw null; } }

        public static CngAlgorithm Sha384 { get { throw null; } }

        public static CngAlgorithm Sha512 { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(CngAlgorithm other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CngAlgorithm left, CngAlgorithm right) { throw null; }

        public static bool operator !=(CngAlgorithm left, CngAlgorithm right) { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class CngAlgorithmGroup : IEquatable<CngAlgorithmGroup>
    {
        public CngAlgorithmGroup(string algorithmGroup) { }

        public string AlgorithmGroup { get { throw null; } }

        public static CngAlgorithmGroup DiffieHellman { get { throw null; } }

        public static CngAlgorithmGroup Dsa { get { throw null; } }

        public static CngAlgorithmGroup ECDiffieHellman { get { throw null; } }

        public static CngAlgorithmGroup ECDsa { get { throw null; } }

        public static CngAlgorithmGroup Rsa { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(CngAlgorithmGroup other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CngAlgorithmGroup left, CngAlgorithmGroup right) { throw null; }

        public static bool operator !=(CngAlgorithmGroup left, CngAlgorithmGroup right) { throw null; }

        public override string ToString() { throw null; }
    }

    [Flags]
    public enum CngExportPolicies
    {
        None = 0,
        AllowExport = 1,
        AllowPlaintextExport = 2,
        AllowArchiving = 4,
        AllowPlaintextArchiving = 8
    }

    public sealed partial class CngKey : IDisposable
    {
        internal CngKey() { }

        public CngAlgorithm Algorithm { get { throw null; } }

        public CngAlgorithmGroup AlgorithmGroup { get { throw null; } }

        public CngExportPolicies ExportPolicy { get { throw null; } }

        public Microsoft.Win32.SafeHandles.SafeNCryptKeyHandle Handle { get { throw null; } }

        public bool IsEphemeral { get { throw null; } }

        public bool IsMachineKey { get { throw null; } }

        public string KeyName { get { throw null; } }

        public int KeySize { get { throw null; } }

        public CngKeyUsages KeyUsage { get { throw null; } }

        public IntPtr ParentWindowHandle { get { throw null; } set { } }

        public CngProvider Provider { get { throw null; } }

        public Microsoft.Win32.SafeHandles.SafeNCryptProviderHandle ProviderHandle { get { throw null; } }

        public CngUIPolicy UIPolicy { get { throw null; } }

        public string UniqueName { get { throw null; } }

        public static CngKey Create(CngAlgorithm algorithm, string keyName, CngKeyCreationParameters creationParameters) { throw null; }

        public static CngKey Create(CngAlgorithm algorithm, string keyName) { throw null; }

        public static CngKey Create(CngAlgorithm algorithm) { throw null; }

        public void Delete() { }

        public void Dispose() { }

        public static bool Exists(string keyName, CngProvider provider, CngKeyOpenOptions options) { throw null; }

        public static bool Exists(string keyName, CngProvider provider) { throw null; }

        public static bool Exists(string keyName) { throw null; }

        public byte[] Export(CngKeyBlobFormat format) { throw null; }

        public CngProperty GetProperty(string name, CngPropertyOptions options) { throw null; }

        public bool HasProperty(string name, CngPropertyOptions options) { throw null; }

        public static CngKey Import(byte[] keyBlob, CngKeyBlobFormat format, CngProvider provider) { throw null; }

        public static CngKey Import(byte[] keyBlob, CngKeyBlobFormat format) { throw null; }

        public static CngKey Open(Microsoft.Win32.SafeHandles.SafeNCryptKeyHandle keyHandle, CngKeyHandleOpenOptions keyHandleOpenOptions) { throw null; }

        public static CngKey Open(string keyName, CngProvider provider, CngKeyOpenOptions openOptions) { throw null; }

        public static CngKey Open(string keyName, CngProvider provider) { throw null; }

        public static CngKey Open(string keyName) { throw null; }

        public void SetProperty(CngProperty property) { }
    }

    public sealed partial class CngKeyBlobFormat : IEquatable<CngKeyBlobFormat>
    {
        public CngKeyBlobFormat(string format) { }

        public static CngKeyBlobFormat EccPrivateBlob { get { throw null; } }

        public static CngKeyBlobFormat EccPublicBlob { get { throw null; } }

        public string Format { get { throw null; } }

        public static CngKeyBlobFormat GenericPrivateBlob { get { throw null; } }

        public static CngKeyBlobFormat GenericPublicBlob { get { throw null; } }

        public static CngKeyBlobFormat OpaqueTransportBlob { get { throw null; } }

        public static CngKeyBlobFormat Pkcs8PrivateBlob { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(CngKeyBlobFormat other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CngKeyBlobFormat left, CngKeyBlobFormat right) { throw null; }

        public static bool operator !=(CngKeyBlobFormat left, CngKeyBlobFormat right) { throw null; }

        public override string ToString() { throw null; }
    }

    [Flags]
    public enum CngKeyCreationOptions
    {
        None = 0,
        MachineKey = 32,
        OverwriteExistingKey = 128
    }

    public sealed partial class CngKeyCreationParameters
    {
        public CngExportPolicies? ExportPolicy { get { throw null; } set { } }

        public CngKeyCreationOptions KeyCreationOptions { get { throw null; } set { } }

        public CngKeyUsages? KeyUsage { get { throw null; } set { } }

        public CngPropertyCollection Parameters { get { throw null; } }

        public IntPtr ParentWindowHandle { get { throw null; } set { } }

        public CngProvider Provider { get { throw null; } set { } }

        public CngUIPolicy UIPolicy { get { throw null; } set { } }
    }

    [Flags]
    public enum CngKeyHandleOpenOptions
    {
        None = 0,
        EphemeralKey = 1
    }

    [Flags]
    public enum CngKeyOpenOptions
    {
        None = 0,
        UserKey = 0,
        MachineKey = 32,
        Silent = 64
    }

    [Flags]
    public enum CngKeyUsages
    {
        None = 0,
        Decryption = 1,
        Signing = 2,
        KeyAgreement = 4,
        AllUsages = 16777215
    }

    public partial struct CngProperty : IEquatable<CngProperty>
    {
        public CngProperty(string name, byte[] value, CngPropertyOptions options) { }

        public string Name { get { throw null; } }

        public CngPropertyOptions Options { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(CngProperty other) { throw null; }

        public override int GetHashCode() { throw null; }

        public byte[] GetValue() { throw null; }

        public static bool operator ==(CngProperty left, CngProperty right) { throw null; }

        public static bool operator !=(CngProperty left, CngProperty right) { throw null; }
    }

    public sealed partial class CngPropertyCollection : Collections.ObjectModel.Collection<CngProperty>
    {
    }

    [Flags]
    public enum CngPropertyOptions
    {
        Persist = int.MinValue,
        None = 0,
        CustomProperty = 1073741824
    }

    public sealed partial class CngProvider : IEquatable<CngProvider>
    {
        public CngProvider(string provider) { }

        public static CngProvider MicrosoftSmartCardKeyStorageProvider { get { throw null; } }

        public static CngProvider MicrosoftSoftwareKeyStorageProvider { get { throw null; } }

        public string Provider { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(CngProvider other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CngProvider left, CngProvider right) { throw null; }

        public static bool operator !=(CngProvider left, CngProvider right) { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class CngUIPolicy
    {
        public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName, string description, string useContext, string creationTitle) { }

        public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName, string description, string useContext) { }

        public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName, string description) { }

        public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName) { }

        public CngUIPolicy(CngUIProtectionLevels protectionLevel) { }

        public string CreationTitle { get { throw null; } }

        public string Description { get { throw null; } }

        public string FriendlyName { get { throw null; } }

        public CngUIProtectionLevels ProtectionLevel { get { throw null; } }

        public string UseContext { get { throw null; } }
    }

    [Flags]
    public enum CngUIProtectionLevels
    {
        None = 0,
        ProtectKey = 1,
        ForceHighProtection = 2
    }

    public sealed partial class RSACng : RSA
    {
        public RSACng() { }

        public RSACng(int keySize) { }

        public RSACng(CngKey key) { }

        public CngKey Key { get { throw null; } }

        public override KeySizes[] LegalKeySizes { get { throw null; } }

        public override byte[] Decrypt(byte[] data, RSAEncryptionPadding padding) { throw null; }

        protected override void Dispose(bool disposing) { }

        public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding) { throw null; }

        public override RSAParameters ExportParameters(bool includePrivateParameters) { throw null; }

        protected override byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm) { throw null; }

        protected override byte[] HashData(IO.Stream data, HashAlgorithmName hashAlgorithm) { throw null; }

        public override void ImportParameters(RSAParameters parameters) { }

        public override byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public override bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }
    }
}
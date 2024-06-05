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
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.Primitives")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Cryptography.Primitives")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.Primitives")]
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
    public abstract partial class AsymmetricAlgorithm : IDisposable
    {
        protected int KeySizeValue;
        protected KeySizes[] LegalKeySizesValue;
        public virtual int KeySize { get { throw null; } set { } }

        public virtual KeySizes[] LegalKeySizes { get { throw null; } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }
    }

    public enum CipherMode
    {
        CBC = 1,
        ECB = 2, // CodeQL [SM02299] This just defines the API and doesn't represent usage // CodeQL [SM00395] This just defines the API and doesn't represent usage
        CTS = 5
    }

    public partial class CryptographicException : Exception
    {
        public CryptographicException() { }

        public CryptographicException(int hr) { }

        public CryptographicException(string message, Exception inner) { }

        public CryptographicException(string format, string insert) { }

        public CryptographicException(string message) { }
    }

    public partial class CryptoStream : IO.Stream, IDisposable
    {
        public CryptoStream(IO.Stream stream, ICryptoTransform transform, CryptoStreamMode mode) { }

        public override bool CanRead { get { throw null; } }

        public override bool CanSeek { get { throw null; } }

        public override bool CanWrite { get { throw null; } }

        public bool HasFlushedFinalBlock { get { throw null; } }

        public override long Length { get { throw null; } }

        public override long Position { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        public override void Flush() { }

        public override Threading.Tasks.Task FlushAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public void FlushFinalBlock() { }

        public override int Read(byte[] buffer, int offset, int count) { throw null; }

        public override Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }

        public override long Seek(long offset, IO.SeekOrigin origin) { throw null; }

        public override void SetLength(long value) { }

        public override void Write(byte[] buffer, int offset, int count) { }

        public override Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }
    }

    public enum CryptoStreamMode
    {
        Read = 0,
        Write = 1
    }

    public abstract partial class HashAlgorithm : IDisposable
    {
        public virtual int HashSize { get { throw null; } }

        public byte[] ComputeHash(byte[] buffer, int offset, int count) { throw null; }

        public byte[] ComputeHash(byte[] buffer) { throw null; }

        public byte[] ComputeHash(IO.Stream inputStream) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        protected abstract void HashCore(byte[] array, int ibStart, int cbSize);
        protected abstract byte[] HashFinal();
        public abstract void Initialize();
    }

    public partial struct HashAlgorithmName : IEquatable<HashAlgorithmName>
    {
        public HashAlgorithmName(string name) { }

        public static HashAlgorithmName MD5 { get { throw null; } }

        public string Name { get { throw null; } }

        public static HashAlgorithmName SHA1 { get { throw null; } }

        public static HashAlgorithmName SHA256 { get { throw null; } }

        public static HashAlgorithmName SHA384 { get { throw null; } }

        public static HashAlgorithmName SHA512 { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(HashAlgorithmName other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(HashAlgorithmName left, HashAlgorithmName right) { throw null; }

        public static bool operator !=(HashAlgorithmName left, HashAlgorithmName right) { throw null; }

        public override string ToString() { throw null; }
    }

    public abstract partial class HMAC : KeyedHashAlgorithm
    {
        public string HashName { get { throw null; } set { } }

        public override byte[] Key { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        protected override void HashCore(byte[] rgb, int ib, int cb) { }

        protected override byte[] HashFinal() { throw null; }

        public override void Initialize() { }
    }

    public partial interface ICryptoTransform : IDisposable
    {
        bool CanReuseTransform { get; }

        bool CanTransformMultipleBlocks { get; }

        int InputBlockSize { get; }

        int OutputBlockSize { get; }

        int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);
        byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);
    }

    public abstract partial class KeyedHashAlgorithm : HashAlgorithm
    {
        public virtual byte[] Key { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }
    }

    public sealed partial class KeySizes
    {
        public KeySizes(int minSize, int maxSize, int skipSize) { }

        public int MaxSize { get { throw null; } }

        public int MinSize { get { throw null; } }

        public int SkipSize { get { throw null; } }
    }

    public enum PaddingMode
    {
        None = 1,
        PKCS7 = 2,
        Zeros = 3
    }

    public abstract partial class SymmetricAlgorithm : IDisposable
    {
        protected int BlockSizeValue;
        protected byte[] IVValue;
        protected int KeySizeValue;
        protected byte[] KeyValue;
        protected KeySizes[] LegalBlockSizesValue;
        protected KeySizes[] LegalKeySizesValue;
        protected CipherMode ModeValue;
        protected PaddingMode PaddingValue;
        public virtual int BlockSize { get { throw null; } set { } }

        public virtual byte[] IV { get { throw null; } set { } }

        public virtual byte[] Key { get { throw null; } set { } }

        public virtual int KeySize { get { throw null; } set { } }

        public virtual KeySizes[] LegalBlockSizes { get { throw null; } }

        public virtual KeySizes[] LegalKeySizes { get { throw null; } }

        public virtual CipherMode Mode { get { throw null; } set { } }

        public virtual PaddingMode Padding { get { throw null; } set { } }

        public virtual ICryptoTransform CreateDecryptor() { throw null; }

        public abstract ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV);
        public virtual ICryptoTransform CreateEncryptor() { throw null; }

        public abstract ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV);
        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public abstract void GenerateIV();
        public abstract void GenerateKey();
    }
}
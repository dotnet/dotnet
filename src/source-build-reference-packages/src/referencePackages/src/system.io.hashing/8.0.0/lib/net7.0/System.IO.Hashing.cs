// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.IO.Hashing")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides non-cryptographic hash algorithms, such as CRC-32.\r\n\r\nCommonly Used Types:\r\nSystem.IO.Hashing.Crc32\r\nSystem.IO.Hashing.XxHash32")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53103")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.IO.Hashing")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.IO.Hashing
{
    public sealed partial class Crc32 : NonCryptographicHashAlgorithm
    {
        public Crc32() : base(default) { }

        public override void Append(ReadOnlySpan<byte> source) { }

        [CLSCompliant(false)]
        public uint GetCurrentHashAsUInt32() { throw null; }

        protected override void GetCurrentHashCore(Span<byte> destination) { }

        protected override void GetHashAndResetCore(Span<byte> destination) { }

        public static byte[] Hash(byte[] source) { throw null; }

        public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination) { throw null; }

        public static byte[] Hash(ReadOnlySpan<byte> source) { throw null; }

        [CLSCompliant(false)]
        public static uint HashToUInt32(ReadOnlySpan<byte> source) { throw null; }

        public override void Reset() { }

        public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten) { throw null; }
    }

    public sealed partial class Crc64 : NonCryptographicHashAlgorithm
    {
        public Crc64() : base(default) { }

        public override void Append(ReadOnlySpan<byte> source) { }

        [CLSCompliant(false)]
        public ulong GetCurrentHashAsUInt64() { throw null; }

        protected override void GetCurrentHashCore(Span<byte> destination) { }

        protected override void GetHashAndResetCore(Span<byte> destination) { }

        public static byte[] Hash(byte[] source) { throw null; }

        public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination) { throw null; }

        public static byte[] Hash(ReadOnlySpan<byte> source) { throw null; }

        [CLSCompliant(false)]
        public static ulong HashToUInt64(ReadOnlySpan<byte> source) { throw null; }

        public override void Reset() { }

        public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten) { throw null; }
    }

    public abstract partial class NonCryptographicHashAlgorithm
    {
        protected NonCryptographicHashAlgorithm(int hashLengthInBytes) { }

        public int HashLengthInBytes { get { throw null; } }

        public void Append(byte[] source) { }

        public void Append(Stream stream) { }

        public abstract void Append(ReadOnlySpan<byte> source);
        public Threading.Tasks.Task AppendAsync(Stream stream, Threading.CancellationToken cancellationToken = default) { throw null; }

        public byte[] GetCurrentHash() { throw null; }

        public int GetCurrentHash(Span<byte> destination) { throw null; }

        protected abstract void GetCurrentHashCore(Span<byte> destination);
        public byte[] GetHashAndReset() { throw null; }

        public int GetHashAndReset(Span<byte> destination) { throw null; }

        protected virtual void GetHashAndResetCore(Span<byte> destination) { }

        [Obsolete("Use GetCurrentHash() to retrieve the computed hash code.", true)]
        public override int GetHashCode() { throw null; }

        public abstract void Reset();
        public bool TryGetCurrentHash(Span<byte> destination, out int bytesWritten) { throw null; }

        public bool TryGetHashAndReset(Span<byte> destination, out int bytesWritten) { throw null; }
    }

    [Runtime.CompilerServices.SkipLocalsInit]
    public sealed partial class XxHash128 : NonCryptographicHashAlgorithm
    {
        public XxHash128() : base(default) { }

        public XxHash128(long seed) : base(default) { }

        public override void Append(ReadOnlySpan<byte> source) { }

        [CLSCompliant(false)]
        public UInt128 GetCurrentHashAsUInt128() { throw null; }

        protected override void GetCurrentHashCore(Span<byte> destination) { }

        public static byte[] Hash(byte[] source, long seed) { throw null; }

        public static byte[] Hash(byte[] source) { throw null; }

        public static byte[] Hash(ReadOnlySpan<byte> source, long seed = 0) { throw null; }

        public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination, long seed = 0) { throw null; }

        [CLSCompliant(false)]
        public static UInt128 HashToUInt128(ReadOnlySpan<byte> source, long seed = 0) { throw null; }

        public override void Reset() { }

        public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, long seed = 0) { throw null; }
    }

    [Runtime.CompilerServices.SkipLocalsInit]
    public sealed partial class XxHash3 : NonCryptographicHashAlgorithm
    {
        public XxHash3() : base(default) { }

        public XxHash3(long seed) : base(default) { }

        public override void Append(ReadOnlySpan<byte> source) { }

        [CLSCompliant(false)]
        public ulong GetCurrentHashAsUInt64() { throw null; }

        protected override void GetCurrentHashCore(Span<byte> destination) { }

        public static byte[] Hash(byte[] source, long seed) { throw null; }

        public static byte[] Hash(byte[] source) { throw null; }

        public static byte[] Hash(ReadOnlySpan<byte> source, long seed = 0) { throw null; }

        public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination, long seed = 0) { throw null; }

        [CLSCompliant(false)]
        public static ulong HashToUInt64(ReadOnlySpan<byte> source, long seed = 0) { throw null; }

        public override void Reset() { }

        public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, long seed = 0) { throw null; }
    }

    public sealed partial class XxHash32 : NonCryptographicHashAlgorithm
    {
        public XxHash32() : base(default) { }

        public XxHash32(int seed) : base(default) { }

        public override void Append(ReadOnlySpan<byte> source) { }

        [CLSCompliant(false)]
        public uint GetCurrentHashAsUInt32() { throw null; }

        protected override void GetCurrentHashCore(Span<byte> destination) { }

        public static byte[] Hash(byte[] source, int seed) { throw null; }

        public static byte[] Hash(byte[] source) { throw null; }

        public static byte[] Hash(ReadOnlySpan<byte> source, int seed = 0) { throw null; }

        public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination, int seed = 0) { throw null; }

        [CLSCompliant(false)]
        public static uint HashToUInt32(ReadOnlySpan<byte> source, int seed = 0) { throw null; }

        public override void Reset() { }

        public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, int seed = 0) { throw null; }
    }

    public sealed partial class XxHash64 : NonCryptographicHashAlgorithm
    {
        public XxHash64() : base(default) { }

        public XxHash64(long seed) : base(default) { }

        public override void Append(ReadOnlySpan<byte> source) { }

        [CLSCompliant(false)]
        public ulong GetCurrentHashAsUInt64() { throw null; }

        protected override void GetCurrentHashCore(Span<byte> destination) { }

        public static byte[] Hash(byte[] source, long seed) { throw null; }

        public static byte[] Hash(byte[] source) { throw null; }

        public static byte[] Hash(ReadOnlySpan<byte> source, long seed = 0) { throw null; }

        public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination, long seed = 0) { throw null; }

        [CLSCompliant(false)]
        public static ulong HashToUInt64(ReadOnlySpan<byte> source, long seed = 0) { throw null; }

        public override void Reset() { }

        public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, long seed = 0) { throw null; }
    }
}
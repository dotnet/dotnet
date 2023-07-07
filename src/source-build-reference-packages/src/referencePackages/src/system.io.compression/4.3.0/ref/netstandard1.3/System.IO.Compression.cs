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
[assembly: System.Reflection.AssemblyTitle("System.IO.Compression")]
[assembly: System.Reflection.AssemblyDescription("System.IO.Compression")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.IO.Compression")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24301.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24301.01. Commit Hash: 4ed15a98d1c957ae661d490ccfcfe77f4ed31d5a")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.IO.Compression
{
    public enum CompressionLevel
    {
        Optimal = 0,
        Fastest = 1,
        NoCompression = 2
    }

    public enum CompressionMode
    {
        Decompress = 0,
        Compress = 1
    }

    public partial class DeflateStream : Stream
    {
        public DeflateStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen) { }

        public DeflateStream(Stream stream, CompressionLevel compressionLevel) { }

        public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) { }

        public DeflateStream(Stream stream, CompressionMode mode) { }

        public Stream BaseStream { get { throw null; } }

        public override bool CanRead { get { throw null; } }

        public override bool CanSeek { get { throw null; } }

        public override bool CanWrite { get { throw null; } }

        public override long Length { get { throw null; } }

        public override long Position { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        public override void Flush() { }

        public override int Read(byte[] array, int offset, int count) { throw null; }

        public override Threading.Tasks.Task<int> ReadAsync(byte[] array, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }

        public override long Seek(long offset, SeekOrigin origin) { throw null; }

        public override void SetLength(long value) { }

        public override void Write(byte[] array, int offset, int count) { }

        public override Threading.Tasks.Task WriteAsync(byte[] array, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }
    }

    public partial class GZipStream : Stream
    {
        public GZipStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen) { }

        public GZipStream(Stream stream, CompressionLevel compressionLevel) { }

        public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) { }

        public GZipStream(Stream stream, CompressionMode mode) { }

        public Stream BaseStream { get { throw null; } }

        public override bool CanRead { get { throw null; } }

        public override bool CanSeek { get { throw null; } }

        public override bool CanWrite { get { throw null; } }

        public override long Length { get { throw null; } }

        public override long Position { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        public override void Flush() { }

        public override int Read(byte[] array, int offset, int count) { throw null; }

        public override Threading.Tasks.Task<int> ReadAsync(byte[] array, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }

        public override long Seek(long offset, SeekOrigin origin) { throw null; }

        public override void SetLength(long value) { }

        public override void Write(byte[] array, int offset, int count) { }

        public override Threading.Tasks.Task WriteAsync(byte[] array, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }
    }

    public partial class ZipArchive : IDisposable
    {
        public ZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen, Text.Encoding entryNameEncoding) { }

        public ZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen) { }

        public ZipArchive(Stream stream, ZipArchiveMode mode) { }

        public ZipArchive(Stream stream) { }

        public Collections.ObjectModel.ReadOnlyCollection<ZipArchiveEntry> Entries { get { throw null; } }

        public ZipArchiveMode Mode { get { throw null; } }

        public ZipArchiveEntry CreateEntry(string entryName, CompressionLevel compressionLevel) { throw null; }

        public ZipArchiveEntry CreateEntry(string entryName) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public ZipArchiveEntry GetEntry(string entryName) { throw null; }
    }

    public partial class ZipArchiveEntry
    {
        internal ZipArchiveEntry() { }

        public ZipArchive Archive { get { throw null; } }

        public long CompressedLength { get { throw null; } }

        public string FullName { get { throw null; } }

        public DateTimeOffset LastWriteTime { get { throw null; } set { } }

        public long Length { get { throw null; } }

        public string Name { get { throw null; } }

        public void Delete() { }

        public Stream Open() { throw null; }

        public override string ToString() { throw null; }
    }

    public enum ZipArchiveMode
    {
        Read = 0,
        Create = 1,
        Update = 2
    }
}
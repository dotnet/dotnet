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
[assembly: System.Reflection.AssemblyTitle("System.IO.FileSystem")]
[assembly: System.Reflection.AssemblyDescription("System.IO.FileSystem")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.IO.FileSystem")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Win32.SafeHandles
{
    public sealed partial class SafeFileHandle : System.Runtime.InteropServices.SafeHandle
    {
        public SafeFileHandle(System.IntPtr preexistingHandle, bool ownsHandle) : base(default, default) { }

        public override bool IsInvalid { get { throw null; } }

        protected override bool ReleaseHandle() { throw null; }
    }
}

namespace System.IO
{
    public static partial class Directory
    {
        public static DirectoryInfo CreateDirectory(string path) { throw null; }

        public static void Delete(string path, bool recursive) { }

        public static void Delete(string path) { }

        public static Collections.Generic.IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static Collections.Generic.IEnumerable<string> EnumerateDirectories(string path, string searchPattern) { throw null; }

        public static Collections.Generic.IEnumerable<string> EnumerateDirectories(string path) { throw null; }

        public static Collections.Generic.IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static Collections.Generic.IEnumerable<string> EnumerateFiles(string path, string searchPattern) { throw null; }

        public static Collections.Generic.IEnumerable<string> EnumerateFiles(string path) { throw null; }

        public static Collections.Generic.IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static Collections.Generic.IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern) { throw null; }

        public static Collections.Generic.IEnumerable<string> EnumerateFileSystemEntries(string path) { throw null; }

        public static bool Exists(string path) { throw null; }

        public static DateTime GetCreationTime(string path) { throw null; }

        public static DateTime GetCreationTimeUtc(string path) { throw null; }

        public static string GetCurrentDirectory() { throw null; }

        public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static string[] GetDirectories(string path, string searchPattern) { throw null; }

        public static string[] GetDirectories(string path) { throw null; }

        public static string GetDirectoryRoot(string path) { throw null; }

        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static string[] GetFiles(string path, string searchPattern) { throw null; }

        public static string[] GetFiles(string path) { throw null; }

        public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static string[] GetFileSystemEntries(string path, string searchPattern) { throw null; }

        public static string[] GetFileSystemEntries(string path) { throw null; }

        public static DateTime GetLastAccessTime(string path) { throw null; }

        public static DateTime GetLastAccessTimeUtc(string path) { throw null; }

        public static DateTime GetLastWriteTime(string path) { throw null; }

        public static DateTime GetLastWriteTimeUtc(string path) { throw null; }

        public static DirectoryInfo GetParent(string path) { throw null; }

        public static void Move(string sourceDirName, string destDirName) { }

        public static void SetCreationTime(string path, DateTime creationTime) { }

        public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc) { }

        public static void SetCurrentDirectory(string path) { }

        public static void SetLastAccessTime(string path, DateTime lastAccessTime) { }

        public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc) { }

        public static void SetLastWriteTime(string path, DateTime lastWriteTime) { }

        public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc) { }
    }

    public sealed partial class DirectoryInfo : FileSystemInfo
    {
        public DirectoryInfo(string path) { }

        public override bool Exists { get { throw null; } }

        public override string Name { get { throw null; } }

        public DirectoryInfo Parent { get { throw null; } }

        public DirectoryInfo Root { get { throw null; } }

        public void Create() { }

        public DirectoryInfo CreateSubdirectory(string path) { throw null; }

        public override void Delete() { }

        public void Delete(bool recursive) { }

        public Collections.Generic.IEnumerable<DirectoryInfo> EnumerateDirectories() { throw null; }

        public Collections.Generic.IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption) { throw null; }

        public Collections.Generic.IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern) { throw null; }

        public Collections.Generic.IEnumerable<FileInfo> EnumerateFiles() { throw null; }

        public Collections.Generic.IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption) { throw null; }

        public Collections.Generic.IEnumerable<FileInfo> EnumerateFiles(string searchPattern) { throw null; }

        public Collections.Generic.IEnumerable<FileSystemInfo> EnumerateFileSystemInfos() { throw null; }

        public Collections.Generic.IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption) { throw null; }

        public Collections.Generic.IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern) { throw null; }

        public DirectoryInfo[] GetDirectories() { throw null; }

        public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption) { throw null; }

        public DirectoryInfo[] GetDirectories(string searchPattern) { throw null; }

        public FileInfo[] GetFiles() { throw null; }

        public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption) { throw null; }

        public FileInfo[] GetFiles(string searchPattern) { throw null; }

        public FileSystemInfo[] GetFileSystemInfos() { throw null; }

        public FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption) { throw null; }

        public FileSystemInfo[] GetFileSystemInfos(string searchPattern) { throw null; }

        public void MoveTo(string destDirName) { }

        public override string ToString() { throw null; }
    }

    public static partial class File
    {
        public static void AppendAllLines(string path, Collections.Generic.IEnumerable<string> contents, Text.Encoding encoding) { }

        public static void AppendAllLines(string path, Collections.Generic.IEnumerable<string> contents) { }

        public static void AppendAllText(string path, string contents, Text.Encoding encoding) { }

        public static void AppendAllText(string path, string contents) { }

        public static StreamWriter AppendText(string path) { throw null; }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite) { }

        public static void Copy(string sourceFileName, string destFileName) { }

        public static FileStream Create(string path, int bufferSize, FileOptions options) { throw null; }

        public static FileStream Create(string path, int bufferSize) { throw null; }

        public static FileStream Create(string path) { throw null; }

        public static StreamWriter CreateText(string path) { throw null; }

        public static void Delete(string path) { }

        public static bool Exists(string path) { throw null; }

        public static FileAttributes GetAttributes(string path) { throw null; }

        public static DateTime GetCreationTime(string path) { throw null; }

        public static DateTime GetCreationTimeUtc(string path) { throw null; }

        public static DateTime GetLastAccessTime(string path) { throw null; }

        public static DateTime GetLastAccessTimeUtc(string path) { throw null; }

        public static DateTime GetLastWriteTime(string path) { throw null; }

        public static DateTime GetLastWriteTimeUtc(string path) { throw null; }

        public static void Move(string sourceFileName, string destFileName) { }

        public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share) { throw null; }

        public static FileStream Open(string path, FileMode mode, FileAccess access) { throw null; }

        public static FileStream Open(string path, FileMode mode) { throw null; }

        public static FileStream OpenRead(string path) { throw null; }

        public static StreamReader OpenText(string path) { throw null; }

        public static FileStream OpenWrite(string path) { throw null; }

        public static byte[] ReadAllBytes(string path) { throw null; }

        public static string[] ReadAllLines(string path, Text.Encoding encoding) { throw null; }

        public static string[] ReadAllLines(string path) { throw null; }

        public static string ReadAllText(string path, Text.Encoding encoding) { throw null; }

        public static string ReadAllText(string path) { throw null; }

        public static Collections.Generic.IEnumerable<string> ReadLines(string path, Text.Encoding encoding) { throw null; }

        public static Collections.Generic.IEnumerable<string> ReadLines(string path) { throw null; }

        public static void SetAttributes(string path, FileAttributes fileAttributes) { }

        public static void SetCreationTime(string path, DateTime creationTime) { }

        public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc) { }

        public static void SetLastAccessTime(string path, DateTime lastAccessTime) { }

        public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc) { }

        public static void SetLastWriteTime(string path, DateTime lastWriteTime) { }

        public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc) { }

        public static void WriteAllBytes(string path, byte[] bytes) { }

        public static void WriteAllLines(string path, Collections.Generic.IEnumerable<string> contents, Text.Encoding encoding) { }

        public static void WriteAllLines(string path, Collections.Generic.IEnumerable<string> contents) { }

        public static void WriteAllText(string path, string contents, Text.Encoding encoding) { }

        public static void WriteAllText(string path, string contents) { }
    }

    public sealed partial class FileInfo : FileSystemInfo
    {
        public FileInfo(string fileName) { }

        public DirectoryInfo Directory { get { throw null; } }

        public string DirectoryName { get { throw null; } }

        public override bool Exists { get { throw null; } }

        public bool IsReadOnly { get { throw null; } set { } }

        public long Length { get { throw null; } }

        public override string Name { get { throw null; } }

        public StreamWriter AppendText() { throw null; }

        public FileInfo CopyTo(string destFileName, bool overwrite) { throw null; }

        public FileInfo CopyTo(string destFileName) { throw null; }

        public FileStream Create() { throw null; }

        public StreamWriter CreateText() { throw null; }

        public override void Delete() { }

        public void MoveTo(string destFileName) { }

        public FileStream Open(FileMode mode, FileAccess access, FileShare share) { throw null; }

        public FileStream Open(FileMode mode, FileAccess access) { throw null; }

        public FileStream Open(FileMode mode) { throw null; }

        public FileStream OpenRead() { throw null; }

        public StreamReader OpenText() { throw null; }

        public FileStream OpenWrite() { throw null; }

        public override string ToString() { throw null; }
    }

    [Flags]
    public enum FileOptions
    {
        WriteThrough = int.MinValue,
        None = 0,
        Encrypted = 16384,
        DeleteOnClose = 67108864,
        SequentialScan = 134217728,
        RandomAccess = 268435456,
        Asynchronous = 1073741824
    }

    public partial class FileStream : Stream
    {
        public FileStream(Microsoft.Win32.SafeHandles.SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) { }

        public FileStream(Microsoft.Win32.SafeHandles.SafeFileHandle handle, FileAccess access, int bufferSize) { }

        public FileStream(Microsoft.Win32.SafeHandles.SafeFileHandle handle, FileAccess access) { }

        public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync) { }

        public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options) { }

        public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize) { }

        public FileStream(string path, FileMode mode, FileAccess access, FileShare share) { }

        public FileStream(string path, FileMode mode, FileAccess access) { }

        public FileStream(string path, FileMode mode) { }

        public override bool CanRead { get { throw null; } }

        public override bool CanSeek { get { throw null; } }

        public override bool CanWrite { get { throw null; } }

        public virtual bool IsAsync { get { throw null; } }

        public override long Length { get { throw null; } }

        public string Name { get { throw null; } }

        public override long Position { get { throw null; } set { } }

        public virtual Microsoft.Win32.SafeHandles.SafeFileHandle SafeFileHandle { get { throw null; } }

        protected override void Dispose(bool disposing) { }

        ~FileStream() {
        }

        public override void Flush() { }

        public virtual void Flush(bool flushToDisk) { }

        public override Threading.Tasks.Task FlushAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public override int Read(byte[] array, int offset, int count) { throw null; }

        public override Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }

        public override int ReadByte() { throw null; }

        public override long Seek(long offset, SeekOrigin origin) { throw null; }

        public override void SetLength(long value) { }

        public override void Write(byte[] array, int offset, int count) { }

        public override Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }

        public override void WriteByte(byte value) { }
    }

    public abstract partial class FileSystemInfo
    {
        protected string FullPath;
        protected string OriginalPath;
        public FileAttributes Attributes { get { throw null; } set { } }

        public DateTime CreationTime { get { throw null; } set { } }

        public DateTime CreationTimeUtc { get { throw null; } set { } }

        public abstract bool Exists { get; }

        public string Extension { get { throw null; } }

        public virtual string FullName { get { throw null; } }

        public DateTime LastAccessTime { get { throw null; } set { } }

        public DateTime LastAccessTimeUtc { get { throw null; } set { } }

        public DateTime LastWriteTime { get { throw null; } set { } }

        public DateTime LastWriteTimeUtc { get { throw null; } set { } }

        public abstract string Name { get; }

        public abstract void Delete();
        public void Refresh() { }
    }

    public enum SearchOption
    {
        TopDirectoryOnly = 0,
        AllDirectories = 1
    }
}
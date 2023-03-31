// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETFramework,Version=v4.7.2", FrameworkDisplayName = ".NET Framework 4.7.2")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.IO.Redist")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Downlevel support package for System.IO classes.")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.IO.Redist")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.IO
{
    public static partial class Directory
    {
        public static DirectoryInfo CreateDirectory(string path) { throw null; }

        public static FileSystemInfo CreateSymbolicLink(string path, string pathToTarget) { throw null; }

        public static void Delete(string path, bool recursive) { }

        public static void Delete(string path) { }

        public static System.Collections.Generic.IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateDirectories(string path, string searchPattern) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateDirectories(string path) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateFiles(string path, string searchPattern) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateFiles(string path) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> EnumerateFileSystemEntries(string path) { throw null; }

        public static bool Exists(string? path) { throw null; }

        public static System.DateTime GetCreationTime(string path) { throw null; }

        public static System.DateTime GetCreationTimeUtc(string path) { throw null; }

        public static string GetCurrentDirectory() { throw null; }

        public static string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static string[] GetDirectories(string path, string searchPattern) { throw null; }

        public static string[] GetDirectories(string path) { throw null; }

        public static string GetDirectoryRoot(string path) { throw null; }

        public static string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static string[] GetFiles(string path, string searchPattern) { throw null; }

        public static string[] GetFiles(string path) { throw null; }

        public static string[] GetFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption) { throw null; }

        public static string[] GetFileSystemEntries(string path, string searchPattern) { throw null; }

        public static string[] GetFileSystemEntries(string path) { throw null; }

        public static System.DateTime GetLastAccessTime(string path) { throw null; }

        public static System.DateTime GetLastAccessTimeUtc(string path) { throw null; }

        public static System.DateTime GetLastWriteTime(string path) { throw null; }

        public static System.DateTime GetLastWriteTimeUtc(string path) { throw null; }

        public static string[] GetLogicalDrives() { throw null; }

        public static DirectoryInfo? GetParent(string path) { throw null; }

        public static void Move(string sourceDirName, string destDirName) { }

        public static FileSystemInfo? ResolveLinkTarget(string linkPath, bool returnFinalTarget) { throw null; }

        public static void SetCreationTime(string path, System.DateTime creationTime) { }

        public static void SetCreationTimeUtc(string path, System.DateTime creationTimeUtc) { }

        public static void SetCurrentDirectory(string path) { }

        public static void SetLastAccessTime(string path, System.DateTime lastAccessTime) { }

        public static void SetLastAccessTimeUtc(string path, System.DateTime lastAccessTimeUtc) { }

        public static void SetLastWriteTime(string path, System.DateTime lastWriteTime) { }

        public static void SetLastWriteTimeUtc(string path, System.DateTime lastWriteTimeUtc) { }
    }

    public sealed partial class DirectoryInfo : FileSystemInfo
    {
        public DirectoryInfo(string path) { }

        public DirectoryInfo? Parent { get { throw null; } }

        public DirectoryInfo Root { get { throw null; } }

        public void Create() { }

        public DirectoryInfo CreateSubdirectory(string path) { throw null; }

        public override void Delete() { }

        public void Delete(bool recursive) { }

        public System.Collections.Generic.IEnumerable<DirectoryInfo> EnumerateDirectories() { throw null; }

        public System.Collections.Generic.IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public System.Collections.Generic.IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption) { throw null; }

        public System.Collections.Generic.IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern) { throw null; }

        public System.Collections.Generic.IEnumerable<FileInfo> EnumerateFiles() { throw null; }

        public System.Collections.Generic.IEnumerable<FileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public System.Collections.Generic.IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption) { throw null; }

        public System.Collections.Generic.IEnumerable<FileInfo> EnumerateFiles(string searchPattern) { throw null; }

        public System.Collections.Generic.IEnumerable<FileSystemInfo> EnumerateFileSystemInfos() { throw null; }

        public System.Collections.Generic.IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public System.Collections.Generic.IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption) { throw null; }

        public System.Collections.Generic.IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern) { throw null; }

        public DirectoryInfo[] GetDirectories() { throw null; }

        public DirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption) { throw null; }

        public DirectoryInfo[] GetDirectories(string searchPattern) { throw null; }

        public FileInfo[] GetFiles() { throw null; }

        public FileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption) { throw null; }

        public FileInfo[] GetFiles(string searchPattern) { throw null; }

        public FileSystemInfo[] GetFileSystemInfos() { throw null; }

        public FileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions) { throw null; }

        public FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption) { throw null; }

        public FileSystemInfo[] GetFileSystemInfos(string searchPattern) { throw null; }

        public void MoveTo(string destDirName) { }
    }

    public partial class EnumerationOptions
    {
        public EnumerationOptions() { }

        public System.IO.FileAttributes AttributesToSkip { get { throw null; } set { } }

        public int BufferSize { get { throw null; } set { } }

        public bool IgnoreInaccessible { get { throw null; } set { } }

        public MatchCasing MatchCasing { get { throw null; } set { } }

        public MatchType MatchType { get { throw null; } set { } }

        public int MaxRecursionDepth { get { throw null; } set { } }

        public bool RecurseSubdirectories { get { throw null; } set { } }

        public bool ReturnSpecialDirectories { get { throw null; } set { } }
    }

    public static partial class File
    {
        public static void AppendAllLines(string path, System.Collections.Generic.IEnumerable<string> contents, System.Text.Encoding encoding) { }

        public static void AppendAllLines(string path, System.Collections.Generic.IEnumerable<string> contents) { }

        public static System.Threading.Tasks.Task AppendAllLinesAsync(string path, System.Collections.Generic.IEnumerable<string> contents, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static System.Threading.Tasks.Task AppendAllLinesAsync(string path, System.Collections.Generic.IEnumerable<string> contents, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static void AppendAllText(string path, string? contents, System.Text.Encoding encoding) { }

        public static void AppendAllText(string path, string? contents) { }

        public static System.Threading.Tasks.Task AppendAllTextAsync(string path, string? contents, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static System.Threading.Tasks.Task AppendAllTextAsync(string path, string? contents, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static System.IO.StreamWriter AppendText(string path) { throw null; }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite) { }

        public static void Copy(string sourceFileName, string destFileName) { }

        public static System.IO.FileStream Create(string path, int bufferSize, System.IO.FileOptions options) { throw null; }

        public static System.IO.FileStream Create(string path, int bufferSize) { throw null; }

        public static System.IO.FileStream Create(string path) { throw null; }

        public static FileSystemInfo CreateSymbolicLink(string path, string pathToTarget) { throw null; }

        public static System.IO.StreamWriter CreateText(string path) { throw null; }

        public static void Decrypt(string path) { }

        public static void Delete(string path) { }

        public static void Encrypt(string path) { }

        public static bool Exists(string? path) { throw null; }

        public static System.IO.FileAttributes GetAttributes(string path) { throw null; }

        public static System.DateTime GetCreationTime(string path) { throw null; }

        public static System.DateTime GetCreationTimeUtc(string path) { throw null; }

        public static System.DateTime GetLastAccessTime(string path) { throw null; }

        public static System.DateTime GetLastAccessTimeUtc(string path) { throw null; }

        public static System.DateTime GetLastWriteTime(string path) { throw null; }

        public static System.DateTime GetLastWriteTimeUtc(string path) { throw null; }

        public static void Move(string sourceFileName, string destFileName, bool overwrite) { }

        public static void Move(string sourceFileName, string destFileName) { }

        public static System.IO.FileStream Open(string path, System.IO.FileMode mode, System.IO.FileAccess access, System.IO.FileShare share) { throw null; }

        public static System.IO.FileStream Open(string path, System.IO.FileMode mode, System.IO.FileAccess access) { throw null; }

        public static System.IO.FileStream Open(string path, System.IO.FileMode mode) { throw null; }

        public static System.IO.FileStream OpenRead(string path) { throw null; }

        public static System.IO.StreamReader OpenText(string path) { throw null; }

        public static System.IO.FileStream OpenWrite(string path) { throw null; }

        public static byte[] ReadAllBytes(string path) { throw null; }

        public static System.Threading.Tasks.Task<byte[]> ReadAllBytesAsync(string path, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static string[] ReadAllLines(string path, System.Text.Encoding encoding) { throw null; }

        public static string[] ReadAllLines(string path) { throw null; }

        public static System.Threading.Tasks.Task<string[]> ReadAllLinesAsync(string path, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static System.Threading.Tasks.Task<string[]> ReadAllLinesAsync(string path, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static string ReadAllText(string path, System.Text.Encoding encoding) { throw null; }

        public static string ReadAllText(string path) { throw null; }

        public static System.Threading.Tasks.Task<string> ReadAllTextAsync(string path, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static System.Threading.Tasks.Task<string> ReadAllTextAsync(string path, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> ReadLines(string path, System.Text.Encoding encoding) { throw null; }

        public static System.Collections.Generic.IEnumerable<string> ReadLines(string path) { throw null; }

        public static void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors) { }

        public static void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName) { }

        public static FileSystemInfo? ResolveLinkTarget(string linkPath, bool returnFinalTarget) { throw null; }

        public static void SetAttributes(string path, System.IO.FileAttributes fileAttributes) { }

        public static void SetCreationTime(string path, System.DateTime creationTime) { }

        public static void SetCreationTimeUtc(string path, System.DateTime creationTimeUtc) { }

        public static void SetLastAccessTime(string path, System.DateTime lastAccessTime) { }

        public static void SetLastAccessTimeUtc(string path, System.DateTime lastAccessTimeUtc) { }

        public static void SetLastWriteTime(string path, System.DateTime lastWriteTime) { }

        public static void SetLastWriteTimeUtc(string path, System.DateTime lastWriteTimeUtc) { }

        public static void WriteAllBytes(string path, byte[] bytes) { }

        public static System.Threading.Tasks.Task WriteAllBytesAsync(string path, byte[] bytes, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static void WriteAllLines(string path, System.Collections.Generic.IEnumerable<string> contents, System.Text.Encoding encoding) { }

        public static void WriteAllLines(string path, System.Collections.Generic.IEnumerable<string> contents) { }

        public static void WriteAllLines(string path, string[] contents, System.Text.Encoding encoding) { }

        public static void WriteAllLines(string path, string[] contents) { }

        public static System.Threading.Tasks.Task WriteAllLinesAsync(string path, System.Collections.Generic.IEnumerable<string> contents, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static System.Threading.Tasks.Task WriteAllLinesAsync(string path, System.Collections.Generic.IEnumerable<string> contents, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static void WriteAllText(string path, string? contents, System.Text.Encoding encoding) { }

        public static void WriteAllText(string path, string? contents) { }

        public static System.Threading.Tasks.Task WriteAllTextAsync(string path, string? contents, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default) { throw null; }

        public static System.Threading.Tasks.Task WriteAllTextAsync(string path, string? contents, System.Threading.CancellationToken cancellationToken = default) { throw null; }
    }

    public sealed partial class FileInfo : FileSystemInfo
    {
        public FileInfo(string fileName) { }

        public DirectoryInfo? Directory { get { throw null; } }

        public string? DirectoryName { get { throw null; } }

        public bool IsReadOnly { get { throw null; } set { } }

        public long Length { get { throw null; } }

        public System.IO.StreamWriter AppendText() { throw null; }

        public FileInfo CopyTo(string destFileName, bool overwrite) { throw null; }

        public FileInfo CopyTo(string destFileName) { throw null; }

        public System.IO.FileStream Create() { throw null; }

        public System.IO.StreamWriter CreateText() { throw null; }

        public void Decrypt() { }

        public override void Delete() { }

        public void Encrypt() { }

        public void MoveTo(string destFileName, bool overwrite) { }

        public void MoveTo(string destFileName) { }

        public System.IO.FileStream Open(System.IO.FileMode mode, System.IO.FileAccess access, System.IO.FileShare share) { throw null; }

        public System.IO.FileStream Open(System.IO.FileMode mode, System.IO.FileAccess access) { throw null; }

        public System.IO.FileStream Open(System.IO.FileMode mode) { throw null; }

        public System.IO.FileStream OpenRead() { throw null; }

        public System.IO.StreamReader OpenText() { throw null; }

        public System.IO.FileStream OpenWrite() { throw null; }

        public FileInfo Replace(string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors) { throw null; }

        public FileInfo Replace(string destinationFileName, string? destinationBackupFileName) { throw null; }
    }

    public abstract partial class FileSystemInfo : System.MarshalByRefObject, System.Runtime.Serialization.ISerializable
    {
        protected string FullPath;
        protected string OriginalPath;
        protected FileSystemInfo() { }

        protected FileSystemInfo(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }

        public System.IO.FileAttributes Attributes { get { throw null; } set { } }

        public System.DateTime CreationTime { get { throw null; } set { } }

        public System.DateTime CreationTimeUtc { get { throw null; } set { } }

        public virtual bool Exists { get { throw null; } }

        public string Extension { get { throw null; } }

        public virtual string FullName { get { throw null; } }

        public System.DateTime LastAccessTime { get { throw null; } set { } }

        public System.DateTime LastAccessTimeUtc { get { throw null; } set { } }

        public System.DateTime LastWriteTime { get { throw null; } set { } }

        public System.DateTime LastWriteTimeUtc { get { throw null; } set { } }

        public string? LinkTarget { get { throw null; } }

        public virtual string Name { get { throw null; } }

        public void CreateAsSymbolicLink(string pathToTarget) { }

        public abstract void Delete();
        public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }

        public void Refresh() { }

        public FileSystemInfo? ResolveLinkTarget(bool returnFinalTarget) { throw null; }

        public override string ToString() { throw null; }
    }

    public enum MatchCasing
    {
        PlatformDefault = 0,
        CaseSensitive = 1,
        CaseInsensitive = 2
    }

    public enum MatchType
    {
        Simple = 0,
        Win32 = 1
    }

    public static partial class Path
    {
        public static readonly char AltDirectorySeparatorChar;
        public static readonly char DirectorySeparatorChar;
        [System.Obsolete("Path.InvalidPathChars has been deprecated. Use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
        public static readonly char[] InvalidPathChars;
        public static readonly char PathSeparator;
        public static readonly char VolumeSeparatorChar;
        public static string? ChangeExtension(string? path, string? extension) { throw null; }

        public static string Combine(string path1, string path2, string path3, string path4) { throw null; }

        public static string Combine(string path1, string path2, string path3) { throw null; }

        public static string Combine(string path1, string path2) { throw null; }

        public static string Combine(params string[] paths) { throw null; }

        public static bool EndsInDirectorySeparator(System.ReadOnlySpan<char> path) { throw null; }

        public static bool EndsInDirectorySeparator(string path) { throw null; }

        public static System.ReadOnlySpan<char> GetDirectoryName(System.ReadOnlySpan<char> path) { throw null; }

        public static string? GetDirectoryName(string? path) { throw null; }

        public static System.ReadOnlySpan<char> GetExtension(System.ReadOnlySpan<char> path) { throw null; }

        public static string? GetExtension(string? path) { throw null; }

        public static System.ReadOnlySpan<char> GetFileName(System.ReadOnlySpan<char> path) { throw null; }

        public static string? GetFileName(string? path) { throw null; }

        public static System.ReadOnlySpan<char> GetFileNameWithoutExtension(System.ReadOnlySpan<char> path) { throw null; }

        public static string? GetFileNameWithoutExtension(string? path) { throw null; }

        public static string GetFullPath(string path, string basePath) { throw null; }

        public static string GetFullPath(string path) { throw null; }

        public static char[] GetInvalidFileNameChars() { throw null; }

        public static char[] GetInvalidPathChars() { throw null; }

        public static System.ReadOnlySpan<char> GetPathRoot(System.ReadOnlySpan<char> path) { throw null; }

        public static string? GetPathRoot(string? path) { throw null; }

        public static string GetRandomFileName() { throw null; }

        public static string GetRelativePath(string relativeTo, string path) { throw null; }

        public static string GetTempFileName() { throw null; }

        public static string GetTempPath() { throw null; }

        public static bool HasExtension(System.ReadOnlySpan<char> path) { throw null; }

        public static bool HasExtension(string? path) { throw null; }

        public static bool IsPathFullyQualified(System.ReadOnlySpan<char> path) { throw null; }

        public static bool IsPathFullyQualified(string path) { throw null; }

        public static bool IsPathRooted(System.ReadOnlySpan<char> path) { throw null; }

        public static bool IsPathRooted(string? path) { throw null; }

        public static string Join(System.ReadOnlySpan<char> path1, System.ReadOnlySpan<char> path2, System.ReadOnlySpan<char> path3, System.ReadOnlySpan<char> path4) { throw null; }

        public static string Join(System.ReadOnlySpan<char> path1, System.ReadOnlySpan<char> path2, System.ReadOnlySpan<char> path3) { throw null; }

        public static string Join(System.ReadOnlySpan<char> path1, System.ReadOnlySpan<char> path2) { throw null; }

        public static string Join(string? path1, string? path2, string? path3, string? path4) { throw null; }

        public static string Join(string? path1, string? path2, string? path3) { throw null; }

        public static string Join(string? path1, string? path2) { throw null; }

        public static string Join(params string?[] paths) { throw null; }

        public static System.ReadOnlySpan<char> TrimEndingDirectorySeparator(System.ReadOnlySpan<char> path) { throw null; }

        public static string TrimEndingDirectorySeparator(string path) { throw null; }

        public static bool TryJoin(System.ReadOnlySpan<char> path1, System.ReadOnlySpan<char> path2, System.ReadOnlySpan<char> path3, System.Span<char> destination, out int charsWritten) { throw null; }

        public static bool TryJoin(System.ReadOnlySpan<char> path1, System.ReadOnlySpan<char> path2, System.Span<char> destination, out int charsWritten) { throw null; }
    }

    public enum SearchOption
    {
        TopDirectoryOnly = 0,
        AllDirectories = 1
    }

    public static partial class StringExtensions
    {
        public static bool Contains(this string s, char value) { throw null; }

        public static string Create<TState>(int length, TState state, SpanAction<char, TState> action) { throw null; }

        public delegate void SpanAction<T, in TArg>(System.Span<T> span, TArg arg);
    }
}

namespace Microsoft.IO.Enumeration
{
    public partial struct FileSystemEntry
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.IO.FileAttributes Attributes { get { throw null; } }

        public System.DateTimeOffset CreationTimeUtc { get { throw null; } }

        public System.ReadOnlySpan<char> Directory { get { throw null; } }

        public System.ReadOnlySpan<char> FileName { get { throw null; } }

        public bool IsDirectory { get { throw null; } }

        public bool IsHidden { get { throw null; } }

        public System.DateTimeOffset LastAccessTimeUtc { get { throw null; } }

        public System.DateTimeOffset LastWriteTimeUtc { get { throw null; } }

        public long Length { get { throw null; } }

        public System.ReadOnlySpan<char> OriginalRootDirectory { get { throw null; } }

        public System.ReadOnlySpan<char> RootDirectory { get { throw null; } }

        public FileSystemInfo ToFileSystemInfo() { throw null; }

        public string ToFullPath() { throw null; }

        public string ToSpecifiedFullPath() { throw null; }
    }

    public partial class FileSystemEnumerable<TResult> : System.Collections.IEnumerable
    {
        public FileSystemEnumerable(string directory, FileSystemEnumerable<TResult>.FindTransform transform, EnumerationOptions? options = null) { }

        public FileSystemEnumerable<TResult>.FindPredicate? ShouldIncludePredicate { get { throw null; } set { } }

        public FileSystemEnumerable<TResult>.FindPredicate? ShouldRecursePredicate { get { throw null; } set { } }

        public System.Collections.Generic.IEnumerator<TResult> GetEnumerator() { throw null; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }

        public delegate bool FindPredicate(ref FileSystemEntry entry);
        public delegate TResult FindTransform(ref FileSystemEntry entry);
    }

    public abstract partial class FileSystemEnumerator<TResult> : System.Runtime.ConstrainedExecution.CriticalFinalizerObject, System.IDisposable, System.Collections.IEnumerator
    {
        public FileSystemEnumerator(string directory, EnumerationOptions? options = null) { }

        public TResult Current { get { throw null; } }

        object? System.Collections.IEnumerator.Current { get { throw null; } }

        protected virtual bool ContinueOnError(int error) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~FileSystemEnumerator() {
        }

        public bool MoveNext() { throw null; }

        protected virtual void OnDirectoryFinished(System.ReadOnlySpan<char> directory) { }

        public void Reset() { }

        protected virtual bool ShouldIncludeEntry(ref FileSystemEntry entry) { throw null; }

        protected virtual bool ShouldRecurseIntoEntry(ref FileSystemEntry entry) { throw null; }

        protected abstract TResult TransformEntry(ref FileSystemEntry entry);
    }

    public static partial class FileSystemName
    {
        public static bool MatchesSimpleExpression(System.ReadOnlySpan<char> expression, System.ReadOnlySpan<char> name, bool ignoreCase = true) { throw null; }

        public static bool MatchesWin32Expression(System.ReadOnlySpan<char> expression, System.ReadOnlySpan<char> name, bool ignoreCase = true) { throw null; }

        public static string TranslateWin32Expression(string? expression) { throw null; }
    }
}
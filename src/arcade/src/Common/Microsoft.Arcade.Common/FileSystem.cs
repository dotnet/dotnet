// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

#nullable enable

namespace Microsoft.Arcade.Common
{
    public class FileSystem : IFileSystem
    {
        public void CreateDirectory(string path) => Directory.CreateDirectory(path);

        public bool DirectoryExists(string path) => Directory.Exists(path);

        public bool FileExists(string path) => File.Exists(path);

        public void DeleteFile(string path) => File.Delete(path);

        public string? GetDirectoryName(string? path) => Path.GetDirectoryName(path);

        public string? GetFileName(string? path) => Path.GetFileName(path);

        public string? GetFileNameWithoutExtension(string? path) => Path.GetFileNameWithoutExtension(path);

        public string? GetExtension(string? path) => Path.GetExtension(path);

        public string GetFullPath(string path) => Path.GetFullPath(path);

        public string PathCombine(string path1, string path2) => Path.Combine(path1, path2);

        public string PathCombine(string path1, string path2, string path3) => Path.Combine(path1, path2, path3);

        public void WriteToFile(string path, string content)
        {
            string? dirPath = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dirPath!);
            File.WriteAllText(path, content);
        }

        public virtual void CopyFile(string sourceFileName, string destFileName, bool overwrite = false) => File.Copy(sourceFileName, destFileName, overwrite);

        public Stream GetFileStream(string path, FileMode mode, FileAccess access) => new FileStream(path, mode, access);

        public FileAttributes GetAttributes(string path) => File.GetAttributes(path);

        /// <summary>
        /// Intentionally NYI since the backing API is not supported on framework and core.
        /// We use this in the PushToBuildStorage task under a limited set of circumstances.
        /// </summary>
        /// <param name="basePath">Base path</param>
        /// <param name="targetPath">Target path that is relative to base path.</param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string GetRelativePath(string basePath, string targetPath) => throw new NotImplementedException("Not supported in default FileSystem implementation");
    }
}

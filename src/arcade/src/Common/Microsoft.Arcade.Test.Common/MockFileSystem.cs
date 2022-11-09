// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using Microsoft.Arcade.Common;

#nullable enable
namespace Microsoft.Arcade.Test.Common
{
    public class MockFileSystem : IFileSystem
    {
        #region File system state

        public HashSet<string> Directories { get; }    
        
        public Dictionary<string, string> Files { get; }

        public List<string> RemovedFiles { get; } = new();

        #endregion

        public MockFileSystem(
            Dictionary<string, string>? files = null,
            IEnumerable<string>? directories = null)
        {
            Directories = new(directories ?? new string[0]);
            Files = files ?? new();
        }

        #region IFileSystem implementation

        public void CreateDirectory(string path) => Directories.Add(path);

        public bool DirectoryExists(string path) => Directories.Contains(path);

        public bool FileExists(string path) => Files.ContainsKey(path);

        public void DeleteFile(string path)
        {
            Files.Remove(path);
            RemovedFiles.Add(path);
        }

        public string? GetDirectoryName(string? path) => Path.GetDirectoryName(path);

        public string? GetFileName(string? path) => Path.GetFileName(path);

        public string? GetFileNameWithoutExtension(string? path) => Path.GetFileNameWithoutExtension(path);

        public string? GetExtension(string? path) => Path.GetExtension(path);

        public string PathCombine(string path1, string path2) => path1 + "/" + path2;

        public void WriteToFile(string path, string content) => Files[path] = content;

        public void CopyFile(string sourceFileName, string destFileName, bool overwrite = false) => Files[destFileName] = Files[sourceFileName];

        public Stream GetFileStream(string path, FileMode mode, FileAccess access)
            => FileExists(path) ? new MemoryStream() : new MockFileStream(this, path);

        public FileAttributes GetAttributes(string path)
        {
            var attributes = FileAttributes.Normal;

            if (Directories.Contains(path))
            {
                attributes |= FileAttributes.Directory;
            }

            return  attributes;
        }

        #endregion

        /// <summary>
        /// Allows to write to a stream that will end up in the MockFileSystem.
        /// </summary>
        private class MockFileStream : MemoryStream
        {
            private readonly MockFileSystem _fileSystem;
            private readonly string _path;
            private bool _disposed = false;

            public MockFileStream(MockFileSystem fileSystem, string path)
                : base(fileSystem.FileExists(path) ? System.Text.Encoding.UTF8.GetBytes(fileSystem.Files[path]) : new byte[2048])
            {
                _fileSystem = fileSystem;
                _path = path;
            }

            protected override void Dispose(bool disposing)
            {
                // flush file to our system
                if (!_disposed)
                {
                    _disposed = true;
                    using var sr = new StreamReader(this);
                    Seek(0, SeekOrigin.Begin);
                    _fileSystem.WriteToFile(_path, sr.ReadToEnd().Replace("\0", ""));
                }
            }
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using System.IO;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(IFileSystem)), Shared]
internal class RemoteFileSystem : IFileSystem
{
    private IFileSystem _fileSystem = new FileSystem();

    public bool FileExists(string filePath)
        => _fileSystem.FileExists(filePath);

    public string ReadFile(string filePath)
        => _fileSystem.ReadFile(filePath);

    public IEnumerable<string> GetDirectories(string workspaceDirectory)
        => _fileSystem.GetDirectories(workspaceDirectory);

    public IEnumerable<string> GetFiles(string workspaceDirectory, string searchPattern, SearchOption searchOption)
        => _fileSystem.GetFiles(workspaceDirectory, searchPattern, searchOption);

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(RemoteFileSystem instance)
    {
        public void SetFileSystem(IFileSystem fileSystem)
        {
            instance._fileSystem = fileSystem;
        }
    }
}

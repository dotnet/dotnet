// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal sealed class FileSystem : IFileSystem
{
    public IEnumerable<string> GetFiles(string workspaceDirectory, string searchPattern, SearchOption searchOption)
        => Directory.GetFiles(workspaceDirectory, searchPattern, searchOption);

    public IEnumerable<string> GetDirectories(string workspaceDirectory)
        => Directory.GetDirectories(workspaceDirectory);

    public bool FileExists(string filePath)
        => File.Exists(filePath);

    public string ReadFile(string filePath)
        => File.ReadAllText(filePath);
}

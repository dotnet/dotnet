// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal interface IFileSystem
{
    IEnumerable<string> GetFiles(string workspaceDirectory, string searchPattern, SearchOption searchOption);

    IEnumerable<string> GetDirectories(string workspaceDirectory);

    bool FileExists(string filePath);

    string ReadFile(string filePath);
}

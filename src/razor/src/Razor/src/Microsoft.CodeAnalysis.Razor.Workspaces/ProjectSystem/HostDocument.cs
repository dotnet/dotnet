// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed record class HostDocument
{
    public string FileKind { get; init; }
    public string FilePath { get; init; }
    public string TargetPath { get; init; }

    public HostDocument(string filePath, string targetPath, string? fileKind = null)
    {
        FilePath = filePath;
        TargetPath = targetPath;
        FileKind = fileKind ?? FileKinds.GetFileKindFromFilePath(filePath);
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class HostDocument
{
    public string FileKind { get; }
    public string FilePath { get; }
    public string TargetPath { get; }

    public HostDocument(HostDocument other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        FileKind = other.FileKind;
        FilePath = other.FilePath;
        TargetPath = other.TargetPath;
    }

    public HostDocument(string filePath, string targetPath)
        : this(filePath, targetPath, fileKind: null)
    {
    }

    public HostDocument(string filePath, string targetPath, string? fileKind)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        TargetPath = targetPath ?? throw new ArgumentNullException(nameof(targetPath));
        FileKind = fileKind ?? FileKinds.GetFileKindFromFilePath(filePath);
    }
}

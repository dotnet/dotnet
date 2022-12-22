// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;

internal sealed class DocumentSnapshotHandle
{
    public DocumentSnapshotHandle(
        string filePath,
        string targetPath,
        string fileKind)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (targetPath is null)
        {
            throw new ArgumentNullException(nameof(targetPath));
        }

        if (fileKind is null)
        {
            throw new ArgumentNullException(nameof(fileKind));
        }

        FilePath = filePath;
        TargetPath = targetPath;
        FileKind = fileKind;
    }

    public string FilePath { get; }

    public string TargetPath { get; }

    public string FileKind { get; }
}

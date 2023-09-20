// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal sealed class DocumentSnapshotHandle
{
    public string FilePath { get; }
    public string TargetPath { get; }
    public string FileKind { get; }

    public DocumentSnapshotHandle(string filePath, string targetPath, string fileKind)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        TargetPath = targetPath ?? throw new ArgumentNullException(nameof(targetPath));
        FileKind = fileKind ?? throw new ArgumentNullException(nameof(fileKind));
    }
}

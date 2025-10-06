// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.VisualStudio.Razor.Documents;

internal sealed class FileChangeEventArgs : EventArgs
{
    public FileChangeEventArgs(string filePath, FileChangeKind kind)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        FilePath = filePath;
        Kind = kind;
    }

    public string FilePath { get; }

    public FileChangeKind Kind { get; }
}

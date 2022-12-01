// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Editor.Razor.Documents;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class ImportChangedEventArgs : EventArgs
{
    public ImportChangedEventArgs(string filePath, FileChangeKind kind, IEnumerable<string> associatedDocuments)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (associatedDocuments is null)
        {
            throw new ArgumentNullException(nameof(associatedDocuments));
        }

        FilePath = filePath;
        Kind = kind;
        AssociatedDocuments = associatedDocuments;
    }

    public string FilePath { get; }

    public FileChangeKind Kind { get; }

    public IEnumerable<string> AssociatedDocuments { get; }
}

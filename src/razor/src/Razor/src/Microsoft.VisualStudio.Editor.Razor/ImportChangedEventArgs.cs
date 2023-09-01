// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Editor.Razor.Documents;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class ImportChangedEventArgs(string filePath, FileChangeKind kind, IEnumerable<string> associatedDocuments) : EventArgs
{
    public string FilePath { get; } = filePath ?? throw new ArgumentNullException(nameof(filePath));
    public FileChangeKind Kind { get; } = kind;
    public IEnumerable<string> AssociatedDocuments { get; } = associatedDocuments ?? throw new ArgumentNullException(nameof(associatedDocuments));
}

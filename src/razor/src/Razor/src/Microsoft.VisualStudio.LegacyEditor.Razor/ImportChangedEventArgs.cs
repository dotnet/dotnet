// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Microsoft.VisualStudio.Razor.Documents;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal class ImportChangedEventArgs(string filePath, FileChangeKind kind, ImmutableArray<string> associatedDocuments) : EventArgs
{
    public string FilePath => filePath;
    public FileChangeKind Kind => kind;
    public ImmutableArray<string> AssociatedDocuments => associatedDocuments;
}

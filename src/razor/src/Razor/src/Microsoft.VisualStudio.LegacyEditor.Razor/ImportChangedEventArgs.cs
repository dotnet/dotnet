// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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

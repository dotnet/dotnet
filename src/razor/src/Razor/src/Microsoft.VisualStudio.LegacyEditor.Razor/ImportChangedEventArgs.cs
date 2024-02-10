// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Editor.Razor.Documents;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal class ImportChangedEventArgs(string filePath, FileChangeKind kind, IEnumerable<string> associatedDocuments) : EventArgs
{
    public string FilePath => filePath;
    public FileChangeKind Kind => kind;
    public IEnumerable<string> AssociatedDocuments => associatedDocuments;
}

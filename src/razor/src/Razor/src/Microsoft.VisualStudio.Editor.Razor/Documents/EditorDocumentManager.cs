// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

internal abstract class EditorDocumentManager : IWorkspaceService
{
    public abstract EditorDocument GetOrCreateDocument(
        DocumentKey key,
        EventHandler? changedOnDisk,
        EventHandler? changedInEditor,
        EventHandler? opened,
        EventHandler? closed);

    public abstract bool TryGetDocument(DocumentKey key, [NotNullWhen(returnValue: true)] out EditorDocument? document);

    public abstract bool TryGetMatchingDocuments(string filePath, [NotNullWhen(returnValue: true)] out EditorDocument[]? documents);

    public abstract void RemoveDocument(EditorDocument document);
}

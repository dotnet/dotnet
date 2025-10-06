// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.Documents;

internal interface IEditorDocumentManager
{
    EditorDocument GetOrCreateDocument(
        DocumentKey key,
        string projectFilePath,
        ProjectKey projectKey,
        EventHandler? changedOnDisk,
        EventHandler? changedInEditor,
        EventHandler? opened,
        EventHandler? closed);

    bool TryGetDocument(DocumentKey key, [NotNullWhen(true)] out EditorDocument? document);
    bool TryGetMatchingDocuments(string filePath, [NotNullWhen(true)] out EditorDocument[]? documents);

    void RemoveDocument(EditorDocument document);
}

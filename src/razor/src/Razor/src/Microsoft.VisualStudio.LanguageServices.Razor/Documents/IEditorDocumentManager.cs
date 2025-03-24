// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.ProjectSystem;

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

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class CodeDocumentReferenceHolder : DocumentProcessedListener
{
    private Dictionary<string, RazorCodeDocument> _codeDocumentCache;
    private ProjectSnapshotManager? _projectManager;

    public CodeDocumentReferenceHolder()
    {
        _codeDocumentCache = new(FilePathComparer.Instance);
    }

    public override void DocumentProcessed(RazorCodeDocument codeDocument, DocumentSnapshot documentSnapshot)
    {
        // We capture a reference to the code document after a document has been processed in order to ensure that
        // latest document state information is readily available without re-computation. The DocumentState type
        // (brains of DocumentSnapshot) will garbage collect its generated output aggressively and due to the
        // nature of LSP being heavily asynchronous (multiple requests for single keystrokes) we don't want to cause
        // multiple parses/regenerations across LSP requests that are all for the same document version.
        _codeDocumentCache[documentSnapshot.FilePath] = codeDocument;
    }

    public override void Initialize(ProjectSnapshotManager projectManager)
    {
        _projectManager = projectManager;
        _projectManager.Changed += ProjectManager_Changed;
    }

    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Goal here is to evict cache entries (really just references to code documents) of known documents when
        // related information changes for them

        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectChanged:
                foreach (var documentFilePath in args.Newer!.DocumentFilePaths)
                {
                    _codeDocumentCache.Remove(documentFilePath);
                }

                break;
            case ProjectChangeKind.ProjectRemoved:
                foreach (var documentFilePath in args.Older!.DocumentFilePaths)
                {
                    _codeDocumentCache.Remove(documentFilePath);
                }

                break;
            case ProjectChangeKind.DocumentChanged:
            case ProjectChangeKind.DocumentRemoved:
                _codeDocumentCache.Remove(args.DocumentFilePath!);
                break;
        }
    }
}

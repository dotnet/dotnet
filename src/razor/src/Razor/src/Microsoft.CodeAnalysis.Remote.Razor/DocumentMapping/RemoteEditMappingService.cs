// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Remote.Razor.DocumentMapping;

[Export(typeof(IEditMappingService)), Shared]
[method: ImportingConstructor]
internal sealed class RemoteEditMappingService(
    IDocumentMappingService documentMappingService,
    IFilePathService filePathService,
    RemoteSnapshotManager snapshotManager) : AbstractEditMappingService(documentMappingService, filePathService)
{
    private readonly RemoteSnapshotManager _snapshotManager = snapshotManager;

    protected override bool TryGetDocumentContext(IDocumentSnapshot contextDocumentSnapshot, Uri razorDocumentUri, VSProjectContext? projectContext, [NotNullWhen(true)] out DocumentContext? documentContext)
    {
        if (contextDocumentSnapshot is not RemoteDocumentSnapshot originSnapshot)
        {
            throw new InvalidOperationException("RemoteEditMappingService can only be used with RemoteDocumentSnapshot instances.");
        }

        var solution = originSnapshot.TextDocument.Project.Solution;
        if (!solution.TryGetRazorDocument(razorDocumentUri, out var razorDocument))
        {
            documentContext = null;
            return false;
        }

        var razorDocumentSnapshot = _snapshotManager.GetSnapshot(razorDocument);

        documentContext = new RemoteDocumentContext(razorDocumentUri, razorDocumentSnapshot);
        return true;
    }
}

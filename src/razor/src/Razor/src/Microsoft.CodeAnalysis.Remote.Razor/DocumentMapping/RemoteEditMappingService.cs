// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

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

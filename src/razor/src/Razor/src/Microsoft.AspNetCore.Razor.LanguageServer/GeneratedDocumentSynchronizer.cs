// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class GeneratedDocumentSynchronizer : DocumentProcessedListener
{
    private readonly IGeneratedDocumentPublisher _publisher;
    private readonly IDocumentVersionCache _documentVersionCache;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher;

    public GeneratedDocumentSynchronizer(
        IGeneratedDocumentPublisher publisher,
        IDocumentVersionCache documentVersionCache,
        ProjectSnapshotManagerDispatcher dispatcher)
    {
        _publisher = publisher;
        _documentVersionCache = documentVersionCache;
        _dispatcher = dispatcher;
    }

    public override void Initialize(ProjectSnapshotManager projectManager)
    {
    }

    public override void DocumentProcessed(RazorCodeDocument codeDocument, IDocumentSnapshot document)
    {
        _dispatcher.AssertDispatcherThread();

        if (!_documentVersionCache.TryGetDocumentVersion(document, out var hostDocumentVersion))
        {
            // Could not resolve document version
            return;
        }

        var filePath = document.FilePath.AssumeNotNull();

        var htmlText = codeDocument.GetHtmlSourceText();

        _publisher.PublishHtml(document.Project.Key, filePath, htmlText, hostDocumentVersion.Value);

        var csharpText = codeDocument.GetCSharpSourceText();

        _publisher.PublishCSharp(document.Project.Key, filePath, csharpText, hostDocumentVersion.Value);
    }
}

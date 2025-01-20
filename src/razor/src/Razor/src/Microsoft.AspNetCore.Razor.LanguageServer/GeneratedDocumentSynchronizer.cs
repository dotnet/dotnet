// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class GeneratedDocumentSynchronizer(
    IGeneratedDocumentPublisher publisher,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    ProjectSnapshotManager projectManager) : IDocumentProcessedListener
{
    private readonly IGeneratedDocumentPublisher _publisher = publisher;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly ProjectSnapshotManager _projectManager = projectManager;

    public void DocumentProcessed(RazorCodeDocument codeDocument, DocumentSnapshot document)
    {
        var hostDocumentVersion = document.Version;
        var filePath = document.FilePath;

        // If the document isn't open, and we're not updating buffers for closed documents, then we don't need to do anything.
        if (!_projectManager.IsDocumentOpen(filePath) &&
            !_languageServerFeatureOptions.UpdateBuffersForClosedDocuments)
        {
            return;
        }

        // If the document has been removed from the project, then don't do anything, or version numbers will be thrown off
        if (!_projectManager.ContainsDocument(document.Project.Key, filePath))
        {
            return;
        }

        // If cohosting is on, then it is responsible for updating the Html buffer
        if (!_languageServerFeatureOptions.UseRazorCohostServer)
        {
            var htmlText = codeDocument.GetHtmlSourceText();

            _publisher.PublishHtml(document.Project.Key, filePath, htmlText, hostDocumentVersion);
        }

        var csharpText = codeDocument.GetCSharpSourceText();

        _publisher.PublishCSharp(document.Project.Key, filePath, csharpText, hostDocumentVersion);
    }
}

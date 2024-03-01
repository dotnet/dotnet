// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Cohost;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Export(typeof(IRazorCohostTextDocumentSyncHandler)), Shared]
[method: ImportingConstructor]
internal class CohostTextDocumentSyncHandler(
    ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
    IRazorProjectService razorProjectService,
    DocumentSnapshotFactory documentSnapshotFactory,
    OpenDocumentGenerator openDocumentGenerator,
    IRazorLoggerFactory loggerFactory) : IRazorCohostTextDocumentSyncHandler
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
    private readonly IRazorProjectService _razorProjectService = razorProjectService;
    private readonly DocumentSnapshotFactory _documentSnapshotFactory = documentSnapshotFactory;
    private readonly OpenDocumentGenerator _openDocumentGenerator = openDocumentGenerator;
    private readonly ILogger _logger = loggerFactory.CreateLogger<CohostTextDocumentSyncHandler>();

    public async Task HandleAsync(int version, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        var textDocument = context.TextDocument.AssumeNotNull();
        var textDocumentPath = context.TextDocument.FilePath.AssumeNotNull();

        _logger.LogDebug("[Cohost] DidChange for '{document}' with version {version}.", textDocumentPath, version);

        var sourceText = await textDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);

        // TODO: This is to support the non-cohost server and should eventually be removed, and replaced with code that
        //       directly triggers creation/deletion of the invisible buffers we use for generated files, as necessary,
        //       rather than using the "project system" because we don't want the project system to actually exist!
        await _projectSnapshotManagerDispatcher.RunAsync(() =>
        {
            switch (context.Method)
            {
                case Methods.TextDocumentDidOpenName:
                    _razorProjectService.OpenDocument(textDocumentPath, sourceText, version);
                    break;
                case Methods.TextDocumentDidChangeName:
                    _razorProjectService.UpdateDocument(textDocumentPath, sourceText, version);
                    break;
                case Methods.TextDocumentDidCloseName:
                    _razorProjectService.CloseDocument(textDocumentPath);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported method: " + context.Method);
            }
        }, cancellationToken).ConfigureAwait(false);

        // To handle multi-targeting, whilst we get given a TextDocument, we have to actually find all of the linked documents
        // it might have.
        var solution = context.Solution.AssumeNotNull();
        var documentIds = solution.GetDocumentIdsWithFilePath(textDocumentPath);

        foreach (var documentId in documentIds)
        {
            var document = solution.GetAdditionalDocument(documentId);
            if (document is null)
            {
                continue;
            }

            var documentSnapshot = _documentSnapshotFactory.GetOrCreate(document);
            _logger.LogDebug("[Cohost] Calling DocumentOpenedOrChangedAsync for '{document}' with version {version}.", textDocumentPath, version);
            await _openDocumentGenerator.UpdateGeneratedDocumentsAsync(documentSnapshot, version, cancellationToken).ConfigureAwait(false);
        }

        _logger.LogDebug("[Cohost] Exiting didChange for '{document}' with version {version}.", textDocumentPath, version);
    }
}

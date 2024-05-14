// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.DocumentPresentation;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentPresentation;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

internal class TextDocumentUriPresentationEndpoint(
    IRazorDocumentMappingService razorDocumentMappingService,
    IClientConnection clientConnection,
    IFilePathService filePathService,
    IDocumentContextFactory documentContextFactory,
    ILoggerFactory loggerFactory)
    : AbstractTextDocumentPresentationEndpointBase<UriPresentationParams>(razorDocumentMappingService, clientConnection, filePathService, loggerFactory.GetOrCreateLogger<TextDocumentUriPresentationEndpoint>()), ITextDocumentUriPresentationHandler
{
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));

    public override string EndpointName => CustomMessageNames.RazorUriPresentationEndpoint;

    public override void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.UriPresentationProvider = true;
    }

    public override TextDocumentIdentifier GetTextDocumentIdentifier(UriPresentationParams request)
    {
        return request.TextDocument;
    }

    protected override IRazorPresentationParams CreateRazorRequestParameters(UriPresentationParams request)
        => new RazorUriPresentationParams()
        {
            TextDocument = request.TextDocument,
            Range = request.Range,
            Uris = request.Uris
        };

    protected override async Task<WorkspaceEdit?> TryGetRazorWorkspaceEditAsync(RazorLanguageKind languageKind, UriPresentationParams request, CancellationToken cancellationToken)
    {
        if (languageKind is not RazorLanguageKind.Html)
        {
            // Component tags can only be inserted into Html contexts, so if this isn't Html there is nothing we can do.
            return null;
        }

        var razorFileUri = UriPresentationHelper.GetComponentFileNameFromUriPresentationRequest(request.Uris, Logger);
        if (razorFileUri == null)
        {
            return null;
        }

        var componentTagText = await TryGetComponentTagAsync(razorFileUri, cancellationToken).ConfigureAwait(false);
        if (componentTagText is null)
        {
            return null;
        }

        return new WorkspaceEdit
        {
            DocumentChanges = new TextDocumentEdit[]
            {
                new TextDocumentEdit
                {
                    TextDocument = new()
                    {
                        Uri = request.TextDocument.Uri
                    },
                    Edits =
                    [
                        new TextEdit
                        {
                            NewText = componentTagText,
                            Range = request.Range
                        }
                    ]
                }
            }
        };
    }

    private async Task<string?> TryGetComponentTagAsync(Uri uri, CancellationToken cancellationToken)
    {
        Logger.LogInformation($"Trying to find document info for dropped uri {uri}.");

        if (!_documentContextFactory.TryCreate(uri, out var documentContext))
        {
            Logger.LogInformation($"Failed to find document for component {uri}.");
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var descriptor = await documentContext.Snapshot.TryGetTagHelperDescriptorAsync(cancellationToken).ConfigureAwait(false);
        if (descriptor is null)
        {
            Logger.LogInformation($"Failed to find tag helper descriptor for {documentContext.Snapshot.FilePath}.");
            return null;
        }

        return descriptor.TryGetComponentTag();
    }
}

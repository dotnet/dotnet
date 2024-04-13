// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorRequestContextFactory(ILspServices lspServices) : AbstractRequestContextFactory<RazorRequestContext>
{
    private readonly ILspServices _lspServices = lspServices;

    public override async Task<RazorRequestContext> CreateRequestContextAsync<TRequestParams>(IQueueItem<RazorRequestContext> queueItem, IMethodHandler methodHandler, TRequestParams @params, CancellationToken cancellationToken)
    {
        var logger = _lspServices.GetRequiredService<ILoggerFactory>().GetOrCreateLogger<RazorRequestContextFactory>();

        VersionedDocumentContext? documentContext = null;
        var textDocumentHandler = methodHandler as ITextDocumentIdentifierHandler;

        Uri? uri = null;
        var documentContextFactory = _lspServices.GetRequiredService<IDocumentContextFactory>();
        if (textDocumentHandler is not null)
        {
            if (textDocumentHandler is ITextDocumentIdentifierHandler<TRequestParams, TextDocumentIdentifier> tdiHandler)
            {
                var textDocumentIdentifier = tdiHandler.GetTextDocumentIdentifier(@params);
                uri = textDocumentIdentifier.Uri;

                logger.LogDebug($"Trying to create DocumentContext for {queueItem.MethodName} for {textDocumentIdentifier.GetProjectContext()?.Id ?? "(no project context)"} for {uri}");

                documentContext = await documentContextFactory.TryCreateForOpenDocumentAsync(textDocumentIdentifier, cancellationToken).ConfigureAwait(false);
            }
            else if (textDocumentHandler is ITextDocumentIdentifierHandler<TRequestParams, Uri> uriHandler)
            {
                uri = uriHandler.GetTextDocumentIdentifier(@params);

                logger.LogDebug($"Trying to create DocumentContext for {queueItem.MethodName}, with no project context, for {uri}");

                documentContext = await documentContextFactory.TryCreateForOpenDocumentAsync(uri, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                throw new NotImplementedException();
            }

            if (documentContext is null)
            {
                logger.LogWarning($"Could not create a document context for {queueItem.MethodName} for {uri}. Endpoint may crash later if it calls GetRequiredDocumentContext.");
            }
        }

        var requestContext = new RazorRequestContext(documentContext, _lspServices, queueItem.MethodName, uri);

        return requestContext;
    }
}

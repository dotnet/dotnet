// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorRequestContextFactory(ILspServices lspServices) : IRequestContextFactory<RazorRequestContext>
{
    private readonly ILspServices _lspServices = lspServices;

    public Task<RazorRequestContext> CreateRequestContextAsync<TRequestParams>(IQueueItem<RazorRequestContext> queueItem, TRequestParams @params, CancellationToken cancellationToken)
    {
        var logger = _lspServices.GetRequiredService<IRazorLoggerFactory>().CreateLogger<RazorRequestContextFactory>();

        VersionedDocumentContext? documentContext = null;
        var textDocumentHandler = queueItem.MethodHandler as ITextDocumentIdentifierHandler;

        Uri? uri = null;
        var documentContextFactory = _lspServices.GetRequiredService<IDocumentContextFactory>();
        if (textDocumentHandler is not null)
        {
            if (textDocumentHandler is ITextDocumentIdentifierHandler<TRequestParams, TextDocumentIdentifier> tdiHandler)
            {
                var textDocumentIdentifier = tdiHandler.GetTextDocumentIdentifier(@params);
                uri = textDocumentIdentifier.Uri;

                logger.LogDebug("Trying to create DocumentContext for {methodName} for {projectContext} for {uri}", queueItem.MethodName, textDocumentIdentifier.GetProjectContext()?.Id ?? "(no project context)", uri);

                documentContext = documentContextFactory.TryCreateForOpenDocument(textDocumentIdentifier);
            }
            else if (textDocumentHandler is ITextDocumentIdentifierHandler<TRequestParams, Uri> uriHandler)
            {
                uri = uriHandler.GetTextDocumentIdentifier(@params);

                logger.LogDebug("Trying to create DocumentContext for {methodName}, with no project context, for {uri}", queueItem.MethodName, uri);

                documentContext = documentContextFactory.TryCreateForOpenDocument(uri);
            }
            else
            {
                throw new NotImplementedException();
            }

            if (documentContext is null)
            {
                logger.LogWarning("Could not create a document context for {methodName} for {uri}. Endpoint may crash later if it calls GetRequiredDocumentContext.", queueItem.MethodName, uri);
            }
        }

        var requestContext = new RazorRequestContext(documentContext, _lspServices, queueItem.MethodName, uri);

        return Task.FromResult(requestContext);
    }
}

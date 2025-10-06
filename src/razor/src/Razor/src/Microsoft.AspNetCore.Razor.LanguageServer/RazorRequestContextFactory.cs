// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class RazorRequestContextFactory(
    LspServices lspServices,
    IDocumentContextFactory documentContextFactory,
    ILoggerFactory loggerFactory) : AbstractRequestContextFactory<RazorRequestContext>
{
    private readonly LspServices _lspServices = lspServices;
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorRequestContextFactory>();

    public override Task<RazorRequestContext> CreateRequestContextAsync<TRequestParams>(IQueueItem<RazorRequestContext> queueItem, IMethodHandler methodHandler, TRequestParams @params, CancellationToken cancellationToken)
    {
        DocumentContext? documentContext = null;
        Uri? uri = null;

        if (methodHandler is ITextDocumentIdentifierHandler textDocumentHandler)
        {
            if (textDocumentHandler is ITextDocumentIdentifierHandler<TRequestParams, TextDocumentIdentifier> tdiHandler)
            {
                var textDocumentIdentifier = tdiHandler.GetTextDocumentIdentifier(@params);
                uri = textDocumentIdentifier.DocumentUri.GetRequiredParsedUri();

                _logger.LogDebug($"Trying to create DocumentContext for {queueItem.MethodName} for {textDocumentIdentifier.GetProjectContext()?.Id ?? "(no project context)"} for {uri}");

                _documentContextFactory.TryCreate(textDocumentIdentifier, out documentContext);
            }
            else if (textDocumentHandler is ITextDocumentIdentifierHandler<TRequestParams, Uri> uriHandler)
            {
                uri = uriHandler.GetTextDocumentIdentifier(@params);

                _logger.LogDebug($"Trying to create DocumentContext for {queueItem.MethodName}, with no project context, for {uri}");

                _documentContextFactory.TryCreate(uri, out documentContext);
            }
            else
            {
                return Assumed.Unreachable<Task<RazorRequestContext>>();
            }

            if (documentContext is null)
            {
                _logger.LogWarning($"Could not create a document context for {queueItem.MethodName} for {uri}. Endpoint may crash later if it calls GetRequiredDocumentContext.");
            }
        }

        var requestContext = new RazorRequestContext(documentContext, _lspServices, queueItem.MethodName, uri);

        return Task.FromResult(requestContext);
    }
}

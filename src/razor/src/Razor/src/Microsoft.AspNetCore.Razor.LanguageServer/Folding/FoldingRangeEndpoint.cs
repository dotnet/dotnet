// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.FoldingRanges;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Folding;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

[RazorLanguageServerEndpoint(Methods.TextDocumentFoldingRangeName)]
internal sealed class FoldingRangeEndpoint(
    IClientConnection clientConnection,
    IFoldingRangeService foldingRangeService,
    ILoggerFactory loggerFactory)
    : IRazorRequestHandler<FoldingRangeParams, IEnumerable<FoldingRange>?>, ICapabilitiesProvider
{
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly IFoldingRangeService _foldingRangeService = foldingRangeService;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<FoldingRangeEndpoint>();

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.FoldingRangeProvider = new FoldingRangeOptions();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(FoldingRangeParams request)
    {
        return request.TextDocument;
    }

    public async Task<IEnumerable<FoldingRange>?> HandleRequestAsync(FoldingRangeParams @params, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var requestParams = new RazorFoldingRangeRequestParam
        {
            HostDocumentVersion = documentContext.Snapshot.Version,
            TextDocument = @params.TextDocument,
        };

        IEnumerable<FoldingRange>? foldingRanges = null;
        var retries = 0;
        const int MaxRetries = 5;

        while (foldingRanges is null && ++retries <= MaxRetries)
        {
            try
            {
                foldingRanges = await HandleCoreAsync(requestParams, documentContext, cancellationToken).ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception e) when (e is not OperationCanceledException && retries < MaxRetries)
            {
                _logger.LogWarning(e, $"Try {retries} to get FoldingRange");
            }
        }

        return foldingRanges;
    }

    private async Task<ImmutableArray<FoldingRange>?> HandleCoreAsync(RazorFoldingRangeRequestParam requestParams, DocumentContext documentContext, CancellationToken cancellationToken)
    {
        var foldingResponse = await _clientConnection.SendRequestAsync<RazorFoldingRangeRequestParam, RazorFoldingRangeResponse?>(
            CustomMessageNames.RazorFoldingRangeEndpoint,
            requestParams,
            cancellationToken).ConfigureAwait(false);
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (foldingResponse is null)
        {
            return null;
        }

        return _foldingRangeService.GetFoldingRanges(codeDocument, foldingResponse.CSharpRanges, foldingResponse.HtmlRanges, cancellationToken);
    }
}

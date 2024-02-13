// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

[RazorLanguageServerEndpoint(Methods.TextDocumentSemanticTokensRangeName)]
internal sealed class SemanticTokensRangeEndpoint(
    IRazorSemanticTokensInfoService semanticTokensInfoService,
    RazorSemanticTokensLegendService razorSemanticTokensLegendService,
    RazorLSPOptionsMonitor razorLSPOptionsMonitor,
    IClientConnection clientConnection)
    : IRazorRequestHandler<SemanticTokensRangeParams, SemanticTokens?>, ICapabilitiesProvider
{
    private readonly IRazorSemanticTokensInfoService _semanticTokensInfoService = semanticTokensInfoService;
    private readonly RazorSemanticTokensLegendService _razorSemanticTokensLegendService = razorSemanticTokensLegendService;
    private readonly RazorLSPOptionsMonitor _razorLSPOptionsMonitor = razorLSPOptionsMonitor;
    private readonly IClientConnection _clientConnection = clientConnection;

    public bool MutatesSolutionState { get; } = false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.EnableSemanticTokens(_razorSemanticTokensLegendService.Legend);
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(SemanticTokensRangeParams request)
    {
        return request.TextDocument;
    }

    public async Task<SemanticTokens?> HandleRequestAsync(SemanticTokensRangeParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        var colorBackground = _razorLSPOptionsMonitor.CurrentValue.ColorBackground;

        var semanticTokens = await _semanticTokensInfoService.GetSemanticTokensAsync(_clientConnection, request.TextDocument, request.Range, documentContext, colorBackground, cancellationToken).ConfigureAwait(false);

        return semanticTokens;
    }
}

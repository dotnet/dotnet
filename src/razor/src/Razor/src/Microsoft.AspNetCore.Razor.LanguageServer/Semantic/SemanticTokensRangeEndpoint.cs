// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

[LanguageServerEndpoint(LspEndpointName)]
internal sealed class SemanticTokensRangeEndpoint : IRazorRequestHandler<SemanticTokensRangeParams, SemanticTokens?>, ICapabilitiesProvider
{
    public const string LspEndpointName = Methods.TextDocumentSemanticTokensRangeName;
    private RazorSemanticTokensLegend? _razorSemanticTokensLegend;
    private readonly ITelemetryReporter? _telemetryReporter;

    public SemanticTokensRangeEndpoint(ITelemetryReporter? telemetryReporter)
    {
        _telemetryReporter = telemetryReporter;
    }

    public bool MutatesSolutionState { get; } = false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _razorSemanticTokensLegend = new RazorSemanticTokensLegend(clientCapabilities);

        serverCapabilities.SemanticTokensOptions = new SemanticTokensOptions
        {
            Full = false,
            Legend = _razorSemanticTokensLegend.Legend,
            Range = true,
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(SemanticTokensRangeParams request)
    {
        return request.TextDocument;
    }

    public async Task<SemanticTokens?> HandleRequestAsync(SemanticTokensRangeParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var documentContext = requestContext.GetRequiredDocumentContext();
        var semanticTokensInfoService = requestContext.GetRequiredService<IRazorSemanticTokensInfoService>();

        var correlationId = Guid.NewGuid();
        using var _ = _telemetryReporter?.TrackLspRequest(LspEndpointName, LanguageServerConstants.RazorLanguageServerName, correlationId);
        var semanticTokens = await semanticTokensInfoService.GetSemanticTokensAsync(request.TextDocument, request.Range, documentContext, _razorSemanticTokensLegend.AssumeNotNull(), correlationId, cancellationToken).ConfigureAwait(false);
        var amount = semanticTokens is null ? "no" : (semanticTokens.Data.Length / 5).ToString(Thread.CurrentThread.CurrentCulture);

        requestContext.Logger.LogInformation("Returned {amount} semantic tokens for range ({startLine},{startChar})-({endLine},{endChar}) in {request.TextDocument.Uri}.", amount, request.Range.Start.Line, request.Range.Start.Character, request.Range.End.Line, request.Range.End.Character, request.TextDocument.Uri);

        if (semanticTokens is not null)
        {
            Debug.Assert(semanticTokens.Data.Length % 5 == 0, $"Number of semantic token-ints should be divisible by 5. Actual number: {semanticTokens.Data.Length}");
            Debug.Assert(semanticTokens.Data.Length == 0 || semanticTokens.Data[0] >= 0, $"Line offset should not be negative.");
        }

        return semanticTokens;
    }
}

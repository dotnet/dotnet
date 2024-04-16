// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.Extensions;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentSemanticTokensRangeName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostSemanticTokensRangeEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostSemanticTokensRangeEndpoint(
    IOutOfProcSemanticTokensService semanticTokensInfoService,
    ISemanticTokensLegendService semanticTokensLegendService,
    ITelemetryReporter telemetryReporter,
    ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<SemanticTokensRangeParams, SemanticTokens?>, IDynamicRegistrationProvider
{
    private readonly IOutOfProcSemanticTokensService _semanticTokensInfoService = semanticTokensInfoService;
    private readonly ISemanticTokensLegendService _semanticTokensLegendService = semanticTokensLegendService;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostSemanticTokensRangeEndpoint>();

    protected override bool MutatesSolutionState => false;
    protected override bool RequiresLSPSolution => true;

    public Registration? GetRegistration(VSInternalClientCapabilities clientCapabilities, DocumentFilter[] filter, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.SemanticTokens?.DynamicRegistration == true)
        {
            var semanticTokensRefreshQueue = requestContext.GetRequiredService<IRazorSemanticTokensRefreshQueue>();
            var clientCapabilitiesString = JsonConvert.SerializeObject(clientCapabilities);
            semanticTokensRefreshQueue.Initialize(clientCapabilitiesString);

            return new Registration()
            {
                Method = Methods.TextDocumentSemanticTokensRangeName,
                RegisterOptions = new SemanticTokensRegistrationOptions()
                {
                    DocumentSelector = filter,
                }.EnableSemanticTokens(_semanticTokensLegendService)
            };
        }

        return null;
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(SemanticTokensRangeParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override async Task<SemanticTokens?> HandleRequestAsync(SemanticTokensRangeParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        var correlationId = Guid.NewGuid();
        using var _ = _telemetryReporter.TrackLspRequest(Methods.TextDocumentSemanticTokensRangeName, RazorLSPConstants.CohostLanguageServerName, correlationId);

        var data = await _semanticTokensInfoService.GetSemanticTokensDataAsync(context.TextDocument.AssumeNotNull(), request.Range.ToLinePositionSpan(), correlationId, cancellationToken);

        if (data is null)
        {
            return null;
        }

        return new SemanticTokens
        {
            Data = data
        };
    }
}

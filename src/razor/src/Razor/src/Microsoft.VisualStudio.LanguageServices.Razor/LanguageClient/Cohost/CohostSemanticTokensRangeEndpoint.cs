// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces.Telemetry;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Settings;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(Methods.TextDocumentSemanticTokensRangeName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostSemanticTokensRangeEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostSemanticTokensRangeEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IClientSettingsManager clientSettingsManager,
    ISemanticTokensLegendService semanticTokensLegendService,
    ITelemetryReporter telemetryReporter)
    : AbstractRazorCohostDocumentRequestHandler<SemanticTokensRangeParams, SemanticTokens?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly ISemanticTokensLegendService _semanticTokensLegendService = semanticTokensLegendService;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    protected override bool MutatesSolutionState => false;
    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.SemanticTokens?.DynamicRegistration == true)
        {
            var semanticTokensRefreshQueue = requestContext.GetRequiredService<IRazorSemanticTokensRefreshQueue>();
            var clientCapabilitiesString = JsonSerializer.Serialize(clientCapabilities);
            semanticTokensRefreshQueue.Initialize(clientCapabilitiesString);

            return [new Registration()
            {
                Method = Methods.TextDocumentSemanticTokensRangeName,
                RegisterOptions = new SemanticTokensRegistrationOptions()
                    .EnableSemanticTokens(_semanticTokensLegendService)
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(SemanticTokensRangeParams request)
        => request.TextDocument.ToRazorTextDocumentIdentifier();

    protected override Task<SemanticTokens?> HandleRequestAsync(SemanticTokensRangeParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        var razorDocument = context.TextDocument.AssumeNotNull();
        var span = request.Range.ToLinePositionSpan();

        return HandleRequestAsync(razorDocument, span, cancellationToken);
    }

    private async Task<SemanticTokens?> HandleRequestAsync(TextDocument razorDocument, LinePositionSpan span, CancellationToken cancellationToken)
    {
        var colorBackground = _clientSettingsManager.GetClientSettings().AdvancedSettings.ColorBackground;

        var correlationId = Guid.NewGuid();
        using var _ = _telemetryReporter.TrackLspRequest(Methods.TextDocumentSemanticTokensRangeName, RazorLSPConstants.CohostLanguageServerName, TelemetryThresholds.SemanticTokensRazorTelemetryThreshold, correlationId);

        var tokens = await _remoteServiceInvoker.TryInvokeAsync<IRemoteSemanticTokensService, int[]?>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetSemanticTokensDataAsync(solutionInfo, razorDocument.Id, span, colorBackground, correlationId, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (tokens is not null)
        {
            return new SemanticTokens
            {
                Data = tokens
            };
        }

        return null;
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostSemanticTokensRangeEndpoint instance)
    {
        public Task<SemanticTokens?> HandleRequestAsync(TextDocument razorDocument, LinePositionSpan span, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(razorDocument, span, cancellationToken);
    }
}

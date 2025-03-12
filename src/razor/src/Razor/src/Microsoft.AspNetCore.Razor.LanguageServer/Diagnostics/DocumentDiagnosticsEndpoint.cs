// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces.Telemetry;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Diagnostics;

[RazorLanguageServerEndpoint(Methods.TextDocumentDiagnosticName)]
internal sealed class DocumentDiagnosticsEndpoint(
    RazorTranslateDiagnosticsService translateDiagnosticsService,
    IClientConnection clientConnection,
    ITelemetryReporter? telemetryReporter)
    : IRazorRequestHandler<DocumentDiagnosticParams, FullDocumentDiagnosticReport?>, ICapabilitiesProvider
{
    private readonly RazorTranslateDiagnosticsService _translateDiagnosticsService = translateDiagnosticsService;
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly ITelemetryReporter? _telemetryReporter = telemetryReporter;
    private readonly MissingTagHelperTelemetryReporter? _missingTagHelperTelemetryReporter = telemetryReporter is null ? null : new(telemetryReporter);

    public bool MutatesSolutionState => false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(DocumentDiagnosticParams request)
        => request.TextDocument;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.SupportsDiagnosticRequests = true;
        serverCapabilities.DiagnosticOptions = new()
        {
            InterFileDependencies = false,
            WorkspaceDiagnostics = false,
            WorkDoneProgress = false
        };
    }

    public async Task<FullDocumentDiagnosticReport?> HandleRequestAsync(DocumentDiagnosticParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        var documentContext = context.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var correlationId = Guid.NewGuid();
        using var __ = _telemetryReporter?.TrackLspRequest(Methods.TextDocumentDiagnosticName, LanguageServerConstants.RazorLanguageServerName, TelemetryThresholds.DiagnosticsRazorTelemetryThreshold, correlationId);

        var documentSnapshot = documentContext.Snapshot;
        var razorDiagnostics = await RazorDiagnosticHelper.GetRazorDiagnosticsAsync(documentSnapshot, cancellationToken).ConfigureAwait(false);
        var csharpDiagnostics = await GetCSharpDiagnosticsAsync(documentSnapshot, request.TextDocument, correlationId, cancellationToken).ConfigureAwait(false);

        var diagnosticCount =
            (razorDiagnostics?.Length ?? 0) +
            (csharpDiagnostics?.Length ?? 0);

        using var _ = ListPool<Diagnostic>.GetPooledObject(out var allDiagnostics);
        allDiagnostics.SetCapacityIfLarger(diagnosticCount);

        if (razorDiagnostics is not null)
        {
            // No extra work to do for Razor diagnostics
            allDiagnostics.AddRange(razorDiagnostics);

            if (_missingTagHelperTelemetryReporter is not null)
            {
                await _missingTagHelperTelemetryReporter.ReportRZ10012TelemetryAsync(documentContext, razorDiagnostics, cancellationToken).ConfigureAwait(false);
            }
        }

        if (csharpDiagnostics is not null)
        {
            var mappedDiagnostics = await _translateDiagnosticsService
                        .TranslateAsync(RazorLanguageKind.CSharp, csharpDiagnostics, documentSnapshot, cancellationToken)
                        .ConfigureAwait(false);
            allDiagnostics.AddRange(mappedDiagnostics);
        }

        return new()
        {
            Items = [.. allDiagnostics]
        };
    }

    private async Task<Diagnostic[]?> GetCSharpDiagnosticsAsync(IDocumentSnapshot documentSnapshot, TextDocumentIdentifier razorDocumentIdentifier, Guid correlationId, CancellationToken cancellationToken)
    {
        var delegatedParams = new DelegatedDiagnosticParams(
            new(razorDocumentIdentifier, documentSnapshot.Version),
            correlationId
        );

        var delegatedResponse = await _clientConnection
                   .SendRequestAsync<DelegatedDiagnosticParams, SumType<FullDocumentDiagnosticReport, UnchangedDocumentDiagnosticReport>?>(
                       CustomMessageNames.RazorCSharpPullDiagnosticsEndpointName,
                       delegatedParams,
                       cancellationToken)
                   .ConfigureAwait(false);

        return delegatedResponse.HasValue
            ? delegatedResponse.Value.TryGetFirst(out var fullReport)
                ? fullReport.Items
                : null
            : null;
    }
}

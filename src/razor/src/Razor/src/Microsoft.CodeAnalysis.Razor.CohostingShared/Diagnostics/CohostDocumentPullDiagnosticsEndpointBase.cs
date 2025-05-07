// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal abstract class CohostDocumentPullDiagnosticsEndpointBase<TRequest, TResponse>(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlRequestInvoker requestInvoker,
    IClientCapabilitiesService clientCapabilitiesService,
    ITelemetryReporter telemetryReporter,
    ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlRequestInvoker _requestInvoker = requestInvoker;
    private readonly IClientCapabilitiesService _clientCapabilitiesService = clientCapabilitiesService;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostDocumentPullDiagnosticsEndpointBase<TRequest, TResponse>>();

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    protected abstract string LspMethodName { get; }
    protected abstract bool SupportsHtmlDiagnostics { get; }

    protected virtual LspDiagnostic[] ExtractHtmlDiagnostics(TResponse result)
    {
        throw new NotSupportedException("If SupportsHtmlDiagnostics is true, you must implement GetHtmlDiagnostics");
    }

    protected virtual TRequest CreateHtmlParams(Uri uri)
    {
        throw new NotSupportedException("If SupportsHtmlDiagnostics is true, you must implement CreateHtmlParams");
    }

    protected async Task<LspDiagnostic[]?> GetDiagnosticsAsync(TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var correlationId = Guid.NewGuid();
        using var _ = _telemetryReporter.TrackLspRequest(LspMethodName, LanguageServerConstants.RazorLanguageServerName, TelemetryThresholds.DiagnosticsRazorTelemetryThreshold, correlationId);

        // Diagnostics is a little different, because Roslyn is not designed to run diagnostics in OOP. Their system will transition to OOP
        // as it needs, but we have to start here in devenv. This is not as big a problem as it sounds, specifically for diagnostics, because
        // we only need to tell Roslyn the document we need diagnostics for. If we had to map positions or ranges etc. it would be worse
        // because we'd have to transition to our OOP to find out that info, then back here to get the diagnostics, then back to OOP to process.
        _logger.LogDebug($"Getting diagnostics for {razorDocument.FilePath}");

        var csharpTask = GetCSharpDiagnosticsAsync(razorDocument, correlationId, cancellationToken);
        var htmlTask = SupportsHtmlDiagnostics
            ? GetHtmlDiagnosticsAsync(razorDocument, correlationId, cancellationToken)
            : SpecializedTasks.EmptyArray<LspDiagnostic>();

        try
        {
            await Task.WhenAll(htmlTask, csharpTask).ConfigureAwait(false);
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            _logger.LogError(e, $"Exception thrown in PullDiagnostic delegation");
            throw;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        var csharpDiagnostics = csharpTask.VerifyCompleted();
        var htmlDiagnostics = htmlTask.VerifyCompleted();

        _logger.LogDebug($"Calling OOP with the {csharpDiagnostics.Length} C# and {htmlDiagnostics.Length} Html diagnostics");
        var diagnostics = await _remoteServiceInvoker.TryInvokeAsync<IRemoteDiagnosticsService, ImmutableArray<LspDiagnostic>>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetDiagnosticsAsync(solutionInfo, razorDocument.Id, csharpDiagnostics, htmlDiagnostics, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (diagnostics.IsDefaultOrEmpty)
        {
            return null;
        }

        _logger.LogDebug($"Reporting {diagnostics.Length} diagnostics back to the client");
        return diagnostics.ToArray();
    }

    private async Task<LspDiagnostic[]> GetCSharpDiagnosticsAsync(TextDocument razorDocument, Guid correletionId, CancellationToken cancellationToken)
    {
        if (!razorDocument.TryComputeHintNameFromRazorDocument(out var hintName) ||
            await razorDocument.Project.TryGetSourceGeneratedDocumentFromHintNameAsync(hintName, cancellationToken).ConfigureAwait(false) is not { } generatedDocument)
        {
            return [];
        }

        _logger.LogDebug($"Getting C# diagnostics for {generatedDocument.FilePath}");

        using var _ = _telemetryReporter.TrackLspRequest(LspMethodName, "Razor.ExternalAccess", TelemetryThresholds.DiagnosticsSubLSPTelemetryThreshold, correletionId);
        var supportsVisualStudioExtensions = _clientCapabilitiesService.ClientCapabilities.SupportsVisualStudioExtensions;
        var diagnostics = await ExternalHandlers.Diagnostics.GetDocumentDiagnosticsAsync(generatedDocument, supportsVisualStudioExtensions, cancellationToken).ConfigureAwait(false);
        return diagnostics.ToArray();
    }

    private async Task<LspDiagnostic[]> GetHtmlDiagnosticsAsync(TextDocument razorDocument, Guid correletionId, CancellationToken cancellationToken)
    {
        var diagnosticsParams = CreateHtmlParams(razorDocument.CreateUri());

        var result = await _requestInvoker.MakeHtmlLspRequestAsync<TRequest, TResponse>(
            razorDocument,
            LspMethodName,
            diagnosticsParams,
            TelemetryThresholds.DiagnosticsSubLSPTelemetryThreshold,
            correletionId,
            cancellationToken).ConfigureAwait(false);

        if (result is null)
        {
            return [];
        }

        return ExtractHtmlDiagnostics(result);
    }
}

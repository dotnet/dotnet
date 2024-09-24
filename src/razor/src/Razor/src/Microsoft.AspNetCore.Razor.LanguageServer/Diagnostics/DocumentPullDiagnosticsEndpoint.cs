// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Diagnostics;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Diagnostics;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

[RazorLanguageServerEndpoint(VSInternalMethods.DocumentPullDiagnosticName)]
internal class DocumentPullDiagnosticsEndpoint : IRazorRequestHandler<VSInternalDocumentDiagnosticsParams, IEnumerable<VSInternalDiagnosticReport>?>, ICapabilitiesProvider
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly IClientConnection _clientConnection;
    private readonly RazorTranslateDiagnosticsService _translateDiagnosticsService;
    private readonly ITelemetryReporter? _telemetryReporter;
    private ImmutableDictionary<ProjectKey, int> _lastReporedProjectTagHelperCount = ImmutableDictionary<ProjectKey, int>.Empty;

    public DocumentPullDiagnosticsEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        RazorTranslateDiagnosticsService translateDiagnosticsService,
        IClientConnection clientConnection,
        ITelemetryReporter? telemetryReporter)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        _translateDiagnosticsService = translateDiagnosticsService ?? throw new ArgumentNullException(nameof(translateDiagnosticsService));
        _clientConnection = clientConnection ?? throw new ArgumentNullException(nameof(clientConnection));
        _telemetryReporter = telemetryReporter;
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.SupportsDiagnosticRequests = true;
        serverCapabilities.DiagnosticProvider ??= new();
        serverCapabilities.DiagnosticProvider.DiagnosticKinds = [VSInternalDiagnosticKind.Syntax];
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(VSInternalDocumentDiagnosticsParams request)
    {
        if (request.TextDocument is null)
        {
            throw new ArgumentNullException(nameof(request.TextDocument));
        }

        return request.TextDocument;
    }

    public async Task<IEnumerable<VSInternalDiagnosticReport>?> HandleRequestAsync(VSInternalDocumentDiagnosticsParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        if (!_languageServerFeatureOptions.SingleServerSupport)
        {
            return default;
        }

        var correlationId = Guid.NewGuid();
        using var __ = _telemetryReporter?.TrackLspRequest(VSInternalMethods.DocumentPullDiagnosticName, LanguageServerConstants.RazorLanguageServerName, correlationId);
        var documentContext = context.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var documentSnapshot = documentContext.Snapshot;

        var razorDiagnostics = await GetRazorDiagnosticsAsync(documentSnapshot).ConfigureAwait(false);

        await ReportRZ10012TelemetryAsync(documentContext, razorDiagnostics, cancellationToken).ConfigureAwait(false);

        var (csharpDiagnostics, htmlDiagnostics) = await GetHtmlCSharpDiagnosticsAsync(documentContext, correlationId, cancellationToken).ConfigureAwait(false);

        var diagnosticCount =
            (razorDiagnostics?.Length ?? 0) +
            (csharpDiagnostics?.Length ?? 0) +
            (htmlDiagnostics?.Length ?? 0);

        using var _ = ListPool<VSInternalDiagnosticReport>.GetPooledObject(out var allDiagnostics);
        allDiagnostics.SetCapacityIfLarger(diagnosticCount);

        if (razorDiagnostics is not null)
        {
            // No extra work to do for Razor diagnostics
            allDiagnostics.AddRange(razorDiagnostics);
        }

        if (csharpDiagnostics is not null)
        {
            foreach (var report in csharpDiagnostics)
            {
                if (report.Diagnostics is not null)
                {
                    var mappedDiagnostics = await _translateDiagnosticsService.TranslateAsync(RazorLanguageKind.CSharp, report.Diagnostics, documentSnapshot).ConfigureAwait(false);
                    report.Diagnostics = mappedDiagnostics;
                }

                allDiagnostics.Add(report);
            }
        }

        if (htmlDiagnostics is not null)
        {
            foreach (var report in htmlDiagnostics)
            {
                if (report.Diagnostics is not null)
                {
                    var mappedDiagnostics = await _translateDiagnosticsService.TranslateAsync(RazorLanguageKind.Html, report.Diagnostics, documentSnapshot).ConfigureAwait(false);
                    report.Diagnostics = mappedDiagnostics;
                }

                allDiagnostics.Add(report);
            }
        }

        return allDiagnostics.ToArray();
    }

    private static async Task<VSInternalDiagnosticReport[]?> GetRazorDiagnosticsAsync(IDocumentSnapshot documentSnapshot)
    {
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        var sourceText = codeDocument.Source.Text;
        var csharpDocument = codeDocument.GetCSharpDocument();
        var diagnostics = csharpDocument.Diagnostics;

        if (diagnostics.Length == 0)
        {
            return null;
        }

        var convertedDiagnostics = RazorDiagnosticConverter.Convert(diagnostics, sourceText, documentSnapshot);

        return
        [
            new()
            {
                Diagnostics = convertedDiagnostics,
                ResultId = Guid.NewGuid().ToString()
            }
        ];
    }

    private async Task<(VSInternalDiagnosticReport[]? CSharpDiagnostics, VSInternalDiagnosticReport[]? HtmlDiagnostics)> GetHtmlCSharpDiagnosticsAsync(DocumentContext documentContext, Guid correlationId, CancellationToken cancellationToken)
    {
        var delegatedParams = new DelegatedDiagnosticParams(documentContext.GetTextDocumentIdentifierAndVersion(), correlationId);
        var delegatedResponse = await _clientConnection.SendRequestAsync<DelegatedDiagnosticParams, RazorPullDiagnosticResponse?>(
            CustomMessageNames.RazorPullDiagnosticEndpointName,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        if (delegatedResponse is null)
        {
            return (null, null);
        }

        return (delegatedResponse.CSharpDiagnostics, delegatedResponse.HtmlDiagnostics);
    }

    /// <summary>
    /// Reports telemetry for RZ10012 "Found markup element with unexpected name" to help track down potential issues
    /// with taghelpers being discovered (or lack thereof)
    /// </summary>
    private async ValueTask ReportRZ10012TelemetryAsync(DocumentContext documentContext, VSInternalDiagnosticReport[]? razorDiagnostics, CancellationToken cancellationToken)
    {
        if (razorDiagnostics is null)
        {
            return;
        }

        if (_telemetryReporter is null)
        {
            return;
        }

        var relevantDiagnosticsCount = razorDiagnostics.Sum(CountDiagnostics);
        if (relevantDiagnosticsCount == 0)
        {
            return;
        }

        var tagHelpers = await documentContext.Project.GetTagHelpersAsync(cancellationToken).ConfigureAwait(false);
        var tagHelperCount = tagHelpers.Count();
        var shouldReport = false;

        ImmutableInterlocked.AddOrUpdate(
            ref _lastReporedProjectTagHelperCount,
            documentContext.Project.Key,
            (k) =>
            {
                shouldReport = true;
                return tagHelperCount;
            },
            (k, currentValue) =>
            {
                shouldReport = currentValue != tagHelperCount;
                return tagHelperCount;
            });

        if (shouldReport)
        {
            _telemetryReporter.ReportEvent(
                "RZ10012",
                Severity.Low,
                new("tagHelpers", tagHelperCount),
                new("RZ10012errors", relevantDiagnosticsCount),
                new("project", documentContext.Project.Key.Id));
        }

        static int CountDiagnostics(VSInternalDiagnosticReport report)
            => report.Diagnostics?.Count(d => d.Code == ComponentDiagnosticFactory.UnexpectedMarkupElement.Id)
            ?? 0;
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

[LanguageServerEndpoint(VSInternalMethods.DocumentPullDiagnosticName)]
internal class DocumentPullDiagnosticsEndpoint : IRazorRequestHandler<VSInternalDocumentDiagnosticsParams, IEnumerable<VSInternalDiagnosticReport>?>, ICapabilitiesProvider
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly RazorTranslateDiagnosticsService _translateDiagnosticsService;
    private readonly ITelemetryReporter? _telemetryReporter;

    public DocumentPullDiagnosticsEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        RazorTranslateDiagnosticsService translateDiagnosticsService,
        ClientNotifierServiceBase languageServer,
        ITelemetryReporter? telemetryReporter)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        _translateDiagnosticsService = translateDiagnosticsService ?? throw new ArgumentNullException(nameof(translateDiagnosticsService));
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
        _telemetryReporter = telemetryReporter;
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.SupportsDiagnosticRequests = true;
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
        var documentContext = context.GetRequiredDocumentContext();

        var razorDiagnostics = await GetRazorDiagnosticsAsync(documentContext, cancellationToken).ConfigureAwait(false);

        var (csharpDiagnostics, htmlDiagnostics) = await GetHtmlCSharpDiagnosticsAsync(documentContext, correlationId, cancellationToken).ConfigureAwait(false);

        using var _ = ListPool<VSInternalDiagnosticReport>.GetPooledObject(out var allDiagnostics);
        allDiagnostics.SetCapacityIfLarger(
            (razorDiagnostics?.Length ?? 0) +
            (csharpDiagnostics?.Length ?? 0) +
            (htmlDiagnostics?.Length ?? 0));

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
                    var mappedDiagnostics = await _translateDiagnosticsService.TranslateAsync(RazorLanguageKind.CSharp, report.Diagnostics, documentContext, cancellationToken).ConfigureAwait(false);
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
                    var mappedDiagnostics = await _translateDiagnosticsService.TranslateAsync(RazorLanguageKind.Html, report.Diagnostics, documentContext, cancellationToken).ConfigureAwait(false);
                    report.Diagnostics = mappedDiagnostics;
                }

                allDiagnostics.Add(report);
            }
        }

        return allDiagnostics.ToArray();
    }

    private static async Task<VSInternalDiagnosticReport[]?> GetRazorDiagnosticsAsync(VersionedDocumentContext documentContext, CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var csharpDocument = codeDocument.GetCSharpDocument();
        var diagnostics = csharpDocument.Diagnostics;

        if (diagnostics.Count == 0)
        {
            return null;
        }

        var convertedDiagnostics = RazorDiagnosticConverter.Convert(diagnostics, sourceText);

        var razorDiagnostics = new VSInternalDiagnosticReport[]
        {
            new VSInternalDiagnosticReport
            {
                Diagnostics = convertedDiagnostics,
                ResultId = Guid.NewGuid().ToString()
            }
        };
        return razorDiagnostics;
    }

    private async Task<(VSInternalDiagnosticReport[]? CSharpDiagnostics, VSInternalDiagnosticReport[]? HtmlDiagnostics)> GetHtmlCSharpDiagnosticsAsync(VersionedDocumentContext documentContext, Guid correlationId, CancellationToken cancellationToken)
    {
        var delegatedParams = new DelegatedDiagnosticParams(documentContext.Identifier, correlationId);
        var delegatedResponse = await _languageServer.SendRequestAsync<DelegatedDiagnosticParams, RazorPullDiagnosticResponse?>(
            CustomMessageNames.RazorPullDiagnosticEndpointName,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        if (delegatedResponse is null)
        {
            return (null, null);
        }

        return (delegatedResponse.CSharpDiagnostics, delegatedResponse.HtmlDiagnostics);
    }
}

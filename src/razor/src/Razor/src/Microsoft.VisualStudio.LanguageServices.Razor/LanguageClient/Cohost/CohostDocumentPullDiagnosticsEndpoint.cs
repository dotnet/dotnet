// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Razor.Settings;
using ExternalHandlers = Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using LspDiagnostic = Microsoft.VisualStudio.LanguageServer.Protocol.Diagnostic;
using RoslynDiagnostic = Roslyn.LanguageServer.Protocol.Diagnostic;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(VSInternalMethods.DocumentPullDiagnosticName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostDocumentPullDiagnosticsEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal class CohostDocumentPullDiagnosticsEndpoint(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentSynchronizer htmlDocumentSynchronizer,
    LSPRequestInvoker requestInvoker,
    IClientSettingsManager clientSettingsManager,
    ILoggerFactory loggerFactory)
    : AbstractRazorCohostDocumentRequestHandler<VSInternalDocumentDiagnosticsParams, VSInternalDiagnosticReport[]?>, IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentSynchronizer _htmlDocumentSynchronizer = htmlDocumentSynchronizer;
    private readonly LSPRequestInvoker _requestInvoker = requestInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostDocumentPullDiagnosticsEndpoint>();

    protected override bool MutatesSolutionState => false;

    protected override bool RequiresLSPSolution => true;

    public ImmutableArray<Registration> GetRegistrations(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext)
    {
        if (clientCapabilities.TextDocument?.Diagnostic?.DynamicRegistration is true)
        {
            return [new Registration()
            {
                Method = VSInternalMethods.DocumentPullDiagnosticName,
                RegisterOptions = new VSInternalDiagnosticRegistrationOptions()
                {
                    DiagnosticKinds = [VSInternalDiagnosticKind.Syntax, VSInternalDiagnosticKind.Task]
                }
            }];
        }

        return [];
    }

    protected override RazorTextDocumentIdentifier? GetRazorTextDocumentIdentifier(VSInternalDocumentDiagnosticsParams request)
        => request.TextDocument?.ToRazorTextDocumentIdentifier();

    protected override Task<VSInternalDiagnosticReport[]?> HandleRequestAsync(VSInternalDocumentDiagnosticsParams request, RazorCohostRequestContext context, CancellationToken cancellationToken)
    {
        if (request.QueryingDiagnosticKind?.Value == VSInternalDiagnosticKind.Task.Value)
        {
            return HandleTaskListItemRequestAsync(
                context.TextDocument.AssumeNotNull(),
                _clientSettingsManager.GetClientSettings().AdvancedSettings.TaskListDescriptors,
                cancellationToken);
        }

        return HandleRequestAsync(context.TextDocument.AssumeNotNull(), cancellationToken);
    }

    private async Task<VSInternalDiagnosticReport[]?> HandleTaskListItemRequestAsync(TextDocument razorDocument, ImmutableArray<string> taskListDescriptors, CancellationToken cancellationToken)
    {
        var diagnostics = await _remoteServiceInvoker.TryInvokeAsync<IRemoteDiagnosticsService, ImmutableArray<LspDiagnostic>>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetTaskListDiagnosticsAsync(solutionInfo, razorDocument.Id, taskListDescriptors, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (diagnostics.IsDefaultOrEmpty)
        {
            return null;
        }

        return
        [
            new()
            {
                Diagnostics = [.. diagnostics],
                ResultId = Guid.NewGuid().ToString()
            }
        ];
    }

    private async Task<VSInternalDiagnosticReport[]?> HandleRequestAsync(TextDocument razorDocument, CancellationToken cancellationToken)
    {
        // Diagnostics is a little different, because Roslyn is not designed to run diagnostics in OOP. Their system will transition to OOP
        // as it needs, but we have to start here in devenv. This is not as big a problem as it sounds, specifically for diagnostics, because
        // we only need to tell Roslyn the document we need diagnostics for. If we had to map positions or ranges etc. it would be worse
        // because we'd have to transition to our OOP to find out that info, then back here to get the diagnostics, then back to OOP to process.
        _logger.LogDebug($"Getting diagnostics for {razorDocument.FilePath}");

        var csharpTask = GetCSharpDiagnosticsAsync(razorDocument, cancellationToken);
        var htmlTask = GetHtmlDiagnosticsAsync(razorDocument, cancellationToken);

        try
        {
            await Task.WhenAll(htmlTask, csharpTask).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (e is not OperationCanceledException)
            {
                _logger.LogError(e, $"Exception thrown in PullDiagnostic delegation");
                throw;
            }
        }

        var csharpDiagnostics = await csharpTask.ConfigureAwait(false);
        var htmlDiagnostics = await htmlTask.ConfigureAwait(false);

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
        return
        [
            new()
            {
                Diagnostics = diagnostics.ToArray(),
                ResultId = Guid.NewGuid().ToString()
            }
        ];
    }

    private async Task<LspDiagnostic[]> GetCSharpDiagnosticsAsync(TextDocument razorDocument, CancellationToken cancellationToken)
    {
        if (!razorDocument.TryComputeHintNameFromRazorDocument(out var hintName) ||
            await razorDocument.Project.TryGetSourceGeneratedDocumentFromHintNameAsync(hintName, cancellationToken).ConfigureAwait(false) is not { } generatedDocument)
        {
            return [];
        }

        _logger.LogDebug($"Getting C# diagnostics for {generatedDocument.FilePath}");
        var csharpDiagnostics = await ExternalHandlers.Diagnostics.GetDocumentDiagnosticsAsync(generatedDocument, supportsVisualStudioExtensions: true, cancellationToken).ConfigureAwait(false);

        // This is, to say the least, not ideal. In future we're going to normalize on to Roslyn LSP types, and this can go.
        if (JsonHelpers.ToVsLSP<LspDiagnostic[], ImmutableArray<RoslynDiagnostic>>(csharpDiagnostics) is not { } convertedDiagnostics)
        {
            return [];
        }

        return convertedDiagnostics;
    }

    private async Task<LspDiagnostic[]> GetHtmlDiagnosticsAsync(TextDocument razorDocument, CancellationToken cancellationToken)
    {
        var htmlDocument = await _htmlDocumentSynchronizer.TryGetSynchronizedHtmlDocumentAsync(razorDocument, cancellationToken).ConfigureAwait(false);
        if (htmlDocument is null)
        {
            return [];
        }

        var diagnosticsParams = new VSInternalDocumentDiagnosticsParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = htmlDocument.Uri }
        };

        _logger.LogDebug($"Getting Html diagnostics for {htmlDocument.Uri}");

        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalDocumentDiagnosticsParams, VSInternalDiagnosticReport[]?>(
            htmlDocument.Buffer,
            VSInternalMethods.DocumentPullDiagnosticName,
            RazorLSPConstants.HtmlLanguageServerName,
            diagnosticsParams,
            cancellationToken).ConfigureAwait(false);

        if (result?.Response is null)
        {
            return [];
        }

        using var allDiagnostics = new PooledArrayBuilder<LspDiagnostic>();
        foreach (var report in result.Response)
        {
            if (report.Diagnostics is not null)
            {
                allDiagnostics.AddRange(report.Diagnostics);
            }
        }

        return allDiagnostics.ToArray();
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostDocumentPullDiagnosticsEndpoint instance)
    {
        public Task<VSInternalDiagnosticReport[]?> HandleRequestAsync(TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.HandleRequestAsync(razorDocument, cancellationToken);

        public Task<VSInternalDiagnosticReport[]?> HandleTaskListItemRequestAsync(TextDocument razorDocument, ImmutableArray<string> taskListDescriptors, CancellationToken cancellationToken)
            => instance.HandleTaskListItemRequestAsync(razorDocument, taskListDescriptors, cancellationToken);
    }
}


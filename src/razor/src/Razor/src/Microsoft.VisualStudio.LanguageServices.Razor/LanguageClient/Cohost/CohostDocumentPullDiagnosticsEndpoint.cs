// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

#pragma warning disable RS0030 // Do not use banned APIs
[Shared]
[CohostEndpoint(VSInternalMethods.DocumentPullDiagnosticName)]
[Export(typeof(IDynamicRegistrationProvider))]
[ExportCohostStatelessLspService(typeof(CohostDocumentPullDiagnosticsEndpoint))]
[method: ImportingConstructor]
#pragma warning restore RS0030 // Do not use banned APIs
internal sealed class CohostDocumentPullDiagnosticsEndpoint(
    IIncompatibleProjectService incompatibleProjectService,
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlRequestInvoker requestInvoker,
    IClientSettingsManager clientSettingsManager,
    IClientCapabilitiesService clientCapabilitiesService,
    ITelemetryReporter telemetryReporter,
    ILoggerFactory loggerFactory)
    : CohostDocumentPullDiagnosticsEndpointBase<VSInternalDocumentDiagnosticsParams, VSInternalDiagnosticReport[]>(incompatibleProjectService, remoteServiceInvoker, requestInvoker, clientCapabilitiesService, telemetryReporter, loggerFactory), IDynamicRegistrationProvider
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;

    protected override string LspMethodName => VSInternalMethods.DocumentPullDiagnosticName;
    protected override bool SupportsHtmlDiagnostics => true;

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

    protected async override Task<VSInternalDiagnosticReport[]?> HandleRequestAsync(VSInternalDocumentDiagnosticsParams request, TextDocument razorDocument, CancellationToken cancellationToken)
    {
        if (request.QueryingDiagnosticKind?.Value == VSInternalDiagnosticKind.Task.Value)
        {
            return await HandleTaskListItemRequestAsync(
                razorDocument,
                _clientSettingsManager.GetClientSettings().AdvancedSettings.TaskListDescriptors,
                cancellationToken).ConfigureAwait(false);
        }

        var results = await GetDiagnosticsAsync(razorDocument, cancellationToken).ConfigureAwait(false);

        if (results is null)
        {
            return [];
        }

        return [new()
        {
            Diagnostics = results,
            ResultId = Guid.NewGuid().ToString()
        }];
    }

    protected override VSInternalDocumentDiagnosticsParams CreateHtmlParams(Uri uri)
    {
        return new VSInternalDocumentDiagnosticsParams
        {
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) }
        };
    }

    protected override LspDiagnostic[] ExtractHtmlDiagnostics(VSInternalDiagnosticReport[] result)
    {
        using var allDiagnostics = new PooledArrayBuilder<LspDiagnostic>();
        foreach (var report in result)
        {
            if (report.Diagnostics is not null)
            {
                allDiagnostics.AddRange(report.Diagnostics);
            }
        }

        return allDiagnostics.ToArray();
    }

    private async Task<VSInternalDiagnosticReport[]> HandleTaskListItemRequestAsync(TextDocument razorDocument, ImmutableArray<string> taskListDescriptors, CancellationToken cancellationToken)
    {
        var diagnostics = await _remoteServiceInvoker.TryInvokeAsync<IRemoteDiagnosticsService, ImmutableArray<LspDiagnostic>>(
            razorDocument.Project.Solution,
            (service, solutionInfo, cancellationToken) => service.GetTaskListDiagnosticsAsync(solutionInfo, razorDocument.Id, taskListDescriptors, cancellationToken),
            cancellationToken).ConfigureAwait(false);

        if (diagnostics.IsDefaultOrEmpty)
        {
            return [];
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

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CohostDocumentPullDiagnosticsEndpoint instance)
    {
        public Task<LspDiagnostic[]?> HandleRequestAsync(TextDocument razorDocument, CancellationToken cancellationToken)
            => instance.GetDiagnosticsAsync(razorDocument, cancellationToken);

        public Task<VSInternalDiagnosticReport[]> HandleTaskListItemRequestAsync(TextDocument razorDocument, ImmutableArray<string> taskListDescriptors, CancellationToken cancellationToken)
            => instance.HandleTaskListItemRequestAsync(razorDocument, taskListDescriptors, cancellationToken);
    }
}


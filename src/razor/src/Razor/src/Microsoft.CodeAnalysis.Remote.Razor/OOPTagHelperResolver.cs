// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal class OOPTagHelperResolver : TagHelperResolver
{
    private readonly TagHelperResultCache _resultCache;
    private readonly DefaultTagHelperResolver _defaultResolver;
    private readonly ProjectSnapshotProjectEngineFactory _factory;
    private readonly ErrorReporter _errorReporter;
    private readonly Workspace _workspace;
    private readonly ITelemetryReporter _telemetryReporter;

    public OOPTagHelperResolver(ProjectSnapshotProjectEngineFactory factory, ErrorReporter errorReporter, Workspace workspace, ITelemetryReporter telemetryReporter)
        : base(telemetryReporter)
    {
        if (factory is null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        if (errorReporter is null)
        {
            throw new ArgumentNullException(nameof(errorReporter));
        }

        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        if (telemetryReporter is null)
        {
            throw new ArgumentNullException(nameof(telemetryReporter));
        }

        _factory = factory;
        _errorReporter = errorReporter;
        _workspace = workspace;
        _telemetryReporter = telemetryReporter;

        _defaultResolver = new DefaultTagHelperResolver(telemetryReporter);
        _resultCache = new TagHelperResultCache();
    }

    public override async Task<TagHelperResolutionResult> GetTagHelpersAsync(Project workspaceProject, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken = default)
    {
        if (workspaceProject is null)
        {
            throw new ArgumentNullException(nameof(workspaceProject));
        }

        if (projectSnapshot is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshot));
        }

        if (projectSnapshot.Configuration is null)
        {
            return TagHelperResolutionResult.Empty;
        }

        // Not every custom factory supports the OOP host. Our priority system should work like this:
        //
        // 1. Use custom factory out of process
        // 2. Use custom factory in process
        // 3. Use fallback factory in process
        //
        // Calling into RazorTemplateEngineFactoryService.Create will accomplish #2 and #3 in one step.
        var factory = _factory.FindSerializableFactory(projectSnapshot);

        try
        {
            TagHelperResolutionResult? result = null;
            if (factory != null)
            {
                result = await ResolveTagHelpersOutOfProcessAsync(factory, workspaceProject, projectSnapshot, cancellationToken).ConfigureAwait(false);
            }

            // Was unable to get tag helpers OOP, fallback to default behavior.
            result ??= await ResolveTagHelpersInProcessAsync(workspaceProject, projectSnapshot, cancellationToken).ConfigureAwait(false);

            return result;
        }
        catch (Exception exception) when (exception is not TaskCanceledException && exception is not OperationCanceledException)
        {
            throw new InvalidOperationException($"An unexpected exception occurred when invoking '{typeof(DefaultTagHelperResolver).FullName}.{nameof(GetTagHelpersAsync)}' on the Razor language service.", exception);
        }
    }

    protected virtual async Task<TagHelperResolutionResult?> ResolveTagHelpersOutOfProcessAsync(IProjectEngineFactory factory, Project workspaceProject, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        // We're being overly defensive here because the OOP host can return null for the client/session/operation
        // when it's disconnected (user stops the process).
        //
        // This will change in the future to an easier to consume API but for VS RTM this is what we have.
        var remoteClient = await RazorRemoteHostClient.TryGetClientAsync(_workspace.Services, RazorServiceDescriptors.TagHelperProviderServiceDescriptors, RazorRemoteServiceCallbackDispatcherRegistry.Empty, cancellationToken);

        if (remoteClient is null)
        {
            // Could not resolve
            return null;
        }

        if (!_resultCache.TryGetId(projectSnapshot.FilePath, out var lastResultId))
        {
            lastResultId = -1;
        }

        var projectHandle = new ProjectSnapshotHandle(projectSnapshot.FilePath, projectSnapshot.Configuration, projectSnapshot.RootNamespace);
        var result = await remoteClient.TryInvokeAsync<IRemoteTagHelperProviderService, TagHelperDeltaResult>(
            workspaceProject.Solution,
            (service, solutionInfo, innerCancellationToken) => service.GetTagHelpersDeltaAsync(solutionInfo, projectHandle, factory?.GetType().AssemblyQualifiedName, lastResultId, innerCancellationToken),
            cancellationToken
        );

        if (!result.HasValue)
        {
            return null;
        }

        var tagHelpers = ProduceTagHelpersFromDelta(projectSnapshot.FilePath, lastResultId, result.Value);

        var resolutionResult = new TagHelperResolutionResult(tagHelpers, diagnostics: Array.Empty<RazorDiagnostic>());
        return resolutionResult;
    }

    // Protected virtual for testing
    protected virtual IReadOnlyCollection<TagHelperDescriptor>? ProduceTagHelpersFromDelta(string projectFilePath, int lastResultId, TagHelperDeltaResult deltaResult)
    {
        var fromCache = true;
        var stopWatch = Stopwatch.StartNew();

        if (!_resultCache.TryGet(projectFilePath, lastResultId, out var tagHelpers))
        {
            // We most likely haven't made a request to the server yet so there's no delta to apply
            tagHelpers = Array.Empty<TagHelperDescriptor>();
            fromCache = false;

            if (deltaResult.Delta)
            {
                // We somehow failed to retrieve a cached object yet the server was able to apply a delta. This
                // is entirely unexpected and means the server & client are catastrophically de-synchronized.
                throw new InvalidOperationException("This should never happen. Razor server & client are de-synchronized. Tearing down");
            }
        }
        else if (!deltaResult.Delta)
        {
            // Not a delta based response, we should treat it as a "refresh"
            tagHelpers = Array.Empty<TagHelperDescriptor>();
            fromCache = false;
        }

        if (deltaResult.ResultId != lastResultId)
        {
            // New results, lets build a coherent TagHelper collection and then cache it
            tagHelpers = deltaResult.Apply(tagHelpers);
            _resultCache.Set(projectFilePath, deltaResult.ResultId, tagHelpers);
            fromCache = false;
        }

        stopWatch.Stop();
        if (fromCache)
        {
            _telemetryReporter.ReportEvent("taghelpers.fromcache", VisualStudio.Telemetry.TelemetrySeverity.Normal, new Dictionary<string, long>()
            {
                { "taghelper.cachedresult.ellapsedms", stopWatch.ElapsedMilliseconds }
            }.ToImmutableDictionary());
        }

        return tagHelpers;
    }

    protected virtual Task<TagHelperResolutionResult> ResolveTagHelpersInProcessAsync(Project project, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        return _defaultResolver.GetTagHelpersAsync(project, projectSnapshot, cancellationToken);
    }
}

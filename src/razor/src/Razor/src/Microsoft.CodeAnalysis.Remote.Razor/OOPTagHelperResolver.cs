// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal class OOPTagHelperResolver : ITagHelperResolver
{
    private readonly TagHelperResultCache _resultCache;
    private readonly CompilationTagHelperResolver _innerResolver;
    private readonly ProjectSnapshotProjectEngineFactory _factory;
    private readonly IErrorReporter _errorReporter;
    private readonly Workspace _workspace;
    private readonly ITelemetryReporter _telemetryReporter;

    public OOPTagHelperResolver(ProjectSnapshotProjectEngineFactory factory, IErrorReporter errorReporter, Workspace workspace, ITelemetryReporter telemetryReporter)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _errorReporter = errorReporter ?? throw new ArgumentNullException(nameof(errorReporter));
        _workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
        _telemetryReporter = telemetryReporter ?? throw new ArgumentNullException(nameof(telemetryReporter));

        _innerResolver = new CompilationTagHelperResolver(telemetryReporter);
        _resultCache = new TagHelperResultCache();
    }

    public async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        Project workspaceProject,
        IProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken)
    {
        if (projectSnapshot.Configuration is null)
        {
            return ImmutableArray<TagHelperDescriptor>.Empty;
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
            ImmutableArray<TagHelperDescriptor> result = default;

            if (factory != null)
            {
                result = await ResolveTagHelpersOutOfProcessAsync(factory, workspaceProject, projectSnapshot, cancellationToken).ConfigureAwait(false);
            }

            // Was unable to get tag helpers OOP, fallback to default behavior.
            if (result.IsDefault)
            {
                result = await ResolveTagHelpersInProcessAsync(workspaceProject, projectSnapshot, cancellationToken).ConfigureAwait(false);
            }

            return result;
        }
        catch (Exception ex) when (ex is not TaskCanceledException && ex is not OperationCanceledException)
        {
            throw new InvalidOperationException(
                $"An unexpected exception occurred when invoking '{typeof(CompilationTagHelperResolver).FullName}.{nameof(GetTagHelpersAsync)}' on the Razor language service.",
                ex);
        }
    }

    protected virtual async ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersOutOfProcessAsync(IProjectEngineFactory factory, Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        // We're being overly defensive here because the OOP host can return null for the client/session/operation
        // when it's disconnected (user stops the process).
        //
        // This will change in the future to an easier to consume API but for VS RTM this is what we have.
        var remoteClient = await RazorRemoteHostClient.TryGetClientAsync(
            _workspace.Services,
            RazorServiceDescriptors.TagHelperProviderServiceDescriptors,
            RazorRemoteServiceCallbackDispatcherRegistry.Empty,
            cancellationToken);

        if (remoteClient is null)
        {
            // Could not resolve
            return default;
        }

        if (!_resultCache.TryGetId(workspaceProject.Id, out var lastResultId))
        {
            lastResultId = -1;
        }

        var projectHandle = new ProjectSnapshotHandle(workspaceProject.Id, projectSnapshot.Configuration, projectSnapshot.RootNamespace);
        var factoryTypeName = factory.GetType().AssemblyQualifiedName;

        var result = await remoteClient.TryInvokeAsync<IRemoteTagHelperProviderService, TagHelperDeltaResult>(
            workspaceProject.Solution,
            (service, solutionInfo, innerCancellationToken) =>
                service.GetTagHelpersDeltaAsync(solutionInfo, projectHandle, factoryTypeName, lastResultId, innerCancellationToken),
            cancellationToken);

        if (!result.HasValue)
        {
            return default;
        }

        var tagHelpers = ProduceTagHelpersFromDelta(workspaceProject.Id, lastResultId, result.Value);

        return tagHelpers;
    }

    // Protected virtual for testing
    protected virtual ImmutableArray<TagHelperDescriptor> ProduceTagHelpersFromDelta(ProjectId projectId, int lastResultId, TagHelperDeltaResult deltaResult)
    {
        var fromCache = true;
        var stopWatch = Stopwatch.StartNew();

        if (!_resultCache.TryGet(projectId, lastResultId, out var tagHelpers))
        {
            // We most likely haven't made a request to the server yet so there's no delta to apply
            tagHelpers = ImmutableArray<TagHelperDescriptor>.Empty;
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
            tagHelpers = ImmutableArray<TagHelperDescriptor>.Empty;
            fromCache = false;
        }

        if (deltaResult.ResultId != lastResultId)
        {
            // New results, lets build a coherent TagHelper collection and then cache it
            tagHelpers = deltaResult.Apply(tagHelpers);
            _resultCache.Set(projectId, deltaResult.ResultId, tagHelpers);
            fromCache = false;
        }

        stopWatch.Stop();
        if (fromCache)
        {
            _telemetryReporter.ReportEvent("taghelpers.fromcache", Severity.Normal, ImmutableDictionary<string, object?>.Empty.Add("taghelper.cachedresult.ellapsedms", stopWatch.ElapsedMilliseconds));
        }

        return tagHelpers;
    }

    protected virtual ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersInProcessAsync(
        Project workspaceProject,
        IProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken)
        => _innerResolver.GetTagHelpersAsync(workspaceProject, projectSnapshot.GetProjectEngine(), cancellationToken);
}

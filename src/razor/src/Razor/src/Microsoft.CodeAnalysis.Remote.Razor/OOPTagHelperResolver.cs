// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
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

    public OOPTagHelperResolver(ProjectSnapshotProjectEngineFactory factory, IErrorReporter errorReporter, Workspace workspace, ITelemetryReporter telemetryReporter)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _errorReporter = errorReporter ?? throw new ArgumentNullException(nameof(errorReporter));
        _workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));

        _innerResolver = new CompilationTagHelperResolver(telemetryReporter);
        _resultCache = new TagHelperResultCache();
    }

    public async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        Project workspaceProject,
        IProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken)
    {
        // Not every custom factory supports the OOP host. Our priority system should work like this:
        //
        // 1. Use custom factory out of process
        // 2. Use custom factory in process
        // 3. Use fallback factory in process
        //
        // Calling into RazorTemplateEngineFactoryService.Create will accomplish #2 and #3 in one step.
        var factory = _factory.FindSerializableFactory(projectSnapshot);

        ImmutableArray<TagHelperDescriptor> result = default;

        if (factory != null)
        {
            try
            {
                result = await ResolveTagHelpersOutOfProcessAsync(factory, workspaceProject, projectSnapshot, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                throw new InvalidOperationException(
                    "An unexpected exception occurred when resolving tag helpers out-of-process.",
                    ex);
            }
        }

        // Was unable to get tag helpers OOP, fallback to default behavior.
        if (result.IsDefault)
        {
            try
            {
                result = await ResolveTagHelpersInProcessAsync(workspaceProject, projectSnapshot, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                throw new InvalidOperationException(
                    $"An unexpected exception occurred when invoking '{typeof(CompilationTagHelperResolver).FullName}.{nameof(GetTagHelpersAsync)}' on the Razor language service.",
                    ex);
            }
        }

        return result;
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

        var deltaResult = await remoteClient.TryInvokeAsync<IRemoteTagHelperProviderService, TagHelperDeltaResult>(
            workspaceProject.Solution,
            (service, solutionInfo, innerCancellationToken) =>
                service.GetTagHelpersDeltaAsync(solutionInfo, projectHandle, factoryTypeName, lastResultId, innerCancellationToken),
            cancellationToken);

        if (!deltaResult.HasValue)
        {
            return default;
        }

        // Apply the delta we received to any cached checksums for the current project.
        var checksums = ProduceChecksumsFromDelta(workspaceProject.Id, lastResultId, deltaResult.Value);

        using var _1 = ArrayBuilderPool<TagHelperDescriptor>.GetPooledObject(out var tagHelpers);
        using var _2 = ArrayBuilderPool<Checksum>.GetPooledObject(out var checksumsToFetch);

        tagHelpers.SetCapacityIfLarger(checksums.Length);

        foreach (var checksum in checksums)
        {
            // See if we have a cached version of this tag helper. If not, we'll need to fetch it from OOP.
            if (TagHelperCache.Default.TryGet(checksum, out var tagHelper))
            {
                tagHelpers.Add(tagHelper);
            }
            else
            {
                checksumsToFetch.Add(checksum);
            }
        }

        if (checksumsToFetch.Count > 0)
        {
            // There are checksums that we don't have cached tag helpers for, so we need to fetch them from OOP.
            var fetchResult = await remoteClient.TryInvokeAsync<IRemoteTagHelperProviderService, FetchTagHelpersResult>(
                workspaceProject.Solution,
                (service, solutionInfo, innerCancellationToken) =>
                    service.FetchTagHelpersAsync(solutionInfo, projectHandle, factoryTypeName, checksumsToFetch.DrainToImmutable(), innerCancellationToken),
                cancellationToken);

            if (!fetchResult.HasValue)
            {
                return default;
            }

            var fetchedTagHelpers = fetchResult.Value.TagHelpers;
            if (fetchedTagHelpers.IsEmpty)
            {
                // If we didn't receive any tag helpers, something catastrophic happened in the Roslyn OOP
                // when calling FetchTagHelpersAsync(...).
                throw new InvalidOperationException("Tag helpers could not be fetched from the Roslyn OOP.");
            }

            // Be sure to add the tag helpers we just fetched to the cache.
            var cache = TagHelperCache.Default;
            foreach (var tagHelper in fetchedTagHelpers)
            {
                tagHelpers.Add(tagHelper);
                cache.TryAdd(tagHelper.Checksum, tagHelper);
            }
        }

        return tagHelpers.DrainToImmutable();
    }

    // Protected virtual for testing
    protected virtual ImmutableArray<Checksum> ProduceChecksumsFromDelta(ProjectId projectId, int lastResultId, TagHelperDeltaResult deltaResult)
    {
        if (!_resultCache.TryGet(projectId, lastResultId, out var checksums))
        {
            // We most likely haven't made a request to the server yet so there's no delta to apply
            checksums = ImmutableArray<Checksum>.Empty;

            if (deltaResult.IsDelta)
            {
                // We somehow failed to retrieve a cached object yet the server was able to apply a delta. This
                // is entirely unexpected and means the server & client are catastrophically de-synchronized.
                throw new InvalidOperationException("This should never happen. Razor server & client are de-synchronized. Tearing down");
            }
        }
        else if (!deltaResult.IsDelta)
        {
            // Not a delta based response, we should treat it as a "refresh"
            checksums = ImmutableArray<Checksum>.Empty;
        }

        if (deltaResult.ResultId != lastResultId)
        {
            // New results, lets build a coherent TagHelper collection and then cache it
            checksums = deltaResult.Apply(checksums);
            _resultCache.Set(projectId, deltaResult.ResultId, checksums);
        }

        return checksums;
    }

    protected virtual ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersInProcessAsync(
        Project workspaceProject,
        IProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken)
        => _innerResolver.GetTagHelpersAsync(workspaceProject, projectSnapshot.GetProjectEngine(), cancellationToken);
}

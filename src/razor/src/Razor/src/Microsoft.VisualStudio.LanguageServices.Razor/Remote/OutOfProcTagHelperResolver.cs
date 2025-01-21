// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Razor.Remote;

/// <summary>
///  Retrieves <see cref="TagHelperDescriptor">tag helpers</see> for a given <see cref="Project"/>
///  using an out-of-process service. If the service isn't available, this falls back to
///  retrieving tag helpers in-process.
/// </summary>
[Export(typeof(ITagHelperResolver))]
[method: ImportingConstructor]
internal class OutOfProcTagHelperResolver(
    IRemoteServiceInvoker remoteServiceInvoker,
    ILoggerFactory loggerFactory,
    ITelemetryReporter telemetryReporter) : ITagHelperResolver
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<OutOfProcTagHelperResolver>();
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly TagHelperResultCache _resultCache = new();

    public async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        Project project,
        ProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken)
    {
        // First, try to retrieve tag helpers out-of-proc. If that fails, try in-proc.

        try
        {
            var result = await ResolveTagHelpersOutOfProcessAsync(project, projectSnapshot, cancellationToken).ConfigureAwait(false);

            // We received tag helpers, so we're done.
            if (!result.IsDefault)
            {
                return result;
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, $"Error encountered from project '{projectSnapshot.FilePath}':{Environment.NewLine}{ex}");
            return default;
        }

        try
        {
            return await ResolveTagHelpersInProcessAsync(project, projectSnapshot, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, $"Error encountered from project '{projectSnapshot.FilePath}':{Environment.NewLine}{ex}");
            return default;
        }
    }

    protected virtual async ValueTask<ImmutableArray<TagHelperDescriptor>> ResolveTagHelpersOutOfProcessAsync(Project project, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        if (!_resultCache.TryGetId(project.Id, out var lastResultId))
        {
            lastResultId = -1;
        }

        var projectHandle = new ProjectSnapshotHandle(project.Id, projectSnapshot.Configuration, projectSnapshot.RootNamespace);

        var deltaResult = await _remoteServiceInvoker.TryInvokeAsync<IRemoteTagHelperProviderService, TagHelperDeltaResult>(
            project.Solution,
            (service, solutionInfo, innerCancellationToken) =>
                service.GetTagHelpersDeltaAsync(solutionInfo, projectHandle, lastResultId, innerCancellationToken),
            cancellationToken);

        if (deltaResult is null)
        {
            return default;
        }

        // Apply the delta we received to any cached checksums for the current project.
        var checksums = ProduceChecksumsFromDelta(project.Id, lastResultId, deltaResult);

        using var tagHelpers = new PooledArrayBuilder<TagHelperDescriptor>(capacity: checksums.Length);
        using var checksumsToFetch = new PooledArrayBuilder<Checksum>(capacity: checksums.Length);

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
            var fetchResult = await _remoteServiceInvoker.TryInvokeAsync<IRemoteTagHelperProviderService, FetchTagHelpersResult>(
                project.Solution,
                (service, solutionInfo, innerCancellationToken) =>
                    service.FetchTagHelpersAsync(solutionInfo, projectHandle, checksumsToFetch.DrainToImmutable(), innerCancellationToken),
                cancellationToken);

            if (fetchResult is null)
            {
                return default;
            }

            var fetchedTagHelpers = fetchResult.TagHelpers;
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
    protected ImmutableArray<Checksum> ProduceChecksumsFromDelta(ProjectId projectId, int lastResultId, TagHelperDeltaResult deltaResult)
    {
        if (!_resultCache.TryGet(projectId, lastResultId, out var checksums))
        {
            // We most likely haven't made a request to the server yet so there's no delta to apply
            checksums = [];

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
            checksums = [];
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
        Project project,
        ProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken)
        => project.GetTagHelpersAsync(projectSnapshot.ProjectEngine, _telemetryReporter, cancellationToken);
}

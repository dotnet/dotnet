// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.Razor.Discovery;

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
#pragma warning disable IDE0052 // Private member 'OutOfProcTagHelperResolver._telemetryReporter' can be removed as the value assigned to it is never read
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
#pragma warning restore IDE0052 // Private member 'OutOfProcTagHelperResolver._telemetryReporter' can be removed as the value assigned to it is never read
    private readonly TagHelperResultCache _resultCache = new();

    public async ValueTask<TagHelperCollection?> GetTagHelpersAsync(
        Project project,
        ProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken)
    {
        // First, try to retrieve tag helpers out-of-proc. If that fails, try in-proc.

        try
        {
            var result = await ResolveTagHelpersOutOfProcessAsync(project, projectSnapshot, cancellationToken).ConfigureAwait(false);

            // We received tag helpers, so we're done.
            if (result is not null)
            {
                return result;
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, $"Error encountered from project '{projectSnapshot.FilePath}':{Environment.NewLine}{ex}");
            // if not cancellation, try getting tag helpers in-proc
        }

        try
        {
            return await ResolveTagHelpersInProcessAsync(project, projectSnapshot, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, $"Error encountered from project '{projectSnapshot.FilePath}':{Environment.NewLine}{ex}");
            return null;
        }
    }

    protected virtual async ValueTask<TagHelperCollection?> ResolveTagHelpersOutOfProcessAsync(Project project, ProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
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
            // For some reason, TryInvokeAsync can return null if it is cancelled while fetching the client.
            return null;
        }

        // Apply the delta we received to any cached checksums for the current project.
        var checksums = ProduceChecksumsFromDelta(project.Id, lastResultId, deltaResult);

        // Create an array to hold the result. We'll wrap it in an ImmutableArray at the end.
        var result = new TagHelperDescriptor[checksums.Length];

        // We need to keep track of which checksums we still need to fetch tag helpers for from OOP.
        // In addition, we'll track the indices in tagHelpers that we'll need to replace with those we
        // fetch to ensure that the results stay in the same order.
        using var checksumsToFetchBuilder = new PooledArrayBuilder<Checksum>(capacity: checksums.Length);
        using var checksumIndicesBuilder = new PooledArrayBuilder<int>(capacity: checksums.Length);

        for (var i = 0; i < checksums.Length; i++)
        {
            var checksum = checksums[i];

            // See if we have a cached version of this tag helper. If not, we'll need to fetch it from OOP.
            if (TagHelperCache.Default.TryGet(checksum, out var tagHelper))
            {
                result[i] = tagHelper;
            }
            else
            {
                checksumsToFetchBuilder.Add(checksum);
                checksumIndicesBuilder.Add(i);
            }
        }

        if (checksumsToFetchBuilder.Count > 0)
        {
            var checksumsToFetch = checksumsToFetchBuilder.ToImmutableAndClear();

            // There are checksums that we don't have cached tag helpers for, so we need to fetch them from OOP.
            var fetchResult = await _remoteServiceInvoker.TryInvokeAsync<IRemoteTagHelperProviderService, FetchTagHelpersResult>(
                project.Solution,
                (service, solutionInfo, innerCancellationToken) =>
                    service.FetchTagHelpersAsync(solutionInfo, projectHandle, checksumsToFetch, innerCancellationToken),
                cancellationToken);

            if (fetchResult is null)
            {
                // For some reason, TryInvokeAsync can return null if it is cancelled while fetching the client.
                return null;
            }

            var fetchedTagHelpers = fetchResult.TagHelpers;
            if (fetchedTagHelpers.IsEmpty)
            {
                // If we didn't receive any tag helpers, something catastrophic happened in the Roslyn OOP
                // when calling FetchTagHelpersAsync(...).
                throw new InvalidOperationException("Tag helpers could not be fetched from the Roslyn OOP.");
            }

            Debug.Assert(
                checksumsToFetch.Length == fetchedTagHelpers.Count,
                $"{nameof(FetchTagHelpersResult)} should return the same number of tag helpers as checksums requested.");

            Debug.Assert(
                checksumsToFetch.SequenceEqual(fetchedTagHelpers.Select(static t => t.Checksum)),
                $"{nameof(FetchTagHelpersResult)} should return tag helpers that match the checksums requested.");

            // Be sure to add the tag helpers we just fetched to the cache.
            var cache = TagHelperCache.Default;

            for (var i = 0; i < fetchedTagHelpers.Count; i++)
            {
                var index = checksumIndicesBuilder[i];
                Debug.Assert(result[index] is null);

                var fetchedTagHelper = fetchedTagHelpers[i];
                result[index] = fetchedTagHelper;
                cache.TryAdd(fetchedTagHelper.Checksum, fetchedTagHelper);
            }

            if (checksumsToFetch.Length != fetchedTagHelpers.Count)
            {
                _logger.LogWarning($"Expected to receive {checksumsToFetch.Length} tag helpers from Roslyn OOP, " +
                    $"but received {fetchedTagHelpers.Count} instead. Returning a partial set of tag helpers.");

                // We didn't receive all the tag helpers we requested. This is bad. However, instead of failing,
                // we'll just return the tag helpers we were able to retrieve.
                using var resultBuilder = new TagHelperCollection.RefBuilder(initialCapacity: result.Length);

                foreach (var tagHelper in result)
                {
                    if (tagHelper is not null)
                    {
                        resultBuilder.Add(tagHelper);
                    }
                }

                return resultBuilder.ToCollection();
            }
        }

        // If we pass 'result' to TagHelperCollection.Create(...) the overload that takes
        // a ReadOnlySpan<TagHelperDescriptor> will be called, resulting in 'result' being
        // copied to a new array. By first wrapping 'result' in an ImmutableArray we ensure
        // that TagHelperCollection.Create(...) slices 'result' into segments.
        var resultArray = ImmutableCollectionsMarshal.AsImmutableArray(result);

        return TagHelperCollection.Create(resultArray);
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

    protected virtual ValueTask<TagHelperCollection> ResolveTagHelpersInProcessAsync(
        Project project,
        ProjectSnapshot projectSnapshot,
        CancellationToken cancellationToken)
        => project.GetTagHelpersAsync(projectSnapshot.ProjectEngine, cancellationToken);
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteTagHelperProviderService : RazorServiceBase, IRemoteTagHelperProviderService
{
    private readonly RemoteTagHelperResolver _tagHelperResolver;
    private readonly RemoteTagHelperDeltaProvider _tagHelperDeltaProvider;

    internal RemoteTagHelperProviderService(IServiceBroker serviceBroker, ExportProvider exportProvider)
        : base(serviceBroker)
    {
        _tagHelperResolver = exportProvider.GetExportedValue<RemoteTagHelperResolver>().AssumeNotNull();
        _tagHelperDeltaProvider = exportProvider.GetExportedValue<RemoteTagHelperDeltaProvider>().AssumeNotNull();
    }

    public ValueTask<FetchTagHelpersResult> FetchTagHelpersAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        ProjectSnapshotHandle projectHandle,
        ImmutableArray<Checksum> checksums,
        CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(
            solutionInfo,
            ServiceBrokerClient,
            solution => FetchTagHelpersCoreAsync(solution, projectHandle, checksums, cancellationToken),
            cancellationToken);

    private async ValueTask<FetchTagHelpersResult> FetchTagHelpersCoreAsync(
        Solution solution,
        ProjectSnapshotHandle projectHandle,
        ImmutableArray<Checksum> checksums,
        CancellationToken cancellationToken)
    {
        if (!TryGetCachedTagHelpers(checksums, out var tagHelpers))
        {
            // If one or more of the tag helpers aren't in the cache, we'll need to re-compute them from the project.
            // In practice, this shouldn't happen because FetchTagHelpersAsync(...) is normally called immediately after
            // calling GetTagHelpersDeltaAsync(...), which caches the tag helpers it computes.

            if (solution.GetProject(projectHandle.ProjectId) is not Project workspaceProject)
            {
                // This is bad. In this case, we're being asked to retrieve tag helpers for a project that no longer exists.
                // The best we can do is just return an empty array.
                return FetchTagHelpersResult.Empty;
            }

            // Compute the latest tag helpers and add them all to the cache.
            var latestTagHelpers = await _tagHelperResolver
                .GetTagHelpersAsync(workspaceProject, projectHandle.Configuration, cancellationToken)
                .ConfigureAwait(false);

            var cache = TagHelperCache.Default;

            foreach (var tagHelper in latestTagHelpers)
            {
                cache.TryAdd(tagHelper.Checksum, tagHelper);
            }

            // Finally, try to retrieve our cached tag helpers
            if (!TryGetCachedTagHelpers(checksums, out tagHelpers))
            {
                // This is extra bad and should not happen. Again, we'll return an empty array and let the client deal with it.
                return FetchTagHelpersResult.Empty;
            }
        }

        return new FetchTagHelpersResult(tagHelpers);

        static bool TryGetCachedTagHelpers(ImmutableArray<Checksum> checksums, out ImmutableArray<TagHelperDescriptor> tagHelpers)
        {
            using var builder = new PooledArrayBuilder<TagHelperDescriptor>(capacity: checksums.Length);
            var cache = TagHelperCache.Default;

            foreach (var checksum in checksums)
            {
                if (!cache.TryGet(checksum, out var tagHelper))
                {
                    tagHelpers = ImmutableArray<TagHelperDescriptor>.Empty;
                    return false;
                }

                builder.Add(tagHelper);
            }

            tagHelpers = builder.DrainToImmutable();
            return true;
        }
    }

    public ValueTask<TagHelperDeltaResult> GetTagHelpersDeltaAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        ProjectSnapshotHandle projectHandle,
        int lastResultId,
        CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(
            solutionInfo,
            ServiceBrokerClient,
            solution => GetTagHelpersDeltaCoreAsync(solution, projectHandle, lastResultId, cancellationToken),
            cancellationToken);

    private async ValueTask<TagHelperDeltaResult> GetTagHelpersDeltaCoreAsync(
        Solution solution,
        ProjectSnapshotHandle projectHandle,
        int lastResultId,
        CancellationToken cancellationToken)
    {
        ImmutableArray<Checksum> checksums;

        if (solution.GetProject(projectHandle.ProjectId) is not Project workspaceProject)
        {
            checksums = ImmutableArray<Checksum>.Empty;
        }
        else
        {
            var tagHelpers = await _tagHelperResolver
                .GetTagHelpersAsync(workspaceProject, projectHandle.Configuration, cancellationToken)
                .ConfigureAwait(false);

            checksums = GetChecksums(tagHelpers);
        }

        return _tagHelperDeltaProvider.GetTagHelpersDelta(projectHandle.ProjectId, lastResultId, checksums);

        static ImmutableArray<Checksum> GetChecksums(ImmutableArray<TagHelperDescriptor> tagHelpers)
        {
            using var builder = new PooledArrayBuilder<Checksum>(capacity: tagHelpers.Length);

            // Add each tag helpers to the cache so that we can retrieve them later if needed.
            var cache = TagHelperCache.Default;

            foreach (var tagHelper in tagHelpers)
            {
                var checksum = tagHelper.Checksum;
                builder.Add(checksum);
                cache.TryAdd(checksum, tagHelper);
            }

            return builder.DrainToImmutable();
        }
    }
}

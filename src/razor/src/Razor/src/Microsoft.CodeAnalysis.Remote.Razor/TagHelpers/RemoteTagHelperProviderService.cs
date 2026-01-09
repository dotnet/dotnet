// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed partial class RemoteTagHelperProviderService(in ServiceArgs args) : RazorBrokeredServiceBase(in args), IRemoteTagHelperProviderService
{
    internal sealed class Factory : FactoryBase<IRemoteTagHelperProviderService>
    {
        protected override IRemoteTagHelperProviderService CreateService(in ServiceArgs args)
            => new RemoteTagHelperProviderService(in args);
    }

    private readonly RemoteTagHelperResolver _tagHelperResolver = args.ExportProvider.GetExportedValue<RemoteTagHelperResolver>();
    private readonly RemoteTagHelperDeltaProvider _tagHelperDeltaProvider = args.ExportProvider.GetExportedValue<RemoteTagHelperDeltaProvider>();

    public ValueTask<FetchTagHelpersResult> FetchTagHelpersAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        ProjectSnapshotHandle projectHandle,
        ImmutableArray<Checksum> checksums,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
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

        static bool TryGetCachedTagHelpers(ImmutableArray<Checksum> checksums, out TagHelperCollection tagHelpers)
        {
            using var builder = new TagHelperCollection.RefBuilder(initialCapacity: checksums.Length);
            var cache = TagHelperCache.Default;

            foreach (var checksum in checksums)
            {
                if (!cache.TryGet(checksum, out var tagHelper))
                {
                    tagHelpers = [];
                    return false;
                }

                builder.Add(tagHelper);
            }

            tagHelpers = builder.ToCollection();
            return true;
        }
    }

    public ValueTask<TagHelperDeltaResult> GetTagHelpersDeltaAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        ProjectSnapshotHandle projectHandle,
        int lastResultId,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
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
            checksums = [];
        }
        else
        {
            var tagHelpers = await _tagHelperResolver
                .GetTagHelpersAsync(workspaceProject, projectHandle.Configuration, cancellationToken)
                .ConfigureAwait(false);

            checksums = GetChecksums(tagHelpers);
        }

        return _tagHelperDeltaProvider.GetTagHelpersDelta(projectHandle.ProjectId, lastResultId, checksums);

        static ImmutableArray<Checksum> GetChecksums(TagHelperCollection tagHelpers)
        {
            var array = new Checksum[tagHelpers.Count];

            // Add each tag helpers to the cache so that we can retrieve them later if needed.
            var cache = TagHelperCache.Default;
            var index = 0;

            foreach (var tagHelper in tagHelpers)
            {
                var checksum = tagHelper.Checksum;
                array[index++] = checksum;
                cache.TryAdd(checksum, tagHelper);
            }

            return ImmutableCollectionsMarshal.AsImmutableArray(array);
        }
    }
}

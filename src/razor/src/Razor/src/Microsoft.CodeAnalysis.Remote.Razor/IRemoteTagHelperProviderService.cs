// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal interface IRemoteTagHelperProviderService
{
    ValueTask<TagHelperDeltaResult> GetTagHelpersDeltaAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        ProjectSnapshotHandle projectHandle,
        string factoryTypeName,
        int lastResultId,
        CancellationToken cancellationToken);

    ValueTask<FetchTagHelpersResult> FetchTagHelpersAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        ProjectSnapshotHandle projectHandle,
        string factoryTypeName,
        ImmutableArray<Checksum> checksums,
        CancellationToken cancellationToken);
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol.Folding;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteFoldingRangeService : IDisposable
{
    ValueTask<ImmutableArray<RemoteFoldingRange>> GetFoldingRangesAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId documentId, ImmutableArray<RemoteFoldingRange> htmlRanges, CancellationToken cancellationToken);
}

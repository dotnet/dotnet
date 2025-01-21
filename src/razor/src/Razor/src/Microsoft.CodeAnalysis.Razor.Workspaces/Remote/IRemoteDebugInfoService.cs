// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteDebugInfoService
{
    ValueTask<LinePositionSpan?> ResolveBreakpointRangeAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        LinePosition position,
        CancellationToken cancellationToken);

    ValueTask<string[]?> ResolveProximityExpressionsAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        LinePosition position,
        CancellationToken cancellationToken);

    ValueTask<LinePositionSpan?> ValidateBreakableRangeAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId documentId,
        LinePositionSpan span,
        CancellationToken cancellationToken);
}

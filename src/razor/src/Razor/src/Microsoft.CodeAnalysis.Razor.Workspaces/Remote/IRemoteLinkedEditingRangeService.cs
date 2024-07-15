// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteLinkedEditingRangeService : IDisposable
{
    ValueTask<LinePositionSpan[]?> GetRangesAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, LinePosition linePosition, CancellationToken cancellationToken);
}

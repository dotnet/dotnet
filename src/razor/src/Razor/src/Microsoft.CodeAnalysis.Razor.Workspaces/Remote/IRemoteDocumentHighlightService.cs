// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentHighlight;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteDocumentHighlightService
{
    ValueTask<RemoteResponse<RemoteDocumentHighlight[]?>> GetHighlightsAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId documentId, LinePosition position, CancellationToken cancellationToken);
}

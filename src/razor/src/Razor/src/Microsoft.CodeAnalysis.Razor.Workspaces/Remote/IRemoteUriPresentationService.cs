// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteUriPresentationService
{
    ValueTask<Response> GetPresentationAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, LinePositionSpan span, Uri[]? uris, CancellationToken cancellationToken);

    [DataContract]
    public record struct Response(
        [property: DataMember(Order = 0)] bool CallHtml,
        [property: DataMember(Order = 1)] TextChange? TextChange);
}

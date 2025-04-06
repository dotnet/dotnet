// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteGoToImplementationService : IRemoteJsonService
{
    ValueTask<RemoteResponse<LspLocation[]?>> GetImplementationAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId razorDocumentId,
        Position position,
        CancellationToken cancellationToken);
}

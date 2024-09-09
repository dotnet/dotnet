// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using RoslynLocation = Roslyn.LanguageServer.Protocol.Location;
using RoslynPosition = Roslyn.LanguageServer.Protocol.Position;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteGoToImplementationService : IRemoteJsonService
{
    ValueTask<RemoteResponse<RoslynLocation[]?>> GetImplementationAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId razorDocumentId,
        RoslynPosition position,
        CancellationToken cancellationToken);
}

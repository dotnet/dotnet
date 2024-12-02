// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Roslyn.LanguageServer.Protocol;
using LspLocation = Roslyn.LanguageServer.Protocol.Location;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteFindAllReferencesService : IRemoteJsonService
{
    ValueTask<RemoteResponse<SumType<VSInternalReferenceItem, LspLocation>[]?>> FindAllReferencesAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId razorDocumentId,
        Position position,
        CancellationToken cancellationToken);
}

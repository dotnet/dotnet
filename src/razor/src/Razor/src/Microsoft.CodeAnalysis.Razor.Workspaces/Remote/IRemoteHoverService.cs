// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using RoslynHover = Roslyn.LanguageServer.Protocol.Hover;
using RoslynPosition = Roslyn.LanguageServer.Protocol.Position;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal interface IRemoteHoverService : IRemoteJsonService
{
    ValueTask<RemoteResponse<RoslynHover?>> GetHoverAsync(
        JsonSerializableRazorPinnedSolutionInfoWrapper solutionInfo,
        JsonSerializableDocumentId documentId,
        RoslynPosition position,
        CancellationToken cancellationToken);
}

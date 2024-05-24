// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteSemanticTokensService(
    IServiceBroker serviceBroker,
    IRazorSemanticTokensInfoService razorSemanticTokensInfoService,
    DocumentSnapshotFactory documentSnapshotFactory)
    : RazorDocumentServiceBase(serviceBroker, documentSnapshotFactory), IRemoteSemanticTokensService
{
    private readonly IRazorSemanticTokensInfoService _razorSemanticTokensInfoService = razorSemanticTokensInfoService;

    public ValueTask<int[]?> GetSemanticTokensDataAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, LinePositionSpan span, bool colorBackground, Guid correlationId, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetSemanticTokensDataAsync(context, span, colorBackground, correlationId, cancellationToken),
            cancellationToken);

    private async ValueTask<int[]?> GetSemanticTokensDataAsync(RemoteDocumentContext context, LinePositionSpan span, bool colorBackground, Guid correlationId, CancellationToken cancellationToken)
    {
        return await _razorSemanticTokensInfoService.GetSemanticTokensAsync(context, span, colorBackground, correlationId, cancellationToken).ConfigureAwait(false);
    }
}

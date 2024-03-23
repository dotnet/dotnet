// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
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
    : RazorServiceBase(serviceBroker), IRemoteSemanticTokensService
{
    private readonly IRazorSemanticTokensInfoService _razorSemanticTokensInfoService = razorSemanticTokensInfoService;
    private readonly DocumentSnapshotFactory _documentSnapshotFactory = documentSnapshotFactory;

    public ValueTask<int[]?> GetSemanticTokensDataAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, LinePositionSpan span, bool colorBackground, Guid correlationId, CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(
            solutionInfo,
            ServiceBrokerClient,
            solution => GetSemanticTokensDataAsync(solution, razorDocumentId, span, colorBackground, correlationId, cancellationToken),
            cancellationToken);

    private async ValueTask<int[]?> GetSemanticTokensDataAsync(Solution solution, DocumentId razorDocumentId, LinePositionSpan span, bool colorBackground, Guid correlationId, CancellationToken cancellationToken)
    {
        var razorDocument = solution.GetAdditionalDocument(razorDocumentId);
        if (razorDocument is null)
        {
            return null;
        }

        var documentContext = Create(razorDocument);

        // TODO: Telemetry?
        return await _razorSemanticTokensInfoService.GetSemanticTokensAsync(documentContext, span, colorBackground, correlationId, cancellationToken).ConfigureAwait(false);
    }

    public VersionedDocumentContext Create(TextDocument textDocument)
    {
        var documentSnapshot = _documentSnapshotFactory.GetOrCreate(textDocument);

        // HACK: Need to revisit version and projectContext here I guess
        return new VersionedDocumentContext(textDocument.CreateUri(), documentSnapshot, projectContext: null, version: 1);
    }
}

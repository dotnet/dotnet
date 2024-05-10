// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor.LinkedEditingRange;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteLinkedEditingRangeService(
    IServiceBroker serviceBroker,
    DocumentSnapshotFactory documentSnapshotFactory,
    ILoggerFactory loggerFactory)
    : RazorDocumentServiceBase(serviceBroker, documentSnapshotFactory), IRemoteLinkedEditingRangeService
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RemoteLinkedEditingRangeService>();

    public ValueTask<LinePositionSpan[]?> GetRangesAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, LinePosition linePosition, CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(
            solutionInfo,
            ServiceBrokerClient,
            solution => GetRangesAsync(solution, razorDocumentId, linePosition, cancellationToken),
            cancellationToken);

    public async ValueTask<LinePositionSpan[]?> GetRangesAsync(Solution solution, DocumentId razorDocumentId, LinePosition linePosition, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        if (await GetRazorCodeDocumentAsync(solution, razorDocumentId).ConfigureAwait(false) is not { } codeDocument)
        {
            return null;
        }

        return LinkedEditingRangeHelper.GetLinkedSpans(linePosition, codeDocument, _logger);
    }
}

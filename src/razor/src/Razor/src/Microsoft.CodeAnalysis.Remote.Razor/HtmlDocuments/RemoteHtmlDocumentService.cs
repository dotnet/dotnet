// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteHtmlDocumentService(
    IServiceBroker serviceBroker,
    DocumentSnapshotFactory documentSnapshotFactory)
    : RazorDocumentServiceBase(serviceBroker, documentSnapshotFactory), IRemoteHtmlDocumentService
{
    public ValueTask<string?> GetHtmlDocumentTextAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, CancellationToken cancellationToken)
       => RazorBrokeredServiceImplementation.RunServiceAsync(
           solutionInfo,
           ServiceBrokerClient,
           solution => GetHtmlDocumentTextAsync(solution, razorDocumentId, cancellationToken),
           cancellationToken);

    private async ValueTask<string?> GetHtmlDocumentTextAsync(Solution solution, DocumentId razorDocumentId, CancellationToken _)
    {
        if (await GetRazorCodeDocumentAsync(solution, razorDocumentId) is not { } codeDocument)
        {
            return null;
        }

        return codeDocument.GetHtmlSourceText().ToString();
    }
}

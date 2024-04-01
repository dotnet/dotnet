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
    : RazorServiceBase(serviceBroker), IRemoteHtmlDocumentService
{
    private readonly DocumentSnapshotFactory _documentSnapshotFactory = documentSnapshotFactory;

    public ValueTask<string?> GetHtmlDocumentTextAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, CancellationToken cancellationToken)
       => RazorBrokeredServiceImplementation.RunServiceAsync(
            solutionInfo,
            ServiceBrokerClient,
            solution => GetHtmlDocumentTextAsync(solution, razorDocumentId, cancellationToken),
            cancellationToken);

    private async ValueTask<string?> GetHtmlDocumentTextAsync(Solution solution, DocumentId razorDocumentId, CancellationToken _)
    {
        var razorDocument = solution.GetAdditionalDocument(razorDocumentId);
        if (razorDocument is null)
        {
            return null;
        }

        var documentSnapshot = _documentSnapshotFactory.GetOrCreate(razorDocument);
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync();

        return codeDocument.GetHtmlSourceText().ToString();
    }
}

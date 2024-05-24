// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
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
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetHtmlDocumentTextAsync(context, cancellationToken),
            cancellationToken);

    private async ValueTask<string?> GetHtmlDocumentTextAsync(RemoteDocumentContext documentContext, CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        return codeDocument.GetHtmlSourceText().ToString();
    }
}

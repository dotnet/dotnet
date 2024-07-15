// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal abstract class RazorDocumentServiceBase(
    IRazorServiceBroker serviceBroker,
    DocumentSnapshotFactory documentSnapshotFactory)
    : RazorServiceBase(serviceBroker)
{
    protected DocumentSnapshotFactory DocumentSnapshotFactory { get; } = documentSnapshotFactory;

    protected ValueTask<T> RunServiceAsync<T>(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, Func<RemoteDocumentContext, ValueTask<T>> implementation, CancellationToken cancellationToken)
    {
        return RunServiceAsync(
            solutionInfo,
            solution =>
            {
                var documentContext = CreateRazorDocumentContext(solution, razorDocumentId);
                if (documentContext is null)
                {
                    return default;
                }

                return implementation(documentContext);
            },
            cancellationToken);
    }

    private RemoteDocumentContext? CreateRazorDocumentContext(Solution solution, DocumentId razorDocumentId)
    {
        var razorDocument = solution.GetAdditionalDocument(razorDocumentId);
        if (razorDocument is null)
        {
            return null;
        }

        var documentSnapshot = DocumentSnapshotFactory.GetOrCreate(razorDocument);

        return new RemoteDocumentContext(razorDocument.CreateUri(), documentSnapshot);
    }
}

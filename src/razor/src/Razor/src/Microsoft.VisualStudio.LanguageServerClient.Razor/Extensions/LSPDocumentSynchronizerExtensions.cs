// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using static Microsoft.VisualStudio.LanguageServer.ContainedLanguage.DefaultLSPDocumentSynchronizer;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

internal static class LSPDocumentSynchronizerExtensions
{
    public static Task<SynchronizedResult<TVirtualDocumentSnapshot>> TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(
        this LSPDocumentSynchronizer synchronizer,
        TrackingLSPDocumentManager documentManager,
        int requiredHostDocumentVersion,
        TextDocumentIdentifier hostDocument,
        CancellationToken cancellationToken,
        bool rejectOnNewerParallelRequest = true)
        where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        if (hostDocument.GetProjectContext() is { } projectContext &&
            FindVirtualDocumentUri<TVirtualDocumentSnapshot>(documentManager, hostDocument.Uri, projectContext) is { } virtualDocumentUri)
        {
            return synchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, virtualDocumentUri, rejectOnNewerParallelRequest, cancellationToken);
        }

        return synchronizer.TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, cancellationToken);
    }

    public static SynchronizedResult<TVirtualDocumentSnapshot>? TryReturnPossiblyFutureSnapshot<TVirtualDocumentSnapshot>(
        this DefaultLSPDocumentSynchronizer synchronizer,
        TrackingLSPDocumentManager documentManager,
        int requiredHostDocumentVersion,
        TextDocumentIdentifier hostDocument) where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        if (hostDocument.GetProjectContext() is { } projectContext &&
            FindVirtualDocumentUri<TVirtualDocumentSnapshot>(documentManager, hostDocument.Uri, projectContext) is { } virtualDocumentUri)
        {
            return synchronizer.TryReturnPossiblyFutureSnapshot<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri, virtualDocumentUri);
        }

        return synchronizer.TryReturnPossiblyFutureSnapshot<TVirtualDocumentSnapshot>(requiredHostDocumentVersion, hostDocument.Uri);
    }

    private static Uri? FindVirtualDocumentUri<TVirtualDocumentSnapshot>(
        TrackingLSPDocumentManager documentManager,
        Uri hostDocumentUri,
        VSProjectContext projectContext) where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        if (!documentManager.TryGetDocument(hostDocumentUri, out var documentSnapshot) ||
            !documentSnapshot.TryGetAllVirtualDocuments<TVirtualDocumentSnapshot>(out var virtualDocuments))
        {
            throw new InvalidOperationException("Could not get the virtual documents for the requested document.");
        }

        foreach (var virtualDocument in virtualDocuments)
        {
            // NOTE: This is _NOT_ the right snapshot, or at least cannot be assumed to be, we just need the Uri
            // to pass to the synchronizer, so it can get the right snapshot
            if (virtualDocument is CSharpVirtualDocumentSnapshot csharpVirtualDocument &&
                IsMatch(csharpVirtualDocument.ProjectKey, projectContext))
            {
                return csharpVirtualDocument.Uri;
            }
        }

        return null;
    }

#pragma warning disable IDE0060 // Remove unused parameter
    private static bool IsMatch(ProjectKey projectKey, VSProjectContext projectContext)
    {
        return true;
    }
#pragma warning restore IDE0060 // Remove unused parameter
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal abstract class RazorDocumentServiceBase(
    IServiceBroker serviceBroker,
    DocumentSnapshotFactory documentSnapshotFactory)
    : RazorServiceBase(serviceBroker)
{
    protected DocumentSnapshotFactory DocumentSnapshotFactory { get; } = documentSnapshotFactory;

    protected async Task<(TextDocument?, RazorCodeDocument?)> GetRazorTextAndCodeDocumentsAsync(Solution solution, DocumentId razorDocumentId)
    {
        var razorDocument = solution.GetAdditionalDocument(razorDocumentId);
        if (razorDocument is null)
        {
            return (null, null);
        }

        var snapshot = DocumentSnapshotFactory.GetOrCreate(razorDocument);
        var codeDocument = await snapshot.GetGeneratedOutputAsync().ConfigureAwait(false);

        return (razorDocument, codeDocument);
    }

    protected async Task<RazorCodeDocument?> GetRazorCodeDocumentAsync(Solution solution, DocumentId razorDocumentId)
    {
        var (_, codeDocument) = await GetRazorTextAndCodeDocumentsAsync(solution, razorDocumentId);

        return codeDocument;
    }

    protected VersionedDocumentContext CreateRazorDocumentContext(TextDocument textDocument)
    {
        var documentSnapshot = DocumentSnapshotFactory.GetOrCreate(textDocument);

        // HACK: Need to revisit version and projectContext here I guess
        return new VersionedDocumentContext(textDocument.CreateUri(), documentSnapshot, projectContext: null, version: 1);
    }
}

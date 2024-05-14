// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.DocumentPresentation;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteUriPresentationService(
    IServiceBroker serviceBroker,
    IRazorDocumentMappingService documentMappingService,
    DocumentSnapshotFactory documentSnapshotFactory,
    ILoggerFactory loggerFactory)
    : RazorDocumentServiceBase(serviceBroker, documentSnapshotFactory), IRemoteUriPresentationService
{
    private readonly IRazorDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RemoteUriPresentationService>();

    public ValueTask<TextChange?> GetPresentationAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, LinePositionSpan span, Uri[]? uris, CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(
            solutionInfo,
            ServiceBrokerClient,
            solution => GetPresentationAsync(solution, razorDocumentId, span, uris, cancellationToken),
            cancellationToken);

    private async ValueTask<TextChange?> GetPresentationAsync(Solution solution, DocumentId razorDocumentId, LinePositionSpan span, Uri[]? uris, CancellationToken cancellationToken)
    {
        var (razorDocument, codeDocument) = await GetRazorTextAndCodeDocumentsAsync(solution, razorDocumentId).ConfigureAwait(false);

        if (razorDocument == null || codeDocument == null)
        {
            return null;
        }

        // If razorDocument was null, codeDocument above would've been null as well
        var sourceText = await razorDocument.AssumeNotNull().GetTextAsync(cancellationToken);

        if (!sourceText.TryGetAbsoluteIndex(span.Start.Line, span.Start.Character, out var index))
        {
            return null;
        }

        var languageKind = _documentMappingService.GetLanguageKind(codeDocument, index, rightAssociative: true);

        if (languageKind is not RazorLanguageKind.Html)
        {
            // Roslyn doesn't currently support Uri presentation, and whilst it might seem counter intuitive,
            // our support for Uri presentation is to insert a Html tag, so we only support Html

            // If Roslyn add support in future then this is where it would go.
            return null;
        }

        var razorFileUri = UriPresentationHelper.GetComponentFileNameFromUriPresentationRequest(uris, _logger);
        if (razorFileUri is null)
        {
            return null;
        }

        // Make sure we go through Roslyn to go from the Uri the client sent us, to one that it has a chance of finding in the solution
        var uriToFind = RazorUri.GetDocumentFilePathFromUri(razorFileUri);
        var ids = razorDocument.Project.Solution.GetDocumentIdsWithFilePath(uriToFind);
        if (ids.Length == 0)
        {
            return null;
        }

        // We assume linked documents would produce the same component tag so just take the first
        var otherDocument = razorDocument.Project.Solution.GetAdditionalDocument(ids[0]);
        if (otherDocument is null)
        {
            return null;
        }

        var otherSnapshot = DocumentSnapshotFactory.GetOrCreate(otherDocument);
        var descriptor = await otherSnapshot.TryGetTagHelperDescriptorAsync(cancellationToken).ConfigureAwait(false);

        if (descriptor is null)
        {
            return null;
        }

        var tag = descriptor.TryGetComponentTag();
        if (tag is null)
        {
            return null;
        }

        return new TextChange(span.ToTextSpan(sourceText), tag);
    }
}

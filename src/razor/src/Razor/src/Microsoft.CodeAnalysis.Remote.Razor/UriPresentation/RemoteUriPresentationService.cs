// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.DocumentPresentation;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteUriPresentationService(
    IRazorServiceBroker serviceBroker,
    IRazorDocumentMappingService documentMappingService,
    DocumentSnapshotFactory documentSnapshotFactory,
    ILoggerFactory loggerFactory)
    : RazorDocumentServiceBase(serviceBroker, documentSnapshotFactory), IRemoteUriPresentationService
{
    private readonly IRazorDocumentMappingService _documentMappingService = documentMappingService;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RemoteUriPresentationService>();

    public ValueTask<TextChange?> GetPresentationAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, LinePositionSpan span, Uri[]? uris, CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetPresentationAsync(context, span, uris, cancellationToken),
            cancellationToken);

    private async ValueTask<TextChange?> GetPresentationAsync(RemoteDocumentContext context, LinePositionSpan span, Uri[]? uris, CancellationToken cancellationToken)
    {
        var sourceText = await context.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(span.Start.Line, span.Start.Character, out var index))
        {
            return null;
        }

        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

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

        var solution = context.TextDocument.Project.Solution;

        // Make sure we go through Roslyn to go from the Uri the client sent us, to one that it has a chance of finding in the solution
        var uriToFind = RazorUri.GetDocumentFilePathFromUri(razorFileUri);
        var ids = solution.GetDocumentIdsWithFilePath(uriToFind);
        if (ids.Length == 0)
        {
            return null;
        }

        // We assume linked documents would produce the same component tag so just take the first
        var otherDocument = solution.GetAdditionalDocument(ids[0]);
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

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.DocumentPresentation;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Response = Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Microsoft.CodeAnalysis.Text.TextChange?>;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed partial class RemoteUriPresentationService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteUriPresentationService
{
    internal sealed class Factory : FactoryBase<IRemoteUriPresentationService>
    {
        protected override IRemoteUriPresentationService CreateService(in ServiceArgs args)
            => new RemoteUriPresentationService(in args);
    }

    private readonly IDocumentMappingService _documentMappingService = args.ExportProvider.GetExportedValue<IDocumentMappingService>();

    public ValueTask<Response> GetPresentationAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId razorDocumentId,
        LinePositionSpan span,
        Uri[]? uris,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetPresentationAsync(context, span, uris, cancellationToken),
            cancellationToken);

    private async ValueTask<Response> GetPresentationAsync(
        RemoteDocumentContext context,
        LinePositionSpan span,
        Uri[]? uris,
        CancellationToken cancellationToken)
    {
        var sourceText = await context.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(span.Start, out var index))
        {
            // If the position is invalid then we shouldn't expect to be able to handle a Html response
            return Response.NoFurtherHandling;
        }

        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        var languageKind = _documentMappingService.GetLanguageKind(codeDocument, index, rightAssociative: true);
        if (languageKind is not RazorLanguageKind.Html)
        {
            // Roslyn doesn't currently support Uri presentation, and whilst it might seem counter intuitive,
            // our support for Uri presentation is to insert a Html tag, so we only support Html

            // If Roslyn add support in future then this is where it would go.
            return Response.NoFurtherHandling;
        }

        var razorFileUri = UriPresentationHelper.GetComponentFileNameFromUriPresentationRequest(uris, Logger);
        if (razorFileUri is null)
        {
            return Response.CallHtml;
        }

        var solution = context.TextDocument.Project.Solution;

        // Make sure we go through Roslyn to go from the Uri the client sent us, to one that it has a chance of finding in the solution
        var ids = solution.GetDocumentIdsWithUri(razorFileUri);
        if (ids.Length == 0)
        {
            return Response.CallHtml;
        }

        // We assume linked documents would produce the same component tag so just take the first
        var otherDocument = solution.GetAdditionalDocument(ids[0]);
        if (otherDocument is null)
        {
            return Response.CallHtml;
        }

        var otherSnapshot = DocumentSnapshotFactory.GetOrCreate(otherDocument);
        var descriptor = await otherSnapshot.TryGetTagHelperDescriptorAsync(cancellationToken).ConfigureAwait(false);

        if (descriptor is null)
        {
            return Response.CallHtml;
        }

        var tag = descriptor.TryGetComponentTag();
        if (tag is null)
        {
            return Response.CallHtml;
        }

        return Response.Results(new TextChange(sourceText.GetTextSpan(span), tag));
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.DocumentPresentation;
using Microsoft.CodeAnalysis.Razor.Logging;
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
    : RazorServiceBase(serviceBroker), IRemoteUriPresentationService
{
    private readonly IRazorDocumentMappingService _documentMappingService = documentMappingService;
    private readonly DocumentSnapshotFactory _documentSnapshotFactory = documentSnapshotFactory;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RemoteUriPresentationService>();

    public ValueTask<TextChange?> GetPresentationAsync(RazorPinnedSolutionInfoWrapper solutionInfo, DocumentId razorDocumentId, LinePositionSpan span, Uri[]? uris, CancellationToken cancellationToken)
        => RazorBrokeredServiceImplementation.RunServiceAsync(
            solutionInfo,
            ServiceBrokerClient,
            solution => GetPresentationAsync(solution, razorDocumentId, span, uris, cancellationToken),
            cancellationToken);

    private async ValueTask<TextChange?> GetPresentationAsync(Solution solution, DocumentId razorDocumentId, LinePositionSpan span, Uri[]? uris, CancellationToken cancellationToken)
    {
        var razorDocument = solution.GetAdditionalDocument(razorDocumentId);
        if (razorDocument is null)
        {
            return null;
        }

        var snapshot = _documentSnapshotFactory.GetOrCreate(razorDocument);
        var codeDocument = await snapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        var sourceText = await razorDocument.GetTextAsync(cancellationToken).ConfigureAwait(false);

        if (!sourceText.TryGetAbsoluteIndex(span.Start.Line, span.Start.Character, out var index))
        {
            return null;
        }

        var languageKind = _documentMappingService.GetLanguageKind(codeDocument, index, rightAssociative: true);

        var razorFileUri = UriPresentationHelper.GetComponentFileNameFromUriPresentationRequest(languageKind, uris, _logger);
        if (razorFileUri is null)
        {
            return null;
        }

        var ids = razorDocument.Project.Solution.GetDocumentIdsWithFilePath(GetDocumentFilePathFromUri(razorFileUri));
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

        var otherSnapshot = _documentSnapshotFactory.GetOrCreate(otherDocument);
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

    // TODO: Call the real Roslyn API once https://github.com/dotnet/roslyn/pull/73289 is merged
    public static string GetDocumentFilePathFromUri(Uri uri)
        => uri.IsFile ? uri.LocalPath : uri.AbsoluteUri;
}

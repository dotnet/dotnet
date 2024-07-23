// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentHighlight;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Response = Microsoft.CodeAnalysis.Razor.Remote.RemoteResponse<Microsoft.CodeAnalysis.Razor.Protocol.DocumentHighlight.RemoteDocumentHighlight[]?>;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed partial class RemoteDocumentHighlightService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteDocumentHighlightService
{
    internal sealed class Factory : FactoryBase<IRemoteDocumentHighlightService>
    {
        protected override IRemoteDocumentHighlightService CreateService(in ServiceArgs args)
            => new RemoteDocumentHighlightService(in args);
    }

    private readonly IRazorDocumentMappingService _documentMappingService = args.ExportProvider.GetExportedValue<IRazorDocumentMappingService>();
    private readonly IFilePathService _filePathService = args.ExportProvider.GetExportedValue<IFilePathService>();

    public ValueTask<Response> GetHighlightsAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId razorDocumentId,
        LinePosition position,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetHighlightsAsync(context, position, cancellationToken),
            cancellationToken);

    private async ValueTask<Response> GetHighlightsAsync(
        RemoteDocumentContext context,
        LinePosition position,
        CancellationToken cancellationToken)
    {
        var sourceText = await context.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(position.Line, position.Character, out var index))
        {
            return Response.NoFurtherHandling;
        }

        var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        var languageKind = _documentMappingService.GetLanguageKind(codeDocument, index, rightAssociative: true);
        if (languageKind is RazorLanguageKind.Html)
        {
            return Response.CallHtml;
        }
        else if (languageKind is RazorLanguageKind.Razor)
        {
            return Response.NoFurtherHandling;
        }

        var csharpDocument = codeDocument.GetCSharpDocument();
        if (_documentMappingService.TryMapToGeneratedDocumentPosition(csharpDocument, index, out var mappedPosition, out _))
        {
            var generatedDocument = await context.GetGeneratedDocumentAsync(_filePathService, cancellationToken).ConfigureAwait(false);

            var highlights = await DocumentHighlights.GetHighlightsAsync(generatedDocument, mappedPosition, cancellationToken).ConfigureAwait(false);

            if (highlights is not null)
            {
                using var results = new PooledArrayBuilder<RemoteDocumentHighlight>();

                foreach (var highlight in highlights)
                {
                    if (_documentMappingService.TryMapToHostDocumentRange(csharpDocument, highlight.Range.ToLinePositionSpan(), out var mappedRange))
                    {
                        highlight.Range = mappedRange.ToRLSPRange();
                        results.Add(RemoteDocumentHighlight.FromRLSPDocumentHighlight(highlight));
                    }
                }

                return Response.Results(results.ToArray());
            }
        }

        return Response.NoFurtherHandling;
    }
}

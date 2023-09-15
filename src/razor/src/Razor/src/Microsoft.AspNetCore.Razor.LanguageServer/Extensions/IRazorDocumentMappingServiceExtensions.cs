// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class IRazorDocumentMappingServiceExtensions
{
    public static bool TryMapToHostDocumentRange(this IRazorDocumentMappingService service, IRazorGeneratedDocument generatedDocument, LinePositionSpan projectedRange, out LinePositionSpan originalRange)
        => service.TryMapToHostDocumentRange(generatedDocument, projectedRange, MappingBehavior.Strict, out originalRange);

    public static bool TryMapToHostDocumentRange(this IRazorDocumentMappingService service, IRazorGeneratedDocument generatedDocument, Range projectedRange, [NotNullWhen(true)] out Range? originalRange)
        => service.TryMapToHostDocumentRange(generatedDocument, projectedRange, MappingBehavior.Strict, out originalRange);

    public static async Task<DocumentPositionInfo> GetPositionInfoAsync(this IRazorDocumentMappingService service, DocumentContext documentContext, int hostDocumentIndex, CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);

        sourceText.GetLineAndOffset(hostDocumentIndex, out var line, out var character);
        var position = new Position(line, character);

        var languageKind = service.GetLanguageKind(codeDocument, hostDocumentIndex, rightAssociative: false);
        if (languageKind is not RazorLanguageKind.Razor)
        {
            var generatedDocument = languageKind is RazorLanguageKind.CSharp
                ? (IRazorGeneratedDocument)codeDocument.GetCSharpDocument()
                : codeDocument.GetHtmlDocument();
            if (service.TryMapToGeneratedDocumentPosition(generatedDocument, hostDocumentIndex, out Position? mappedPosition, out _))
            {
                // For C# locations, we attempt to return the corresponding position
                // within the projected document
                position = mappedPosition;
            }
            else
            {
                // It no longer makes sense to think of this location as C# or Html, since it doesn't
                // correspond to any position in the projected document. This should not happen
                // since there should be source mappings for all the C# spans.
                languageKind = RazorLanguageKind.Razor;
            }
        }

        return new DocumentPositionInfo(languageKind, position, hostDocumentIndex);
    }

    public static bool TryMapToHostDocumentRange(this IRazorDocumentMappingService service, IRazorGeneratedDocument generatedDocument, Range generatedDocumentRange, MappingBehavior mappingBehavior, [NotNullWhen(true)] out Range? hostDocumentRange)
    {
        var result = service.TryMapToHostDocumentRange(generatedDocument, generatedDocumentRange.ToLinePositionSpan(), mappingBehavior, out var hostDocumentLinePositionSpan);
        hostDocumentRange = result ? hostDocumentLinePositionSpan.ToRange() : null;
        return result;
    }

    public static bool TryMapToGeneratedDocumentRange(this IRazorDocumentMappingService service, IRazorGeneratedDocument generatedDocument, Range hostDocumentRange, [NotNullWhen(true)] out Range? generatedDocumentRange)
    {
        var result = service.TryMapToGeneratedDocumentRange(generatedDocument, hostDocumentRange.ToLinePositionSpan(), out var generatedDocumentLinePositionSpan);
        generatedDocumentRange = result ? generatedDocumentLinePositionSpan.ToRange() : null;
        return result;
    }

    public static bool TryMapToHostDocumentPosition(this IRazorDocumentMappingService service, IRazorGeneratedDocument generatedDocument, int generatedDocumentIndex, [NotNullWhen(true)] out Position? hostDocumentPosition, out int hostDocumentIndex)
    {
        var result = service.TryMapToHostDocumentPosition(generatedDocument, generatedDocumentIndex, out var hostDocumentLinePosition, out hostDocumentIndex);
        hostDocumentPosition = result ? hostDocumentLinePosition.ToPosition() : null;
        return result;
    }

    public static bool TryMapToGeneratedDocumentPosition(this IRazorDocumentMappingService service, IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, [NotNullWhen(true)] out Position? generatedPosition, out int generatedIndex)
    {
        var result = service.TryMapToGeneratedDocumentPosition(generatedDocument, hostDocumentIndex, out var generatedLinePosition, out generatedIndex);
        generatedPosition = result ? generatedLinePosition.ToPosition() : null;
        return result;
    }

    public static bool TryMapToGeneratedDocumentOrNextCSharpPosition(this IRazorDocumentMappingService service, IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, [NotNullWhen(true)] out Position? generatedPosition, out int generatedIndex)
    {
        var result = service.TryMapToGeneratedDocumentOrNextCSharpPosition(generatedDocument, hostDocumentIndex, out var generatedLinePosition, out generatedIndex);
        generatedPosition = result ? generatedLinePosition.ToPosition() : null;
        return result;
    }

    /// <summary>
    /// Maps a range in the specified generated document uri to a range in the Razor document that owns the
    /// generated document. If the uri passed in is not for a generated document, or the range cannot be mapped
    /// for some other reason, the original passed in range is returned unchanged.
    /// </summary>
    public static async Task<(Uri MappedDocumentUri, Range MappedRange)> MapToHostDocumentUriAndRangeAsync(this IRazorDocumentMappingService service, Uri generatedDocumentUri, Range generatedDocumentRange, CancellationToken cancellationToken)
    {
        var result = await service.MapToHostDocumentUriAndRangeAsync(generatedDocumentUri, generatedDocumentRange.ToLinePositionSpan(), cancellationToken).ConfigureAwait(false);
        return (result.MappedDocumentUri, result.MappedRange.ToRange());
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal abstract class RazorDocumentMappingService
{
    public abstract TextEdit[] GetProjectedDocumentEdits(IRazorGeneratedDocument generatedDocument, TextEdit[] edits);

    public abstract bool TryMapFromProjectedDocumentRange(IRazorGeneratedDocument generatedDocument, Range projectedRange, [NotNullWhen(true)] out Range? originalRange);

    public abstract bool TryMapFromProjectedDocumentRange(IRazorGeneratedDocument generatedDocument, Range projectedRange, MappingBehavior mappingBehavior, [NotNullWhen(true)] out Range? originalRange);

    public abstract bool TryMapToProjectedDocumentRange(IRazorGeneratedDocument generatedDocument, Range originalRange, [NotNullWhen(true)] out Range? projectedRange);

    public abstract bool TryMapFromProjectedDocumentPosition(IRazorGeneratedDocument generatedDocument, int csharpAbsoluteIndex, [NotNullWhen(true)] out Position? originalPosition, out int originalIndex);

    public abstract bool TryMapToProjectedDocumentPosition(IRazorGeneratedDocument generatedDocument, int absoluteIndex, [NotNullWhen(true)] out Position? projectedPosition, out int projectedIndex);

    public abstract bool TryMapToProjectedDocumentOrNextCSharpPosition(IRazorGeneratedDocument generatedDocument, int absoluteIndex, [NotNullWhen(true)] out Position? projectedPosition, out int projectedIndex);

    public abstract RazorLanguageKind GetLanguageKind(RazorCodeDocument codeDocument, int originalIndex, bool rightAssociative);

    public abstract Task<WorkspaceEdit> RemapWorkspaceEditAsync(WorkspaceEdit workspaceEdit, CancellationToken cancellationToken);

    /// <summary>
    /// Maps a range in the specified virtual document uri to a range in the Razor document that owns the
    /// virtual document. If the uri passed in is not for a virtual document, or the range cannot be mapped
    /// for some other reason, the original passed in range is returned unchanged.
    /// </summary>
    public abstract Task<(Uri MappedDocumentUri, Range MappedRange)> MapFromProjectedDocumentRangeAsync(Uri virtualDocumentUri, Range projectedRange, CancellationToken cancellationToken);

    public async Task<Projection> GetProjectionAsync(DocumentContext documentContext, int absoluteIndex, CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);

        sourceText.GetLineAndOffset(absoluteIndex, out var line, out var character);
        var projectedPosition = new Position(line, character);

        var languageKind = GetLanguageKind(codeDocument, absoluteIndex, rightAssociative: false);
        if (languageKind is not RazorLanguageKind.Razor)
        {
            var generatedDocument = languageKind is RazorLanguageKind.CSharp
                ? (IRazorGeneratedDocument)codeDocument.GetCSharpDocument()
                : codeDocument.GetHtmlDocument();
            if (TryMapToProjectedDocumentPosition(generatedDocument, absoluteIndex, out var mappedPosition, out _))
            {
                // For C# locations, we attempt to return the corresponding position
                // within the projected document
                projectedPosition = mappedPosition;
            }
            else
            {
                // It no longer makes sense to think of this location as C# or Html, since it doesn't
                // correspond to any position in the projected document. This should not happen
                // since there should be source mappings for all the C# spans.
                languageKind = RazorLanguageKind.Razor;
            }
        }

        return new Projection(languageKind, projectedPosition, absoluteIndex);
    }

    public async Task<Projection?> TryGetProjectionAsync(DocumentContext documentContext, Position position, ILogger logger, CancellationToken cancellationToken)
    {
        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!position.TryGetAbsoluteIndex(sourceText, logger, out var absoluteIndex))
        {
            return null;
        }

        return await GetProjectionAsync(documentContext, absoluteIndex, cancellationToken).ConfigureAwait(false);
    }
}

internal record Projection(RazorLanguageKind LanguageKind, Position Position, int AbsoluteIndex);

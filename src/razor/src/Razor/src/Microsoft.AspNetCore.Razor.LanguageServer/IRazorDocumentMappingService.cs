// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IRazorDocumentMappingService
{
    TextEdit[] GetHostDocumentEdits(IRazorGeneratedDocument generatedDocument, TextEdit[] generatedDocumentEdits);

    bool TryMapToHostDocumentRange(IRazorGeneratedDocument generatedDocument, LinePositionSpan generatedDocumentRange, MappingBehavior mappingBehavior, out LinePositionSpan hostDocumentRange);

    bool TryMapToGeneratedDocumentRange(IRazorGeneratedDocument generatedDocument, LinePositionSpan hostDocumentRange, out LinePositionSpan generatedDocumentRange);

    bool TryMapToHostDocumentPosition(IRazorGeneratedDocument generatedDocument, int generatedDocumentIndex, out LinePosition hostDocumentPosition, out int hostDocumentIndex);

    bool TryMapToGeneratedDocumentPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex);

    bool TryMapToGeneratedDocumentOrNextCSharpPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex);

    RazorLanguageKind GetLanguageKind(RazorCodeDocument codeDocument, int hostDocumentIndex, bool rightAssociative);

    Task<WorkspaceEdit> RemapWorkspaceEditAsync(WorkspaceEdit workspaceEdit, CancellationToken cancellationToken);

    /// <summary>
    /// Maps a range in the specified generated document uri to a range in the Razor document that owns the
    /// generated document. If the uri passed in is not for a generated document, or the range cannot be mapped
    /// for some other reason, the original passed in range is returned unchanged.
    /// </summary>
    Task<(Uri MappedDocumentUri, LinePositionSpan MappedRange)> MapToHostDocumentUriAndRangeAsync(Uri generatedDocumentUri, LinePositionSpan generatedDocumentRange, CancellationToken cancellationToken);
}

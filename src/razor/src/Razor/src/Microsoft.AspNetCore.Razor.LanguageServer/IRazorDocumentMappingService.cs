// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IRazorDocumentMappingService
{
    TextEdit[] GetHostDocumentEdits(IRazorGeneratedDocument generatedDocument, TextEdit[] generatedDocumentEdits);

    bool TryMapToHostDocumentRange(IRazorGeneratedDocument generatedDocument, Range generatedDocumentRange, MappingBehavior mappingBehavior, [NotNullWhen(true)] out Range? hostDocumentRange);

    bool TryMapToGeneratedDocumentRange(IRazorGeneratedDocument generatedDocument, Range hostDocumentRange, [NotNullWhen(true)] out Range? generatedDocumentRange);

    bool TryMapToHostDocumentPosition(IRazorGeneratedDocument generatedDocument, int generatedDocumentIndex, [NotNullWhen(true)] out Position? hostDocumentPosition, out int hostDocumentIndex);

    bool TryMapToGeneratedDocumentPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, [NotNullWhen(true)] out Position? generatedPosition, out int generatedIndex);

    bool TryMapToGeneratedDocumentOrNextCSharpPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, [NotNullWhen(true)] out Position? generatedPosition, out int generatedIndex);

    RazorLanguageKind GetLanguageKind(RazorCodeDocument codeDocument, int hostDocumentIndex, bool rightAssociative);

    Task<WorkspaceEdit> RemapWorkspaceEditAsync(WorkspaceEdit workspaceEdit, CancellationToken cancellationToken);

    /// <summary>
    /// Maps a range in the specified generated document uri to a range in the Razor document that owns the
    /// generated document. If the uri passed in is not for a generated document, or the range cannot be mapped
    /// for some other reason, the original passed in range is returned unchanged.
    /// </summary>
    Task<(Uri MappedDocumentUri, Range MappedRange)> MapToHostDocumentUriAndRangeAsync(Uri generatedDocumentUri, Range generatedDocumentRange, CancellationToken cancellationToken);
}

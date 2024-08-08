// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal interface IDocumentMappingService
{
    IEnumerable<TextChange> GetHostDocumentEdits(IRazorGeneratedDocument generatedDocument, IEnumerable<TextChange> generatedDocumentEdits);

    bool TryMapToHostDocumentRange(IRazorGeneratedDocument generatedDocument, LinePositionSpan generatedDocumentRange, MappingBehavior mappingBehavior, out LinePositionSpan hostDocumentRange);

    bool TryMapToGeneratedDocumentRange(IRazorGeneratedDocument generatedDocument, LinePositionSpan hostDocumentRange, out LinePositionSpan generatedDocumentRange);

    bool TryMapToHostDocumentPosition(IRazorGeneratedDocument generatedDocument, int generatedDocumentIndex, out LinePosition hostDocumentPosition, out int hostDocumentIndex);

    bool TryMapToGeneratedDocumentPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex);

    bool TryMapToGeneratedDocumentOrNextCSharpPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex);

    RazorLanguageKind GetLanguageKind(RazorCodeDocument codeDocument, int hostDocumentIndex, bool rightAssociative);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.DocumentMapping;

internal interface IDocumentMappingService
{
    IEnumerable<TextChange> GetHostDocumentEdits(IRazorGeneratedDocument generatedDocument, ImmutableArray<TextChange> generatedDocumentEdits);

    bool TryMapToHostDocumentRange(IRazorGeneratedDocument generatedDocument, LinePositionSpan generatedDocumentRange, MappingBehavior mappingBehavior, out LinePositionSpan hostDocumentRange);

    bool TryMapToGeneratedDocumentRange(IRazorGeneratedDocument generatedDocument, LinePositionSpan hostDocumentRange, out LinePositionSpan generatedDocumentRange);

    bool TryMapToHostDocumentPosition(IRazorGeneratedDocument generatedDocument, int generatedDocumentIndex, out LinePosition hostDocumentPosition, out int hostDocumentIndex);

    bool TryMapToGeneratedDocumentPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex);

    bool TryMapToGeneratedDocumentOrNextCSharpPosition(IRazorGeneratedDocument generatedDocument, int hostDocumentIndex, out LinePosition generatedPosition, out int generatedIndex);
}

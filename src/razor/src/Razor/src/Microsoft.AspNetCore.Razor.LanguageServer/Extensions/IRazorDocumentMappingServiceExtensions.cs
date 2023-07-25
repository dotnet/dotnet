// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class IRazorDocumentMappingServiceExtensions
{
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
            if (service.TryMapToGeneratedDocumentPosition(generatedDocument, hostDocumentIndex, out var mappedPosition, out _))
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
}

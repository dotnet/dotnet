// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal abstract class RazorFormattingService
{
    public abstract Task<TextEdit[]> FormatAsync(
        VersionedDocumentContext documentContext,
        Range? range,
        FormattingOptions options,
        CancellationToken cancellationToken);

    public abstract Task<TextEdit[]> FormatOnTypeAsync(
       DocumentContext documentContext,
       RazorLanguageKind kind,
       TextEdit[] formattedEdits,
       FormattingOptions options,
       int hostDocumentIndex,
       char triggerCharacter,
       CancellationToken cancellationToken);

    public abstract Task<TextEdit[]> FormatCodeActionAsync(
        DocumentContext documentContext,
        RazorLanguageKind kind,
        TextEdit[] formattedEdits,
        FormattingOptions options,
        CancellationToken cancellationToken);

    public abstract Task<TextEdit[]> FormatSnippetAsync(
        DocumentContext documentContext,
        RazorLanguageKind kind,
        TextEdit[] edits,
        FormattingOptions options,
        CancellationToken cancellationToken);
}

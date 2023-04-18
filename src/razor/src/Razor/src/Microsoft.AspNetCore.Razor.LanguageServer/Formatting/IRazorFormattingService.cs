// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal interface IRazorFormattingService
{
    Task<TextEdit[]> FormatAsync(
       VersionedDocumentContext documentContext,
       Range? range,
       FormattingOptions options,
       CancellationToken cancellationToken);

    Task<TextEdit[]> FormatOnTypeAsync(
      DocumentContext documentContext,
      RazorLanguageKind kind,
      TextEdit[] formattedEdits,
      FormattingOptions options,
      int hostDocumentIndex,
      char triggerCharacter,
      CancellationToken cancellationToken);

    Task<TextEdit[]> FormatCodeActionAsync(
       DocumentContext documentContext,
       RazorLanguageKind kind,
       TextEdit[] formattedEdits,
       FormattingOptions options,
       CancellationToken cancellationToken);

    Task<TextEdit[]> FormatSnippetAsync(
       DocumentContext documentContext,
       RazorLanguageKind kind,
       TextEdit[] edits,
       FormattingOptions options,
       CancellationToken cancellationToken);
}

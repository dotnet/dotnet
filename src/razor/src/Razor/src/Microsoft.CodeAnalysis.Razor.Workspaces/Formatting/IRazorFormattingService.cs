// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal interface IRazorFormattingService
{
    Task<TextEdit[]> GetDocumentFormattingEditsAsync(
       DocumentContext documentContext,
       TextEdit[] htmlEdits,
       Range? range,
       RazorFormattingOptions options,
       CancellationToken cancellationToken);

    Task<TextEdit[]> GetHtmlOnTypeFormattingEditsAsync(
      DocumentContext documentContext,
      TextEdit[] htmlEdits,
      RazorFormattingOptions options,
      int hostDocumentIndex,
      char triggerCharacter,
      CancellationToken cancellationToken);

    Task<TextEdit[]> GetCSharpOnTypeFormattingEditsAsync(
      DocumentContext documentContext,
      RazorFormattingOptions options,
      int hostDocumentIndex,
      char triggerCharacter,
      CancellationToken cancellationToken);

    Task<TextEdit?> GetSingleCSharpEditAsync(
        DocumentContext documentContext,
        TextEdit csharpEdit,
        RazorFormattingOptions options,
        CancellationToken cancellationToken);

    Task<TextEdit?> GetCSharpCodeActionEditAsync(
       DocumentContext documentContext,
       TextEdit[] csharpEdits,
       RazorFormattingOptions options,
       CancellationToken cancellationToken);

    Task<TextEdit?> GetCSharpSnippetFormattingEditAsync(
       DocumentContext documentContext,
       TextEdit[] csharpEdits,
       RazorFormattingOptions options,
       CancellationToken cancellationToken);

    bool TryGetOnTypeFormattingTriggerKind(
        RazorCodeDocument codeDocument,
        int hostDocumentIndex,
        string triggerCharacter,
        out RazorLanguageKind triggerCharacterKind);
}

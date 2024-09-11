// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal interface IRazorFormattingService
{
    Task<ImmutableArray<TextChange>> GetDocumentFormattingChangesAsync(
       DocumentContext documentContext,
       ImmutableArray<TextChange> htmlEdits,
       LinePositionSpan? span,
       RazorFormattingOptions options,
       CancellationToken cancellationToken);

    Task<ImmutableArray<TextChange>> GetHtmlOnTypeFormattingChangesAsync(
      DocumentContext documentContext,
      ImmutableArray<TextChange> htmlEdits,
      RazorFormattingOptions options,
      int hostDocumentIndex,
      char triggerCharacter,
      CancellationToken cancellationToken);

    Task<ImmutableArray<TextChange>> GetCSharpOnTypeFormattingChangesAsync(
      DocumentContext documentContext,
      RazorFormattingOptions options,
      int hostDocumentIndex,
      char triggerCharacter,
      CancellationToken cancellationToken);

    Task<TextChange?> TryGetSingleCSharpEditAsync(
        DocumentContext documentContext,
        TextChange csharpEdit,
        RazorFormattingOptions options,
        CancellationToken cancellationToken);

    Task<TextChange?> TryGetCSharpCodeActionEditAsync(
       DocumentContext documentContext,
       ImmutableArray<TextChange> csharpEdits,
       RazorFormattingOptions options,
       CancellationToken cancellationToken);

    Task<TextChange?> TryGetCSharpSnippetFormattingEditAsync(
       DocumentContext documentContext,
       ImmutableArray<TextChange> csharpEdits,
       RazorFormattingOptions options,
       CancellationToken cancellationToken);

    bool TryGetOnTypeFormattingTriggerKind(
        RazorCodeDocument codeDocument,
        int hostDocumentIndex,
        string triggerCharacter,
        out RazorLanguageKind triggerCharacterKind);
}

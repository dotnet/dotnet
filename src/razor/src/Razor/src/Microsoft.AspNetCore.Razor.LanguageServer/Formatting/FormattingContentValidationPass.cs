// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal sealed class FormattingContentValidationPass(
    IDocumentMappingService documentMappingService,
    ILoggerFactory loggerFactory)
    : FormattingPassBase(documentMappingService)
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<FormattingContentValidationPass>();

    // We want this to run at the very end.
    public override int Order => DefaultOrder + 1000;

    public override bool IsValidationPass => true;

    // Internal for testing.
    internal bool DebugAssertsEnabled { get; set; } = true;

    public override Task<FormattingResult> ExecuteAsync(FormattingContext context, FormattingResult result, CancellationToken cancellationToken)
    {
        if (result.Kind != RazorLanguageKind.Razor)
        {
            // We don't care about changes to projected documents here.
            return Task.FromResult(result);
        }

        var text = context.SourceText;
        var edits = result.Edits;
        var changes = edits.Select(text.GetTextChange);
        var changedText = text.WithChanges(changes);

        if (!text.NonWhitespaceContentEquals(changedText))
        {
            // Looks like we removed some non-whitespace content as part of formatting. Oops.
            // Discard this formatting result.

            _logger.LogWarning($"{SR.Format_operation_changed_nonwhitespace}");

            foreach (var edit in edits)
            {
                if (edit.NewText.Any(c => !char.IsWhiteSpace(c)))
                {
                    _logger.LogWarning($"{SR.FormatEdit_at_adds(edit.Range.ToDisplayString(), edit.NewText)}");
                }
                else if (text.TryGetFirstNonWhitespaceOffset(text.GetTextSpan(edit.Range), out _))
                {
                    _logger.LogWarning($"{SR.FormatEdit_at_deletes(edit.Range.ToDisplayString(), text.ToString(text.GetTextSpan(edit.Range)))}");
                }
            }

            if (DebugAssertsEnabled)
            {
                Debug.Fail("A formatting result was rejected because it was going to change non-whitespace content in the document.");
            }

            return Task.FromResult(new FormattingResult([]));
        }

        return Task.FromResult(result);
    }
}

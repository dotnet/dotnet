// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal sealed class FormattingContentValidationPass(ILoggerFactory loggerFactory) : IFormattingPass
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<FormattingContentValidationPass>();

    // Internal for testing.
    internal bool DebugAssertsEnabled { get; set; } = true;

    public Task<TextEdit[]> ExecuteAsync(FormattingContext context, TextEdit[] edits, CancellationToken cancellationToken)
    {
        var text = context.SourceText;
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

            return Task.FromResult<TextEdit[]>([]);
        }

        return Task.FromResult(edits);
    }
}

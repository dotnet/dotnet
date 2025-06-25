// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal sealed class FormattingContentValidationPass(ILoggerFactory loggerFactory) : IFormattingValidationPass
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<FormattingContentValidationPass>();

    // Internal for testing.
    internal bool DebugAssertsEnabled { get; set; } = true;

    public Task<bool> IsValidAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        var text = context.SourceText;
        var changedText = text.WithChanges(changes);

        if (!text.NonWhitespaceContentEquals(changedText))
        {
            // Looks like we removed some non-whitespace content as part of formatting. Oops.
            // Discard this formatting result.

            _logger.LogWarning($"{SR.Format_operation_changed_nonwhitespace}");

            foreach (var change in changes)
            {
                if (change.NewText?.Any(c => !char.IsWhiteSpace(c)) ?? false)
                {
                    _logger.LogWarning($"{SR.FormatEdit_at_adds(text.GetLinePositionSpan(change.Span), change.NewText)}");
                }
                else if (text.TryGetFirstNonWhitespaceOffset(change.Span, out _))
                {
                    _logger.LogWarning($"{SR.FormatEdit_at_deletes(text.GetLinePositionSpan(change.Span), text.ToString(change.Span))}");
                }
            }

            if (DebugAssertsEnabled)
            {
                Debug.Fail("A formatting result was rejected because it was going to change non-whitespace content in the document.");
            }

            return SpecializedTasks.False;
        }

        return SpecializedTasks.True;
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal sealed class FormattingDiagnosticValidationPass(ILoggerFactory loggerFactory) : IFormattingValidationPass
{
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<FormattingDiagnosticValidationPass>();

    // Internal for testing.
    internal bool DebugAssertsEnabled { get; set; } = true;

    public async Task<bool> IsValidAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        var originalDiagnostics = context.CodeDocument.GetRequiredSyntaxTree().Diagnostics;

        var text = context.SourceText;
        var changedText = text.WithChanges(changes);
        var changedContext = await context.WithTextAsync(changedText, cancellationToken).ConfigureAwait(false);
        var changedDiagnostics = changedContext.CodeDocument.GetRequiredSyntaxTree().Diagnostics;

        // We want to ensure diagnostics didn't change, but since we're formatting things, its expected
        // that some of them might have moved around.
        // This is not 100% correct, as the formatting technically could still cause a compile error,
        // but only if it also fixes one at the same time, so its probably an edge case (if indeed it's
        // at all possible). Also worth noting the order has to be maintained in that case.
        if (!originalDiagnostics.SequenceEqual(changedDiagnostics, LocationIgnoringDiagnosticComparer.Instance))
        {
            _logger.LogWarning($"{SR.Format_operation_changed_diagnostics}");
            _logger.LogWarning($"{SR.Diagnostics_before}");
            foreach (var diagnostic in originalDiagnostics)
            {
                _logger.LogWarning($"{diagnostic}");
            }

            _logger.LogWarning($"{SR.Diagnostics_after}");
            foreach (var diagnostic in changedDiagnostics)
            {
                _logger.LogWarning($"{diagnostic}");
            }

            if (DebugAssertsEnabled)
            {
                Debug.Fail("A formatting result was rejected because the formatted text produced different diagnostics compared to the original text.");
            }

            return false;
        }

        return true;
    }

    private class LocationIgnoringDiagnosticComparer : IEqualityComparer<RazorDiagnostic>
    {
        public static IEqualityComparer<RazorDiagnostic> Instance = new LocationIgnoringDiagnosticComparer();

        public bool Equals(RazorDiagnostic? x, RazorDiagnostic? y)
            => x is not null &&
                y is not null &&
                x.Severity == y.Severity &&
                x.Id == y.Id;

        public int GetHashCode(RazorDiagnostic obj)
            => obj.GetHashCode();
    }
}

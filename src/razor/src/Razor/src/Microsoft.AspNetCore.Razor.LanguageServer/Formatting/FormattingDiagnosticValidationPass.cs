// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class FormattingDiagnosticValidationPass : FormattingPassBase
{
    private readonly ILogger _logger;

    public FormattingDiagnosticValidationPass(
        IRazorDocumentMappingService documentMappingService,
        IClientConnection clientConnection,
        IRazorLoggerFactory loggerFactory)
        : base(documentMappingService, clientConnection)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _logger = loggerFactory.CreateLogger<FormattingDiagnosticValidationPass>();
    }

    // We want this to run at the very end.
    public override int Order => DefaultOrder + 1000;

    public override bool IsValidationPass => true;

    // Internal for testing.
    internal bool DebugAssertsEnabled { get; set; } = true;

    public async override Task<FormattingResult> ExecuteAsync(FormattingContext context, FormattingResult result, CancellationToken cancellationToken)
    {
        if (result.Kind != RazorLanguageKind.Razor)
        {
            // We don't care about changes to projected documents here.
            return result;
        }

        var originalDiagnostics = context.CodeDocument.GetSyntaxTree().Diagnostics;

        var text = context.SourceText;
        var edits = result.Edits;
        var changes = edits.Select(e => e.ToTextChange(text));
        var changedText = text.WithChanges(changes);
        var changedContext = await context.WithTextAsync(changedText).ConfigureAwait(false);
        var changedDiagnostics = changedContext.CodeDocument.GetSyntaxTree().Diagnostics;

        // We want to ensure diagnostics didn't change, but since we're formatting things, its expected
        // that some of them might have moved around.
        // This is not 100% correct, as the formatting technically could still cause a compile error,
        // but only if it also fixes one at the same time, so its probably an edge case (if indeed it's
        // at all possible). Also worth noting the order has to be maintained in that case.
        if (!originalDiagnostics.SequenceEqual(changedDiagnostics, LocationIgnoringDiagnosticComparer.Instance))
        {
            // Yes, these log messages look weird, but this is how structured logging works. The first parameter, the "template" is not
            // supposed to change, or it causes lots of allocations. The second parameter is the "argument" which is supposed to change.
            // In our case, we never use structured logging such that the argument name is important, so we just use the same one, and
            // save some memory, and still log the expected values.
            _logger.LogWarning("{value}", SR.Format_operation_changed_diagnostics);
            _logger.LogWarning("{value}", SR.Diagnostics_before);
            foreach (var diagnostic in originalDiagnostics)
            {
                _logger.LogWarning("{value}", diagnostic);
            }

            _logger.LogWarning("{value}", SR.Diagnostics_after);
            foreach (var diagnostic in changedDiagnostics)
            {
                _logger.LogWarning("{value}", diagnostic);
            }

            if (DebugAssertsEnabled)
            {
                Debug.Fail("A formatting result was rejected because the formatted text produced different diagnostics compared to the original text.");
            }

            return new FormattingResult(Array.Empty<TextEdit>());
        }

        return result;
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

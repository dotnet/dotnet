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
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class FormattingDiagnosticValidationPass : FormattingPassBase
{
    private readonly ILogger _logger;

    public FormattingDiagnosticValidationPass(
        RazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase server,
        ILoggerFactory loggerFactory)
        : base(documentMappingService, server)
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
        var changes = edits.Select(e => e.AsTextChange(text));
        var changedText = text.WithChanges(changes);
        var changedContext = await context.WithTextAsync(changedText);
        var changedDiagnostics = changedContext.CodeDocument.GetSyntaxTree().Diagnostics;

        // We want to ensure diagnostics didn't change, but since we're formatting things, its expected
        // that some of them might have moved around.
        // This is not 100% correct, as the formatting technically could still cause a compile error,
        // but only if it also fixes one at the same time, so its probably an edge case (if indeed it's
        // at all possible). Also worth noting the order has to be maintained in that case.
        if (!originalDiagnostics.SequenceEqual(changedDiagnostics, LocationIgnoringDiagnosticComparer.Instance))
        {
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

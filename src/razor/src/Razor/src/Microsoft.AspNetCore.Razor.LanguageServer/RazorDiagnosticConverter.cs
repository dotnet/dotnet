// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal static class RazorDiagnosticConverter
{
    public static Diagnostic Convert(RazorDiagnostic razorDiagnostic, SourceText sourceText)
    {
        if (razorDiagnostic is null)
        {
            throw new ArgumentNullException(nameof(razorDiagnostic));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        var diagnostic = new Diagnostic()
        {
            Message = razorDiagnostic.GetMessage(CultureInfo.InvariantCulture),
            Code = razorDiagnostic.Id,
            Severity = ConvertSeverity(razorDiagnostic.Severity),
            // This is annotated as not null, but we have tests that validate the behaviour when
            // we pass in null here
            Range = ConvertSpanToRange(razorDiagnostic.Span, sourceText)!,
        };

        return diagnostic;
    }

    // Internal for testing
    internal static DiagnosticSeverity ConvertSeverity(RazorDiagnosticSeverity severity)
    {
        return severity switch
        {
            RazorDiagnosticSeverity.Error => DiagnosticSeverity.Error,
            RazorDiagnosticSeverity.Warning => DiagnosticSeverity.Warning,
            _ => DiagnosticSeverity.Information,
        };
    }

    // Internal for testing
    internal static Range? ConvertSpanToRange(SourceSpan sourceSpan, SourceText sourceText)
    {
        if (sourceSpan == SourceSpan.Undefined)
        {
            return null;
        }

        var spanStartIndex = NormalizeIndex(sourceSpan.AbsoluteIndex);
        var startPosition = sourceText.Lines.GetLinePosition(spanStartIndex);
        var start = new Position()
        {
            Line = startPosition.Line,
            Character = startPosition.Character,
        };

        var spanEndIndex = NormalizeIndex(sourceSpan.AbsoluteIndex + sourceSpan.Length);
        var endPosition = sourceText.Lines.GetLinePosition(spanEndIndex);
        var end = new Position()
        {
            Line = endPosition.Line,
            Character = endPosition.Character,
        };
        var range = new Range()
        {
            Start = start,
            End = end,
        };

        return range;

        int NormalizeIndex(int index)
        {
            if (index >= sourceText.Length)
            {
                // Span start index is past the end of the document. Roslyn and VSCode don't support
                // virtual positions that don't exist on the document; normalize to the last character.
                index = sourceText.Length - 1;
            }

            return Math.Max(index, 0);
        }
    }
}

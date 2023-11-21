// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

internal static class RazorDiagnosticConverter
{
    public static VSDiagnostic Convert(RazorDiagnostic razorDiagnostic, SourceText sourceText, IDocumentSnapshot? documentSnapshot)
    {
        if (razorDiagnostic is null)
        {
            throw new ArgumentNullException(nameof(razorDiagnostic));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        var projects = documentSnapshot is null
            ? Array.Empty<VSDiagnosticProjectInformation>()
            : [
                new VSDiagnosticProjectInformation()
                {
                    Context = null,
                    ProjectIdentifier = documentSnapshot.Project.Key.Id,
                    ProjectName = documentSnapshot.Project.DisplayName
                }
            ];

        var diagnostic = new VSDiagnostic()
        {
            Message = razorDiagnostic.GetMessage(CultureInfo.InvariantCulture),
            Code = razorDiagnostic.Id,
            Source = "Razor",
            Severity = ConvertSeverity(razorDiagnostic.Severity),
            // This is annotated as not null, but we have tests that validate the behaviour when
            // we pass in null here
            Range = ConvertSpanToRange(razorDiagnostic.Span, sourceText)!,
            Projects = projects
        };

        return diagnostic;
    }

    internal static Diagnostic[] Convert(IReadOnlyList<RazorDiagnostic> diagnostics, SourceText sourceText, IDocumentSnapshot documentSnapshot)
    {
        var convertedDiagnostics = new Diagnostic[diagnostics.Count];

        var i = 0;
        foreach (var diagnostic in diagnostics)
        {
            convertedDiagnostics[i++] = Convert(diagnostic, sourceText, documentSnapshot);
        }

        return convertedDiagnostics;
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

        var spanStartIndex = Math.Min(sourceSpan.AbsoluteIndex, sourceText.Length);
        var startPosition = sourceText.Lines.GetLinePosition(spanStartIndex);
        var start = new Position()
        {
            Line = startPosition.Line,
            Character = startPosition.Character,
        };

        var spanEndIndex = Math.Min(sourceSpan.AbsoluteIndex + sourceSpan.Length, sourceText.Length);
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
    }
}

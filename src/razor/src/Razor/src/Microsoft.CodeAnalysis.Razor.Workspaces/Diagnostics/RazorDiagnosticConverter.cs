// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Globalization;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using LspDiagnostic = Microsoft.VisualStudio.LanguageServer.Protocol.Diagnostic;
using LspDiagnosticSeverity = Microsoft.VisualStudio.LanguageServer.Protocol.DiagnosticSeverity;
using LspRange = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace Microsoft.CodeAnalysis.Razor.Diagnostics;

internal static class RazorDiagnosticConverter
{
    public static VSDiagnostic Convert(RazorDiagnostic razorDiagnostic, SourceText sourceText, IDocumentSnapshot? documentSnapshot)
    {
        var diagnostic = new VSDiagnostic()
        {
            Message = razorDiagnostic.GetMessage(CultureInfo.InvariantCulture),
            Code = razorDiagnostic.Id,
            Source = "Razor",
            Severity = ConvertSeverity(razorDiagnostic.Severity),
            // This is annotated as not null, but we have tests that validate the behaviour when
            // we pass in null here
            Range = ConvertSpanToRange(razorDiagnostic.Span, sourceText)!,
            Projects = GetProjectInformation(documentSnapshot)
        };

        return diagnostic;
    }

    public static VSDiagnosticProjectInformation[] GetProjectInformation(IDocumentSnapshot? documentSnapshot)
    {
        if (documentSnapshot is null)
        {
            return [];
        }

        return [new VSDiagnosticProjectInformation()
                {
                    Context = null,
                    ProjectIdentifier = documentSnapshot.Project.Key.Id,
                    ProjectName = documentSnapshot.Project.DisplayName
                }];
    }

    internal static LspDiagnostic[] Convert(ImmutableArray<RazorDiagnostic> diagnostics, SourceText sourceText, IDocumentSnapshot documentSnapshot)
    {
        var convertedDiagnostics = new LspDiagnostic[diagnostics.Length];

        var i = 0;
        foreach (var diagnostic in diagnostics)
        {
            convertedDiagnostics[i++] = Convert(diagnostic, sourceText, documentSnapshot);
        }

        return convertedDiagnostics;
    }

    // Internal for testing
    internal static LspDiagnosticSeverity ConvertSeverity(RazorDiagnosticSeverity severity)
    {
        return severity switch
        {
            RazorDiagnosticSeverity.Error => LspDiagnosticSeverity.Error,
            RazorDiagnosticSeverity.Warning => LspDiagnosticSeverity.Warning,
            _ => LspDiagnosticSeverity.Information,
        };
    }

    // Internal for testing
    internal static LspRange? ConvertSpanToRange(SourceSpan sourceSpan, SourceText sourceText)
    {
        if (sourceSpan == SourceSpan.Undefined)
        {
            return null;
        }

        var spanStartIndex = Math.Min(sourceSpan.AbsoluteIndex, sourceText.Length);
        var spanEndIndex = Math.Min(sourceSpan.AbsoluteIndex + sourceSpan.Length, sourceText.Length);

        return sourceText.GetRange(spanStartIndex, spanEndIndex);
    }
}

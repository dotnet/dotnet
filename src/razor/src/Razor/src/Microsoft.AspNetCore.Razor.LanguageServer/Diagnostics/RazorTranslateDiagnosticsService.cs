// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Diagnostic = Microsoft.VisualStudio.LanguageServer.Protocol.Diagnostic;
using DiagnosticSeverity = Microsoft.VisualStudio.LanguageServer.Protocol.DiagnosticSeverity;
using Position = Microsoft.VisualStudio.LanguageServer.Protocol.Position;
using RazorDiagnosticFactory = Microsoft.AspNetCore.Razor.Language.RazorDiagnosticFactory;
using SourceText = Microsoft.CodeAnalysis.Text.SourceText;
using SyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;
using TextSpan = Microsoft.AspNetCore.Razor.Language.Syntax.TextSpan;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

internal class RazorTranslateDiagnosticsService
{
    private readonly ILogger _logger;
    private readonly RazorDocumentMappingService _documentMappingService;

    // Internal for testing
    internal static readonly IReadOnlyCollection<string> CSharpDiagnosticsToIgnore = new HashSet<string>()
    {
        "RemoveUnnecessaryImportsFixable",
        "IDE0005_gen", // Using directive is unnecessary
    };

    public RazorTranslateDiagnosticsService(RazorDocumentMappingService documentMappingService, ILoggerFactory loggerFactory)
    {
        if (documentMappingService is null)
        {
            throw new ArgumentNullException(nameof(documentMappingService));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _documentMappingService = documentMappingService;
        _logger = loggerFactory.CreateLogger<RazorTranslateDiagnosticsService>();
    }

    internal async Task<Diagnostic[]> TranslateAsync(
        RazorLanguageKind diagnosticKind,
        Diagnostic[] diagnostics,
        DocumentContext documentContext,
        CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported() != false)
        {
            _logger.LogInformation("Unsupported code document.");
            return Array.Empty<Diagnostic>();
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken);

        var filteredDiagnostics = diagnosticKind == RazorLanguageKind.CSharp
        ? FilterCSharpDiagnostics(diagnostics, codeDocument, sourceText)
            : FilterHTMLDiagnostics(diagnostics, codeDocument, sourceText, _logger);
        if (!filteredDiagnostics.Any())
        {
            _logger.LogInformation("No diagnostics remaining after filtering.");

            return Array.Empty<Diagnostic>();
        }

        _logger.LogInformation("{filteredDiagnosticsLength}/{unmappedDiagnosticsLength} diagnostics remain after filtering.", filteredDiagnostics.Length, diagnostics.Length);

        var mappedDiagnostics = MapDiagnostics(
            diagnosticKind,
            filteredDiagnostics,
            codeDocument,
            sourceText);

        return mappedDiagnostics;
    }

    private Diagnostic[] FilterCSharpDiagnostics(Diagnostic[] unmappedDiagnostics, RazorCodeDocument codeDocument, SourceText sourceText)
    {
        return unmappedDiagnostics.Where(d =>
            !ShouldFilterCSharpDiagnosticBasedOnErrorCode(d, codeDocument, sourceText)).ToArray();
    }

    private static Diagnostic[] FilterHTMLDiagnostics(
        Diagnostic[] unmappedDiagnostics,
        RazorCodeDocument codeDocument,
        SourceText sourceText,
        ILogger logger)
    {
        var syntaxTree = codeDocument.GetSyntaxTree();

        var processedAttributes = new Dictionary<TextSpan, bool>();

        var filteredDiagnostics = unmappedDiagnostics
            .Where(d =>
                !InCSharpLiteral(d, sourceText, syntaxTree, logger) &&
                !InAttributeContainingCSharp(d, sourceText, syntaxTree, processedAttributes, logger) &&
                !AppliesToTagHelperTagName(d, sourceText, syntaxTree, logger) &&
                !ShouldFilterHtmlDiagnosticBasedOnErrorCode(d, sourceText, syntaxTree, logger))
            .ToArray();

        return filteredDiagnostics;
    }

    private Diagnostic[] MapDiagnostics(
        RazorLanguageKind languageKind,
        IReadOnlyList<Diagnostic> diagnostics,
        RazorCodeDocument codeDocument,
        SourceText sourceText)
    {
        if (languageKind != RazorLanguageKind.CSharp)
        {
            // All other non-C# requests map directly to where they are in the document.
            return diagnostics.ToArray();
        }

        var mappedDiagnostics = new List<Diagnostic>();

        for (var i = 0; i < diagnostics.Count; i++)
        {
            var diagnostic = diagnostics[i];

            if (!TryGetOriginalDiagnosticRange(diagnostic, codeDocument, sourceText, out var originalRange))
            {
                continue;
            }

            diagnostic.Range = originalRange;
            mappedDiagnostics.Add(diagnostic);
        }

        return mappedDiagnostics.ToArray();
    }

    private static bool InCSharpLiteral(
        Diagnostic d,
        SourceText sourceText,
        RazorSyntaxTree syntaxTree,
        ILogger logger)
    {
        if (d.Range is null)
        {
            return false;
        }

        var owner = syntaxTree.GetOwner(sourceText, d.Range.End, logger);
        if (owner is null)
        {
            return false;
        }

        var isCSharp = owner.Kind is SyntaxKind.CSharpExpressionLiteral or SyntaxKind.CSharpStatementLiteral or SyntaxKind.CSharpEphemeralTextLiteral;

        return isCSharp;
    }

    private static bool AppliesToTagHelperTagName(
        Diagnostic diagnostic,
        SourceText sourceText,
        RazorSyntaxTree syntaxTree,
        ILogger logger)
    {
        // Goal of this method is to filter diagnostics that touch TagHelper tag names. Reason being is TagHelpers can output anything. Meaning
        // If you have a TagHelper like:
        //
        // <Input>
        // </Input>
        //
        // HTML would see this as an error because the input element can't have a body; however, a TagHelper could respect this in a totally valid
        // way.

        if (diagnostic.Range is null)
        {
            return false;
        }

        var owner = syntaxTree.GetOwner(sourceText, diagnostic.Range.End, logger);

        var startOrEndTag = owner?.FirstAncestorOrSelf<RazorSyntaxNode>(n => n is MarkupTagHelperStartTagSyntax || n is MarkupTagHelperEndTagSyntax);
        if (startOrEndTag is null)
        {
            return false;
        }

        var tagName = startOrEndTag is MarkupTagHelperStartTagSyntax startTag ? startTag.Name : ((MarkupTagHelperEndTagSyntax)startOrEndTag).Name;
        var tagNameRange = tagName.GetRange(syntaxTree.Source);

        if (!tagNameRange.IntersectsOrTouches(diagnostic.Range))
        {
            // The diagnostic doesn't touch the tag name
            return false;
        }

        // Diagnostic is touching the start or end tag name range
        return true;
    }

    private static bool ShouldFilterHtmlDiagnosticBasedOnErrorCode(Diagnostic diagnostic, SourceText sourceText, RazorSyntaxTree syntaxTree, ILogger logger)
    {
        if (!diagnostic.Code.HasValue)
        {
            return false;
        }

        diagnostic.Code.Value.TryGetSecond(out var str);

        return str switch
        {
            CSSErrorCodes.MissingOpeningBrace => IsCSharpInStyleBlock(diagnostic, sourceText, syntaxTree, logger),
            CSSErrorCodes.MissingSelectorAfterCombinator => IsCSharpInStyleBlock(diagnostic, sourceText, syntaxTree, logger),
            CSSErrorCodes.MissingSelectorBeforeCombinatorCode => IsCSharpInStyleBlock(diagnostic, sourceText, syntaxTree, logger),
            HtmlErrorCodes.UnexpectedEndTagErrorCode => IsHtmlWithBangAndMatchingTags(diagnostic, sourceText, syntaxTree, logger),
            HtmlErrorCodes.InvalidNestingErrorCode => IsAnyFilteredInvalidNestingError(diagnostic, sourceText, syntaxTree, logger),
            HtmlErrorCodes.MissingEndTagErrorCode => FileKinds.IsComponent(syntaxTree.Options.FileKind), // Redundant with RZ9980 in Components
            HtmlErrorCodes.TooFewElementsErrorCode => IsAnyFilteredTooFewElementsError(diagnostic, sourceText, syntaxTree, logger),
            _ => false,
        };

        static bool IsCSharpInStyleBlock(Diagnostic diagnostic, SourceText sourceText, RazorSyntaxTree syntaxTree, ILogger logger)
        {
            // C# in a style block causes diagnostics because the HTML background document replaces C# with "~"
            var owner = syntaxTree.GetOwner(sourceText, diagnostic.Range.Start, logger);
            if (owner is null)
            {
                return false;
            }

            var element = owner.FirstAncestorOrSelf<MarkupElementSyntax>(n => n.StartTag?.Name.Content == "style");
            var csharp = owner.FirstAncestorOrSelf<CSharpCodeBlockSyntax>();

            return element.Body.Any(c => c is CSharpCodeBlockSyntax) || csharp is not null;
        }

        // Ideally this would be solved instead by not emitting the "!" at the HTML backing file,
        // but we don't currently have a system to accomplish that
        static bool IsAnyFilteredTooFewElementsError(Diagnostic diagnostic, SourceText sourceText, RazorSyntaxTree syntaxTree, ILogger logger)
        {
            var owner = syntaxTree.GetOwner(sourceText, diagnostic.Range.Start, logger);
            if (owner is null)
            {
                return false;
            }

            var element = owner.FirstAncestorOrSelf<MarkupElementSyntax>();

            if (element.StartTag?.Name.Content != "html")
            {
                return false;
            }

            var bodyElement = element
                .ChildNodes()
                .SingleOrDefault(c => c is MarkupElementSyntax tag && tag.StartTag?.Name.Content == "body") as MarkupElementSyntax;

            return bodyElement is not null &&
                   bodyElement.StartTag?.Bang is not null;
        }

        // Ideally this would be solved instead by not emitting the "!" at the HTML backing file,
        // but we don't currently have a system to accomplish that
        static bool IsHtmlWithBangAndMatchingTags(Diagnostic diagnostic, SourceText sourceText, RazorSyntaxTree syntaxTree, ILogger logger)
        {
            var owner = syntaxTree.GetOwner(sourceText, diagnostic.Range.Start, logger);
            if (owner is null)
            {
                return false;
            }

            var element = owner.FirstAncestorOrSelf<MarkupElementSyntax>();
            var startNode = element.StartTag;
            var endNode = element.EndTag;

            if (startNode is null || endNode is null)
            {
                // We only care about tags with a start and an end because we want to exclude diagnostics from their children
                return false;
            }

            var haveBang = startNode.Bang is not null && endNode.Bang is not null;
            var namesEquivilant = startNode.Name.Content == endNode.Name.Content;

            return haveBang && namesEquivilant;
        }

        static bool IsAnyFilteredInvalidNestingError(Diagnostic diagnostic, SourceText sourceText, RazorSyntaxTree syntaxTree, ILogger logger)
            => IsInvalidNestingWarningWithinComponent(diagnostic, sourceText, syntaxTree, logger) ||
               IsInvalidNestingFromBody(diagnostic, sourceText, syntaxTree, logger);

        static bool IsInvalidNestingWarningWithinComponent(Diagnostic diagnostic, SourceText sourceText, RazorSyntaxTree syntaxTree, ILogger logger)
        {
            var owner = syntaxTree.GetOwner(sourceText, diagnostic.Range.Start, logger);
            if (owner is null)
            {
                return false;
            }

            var taghelperNode = owner.FirstAncestorOrSelf<MarkupTagHelperElementSyntax>();

            return taghelperNode is not null;
        }

        // Ideally this would be solved instead by not emitting the "!" at the HTML backing file,
        // but we don't currently have a system to accomplish that
        static bool IsInvalidNestingFromBody(Diagnostic diagnostic, SourceText sourceText, RazorSyntaxTree syntaxTree, ILogger logger)
        {
            var owner = syntaxTree.GetOwner(sourceText, diagnostic.Range.Start, logger);
            if (owner is null)
            {
                return false;
            }

            var body = owner.FirstAncestorOrSelf<MarkupElementSyntax>(n => n.StartTag?.Name.Content.Equals("body", StringComparison.Ordinal) == true);

            if (ReferenceEquals(body, owner))
            {
                return false;
            }

            if (diagnostic.Message is null)
            {
                return false;
            }

            return diagnostic.Message.EndsWith("cannot be nested inside element 'html'.") && body.StartTag?.Bang is not null;
        }
    }

    private static bool InAttributeContainingCSharp(
        Diagnostic diagnostic,
        SourceText sourceText,
        RazorSyntaxTree syntaxTree,
        Dictionary<TextSpan, bool> processedAttributes,
        ILogger logger)
    {
        // Examine the _end_ of the diagnostic to see if we're at the
        // start of an (im/ex)plicit expression. Looking at the start
        // of the diagnostic isn't sufficient.
        if (diagnostic.Range is null)
        {
            return false;
        }

        var owner = syntaxTree.GetOwner(sourceText, diagnostic.Range.End, logger);
        if (owner is null)
        {
            return false;
        }

        var markupAttributeNode = owner.FirstAncestorOrSelf<RazorSyntaxNode>(n =>
            n is MarkupAttributeBlockSyntax ||
            n is MarkupTagHelperAttributeSyntax ||
            n is MarkupMiscAttributeContentSyntax);

        if (markupAttributeNode is not null)
        {
            if (!processedAttributes.TryGetValue(markupAttributeNode.FullSpan, out var doesAttributeContainNonMarkup))
            {
                doesAttributeContainNonMarkup = CheckIfAttributeContainsNonMarkupNodes(markupAttributeNode);
                processedAttributes.Add(markupAttributeNode.FullSpan, doesAttributeContainNonMarkup);
            }

            return doesAttributeContainNonMarkup;
        }

        return false;

        static bool CheckIfAttributeContainsNonMarkupNodes(RazorSyntaxNode attributeNode)
        {
            // Only allow markup, generic & (non-razor comment) token nodes
            var containsNonMarkupNodes = attributeNode.DescendantNodes()
                .Any(n => !(n is MarkupBlockSyntax ||
                    n is MarkupSyntaxNode ||
                    n is GenericBlockSyntax ||
                    (n is SyntaxNode sn && sn.IsToken && sn.Kind != SyntaxKind.RazorCommentTransition)));

            return containsNonMarkupNodes;
        }
    }

    private bool ShouldFilterCSharpDiagnosticBasedOnErrorCode(Diagnostic diagnostic, RazorCodeDocument codeDocument, SourceText sourceText)
    {
        if (!diagnostic.Code.HasValue)
        {
            return false;
        }

        diagnostic.Code.Value.TryGetSecond(out var str);

        return str switch
        {
            "CS1525" => ShouldIgnoreCS1525(diagnostic, codeDocument, sourceText),
            _ => CSharpDiagnosticsToIgnore.Contains(str) &&
                    diagnostic.Severity != DiagnosticSeverity.Error,
        };

        bool ShouldIgnoreCS1525(Diagnostic diagnostic, RazorCodeDocument codeDocument, SourceText sourceText)
        {
            if (CheckIfDocumentHasRazorDiagnostic(codeDocument, RazorDiagnosticFactory.TagHelper_EmptyBoundAttribute.Id) &&
                TryGetOriginalDiagnosticRange(diagnostic, codeDocument, sourceText, out var originalRange) &&
                originalRange.IsUndefined())
            {
                // Empty attribute values will take the following form in the generated C# document:
                // __o = Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.ProgressEventArgs>(this, );
                // The trailing `)` with no value preceding it, will lead to a C# error which doesn't make sense within the razor file.
                // The empty attribute value is not directly mappable to Razor, hence we check if the diagnostic has an undefined range.
                // Note; Error RZ2008 informs the user that the empty attribute value is not allowed.
                // https://github.com/dotnet/aspnetcore/issues/30480
                return true;
            }

            return false;
        }
    }

    // Internal & virtual for testing
    internal virtual bool CheckIfDocumentHasRazorDiagnostic(RazorCodeDocument codeDocument, string razorDiagnosticCode)
    {
        return codeDocument.GetSyntaxTree().Diagnostics.Any(d => d.Id.Equals(razorDiagnosticCode, StringComparison.Ordinal));
    }

    private bool TryGetOriginalDiagnosticRange(Diagnostic diagnostic, RazorCodeDocument codeDocument, SourceText sourceText, [NotNullWhen(true)] out Range? originalRange)
    {
        if (IsRudeEditDiagnostic(diagnostic))
        {
            if (TryRemapRudeEditRange(diagnostic.Range, codeDocument, sourceText, out originalRange))
            {
                return true;
            }

            return false;
        }

        if (!_documentMappingService.TryMapFromProjectedDocumentRange(
            codeDocument,
            diagnostic.Range,
            MappingBehavior.Inferred,
            out originalRange))
        {
            // Couldn't remap the range correctly.
            // If this isn't an `Error` Severity Diagnostic we can discard it.
            if (diagnostic.Severity != DiagnosticSeverity.Error)
            {
                return false;
            }

            // For `Error` Severity diagnostics we still show the diagnostics to
            // the user, however we set the range to an undefined range to ensure
            // clicking on the diagnostic doesn't cause errors.
            originalRange = RangeExtensions.UndefinedRange;
        }

        return true;
    }

    private static bool IsRudeEditDiagnostic(Diagnostic diagnostic)
    {
        return diagnostic.Code.HasValue &&
            diagnostic.Code.Value.TryGetSecond(out var str) &&
            str.StartsWith("ENC");
    }

    private bool TryRemapRudeEditRange(Range diagnosticRange, RazorCodeDocument codeDocument, SourceText sourceText, [NotNullWhen(true)] out Range? remappedRange)
    {
        // This is a rude edit diagnostic that has already been mapped to the Razor document. The mapping isn't absolutely correct though,
        // it's based on the runtime code generation of the Razor document therefore we need to re-map the already mapped diagnostic in a
        // semi-intelligent way.

        var syntaxTree = codeDocument.GetSyntaxTree();
        var owner = syntaxTree.GetOwner(sourceText, diagnosticRange, logger: _logger);

        switch (owner?.Kind)
        {
            case SyntaxKind.CSharpStatementLiteral: // Simple C# in @code block, @{ ... } etc.
            case SyntaxKind.CSharpExpressionLiteral: // Referenced simple C# in an implicit expression @Foo((abc) => {....})
                // Good as is, we were able to find a known leaf-node that fully contains the diagnostic range. Therefore we can
                // return the diagnostic range as is.
                remappedRange = diagnosticRange;
                return true;

            default:
                // Unsupported owner of rude diagnostic, lets map to the entirety of the diagnostic range to be sure the diagnostic can be presented

                _logger.LogInformation("Failed to remap rude edit for SyntaxTree owner '{ownerKind}'.", owner?.Kind);

                var startLineIndex = diagnosticRange.Start.Line;
                if (startLineIndex >= sourceText.Lines.Count)
                {
                    // Documents aren't sync'd we can't remap the ranges correctly, drop the diagnostic.
                    remappedRange = null;
                    return false;
                }

                var startLine = sourceText.Lines[startLineIndex];

                // Look for the first non-whitespace character so we're not squiggling random whitespace at the start of the diagnostic
                var firstNonWhitespaceCharacterOffset = sourceText.GetFirstNonWhitespaceOffset(startLine.Span, out _);
                var diagnosticStartCharacter = firstNonWhitespaceCharacterOffset ?? 0;
                var startLinePosition = new Position(startLineIndex, diagnosticStartCharacter);

                var endLineIndex = diagnosticRange.End.Line;
                if (endLineIndex >= sourceText.Lines.Count)
                {
                    // Documents aren't sync'd we can't remap the ranges correctly, drop the diagnostic.
                    remappedRange = null;
                    return false;
                }

                var endLine = sourceText.Lines[endLineIndex];

                // Look for the last non-whitespace character so we're not squiggling random whitespace at the end of the diagnostic
                var lastNonWhitespaceCharacterOffset = sourceText.GetLastNonWhitespaceOffset(endLine.Span, out _);
                var diagnosticEndCharacter = lastNonWhitespaceCharacterOffset ?? 0;
                var diagnosticEndWhitespaceOffset = diagnosticEndCharacter + 1;
                var endLinePosition = new Position(endLineIndex, diagnosticEndWhitespaceOffset);

                remappedRange = new Range
                {
                    Start = startLinePosition,
                    End = endLinePosition
                };
                return true;
        }
    }
}

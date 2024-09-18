// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal sealed class FormattingContext
{
    private readonly IFormattingCodeDocumentProvider _codeDocumentProvider;

    private IReadOnlyList<FormattingSpan>? _formattingSpans;
    private IReadOnlyDictionary<int, IndentationContext>? _indentations;

    private FormattingContext(
        IFormattingCodeDocumentProvider codeDocumentProvider,
        IDocumentSnapshot originalSnapshot,
        RazorCodeDocument codeDocument,
        RazorFormattingOptions options,
        bool automaticallyAddUsings,
        int hostDocumentIndex,
        char triggerCharacter)
    {
        _codeDocumentProvider = codeDocumentProvider;
        OriginalSnapshot = originalSnapshot;
        CodeDocument = codeDocument;
        Options = options;
        AutomaticallyAddUsings = automaticallyAddUsings;
        HostDocumentIndex = hostDocumentIndex;
        TriggerCharacter = triggerCharacter;
    }

    public static bool SkipValidateComponents { get; set; }

    public IDocumentSnapshot OriginalSnapshot { get; }
    public RazorCodeDocument CodeDocument { get; }
    public RazorFormattingOptions Options { get; }
    public bool AutomaticallyAddUsings { get; }
    public int HostDocumentIndex { get; }
    public char TriggerCharacter { get; }

    public SourceText SourceText => CodeDocument.Source.Text;

    public SourceText CSharpSourceText => CodeDocument.GetCSharpSourceText();

    public string NewLineString => Environment.NewLine;

    /// <summary>A Dictionary of int (line number) to IndentationContext.</summary>
    /// <remarks>
    /// Don't use this to discover the indentation level you should have, use
    /// <see cref="TryGetIndentationLevel(int, out int)"/> which operates on the position rather than just the line.
    /// </remarks>
    public IReadOnlyDictionary<int, IndentationContext> GetIndentations()
    {
        if (_indentations is null)
        {
            var sourceText = SourceText;
            var indentations = new Dictionary<int, IndentationContext>();

            var previousIndentationLevel = 0;
            for (var i = 0; i < sourceText.Lines.Count; i++)
            {
                var line = sourceText.Lines[i];
                // Get first non-whitespace character position
                var nonWsPos = line.GetFirstNonWhitespacePosition();
                var existingIndentation = (nonWsPos ?? line.End) - line.Start;

                // The existingIndentation above is measured in characters, and is used to create text edits
                // The below is measured in columns, so takes into account tab size. This is useful for creating
                // new indentation strings
                var existingIndentationSize = line.GetIndentationSize(Options.TabSize);

                var emptyOrWhitespaceLine = false;
                if (nonWsPos is null)
                {
                    emptyOrWhitespaceLine = true;
                    nonWsPos = line.Start;
                }

                // position now contains the first non-whitespace character or 0. Get the corresponding FormattingSpan.
                if (TryGetFormattingSpan(nonWsPos.Value, out var span))
                {
                    indentations[i] = new IndentationContext(firstSpan: span)
                    {
                        Line = i,
#if DEBUG
                        DebugOnly_LineText = line.ToString(),
#endif
                        RazorIndentationLevel = span.RazorIndentationLevel,
                        HtmlIndentationLevel = span.HtmlIndentationLevel,
                        RelativeIndentationLevel = span.IndentationLevel - previousIndentationLevel,
                        ExistingIndentation = existingIndentation,
                        ExistingIndentationSize = existingIndentationSize,
                        EmptyOrWhitespaceLine = emptyOrWhitespaceLine,
                    };
                    previousIndentationLevel = span.IndentationLevel;
                }
                else
                {
                    // Couldn't find a corresponding FormattingSpan. Happens if it is a 0 length line.
                    // Let's create a 0 length span to represent this and default it to HTML.
                    var placeholderSpan = new FormattingSpan(
                        new TextSpan(nonWsPos.Value, 0),
                        new TextSpan(nonWsPos.Value, 0),
                        FormattingSpanKind.Markup,
                        FormattingBlockKind.Markup,
                        razorIndentationLevel: 0,
                        htmlIndentationLevel: 0,
                        isInGlobalNamespace: false,
                        isInClassBody: false,
                        componentLambdaNestingLevel: 0);

                    indentations[i] = new IndentationContext(firstSpan: placeholderSpan)
                    {
                        Line = i,
#if DEBUG
                        DebugOnly_LineText = line.ToString(),
#endif
                        RazorIndentationLevel = 0,
                        HtmlIndentationLevel = 0,
                        RelativeIndentationLevel = previousIndentationLevel,
                        ExistingIndentation = existingIndentation,
                        ExistingIndentationSize = existingIndentation,
                        EmptyOrWhitespaceLine = emptyOrWhitespaceLine,
                    };
                }
            }

            _indentations = indentations;
        }

        return _indentations;
    }

    private IReadOnlyList<FormattingSpan> GetFormattingSpans()
    {
        if (_formattingSpans is null)
        {
            var syntaxTree = CodeDocument.GetSyntaxTree();
            var inGlobalNamespace = CodeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace) &&
                string.IsNullOrEmpty(@namespace);
            _formattingSpans = GetFormattingSpans(syntaxTree, inGlobalNamespace: inGlobalNamespace);
        }

        return _formattingSpans;
    }

    private static IReadOnlyList<FormattingSpan> GetFormattingSpans(RazorSyntaxTree syntaxTree, bool inGlobalNamespace)
    {
        var visitor = new FormattingVisitor(inGlobalNamespace: inGlobalNamespace);
        visitor.Visit(syntaxTree.Root);

        return visitor.FormattingSpans;
    }

    /// <summary>
    /// Generates a string of indentation based on a specific indentation level. For instance, inside of a C# method represents 1 indentation level. A method within a class would have indentaiton level of 2 by default etc.
    /// </summary>
    /// <param name="indentationLevel">The indentation level to represent</param>
    /// <returns>A whitespace string representing the indentation level based on the configuration.</returns>
    public string GetIndentationLevelString(int indentationLevel)
    {
        var indentation = GetIndentationOffsetForLevel(indentationLevel);
        var indentationString = FormattingUtilities.GetIndentationString(indentation, Options.InsertSpaces, Options.TabSize);
        return indentationString;
    }

    /// <summary>
    /// Given a level, returns the corresponding offset.
    /// </summary>
    /// <param name="level">A value representing the indentation level.</param>
    /// <returns></returns>
    public int GetIndentationOffsetForLevel(int level)
    {
        return level * Options.TabSize;
    }

    public bool TryGetIndentationLevel(int position, out int indentationLevel)
    {
        if (TryGetFormattingSpan(position, out var span))
        {
            indentationLevel = span.IndentationLevel;
            return true;
        }

        indentationLevel = 0;
        return false;
    }

    public bool TryGetFormattingSpan(int absoluteIndex, [NotNullWhen(true)] out FormattingSpan? result)
    {
        result = null;
        var formattingSpans = GetFormattingSpans();
        foreach (var formattingSpan in formattingSpans.AsEnumerable())
        {
            var span = formattingSpan.Span;

            if (span.Start <= absoluteIndex && span.End >= absoluteIndex)
            {
                if (span.End == absoluteIndex && span.Length > 0)
                {
                    // We're at an edge.
                    // Non-marker spans (spans.length == 0) do not own the edges after it
                    continue;
                }

                result = formattingSpan;
                return true;
            }
        }

        return false;
    }

    public async Task<FormattingContext> WithTextAsync(SourceText changedText)
    {
        var changedSnapshot = OriginalSnapshot.WithText(changedText);

        var codeDocument = await _codeDocumentProvider.GetCodeDocumentAsync(changedSnapshot).ConfigureAwait(false);

        DEBUG_ValidateComponents(CodeDocument, codeDocument);

        var newContext = new FormattingContext(
            _codeDocumentProvider,
            OriginalSnapshot,
            codeDocument,
            Options,
            AutomaticallyAddUsings,
            HostDocumentIndex,
            TriggerCharacter);

        return newContext;
    }

    /// <summary>
    /// It can be difficult in the testing infrastructure to correct constructs input files that work consistently across
    /// context changes, so this method validates that the number of components isn't changing due to lost tag help info.
    /// Without this guarantee its hard to reason about test behaviour/failures.
    /// </summary>
    [Conditional("DEBUG")]
    private static void DEBUG_ValidateComponents(RazorCodeDocument oldCodeDocument, RazorCodeDocument newCodeDocument)
    {
        if (SkipValidateComponents)
        {
            return;
        }

        var oldTagHelperElements = oldCodeDocument.GetSyntaxTree().Root.DescendantNodesAndSelf().OfType<MarkupTagHelperElementSyntax>().Count();
        var newTagHelperElements = newCodeDocument.GetSyntaxTree().Root.DescendantNodesAndSelf().OfType<MarkupTagHelperElementSyntax>().Count();
        Debug.Assert(oldTagHelperElements == newTagHelperElements, $"Previous context had {oldTagHelperElements} components, new only has {newTagHelperElements}.");
    }

    public static FormattingContext CreateForOnTypeFormatting(
        IDocumentSnapshot originalSnapshot,
        RazorCodeDocument codeDocument,
        RazorFormattingOptions options,
        IFormattingCodeDocumentProvider codeDocumentProvider,
        bool automaticallyAddUsings,
        int hostDocumentIndex,
        char triggerCharacter)
    {
        return new FormattingContext(
            codeDocumentProvider,
            originalSnapshot,
            codeDocument,
            options,
            automaticallyAddUsings,
            hostDocumentIndex,
            triggerCharacter);
    }

    public static FormattingContext Create(
        IDocumentSnapshot originalSnapshot,
        RazorCodeDocument codeDocument,
        RazorFormattingOptions options,
        IFormattingCodeDocumentProvider codeDocumentProvider)
    {
        return new FormattingContext(
            codeDocumentProvider,
            originalSnapshot,
            codeDocument,
            options,
            automaticallyAddUsings: false,
            hostDocumentIndex: 0,
            triggerCharacter: '\0'
       );
    }
}

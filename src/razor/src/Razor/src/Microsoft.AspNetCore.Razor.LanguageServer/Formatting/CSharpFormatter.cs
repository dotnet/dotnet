// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class CSharpFormatter
{
    private const string MarkerId = "RazorMarker";

    private readonly IRazorDocumentMappingService _documentMappingService;
    private readonly ClientNotifierServiceBase _server;

    public CSharpFormatter(
        IRazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer)
    {
        if (documentMappingService is null)
        {
            throw new ArgumentNullException(nameof(documentMappingService));
        }

        if (languageServer is null)
        {
            throw new ArgumentNullException(nameof(languageServer));
        }

        _documentMappingService = documentMappingService;
        _server = languageServer;
    }

    public async Task<TextEdit[]> FormatAsync(FormattingContext context, Range rangeToFormat, CancellationToken cancellationToken)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (rangeToFormat is null)
        {
            throw new ArgumentNullException(nameof(rangeToFormat));
        }

        if (!_documentMappingService.TryMapToGeneratedDocumentRange(context.CodeDocument.GetCSharpDocument(), rangeToFormat, out var projectedRange))
        {
            return Array.Empty<TextEdit>();
        }

        var edits = await GetFormattingEditsAsync(context, projectedRange, cancellationToken).ConfigureAwait(false);
        var mappedEdits = MapEditsToHostDocument(context.CodeDocument, edits);
        return mappedEdits;
    }

    public static async Task<IReadOnlyDictionary<int, int>> GetCSharpIndentationAsync(
        FormattingContext context,
        IReadOnlyCollection<int> projectedDocumentLocations,
        CancellationToken cancellationToken)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (projectedDocumentLocations is null)
        {
            throw new ArgumentNullException(nameof(projectedDocumentLocations));
        }

        // Sorting ensures we count the marker offsets correctly.
        // We also want to ensure there are no duplicates to avoid duplicate markers.
        var filteredLocations = projectedDocumentLocations.Distinct().OrderBy(l => l).ToList();

        var indentations = await GetCSharpIndentationCoreAsync(context, filteredLocations, cancellationToken).ConfigureAwait(false);
        return indentations;
    }

    private TextEdit[] MapEditsToHostDocument(RazorCodeDocument codeDocument, TextEdit[] csharpEdits)
    {
        var actualEdits = _documentMappingService.GetHostDocumentEdits(codeDocument.GetCSharpDocument(), csharpEdits);

        return actualEdits;
    }

    private static async Task<TextEdit[]> GetFormattingEditsAsync(FormattingContext context, Range projectedRange, CancellationToken cancellationToken)
    {
        var csharpSourceText = context.CodeDocument.GetCSharpSourceText();
        var spanToFormat = projectedRange.ToTextSpan(csharpSourceText);
        var root = await context.CSharpWorkspaceDocument.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        Assumes.NotNull(root);

        var workspace = context.CSharpWorkspace;

        // Formatting options will already be set in the workspace.
        var changes = CodeAnalysis.Formatting.Formatter.GetFormattedTextChanges(root, spanToFormat, workspace, cancellationToken: cancellationToken);

        var edits = changes.Select(c => c.ToTextEdit(csharpSourceText)).ToArray();
        return edits;
    }

    private static async Task<Dictionary<int, int>> GetCSharpIndentationCoreAsync(FormattingContext context, List<int> projectedDocumentLocations, CancellationToken cancellationToken)
    {
        // No point calling the C# formatting if we won't be interested in any of its work anyway
        if (projectedDocumentLocations.Count == 0)
        {
            return new Dictionary<int, int>();
        }

        var (indentationMap, syntaxTree) = InitializeIndentationData(context, projectedDocumentLocations, cancellationToken);

        var root = await syntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);

        root = AttachAnnotations(indentationMap, projectedDocumentLocations, root);

        // At this point, we have added all the necessary markers and attached annotations.
        // Let's invoke the C# formatter and hope for the best.
        var formattedRoot = CodeAnalysis.Formatting.Formatter.Format(root, context.CSharpWorkspace, cancellationToken: cancellationToken);
        var formattedText = formattedRoot.GetText();

        var desiredIndentationMap = new Dictionary<int, int>();

        // Assuming the C# formatter did the right thing, let's extract the indentation offset from
        // the line containing trivia and token that has our attached annotations.
        ExtractTriviaAnnotations(context, formattedRoot, formattedText, desiredIndentationMap);
        ExtractTokenAnnotations(context, formattedRoot, formattedText, indentationMap, desiredIndentationMap);

        return desiredIndentationMap;

        static void ExtractTriviaAnnotations(
            FormattingContext context,
            SyntaxNode formattedRoot,
            SourceText formattedText,
            Dictionary<int, int> desiredIndentationMap)
        {
            var formattedTriviaList = formattedRoot.GetAnnotatedTrivia(MarkerId);
            foreach (var trivia in formattedTriviaList)
            {
                // We only expect one annotation because we built the entire trivia with a single annotation.
                var annotation = trivia.GetAnnotations(MarkerId).Single();
                if (!int.TryParse(annotation.Data, out var projectedIndex))
                {
                    // This shouldn't happen realistically unless someone messed with the annotations we added.
                    // Let's ignore this annotation.
                    continue;
                }

                var line = formattedText.Lines.GetLineFromPosition(trivia.SpanStart);
                var offset = GetIndentationOffsetFromLine(context, line);

                desiredIndentationMap[projectedIndex] = offset;
            }
        }

        static void ExtractTokenAnnotations(
            FormattingContext context,
            SyntaxNode formattedRoot,
            SourceText formattedText,
            Dictionary<int, IndentationMapData> indentationMap,
            Dictionary<int, int> desiredIndentationMap)
        {
            var formattedTokenList = formattedRoot.GetAnnotatedTokens(MarkerId);
            foreach (var token in formattedTokenList)
            {
                // There could be multiple annotations per token because a token can span multiple lines.
                // E.g, a multiline string literal.
                var annotations = token.GetAnnotations(MarkerId);
                foreach (var annotation in annotations)
                {
                    if (!int.TryParse(annotation.Data, out var projectedIndex))
                    {
                        // This shouldn't happen realistically unless someone messed with the annotations we added.
                        // Let's ignore this annotation.
                        continue;
                    }

                    var indentationMapData = indentationMap[projectedIndex];
                    var line = formattedText.Lines.GetLineFromPosition(token.SpanStart + indentationMapData.CharacterOffset);
                    var offset = GetIndentationOffsetFromLine(context, line);

                    // Every bit of C# in a Razor file is assumed to be indented by at least 2 levels (namespace and class)
                    // and the Razor formatter works based on that assumption. For some specific C# nodes however, the C# formatter
                    // will not indent them at all. When they happen to be indented more than 2 levels this causes a problem
                    // because we essentially assume that we should always move them left by at least 2 levels. This means that these
                    // nodes end up moving left with every format operation, until they hit the minimum of 2 indent levels.
                    // We can't fix this, so we just work around it by ignoring those lines compeletely, and leaving them where the
                    // user put them.

                    if (ShouldIgnoreLineCompletely(token, formattedText))
                    {
                        offset = -1;
                    }

                    desiredIndentationMap[projectedIndex] = offset;
                }
            }
        }
    }

    private static bool ShouldIgnoreLineCompletely(SyntaxToken token, SourceText text)
    {
        return ShouldIgnoreLineCompletelyBecauseOfNode(token.Parent, text)
            || ShouldIgnoreLineCompletelyBecauseOfAncestors(token, text);

        static bool ShouldIgnoreLineCompletelyBecauseOfNode(SyntaxNode? node, SourceText text)
        {
            return node switch
            {
                // We don't want to format lines that are part of multi-line string literals
                LiteralExpressionSyntax { RawKind: (int)CodeAnalysis.CSharp.SyntaxKind.StringLiteralExpression } => SpansMultipleLines(node, text),
                // As above, but for mutli-line interpolated strings
                InterpolatedStringExpressionSyntax => SpansMultipleLines(node, text),
                InterpolatedStringTextSyntax => SpansMultipleLines(node, text),
                _ => false
            };
        }

        static bool ShouldIgnoreLineCompletelyBecauseOfAncestors(SyntaxToken token, SourceText text)
        {
            var parent = token.Parent;
            if (parent is null)
            {
                return false;
            }

            // When directly in an implicit object creation expression, it seems the C# formatter
            // does format the braces of an array initializer, so we need to special case those
            // node types. Doing it outside the loop is good for perf, but also makes things easier.
            if (parent is InitializerExpressionSyntax initializer &&
                initializer.IsKind(CodeAnalysis.CSharp.SyntaxKind.ArrayInitializerExpression) &&
                (token == initializer.OpenBraceToken || token == initializer.CloseBraceToken) &&
                initializer.Parent?.Parent?.Parent?.Parent is ImplicitObjectCreationExpressionSyntax)
            {
                return false;
            }

            return parent.AncestorsAndSelf().Any(node =>
            {
                if (node is not InitializerExpressionSyntax initializer)
                {
                    return false;
                }

                if (initializer.IsKind(CodeAnalysis.CSharp.SyntaxKind.ArrayInitializerExpression))
                {
                    // For array initializers we don't want to ignore the open and close braces
                    // as the formatter does move them relative to the variable declaration they
                    // are part of, but doesn't otherwise touch them.
                    // This isn't true if they are part of other collection or object initializers, but
                    // fortunately we can ignore that because of the recursive nature of this method,
                    // I just wanted to mention it so you understood how annoying this is :)
                    // This also isn't true for the close brace token of an _implicit_ array creation
                    // expression, because Roslyn was designed to hurt me.
                    if (token == initializer.OpenBraceToken ||
                        (token == initializer.CloseBraceToken && initializer.Parent is not ImplicitArrayCreationExpressionSyntax))
                    {
                        return false;
                    }

                    // Anything else in an array initializer we ignore
                    return true;
                }

                // Any other type of initializer, as long as its not empty, we also ignore
                if (initializer.Expressions.Count > 0)
                {
                    return true;
                }

                return false;
            });
        }

        static bool SpansMultipleLines(SyntaxNode node, SourceText text)
        {
            var range = node.Span.ToRange(text);
            return range.Start.Line != range.End.Line;
        }
    }

    private static (Dictionary<int, IndentationMapData>, SyntaxTree) InitializeIndentationData(
        FormattingContext context,
        IEnumerable<int> projectedDocumentLocations,
        CancellationToken cancellationToken)
    {
        // The approach we're taking here is to add markers only when absolutely necessary.
        // We'll attach annotations to tokens directly when possible.

        var indentationMap = new Dictionary<int, IndentationMapData>();
        var marker = "/*__marker__*/";
        var markerString = $"{context.NewLineString}{marker}{context.NewLineString}";
        var changes = new List<TextChange>();

        var previousMarkerOffset = 0;
        foreach (var projectedDocumentIndex in projectedDocumentLocations)
        {
            var useMarker = char.IsWhiteSpace(context.CSharpSourceText[projectedDocumentIndex]);
            if (useMarker)
            {
                // We want to add a marker here because the location points to a whitespace
                // which will not get preserved during formatting.

                // position points to the start of the /*__marker__*/ comment.
                var position = projectedDocumentIndex + context.NewLineString.Length;
                var change = new TextChange(new TextSpan(projectedDocumentIndex, 0), markerString);
                changes.Add(change);

                indentationMap.Add(projectedDocumentIndex, new IndentationMapData()
                {
                    OriginalProjectedDocumentIndex = projectedDocumentIndex,
                    AnnotationAttachIndex = position + previousMarkerOffset,
                    MarkerKind = MarkerKind.Trivia,
                });

                // We have added a marker. This means we need to account for the length of the marker in future calculations.
                previousMarkerOffset += markerString.Length;
            }
            else
            {
                // No marker needed. Let's attach the annotation directly at the given location.
                indentationMap.Add(projectedDocumentIndex, new IndentationMapData()
                {
                    OriginalProjectedDocumentIndex = projectedDocumentIndex,
                    AnnotationAttachIndex = projectedDocumentIndex + previousMarkerOffset,
                    MarkerKind = MarkerKind.Token,
                });
            }
        }

        var changedText = context.CSharpSourceText.WithChanges(changes);
        var syntaxTree = CSharpSyntaxTree.ParseText(changedText, cancellationToken: cancellationToken);
        return (indentationMap, syntaxTree);
    }

    private static SyntaxNode AttachAnnotations(
        Dictionary<int, IndentationMapData> indentationMap,
        IEnumerable<int> projectedDocumentLocations,
        SyntaxNode root)
    {
        foreach (var projectedDocumentIndex in projectedDocumentLocations)
        {
            var indentationMapData = indentationMap[projectedDocumentIndex];
            var annotation = new SyntaxAnnotation(MarkerId, $"{projectedDocumentIndex}");

            if (indentationMapData.MarkerKind == MarkerKind.Trivia)
            {
                var trackingTrivia = root.FindTrivia(indentationMapData.AnnotationAttachIndex, findInsideTrivia: true);
                var annotatedTrivia = trackingTrivia.WithAdditionalAnnotations(annotation);
                root = root.ReplaceTrivia(trackingTrivia, annotatedTrivia);
            }
            else
            {
                var trackingToken = root.FindToken(indentationMapData.AnnotationAttachIndex, findInsideTrivia: true);
                var annotatedToken = trackingToken.WithAdditionalAnnotations(annotation);
                root = root.ReplaceToken(trackingToken, annotatedToken);

                // Since a token can span multiple lines, we need to keep track of the offset within the token span.
                // We will use this later when determining the exact line within a token in cases like a multiline string literal.
                indentationMapData.CharacterOffset = indentationMapData.AnnotationAttachIndex - trackingToken.SpanStart;
            }
        }

        return root;
    }

    private static int GetIndentationOffsetFromLine(FormattingContext context, TextLine line)
    {
        var offset = line.GetFirstNonWhitespaceOffset() ?? 0;
        if (!context.Options.InsertSpaces)
        {
            // Normalize to spaces because the rest of the formatting pipeline operates based on the assumption.
            offset *= (int)context.Options.TabSize;
        }

        return offset;
    }

    private class IndentationMapData
    {
        public int OriginalProjectedDocumentIndex { get; set; }

        public int AnnotationAttachIndex { get; set; }

        public int CharacterOffset { get; set; }

        public MarkerKind MarkerKind { get; set; }

        public override string ToString()
        {
            return $"Original: {OriginalProjectedDocumentIndex}, MarkerAdjusted: {AnnotationAttachIndex}, Kind: {MarkerKind}, TokenOffset: {CharacterOffset}";
        }
    }

    private enum MarkerKind
    {
        Trivia,
        Token
    }
}

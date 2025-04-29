// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using RazorRazorSyntaxNodeList = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxList<Microsoft.AspNetCore.Razor.Language.Syntax.RazorSyntaxNode>;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;
using RazorSyntaxNodeList = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxList<Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode>;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal sealed class RazorFormattingPass(LanguageServerFeatureOptions languageServerFeatureOptions, ILoggerFactory loggerFactory) : IFormattingPass
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorFormattingPass>();

    public async Task<ImmutableArray<TextChange>> ExecuteAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        // Apply previous edits if any.
        var originalText = context.SourceText;
        var changedText = originalText;
        var changedContext = context;
        if (changes.Length > 0)
        {
            changedText = changedText.WithChanges(changes);
            changedContext = await context.WithTextAsync(changedText, cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();
        }

        // Format the razor bits of the file
        var syntaxTree = changedContext.CodeDocument.GetSyntaxTree();
        var razorChanges = FormatRazor(changedContext, syntaxTree);

        if (razorChanges.Length > 0)
        {
            // Compute the final combined set of edits
            changedText = changedText.WithChanges(razorChanges);
            _logger.LogTestOnly($"After RazorFormattingPass:\r\n{changedText}");
        }

        return changedText.GetTextChangesArray(originalText);
    }

    private ImmutableArray<TextChange> FormatRazor(FormattingContext context, RazorSyntaxTree syntaxTree)
    {
        using var changes = new PooledArrayBuilder<TextChange>();
        var source = syntaxTree.Source;

        foreach (var node in syntaxTree.Root.DescendantNodes())
        {
            // Disclaimer: CSharpCodeBlockSyntax is used a _lot_ in razor so these methods are probably
            // being overly careful to only try to format syntax forms they care about.
            TryFormatCSharpBlockStructure(context, ref changes.AsRef(), source, node);
            TryFormatSingleLineDirective(ref changes.AsRef(), node);
            TryFormatBlocks(context, ref changes.AsRef(), source, node);
        }

        return changes.ToImmutable();
    }

    private void TryFormatBlocks(FormattingContext context, ref PooledArrayBuilder<TextChange> changes, RazorSourceDocument source, RazorSyntaxNode node)
    {
        // We only want to run one of these
        _ = TryFormatFunctionsBlock(context, ref changes, source, node) ||
            TryFormatCSharpExplicitTransition(context, ref changes, source, node) ||
            TryFormatHtmlInCSharp(context, ref changes, source, node) ||
            TryFormatComplexCSharpBlock(context, ref changes, source, node) ||
            TryFormatSectionBlock(context, ref changes, source, node);
    }

    private bool TryFormatSectionBlock(FormattingContext context, ref PooledArrayBuilder<TextChange> changes, RazorSourceDocument source, RazorSyntaxNode node)
    {
        // @section Goo {
        // }
        //
        // or
        //
        // @section Goo
        // {
        // }
        if (node is CSharpCodeBlockSyntax directiveCode &&
            directiveCode.Children is [RazorDirectiveSyntax directive, ..] &&
            directive.DirectiveDescriptor?.Directive == SectionDirective.Directive.Directive &&
            directive.Body is RazorDirectiveBodySyntax { CSharpCode: { Children: var children } })
        {
            // This doesn't cause any harm with the new engine, but its a waste of effort.
            if (!_languageServerFeatureOptions.UseNewFormattingEngine)
            {
                // Section directives are really annoying in their implementation, and we have some code in the C# formatting pass
                // to work around those annoyances, but if the section content has no C# mappings then that code won't get hit.
                // Fortunately for a Html-only section block, the indentation is entirely handled by the Html formatter, and we
                // just need to push it out one level, because the Html formatter will have pushed it back to position 0.
                if (children is [.., MarkupBlockSyntax block, RazorMetaCodeSyntax /* close brace */] &&
                    !context.CodeDocument.GetCSharpDocument().SourceMappings.Any(m => block.Span.Contains(m.OriginalSpan.AbsoluteIndex)))
                {
                    // The Html formatter will have "collapsed" the @section block contents to 0 indent, so we push it back out
                    // again because we're opinionated about section blocks
                    var indentationString = context.GetIndentationLevelString(1);
                    var sourceText = context.CodeDocument.Source.Text;
                    var span = sourceText.GetLinePositionSpan(block.Span);
                    // The block starts with the newline after the open brace, so we start from the next line
                    for (var i = span.Start.Line + 1; i < span.End.Line; i++)
                    {
                        changes.Add(new TextChange(new TextSpan(sourceText.Lines[i].Start, 0), indentationString));
                    }
                }
            }

            if (TryGetWhitespace(children, out var whitespaceBeforeSectionName, out var whitespaceAfterSectionName))
            {
                // For whitespace we normalize it differently depending on if its multi-line or not
                FormatWhitespaceBetweenDirectiveAndBrace(whitespaceBeforeSectionName, directive, ref changes, source, context, forceNewLine: false);
                FormatWhitespaceBetweenDirectiveAndBrace(whitespaceAfterSectionName, directive, ref changes, source, context, forceNewLine: false);

                return true;
            }
            else if (children.TryGetOpenBraceToken(out var brace))
            {
                // If there is no whitespace at all we normalize to a single space
                changes.Add(new TextChange(new TextSpan(brace.SpanStart, 0), " "));

                return true;
            }
        }

        return false;

        static bool TryGetWhitespace(RazorRazorSyntaxNodeList children, [NotNullWhen(true)] out CSharpStatementLiteralSyntax? whitespaceBeforeSectionName, [NotNullWhen(true)] out UnclassifiedTextLiteralSyntax? whitespaceAfterSectionName)
        {
            // If there is whitespace between the directive and the section name, and the section name and the brace, they will be in the first child
            // and third child of the 6 total children
            whitespaceBeforeSectionName = null;
            whitespaceAfterSectionName = null;
            if (children.Count == 6 &&
                children[0] is CSharpStatementLiteralSyntax before &&
                before.ContainsOnlyWhitespace() &&
                children[2] is UnclassifiedTextLiteralSyntax after &&
                after.ContainsOnlyWhitespace())
            {
                whitespaceBeforeSectionName = before;
                whitespaceAfterSectionName = after;

            }

            return whitespaceBeforeSectionName != null;
        }
    }

    private bool TryFormatFunctionsBlock(FormattingContext context, ref PooledArrayBuilder<TextChange> changes, RazorSourceDocument source, RazorSyntaxNode node)
    {
        // @functions
        // {
        // }
        //
        // or
        //
        // @code
        // {
        // }

        // In design time code gen, there is only one child of a node like this, but at runtime any leading whitespace is included
        // as a child, so we handle both cases by just checking the last child.
        if (node is CSharpCodeBlockSyntax { Children: [.., RazorDirectiveSyntax { Body: RazorDirectiveBodySyntax body } directive] })
        {
            if (!IsCodeOrFunctionsBlock(body.Keyword))
            {
                return false;
            }

            var csharpCodeChildren = body.CSharpCode.Children;
            if (!csharpCodeChildren.TryGetOpenBraceNode(out var openBrace) ||
                !csharpCodeChildren.TryGetCloseBraceNode(out var closeBrace))
            {
                // Don't trust ourselves in an incomplete scenario.
                return false;
            }

            var code = csharpCodeChildren.PreviousSiblingOrSelf(closeBrace) as CSharpCodeBlockSyntax;

            var openBraceNode = openBrace;
            var codeNode = code.AssumeNotNull();
            var closeBraceNode = closeBrace;

            return FormatBlock(context, source, directive, openBraceNode, codeNode, closeBraceNode, ref changes);
        }

        return false;
    }

    private bool TryFormatCSharpExplicitTransition(FormattingContext context, ref PooledArrayBuilder<TextChange> changes, RazorSourceDocument source, RazorSyntaxNode node)
    {
        // We're looking for a code block like this:
        //
        // @{
        //     var x = 1;
        // }
        // Using LastOrDefault because runtime code-gen puts whitespace before the statement
        if (node is CSharpCodeBlockSyntax explicitCode &&
            explicitCode.Children.LastOrDefault() is CSharpStatementSyntax statement &&
            statement.Body is CSharpStatementBodySyntax csharpStatementBody)
        {
            var openBraceNode = csharpStatementBody.OpenBrace;
            var codeNode = csharpStatementBody.CSharpCode;
            var closeBraceNode = csharpStatementBody.CloseBrace;

            return FormatBlock(context, source, directiveNode: null, openBraceNode, codeNode, closeBraceNode, ref changes);
        }

        return false;
    }

    private bool TryFormatComplexCSharpBlock(FormattingContext context, ref PooledArrayBuilder<TextChange> changes, RazorSourceDocument source, RazorSyntaxNode node)
    {
        // complex situations like
        // @{
        //  void Method(){
        //      @(DateTime.Now)
        //  }
        // }
        if (node is CSharpRazorBlockSyntax csharpRazorBlock &&
            csharpRazorBlock.Parent is CSharpCodeBlockSyntax innerCodeBlock &&
            csharpRazorBlock.Parent.Parent is CSharpCodeBlockSyntax outerCodeBlock)
        {
            var codeNode = csharpRazorBlock;
            var openBraceNode = outerCodeBlock.Children.PreviousSiblingOrSelf(innerCodeBlock);
            var closeBraceNode = outerCodeBlock.Children.NextSiblingOrSelf(innerCodeBlock);

            return FormatBlock(context, source, directiveNode: null, openBraceNode, codeNode, closeBraceNode, ref changes);
        }

        return false;
    }

    private bool TryFormatHtmlInCSharp(FormattingContext context, ref PooledArrayBuilder<TextChange> changes, RazorSourceDocument source, RazorSyntaxNode node)
    {
        // void Method()
        // {
        //     <div></div>
        // }
        if (node is MarkupBlockSyntax markupBlockNode &&
            markupBlockNode.Parent is CSharpCodeBlockSyntax cSharpCodeBlock)
        {
            var openBraceNode = cSharpCodeBlock.Children.PreviousSiblingOrSelf(markupBlockNode);
            var closeBraceNode = cSharpCodeBlock.Children.NextSiblingOrSelf(markupBlockNode);

            return FormatBlock(context, source, directiveNode: null, openBraceNode, markupBlockNode, closeBraceNode, ref changes);
        }

        return false;
    }

    private static void TryFormatCSharpBlockStructure(FormattingContext context, ref PooledArrayBuilder<TextChange> changes, RazorSourceDocument source, RazorSyntaxNode node)
    {
        // We're looking for a code block like this:
        //
        // @code {
        //    var x = 1;
        // }
        //
        // The nodes will be a grandchild of a RazorDirective (the "@code") and we expect there to be
        // at least three children, being:
        // 1. Optional whitespace
        // 2. The opening brace
        // 3. The C# code
        // 4. The closing brace
        if (node is CSharpCodeBlockSyntax code &&
            node.Parent?.Parent is RazorDirectiveSyntax directive &&
            !directive.ContainsDiagnostics &&
            directive.DirectiveDescriptor?.Kind == DirectiveKind.CodeBlock)
        {
            // If we're formatting a @code or @functions directive, the user might have indicated they always want a newline
            var forceNewLine = context.Options.CodeBlockBraceOnNextLine &&
                directive.Body is RazorDirectiveBodySyntax { Keyword: { } keyword } &&
                IsCodeOrFunctionsBlock(keyword);

            var children = code.Children;
            if (TryGetLeadingWhitespace(children, out var whitespace))
            {
                // For whitespace we normalize it differently depending on if its multi-line or not
                FormatWhitespaceBetweenDirectiveAndBrace(whitespace, directive, ref changes, source, context, forceNewLine);
            }
            else if (children.TryGetOpenBraceToken(out var brace))
            {
                // If there is no whitespace at all we normalize to a single space
                var newText = forceNewLine
                    ? context.NewLineString + FormattingUtilities.GetIndentationString(
                        directive.GetLinePositionSpan(source).Start.Character, context.Options.InsertSpaces, context.Options.TabSize)
                    : " ";

                changes.Add(new TextChange(new TextSpan(brace.SpanStart, 0), newText));
            }
        }

        static bool TryGetLeadingWhitespace(RazorRazorSyntaxNodeList children, [NotNullWhen(true)] out UnclassifiedTextLiteralSyntax? whitespace)
        {
            // If there is whitespace between the directive and the brace, it will be in the first child
            // of the 4 total children
            whitespace = null;
            if (children.Count == 4 &&
                children[0] is UnclassifiedTextLiteralSyntax literal &&
                literal.ContainsOnlyWhitespace())
            {
                whitespace = literal;
            }

            return whitespace != null;
        }
    }

    private static void TryFormatSingleLineDirective(ref PooledArrayBuilder<TextChange> changes, RazorSyntaxNode node)
    {
        // Looking for single line directives like
        //
        // @attribute [Obsolete("old")]
        //
        // The CSharpCodeBlockSyntax covers everything from the end of "attribute" to the end of the line
        if (IsSingleLineDirective(node, out var children) || node.IsUsingDirective(out children))
        {
            // Shrink any block of C# that only has whitespace down to a single space.
            // In the @attribute case above this would only be the whitespace between the directive and code
            // but for @inject its also between the type and the field name.
            foreach (var child in children)
            {
                if (child.ContainsOnlyWhitespace(includingNewLines: false))
                {
                    ShrinkToSingleSpace(child, ref changes);
                }
            }
        }

        static bool IsSingleLineDirective(RazorSyntaxNode node, out RazorSyntaxNodeList children)
        {
            if (node is CSharpCodeBlockSyntax content &&
                node.Parent?.Parent is RazorDirectiveSyntax directive &&
                directive.DirectiveDescriptor?.Kind == DirectiveKind.SingleLine)
            {
                children = content.Children;
                return true;
            }

            children = default;
            return false;
        }
    }

    private static void FormatWhitespaceBetweenDirectiveAndBrace(RazorSyntaxNode node, RazorDirectiveSyntax directive, ref PooledArrayBuilder<TextChange> changes, RazorSourceDocument source, FormattingContext context, bool forceNewLine)
    {
        if (node.ContainsOnlyWhitespace(includingNewLines: false) && !forceNewLine)
        {
            ShrinkToSingleSpace(node, ref changes);
        }
        else
        {
            // If there is a newline then we want to have just one newline after the directive
            // and indent the { to match the @
            var newText = context.NewLineString + FormattingUtilities.GetIndentationString(
                    directive.GetLinePositionSpan(source).Start.Character, context.Options.InsertSpaces, context.Options.TabSize);

            changes.Add(new TextChange(node.Span, newText));
        }
    }

    private static void ShrinkToSingleSpace(RazorSyntaxNode node, ref PooledArrayBuilder<TextChange> changes)
    {
        // If there is anything other than one single space then we replace with one space between directive and brace.
        //
        // ie, "@code     {" will become "@code {"
        changes.Add(new TextChange(node.Span, " "));
    }

    private bool FormatBlock(FormattingContext context, RazorSourceDocument source, RazorSyntaxNode? directiveNode, RazorSyntaxNode openBraceNode, RazorSyntaxNode codeNode, RazorSyntaxNode closeBraceNode, ref PooledArrayBuilder<TextChange> changes)
    {
        var didFormat = false;

        if (!codeNode.TryGetLinePositionSpanWithoutWhitespace(source, out var codeRange))
        {
            return didFormat;
        }

        var additionalIndentation = "";
        if (_languageServerFeatureOptions.UseNewFormattingEngine)
        {
            // It's important with the new formatting engine that we maintain the indentation that the Html formatter would have applied,
            // if the Razor formatting pass had happened first. This is only applicable inside an element, as that is the only place that
            // the Html formatter will do anything.
            // TODO: Rather than ascend up the tree, this could be smarter as this class already descends down the tree
            if (openBraceNode.AncestorsAndSelf().Any(n => n is MarkupTagHelperElementSyntax or MarkupElementSyntax))
            {
                var openBraceLineNumber = openBraceNode.GetLinePositionSpan(source).Start.Line;
                var openBraceLine = source.Text.Lines[openBraceLineNumber];
                Debug.Assert(openBraceLine.GetFirstNonWhitespacePosition().HasValue);
                additionalIndentation = source.Text.GetSubTextString(TextSpan.FromBounds(openBraceLine.Start, openBraceLine.GetFirstNonWhitespacePosition().GetValueOrDefault()));
            }
        }

        if (openBraceNode.TryGetLinePositionSpanWithoutWhitespace(source, out var openBraceRange) &&
            openBraceRange.End.Line == codeRange.Start.Line &&
            !RangeHasBeenModified(ref changes, source.Text, codeRange))
        {
            var end = codeRange.Start;
            if (!_languageServerFeatureOptions.UseNewFormattingEngine)
            {
                // This logic is harmful in the new formatting engine, because it is interpreted as being the result of the Html formatter
                end = openBraceRange.End;
                var additionalIndentationLevel = GetAdditionalIndentationLevel(context, openBraceRange, openBraceNode, codeNode);
                if (additionalIndentationLevel > 0)
                {
                    additionalIndentation = FormattingUtilities.GetIndentationString(additionalIndentationLevel, context.Options.InsertSpaces, context.Options.TabSize);
                }
            }

            var newText = context.NewLineString + additionalIndentation;
            changes.Add(new TextChange(source.Text.GetTextSpan(openBraceRange.End, end), newText));
            didFormat = true;
        }

        if (closeBraceNode.Span.Length > 0 &&
            closeBraceNode.TryGetLinePositionSpanWithoutWhitespace(source, out var closeBraceRange) &&
            !RangeHasBeenModified(ref changes, source.Text, codeRange))
        {
            if (directiveNode is not null &&
                directiveNode.GetRange(source).Start.Character < closeBraceRange.Start.Character)
            {
                // If we have a directive, then we line the close brace up with it, and ensure
                // there is a close brace
                var span = new LinePositionSpan(codeRange.End, closeBraceRange.Start);
                var newText = context.NewLineString + FormattingUtilities.GetIndentationString(
                        directiveNode.GetRange(source).Start.Character, context.Options.InsertSpaces, context.Options.TabSize);

                changes.Add(new TextChange(source.Text.GetTextSpan(span), newText));
                didFormat = true;
            }
            else if (codeRange.End.Line == closeBraceRange.Start.Line &&
                codeNode.GetLastToken(includeZeroWidth: false) is not { Kind: SyntaxKind.NewLine })
            {
                // Add a Newline between the content and the "}" if one doesn't already exist, and make sure it lines
                // up with the start of the line that the open brace is on, as though it had been through the Html formatter.
                // In the new formatter, we have to make sure there is no extra whitespace on the new line, or it will be
                // kept when recording Html indentation. This probably wouldn't be an issue in the old engine, but I'm being
                // cautious.
                var start = _languageServerFeatureOptions.UseNewFormattingEngine
                    ? closeBraceRange.Start
                    : codeRange.End;
                changes.Add(new TextChange(source.Text.GetTextSpan(codeRange.End, start), context.NewLineString + additionalIndentation));
                didFormat = true;
            }

            // If there is code after the close brace, then we want to add a newline after it and push the code to the next
            // line. In other words, we expect only whitespace characters after the close brace, on this line.
            var closeBraceLine = source.Text.Lines[closeBraceRange.End.Line];
            if (closeBraceLine.GetFirstNonWhitespaceOffset(closeBraceRange.End.Character).HasValue)
            {
                // Insert a newline after the close brace
                changes.Add(new TextChange(source.Text.GetTextSpan(closeBraceRange.End, closeBraceRange.End), context.NewLineString));
            }
        }

        return didFormat;

        static bool RangeHasBeenModified(ref readonly PooledArrayBuilder<TextChange> changes, SourceText sourceText, LinePositionSpan span)
        {
            // Because we don't always know what kind of Razor object we're operating on we have to do this to avoid duplicate edits.
            // The other way to accomplish this would be to apply the edits after every node and function, but that's not in scope for my current work.
            var endIndex = sourceText.GetRequiredAbsoluteIndex(span.End);
            var hasBeenModified = changes.Any(e => e.Span.End == endIndex);

            return hasBeenModified;
        }

        static int GetAdditionalIndentationLevel(FormattingContext context, LinePositionSpan range, RazorSyntaxNode openBraceNode, RazorSyntaxNode codeNode)
        {
            if (!context.TryGetIndentationLevel(codeNode.Position, out var desiredIndentationLevel))
            {
                // If for some reason we don't match a particular span use the indentation for the whole line
                var indentations = context.GetIndentations();
                var indentation = indentations[range.Start.Line];
                desiredIndentationLevel = indentation.HtmlIndentationLevel + indentation.RazorIndentationLevel;
            }

            var desiredIndentationOffset = context.GetIndentationOffsetForLevel(desiredIndentationLevel);
            var currentIndentationOffset = GetTrailingWhitespaceLength(openBraceNode, context) + GetLeadingWhitespaceLength(codeNode, context);

            return desiredIndentationOffset - currentIndentationOffset;

            static int GetLeadingWhitespaceLength(RazorSyntaxNode node, FormattingContext context)
            {
                var tokens = node.GetTokens();
                var whitespaceLength = 0;

                foreach (var token in tokens)
                {
                    if (token.IsWhitespace())
                    {
                        if (token.Kind == SyntaxKind.NewLine)
                        {
                            // We need to reset when we move to a new line.
                            whitespaceLength = 0;
                        }
                        else if (token.IsSpace())
                        {
                            whitespaceLength++;
                        }
                        else if (token.IsTab())
                        {
                            whitespaceLength += (int)context.Options.TabSize;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                return whitespaceLength;
            }

            static int GetTrailingWhitespaceLength(RazorSyntaxNode node, FormattingContext context)
            {
                var tokens = node.GetTokens();
                var whitespaceLength = 0;

                for (var i = tokens.Count - 1; i >= 0; i--)
                {
                    var token = tokens[i];
                    if (token.IsWhitespace())
                    {
                        if (token.Kind == SyntaxKind.NewLine)
                        {
                            whitespaceLength = 0;
                        }
                        else if (token.IsSpace())
                        {
                            whitespaceLength++;
                        }
                        else if (token.IsTab())
                        {
                            whitespaceLength += (int)context.Options.TabSize;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                return whitespaceLength;
            }
        }
    }

    private static bool IsCodeOrFunctionsBlock(RazorSyntaxNode keyword)
    {
        var keywordContent = keyword.GetContent();
        return keywordContent == FunctionsDirective.Directive.Directive ||
            keywordContent == ComponentCodeDirective.Directive.Directive;
    }
}

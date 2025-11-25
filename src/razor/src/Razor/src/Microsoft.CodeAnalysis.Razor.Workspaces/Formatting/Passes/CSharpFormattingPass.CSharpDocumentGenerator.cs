// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;
using RazorSyntaxToken = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxToken;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

internal partial class CSharpFormattingPass
{
    /// <summary>
    /// Generates a C# document in order to get Roslyn formatting behaviour on a Razor document
    /// </summary>
    /// <remarks>
    /// <para>
    /// The general theory is to take a Razor syntax tree and convert it to something that looks to Roslyn like a C#
    /// document, in a way that accurately represents the indentation constructs that a Razor user is expressing when
    /// they write the Razor code.
    /// </para>
    /// <para>
    /// For example, given the following Razor file:
    /// <code>
    /// &lt;div&gt;
    ///     @if (true)
    ///     {
    ///         // Some code
    ///     }
    /// &lt;/div&gt;
    ///
    /// @code {
    ///     private string Name { get; set; }
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// The generator will go through that syntax tree and produce the following C# document:
    /// <code>
    /// // &lt;div&gt;
    ///     @if (true)
    ///     {
    ///         // Some code
    ///     }
    /// // &lt;/div&gt;
    ///
    /// class F
    /// {
    ///     private string Name { get; set; }
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// The class definition is clearly not present in the source Razor document, but it represents the intended
    /// indentation that the user would expect to see for the property declaration. Additionally the indentation
    /// of the @if block is recorded, so that after formatting the C#, which Roslyn will set back to column 0, we
    /// reapply it so we end up with the C# indentation and Html indentation combined.
    /// </para>
    /// <para>
    /// For more complete examples, the full test log for every formatting test includes the generated C# document.
    /// </para>
    /// <para>
    /// A final important note about this class, whilst it is a SyntaxVisitor, it is not intended to be a general
    /// purpose one, and things won't work as expected if the Visit method is called on arbitrary nodes. The visit
    /// methods are implemented with the assumption they will only see a node if it is the first one on a line of
    /// a Razor file.
    /// </para>
    /// </remarks>
    private sealed class CSharpDocumentGenerator
    {
        public static CSharpFormattingDocument Generate(RazorCodeDocument codeDocument, RazorFormattingOptions options)
        {
            using var _1 = StringBuilderPool.GetPooledObject(out var builder);
            using var _2 = ArrayBuilderPool<LineInfo>.GetPooledObject(out var lineInfoBuilder);
            lineInfoBuilder.SetCapacityIfLarger(codeDocument.Source.Text.Lines.Count);

            var generator = new Generator(codeDocument, options, builder, lineInfoBuilder);

            generator.Generate();

            var text = SourceText.From(builder.ToString());

            return new(text, lineInfoBuilder.ToImmutableAndClear());
        }

        private static string GetAdditionalLineComment(SourceSpan originalSpan)
        {
            // IMPORTANT: The format here needs to match the parse method below
            return $"// {originalSpan.AbsoluteIndex} {originalSpan.Length}";
        }

        public static bool TryParseAdditionalLineComment(TextLine line, out int start, out int length)
        {
            start = 0;
            length = 0;

            // We're looking for a line that matches "// {start} {length}", where start and length are integers.

            // Need at least 6 chars for two single digit integers separated by a space, plus "// "
            if (line.Span.Length < 6)
            {
                return false;
            }

            if (line.CharAt(0) != '/' ||
                line.CharAt(1) != '/' ||
                line.CharAt(2) != ' ')
            {
                return false;
            }

            var span = line.ToString().AsSpan();
            var toParse = span[3..];
            var space = toParse.IndexOf(' ');
            if (space == -1)
            {
                return false;
            }

            var startSpan = toParse[..space];
            var lengthSpan = toParse[(space + 1)..];

#if NET8_0_OR_GREATER
            return int.TryParse(startSpan, out start)
                && int.TryParse(lengthSpan, out length);
#else
            return int.TryParse(startSpan.ToString(), out start)
                && int.TryParse(lengthSpan.ToString(), out length);
#endif
        }

        private sealed class Generator(
            RazorCodeDocument codeDocument,
            RazorFormattingOptions options,
            StringBuilder builder,
            ImmutableArray<LineInfo>.Builder lineInfoBuilder) : SyntaxVisitor<LineInfo>
        {
            private readonly SourceText _sourceText = codeDocument.Source.Text;
            private readonly RazorCodeDocument _codeDocument = codeDocument;
            private readonly bool _insertSpaces = options.InsertSpaces;
            private readonly int _tabSize = options.TabSize;
            private readonly RazorCSharpSyntaxFormattingOptions? _csharpSyntaxFormattingOptions = options.CSharpSyntaxFormattingOptions;
            private readonly StringBuilder _builder = builder;
            private readonly ImmutableArray<LineInfo>.Builder _lineInfoBuilder = lineInfoBuilder;

            private TextLine _currentLine;
            private int _currentFirstNonWhitespacePosition;

            private RazorSyntaxToken _currentToken;
            private RazorSyntaxToken _previousCurrentToken;

            /// <summary>
            /// The line number of the last line of the current element, if we're inside one.
            /// </summary>
            /// <remarks>
            /// This is used to track if the syntax node at the start of each line is parented by an element node, without
            /// having to do lots of tree traversal.
            /// </remarks>
            private int? _elementEndLine;
            /// <summary>
            /// The line number of the last line of a block where formatting should be completely ignored
            /// </summary>
            /// <remarks>
            /// Some Html constructs, namely &lt;textarea&gt; and &lt;pre&gt;, should not be formatted at all, and we essentially
            /// need to treat them as multiline Razor comments. This field is used to track the line number of the last line of such
            /// an element, so we can ignore every line in it without having to do lots of tree traversal to check "are we parented
            /// by a pre tag" etc.
            /// </remarks>
            private int? _ignoreUntilLine;

            public void Generate()
            {
                using var _ = StringBuilderPool.GetPooledObject(out var additionalLinesBuilder);

                var root = _codeDocument.GetRequiredSyntaxRoot();
                var sourceMappings = _codeDocument.GetRequiredCSharpDocument().SourceMappings;
                var iMapping = 0;
                foreach (var line in _sourceText.Lines)
                {
                    if (line.GetFirstNonWhitespacePosition() is int firstNonWhitespacePosition)
                    {
                        _previousCurrentToken = _currentToken;
                        _currentLine = line;
                        _currentFirstNonWhitespacePosition = firstNonWhitespacePosition;
                        _currentToken = root.FindToken(firstNonWhitespacePosition);

                        var length = _builder.Length;
                        _lineInfoBuilder.Add(Visit(_currentToken.Parent));
                        Debug.Assert(_builder.Length > length, "Didn't output any generated code!");

                        // If there are C# mappings on this line, we want to output additional lines that represent the C# blocks.
                        while (iMapping < sourceMappings.Length)
                        {
                            var originalSpan = sourceMappings[iMapping].OriginalSpan;
                            if (originalSpan.AbsoluteIndex < _currentFirstNonWhitespacePosition)
                            {
                                iMapping++;
                            }
                            else if (originalSpan.AbsoluteIndex > _currentFirstNonWhitespacePosition &&
                                (originalSpan.AbsoluteIndex + originalSpan.Length) <= line.Span.End)
                            {
                                // We've found a span mapping that means there is some C# on this line, so if its an explicit or implicit expression
                                // we need to format it, but separately to the rest of the document.
                                var node = root.FindInnermostNode(originalSpan.AbsoluteIndex);
                                if (node is CSharpExpressionLiteralSyntax)
                                {
                                    AddAdditionalLineFormattingContent(additionalLinesBuilder, node, originalSpan);
                                }

                                iMapping++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        _builder.AppendLine();
                        _lineInfoBuilder.Add(CreateLineInfo(processIndentation: false));
                    }

                    // If we're inside an element that ends on this line, clear the field that tracks it.
                    if (_elementEndLine is { } endLine &&
                        endLine == line.LineNumber)
                    {
                        _elementEndLine = null;
                    }

                    if (_ignoreUntilLine is { } endLine2 &&
                        endLine2 == line.LineNumber)
                    {
                        _ignoreUntilLine = null;
                    }
                }

                _builder.AppendLine();
                _builder.AppendLine(additionalLinesBuilder.ToString());
            }

            private void AddAdditionalLineFormattingContent(StringBuilder additionalLinesBuilder, RazorSyntaxNode node, SourceSpan originalSpan)
            {
                // Rather than bother to store more data about the formatted file, since we don't actually know where
                // these will end up in that file once it's all said and done, we are just going to use a simple comment
                // format that we can easily parse.

                // Special case, for attributes that represent generic type parameters, we want to output something such
                // that Roslyn knows to format it as a type. For example, the meaning and spacing around "?"s should be
                // what the user expects.
                if (node is { Parent.Parent: MarkupTagHelperAttributeSyntax attribute } &&
                    attribute is { Parent.Parent: MarkupTagHelperElementSyntax element } &&
                    element.TagHelperInfo.BindingResult.TagHelpers is [{ } descriptor] &&
                    descriptor.IsGenericTypedComponent() &&
                    descriptor.BoundAttributes.FirstOrDefault(d => d.Name == attribute.TagHelperAttributeInfo.Name) is { } boundAttribute &&
                    boundAttribute.IsTypeParameterProperty())
                {
                    additionalLinesBuilder.AppendLine("F<");
                    additionalLinesBuilder.AppendLine(GetAdditionalLineComment(originalSpan));
                    additionalLinesBuilder.AppendLine(_sourceText.GetSubTextString(originalSpan.ToTextSpan()));
                    additionalLinesBuilder.AppendLine("> x;");
                    return;
                }

                additionalLinesBuilder.AppendLine(GetAdditionalLineComment(originalSpan));
                additionalLinesBuilder.AppendLine(_sourceText.GetSubTextString(originalSpan.ToTextSpan()));
                additionalLinesBuilder.AppendLine(";");
            }

            public override LineInfo Visit(RazorSyntaxNode? node)
            {
                // Sometimes we are in a block where we want to do no formatting at all
                if (_ignoreUntilLine is not null)
                {
                    return EmitCurrentLineWithNoFormatting();
                }

                return base.Visit(node);
            }

            protected override LineInfo DefaultVisit(RazorSyntaxNode node)
            {
                return EmitCurrentLineAsCSharp();
            }

            public override LineInfo VisitCSharpExpressionLiteral(CSharpExpressionLiteralSyntax node)
            {
                if (_sourceText.GetLinePositionSpan(node.Span).SpansMultipleLines())
                {
                    return VisitMultilineCSharpExpressionLiteral(node);
                }

                Debug.Assert(node.LiteralTokens.Count > 0);
                return VisitCSharpLiteral(node, node.LiteralTokens[^1]);
            }

            private LineInfo VisitMultilineCSharpExpressionLiteral(CSharpExpressionLiteralSyntax node)
            {
                // Literals that span multiple lines are interesting. eg, given:
                //
                // <div class="@(foo
                //              .bar)" />
                //
                // The first line of this we hit will actually be the middle of the expression, so we need to make sure to include
                // the end of the previous line, so the C# code is more correct, if that line didn't start with C#.
                // On the last line of C# we need to ensure we don't inadvertently output any non-C# content, ie the ')" />' above.
                // And of course the second line could be the last line :)
                var skipPreviousLine = false;
                var nodeStartLine = GetLineNumber(node);
                if (nodeStartLine == _currentLine.LineNumber - 1 &&
                    _sourceText.Lines[nodeStartLine] is { } previousLine &&
                    previousLine.GetFirstNonWhitespacePosition() != node.Position &&
                    _previousCurrentToken.Kind != SyntaxKind.Transition)
                {
                    // This is a multi-line literal, and we're emiting the 2nd line, and the literal didn't start at the start of
                    // the previous line, so it wouldn't have been handled by that lines formatting. We need to include it here,
                    // but skip it to not confuse things.
                    _builder.AppendLine(_sourceText.GetSubTextString(TextSpan.FromBounds(node.SpanStart, previousLine.End)));
                    skipPreviousLine = true;
                }

                // The last line of this might not be entirely C#, so we have to trim off the end so as not to cause issues. For the
                // middle lines, we don't need to worry about that, but we do have to deal with quirks for Html attribute (see below)
                // so this code handles both cases:

                // We can't use node.Span because it can contain newlines from the line before, so we have to work a little.

                // First, emit the whitespace, so user spacing is honoured if possible
                _builder.Append(_sourceText.ToString(TextSpan.FromBounds(_currentLine.Start, _currentFirstNonWhitespacePosition)));

                // Now emit the contents of the line. If this is the last line of the expression literal, then we want to stop at the
                // end of the node, as there could be other contents afterwards, so work out if we're in that case first.
                var toEndOfNode = _sourceText.GetLinePosition(node.EndPosition).Line == _currentLine.LineNumber;

                var isEndOfExplicitExpression = false;
                var end = _currentLine.End;

                if (toEndOfNode)
                {
                    // A special case here is if we're inside an explicit expression body, one of the bits of content after the node will
                    // be the final close parens, so we need to emit that or the C# expression won't be valid, and we can't trust the formatter.
                    if (node.Parent.Parent is CSharpExplicitExpressionBodySyntax explicitExpression &&
                        _sourceText.GetLinePosition(explicitExpression.EndPosition).Line == _currentLine.LineNumber)
                    {
                        isEndOfExplicitExpression = true;
                        end = explicitExpression.EndPosition;
                    }
                    else
                    {
                        end = node.EndPosition;
                    }
                }

                var span = TextSpan.FromBounds(_currentFirstNonWhitespacePosition, end);
                _builder.Append(_sourceText.ToString(span));

                // Keep track of how many characters we add to the end of the line, so the formatter knows to ignore them.
                var offsetFromEnd = 0;

                // If we're at the end of an explicit expression, we want to add a semi-colon to the end of the line, so that C# after the
                // expression is formatted correctly. ie, we need to "close" the expression.
                if (isEndOfExplicitExpression)
                {
                    offsetFromEnd++;
                    _builder.Append(';');
                }

                // Append a comment at the end so whitespace isn't removed, as Roslyn thinks its the end of the line, but we know it isn't.
                // eg, given "4, 5, @<div></div>", we want Roslyn to keep the space after the last comma, because there is something after it,
                // but we can't let Roslyn see the "@<div>" that it is.
                // We use a multi-line comment because Roslyn has a desire to line up "//" comments with the previous line, which we could interpret
                // as Roslyn suggesting we indent some trailing Html.
                const string EndOfLineComment = " /* */";
                offsetFromEnd += EndOfLineComment.Length;
                _builder.AppendLine(EndOfLineComment);

                // Final quirk: If we're inside an Html attribute, it means the Html formatter won't have formatted this line, as multi-line
                // Html attributes are not valid.
                // TODO: The traverse up the tree here is not ideal. See comments in https://github.com/dotnet/razor/issues/11371
                var htmlIndentLevel = 0;
                string? additionalIndentation = null;
                if (node.Ancestors().FirstOrDefault(n => n.IsAnyAttributeSyntax()) is { } attributeNode)
                {
                    // The attribute node can have whitespace, including even a newline, in front of it, so we get the first non-whitespace
                    // character, then find the offset of that in order to calculate our desired indent level.
                    _sourceText.TryGetFirstNonWhitespaceOffset(attributeNode.Span, out var startChar, out _);
                    startChar = _sourceText.GetLinePosition(attributeNode.SpanStart + startChar).Character;
                    htmlIndentLevel = startChar / _tabSize;
                    additionalIndentation = new string(' ', startChar % _tabSize);
                }

                return CreateLineInfo(
                    skipPreviousLine: skipPreviousLine,
                    processFormatting: true,
                    formattedLength: span.Length,
                    formattedOffsetFromEndOfLine: offsetFromEnd,
                    htmlIndentLevel: htmlIndentLevel,
                    additionalIndentation: additionalIndentation,
                    checkForNewLines: false);
            }

            public override LineInfo VisitCSharpStatementLiteral(CSharpStatementLiteralSyntax node)
            {
                Debug.Assert(node.LiteralTokens.Count > 0);

                // If this is the end of a multi-line CSharp template (ie, RenderFragment) then we need to close
                // out the lambda expression that we started when we opened it.
                if (node.LiteralTokens.Where(static t => !t.IsWhitespace()).FirstOrDefault() is { Content: ";" } &&
                    node.TryGetPreviousSibling(out var previousSibling) &&
                    previousSibling is CSharpTemplateBlockSyntax &&
                    GetLineNumber(previousSibling.GetFirstToken()) != GetLineNumber(previousSibling.GetLastToken()))
                {
                    _builder.AppendLine("};");
                    return CreateLineInfo();
                }

                return VisitCSharpLiteral(node, node.LiteralTokens[^1]);
            }

            private LineInfo VisitCSharpLiteral(RazorSyntaxNode node, RazorSyntaxToken lastToken)
            {
                // If we get here we have a line of code which starts in C#, but Razor being Razor means we can't assume it
                // is entirely C#. For example it could be:
                //
                // Render(@<div></div>);
                //
                // In these situations we simply stop at the transition away from C# and output the line as C#. The only
                // interesting thing we have to do is tell the formatter that we've done that, so it doesn't expect the
                // line contents to match the original line entirely. The Html formatter will have dealt with the bits after
                // the transition anyway.
                //
                // The final quirk is that the node in question can span multiple lines, but the transition can only
                // possibly be on the last line.
                if (lastToken.GetNextToken() is { Kind: SyntaxKind.Transition } token &&
                    GetLineNumber(token) == GetLineNumber(_currentToken))
                {
                    // We can't use node.Span because it can contain newlines from the line before.
                    // Emit the whitespace, so user spacing is honoured if possible
                    _builder.Append(_sourceText.ToString(TextSpan.FromBounds(_currentLine.Start, _currentFirstNonWhitespacePosition)));
                    // Now emit the contents
                    var span = TextSpan.FromBounds(_currentFirstNonWhitespacePosition, node.EndPosition);
                    _builder.Append(_sourceText.ToString(span));

                    // Putting a semi-colon on the end might make for invalid C#, but it means this line won't cause indentation,
                    // which is all we need. If we're in an explicit expression body though, we don't want to do this, as the
                    // close paren of the expression will do the same job (and the semi-colon would confuse that).
                    var emitSemiColon = node.Parent.Parent is not CSharpExplicitExpressionBodySyntax;

                    var skipNextLineIfBrace = false;
                    int formattedOffsetFromEndOfLine;

                    // If the template is multiline we emit a lambda expression, otherwise just a null statement so there
                    // is something there. See VisitMarkupTransition for more info
                    if (token.Parent?.Parent.Parent is CSharpTemplateBlockSyntax template &&
                        _sourceText.GetLinePositionSpan(template.Span).SpansMultipleLines())
                    {
                        emitSemiColon = false;
                        skipNextLineIfBrace = true;
                        _builder.AppendLine("() => {");

                        // We only want to format up to the text we added, but if Roslyn inserted a newline before the brace
                        // then that position will be different. If we're not given the options then we assume the default behaviour of
                        // Roslyn which is to insert the newline.
                        formattedOffsetFromEndOfLine = _csharpSyntaxFormattingOptions?.NewLines.IsFlagSet(RazorNewLinePlacement.BeforeOpenBraceInLambdaExpressionBody) ?? true
                            ? 5
                            : 7;
                    }
                    else
                    {
                        _builder.AppendLine("null");
                        formattedOffsetFromEndOfLine = 4;
                    }

                    if (emitSemiColon)
                    {
                        _builder.AppendLine(";");
                    }

                    return CreateLineInfo(
                        skipNextLine: emitSemiColon,
                        skipNextLineIfBrace: skipNextLineIfBrace,
                        formattedLength: span.Length,
                        formattedOffsetFromEndOfLine: formattedOffsetFromEndOfLine,
                        processFormatting: true,
                        // We turn off check for new lines because that only works if the content doesn't change from the original,
                        // but we're deliberately leaving out a bunch of the original file because it would confuse the Roslyn formatter.
                        checkForNewLines: false);
                }

                return EmitCurrentLineAsCSharp();
            }

            public override LineInfo VisitMarkupStartTag(MarkupStartTagSyntax node)
            {
                var element = (MarkupElementSyntax)node.Parent;

                if (node.Name.Content == "textarea")
                {
                    // The contents of textareas is significant, so we never want any formatting to happen inside them
                    _ignoreUntilLine = GetLineNumber(element.EndTag?.CloseAngle ?? element.StartTag.CloseAngle);
                }
                else if (_elementEndLine is null)
                {
                    // If this is an element at the root level, we want to record where it ends. We can't rely on the Visit method
                    // for it, because it might not be at the start of a line.
                    _elementEndLine = GetLineNumber(element.EndTag?.CloseAngle ?? element.StartTag.CloseAngle);
                }

                return EmitCurrentLineAsComment();
            }

            public override LineInfo VisitMarkupEndTag(MarkupEndTagSyntax node)
            {
                return VisitEndTag(node);
            }

            private LineInfo VisitEndTag(BaseMarkupEndTagSyntax node)
            {
                // If this is the last line of a multi-line CSharp template (ie, RenderFragment), and the semicolon that ends
                // if is on the same line, then we need to close out the lambda expression that we started when we opened it.
                // If the semicolon is on the next line, then we'll take care of that when we get to it.
                if (node.Parent.Parent.Parent is CSharpTemplateBlockSyntax template &&
                    GetLineNumber(template.GetLastToken()) == GetLineNumber(_currentToken) &&
                    GetLineNumber(template.GetFirstToken()) != GetLineNumber(template.GetLastToken()) &&
                    template.GetLastToken().GetNextToken() is { } semiColonToken &&
                    semiColonToken.Content == ";" &&
                    GetLineNumber(semiColonToken) == GetLineNumber(_currentToken))
                {
                    _builder.AppendLine("};");
                    return CreateLineInfo();
                }

                return EmitCurrentLineAsComment();
            }

            public override LineInfo VisitMarkupTagHelperStartTag(MarkupTagHelperStartTagSyntax node)
            {
                // If this is an element at the root level, we want to record where it ends. We can't rely on the Visit method
                // for it, because it might not be at the start of a line.
                if (_elementEndLine is null)
                {
                    var element = (MarkupTagHelperElementSyntax)node.Parent;
                    _elementEndLine = GetLineNumber(element.EndTag?.CloseAngle ?? element.StartTag.CloseAngle);
                }

                return EmitCurrentLineAsComment();
            }

            public override LineInfo VisitRazorMetaCode(RazorMetaCodeSyntax node)
            {
                // Meta code is a few things, and mostly they're valid C#, but one case we have to specifically handle is
                // bound attributes that start on their own line, eg the second line of:
                //
                // <Thing foo="bar"
                //        @bind-value="baz" />
                if (node.MetaCode is [{ Kind: SyntaxKind.Transition }, ..])
                {
                    // This is not C# so we just need to avoid the default visit
                    return EmitCurrentLineAsComment();
                }

                if (node is
                    {
                        Parent: CSharpExplicitExpressionBodySyntax,
                        MetaCode: [{ Kind: SyntaxKind.RightParenthesis } paren]
                    } &&
                    paren.GetPreviousToken() is { Parent: CSharpExpressionLiteralSyntax literal })
                {
                    // This is the close bracket of a multi-line "@( .. )" expression at the start of a line, ie the last line of:
                    //
                    //    @(DateTime.
                    //      Now
                    //    )
                    //
                    // This needs some funky handling because there could be non-C# after the close parens, but fortunately
                    // the method we already have that handles the intervening lines has all of the right code.
                    return VisitMultilineCSharpExpressionLiteral(literal);
                }

                return EmitCurrentLineAsCSharp();
            }

            public override LineInfo VisitMarkupEphemeralTextLiteral(MarkupEphemeralTextLiteralSyntax node)
            {
                // A MarkupEphemeralTextLiteral is an escaped @ sign, eg in CSS "@@font-face". We just treat it like markup text
                return VisitMarkupLiteral();
            }

            public override LineInfo VisitMarkupTextLiteral(MarkupTextLiteralSyntax node)
            {
                return VisitMarkupLiteral();
            }

            private LineInfo VisitMarkupLiteral()
            {
                // For markup text literal, we always want to honour the Html formatter, so we supply the Html indent.
                // Normally that would only happen if we were inside a markup element
#if DEBUG
                _builder.AppendLine($"// {_currentLine}");
#else
                _builder.AppendLine($"//");
#endif
                return CreateLineInfo(
                    htmlIndentLevel: FormattingUtilities.GetIndentationLevel(_currentLine, _currentFirstNonWhitespacePosition, _insertSpaces, _tabSize, out var additionalIndentation),
                    additionalIndentation: additionalIndentation);
            }

            public override LineInfo VisitMarkupTagHelperEndTag(MarkupTagHelperEndTagSyntax node)
            {
                return VisitEndTag(node);
            }

            public override LineInfo VisitMarkupTransition(MarkupTransitionSyntax node)
            {
                // A transition to Html means the start of a RenderFragment. These are challenging because conceptually
                // they are like a Write() call, because their contents are sent to the output, but they can also contain
                // statements. eg:
                //
                // RenderFragment f =
                //     @<div>
                //          @if (true)
                //          {
                //              <p>Some text</p>
                //          }
                //     </div>;
                //
                // If we convert that to C# the way we normally do, we end up with statements in a C# context where only
                // expressions are valid. To avoid that, we need to emit C# such that we can be sure we're in a context
                // where statements are valid. To do this we emit a block bodied lambda expression. Ironically this whole
                // formatting engine arguably exists because the compiler loves to emit lambda expressions, but they're
                // really annoying to format. This just happens to be the one case where a lambda is the right choice.

                // Emit the whitespace, so user spacing is honoured if possible
                _builder.Append(_sourceText.ToString(TextSpan.FromBounds(_currentLine.Start, _currentFirstNonWhitespacePosition)));

                // If its a one-line render fragment, then we don't need to worry.
                if (GetLineNumber(node.Parent.GetLastToken()) == GetLineNumber(_currentToken))
                {
                    _builder.AppendLine("null;");
                    return CreateLineInfo();
                }

                // Roslyn may move the opening brace to the next line, depending on its options. Unlike with code block
                // formatting where we put the opening brace on the next line ourselves (and Roslyn might bring it back)
                // if we do that for lambdas, Roslyn won't adjust the opening brace position at all. See, told you lambdas
                // were annoying to format.
                _builder.AppendLine("() => {");
                return CreateLineInfo(skipNextLineIfBrace: true);
            }

            public override LineInfo VisitRazorCommentBlock(RazorCommentBlockSyntax node)
            {
                // Every line of a multiline Razor comment will hit this method, but we have two different ways to handle it.
                // For the start comment, we output a C# comment, so it gets indented as normal. For all of the other lines,
                // we just tell the formatter to completely skip this line. The Html formatter also skips comment lines, so
                // they will be left exactly as the user wrote them.
                if (_currentToken.Kind == SyntaxKind.RazorCommentTransition)
                {
                    return EmitCurrentLineAsComment();
                }

                // Do nothing for any lines inside the comment
                return EmitCurrentLineWithNoFormatting();
            }

            public override LineInfo VisitCSharpTransition(CSharpTransitionSyntax node)
            {
                // Empty transition we just emit as nothing interesting
                if (node.Parent is null)
                {
                    return EmitCurrentLineAsComment();
                }

                // Other transitions, we decide based on the parent
                return base.Visit(node.Parent);
            }

            public override LineInfo VisitCSharpImplicitExpression(CSharpImplicitExpressionSyntax node)
            {
                // This matches like @DateTime.Now, which you would think we want to format as C#, but there can be multiple of them
                // on the same line, and they don't have to be the first thing on the line. We handle these above in GetCSharpDocumentContents,
                // so we can actually just emit these lines as a comment so the indentation is correct, and then let the code above
                // handle them. Essentially, whether these are at the start or int he middle of a line is irrelevant.

                return EmitCurrentLineAsComment();
            }

            public override LineInfo VisitCSharpExplicitExpression(CSharpExplicitExpressionSyntax node)
            {
                // If this is a single line expression, we handle it like we do for implicit expressions, irrelevant
                // of whether its at the start or in the middle of the line.
                var body = (CSharpExplicitExpressionBodySyntax)node.Body;
                var closeParen = body.CloseParen;
                if (GetLineNumber(closeParen) == GetLineNumber(node))
                {
                    return EmitCurrentLineAsComment();
                }

                // If this spans multiple lines however, the indentation of this line will affect the next, so we handle it in the
                // same way we handle a C# literal syntax. That includes checking if the C# doesn't go to the end of the line.
                // If the whole explicit expression is C#, then the children will be a single CSharpExpressionLiteral. If not, there
                // will be multiple children, and the second one is not C#, so thats the one we need to exclude from the generated
                // document.
                if (body.CSharpCode.Children is [_, { } secondChild, ..] &&
                    GetLineNumber(secondChild) == GetLineNumber(node))
                {
                    var span = TextSpan.FromBounds(_currentFirstNonWhitespacePosition + 1, secondChild.Position);
                    _builder.Append(_sourceText.ToString(span));
                    // Append a comment at the end so whitespace isn't removed, as Roslyn thinks its the end of the line, but we know it isn't.
                    _builder.AppendLine(" //");

                    return CreateLineInfo(
                        formattedLength: span.Length,
                        formattedOffsetFromEndOfLine: 3,
                        originOffset: 1,
                        processFormatting: true,
                        // We turn off check for new lines because that only works if the content doesn't change from the original,
                        // but we're deliberately leaving out a bunch of the original file because it would confuse the Roslyn formatter.
                        checkForNewLines: false);
                }

                _builder.AppendLine(_sourceText.GetSubTextString(TextSpan.FromBounds(_currentToken.Position + 1, _currentLine.End)));
                return CreateLineInfo(
                    processFormatting: true,
                    checkForNewLines: false,
                    originOffset: 1,
                    formattedOffset: 0);
            }

            public override LineInfo VisitCSharpCodeBlock(CSharpCodeBlockSyntax node)
            {
                // Matches things like @if, so skip the first character, but output as C# otherwise
                _builder.AppendLine(_sourceText.GetSubTextString(TextSpan.FromBounds(_currentToken.Position + 1, _currentLine.End)));

                return CreateLineInfo(
                    processFormatting: true,
                    checkForNewLines: true,
                    originOffset: 1,
                    formattedOffset: 0);
            }

            public override LineInfo VisitCSharpStatement(CSharpStatementSyntax node)
            {
                // Matches "@{".
                // Logically we can just output an open brace, but there is one quirk we have to handle, which is if there is nothing but
                // whitespace between the open and close braces, then we have to check if they're on the same line. Otherwise we'd output
                // and open brace and there would be no matching close. Fortunately in this situation we don't actually have to do anything
                // because an empty set of braces has no flow on effects. If we wanted to be opinionated and shrink the whitespace down
                // to a single space, that would be a job for the RazorFormattingPass.
                var body = (CSharpStatementBodySyntax)node.Body;
                if (GetLineNumber(body.OpenBrace) == GetLineNumber(body.CloseBrace))
                {
                    return EmitCurrentLineAsComment();
                }

                // We don't need to worry about formatting, or offsetting, because the RazorFormattingPass will
                // have ensured this node is followed by a newline, and if there was a space between the "@" and "{"
                // then it wouldn't be a CSharpStatementSyntax so we wouldn't be here!
                _builder.AppendLine("{");
                return CreateLineInfo();
            }

            public override LineInfo VisitRazorDirective(RazorDirectiveSyntax node)
            {
                // Unfortunately the Razor syntax tree doesn't distinguish different directives with different syntax node types,
                // so this method is handles way more cases that ideally it would. Sorry! I've split it up into separate methods
                // so we can pretend, for readability of those methods, if not this one.

                if (node.IsUsingDirective())
                {
                    return VisitUsingDirective();
                }

                if (node.IsAttributeDirective(out var attribute))
                {
                    return VisitAttributeDirective(attribute);
                }

                if (node.IsConstrainedTypeParamDirective(out var typeParam, out var conditions))
                {
                    return VisitTypeParamDirective(typeParam, conditions);
                }

                if (node.IsCodeDirective() ||
                    node.IsFunctionsDirective())
                {
                    return VisitCodeOrFunctionsDirective();
                }

                // All other directives that have braces are handled here
                if (node.Body is RazorDirectiveBodySyntax body &&
                    body.CSharpCode is CSharpCodeBlockSyntax code &&
                    code.Children.TryGetOpenBraceToken(out var brace) &&
                    // If the open brace is on the same line as the directive, then we need to ensure the contents are indented.
                    GetLineNumber(brace) == GetLineNumber(_currentToken))
                {
                    _builder.AppendLine("{");
                    return CreateLineInfo();
                }

                // If the brace is on a different line, then we don't need to do anything, as the brace will be output when
                // processing the next line.
                return EmitCurrentLineAsComment();
            }

            private LineInfo VisitCodeOrFunctionsDirective()
            {
                // If its an @code or @functions we want to wrap the contents in a class so that access modifiers
                // on any members declared within it are valid, and will be formatted as appropriate.
                // We let the users content be the class name, as it will either be "@code" or "@functions", which
                // are both valid, and it might have an open brace after it, or that might be on the next line,
                // but if we just let that flow to the generated document, we don't need to do any fancy checking.
                _builder.Append("class ");
                _builder.AppendLine(_currentLine.ToString());

                return CreateLineInfo(skipNextLineIfBrace: true);
            }

            private LineInfo VisitUsingDirective()
            {
                // For @using we just skip over the @ and format as a C# using directive
                // "@using System" to "using System"
                // Roslyn's parser is smart enough to not care about missing semicolons.
                _builder.AppendLine(_sourceText.GetSubTextString(TextSpan.FromBounds(_currentToken.Position + 1, _currentLine.End)));
                return CreateLineInfo(
                    processFormatting: true,
                    originOffset: 1,
                    formattedOffset: 0);
            }

            private LineInfo VisitTypeParamDirective(RazorSyntaxNode typeParam, RazorSyntaxNode conditions)
            {
                // For @typeparam we just need C# to format things after the "where", so we construct a local function that looks right
                // "@typeparam T where T : IDisposable" to "void F<T>() where T : IDisposable"
                // This is one of the weirder ones.
                var methodDef = $"void F<{typeParam.GetContent()}>() ";
                _builder.Append(methodDef);
                _builder.AppendLine(conditions.GetContent());
                _builder.AppendLine("=> null");

                return CreateLineInfo(
                    skipNextLine: true,
                    processFormatting: true,
                    originOffset: conditions.SpanStart - _currentToken.Position,
                    formattedOffset: methodDef.Length);
            }

            private LineInfo VisitAttributeDirective(RazorSyntaxNode attribute)
            {
                // For @attribute we skip over the directive itself and Roslyn can handle the rest
                // "@attribute [AttributeUsage(AttributeTargets.All)]" to "[AttributeUsage(AttributeTargets.All)]"
                // Roslyn's parser doesn't care whether the attribute is on a valid member, at least for formatting purposes
                _builder.AppendLine(attribute.GetContent());
                return CreateLineInfo(
                    processFormatting: true,
                    originOffset: attribute.SpanStart - _currentToken.Position,
                    formattedOffset: 0);
            }

            private int GetLineNumber(RazorSyntaxNode node)
                => _sourceText.Lines.GetLineFromPosition(node.Position).LineNumber;

            private int GetLineNumber(RazorSyntaxToken token)
                => _sourceText.Lines.GetLineFromPosition(token.Position).LineNumber;

            private LineInfo EmitCurrentLineAsCSharp()
            {
                _builder.AppendLine(_currentLine.ToString());
                return CreateLineInfo(processFormatting: true, checkForNewLines: true);
            }

            private LineInfo EmitCurrentLineAsComment()
            {
#if DEBUG
                _builder.AppendLine($"// {_currentLine}");
#else
                _builder.AppendLine($"//");
#endif
                return CreateLineInfo();
            }

            private LineInfo EmitCurrentLineWithNoFormatting()
            {
                _builder.AppendLine();
                return CreateLineInfo(processIndentation: false);
            }

            private LineInfo CreateLineInfo(
                bool processIndentation = true,
                bool processFormatting = false,
                bool checkForNewLines = false,
                bool skipPreviousLine = false,
                bool skipNextLine = false,
                bool skipNextLineIfBrace = false,
                int htmlIndentLevel = 0,
                int originOffset = 0,
                int formattedLength = 0,
                int formattedOffset = 0,
                int formattedOffsetFromEndOfLine = 0,
                string? additionalIndentation = null)
            {
                // We want to honour the indentation that the Html formatter supplied, but annoyingly it only actually indents
                // the contents of elements, not anything which is not contained in an element. This makes sense from the point
                // of view of Html, as it would expect the <html> element to always be present, but that is not true in Razor.
                // So we have to check if we're inside an element before we record the indentation, otherwise we could be
                // recording incorrect information.
                if (additionalIndentation is null &&
                    htmlIndentLevel == 0 &&
                    _elementEndLine is { } endLine &&
                    endLine >= _currentLine.LineNumber)
                {
                    htmlIndentLevel = FormattingUtilities.GetIndentationLevel(_currentLine, _currentFirstNonWhitespacePosition, _insertSpaces, _tabSize, out additionalIndentation);
                }

                return new(
                    ProcessIndentation: processIndentation,
                    ProcessFormatting: processFormatting,
                    CheckForNewLines: checkForNewLines,
                    SkipPreviousLine: skipPreviousLine,
                    SkipNextLine: skipNextLine,
                    SkipNextLineIfBrace: skipNextLineIfBrace,
                    HtmlIndentLevel: htmlIndentLevel,
                    OriginOffset: originOffset,
                    FormattedLength: formattedLength,
                    FormattedOffset: formattedOffset,
                    FormattedOffsetFromEndOfLine: formattedOffsetFromEndOfLine,
                    AdditionalIndentation: additionalIndentation);
            }
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class TagHelperSemanticRangeVisitor : SyntaxWalker
{
    private readonly List<SemanticRange> _semanticRanges;
    private readonly RazorCodeDocument _razorCodeDocument;

    private TagHelperSemanticRangeVisitor(RazorCodeDocument razorCodeDocument, TextSpan? range) : base(range)
    {
        _semanticRanges = new List<SemanticRange>();
        _razorCodeDocument = razorCodeDocument;
    }

    public static IReadOnlyList<SemanticRange> VisitAllNodes(RazorCodeDocument razorCodeDocument, Range? range = null)
    {
        TextSpan? rangeAsTextSpan = null;
        if (range is not null)
        {
            var sourceText = razorCodeDocument.GetSourceText();
            rangeAsTextSpan = range.AsRazorTextSpan(sourceText);
        }

        var visitor = new TagHelperSemanticRangeVisitor(razorCodeDocument, rangeAsTextSpan);

        visitor.Visit(razorCodeDocument.GetSyntaxTree().Root);

        return visitor._semanticRanges;
    }

    private void Visit(SyntaxList<RazorSyntaxNode> syntaxNodes)
    {
        for (var i = 0; i < syntaxNodes.Count; i++)
        {
            Visit(syntaxNodes[i]);
        }
    }

    #region HTML
    public override void VisitMarkupTextLiteral(MarkupTextLiteralSyntax node)
    {
        // Don't return anything for MarkupTextLiterals. It translates to "text" on the VS side, which is the default color anyway
    }

    public override void VisitMarkupLiteralAttributeValue(MarkupLiteralAttributeValueSyntax node)
    {
        AddSemanticRange(node, RazorSemanticTokensLegend.MarkupAttributeQuote);
    }

    public override void VisitMarkupAttributeBlock(MarkupAttributeBlockSyntax node)
    {
        Visit(node.NamePrefix);
        AddSemanticRange(node.Name, RazorSemanticTokensLegend.MarkupAttribute);
        Visit(node.NameSuffix);
        AddSemanticRange(node.EqualsToken, RazorSemanticTokensLegend.MarkupOperator);

        AddSemanticRange(node.ValuePrefix, RazorSemanticTokensLegend.MarkupAttributeQuote);
        Visit(node.Value);
        AddSemanticRange(node.ValueSuffix, RazorSemanticTokensLegend.MarkupAttributeQuote);
    }

    public override void VisitMarkupStartTag(MarkupStartTagSyntax node)
    {
        if (node.IsMarkupTransition)
        {
            AddSemanticRange(node, RazorSemanticTokensLegend.RazorDirective);
        }
        else
        {
            AddSemanticRange(node.OpenAngle, RazorSemanticTokensLegend.MarkupTagDelimiter);
            if (node.Bang != null)
            {
                AddSemanticRange(node.Bang, RazorSemanticTokensLegend.RazorTransition);
            }

            AddSemanticRange(node.Name, RazorSemanticTokensLegend.MarkupElement);

            Visit(node.Attributes);
            if (node.ForwardSlash != null)
            {
                AddSemanticRange(node.ForwardSlash, RazorSemanticTokensLegend.MarkupTagDelimiter);
            }

            AddSemanticRange(node.CloseAngle, RazorSemanticTokensLegend.MarkupTagDelimiter);
        }
    }

    public override void VisitMarkupEndTag(MarkupEndTagSyntax node)
    {
        if (node.IsMarkupTransition)
        {
            AddSemanticRange(node, RazorSemanticTokensLegend.RazorDirective);
        }
        else
        {
            AddSemanticRange(node.OpenAngle, RazorSemanticTokensLegend.MarkupTagDelimiter);
            if (node.Bang != null)
            {
                AddSemanticRange(node.Bang, RazorSemanticTokensLegend.RazorTransition);
            }

            if (node.ForwardSlash != null)
            {
                AddSemanticRange(node.ForwardSlash, RazorSemanticTokensLegend.MarkupTagDelimiter);
            }

            AddSemanticRange(node.Name, RazorSemanticTokensLegend.MarkupElement);

            AddSemanticRange(node.CloseAngle, RazorSemanticTokensLegend.MarkupTagDelimiter);
        }
    }

    public override void VisitMarkupCommentBlock(MarkupCommentBlockSyntax node)
    {
        AddSemanticRange(node.Children[0], RazorSemanticTokensLegend.MarkupCommentPunctuation);

        for (var i = 1; i < node.Children.Count - 1; i++)
        {
            var commentNode = node.Children[i];
            switch (commentNode.Kind)
            {
                case SyntaxKind.MarkupTextLiteral:
                    AddSemanticRange(commentNode, RazorSemanticTokensLegend.MarkupComment);
                    break;
                default:
                    Visit(commentNode);
                    break;
            }
        }

        AddSemanticRange(node.Children[node.Children.Count - 1], RazorSemanticTokensLegend.MarkupCommentPunctuation);
    }

    public override void VisitMarkupMinimizedAttributeBlock(MarkupMinimizedAttributeBlockSyntax node)
    {
        Visit(node.NamePrefix);
        AddSemanticRange(node.Name, RazorSemanticTokensLegend.MarkupAttribute);
    }
    #endregion HTML

    #region C#

    public override void VisitCSharpStatementBody(CSharpStatementBodySyntax node)
    {
        AddSemanticRange(node.OpenBrace, RazorSemanticTokensLegend.RazorTransition);
        Visit(node.CSharpCode);
        AddSemanticRange(node.CloseBrace, RazorSemanticTokensLegend.RazorTransition);
    }

    public override void VisitCSharpExplicitExpressionBody(CSharpExplicitExpressionBodySyntax node)
    {
        AddSemanticRange(node.OpenParen, RazorSemanticTokensLegend.RazorTransition);
        Visit(node.CSharpCode);
        AddSemanticRange(node.CloseParen, RazorSemanticTokensLegend.RazorTransition);
    }
    #endregion C#

    #region Razor
    public override void VisitRazorCommentBlock(RazorCommentBlockSyntax node)
    {
        AddSemanticRange(node.StartCommentTransition, RazorSemanticTokensLegend.RazorCommentTransition);
        AddSemanticRange(node.StartCommentStar, RazorSemanticTokensLegend.RazorCommentStar);
        AddSemanticRange(node.Comment, RazorSemanticTokensLegend.RazorComment);
        AddSemanticRange(node.EndCommentStar, RazorSemanticTokensLegend.RazorCommentStar);
        AddSemanticRange(node.EndCommentTransition, RazorSemanticTokensLegend.RazorCommentTransition);
    }

    public override void VisitRazorMetaCode(RazorMetaCodeSyntax node)
    {
        if (node.Kind == SyntaxKind.RazorMetaCode)
        {
            AddSemanticRange(node, RazorSemanticTokensLegend.RazorTransition);
        }
        else
        {
            throw new NotSupportedException(SR.Unknown_RazorMetaCode);
        }
    }

    public override void VisitRazorDirectiveBody(RazorDirectiveBodySyntax node)
    {
        // We can't provide colors for CSharp because if we both provided them then they would overlap, which violates the LSP spec.
        if (node.Keyword.Kind != SyntaxKind.CSharpStatementLiteral)
        {
            AddSemanticRange(node.Keyword, RazorSemanticTokensLegend.RazorDirective);
        }
        else
        {
            Visit(node.Keyword);
        }

        Visit(node.CSharpCode);
    }

    public override void VisitMarkupTagHelperStartTag(MarkupTagHelperStartTagSyntax node)
    {
        AddSemanticRange(node.OpenAngle, RazorSemanticTokensLegend.MarkupTagDelimiter);
        if (node.Bang != null)
        {
            AddSemanticRange(node.Bang, RazorSemanticTokensLegend.RazorTransition);
        }

        if (ClassifyTagName((MarkupTagHelperElementSyntax)node.Parent))
        {
            var semanticKind = GetElementSemanticKind(node);
            AddSemanticRange(node.Name, semanticKind);
        }
        else
        {
            AddSemanticRange(node.Name, RazorSemanticTokensLegend.MarkupElement);
        }

        Visit(node.Attributes);

        if (node.ForwardSlash != null)
        {
            AddSemanticRange(node.ForwardSlash, RazorSemanticTokensLegend.MarkupTagDelimiter);
        }

        AddSemanticRange(node.CloseAngle, RazorSemanticTokensLegend.MarkupTagDelimiter);
    }

    public override void VisitMarkupTagHelperEndTag(MarkupTagHelperEndTagSyntax node)
    {
        AddSemanticRange(node.OpenAngle, RazorSemanticTokensLegend.MarkupTagDelimiter);
        AddSemanticRange(node.ForwardSlash, RazorSemanticTokensLegend.MarkupTagDelimiter);

        if (node.Bang != null)
        {
            AddSemanticRange(node.Bang, RazorSemanticTokensLegend.RazorTransition);
        }

        if (ClassifyTagName((MarkupTagHelperElementSyntax)node.Parent))
        {
            var semanticKind = GetElementSemanticKind(node);
            AddSemanticRange(node.Name, semanticKind);
        }
        else
        {
            AddSemanticRange(node.Name, RazorSemanticTokensLegend.MarkupElement);
        }

        AddSemanticRange(node.CloseAngle, RazorSemanticTokensLegend.MarkupTagDelimiter);
    }

    public override void VisitMarkupMinimizedTagHelperAttribute(MarkupMinimizedTagHelperAttributeSyntax node)
    {
        Visit(node.NamePrefix);

        if (node.TagHelperAttributeInfo.Bound)
        {
            var semanticKind = GetAttributeSemanticKind(node);
            AddSemanticRange(node.Name, semanticKind);
        }
        else
        {
            AddSemanticRange(node.Name, RazorSemanticTokensLegend.MarkupAttribute);
        }
    }

    public override void VisitMarkupTagHelperAttribute(MarkupTagHelperAttributeSyntax node)
    {
        Visit(node.NamePrefix);
        if (node.TagHelperAttributeInfo.Bound)
        {
            var semanticKind = GetAttributeSemanticKind(node);
            AddSemanticRange(node.Name, semanticKind);
        }
        else
        {
            AddSemanticRange(node.Name, RazorSemanticTokensLegend.MarkupAttribute);
        }

        Visit(node.NameSuffix);

        AddSemanticRange(node.EqualsToken, RazorSemanticTokensLegend.MarkupOperator);

        AddSemanticRange(node.ValuePrefix, RazorSemanticTokensLegend.MarkupAttributeQuote);
        Visit(node.Value);
        AddSemanticRange(node.ValueSuffix, RazorSemanticTokensLegend.MarkupAttributeQuote);
    }

    public override void VisitMarkupTagHelperAttributeValue(MarkupTagHelperAttributeValueSyntax node)
    {
        foreach (var child in node.Children)
        {
            if (child.Kind == SyntaxKind.MarkupTextLiteral)
            {
                AddSemanticRange(child, RazorSemanticTokensLegend.MarkupAttributeQuote);
            }
            else
            {
                Visit(child);
            }
        }
    }

    public override void VisitMarkupTagHelperDirectiveAttribute(MarkupTagHelperDirectiveAttributeSyntax node)
    {
        if (node.TagHelperAttributeInfo.Bound)
        {
            Visit(node.Transition);
            Visit(node.NamePrefix);
            AddSemanticRange(node.Name, RazorSemanticTokensLegend.RazorDirectiveAttribute);
            Visit(node.NameSuffix);

            if (node.Colon != null)
            {
                AddSemanticRange(node.Colon, RazorSemanticTokensLegend.RazorDirectiveColon);
            }

            if (node.ParameterName != null)
            {
                AddSemanticRange(node.ParameterName, RazorSemanticTokensLegend.RazorDirectiveAttribute);
            }
        }

        AddSemanticRange(node.EqualsToken, RazorSemanticTokensLegend.MarkupOperator);
        AddSemanticRange(node.ValuePrefix, RazorSemanticTokensLegend.MarkupAttributeQuote);
        Visit(node.Value);
        AddSemanticRange(node.ValueSuffix, RazorSemanticTokensLegend.MarkupAttributeQuote);
    }

    public override void VisitMarkupMinimizedTagHelperDirectiveAttribute(MarkupMinimizedTagHelperDirectiveAttributeSyntax node)
    {
        if (node.TagHelperAttributeInfo.Bound)
        {
            AddSemanticRange(node.Transition, RazorSemanticTokensLegend.RazorTransition);
            Visit(node.NamePrefix);
            AddSemanticRange(node.Name, RazorSemanticTokensLegend.RazorDirectiveAttribute);

            if (node.Colon != null)
            {
                AddSemanticRange(node.Colon, RazorSemanticTokensLegend.RazorDirectiveColon);
            }

            if (node.ParameterName != null)
            {
                AddSemanticRange(node.ParameterName, RazorSemanticTokensLegend.RazorDirectiveAttribute);
            }
        }
    }

    public override void VisitCSharpTransition(CSharpTransitionSyntax node)
    {
        AddSemanticRange(node, RazorSemanticTokensLegend.RazorTransition);
    }

    public override void VisitMarkupTransition(MarkupTransitionSyntax node)
    {
        AddSemanticRange(node, RazorSemanticTokensLegend.RazorTransition);
    }
    #endregion Razor

    private static int GetElementSemanticKind(SyntaxNode node)
    {
        var semanticKind = IsComponent(node) ? RazorSemanticTokensLegend.RazorComponentElement : RazorSemanticTokensLegend.RazorTagHelperElement;
        return semanticKind;
    }

    private static int GetAttributeSemanticKind(SyntaxNode node)
    {
        var semanticKind = IsComponent(node) ? RazorSemanticTokensLegend.RazorComponentAttribute : RazorSemanticTokensLegend.RazorTagHelperAttribute;
        return semanticKind;
    }

    private static bool IsComponent(SyntaxNode node)
    {
        if (node is MarkupTagHelperElementSyntax element)
        {
            var componentDescriptor = element.TagHelperInfo.BindingResult.Descriptors.FirstOrDefault(d => d.IsComponentTagHelper());
            return componentDescriptor is not null;
        }
        else if (node is MarkupTagHelperStartTagSyntax startTag)
        {
            return IsComponent(startTag.Parent);
        }
        else if (node is MarkupTagHelperEndTagSyntax endTag)
        {
            return IsComponent(endTag.Parent);
        }
        else if (node is MarkupTagHelperAttributeSyntax attribute)
        {
            return IsComponent(attribute.Parent.Parent);
        }
        else if (node is MarkupMinimizedTagHelperAttributeSyntax minimizedTagHelperAttribute)
        {
            return IsComponent(minimizedTagHelperAttribute.Parent.Parent);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    // We don't want to classify TagNames of well-known HTML
    // elements as TagHelpers (even if they are). So the 'input' in`<input @onclick='...' />`
    // needs to not be marked as a TagHelper, but `<Input @onclick='...' />` should be.
    private static bool ClassifyTagName(MarkupTagHelperElementSyntax node)
    {
        if (node is null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (node.StartTag != null && node.StartTag.Name != null)
        {
            var binding = node.TagHelperInfo.BindingResult;
            return !binding.IsAttributeMatch;
        }

        return false;
    }

    private void AddSemanticRange(SyntaxNode node, int semanticKind)
    {
        if (node is null)
        {
            // This can happen in situations like "<p class='", where the trailing ' hasn't been typed yet.
            return;
        }

        if (node.Width == 0)
        {
            // Under no circumstances can we have 0-width spans.
            // This can happen in situations like "@* comment ", where EndCommentStar and EndCommentTransition are empty.
            return;
        }

        var source = _razorCodeDocument.Source;
        var range = node.GetRange(source);

        // LSP spec forbids multi-line tokens, so we need to split this up.
        if (range.Start.Line != range.End.Line)
        {
            var childNodes = node.ChildNodes();
            if (childNodes.Count == 0)
            {
                var content = node.GetContent();
                var lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                var charPosition = range.Start.Character;
                for (var i = 0; i < lines.Length; i++)
                {
                    var startPosition = new Position(range.Start.Line + i, charPosition);
                    var endPosition = new Position(range.Start.Line + i, charPosition + lines[i].Length);
                    var lineRange = new Range
                    {
                        Start = startPosition,
                        End = endPosition
                    };
                    var semantic = new SemanticRange(semanticKind, lineRange, modifier: 0);
                    AddRange(semantic);
                    charPosition = 0;
                }
            }
            else
            {
                // We have to iterate over the individual nodes because this node might consist of multiple lines
                // ie: "/r/ntext/r/n" would be parsed as one node containing three elements (newline, "text", newline).
                foreach (var token in node.ChildNodes())
                {
                    // We skip whitespace to avoid "multiline" ranges for "/r/n", where the /n is interpreted as being on a new line.
                    // This also stops us from returning data for " ", which seems like a nice side-effect as it's not likly to have any colorization anyway.
                    if (!token.ContainsOnlyWhitespace())
                    {
                        var tokenRange = token.GetRange(source);

                        var semantic = new SemanticRange(semanticKind, tokenRange, modifier: 0);
                        AddRange(semantic);
                    }
                }
            }
        }
        else
        {
            var semanticRange = new SemanticRange(semanticKind, range, modifier: 0);
            AddRange(semanticRange);
        }

        void AddRange(SemanticRange semanticRange)
        {
            if (semanticRange.Range.Start != semanticRange.Range.End)
            {
                _semanticRanges.Add(semanticRange);
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

internal class ClassifiedSpanVisitor : SyntaxWalker
{
    private readonly RazorSourceDocument _source;
    private readonly ImmutableArray<ClassifiedSpanInternal>.Builder _spans;

    private readonly Action<CSharpCodeBlockSyntax> _baseVisitCSharpCodeBlock;
    private readonly Action<CSharpStatementSyntax> _baseVisitCSharpStatement;
    private readonly Action<CSharpExplicitExpressionSyntax> _baseVisitCSharpExplicitExpression;
    private readonly Action<CSharpImplicitExpressionSyntax> _baseVisitCSharpImplicitExpression;
    private readonly Action<RazorDirectiveSyntax> _baseVisitRazorDirective;
    private readonly Action<CSharpTemplateBlockSyntax> _baseVisitCSharpTemplateBlock;
    private readonly Action<MarkupBlockSyntax> _baseVisitMarkupBlock;
    private readonly Action<MarkupTagHelperAttributeValueSyntax> _baseVisitMarkupTagHelperAttributeValue;
    private readonly Action<MarkupTagHelperElementSyntax> _baseVisitMarkupTagHelperElement;
    private readonly Action<MarkupCommentBlockSyntax> _baseVisitMarkupCommentBlock;
    private readonly Action<MarkupDynamicAttributeValueSyntax> _baseVisitMarkupDynamicAttributeValue;

    private BlockKindInternal _currentBlockKind;
    private SyntaxNode? _currentBlock;

    private ClassifiedSpanVisitor(RazorSourceDocument source, ImmutableArray<ClassifiedSpanInternal>.Builder spans)
    {
        _source = source;
        _spans = spans;

        _baseVisitCSharpCodeBlock = base.VisitCSharpCodeBlock;
        _baseVisitCSharpStatement = base.VisitCSharpStatement;
        _baseVisitCSharpExplicitExpression = base.VisitCSharpExplicitExpression;
        _baseVisitCSharpImplicitExpression = base.VisitCSharpImplicitExpression;
        _baseVisitRazorDirective = base.VisitRazorDirective;
        _baseVisitCSharpTemplateBlock = base.VisitCSharpTemplateBlock;
        _baseVisitMarkupBlock = base.VisitMarkupBlock;
        _baseVisitMarkupTagHelperAttributeValue = base.VisitMarkupTagHelperAttributeValue;
        _baseVisitMarkupTagHelperElement = base.VisitMarkupTagHelperElement;
        _baseVisitMarkupCommentBlock = base.VisitMarkupCommentBlock;
        _baseVisitMarkupDynamicAttributeValue = base.VisitMarkupDynamicAttributeValue;

        _currentBlockKind = BlockKindInternal.Markup;
    }

    public static ImmutableArray<ClassifiedSpanInternal> VisitRoot(RazorSyntaxTree syntaxTree)
    {
        using var _ = ArrayBuilderPool<ClassifiedSpanInternal>.GetPooledObject(out var builder);

        var visitor = new ClassifiedSpanVisitor(syntaxTree.Source, builder);
        visitor.Visit(syntaxTree.Root);

        return builder.DrainToImmutable();
    }

    public override void VisitRazorCommentBlock(RazorCommentBlockSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Comment, razorCommentSyntax =>
        {
            WriteSpan(razorCommentSyntax.StartCommentTransition, SpanKindInternal.Transition, AcceptedCharactersInternal.None);
            WriteSpan(razorCommentSyntax.StartCommentStar, SpanKindInternal.MetaCode, AcceptedCharactersInternal.None);

            var comment = razorCommentSyntax.Comment;
            if (comment.IsMissing)
            {
                    // We need to generate a classified span at this position. So insert a marker in its place.
                    comment = (SyntaxToken)SyntaxFactory.Token(SyntaxKind.Marker, string.Empty).Green.CreateRed(razorCommentSyntax, razorCommentSyntax.StartCommentStar.EndPosition);
            }
            WriteSpan(comment, SpanKindInternal.Comment, AcceptedCharactersInternal.Any);

            WriteSpan(razorCommentSyntax.EndCommentStar, SpanKindInternal.MetaCode, AcceptedCharactersInternal.None);
            WriteSpan(razorCommentSyntax.EndCommentTransition, SpanKindInternal.Transition, AcceptedCharactersInternal.None);
        });
    }

    public override void VisitCSharpCodeBlock(CSharpCodeBlockSyntax node)
    {
        if (node.Parent is CSharpStatementBodySyntax ||
            node.Parent is CSharpExplicitExpressionBodySyntax ||
            node.Parent is CSharpImplicitExpressionBodySyntax ||
            node.Parent is RazorDirectiveBodySyntax ||
            (_currentBlockKind == BlockKindInternal.Directive &&
            node.Children.Count == 1 &&
            node.Children[0] is CSharpStatementLiteralSyntax))
        {
            base.VisitCSharpCodeBlock(node);
            return;
        }

        WriteBlock(node, BlockKindInternal.Statement, _baseVisitCSharpCodeBlock);
    }

    public override void VisitCSharpStatement(CSharpStatementSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Statement, _baseVisitCSharpStatement);
    }

    public override void VisitCSharpExplicitExpression(CSharpExplicitExpressionSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Expression, _baseVisitCSharpExplicitExpression);
    }

    public override void VisitCSharpImplicitExpression(CSharpImplicitExpressionSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Expression, _baseVisitCSharpImplicitExpression);
    }

    public override void VisitRazorDirective(RazorDirectiveSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Directive, _baseVisitRazorDirective);
    }

    public override void VisitCSharpTemplateBlock(CSharpTemplateBlockSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Template, _baseVisitCSharpTemplateBlock);
    }

    public override void VisitMarkupBlock(MarkupBlockSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Markup, _baseVisitMarkupBlock);
    }

    public override void VisitMarkupTagHelperAttributeValue(MarkupTagHelperAttributeValueSyntax node)
    {
        // We don't generate a classified span when the attribute value is a simple literal value.
        // This is done so we maintain the classified spans generated in 2.x which
        // used ConditionalAttributeCollapser (combines markup literal attribute values into one span with no block parent).
        if (node.Children.Count > 1 ||
            (node.Children.Count == 1 && node.Children[0] is MarkupDynamicAttributeValueSyntax))
        {
            WriteBlock(node, BlockKindInternal.Markup, _baseVisitMarkupTagHelperAttributeValue);
            return;
        }

        base.VisitMarkupTagHelperAttributeValue(node);
    }

    public override void VisitMarkupStartTag(MarkupStartTagSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Tag, n =>
        {
            var children = GetRewrittenMarkupStartTagChildren(node);
            foreach (var child in children)
            {
                Visit(child);
            }
        });
    }

    public override void VisitMarkupEndTag(MarkupEndTagSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Tag, n =>
        {
            var children = GetRewrittenMarkupEndTagChildren(node);
            foreach (var child in children)
            {
                Visit(child);
            }
        });
    }

    public override void VisitMarkupTagHelperElement(MarkupTagHelperElementSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Tag, _baseVisitMarkupTagHelperElement);
    }

    public override void VisitMarkupTagHelperStartTag(MarkupTagHelperStartTagSyntax node)
    {
        foreach (var child in node.Attributes)
        {
            if (child is MarkupTagHelperAttributeSyntax ||
                child is MarkupTagHelperDirectiveAttributeSyntax ||
                child is MarkupMinimizedTagHelperDirectiveAttributeSyntax)
            {
                Visit(child);
            }
        }
    }

    public override void VisitMarkupTagHelperEndTag(MarkupTagHelperEndTagSyntax node)
    {
        // We don't want to generate a classified span for a tag helper end tag. Do nothing.
    }

    public override void VisitMarkupAttributeBlock(MarkupAttributeBlockSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Markup, n =>
        {
            var equalsSyntax = SyntaxFactory.MarkupTextLiteral(new SyntaxList<SyntaxToken>(node.EqualsToken), chunkGenerator: null);
            var mergedAttributePrefix = SyntaxUtilities.MergeTextLiterals(node.NamePrefix, node.Name, node.NameSuffix, equalsSyntax, node.ValuePrefix);
            Visit(mergedAttributePrefix);
            Visit(node.Value);
            Visit(node.ValueSuffix);
        });
    }

    public override void VisitMarkupTagHelperAttribute(MarkupTagHelperAttributeSyntax node)
    {
        Visit(node.Value);
    }

    public override void VisitMarkupTagHelperDirectiveAttribute(MarkupTagHelperDirectiveAttributeSyntax node)
    {
        Visit(node.Transition);
        Visit(node.Colon);
        Visit(node.Value);
    }

    public override void VisitMarkupMinimizedTagHelperDirectiveAttribute(MarkupMinimizedTagHelperDirectiveAttributeSyntax node)
    {
        Visit(node.Transition);
        Visit(node.Colon);
    }

    public override void VisitMarkupMinimizedAttributeBlock(MarkupMinimizedAttributeBlockSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Markup, n =>
        {
            var mergedAttributePrefix = SyntaxUtilities.MergeTextLiterals(node.NamePrefix, node.Name);
            Visit(mergedAttributePrefix);
        });
    }

    public override void VisitMarkupCommentBlock(MarkupCommentBlockSyntax node)
    {
        WriteBlock(node, BlockKindInternal.HtmlComment, _baseVisitMarkupCommentBlock);
    }

    public override void VisitMarkupDynamicAttributeValue(MarkupDynamicAttributeValueSyntax node)
    {
        WriteBlock(node, BlockKindInternal.Markup, _baseVisitMarkupDynamicAttributeValue);
    }

    public override void VisitRazorMetaCode(RazorMetaCodeSyntax node)
    {
        WriteSpan(node, SpanKindInternal.MetaCode);
        base.VisitRazorMetaCode(node);
    }

    public override void VisitCSharpTransition(CSharpTransitionSyntax node)
    {
        WriteSpan(node, SpanKindInternal.Transition);
        base.VisitCSharpTransition(node);
    }

    public override void VisitMarkupTransition(MarkupTransitionSyntax node)
    {
        WriteSpan(node, SpanKindInternal.Transition);
        base.VisitMarkupTransition(node);
    }

    public override void VisitCSharpStatementLiteral(CSharpStatementLiteralSyntax node)
    {
        WriteSpan(node, SpanKindInternal.Code);
        base.VisitCSharpStatementLiteral(node);
    }

    public override void VisitCSharpExpressionLiteral(CSharpExpressionLiteralSyntax node)
    {
        WriteSpan(node, SpanKindInternal.Code);
        base.VisitCSharpExpressionLiteral(node);
    }

    public override void VisitCSharpEphemeralTextLiteral(CSharpEphemeralTextLiteralSyntax node)
    {
        WriteSpan(node, SpanKindInternal.Code);
        base.VisitCSharpEphemeralTextLiteral(node);
    }

    public override void VisitUnclassifiedTextLiteral(UnclassifiedTextLiteralSyntax node)
    {
        WriteSpan(node, SpanKindInternal.None);
        base.VisitUnclassifiedTextLiteral(node);
    }

    public override void VisitMarkupLiteralAttributeValue(MarkupLiteralAttributeValueSyntax node)
    {
        WriteSpan(node, SpanKindInternal.Markup);
        base.VisitMarkupLiteralAttributeValue(node);
    }

    public override void VisitMarkupTextLiteral(MarkupTextLiteralSyntax node)
    {
        if (node.Parent is MarkupLiteralAttributeValueSyntax)
        {
            base.VisitMarkupTextLiteral(node);
            return;
        }

        WriteSpan(node, SpanKindInternal.Markup);
        base.VisitMarkupTextLiteral(node);
    }

    public override void VisitMarkupEphemeralTextLiteral(MarkupEphemeralTextLiteralSyntax node)
    {
        WriteSpan(node, SpanKindInternal.Markup);
        base.VisitMarkupEphemeralTextLiteral(node);
    }

    private void WriteBlock<TNode>(TNode node, BlockKindInternal kind, Action<TNode> handler) where TNode : SyntaxNode
    {
        var previousBlock = _currentBlock;
        var previousKind = _currentBlockKind;

        _currentBlock = node;
        _currentBlockKind = kind;

        handler(node);

        _currentBlock = previousBlock;
        _currentBlockKind = previousKind;
    }

    private void WriteSpan(SyntaxNode node, SpanKindInternal kind, AcceptedCharactersInternal? acceptedCharacters = null)
    {
        if (node.IsMissing)
        {
            return;
        }

        var spanSource = node.GetSourceSpan(_source);
        var blockSource = _currentBlock.GetSourceSpan(_source);
        if (!acceptedCharacters.HasValue)
        {
            acceptedCharacters = AcceptedCharactersInternal.Any;
            var context = node.GetEditHandler();
            if (context != null)
            {
                acceptedCharacters = context.AcceptedCharacters;
            }
        }

        var span = new ClassifiedSpanInternal(spanSource, blockSource, kind, _currentBlockKind, acceptedCharacters.Value);
        _spans.Add(span);
    }

    private static SyntaxList<RazorSyntaxNode> GetRewrittenMarkupStartTagChildren(MarkupStartTagSyntax node)
    {
        // Rewrites the children of the start tag to look like the legacy syntax tree.
        if (node.IsMarkupTransition)
        {
            var tokens = node.DescendantNodes().Where(n => n is SyntaxToken token && !token.IsMissing).Cast<SyntaxToken>().ToArray();
            var tokenBuilder = SyntaxListBuilder<SyntaxToken>.Create();
            tokenBuilder.AddRange(tokens, 0, tokens.Length);
            var markupTransition = SyntaxFactory.MarkupTransition(tokenBuilder.ToList(), node.ChunkGenerator).Green.CreateRed(node, node.Position);
            var editHandler = node.GetEditHandler();
            if (editHandler != null)
            {
                markupTransition = markupTransition.WithEditHandler(editHandler);
            }

            var builder = new SyntaxListBuilder(1);
            builder.Add(markupTransition);
            return new SyntaxList<RazorSyntaxNode>(builder.ToListNode().CreateRed(node, node.Position));
        }

        SpanEditHandler? latestEditHandler = null;
        var children = node.Children;
        var newChildren = new SyntaxListBuilder(children.Count);
        var literals = new List<MarkupTextLiteralSyntax>();
        foreach (var child in children)
        {
            if (child is MarkupTextLiteralSyntax literal)
            {
                literals.Add(literal);
                latestEditHandler = literal.GetEditHandler() ?? latestEditHandler;
            }
            else if (child is MarkupMiscAttributeContentSyntax miscContent)
            {
                foreach (var contentChild in miscContent.Children)
                {
                    if (contentChild is MarkupTextLiteralSyntax contentLiteral)
                    {
                        literals.Add(contentLiteral);
                        latestEditHandler = contentLiteral.GetEditHandler() ?? latestEditHandler;
                    }
                    else
                    {
                        // Pop stack
                        AddLiteralIfExists();
                        newChildren.Add(contentChild);
                    }
                }
            }
            else
            {
                AddLiteralIfExists();
                newChildren.Add(child);
            }
        }

        AddLiteralIfExists();

        return new SyntaxList<RazorSyntaxNode>(newChildren.ToListNode().CreateRed(node, node.Position));

        void AddLiteralIfExists()
        {
            if (literals.Count > 0)
            {
                var mergedLiteral = SyntaxUtilities.MergeTextLiterals(literals.ToArray());
                mergedLiteral = mergedLiteral.WithEditHandler(latestEditHandler);
                literals.Clear();
                latestEditHandler = null;
                newChildren.Add(mergedLiteral);
            }
        }
    }

    private static SyntaxList<RazorSyntaxNode> GetRewrittenMarkupEndTagChildren(MarkupEndTagSyntax node)
    {
        // Rewrites the children of the end tag to look like the legacy syntax tree.
        if (node.IsMarkupTransition)
        {
            var tokens = node.DescendantNodes().Where(n => n is SyntaxToken token && !token.IsMissing).Cast<SyntaxToken>().ToArray();
            var tokenBuilder = SyntaxListBuilder<SyntaxToken>.Create();
            tokenBuilder.AddRange(tokens, 0, tokens.Length);
            var markupTransition = SyntaxFactory.MarkupTransition(tokenBuilder.ToList(), node.ChunkGenerator).Green.CreateRed(node, node.Position);
            var editHandler = node.GetEditHandler();
            if (editHandler != null)
            {
                markupTransition = markupTransition.WithEditHandler(editHandler);
            }

            var builder = new SyntaxListBuilder(1);
            builder.Add(markupTransition);
            return new SyntaxList<RazorSyntaxNode>(builder.ToListNode().CreateRed(node, node.Position));
        }

        return node.Children;
    }
}

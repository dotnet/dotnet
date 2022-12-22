// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.AspNetCore.Razor.Language;

// We want to generate a HTML document that contains only pure HTML.
// So we want replace all non-HTML content with whitespace.
// Ideally we should just use ClassifiedSpans to generate this document but
// not all characters in the document are included in the ClassifiedSpans.
internal class RazorHtmlWriter : SyntaxWalker
{
    private readonly Action<RazorCommentBlockSyntax> _baseVisitRazorCommentBlock;
    private readonly Action<RazorMetaCodeSyntax> _baseVisitRazorMetaCode;
    private readonly Action<MarkupTransitionSyntax> _baseVisitMarkupTransition;
    private readonly Action<CSharpTransitionSyntax> _baseVisitCSharpTransition;
    private readonly Action<CSharpEphemeralTextLiteralSyntax> _baseVisitCSharpEphemeralTextLiteral;
    private readonly Action<CSharpExpressionLiteralSyntax> _baseVisitCSharpExpressionLiteral;
    private readonly Action<CSharpStatementLiteralSyntax> _baseVisitCSharpStatementLiteral;
    private readonly Action<MarkupStartTagSyntax> _baseVisitMarkupStartTag;
    private readonly Action<MarkupEndTagSyntax> _baseVisitMarkupEndTag;
    private readonly Action<MarkupTagHelperStartTagSyntax> _baseVisitMarkupTagHelperStartTag;
    private readonly Action<MarkupTagHelperEndTagSyntax> _baseVisitMarkupTagHelperEndTag;
    private readonly Action<MarkupEphemeralTextLiteralSyntax> _baseVisitMarkupEphemeralTextLiteral;
    private readonly Action<MarkupTextLiteralSyntax> _baseVisitMarkupTextLiteral;
    private readonly Action<UnclassifiedTextLiteralSyntax> _baseVisitUnclassifiedTextLiteral;

    private bool _isHtml;

    private RazorHtmlWriter(RazorSourceDocument source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        Source = source;
        Builder = new StringBuilder(Source.Length);
        _isHtml = true;

        _baseVisitRazorCommentBlock = base.VisitRazorCommentBlock;
        _baseVisitRazorMetaCode = base.VisitRazorMetaCode;
        _baseVisitMarkupTransition = base.VisitMarkupTransition;
        _baseVisitCSharpTransition = base.VisitCSharpTransition;
        _baseVisitCSharpEphemeralTextLiteral = base.VisitCSharpEphemeralTextLiteral;
        _baseVisitCSharpExpressionLiteral = base.VisitCSharpExpressionLiteral;
        _baseVisitCSharpStatementLiteral = base.VisitCSharpStatementLiteral;
        _baseVisitMarkupStartTag = base.VisitMarkupStartTag;
        _baseVisitMarkupEndTag = base.VisitMarkupEndTag;
        _baseVisitMarkupTagHelperStartTag = base.VisitMarkupTagHelperStartTag;
        _baseVisitMarkupTagHelperEndTag = base.VisitMarkupTagHelperEndTag;
        _baseVisitMarkupEphemeralTextLiteral = base.VisitMarkupEphemeralTextLiteral;
        _baseVisitMarkupTextLiteral = base.VisitMarkupTextLiteral;
        _baseVisitUnclassifiedTextLiteral = base.VisitUnclassifiedTextLiteral;
    }

    public RazorSourceDocument Source { get; }

    public StringBuilder Builder { get; }

    public static RazorHtmlDocument? GetHtmlDocument(RazorCodeDocument codeDocument)
    {
        var options = codeDocument.GetCodeGenerationOptions();
        if (options == null || !options.DesignTime)
        {
            // Not needed in run time. This pass generates the backing HTML document that is used to provide HTML intellisense.
            return null;
        }

        var writer = new RazorHtmlWriter(codeDocument.Source);
        var syntaxTree = codeDocument.GetSyntaxTree();

        writer.Visit(syntaxTree.Root);

        var generatedHtml = writer.Builder.ToString();
        Debug.Assert(
            writer.Source.Length == writer.Builder.Length,
            $"The backing HTML document should be the same length as the original document. Expected: {writer.Source.Length} Actual: {writer.Builder.Length}");

        var razorHtmlDocument = new DefaultRazorHtmlDocument(generatedHtml, options);
        return razorHtmlDocument;
    }

    public override void VisitRazorCommentBlock(RazorCommentBlockSyntax node)
    {
        WriteNode(node, isHtml: false, _baseVisitRazorCommentBlock);
    }

    public override void VisitRazorMetaCode(RazorMetaCodeSyntax node)
    {
        WriteNode(node, isHtml: false, _baseVisitRazorMetaCode);
    }

    public override void VisitMarkupTransition(MarkupTransitionSyntax node)
    {
        WriteNode(node, isHtml: false, _baseVisitMarkupTransition);
    }

    public override void VisitCSharpTransition(CSharpTransitionSyntax node)
    {
        WriteNode(node, isHtml: false, _baseVisitCSharpTransition);
    }

    public override void VisitCSharpEphemeralTextLiteral(CSharpEphemeralTextLiteralSyntax node)
    {
        WriteNode(node, isHtml: false, _baseVisitCSharpEphemeralTextLiteral);
    }

    public override void VisitCSharpExpressionLiteral(CSharpExpressionLiteralSyntax node)
    {
        WriteNode(node, isHtml: false, _baseVisitCSharpExpressionLiteral);
    }

    public override void VisitCSharpStatementLiteral(CSharpStatementLiteralSyntax node)
    {
        WriteNode(node, isHtml: false, _baseVisitCSharpStatementLiteral);
    }

    public override void VisitMarkupStartTag(MarkupStartTagSyntax node)
    {
        WriteNode(node, isHtml: true, _baseVisitMarkupStartTag);
    }

    public override void VisitMarkupEndTag(MarkupEndTagSyntax node)
    {
        WriteNode(node, isHtml: true, _baseVisitMarkupEndTag);
    }

    public override void VisitMarkupTagHelperStartTag(MarkupTagHelperStartTagSyntax node)
    {
        WriteNode(node, isHtml: true, _baseVisitMarkupTagHelperStartTag);
    }

    public override void VisitMarkupTagHelperEndTag(MarkupTagHelperEndTagSyntax node)
    {
        WriteNode(node, isHtml: true, _baseVisitMarkupTagHelperEndTag);
    }

    public override void VisitMarkupEphemeralTextLiteral(MarkupEphemeralTextLiteralSyntax node)
    {
        WriteNode(node, isHtml: true, _baseVisitMarkupEphemeralTextLiteral);
    }

    public override void VisitMarkupTextLiteral(MarkupTextLiteralSyntax node)
    {
        WriteNode(node, isHtml: true, _baseVisitMarkupTextLiteral);
    }

    public override void VisitUnclassifiedTextLiteral(UnclassifiedTextLiteralSyntax node)
    {
        WriteNode(node, isHtml: true, _baseVisitUnclassifiedTextLiteral);
    }

    public override void VisitToken(SyntaxToken token)
    {
        base.VisitToken(token);
        WriteToken(token);
    }

    private void WriteToken(SyntaxToken token)
    {
        var content = token.Content;
        if (_isHtml)
        {
            // If we're in HTML context, append the content directly.
            Builder.Append(content);
            return;
        }

        // We're in non-HTML context. Let's replace all non-whitespace chars with a tilde(~).
        foreach (var c in content)
        {
            if (char.IsWhiteSpace(c))
            {
                Builder.Append(c);
            }
            else
            {
                Builder.Append('~');
            }
        }
    }

    private void WriteNode<TNode>(TNode node, bool isHtml, Action<TNode> handler) where TNode : SyntaxNode
    {
        var old = _isHtml;
        _isHtml = isHtml;
        handler(node);
        _isHtml = old;
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

// We want to generate a HTML document that contains only pure HTML.
// So we want replace all non-HTML content with whitespace.
// Ideally we should just use ClassifiedSpans to generate this document but
// not all characters in the document are included in the ClassifiedSpans.
internal class RazorHtmlWriter : SyntaxWalker, IDisposable
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

    private readonly PooledObject<ImmutableArray<SourceMapping>.Builder> _sourceMappingsBuilder;

    private bool _isHtml;
    private SourceSpan _lastOriginalSourceSpan = SourceSpan.Undefined;
    private SourceSpan _lastGeneratedSourceSpan = SourceSpan.Undefined;

    // Rather than writing out C# characters as we find them (as '~') we keep a count so that consecutive characters
    // can be written as a block, allowing any block of 4 characters or more to be written as a comment (ie '/**/`)
    // which takes pressure off the TypeScript/JavaScript compiler. Doing this per token means we can end up with
    // "@className" being written as '~/*~~~~~*/', which means Html formatting will insert a space which breaks things.
    private int _csharpCharacterCount;

    private RazorHtmlWriter(RazorSourceDocument source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        Source = source;
        Builder = new CodeWriter();
        _sourceMappingsBuilder = ArrayBuilderPool<SourceMapping>.GetPooledObject();
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

    public CodeWriter Builder { get; }

    public ImmutableArray<SourceMapping>.Builder SourceMappings => _sourceMappingsBuilder.Object;

    public static RazorHtmlDocument GetHtmlDocument(RazorCodeDocument codeDocument)
    {
        using var writer = new RazorHtmlWriter(codeDocument.Source);
        var syntaxTree = codeDocument.GetSyntaxTree();

        writer.Visit(syntaxTree);

        Debug.Assert(
            writer.Source.Text.Length == writer.Builder.Length,
            $"The backing HTML document should be the same length as the original document. Expected: {writer.Source.Text.Length} Actual: {writer.Builder.Length}");
        var text = writer.Builder.GetText();

        return new RazorHtmlDocument(codeDocument, text, codeDocument.CodeGenerationOptions, writer.SourceMappings.DrainToImmutable());
    }

    public void Visit(RazorSyntaxTree syntaxTree)
    {
        Visit(syntaxTree.Root);

        WriteDeferredCSharpContent();

        if (_lastGeneratedSourceSpan != SourceSpan.Undefined)
        {
            // If we finished up with a source mapping being tracked, then add it to the list now

            Debug.Assert(_lastOriginalSourceSpan != SourceSpan.Undefined);

            var sourceMapping = new SourceMapping(_lastOriginalSourceSpan, _lastGeneratedSourceSpan);
            SourceMappings.Add(sourceMapping);

            _lastOriginalSourceSpan = SourceSpan.Undefined;
            _lastGeneratedSourceSpan = SourceSpan.Undefined;
        }
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
            WriteDeferredCSharpContent();

            var source = token.GetSourceSpan(Source);

            // No point source mapping an empty token
            if (source.Length > 0)
            {
                var generatedLocation = new SourceSpan(Builder.Location, source.Length);

                if (_lastGeneratedSourceSpan == SourceSpan.Undefined)
                {
                    // Not tracking any current source mapping, so start tracking one

                    Debug.Assert(_lastOriginalSourceSpan == SourceSpan.Undefined);

                    _lastGeneratedSourceSpan = generatedLocation;
                    _lastOriginalSourceSpan = source;
                }
                else if (generatedLocation.AbsoluteIndex == _lastGeneratedSourceSpan.AbsoluteIndex + _lastGeneratedSourceSpan.Length &&
                    source.AbsoluteIndex == _lastOriginalSourceSpan.AbsoluteIndex + _lastOriginalSourceSpan.Length &&
                    generatedLocation.LineCount <= 1 &&
                    source.LineCount <= 1)
                {
                    // We're tracking a span, and it ends at the same spot the current token starts, so lets just extend the existing
                    // source mapping we're tracking, so we produce a minimal set
                    // eg, in "<div>" there are three tokens that are written (open angle bracket, tag name, close angle bracket)
                    //     but having three source mappings in unnecessarily complex
                    _lastGeneratedSourceSpan = _lastGeneratedSourceSpan.With(length: _lastGeneratedSourceSpan.Length + source.Length, endCharacterIndex: source.EndCharacterIndex);
                    _lastOriginalSourceSpan = _lastOriginalSourceSpan.With(length: _lastOriginalSourceSpan.Length + source.Length, endCharacterIndex: source.EndCharacterIndex);
                }
                else
                {
                    // New span is not directly next to the previous one, so add the previous to the list, and start tracking the new one
                    var sourceMapping = new SourceMapping(_lastOriginalSourceSpan, _lastGeneratedSourceSpan);
                    SourceMappings.Add(sourceMapping);

                    _lastOriginalSourceSpan = source;
                    _lastGeneratedSourceSpan = generatedLocation;
                }
            }

            // If we're in HTML context, append the content directly.
            Builder.Write(content);
            return;
        }

        if (_lastGeneratedSourceSpan != SourceSpan.Undefined)
        {
            // If we were tracking a source mapping span before now, add it to the list. Importantly there are cases
            // where there are 0-length C# nodes, so this step is very important if the source mappings are to match
            // the syntax tree.

            Debug.Assert(_lastOriginalSourceSpan != SourceSpan.Undefined);

            var sourceMapping = new SourceMapping(_lastOriginalSourceSpan, _lastGeneratedSourceSpan);
            SourceMappings.Add(sourceMapping);

            _lastGeneratedSourceSpan = SourceSpan.Undefined;
            _lastOriginalSourceSpan = SourceSpan.Undefined;
        }

        // We're in non-HTML context. Let's replace all non-whitespace chars with a tilde(~).
        foreach (var c in content)
        {
            if (char.IsWhiteSpace(c))
            {
                WriteDeferredCSharpContent();
                Builder.Write(c.ToString());
            }
            else
            {
                _csharpCharacterCount++;
            }
        }
    }

    private void WriteDeferredCSharpContent()
    {
        if (_csharpCharacterCount == 0)
        {
            return;
        }

        Builder.Write(_csharpCharacterCount switch
        {
            // Less than 4 chars, just use tildes. We can't do anything more fancy in a small space
            1 => "~",
            2 => "~~",
            3 => "~~~",

            // Special case for unquoted attributes that appear at the end of a tag. eg `<div class=@className>`
            // Without this special handling, our replacement would result in `<div class=/*~~~~~*/>` which differs
            // from the original meaning, as the div is now self-closing.
            // Note that we don't actually know if we're in an attribute here, or some other construct, but false
            // positives are totally fine in this scenario.
            _ when NextCharacterIsGreaterThanSymbol() => new string('~', _csharpCharacterCount),

            // All other cases, use a comment to relieve pressure on the JS/TS compiler
            _ => "/*" + new string('~', _csharpCharacterCount - 4) + "*/",
        });
        _csharpCharacterCount = 0;

        bool NextCharacterIsGreaterThanSymbol()
        {
            var sourceText = Source.Text;
            var index = Builder.Location.AbsoluteIndex + _csharpCharacterCount;
            if (sourceText.Length <= index)
            {
                return false;
            }

            return sourceText[index] == '>';
        }
    }

    private void WriteNode<TNode>(TNode node, bool isHtml, Action<TNode> handler) where TNode : SyntaxNode
    {
        var old = _isHtml;
        _isHtml = isHtml;
        handler(node);
        _isHtml = old;
    }

    public void Dispose()
    {
        _sourceMappingsBuilder.Dispose();
        Builder.Dispose();
    }
}

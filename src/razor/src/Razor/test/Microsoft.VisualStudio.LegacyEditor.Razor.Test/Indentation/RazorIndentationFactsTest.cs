// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Indentation;

public class RazorIndentationFactsTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void GetPreviousLineEndIndex_ReturnsPreviousLine()
    {
        // Arrange
        var txt = @"@{
    <p>Hello World</p>
}";
        var textSnapshot = new StringTextSnapshot(txt);
        var line = textSnapshot.GetLineFromLineNumber(2);

        // Act
        var previousLineEndIndex = RazorIndentationFacts.GetPreviousLineEndIndex(textSnapshot, line);

        // Assert
        Assert.Equal(txt.IndexOf("</p>", StringComparison.Ordinal) + 2 + Environment.NewLine.Length, previousLineEndIndex);
    }

    [Fact]
    public void IsCSharpOpenCurlyBrace_SpanWithLeftBrace_ReturnTrue()
    {
        // Arrange
        using PooledArrayBuilder<SyntaxToken> builder = [];
        builder.Add(SyntaxFactory.Token(SyntaxKind.LeftBrace, "{"));
        var child = SyntaxFactory.RazorMetaCode(builder.ToList());

        // Act
        var result = RazorIndentationFacts.IsCSharpOpenCurlyBrace(child);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("if", SyntaxKind.Keyword)]
    [InlineData("}", SyntaxKind.RightBrace)]
    [InlineData("++", SyntaxKind.Increment)]
    [InlineData("text", SyntaxKind.Identifier)]
    public void IsCSharpOpenCurlyBrace_SpanWithUnsupportedSymbolType_ReturnFalse(string content, object symbolTypeObject)
    {
        // Arrange
        var symbolType = (SyntaxKind)symbolTypeObject;
        using PooledArrayBuilder<SyntaxToken> builder = [];
        builder.Add(SyntaxFactory.Token(symbolType, content));
        var child = SyntaxFactory.MarkupTextLiteral(builder.ToList());

        // Act
        var result = RazorIndentationFacts.IsCSharpOpenCurlyBrace(child);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsCSharpOpenCurlyBrace_MultipleSymbols_ReturnFalse()
    {
        // Arrange
        using PooledArrayBuilder<SyntaxToken> builder = [];
        builder.Add(SyntaxFactory.Token(SyntaxKind.Identifier, "hello"));
        builder.Add(SyntaxFactory.Token(SyntaxKind.Comma, ","));
        var child = SyntaxFactory.MarkupTextLiteral(builder.ToList());

        // Act
        var result = RazorIndentationFacts.IsCSharpOpenCurlyBrace(child);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsCSharpOpenCurlyBrace_SpanWithHtmlSymbol_ReturnFalse()
    {
        // Arrange
        using PooledArrayBuilder<SyntaxToken> builder = [];
        builder.Add(SyntaxFactory.Token(SyntaxKind.Text, "hello"));
        var child = SyntaxFactory.MarkupTextLiteral(builder.ToList());

        // Act
        var result = RazorIndentationFacts.IsCSharpOpenCurlyBrace(child);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsCSharpOpenCurlyBrace_Blocks_ReturnFalse()
    {
        // Arrange
        var child = SyntaxFactory.MarkupBlock();

        // Act
        var result = RazorIndentationFacts.IsCSharpOpenCurlyBrace(child);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetIndentLevelOfLine_AddsTabsOnlyAtBeginningOfLine()
    {
        // Arrange
        var text = "\t\tHello\tWorld.\t";

        // Act
        var indentLevel = RazorIndentationFacts.GetIndentLevelOfLine(text, 4);

        // Assert
        Assert.Equal(8, indentLevel);
    }

    [Fact]
    public void GetIndentLevelOfLine_AddsSpacesOnlyAtBeginningOfLine()
    {
        // Arrange
        var text = "   Hello World. ";

        // Act
        var indentLevel = RazorIndentationFacts.GetIndentLevelOfLine(text, 4);

        // Assert
        Assert.Equal(3, indentLevel);
    }

    [Fact]
    public void GetIndentLevelOfLine_AddsTabsAndSpacesOnlyAtBeginningOfLine()
    {
        // Arrange
        var text = "  \t \tHello\t World.\t ";

        // Act
        var indentLevel = RazorIndentationFacts.GetIndentLevelOfLine(text, 4);

        // Assert
        Assert.Equal(11, indentLevel);
    }

    [Fact]
    public void GetIndentLevelOfLine_NoIndent()
    {
        // Arrange
        var text = "Hello World.";

        // Act
        var indentLevel = RazorIndentationFacts.GetIndentLevelOfLine(text, 4);

        // Assert
        Assert.Equal(0, indentLevel);
    }

    // This test verifies that we still operate on SyntaxTree's that have gaps in them. The gaps are temporary
    // until our work with the parser has been completed.
    [Fact]
    public void GetDesiredIndentation_ReturnsNull_IfOwningSpanDoesNotExist()
    {
        // Arrange
        var source = new StringTextSnapshot($@"
<div>
    <div>
    </div>
</div>
");
        _ = new TestTextBuffer(source, VsMocks.ContentTypes.LegacyRazorCore);
        var syntaxTree = GetSyntaxTree(new StringTextSnapshot("something else"));

        // Act
        var indentation = RazorIndentationFacts.GetDesiredIndentation(
            syntaxTree,
            source,
            source.GetLineFromLineNumber(3),
            indentSize: 4,
            tabSize: 1);

        // Assert
        Assert.Null(indentation);
    }

    [Fact]
    public void GetDesiredIndentation_ReturnsNull_IfOwningSpanIsCode()
    {
        // Arrange
        var source = new StringTextSnapshot("""

            @{

            """);
        _ = new TestTextBuffer(source, VsMocks.ContentTypes.LegacyRazorCore);
        var syntaxTree = GetSyntaxTree(source);

        // Act
        var indentation = RazorIndentationFacts.GetDesiredIndentation(
            syntaxTree,
            source,
            source.GetLineFromLineNumber(2),
            indentSize: 4,
            tabSize: 1);

        // Assert
        Assert.Null(indentation);
    }

    [Fact]
    public void GetDesiredIndentation_ReturnsNull_IfOwningSpanIsNone()
    {
        // Arrange
        var customDirective = DirectiveDescriptor.CreateSingleLineDirective("custom");
        var source = new StringTextSnapshot($@"
@custom
");
        _ = new TestTextBuffer(source, VsMocks.ContentTypes.LegacyRazorCore);
        var syntaxTree = GetSyntaxTree(source, [customDirective]);

        // Act
        var indentation = RazorIndentationFacts.GetDesiredIndentation(
            syntaxTree,
            source,
            source.GetLineFromLineNumber(2),
            indentSize: 4,
            tabSize: 1);

        // Assert
        Assert.Null(indentation);
    }

    [Fact]
    public void GetDesiredIndentation_ReturnsCorrectIndentation_ForMarkupWithinCodeBlock()
    {
        // Arrange
        var source = new StringTextSnapshot("""
            @{
                <div>

            """);
        _ = new TestTextBuffer(source, VsMocks.ContentTypes.LegacyRazorCore);
        var syntaxTree = GetSyntaxTree(source);

        // Act
        var indentation = RazorIndentationFacts.GetDesiredIndentation(
            syntaxTree,
            source,
            source.GetLineFromLineNumber(2),
            indentSize: 4,
            tabSize: 4);

        // Assert
        Assert.Equal(4, indentation);
    }

    [Fact]
    public void GetDesiredIndentation_ReturnsCorrectIndentation_ForMarkupWithinDirectiveBlock()
    {
        // Arrange
        var customDirective = DirectiveDescriptor.CreateRazorBlockDirective("custom");
        var source = new StringTextSnapshot("""
            @custom
            {
                <div>
            }
            """);
        _ = new TestTextBuffer(source, VsMocks.ContentTypes.LegacyRazorCore);
        var syntaxTree = GetSyntaxTree(source, [customDirective]);

        // Act
        var indentation = RazorIndentationFacts.GetDesiredIndentation(
            syntaxTree,
            source,
            source.GetLineFromLineNumber(3),
            indentSize: 4,
            tabSize: 4);

        // Assert
        Assert.Equal(4, indentation);
    }

    [Fact]
    public void GetDesiredIndentation_ReturnsCorrectIndentation_ForNestedMarkupWithinCodeBlock()
    {
        // Arrange
        var source = new StringTextSnapshot("""
            <div>
                @{
                    <span>
                }
            </div>
            """);
        _ = new TestTextBuffer(source, VsMocks.ContentTypes.LegacyRazorCore);
        var syntaxTree = GetSyntaxTree(source);

        // Act
        var indentation = RazorIndentationFacts.GetDesiredIndentation(
            syntaxTree,
            source,
            source.GetLineFromLineNumber(4),
            indentSize: 4,
            tabSize: 4);

        // Assert
        Assert.Equal(8, indentation);
    }

    [Fact]
    public void GetDesiredIndentation_ReturnsCorrectIndentation_ForMarkupWithinCodeBlockInADirectiveBlock()
    {
        // Arrange
        var customDirective = DirectiveDescriptor.CreateRazorBlockDirective("custom");
        var source = new StringTextSnapshot("""
            @custom
            {
                @{
                    <div>
                }
            }
            """);
        _ = new TestTextBuffer(source, VsMocks.ContentTypes.LegacyRazorCore);
        var syntaxTree = GetSyntaxTree(source, [customDirective]);

        // Act
        var indentation = RazorIndentationFacts.GetDesiredIndentation(
            syntaxTree,
            source,
            source.GetLineFromLineNumber(4),
            indentSize: 4,
            tabSize: 4);

        // Assert
        Assert.Equal(8, indentation);
    }

    private static RazorSyntaxTree GetSyntaxTree(StringTextSnapshot source, IEnumerable<DirectiveDescriptor>? directives = null)
    {
        directives ??= [];
        var engine = RazorProjectEngine.Create(builder =>
        {
            foreach (var directive in directives)
            {
                builder.AddDirective(directive);
            }

            builder.ConfigureParserOptions(VisualStudioRazorParser.ConfigureParserOptions);
        });

        var sourceProjectItem = new TestRazorProjectItem("test.cshtml")
        {
            Content = source.GetText()
        };

        var codeDocument = engine.ProcessDesignTime(sourceProjectItem);

        return codeDocument.GetRequiredSyntaxTree();
    }
}

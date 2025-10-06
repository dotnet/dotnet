// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;
using Microsoft.CodeAnalysis.Razor.Completion.Delegation;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Completion.Delegation;

public class SnippetResponseRewriterTest(ITestOutputHelper testOutput) : ResponseRewriterTestBase(testOutput)
{
    [Fact]
    public async Task RewriteAsync_RemovesUsingSnippetLabel()
    {
        // Arrange
        var documentContent = "@$$";
        TestFileMarkupParser.GetPosition(documentContent, out documentContent, out var cursorPosition);
        var delegatedCompletionList = GenerateCompletionList(
            ("using", CompletionItemKind.Snippet),
            ("if", CompletionItemKind.Keyword)
            );
        var rewriter = new SnippetResponseRewriter();

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            cursorPosition, documentContent, delegatedCompletionList);

        // Assert
        Assert.NotNull(rewrittenCompletionList);
        Assert.Null(rewrittenCompletionList.CommitCharacters);
        Assert.Collection(
            rewrittenCompletionList.Items,
            completion =>
            {
                Assert.Equal("if", completion.Label);
                Assert.Equal("if", completion.SortText);
            }
        );
    }

    [Fact]
    public async Task RewriteAsync_DoesNotChangeUsingKeywordLabel()
    {
        // Arrange
        var documentContent = "@$$";
        TestFileMarkupParser.GetPosition(documentContent, out documentContent, out var cursorPosition);
        var delegatedCompletionList = GenerateCompletionList(
            ("using", CompletionItemKind.Keyword),
            ("if", CompletionItemKind.Keyword)
            );
        var rewriter = new SnippetResponseRewriter();

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            cursorPosition, documentContent, delegatedCompletionList);

        // Assert
        Assert.NotNull(rewrittenCompletionList);
        Assert.Null(rewrittenCompletionList.CommitCharacters);
        Assert.Collection(
            rewrittenCompletionList.Items,
            completion =>
            {
                Assert.Equal("using", completion.Label);
                Assert.Equal("using", completion.SortText);
            },
            completion =>
            {
                Assert.Equal("if", completion.Label);
                Assert.Equal("if", completion.SortText);
            }
        );
    }

    [Fact]
    public async Task RewriteAsync_DoesNotChangeIfSnippetLabel()
    {
        // Arrange
        var documentContent = "@$$";
        TestFileMarkupParser.GetPosition(documentContent, out documentContent, out var cursorPosition);
        var delegatedCompletionList = GenerateCompletionList(
            ("using", CompletionItemKind.Keyword),
            ("if", CompletionItemKind.Snippet)
            );
        var rewriter = new SnippetResponseRewriter();

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            cursorPosition, documentContent, delegatedCompletionList);

        // Assert
        Assert.NotNull(rewrittenCompletionList);
        Assert.Null(rewrittenCompletionList.CommitCharacters);
        Assert.Collection(
            rewrittenCompletionList.Items,
            completion =>
            {
                Assert.Equal("using", completion.Label);
                Assert.Equal("using", completion.SortText);
            },
            completion =>
            {
                Assert.Equal("if", completion.Label);
                Assert.Equal("if", completion.SortText);
            }
        );
    }

    private static RazorVSInternalCompletionList GenerateCompletionList(params (string? Label, CompletionItemKind Kind)[] itemsData)
        => new()
        {
            Items = [.. itemsData.Select(itemData => new VSInternalCompletionItem()
            {
                Label = itemData.Label!,
                SortText = itemData.Label,
                Kind = itemData.Kind
            })]
        };
}

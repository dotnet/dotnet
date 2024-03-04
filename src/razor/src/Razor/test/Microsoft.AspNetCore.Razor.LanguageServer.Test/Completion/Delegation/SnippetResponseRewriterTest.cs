// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test.Completion.Delegation;

public class SnippetResponseRewriterTest(ITestOutputHelper testOutput)
    : ResponseRewriterTestBase(new SnippetResponseRewriter(), testOutput)
{
    [Fact]
    public async Task RewriteAsync_ChangesUsingSnippetLabel()
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
            cursorPosition, documentContent, delegatedCompletionList, rewriter);

        // Assert
        Assert.Null(rewrittenCompletionList.CommitCharacters);
        Assert.Collection(
            rewrittenCompletionList.Items,
            completion =>
            {
                Assert.Equal("using statement", completion.Label);
                Assert.Equal("using ", completion.SortText);
            },
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
            cursorPosition, documentContent, delegatedCompletionList, rewriter);

        // Assert
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
            cursorPosition, documentContent, delegatedCompletionList, rewriter);

        // Assert
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
    public async Task RewriteAsync_HandlesNullLabels()
    {
        // Arrange
        var documentContent = "@$$";
        TestFileMarkupParser.GetPosition(documentContent, out documentContent, out var cursorPosition);
        var delegatedCompletionList = GenerateCompletionList(
            (null, CompletionItemKind.Keyword),
            ("using", CompletionItemKind.Snippet)
            );
        var rewriter = new SnippetResponseRewriter();

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            cursorPosition, documentContent, delegatedCompletionList, rewriter);

        // Assert
        Assert.Null(rewrittenCompletionList.CommitCharacters);
        Assert.Collection(
            rewrittenCompletionList.Items,
            completion =>
            {
                Assert.Null(completion.Label);
                Assert.Null(completion.SortText);
            },
            completion =>
            {
                Assert.Equal("using statement", completion.Label);
                Assert.Equal("using ", completion.SortText);
            }
        );
    }

    private static VSInternalCompletionList GenerateCompletionList(params (string? Label, CompletionItemKind Kind)[] itemsData)
    {
        var items = itemsData.Select(itemData => new VSInternalCompletionItem()
            {
                Label = itemData.Label!,
                SortText = itemData.Label,
                Kind = itemData.Kind})
        .ToArray();

        return new VSInternalCompletionList()
        {
            Items = items
        };
    }
}

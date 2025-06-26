// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Completion.Delegation;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class HtmlCommitCharacterResponseRewriterTest(ITestOutputHelper testOutput) : ResponseRewriterTestBase(testOutput)
{
    [Theory]
    [CombinatorialData]
    public async Task RewriteAsync_CSharp_DoesNothing(bool useDefaultCommitCharacters, bool useVSTypes)
    {
        // Arrange
        var input = """
            @$$DateTime
            """;

        TestFileMarkupParser.GetPosition(input, out var documentContent, out var cursorPosition);
        var delegatedCompletionList = GenerateCompletionList(useDefaultCommitCharacters, useVSTypes, "Element1", "Element2");

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            cursorPosition, documentContent, delegatedCompletionList);

        // Assert
        Assert.Same(delegatedCompletionList, rewrittenCompletionList);
    }

    [Theory]
    [CombinatorialData]
    public async Task RewriteAsync_Default_DoesNothing(bool useDefaultCommitCharacters, bool useVSTypes)
    {
        // Arrange
        var input = """
            <$$
            """;

        TestFileMarkupParser.GetPosition(input, out var documentContent, out var cursorPosition);
        var delegatedCompletionList = GenerateCompletionList(useDefaultCommitCharacters, useVSTypes, "Element1", "Element2");

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            cursorPosition, documentContent, delegatedCompletionList);

        // Assert
        Assert.Same(delegatedCompletionList, rewrittenCompletionList);
    }

    [Theory]
    [CombinatorialData]
    public async Task RewriteAsync_DefaultCommitCharacters_RemovesSpace(bool useVSTypes)
    {
        // Arrange
        var input = """
            <$$
            """;

        TestFileMarkupParser.GetPosition(input, out var documentContent, out var cursorPosition);
        var delegatedCompletionList = GenerateCompletionList(useDefaultCommitCharacters: true, useVSTypes, "Element1", "Element2");

        var razorCompletionOptions = new RazorCompletionOptions(
                SnippetsSupported: true,
                AutoInsertAttributeQuotes: true,
                CommitElementsWithSpace: false);

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            cursorPosition, documentContent, delegatedCompletionList, razorCompletionOptions);

        // Assert
        Assert.NotNull(rewrittenCompletionList);
        Assert.NotNull(rewrittenCompletionList.CommitCharacters);

        if (useVSTypes)
        {
            Assert.Contains(rewrittenCompletionList.CommitCharacters.Value.Second, c => c.Character == " ");
        }
        else
        {
            Assert.Contains(rewrittenCompletionList.CommitCharacters.Value.First, c => c == " ");
        }

        Assert.Collection(
            rewrittenCompletionList.Items,
            completion =>
            {
                Assert.Equal("Element1", completion.Label);
                Assert.NotNull(completion.CommitCharacters);
                Assert.DoesNotContain(completion.CommitCharacters, c => c == " ");
            },
            completion =>
            {
                Assert.Equal("Element2", completion.Label);
                Assert.NotNull(completion.CommitCharacters);
                Assert.DoesNotContain(completion.CommitCharacters, c => c == " ");
            });
    }

    [Theory]
    [CombinatorialData]
    public async Task RewriteAsync_ItemCommitCharacters_RemovesSpace(bool useVSTypes)
    {
        // Arrange
        var input = """
            <$$
            """;

        TestFileMarkupParser.GetPosition(input, out var documentContent, out var cursorPosition);
        var delegatedCompletionList = GenerateCompletionList(useDefaultCommitCharacters: false, useVSTypes, "Element1", "Element2");

        var razorCompletionOptions = new RazorCompletionOptions(
            SnippetsSupported: true,
            AutoInsertAttributeQuotes: true,
            CommitElementsWithSpace: false);
        var rewriter = new HtmlCommitCharacterResponseRewriter();

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            cursorPosition,
            documentContent,
            delegatedCompletionList,
            razorCompletionOptions);

        // Assert
        Assert.NotNull(rewrittenCompletionList);
        Assert.Null(rewrittenCompletionList.CommitCharacters);
        Assert.Collection(
            rewrittenCompletionList.Items,
            completion =>
            {
                Assert.Equal("Element1", completion.Label);
                Assert.NotNull(completion.CommitCharacters);
                Assert.DoesNotContain(completion.CommitCharacters, c => c == " ");
            },
            completion =>
            {
                Assert.Equal("Element2", completion.Label);
                Assert.NotNull(completion.CommitCharacters);
                Assert.DoesNotContain(completion.CommitCharacters, c => c == " ");
            });
    }

    private static RazorVSInternalCompletionList GenerateCompletionList(bool useDefaultCommitCharacters, bool useVSTypes, params string[] itemLabels)
        => new()
        {
            Items = [.. itemLabels.Select(label => new VSInternalCompletionItem()
            {
                Kind = CompletionItemKind.Element,
                Label = label,
                CommitCharacters = useDefaultCommitCharacters ? null : [" ", ">"]
            })],
            CommitCharacters = (useDefaultCommitCharacters, useVSTypes) switch
            {
                (true, true) => new VSInternalCommitCharacter[] { new() { Character = " " }, new() { Character = ">" } },
                (true, false) => new string[] { " ", ">" },
                _ => null
            }
        };
}

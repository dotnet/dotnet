// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class HtmlCommitCharacterResponseRewriterTest(ITestOutputHelper testOutput)
    : ResponseRewriterTestBase(new HtmlCommitCharacterResponseRewriter(TestRazorLSPOptionsMonitor.Create()), testOutput)
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

        var options = TestRazorLSPOptionsMonitor.Create();
        await options.UpdateAsync(options.CurrentValue with { CommitElementsWithSpace = false }, CancellationToken.None);
        var rewriter = new HtmlCommitCharacterResponseRewriter(options);

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(cursorPosition, documentContent, delegatedCompletionList, rewriter);

        // Assert
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
                Assert.DoesNotContain(completion.CommitCharacters, c => c == " ");
            },
            completion =>
            {
                Assert.Equal("Element2", completion.Label);
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

        var options = TestRazorLSPOptionsMonitor.Create();
        await options.UpdateAsync(options.CurrentValue with { CommitElementsWithSpace = false }, CancellationToken.None);
        var rewriter = new HtmlCommitCharacterResponseRewriter(options);

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(cursorPosition, documentContent, delegatedCompletionList, rewriter);

        // Assert
        Assert.Null(rewrittenCompletionList.CommitCharacters);
        Assert.Collection(
            rewrittenCompletionList.Items,
            completion =>
            {
                Assert.Equal("Element1", completion.Label);
                Assert.DoesNotContain(completion.CommitCharacters, c => c == " ");
            },
            completion =>
            {
                Assert.Equal("Element2", completion.Label);
                Assert.DoesNotContain(completion.CommitCharacters, c => c == " ");
            });
    }

    private static VSInternalCompletionList GenerateCompletionList(bool useDefaultCommitCharacters, bool useVSTypes, params string[] itemLabels)
    {
        var items = itemLabels.Select(label => new VSInternalCompletionItem()
        {
            Kind = CompletionItemKind.Element,
            Label = label,
            CommitCharacters = useDefaultCommitCharacters ? null : new string[] { " ", ">" }
        }).ToArray();
        return new VSInternalCompletionList()
        {
            Items = items,
            CommitCharacters = (useDefaultCommitCharacters, useVSTypes) switch
            {
                (true, true) => new VSInternalCommitCharacter[] { new() { Character = " " }, new() { Character = ">" } },
                (true, false) => new string[] { " ", ">" },
                _ => null
            }
        };
    }
}

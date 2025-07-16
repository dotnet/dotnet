// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class TextEditResponseRewriterTest(ITestOutputHelper testOutput) : ResponseRewriterTestBase(testOutput)
{
    [Fact]
    public async Task RewriteAsync_NotCSharp_Noops()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "<";
        var textEditRange = LspFactory.CreateSingleLineRange(start: (0, 0), length: 1);
        var delegatedCompletionList = GenerateCompletionList(textEditRange);

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        Assert.NotNull(rewrittenCompletionList);

        var firstItem = rewrittenCompletionList.Items[0];
        Assert.NotNull(firstItem.TextEdit);
        Assert.Equal(textEditRange, firstItem.TextEdit.Value.First.Range);
    }

    [Fact]
    public async Task RewriteAsync_CSharp_AdjustsItemRange()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "@DateTime";
        // Line 19: __o = DateTime
        var textEditRange = LspFactory.CreateSingleLineRange(line: 19, character: 6, length: 8);
        var delegatedCompletionList = GenerateCompletionList(textEditRange);
        var expectedRange = LspFactory.CreateSingleLineRange(line: 0, character: 1, length: 8);

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        Assert.NotNull(rewrittenCompletionList);

        var firstItem = rewrittenCompletionList.Items[0];
        Assert.NotNull(firstItem.TextEdit);
        Assert.Equal(expectedRange, firstItem.TextEdit.Value.First.Range);
    }

    [Fact]
    public async Task RewriteAsync_CSharp_AdjustsListRange()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "@DateTime";
        // Line 19: __o = DateTime
        var textEditRange = LspFactory.CreateSingleLineRange(line: 19, character: 6, length: 8);
        var delegatedCompletionList = GenerateCompletionList(textEditRange);
        delegatedCompletionList.ItemDefaults = new CompletionListItemDefaults()
        {
            EditRange = textEditRange,
        };
        var expectedRange = LspFactory.CreateSingleLineRange(line: 0, character: 1, length: 8);

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        Assert.NotNull(rewrittenCompletionList);
        Assert.NotNull(rewrittenCompletionList.ItemDefaults);
        Assert.Equal(expectedRange, rewrittenCompletionList.ItemDefaults.EditRange);
    }

    private static RazorVSInternalCompletionList GenerateCompletionList(LspRange textEditRange)
        => new()
        {
            Items = [
                new VSInternalCompletionItem()
                {
                    Label = string.Empty, // label string is non-nullable
                    TextEdit = LspFactory.CreateTextEdit(textEditRange, "Hello")
                }
            ]
        };
}

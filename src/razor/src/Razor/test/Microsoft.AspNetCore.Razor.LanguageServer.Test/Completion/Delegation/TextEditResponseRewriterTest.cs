// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class TextEditResponseRewriterTest(ITestOutputHelper testOutput)
    : ResponseRewriterTestBase(new TextEditResponseRewriter(), testOutput)
{
    [Fact]
    public async Task RewriteAsync_NotCSharp_Noops()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "<";
        var textEditRange = VsLspFactory.CreateSingleLineRange(start: (0, 0), length: 1);
        var delegatedCompletionList = GenerateCompletionList(textEditRange);

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        Assert.Equal(textEditRange, rewrittenCompletionList.Items[0].TextEdit.Value.First.Range);
    }

    [Fact]
    public async Task RewriteAsync_CSharp_AdjustsItemRange()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "@DateTime";
        // Line 19: __o = DateTime
        var textEditRange = VsLspFactory.CreateSingleLineRange(line: 19, character: 6, length: 8);
        var delegatedCompletionList = GenerateCompletionList(textEditRange);
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 8);

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        Assert.Equal(expectedRange, rewrittenCompletionList.Items[0].TextEdit.Value.First.Range);
    }

    [Fact]
    public async Task RewriteAsync_CSharp_AdjustsListRange()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "@DateTime";
        // Line 19: __o = DateTime
        var textEditRange = VsLspFactory.CreateSingleLineRange(line: 19, character: 6, length: 8);
        var delegatedCompletionList = GenerateCompletionList(textEditRange);
        delegatedCompletionList.ItemDefaults = new CompletionListItemDefaults()
        {
            EditRange = textEditRange,
        };
        var expectedRange = VsLspFactory.CreateSingleLineRange(line: 0, character: 1, length: 8);

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        Assert.Equal(expectedRange, rewrittenCompletionList.ItemDefaults.EditRange);
    }

    private static VSInternalCompletionList GenerateCompletionList(Range textEditRange)
    {
        return new VSInternalCompletionList()
        {
            Items = [
                new VSInternalCompletionItem()
                {
                    TextEdit = VsLspFactory.CreateTextEdit(textEditRange, "Hello")
                }
            ]
        };
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class TextEditResponseRewriterTest : ResponseRewriterTestBase
{
    public TextEditResponseRewriterTest(ITestOutputHelper testOutput)
        : base(new TextEditResponseRewriter(), testOutput)
    {
    }

    [Fact]
    public async Task RewriteAsync_NotCSharp_Noops()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "<";
        var textEditRange = new Range()
        {
            Start = new Position(0, 0),
            End = new Position(0, 1),
        };
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
        var textEditRange = new Range()
        {
            // Line 19: __o = DateTime
            Start = new Position(19, 6),
            End = new Position(19, 14),
        };
        var delegatedCompletionList = GenerateCompletionList(textEditRange);
        var expectedRange = new Range()
        {
            Start = new Position(0, 1),
            End = new Position(0, 9),
        };

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
        var textEditRange = new Range()
        {
            // Line 19: __o = DateTime
            Start = new Position(19, 6),
            End = new Position(19, 14),
        };
        var delegatedCompletionList = GenerateCompletionList(textEditRange);
        delegatedCompletionList.ItemDefaults = new CompletionListItemDefaults()
        {
            EditRange = textEditRange,
        };
        var expectedRange = new Range()
        {
            Start = new Position(0, 1),
            End = new Position(0, 9),
        };

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
            Items = new[]
            {
                new VSInternalCompletionItem()
                {
                    TextEdit = new TextEdit()
                    {
                        NewText = "Hello",
                        Range = textEditRange,
                    }
                }
            }
        };
    }
}

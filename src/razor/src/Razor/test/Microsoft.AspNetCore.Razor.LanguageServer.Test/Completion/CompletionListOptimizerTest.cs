// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class CompletionListOptimizerTest : TestBase
{
    public CompletionListOptimizerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void Convert_CommitCharactersTrue_RemovesCommitCharactersFromItems()
    {
        // Arrange
        var commitCharacters = new[] { "<" };
        var completionList = new VSInternalCompletionList()
        {
            Items = new[]
            {
                new VSInternalCompletionItem()
                {
                    Label = "Test",
                    VsCommitCharacters = commitCharacters
                }
            }
        };
        var capabilities = new VSInternalCompletionSetting()
        {
            CompletionList = new VSInternalCompletionListSetting()
            {
                CommitCharacters = true,
            }
        };

        // Act
        var vsCompletionList = CompletionListOptimizer.Optimize(completionList, capabilities);

        // Assert
        Assert.Collection(vsCompletionList.Items, (item) => Assert.Null(item.CommitCharacters));

        Assert.Collection(vsCompletionList.CommitCharacters.Value.First, (e) => Assert.Equal("<", e));
    }

    [Fact]
    public void Convert_CommitCharactersFalse_DoesNotTouchCommitCharacters()
    {
        // Arrange
        var commitCharacters = new[] { "<" };
        var completionList = new VSInternalCompletionList()
        {
            Items = new[]
            {
                new VSInternalCompletionItem()
                {
                    Label = "Test",
                    VsCommitCharacters = commitCharacters
                }
            }
        };
        var capabilities = new VSInternalCompletionSetting()
        {
            CompletionList = new VSInternalCompletionListSetting()
            {
                CommitCharacters = false,
            }
        };

        // Act
        var vsCompletionList = CompletionListOptimizer.Optimize(completionList, capabilities);

        // Assert
        Assert.Collection(vsCompletionList.Items, item => Assert.Equal(commitCharacters, ((VSInternalCompletionItem)item).VsCommitCharacters));
        Assert.Null(vsCompletionList.CommitCharacters);
    }
}

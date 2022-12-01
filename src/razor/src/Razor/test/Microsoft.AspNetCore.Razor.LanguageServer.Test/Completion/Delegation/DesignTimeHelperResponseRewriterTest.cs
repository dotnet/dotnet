// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class DesignTimeHelperResponseRewriterTest : ResponseRewriterTestBase
{
    public DesignTimeHelperResponseRewriterTest(ITestOutputHelper testOutput)
        : base(new DesignTimeHelperResponseRewriter(), testOutput)
    {
    }

    [Fact]
    public async Task RewriteAsync_NotCSharp_Noops()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "<";
        var delegatedCompletionList = GenerateCompletionList("p", "div");

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        Assert.Equal(2, rewrittenCompletionList.Items.Length);
    }

    [Fact]
    public async Task RewriteAsync_RemovesHelper()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "@DateTime";
        var delegatedCompletionList = GenerateCompletionList("__helper", "DateTime");

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        var item = Assert.Single(rewrittenCompletionList.Items);
        Assert.Equal("DateTime", item.Label);
    }

    [Fact]
    public async Task RewriteAsync_TryingToUseHelper_Noops()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "@__hel";
        var delegatedCompletionList = GenerateCompletionList("__helper", "DateTime");

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        Assert.Equal(2, rewrittenCompletionList.Items.Length);
    }

    [Fact]
    public async Task RewriteAsync_AlwaysRemovesRazorHelpers()
    {
        // Arrange
        var getCompletionsAt = 1;
        var documentContent = "@__hel";
        var delegatedCompletionList = GenerateCompletionList("__helper", "__builder");

        // Act
        var rewrittenCompletionList = await GetRewrittenCompletionListAsync(
            getCompletionsAt, documentContent, delegatedCompletionList);

        // Assert
        var item = Assert.Single(rewrittenCompletionList.Items);
        Assert.Equal("__helper", item.Label);
    }

    private static VSInternalCompletionList GenerateCompletionList(params string[] itemLabels)
    {
        var items = itemLabels.Select(label => new VSInternalCompletionItem() { Label = label }).ToArray();
        return new VSInternalCompletionList()
        {
            Items = items
        };
    }
}

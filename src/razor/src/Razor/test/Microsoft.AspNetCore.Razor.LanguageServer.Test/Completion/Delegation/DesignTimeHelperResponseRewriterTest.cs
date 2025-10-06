// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

public class DesignTimeHelperResponseRewriterTest(ITestOutputHelper testOutput) : ResponseRewriterTestBase(testOutput)
{
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
        Assert.NotNull(rewrittenCompletionList);
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
        Assert.NotNull(rewrittenCompletionList);
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
        Assert.NotNull(rewrittenCompletionList);
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
        Assert.NotNull(rewrittenCompletionList);
        var item = Assert.Single(rewrittenCompletionList.Items);
        Assert.Equal("__helper", item.Label);
    }

    private static RazorVSInternalCompletionList GenerateCompletionList(params string[] itemLabels)
        => new()
        {
            Items = [.. itemLabels.Select(label => new VSInternalCompletionItem() { Label = label })]
        };
}

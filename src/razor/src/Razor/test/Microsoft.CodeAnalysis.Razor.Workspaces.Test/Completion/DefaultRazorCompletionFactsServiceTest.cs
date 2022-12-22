// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Completion;

public class DefaultRazorCompletionFactsServiceTest : TestBase
{
    public DefaultRazorCompletionFactsServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void GetDirectiveCompletionItems_AllProvidersCompletionItems()
    {
        // Arrange
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create());
        var tagHelperDocumentContext = TagHelperDocumentContext.Create(prefix: null, Enumerable.Empty<TagHelperDescriptor>());
        var completionItem1 = new RazorCompletionItem("displayText1", "insertText1", RazorCompletionItemKind.Directive);
        var context = new RazorCompletionContext(0, null, syntaxTree, tagHelperDocumentContext);
        var provider1 = Mock.Of<RazorCompletionItemProvider>(p => p.GetCompletionItems(context) == new[] { completionItem1 }, MockBehavior.Strict);
        var completionItem2 = new RazorCompletionItem("displayText2", "insertText2", RazorCompletionItemKind.Directive);
        var provider2 = Mock.Of<RazorCompletionItemProvider>(p => p.GetCompletionItems(context) == new[] { completionItem2 }, MockBehavior.Strict);
        var completionFactsService = new DefaultRazorCompletionFactsService(new[] { provider1, provider2 });

        // Act
        var completionItems = completionFactsService.GetCompletionItems(context);

        // Assert
        Assert.Equal(new[] { completionItem1, completionItem2 }, completionItems);
    }
}

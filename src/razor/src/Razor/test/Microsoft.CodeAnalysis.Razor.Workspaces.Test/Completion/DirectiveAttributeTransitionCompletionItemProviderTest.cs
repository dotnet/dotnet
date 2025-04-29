// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Completion;

public class DirectiveAttributeTransitionCompletionItemProviderTest : ToolingTestBase
{
    private readonly TagHelperDocumentContext _tagHelperDocumentContext;
    private readonly DirectiveAttributeTransitionCompletionItemProvider _provider;

    public DirectiveAttributeTransitionCompletionItemProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _tagHelperDocumentContext = TagHelperDocumentContext.Create(prefix: string.Empty, tagHelpers: []);
        _provider = new DirectiveAttributeTransitionCompletionItemProvider(TestLanguageServerFeatureOptions.Instance);
    }

    [Fact]
    public void IsValidCompletionPoint_AtPrefixLeadingEdge_ReturnsFalse()
    {
        // Arrange

        // <p| class=""></p>
        var absoluteIndex = 2;
        var prefixLocation = new TextSpan(2, 1);
        var attributeNameLocation = new TextSpan(3, 5);

        // Act
        var result = DirectiveAttributeTransitionCompletionItemProvider.IsValidCompletionPoint(absoluteIndex, prefixLocation, attributeNameLocation);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidCompletionPoint_WithinPrefix_ReturnsTrue()
    {
        // Arrange

        // <p | class=""></p>
        var absoluteIndex = 3;
        var prefixLocation = new TextSpan(2, 2);
        var attributeNameLocation = new TextSpan(4, 5);

        // Act
        var result = DirectiveAttributeTransitionCompletionItemProvider.IsValidCompletionPoint(absoluteIndex, prefixLocation, attributeNameLocation);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidCompletionPoint_NullPrefix_ReturnsFalse()
    {
        // Arrange

        // <svg xml:base="abc"xm| ></svg>
        var absoluteIndex = 21;
        TextSpan? prefixLocation = null;
        var attributeNameLocation = new TextSpan(4, 5);

        // Act
        var result = DirectiveAttributeTransitionCompletionItemProvider.IsValidCompletionPoint(absoluteIndex, prefixLocation, attributeNameLocation);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidCompletionPoint_AtNameLeadingEdge_ReturnsFalse()
    {
        // Arrange

        // <p |class=""></p>
        var absoluteIndex = 3;
        var prefixLocation = new TextSpan(2, 1);
        var attributeNameLocation = new TextSpan(3, 5);

        // Act
        var result = DirectiveAttributeTransitionCompletionItemProvider.IsValidCompletionPoint(absoluteIndex, prefixLocation, attributeNameLocation);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidCompletionPoint_WithinName_ReturnsFalse()
    {
        // Arrange

        // <p cl|ass=""></p>
        var absoluteIndex = 5;
        var prefixLocation = new TextSpan(2, 1);
        var attributeNameLocation = new TextSpan(3, 5);

        // Act
        var result = DirectiveAttributeTransitionCompletionItemProvider.IsValidCompletionPoint(absoluteIndex, prefixLocation, attributeNameLocation);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidCompletionPoint_OutsideOfNameAndPrefix_ReturnsFalse()
    {
        // Arrange

        // <p class=|""></p>
        var absoluteIndex = 9;
        var prefixLocation = new TextSpan(2, 1);
        var attributeNameLocation = new TextSpan(3, 5);

        // Act
        var result = DirectiveAttributeTransitionCompletionItemProvider.IsValidCompletionPoint(absoluteIndex, prefixLocation, attributeNameLocation);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetCompletionItems_AttributeAreaInNonComponentFile_ReturnsEmptyList()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 7, "<input  />", FileKinds.Legacy);

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetCompletionItems_NonAttribute_ReturnsEmptyList()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 2, "<input  />");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetCompletionItems_ExistingAttribute_ReturnsEmptyList()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 8, "<input @ />");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetCompletionItems_InbetweenSelfClosingEnd_ReturnsEmptyList()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 8, """
            <input /
            
            """);

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetCompletionItems_AttributeAreaInComponentFile_ReturnsTransitionCompletionItem()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 7, "<input  />");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        var item = Assert.Single(result);
        Assert.True(DirectiveAttributeTransitionCompletionItemProvider.IsTransitionCompletionItem(item));
    }

    [Fact]
    public void GetCompletionItems_AttributeAreaEndOfSelfClosingTag_ReturnsTransitionCompletionItem()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 7, "<input />");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        var item = Assert.Single(result);
        Assert.True(DirectiveAttributeTransitionCompletionItemProvider.IsTransitionCompletionItem(item));
    }

    [Fact]
    public void GetCompletionItems_AttributeAreaEndOfOpeningTag_ReturnsTransitionCompletionItem()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 7, "<input ></input>");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        var item = Assert.Single(result);
        Assert.True(DirectiveAttributeTransitionCompletionItemProvider.IsTransitionCompletionItem(item));
    }

    [Fact]
    public void GetCompletionItems_ExistingAttribute_LeadingEdge_ReturnsEmptyList()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 7, "<input src=\"xyz\" />");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetCompletionItems_ExistingAttribute_TrailingEdge_ReturnsEmptyList()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 16, "<input src=\"xyz\" />");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetCompletionItems_ExistingAttribute_TrailingEdgeOnSpace_ReturnsEmptyList()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 16, "<input src=\"xyz\"   />");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetCompletionItems_ExistingAttribute_Partial_ReturnsEmptyList()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 9, "<svg xml: ></svg>");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetCompletionItems_AttributeAreaInIncompleteAttributeTransition_ReturnsTransitionCompletionItem()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 7, "<input   @{");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        var item = Assert.Single(result);
        Assert.True(DirectiveAttributeTransitionCompletionItemProvider.IsTransitionCompletionItem(item));
    }

    [Fact]
    public void GetCompletionItems_AttributeAreaInIncompleteComponent_ReturnsTransitionCompletionItem()
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 5, "<svg  xml:base=\"d\"></svg>");

        // Act
        var result = _provider.GetCompletionItems(context);

        // Assert
        var item = Assert.Single(result);
        Assert.True(DirectiveAttributeTransitionCompletionItemProvider.IsTransitionCompletionItem(item));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetCompletionItems_WithAvoidExplicitCommitOption_ReturnsAppropriateCommitCharacters(bool supportsSoftSelection)
    {
        // Arrange
        var context = CreateContext(absoluteIndex: 7, "<input  />");
        var provider = new DirectiveAttributeTransitionCompletionItemProvider(new TestLanguageServerFeatureOptions(supportsSoftSelectionInCompletion: supportsSoftSelection));

        // Act
        var result = provider.GetCompletionItems(context);

        // Assert
        var item = Assert.Single(result);
        Assert.True(DirectiveAttributeTransitionCompletionItemProvider.IsTransitionCompletionItem(item));
        if (supportsSoftSelection)
        {
            Assert.NotEmpty(item.CommitCharacters);
        }
        else
        {
            Assert.Empty(item.CommitCharacters);
        }
    }

    private static RazorSyntaxTree GetSyntaxTree(string text, string? fileKind = null)
    {
        fileKind ??= FileKinds.Component;
        var sourceDocument = TestRazorSourceDocument.Create(text);
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });

        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, importSources: default, tagHelpers: []);

        return codeDocument.GetSyntaxTree();
    }

    private RazorCompletionContext CreateContext(int absoluteIndex, string documentContent, string? fileKind = null)
    {
        var syntaxTree = GetSyntaxTree(documentContent, fileKind);
        var owner = syntaxTree.Root.FindInnermostNode(absoluteIndex, includeWhitespace: true, walkMarkersBack: true);
        owner = AbstractRazorCompletionFactsService.AdjustSyntaxNodeForWordBoundary(owner, absoluteIndex);

        return new RazorCompletionContext(absoluteIndex, owner, syntaxTree, _tagHelperDocumentContext);
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class RazorCompletionItemResolverTest : LanguageServerTestBase
{
    private readonly LSPTagHelperTooltipFactory _lspTagHelperTooltipFactory;
    private readonly VSLSPTagHelperTooltipFactory _vsLspTagHelperTooltipFactory;
    private readonly VSInternalCompletionSetting _completionCapability;
    private readonly VSInternalClientCapabilities _defaultClientCapability;
    private readonly VSInternalClientCapabilities _vsClientCapability;
    private readonly AggregateBoundAttributeDescription _attributeDescription;
    private readonly AggregateBoundElementDescription _elementDescription;

    public RazorCompletionItemResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _lspTagHelperTooltipFactory = new DefaultLSPTagHelperTooltipFactory();
        _vsLspTagHelperTooltipFactory = new DefaultVSLSPTagHelperTooltipFactory();
        _completionCapability = new VSInternalCompletionSetting()
        {
            CompletionItem = new CompletionItemSetting()
            {
                DocumentationFormat = new[] { MarkupKind.PlainText, MarkupKind.Markdown },
            }
        };

        _defaultClientCapability = new VSInternalClientCapabilities()
        {
            TextDocument = new TextDocumentClientCapabilities()
            {
                Completion = _completionCapability,
            },
        };

        _vsClientCapability = new VSInternalClientCapabilities()
        {
            TextDocument = new TextDocumentClientCapabilities()
            {
                Completion = _completionCapability,
            },
            SupportsVisualStudioExtensions = true,
        };

        var attributeDescriptionInfo = new BoundAttributeDescriptionInfo("System.DateTime", "System.DateTime", "DateTime", "Returns the time.");
        _attributeDescription = new AggregateBoundAttributeDescription(ImmutableArray.Create(attributeDescriptionInfo));
        var elementDescriptionInfo = new BoundElementDescriptionInfo("System.SomeTagHelper", "This is some TagHelper.");
        _elementDescription = new AggregateBoundElementDescription(ImmutableArray.Create(elementDescriptionInfo));
    }

    [Fact]
    public async Task ResolveAsync_DirectiveCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.Directive);
        razorCompletionItem.SetDirectiveCompletionDescription(new DirectiveCompletionDescription("Test directive"));
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _defaultClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_MarkupTransitionCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("@...", "@", RazorCompletionItemKind.MarkupTransition);
        razorCompletionItem.SetMarkupTransitionCompletionDescription(new MarkupTransitionCompletionDescription("Test description"));
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _defaultClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_DirectiveAttributeCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.DirectiveAttribute);
        razorCompletionItem.SetAttributeCompletionDescription(_attributeDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _defaultClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_DirectiveAttributeParameterCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.DirectiveAttributeParameter);
        razorCompletionItem.SetAttributeCompletionDescription(_attributeDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _defaultClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_TagHelperElementCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.TagHelperElement);
        razorCompletionItem.SetTagHelperElementDescriptionInfo(_elementDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _defaultClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_TagHelperAttribute_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.TagHelperAttribute);
        razorCompletionItem.SetAttributeCompletionDescription(_attributeDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _defaultClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_VS_DirectiveAttributeCompletion_ReturnsCompletionItemWithDescription()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.DirectiveAttribute);
        razorCompletionItem.SetAttributeCompletionDescription(_attributeDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _vsClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Description);
    }

    [Fact]
    public async Task ResolveAsync_VS_DirectiveAttributeParameterCompletion_ReturnsCompletionItemWithDescription()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.DirectiveAttributeParameter);
        razorCompletionItem.SetAttributeCompletionDescription(_attributeDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _vsClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Description);
    }

    [Fact]
    public async Task ResolveAsync_VS_TagHelperElementCompletion_ReturnsCompletionItemWithDescription()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.TagHelperElement);
        razorCompletionItem.SetTagHelperElementDescriptionInfo(_elementDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _vsClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Description);
    }

    [Fact]
    public async Task ResolveAsync_VS_TagHelperAttribute_ReturnsCompletionItemWithDescription()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.TagHelperAttribute);
        razorCompletionItem.SetAttributeCompletionDescription(_attributeDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single() as VSInternalCompletionItem;

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, new[] { razorCompletionItem }, _vsClientCapability, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem.Description);
    }

    [Fact]
    public async Task ResolveAsync_NonTagHelperCompletion_Noops()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory);
        var completionItem = new VSInternalCompletionItem();
        var completionList = new VSInternalCompletionList() { Items = new[] { completionItem } };

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, Array.Empty<RazorCompletionItem>(), _defaultClientCapability, DisposalToken);

        // Assert
        Assert.Null(resolvedCompletionItem);
    }

    private VSInternalCompletionList CreateLSPCompletionList(params RazorCompletionItem[] razorCompletionItems)
        => RazorCompletionListProvider.CreateLSPCompletionList(razorCompletionItems.ToImmutableArray(), _defaultClientCapability);
}

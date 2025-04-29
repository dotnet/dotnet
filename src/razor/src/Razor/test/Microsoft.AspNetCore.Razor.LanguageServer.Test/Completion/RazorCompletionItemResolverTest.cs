// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class RazorCompletionItemResolverTest : LanguageServerTestBase
{
    private readonly IComponentAvailabilityService _componentAvailabilityService;
    private readonly VSInternalCompletionSetting _completionCapability;
    private readonly VSInternalClientCapabilities _defaultClientCapability;
    private readonly VSInternalClientCapabilities _vsClientCapability;
    private readonly AggregateBoundAttributeDescription _attributeDescription;
    private readonly AggregateBoundElementDescription _elementDescription;

    public RazorCompletionItemResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _componentAvailabilityService = CreateComponentAvailabilityService();

        _completionCapability = new VSInternalCompletionSetting()
        {
            CompletionItem = new CompletionItemSetting()
            {
                DocumentationFormat = [MarkupKind.PlainText, MarkupKind.Markdown],
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
        _attributeDescription = new AggregateBoundAttributeDescription([attributeDescriptionInfo]);
        var elementDescriptionInfo = new BoundElementDescriptionInfo("System.SomeTagHelper", "This is some TagHelper.");
        _elementDescription = new AggregateBoundElementDescription([elementDescriptionInfo]);
    }

    private static IComponentAvailabilityService CreateComponentAvailabilityService()
    {
        var mock = new StrictMock<IComponentAvailabilityService>();
        mock.Setup(x => x.GetComponentAvailabilityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        return mock.Object;
    }

    [Fact]
    public async Task ResolveAsync_DirectiveCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var descriptionText = "Test directive";
        var razorCompletionItem = RazorCompletionItem.CreateDirective(
            displayText: "TestItem",
            insertText: "TestItem",
            sortText: null,
            descriptionInfo: new(descriptionText),
            commitCharacters: [],
            isSnippet: false);

        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _defaultClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.Equal(descriptionText, resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_MarkupTransitionCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var descriptionText = "Test description";
        var razorCompletionItem = RazorCompletionItem.CreateMarkupTransition("@...", "@", new(descriptionText), commitCharacters: []);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _defaultClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.Equal(descriptionText, resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_DirectiveAttributeCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var razorCompletionItem = RazorCompletionItem.CreateDirectiveAttribute(
            displayText: "TestItem",
            insertText: "TestItem",
            _attributeDescription,
            commitCharacters: []);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _defaultClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_DirectiveAttributeParameterCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var razorCompletionItem = RazorCompletionItem.CreateDirectiveAttributeParameter(
            displayText: "TestItem",
            insertText: "TestItem",
            _attributeDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _defaultClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_TagHelperElementCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var razorCompletionItem = RazorCompletionItem.CreateTagHelperElement(
            displayText: "TestItem",
            insertText: "TestItem",
            _elementDescription,
            commitCharacters: []);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _defaultClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_TagHelperAttribute_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var razorCompletionItem = RazorCompletionItem.CreateTagHelperAttribute(
            displayText: "TestItem",
            insertText: "TestItem",
            sortText: null,
            _attributeDescription,
            commitCharacters: [],
            isSnippet: false);

        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _defaultClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.NotNull(resolvedCompletionItem.Documentation);
    }

    [Fact]
    public async Task ResolveAsync_VS_DirectiveAttributeCompletion_ReturnsCompletionItemWithDescription()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var razorCompletionItem = RazorCompletionItem.CreateDirectiveAttribute(
            displayText: "TestItem",
            insertText: "TestItem", _attributeDescription,
            commitCharacters: []);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _vsClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.NotNull(resolvedCompletionItem.Description);
    }

    [Fact]
    public async Task ResolveAsync_VS_DirectiveAttributeParameterCompletion_ReturnsCompletionItemWithDescription()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var razorCompletionItem = RazorCompletionItem.CreateDirectiveAttributeParameter(
            displayText: "TestItem",
            insertText: "TestItem",
            _attributeDescription);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _vsClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.NotNull(resolvedCompletionItem.Description);
    }

    [Fact]
    public async Task ResolveAsync_VS_TagHelperElementCompletion_ReturnsCompletionItemWithDescription()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var razorCompletionItem = RazorCompletionItem.CreateTagHelperElement(
            displayText: "TestItem",
            insertText: "TestItem",
            _elementDescription,
            commitCharacters: []);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _vsClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.NotNull(resolvedCompletionItem.Description);
    }

    [Fact]
    public async Task ResolveAsync_VS_TagHelperAttribute_ReturnsCompletionItemWithDescription()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var razorCompletionItem = RazorCompletionItem.CreateTagHelperAttribute(
            displayText: "TestItem",
            insertText: "TestItem",
            sortText: null,
            _attributeDescription,
            commitCharacters: [],
            isSnippet: false);

        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = (VSInternalCompletionItem)completionList.Items.Single();

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, CreateCompletionResolveContext(razorCompletionItem), _vsClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.NotNull(resolvedCompletionItem);
        Assert.NotNull(resolvedCompletionItem.Description);
    }

    [Fact]
    public async Task ResolveAsync_NonTagHelperCompletion_Noops()
    {
        // Arrange
        var resolver = new RazorCompletionItemResolver();
        var completionItem = new VSInternalCompletionItem();
        var completionList = new VSInternalCompletionList() { Items = [completionItem] };

        // Act
        var resolvedCompletionItem = await resolver.ResolveAsync(
            completionItem, completionList, StrictMock.Of<ICompletionResolveContext>(), _defaultClientCapability, _componentAvailabilityService, DisposalToken);

        // Assert
        Assert.Null(resolvedCompletionItem);
    }

    private VSInternalCompletionList CreateLSPCompletionList(params ImmutableArray<RazorCompletionItem> razorCompletionItems)
        => RazorCompletionListProvider.CreateLSPCompletionList(razorCompletionItems, _defaultClientCapability);

    private static RazorCompletionResolveContext CreateCompletionResolveContext(RazorCompletionItem razorCompletionItem)
        => new("file.razor", [razorCompletionItem]);
}

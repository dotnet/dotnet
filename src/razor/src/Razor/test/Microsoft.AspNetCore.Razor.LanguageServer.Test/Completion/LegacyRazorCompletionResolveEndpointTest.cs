// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class LegacyRazorCompletionResolveEndpointTest : LanguageServerTestBase
{
    private readonly LSPTagHelperTooltipFactory _lspTagHelperTooltipFactory;
    private readonly VSLSPTagHelperTooltipFactory _vsLspTagHelperTooltipFactory;
    private readonly CompletionListCache _completionListCache;
    private readonly VSInternalCompletionSetting _completionCapability;
    private readonly VSInternalClientCapabilities _defaultClientCapability;

    public LegacyRazorCompletionResolveEndpointTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _lspTagHelperTooltipFactory = Mock.Of<LSPTagHelperTooltipFactory>(MockBehavior.Strict);
        _vsLspTagHelperTooltipFactory = Mock.Of<VSLSPTagHelperTooltipFactory>(MockBehavior.Strict);
        _completionListCache = new CompletionListCache();
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
    }

    [Fact]
    public async Task Handle_Resolve_DirectiveCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var endpoint = new LegacyRazorCompletionResolveEndpoint(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory, _completionListCache, LoggerFactory);
        endpoint.ApplyCapabilities(new(), _defaultClientCapability);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.Directive);
        razorCompletionItem.SetDirectiveCompletionDescription(new DirectiveCompletionDescription("Test directive"));
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single();
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var newCompletionItem = await endpoint.HandleRequestAsync(parameters, requestContext, default);

        // Assert
        Assert.NotNull(newCompletionItem.Documentation);
    }

    [Fact]
    public async Task Handle_Resolve_MarkupTransitionCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var endpoint = new LegacyRazorCompletionResolveEndpoint(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory, _completionListCache, LoggerFactory);
        endpoint.ApplyCapabilities(new(), _defaultClientCapability);
        var razorCompletionItem = new RazorCompletionItem("@...", "@", RazorCompletionItemKind.MarkupTransition);
        razorCompletionItem.SetMarkupTransitionCompletionDescription(new MarkupTransitionCompletionDescription("Test description"));
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single();
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var newCompletionItem = await endpoint.HandleRequestAsync(parameters, requestContext, default);

        // Assert
        Assert.NotNull(newCompletionItem.Documentation);
    }

    [Fact]
    public async Task Handle_Resolve_DirectiveAttributeCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var lspDescriptionFactory = new Mock<LSPTagHelperTooltipFactory>(MockBehavior.Strict);
        var markdown = new MarkupContent
        {
            Kind = MarkupKind.Markdown,
            Value = "Some Markdown"
        };
        lspDescriptionFactory.Setup(factory => factory.TryCreateTooltip(It.IsAny<AggregateBoundAttributeDescription>(), MarkupKind.Markdown, out markdown))
            .Returns(true);
        var endpoint = new LegacyRazorCompletionResolveEndpoint(lspDescriptionFactory.Object, _vsLspTagHelperTooltipFactory, _completionListCache, LoggerFactory);
        endpoint.ApplyCapabilities(new(), _defaultClientCapability);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.DirectiveAttribute);
        razorCompletionItem.SetAttributeCompletionDescription(AggregateBoundAttributeDescription.Empty);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single();
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var newCompletionItem = await endpoint.HandleRequestAsync(parameters, requestContext, default);

        // Assert
        Assert.NotNull(newCompletionItem.Documentation);
    }

    [Fact]
    public async Task Handle_Resolve_DirectiveAttributeParameterCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var descriptionFactory = new Mock<LSPTagHelperTooltipFactory>(MockBehavior.Strict);
        var markdown = new MarkupContent
        {
            Kind = MarkupKind.Markdown,
            Value = "Some Markdown"
        };
        descriptionFactory.Setup(factory => factory.TryCreateTooltip(It.IsAny<AggregateBoundAttributeDescription>(), MarkupKind.Markdown, out markdown))
            .Returns(true);
        var endpoint = new LegacyRazorCompletionResolveEndpoint(descriptionFactory.Object, _vsLspTagHelperTooltipFactory, _completionListCache, LoggerFactory);
        endpoint.ApplyCapabilities(new(), _defaultClientCapability);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.DirectiveAttributeParameter);
        razorCompletionItem.SetAttributeCompletionDescription(AggregateBoundAttributeDescription.Empty);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single();
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var newCompletionItem = await endpoint.HandleRequestAsync(parameters, requestContext, default);

        // Assert
        Assert.NotNull(newCompletionItem.Documentation);
    }

    [Fact]
    public async Task Handle_Resolve_TagHelperElementCompletion_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var lspDescriptionFactory = new Mock<LSPTagHelperTooltipFactory>(MockBehavior.Strict);
        var markdown = new MarkupContent
        {
            Kind = MarkupKind.Markdown,
            Value = "Some Markdown"
        };
        lspDescriptionFactory.Setup(factory => factory.TryCreateTooltip(It.IsAny<AggregateBoundElementDescription>(), MarkupKind.Markdown, out markdown))
            .Returns(true);
        var endpoint = new LegacyRazorCompletionResolveEndpoint(lspDescriptionFactory.Object, _vsLspTagHelperTooltipFactory, _completionListCache, LoggerFactory);
        endpoint.ApplyCapabilities(new(), _defaultClientCapability);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.TagHelperElement);
        razorCompletionItem.SetTagHelperElementDescriptionInfo(AggregateBoundElementDescription.Empty);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single();
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var newCompletionItem = await endpoint.HandleRequestAsync(parameters, requestContext, default);

        // Assert
        Assert.NotNull(newCompletionItem.Documentation);
    }

    [Fact]
    public async Task Handle_Resolve_TagHelperAttribute_ReturnsCompletionItemWithDocumentation()
    {
        // Arrange
        var lspDescriptionFactory = new Mock<LSPTagHelperTooltipFactory>(MockBehavior.Strict);
        var markdown = new MarkupContent
        {
            Kind = MarkupKind.Markdown,
            Value = "Some Markdown"
        };
        lspDescriptionFactory.Setup(factory => factory.TryCreateTooltip(It.IsAny<AggregateBoundAttributeDescription>(), MarkupKind.Markdown, out markdown))
            .Returns(true);
        var endpoint = new LegacyRazorCompletionResolveEndpoint(lspDescriptionFactory.Object, _vsLspTagHelperTooltipFactory, _completionListCache, LoggerFactory);
        endpoint.ApplyCapabilities(new(), _defaultClientCapability);
        var razorCompletionItem = new RazorCompletionItem("TestItem", "TestItem", RazorCompletionItemKind.TagHelperAttribute);
        razorCompletionItem.SetAttributeCompletionDescription(AggregateBoundAttributeDescription.Empty);
        var completionList = CreateLSPCompletionList(razorCompletionItem);
        var completionItem = completionList.Items.Single();
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var newCompletionItem = await endpoint.HandleRequestAsync(parameters, requestContext, default);

        // Assert
        Assert.NotNull(newCompletionItem.Documentation);
    }

    [Fact]
    public async Task Handle_Resolve_NonTagHelperCompletion_Noops()
    {
        // Arrange
        var lspDescriptionFactory = new Mock<LSPTagHelperTooltipFactory>(MockBehavior.Strict);
        var markdown = new MarkupContent
        {
            Kind = MarkupKind.Markdown,
            Value = "Some Markdown"
        };
        lspDescriptionFactory.Setup(factory => factory.TryCreateTooltip(It.IsAny<AggregateBoundElementDescription>(), MarkupKind.Markdown, out markdown))
            .Returns(true);
        var endpoint = new LegacyRazorCompletionResolveEndpoint(_lspTagHelperTooltipFactory, _vsLspTagHelperTooltipFactory, _completionListCache, LoggerFactory);
        endpoint.ApplyCapabilities(new(), _defaultClientCapability);
        var completionItem = new CompletionItem();
        var parameters = ConvertToBridgedItem(completionItem);
        var requestContext = CreateRazorRequestContext(documentContext: null);

        // Act
        var newCompletionItem = await endpoint.HandleRequestAsync(parameters, requestContext, default);

        // Assert
        Assert.Null(newCompletionItem.Documentation);
    }

    private VSInternalCompletionList CreateLSPCompletionList(params RazorCompletionItem[] razorCompletionItems)
    {
        var completionList = LegacyRazorCompletionEndpoint.CreateLSPCompletionList(razorCompletionItems.ToImmutableArray(), _defaultClientCapability);
        var resultId = _completionListCache.Add(completionList, razorCompletionItems);
        completionList.SetResultId(resultId, completionSetting: null);
        return completionList;
    }

    private VSInternalCompletionItem ConvertToBridgedItem(CompletionItem completionItem)
    {
        var textWriter = new StringWriter();
        Serializer.Serialize(textWriter, completionItem);
        var stringBuilder = textWriter.GetStringBuilder();
        var jsonReader = new JsonTextReader(new StringReader(stringBuilder.ToString()));
        var bridgedItem = Serializer.Deserialize<VSInternalCompletionItem>(jsonReader);
        return bridgedItem;
    }
}

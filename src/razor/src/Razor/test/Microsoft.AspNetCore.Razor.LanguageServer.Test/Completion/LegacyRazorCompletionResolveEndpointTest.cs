// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
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
        var projectManager = CreateProjectSnapshotManager();
        _lspTagHelperTooltipFactory = new StrictMock<LSPTagHelperTooltipFactory>(projectManager).Object;
        _vsLspTagHelperTooltipFactory = new StrictMock<VSLSPTagHelperTooltipFactory>(projectManager).Object;
        _completionListCache = new CompletionListCache();
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
        var projectManager = CreateProjectSnapshotManager();
        var lspDescriptionFactory = new StrictMock<LSPTagHelperTooltipFactory>(projectManager);
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
        var projectManager = CreateProjectSnapshotManager();
        var descriptionFactory = new StrictMock<LSPTagHelperTooltipFactory>(projectManager);
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
        var projectManager = CreateProjectSnapshotManager();
        var lspDescriptionFactory = new StrictMock<LSPTagHelperTooltipFactory>(projectManager);
        var markdown = new MarkupContent
        {
            Kind = MarkupKind.Markdown,
            Value = "Some Markdown"
        };
        lspDescriptionFactory.Setup(factory => factory.TryCreateTooltipAsync(It.IsAny<string>(), It.IsAny<AggregateBoundElementDescription>(), MarkupKind.Markdown, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(markdown));
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
        var projectManager = CreateProjectSnapshotManager();
        var lspDescriptionFactory = new StrictMock<LSPTagHelperTooltipFactory>(projectManager);
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
        lspDescriptionFactory.Setup(factory => factory.TryCreateTooltipAsync(It.IsAny<string>(), It.IsAny<AggregateBoundElementDescription>(), MarkupKind.Markdown, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(markdown));
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
        var completionItems = razorCompletionItems.ToImmutableArray();
        var completionList = LegacyRazorCompletionEndpoint.CreateLSPCompletionList(completionItems, _defaultClientCapability);
        var context = new RazorCompletionResolveContext("file.cshtml", completionItems);
        var resultId = _completionListCache.Add(completionList, context);
        completionList.SetResultId(resultId, completionSetting: null);
        return completionList;
    }

    private VSInternalCompletionItem ConvertToBridgedItem(CompletionItem completionItem)
    {
        var textWriter = new StringWriter();
        ProtocolSerializer.Instance.Serialize(textWriter, completionItem);
        var stringBuilder = textWriter.GetStringBuilder();
        var jsonReader = new JsonTextReader(new StringReader(stringBuilder.ToString()));
        var bridgedItem = ProtocolSerializer.Instance.Deserialize<VSInternalCompletionItem>(jsonReader);
        return bridgedItem;
    }
}

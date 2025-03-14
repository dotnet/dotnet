// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor.Completion;

public class RazorCompletionListProviderTest : LanguageServerTestBase
{
    private readonly IRazorCompletionFactsService _completionFactsService;
    private readonly CompletionListCache _completionListCache;
    private readonly VSInternalClientCapabilities _clientCapabilities;
    private readonly VSInternalCompletionContext _defaultCompletionContext;
    private readonly RazorCompletionOptions _razorCompletionOptions;

    public RazorCompletionListProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _completionFactsService = new LspRazorCompletionFactsService(GetCompletionProviders());
        _completionListCache = new CompletionListCache();
        _clientCapabilities = new VSInternalClientCapabilities()
        {
            TextDocument = new TextDocumentClientCapabilities()
            {
                Completion = new VSInternalCompletionSetting()
                {
                    CompletionItemKind = new CompletionItemKindSetting()
                    {
                        ValueSet = new[] { CompletionItemKind.TagHelper }
                    },
                    CompletionList = new VSInternalCompletionListSetting()
                    {
                        CommitCharacters = true,
                        Data = true,
                    }
                }
            }
        };

        _defaultCompletionContext = new VSInternalCompletionContext();
        _razorCompletionOptions = new RazorCompletionOptions(SnippetsSupported: true, AutoInsertAttributeQuotes: true, CommitElementsWithSpace: true);
    }

    private static IEnumerable<IRazorCompletionItemProvider> GetCompletionProviders()
    {
        // Working around strong naming restriction.
        var tagHelperCompletionService = new TagHelperCompletionService();

        var completionProviders = new IRazorCompletionItemProvider[]
        {
            new DirectiveCompletionItemProvider(),
            new DirectiveAttributeCompletionItemProvider(),
            new DirectiveAttributeParameterCompletionItemProvider(),
            new TagHelperCompletionProvider(tagHelperCompletionService)
        };

        return completionProviders;
    }

    [Fact]
    public void IsApplicableTriggerContext_Deletion_ReturnsFalse()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext()
        {
            InvokeKind = VSInternalCompletionInvokeKind.Deletion
        };

        // Act
        var result = RazorCompletionListProvider.IsApplicableTriggerContext(completionContext);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsApplicableTriggerContext_Explicit_ReturnsTrue()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext()
        {
            InvokeKind = VSInternalCompletionInvokeKind.Explicit
        };

        // Act
        var result = RazorCompletionListProvider.IsApplicableTriggerContext(completionContext);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsApplicableTriggerContext_Typing_ReturnsTrue()
    {
        // Arrange
        var completionContext = new VSInternalCompletionContext()
        {
            InvokeKind = VSInternalCompletionInvokeKind.Typing
        };

        // Act
        var result = RazorCompletionListProvider.IsApplicableTriggerContext(completionContext);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TryConvert_Directive_ReturnsTrue()
    {
        // Arrange
        var completionItem = RazorCompletionItem.CreateDirective(
            displayText: "testDisplay",
            insertText: "testInsert",
            sortText: null,
            descriptionInfo: new("Something"),
            commitCharacters: [],
            isSnippet: false);

        // Act
        Assert.True(RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted));

        // Assert
        Assert.Equal(completionItem.DisplayText, converted.Label);
        Assert.Equal(completionItem.InsertText, converted.InsertText);
        Assert.Equal(completionItem.DisplayText, converted.FilterText);
        Assert.Equal(completionItem.DisplayText, converted.SortText);
        Assert.Null(converted.Detail);
        Assert.Null(converted.Documentation);
    }

    [Fact]
    public void TryConvert_Directive_SerializationDoesNotThrow()
    {
        // Arrange
        var completionItem = RazorCompletionItem.CreateDirective(
            displayText: "testDisplay",
            insertText: "testInsert",
            sortText: null,
            descriptionInfo: new("Something"),
            commitCharacters: [],
            isSnippet: false);

        RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Act & Assert
        JsonSerializer.Serialize(converted);
    }

    [Fact]
    public void TryConvert_DirectiveAttributeTransition_SerializationDoesNotThrow()
    {
        // Arrange
        var directiveAttributeTransitionCompletionItemProvider = new DirectiveAttributeTransitionCompletionItemProvider(TestLanguageServerFeatureOptions.Instance);
        var completionItem = directiveAttributeTransitionCompletionItemProvider.TransitionCompletionItem;
        RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Act & Assert
        JsonSerializer.Serialize(converted);
    }

    [Fact]
    public void TryConvert_DirectiveAttributeTransition_ReturnsTrue()
    {
        // Arrange
        var directiveAttributeTransitionCompletionItemProvider = new DirectiveAttributeTransitionCompletionItemProvider(TestLanguageServerFeatureOptions.Instance);
        var completionItem = directiveAttributeTransitionCompletionItemProvider.TransitionCompletionItem;

        // Act
        Assert.True(RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted));

        // Assert
        Assert.False(converted.Preselect);
        Assert.Equal(completionItem.DisplayText, converted.Label);
        Assert.Equal(completionItem.InsertText, converted.InsertText);
        Assert.Equal(completionItem.DisplayText, converted.FilterText);
        Assert.Equal(completionItem.DisplayText, converted.SortText);
        Assert.Null(converted.Detail);
        Assert.Null(converted.Documentation);
        Assert.NotNull(converted.Command);
    }

    [Fact]
    public void TryConvert_MarkupTransition_ReturnsTrue()
    {
        // Arrange
        var completionItem = MarkupTransitionCompletionItemProvider.MarkupTransitionCompletionItem;

        // Act
        Assert.True(RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted));

        // Assert
        Assert.Equal(completionItem.DisplayText, converted.Label);
        Assert.Equal(completionItem.InsertText, converted.InsertText);
        Assert.Equal(completionItem.DisplayText, converted.FilterText);
        Assert.Equal(completionItem.DisplayText, converted.SortText);
        Assert.Null(converted.Detail);
        Assert.Null(converted.Documentation);
        Assert.Equal(converted.CommitCharacters, completionItem.CommitCharacters.Select(c => c.Character));
    }

    [Fact]
    public void TryConvert_MarkupTransition_SerializationDoesNotThrow()
    {
        // Arrange
        var completionItem = MarkupTransitionCompletionItemProvider.MarkupTransitionCompletionItem;
        RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Act & Assert
        JsonSerializer.Serialize(converted);
    }

    [Fact]
    public void TryConvert_DirectiveAttribute_ReturnsTrue()
    {
        // Arrange
        var completionItem = RazorCompletionItem.CreateDirectiveAttribute(
            displayText: "@testDisplay",
            insertText: "testInsert",
            descriptionInfo: null!,
            commitCharacters: RazorCommitCharacter.CreateArray(["=", ":"]));

        // Act
        Assert.True(RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted));

        // Assert
        Assert.Equal(completionItem.DisplayText, converted.Label);
        Assert.Equal(completionItem.InsertText, converted.InsertText);
        Assert.Equal(completionItem.InsertText, converted.FilterText);
        Assert.Equal(completionItem.DisplayText, converted.SortText);
        Assert.Equal(completionItem.CommitCharacters.Select(c => c.Character), converted.CommitCharacters);
        Assert.Null(converted.Detail);
        Assert.Null(converted.Documentation);
        Assert.Null(converted.Command);
    }

    [Fact]
    public void TryConvert_DirectiveAttributeParameter_ReturnsTrue()
    {
        // Arrange
        var completionItem = RazorCompletionItem.CreateDirectiveAttributeParameter(displayText: "format", insertText: "format", descriptionInfo: null!);

        // Act
        Assert.True(RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted));

        // Assert
        Assert.Equal(completionItem.DisplayText, converted.Label);
        Assert.Equal(completionItem.InsertText, converted.InsertText);
        Assert.Equal(completionItem.InsertText, converted.FilterText);
        Assert.Equal(completionItem.InsertText, converted.SortText);
        Assert.Null(converted.Detail);
        Assert.Null(converted.Documentation);
        Assert.Null(converted.Command);
    }

    [Fact]
    public void TryConvert_TagHelperElement_ReturnsTrue()
    {
        // Arrange
        var completionItem = RazorCompletionItem.CreateTagHelperElement(displayText: "format", insertText: "format", descriptionInfo: null!, commitCharacters: []);

        // Act
        Assert.True(RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted));

        // Assert
        Assert.Equal(completionItem.DisplayText, converted.Label);
        Assert.Equal(completionItem.InsertText, converted.InsertText);
        Assert.Equal(completionItem.InsertText, converted.FilterText);
        Assert.Equal(completionItem.InsertText, converted.SortText);
        Assert.Null(converted.Detail);
        Assert.Null(converted.Documentation);
        Assert.Null(converted.Command);
    }

    [Fact]
    public void TryConvert_TagHelperAttribute_ForBool_ReturnsTrue()
    {
        // Arrange
        var attributeCompletionDescription = new AggregateBoundAttributeDescription([
            new BoundAttributeDescriptionInfo("System.Boolean", "Stuff", "format", "SomeDocs")
        ]);

        var completionItem = RazorCompletionItem.CreateTagHelperAttribute(
            displayText: "format",
            insertText: "format",
            sortText: null,
            descriptionInfo: attributeCompletionDescription,
            commitCharacters: [],
            isSnippet: false);

        // Act
        Assert.True(RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted));

        // Assert
        Assert.Equal(completionItem.DisplayText, converted.Label);
        Assert.Equal("format", converted.InsertText);
        Assert.Equal(InsertTextFormat.Plaintext, converted.InsertTextFormat);
        Assert.Equal(completionItem.InsertText, converted.FilterText);
        Assert.Equal(completionItem.InsertText, converted.SortText);
        Assert.Null(converted.Detail);
        Assert.Null(converted.Documentation);
        Assert.Null(converted.Command);
    }

    [Fact]
    public void TryConvert_TagHelperAttribute_ForHtml_ReturnsTrue()
    {
        // Arrange
        var completionItem = RazorCompletionItem.CreateTagHelperAttribute(
            displayText: "format",
            insertText: "format=\"$0\"",
            sortText: null,
            descriptionInfo: AggregateBoundAttributeDescription.Empty,
            commitCharacters: [],
            isSnippet: true);

        // Act
        Assert.True(RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted));

        // Assert
        Assert.Equal(completionItem.DisplayText, converted.Label);
        Assert.Equal("format=\"$0\"", converted.InsertText);
        Assert.Equal(InsertTextFormat.Snippet, converted.InsertTextFormat);
        Assert.Equal(completionItem.DisplayText, converted.FilterText);
        Assert.Equal(completionItem.DisplayText, converted.SortText);
        Assert.Null(converted.Detail);
        Assert.Null(converted.Documentation);
        Assert.Null(converted.Command);
    }

    [Fact]
    public void TryConvert_TagHelperAttribute_ReturnsTrue()
    {
        // Arrange
        var completionItem = RazorCompletionItem.CreateTagHelperAttribute(
            displayText: "format",
            insertText: "format=\"$0\"",
            sortText: null,
            descriptionInfo: null!,
            commitCharacters: [],
            isSnippet: true);

        // Act
        Assert.True(RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted));

        // Assert
        Assert.Equal(completionItem.DisplayText, converted.Label);
        Assert.Equal("format=\"$0\"", converted.InsertText);
        Assert.Equal(InsertTextFormat.Snippet, converted.InsertTextFormat);
        Assert.Equal(completionItem.DisplayText, converted.FilterText);
        Assert.Equal(completionItem.DisplayText, converted.SortText);
        Assert.Null(converted.Detail);
        Assert.Null(converted.Documentation);
        Assert.Null(converted.Command);
    }

    // This is more of an integration test to validate that all the pieces work together
    [Theory]
    [InlineData("@$$")]
    [InlineData("@$$\r\n")]
    [InlineData("@page\r\n@$$")]
    [InlineData("@page\r\n@$$\r\n")]
    [InlineData("@page\r\n<div></div>\r\n@f$$")]
    [InlineData("@page\r\n<div></div>\r\n@f$$\r\n")]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/4547")]
    [WorkItem("https://github.com/dotnet/razor/issues/9955")]
    public void GetCompletionList_ProvidesDirectiveCompletionItems(string documentText)
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        TestFileMarkupParser.GetPosition(documentText, out documentText, out var cursorPosition);
        var codeDocument = CreateCodeDocument(documentText, documentPath);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = provider.GetCompletionList(
            codeDocument, absoluteIndex: cursorPosition, _defaultCompletionContext, _clientCapabilities, existingCompletions: null, _razorCompletionOptions);

        // Assert

        Assert.NotNull(completionList);

        // These are the default directives that don't need to be separately registered, they should always be part of the completion list.
        Assert.Collection(completionList.Items,
            DirectiveVerifier.DefaultDirectiveCollectionVerifiers
        );
    }

    [Fact]
    public void GetCompletionListAsync_ProvidesDirectiveCompletions_IncompleteTriggerOnDeletion()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var codeDocument = CreateCodeDocument("@", documentPath);
        var completionContext = new VSInternalCompletionContext()
        {
            TriggerKind = CompletionTriggerKind.TriggerForIncompleteCompletions,
            InvokeKind = VSInternalCompletionInvokeKind.Deletion,
        };

        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = provider.GetCompletionList(
            codeDocument, absoluteIndex: 1, completionContext, _clientCapabilities, existingCompletions: null, _razorCompletionOptions);

        // Assert
        Assert.NotNull(completionList);

        // These are the default directives that don't need to be separately registered, they should always be part of the completion list.
        Assert.Contains(completionList.Items, item => item.InsertText == "addTagHelper");
        Assert.Contains(completionList.Items, item => item.InsertText == "removeTagHelper");
        Assert.Contains(completionList.Items, item => item.InsertText == "tagHelperPrefix");
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/4547")]
    public void GetCompletionList_ProvidesInjectOnIncomplete_KeywordIn()
    {
        // Arrange
        var documentPath = "C:/path/to/document.razor";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        builder.Metadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, [tagHelper]);
        var codeDocument = CreateCodeDocument("@in", documentPath);
        codeDocument.SetTagHelperContext(tagHelperContext);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);
        var completionContext = new VSInternalCompletionContext()
        {
            TriggerKind = CompletionTriggerKind.TriggerForIncompleteCompletions,
        };

        // Act
        var completionList = provider.GetCompletionList(
            codeDocument, absoluteIndex: 1, completionContext, _clientCapabilities, existingCompletions: null, _razorCompletionOptions);

        // Assert
        Assert.NotNull(completionList);

        Assert.Collection(completionList.Items,
            DirectiveVerifier.DefaultDirectiveCollectionVerifiers
        );
    }

    [Fact]
    public void GetCompletionList_DoesNotProvideInjectOnInvoked()
    {
        // Arrange
        var documentPath = "C:/path/to/document.razor";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        builder.Metadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, [tagHelper]);
        var codeDocument = CreateCodeDocument("@inje", documentPath);
        codeDocument.SetTagHelperContext(tagHelperContext);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);
        var completionContext = new VSInternalCompletionContext()
        {
            TriggerKind = CompletionTriggerKind.TriggerCharacter,
        };

        // Act
        var completionList = provider.GetCompletionList(
            codeDocument, absoluteIndex: 1, completionContext, _clientCapabilities, existingCompletions: null, _razorCompletionOptions);

        // Assert
        Assert.NotNull(completionList);
        Assert.Empty(completionList.Items);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/4547")]
    public void GetCompletionList_ProvidesInjectOnIncomplete()
    {
        // Arrange
        var documentPath = "C:/path/to/document.razor";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        builder.Metadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, [tagHelper]);
        var codeDocument = CreateCodeDocument("@inje", documentPath);
        codeDocument.SetTagHelperContext(tagHelperContext);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);
        var completionContext = new VSInternalCompletionContext()
        {
            TriggerKind = CompletionTriggerKind.TriggerForIncompleteCompletions,
        };

        // Act
        var completionList = provider.GetCompletionList(
            codeDocument, absoluteIndex: 1, completionContext, _clientCapabilities, existingCompletions: null, _razorCompletionOptions);

        // Assert
        Assert.NotNull(completionList);

        Assert.Collection(completionList.Items,
            DirectiveVerifier.DefaultDirectiveCollectionVerifiers
        );
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public void GetCompletionList_ProvidesTagHelperElementCompletionItems()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        builder.Metadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, [tagHelper]);
        var codeDocument = CreateCodeDocument("<", documentPath);
        codeDocument.SetTagHelperContext(tagHelperContext);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = provider.GetCompletionList(
            codeDocument, absoluteIndex: 1, _defaultCompletionContext, _clientCapabilities, existingCompletions: null, _razorCompletionOptions);

        // Assert
        Assert.NotNull(completionList);
        Assert.Contains(completionList.Items, item => item.InsertText == "Test");
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public void GetCompletionList_ProvidesTagHelperAttributeItems()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "*");
        builder.BindAttribute(attribute =>
        {
            attribute.Name = "testAttribute";
            attribute.TypeName = typeof(string).FullName;
            attribute.SetMetadata(PropertyName("TestAttribute"));
        });
        builder.Metadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, [tagHelper]);
        var codeDocument = CreateCodeDocument("<test  ", documentPath);
        codeDocument.SetTagHelperContext(tagHelperContext);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = provider.GetCompletionList(
            codeDocument, absoluteIndex: 6, _defaultCompletionContext, _clientCapabilities, existingCompletions: null, _razorCompletionOptions);

        // Assert
        Assert.NotNull(completionList);
        Assert.Contains(completionList.Items, item => item.InsertText == "testAttribute=\"$0\"");
    }

    [Fact]
    public void GetCompletionList_ProvidesTagHelperAttributeItems_AttributeQuotesOff()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "*");
        builder.BindAttribute(attribute =>
        {
            attribute.Name = "testAttribute";
            attribute.TypeName = typeof(string).FullName;
            attribute.SetMetadata(PropertyName("TestAttribute"));
        });
        builder.SetMetadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, [tagHelper]);
        var codeDocument = CreateCodeDocument("<test  ", documentPath);
        codeDocument.SetTagHelperContext(tagHelperContext);

        // Set up desired options
        var razorCompletionOptions = new RazorCompletionOptions(SnippetsSupported: true, AutoInsertAttributeQuotes: false, CommitElementsWithSpace: true);

        var completionFactsService = new LspRazorCompletionFactsService(GetCompletionProviders());
        var provider = new RazorCompletionListProvider(completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = provider.GetCompletionList(
            codeDocument, absoluteIndex: 6, _defaultCompletionContext, _clientCapabilities, existingCompletions: null, razorCompletionOptions);

        // Assert
        Assert.NotNull(completionList);
        Assert.Contains(completionList.Items, item => item.InsertText == "testAttribute=$0");
    }

    private static RazorCodeDocument CreateCodeDocument(string text, string documentFilePath)
    {
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: documentFilePath);
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument);
        codeDocument.SetSyntaxTree(syntaxTree);
        var tagHelperDocumentContext = TagHelperDocumentContext.Create(prefix: string.Empty, tagHelpers: []);
        codeDocument.SetTagHelperContext(tagHelperDocumentContext);
        return codeDocument;
    }
}

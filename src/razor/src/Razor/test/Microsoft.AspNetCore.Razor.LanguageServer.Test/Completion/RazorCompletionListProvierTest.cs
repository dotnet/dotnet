// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class RazorCompletionListProvierTest : LanguageServerTestBase
{
    private readonly IRazorCompletionFactsService _completionFactsService;
    private readonly CompletionListCache _completionListCache;
    private readonly VSInternalClientCapabilities _clientCapabilities;
    private readonly VSInternalCompletionContext _defaultCompletionContext;

    public RazorCompletionListProvierTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {     
        _completionFactsService = new RazorCompletionFactsService(GetCompletionProviders());
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
    }

    private static IEnumerable<IRazorCompletionItemProvider> GetCompletionProviders(IOptionsMonitor<RazorLSPOptions> optionsMonitor = null)
    {
        // Working around strong naming restriction.
        var tagHelperFactsService = new TagHelperFactsService();
        var tagHelperCompletionService = new LanguageServerTagHelperCompletionService(tagHelperFactsService);

        optionsMonitor ??= TestRazorLSPOptionsMonitor.Create();

        var completionProviders = new IRazorCompletionItemProvider[]
        {
            new DirectiveCompletionItemProvider(),
            new DirectiveAttributeCompletionItemProvider(tagHelperFactsService),
            new DirectiveAttributeParameterCompletionItemProvider(tagHelperFactsService),
            new TagHelperCompletionProvider(tagHelperCompletionService, new DefaultHtmlFactsService(), tagHelperFactsService, optionsMonitor)
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
        var completionItem = new RazorCompletionItem("testDisplay", "testInsert", RazorCompletionItemKind.Directive);
        var description = "Something";
        completionItem.SetDirectiveCompletionDescription(new DirectiveCompletionDescription(description));

        // Act
        var result = RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Assert
        Assert.True(result);
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
        var completionItem = new RazorCompletionItem("testDisplay", "testInsert", RazorCompletionItemKind.Directive);
        var description = "Something";
        completionItem.SetDirectiveCompletionDescription(new DirectiveCompletionDescription(description));
        RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Act & Assert
        JsonConvert.SerializeObject(converted);
    }

    [Fact]
    public void TryConvert_DirectiveAttributeTransition_SerializationDoesNotThrow()
    {
        // Arrange
        var completionItem = DirectiveAttributeTransitionCompletionItemProvider.TransitionCompletionItem;
        RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Act & Assert
        JsonConvert.SerializeObject(converted);
    }

    [Fact]
    public void TryConvert_DirectiveAttributeTransition_ReturnsTrue()
    {
        // Arrange
        var completionItem = DirectiveAttributeTransitionCompletionItemProvider.TransitionCompletionItem;

        // Act
        var result = RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Assert
        Assert.True(result);
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
        var result = RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Assert
        Assert.True(result);
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
        JsonConvert.SerializeObject(converted);
    }

    [Fact]
    public void TryConvert_DirectiveAttribute_ReturnsTrue()
    {
        // Arrange
        var completionItem = new RazorCompletionItem("@testDisplay", "testInsert", RazorCompletionItemKind.DirectiveAttribute, commitCharacters: RazorCommitCharacter.FromArray(new[] { "=", ":" }));

        // Act
        var result = RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Assert
        Assert.True(result);
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
        var completionItem = new RazorCompletionItem("format", "format", RazorCompletionItemKind.DirectiveAttributeParameter);

        // Act
        var result = RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Assert
        Assert.True(result);
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
        var completionItem = new RazorCompletionItem("format", "format", RazorCompletionItemKind.TagHelperElement);

        // Act
        var result = RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Assert
        Assert.True(result);
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
        var completionItem = new RazorCompletionItem("format", "format", RazorCompletionItemKind.TagHelperAttribute);
        var attributeCompletionDescription = new AggregateBoundAttributeDescription(ImmutableArray.Create(
            new BoundAttributeDescriptionInfo("System.Boolean", "Stuff", "format", "SomeDocs")));
        completionItem.SetAttributeCompletionDescription(attributeCompletionDescription);

        // Act
        var result = RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Assert
        Assert.True(result);
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
        var completionItem = new RazorCompletionItem("format", "format=\"$0\"", RazorCompletionItemKind.TagHelperAttribute, isSnippet: true);
        var attributeCompletionDescription = AggregateBoundAttributeDescription.Empty;
        completionItem.SetAttributeCompletionDescription(attributeCompletionDescription);

        // Act
        var result = RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Assert
        Assert.True(result);
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
        var completionItem = new RazorCompletionItem("format", "format=\"$0\"", RazorCompletionItemKind.TagHelperAttribute, isSnippet: true);

        // Act
        var result = RazorCompletionListProvider.TryConvert(completionItem, _clientCapabilities, out var converted);

        // Assert
        Assert.True(result);
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
    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/4547")]
    public async Task GetCompletionListAsync_ProvidesDirectiveCompletionItems()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var codeDocument = CreateCodeDocument("@");
        var documentContext = TestDocumentContext.From(documentPath, codeDocument, hostDocumentVersion: 0);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 1, _defaultCompletionContext, documentContext, _clientCapabilities, existingCompletions: null, DisposalToken);

        // Assert

        // These are the default directives that don't need to be separately registered, they should always be part of the completion list.
        Assert.Collection(completionList.Items,
            item => Assert.Equal("addTagHelper", item.InsertText),
            item => AssertDirectiveSnippet(item, "addTagHelper"),
            item => Assert.Equal("removeTagHelper", item.InsertText),
            item => AssertDirectiveSnippet(item, "removeTagHelper"),
            item => Assert.Equal("tagHelperPrefix", item.InsertText),
            item => AssertDirectiveSnippet(item, "tagHelperPrefix")
        );
    }

    [Fact]
    public async Task GetCompletionListAsync_ProvidesDirectiveCompletions_IncompleteTriggerOnDeletion()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var codeDocument = CreateCodeDocument("@");
        var documentContext = TestDocumentContext.From(documentPath, codeDocument, hostDocumentVersion: 0);
        var completionContext = new VSInternalCompletionContext()
        {
            TriggerKind = CompletionTriggerKind.TriggerForIncompleteCompletions,
            InvokeKind = VSInternalCompletionInvokeKind.Deletion,
        };
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 1, completionContext, documentContext, _clientCapabilities, existingCompletions: null, DisposalToken);

        // Assert

        // These are the default directives that don't need to be separately registered, they should always be part of the completion list.
        Assert.Contains(completionList.Items, item => item.InsertText == "addTagHelper");
        Assert.Contains(completionList.Items, item => item.InsertText == "removeTagHelper");
        Assert.Contains(completionList.Items, item => item.InsertText == "tagHelperPrefix");
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/4547")]
    public async Task GetCompletionListAsync_ProvidesInjectOnIncomplete_KeywordIn()
    {
        // Arrange
        var documentPath = "C:/path/to/document.razor";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        builder.Metadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, new[] { tagHelper });
        var codeDocument = CreateCodeDocument("@in");
        codeDocument.SetTagHelperContext(tagHelperContext);
        var documentContext = TestDocumentContext.From(documentPath, codeDocument, hostDocumentVersion: 0);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);
        var completionContext = new VSInternalCompletionContext()
        {
            TriggerKind = CompletionTriggerKind.TriggerForIncompleteCompletions,
        };

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 1, completionContext, documentContext, _clientCapabilities, existingCompletions: null, DisposalToken);

        // Assert
        Assert.Collection(completionList.Items,
            item => Assert.Equal("addTagHelper", item.InsertText),
            item => AssertDirectiveSnippet(item, "addTagHelper"),
            item => Assert.Equal("removeTagHelper", item.InsertText),
            item => AssertDirectiveSnippet(item, "removeTagHelper"),
            item => Assert.Equal("tagHelperPrefix", item.InsertText),
            item => AssertDirectiveSnippet(item, "tagHelperPrefix")
        );
    }

    [Fact]
    public async Task GetCompletionListAsync_DoesNotProvideInjectOnInvoked()
    {
        // Arrange
        var documentPath = "C:/path/to/document.razor";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        builder.Metadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, new[] { tagHelper });
        var codeDocument = CreateCodeDocument("@inje");
        codeDocument.SetTagHelperContext(tagHelperContext);
        var documentContext = TestDocumentContext.From(documentPath, codeDocument, hostDocumentVersion: 0);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);
        var completionContext = new VSInternalCompletionContext()
        {
            TriggerKind = CompletionTriggerKind.TriggerCharacter,
        };

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 1, completionContext, documentContext, _clientCapabilities, existingCompletions: null, DisposalToken);

        // Assert
        Assert.Empty(completionList.Items);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/4547")]
    public async Task GetCompletionListAsync_ProvidesInjectOnIncomplete()
    {
        // Arrange
        var documentPath = "C:/path/to/document.razor";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        builder.Metadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, new[] { tagHelper });
        var codeDocument = CreateCodeDocument("@inje");
        codeDocument.SetTagHelperContext(tagHelperContext);
        var documentContext = TestDocumentContext.From(documentPath, codeDocument, hostDocumentVersion: 0);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);
        var completionContext = new VSInternalCompletionContext()
        {
            TriggerKind = CompletionTriggerKind.TriggerForIncompleteCompletions,
        };

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 1, completionContext, documentContext, _clientCapabilities, existingCompletions: null, DisposalToken);

        // Assert
        Assert.Collection(completionList.Items,
            item => Assert.Equal("addTagHelper", item.InsertText),
            item => AssertDirectiveSnippet(item, "addTagHelper"),
            item => Assert.Equal("removeTagHelper", item.InsertText),
            item => AssertDirectiveSnippet(item, "removeTagHelper"),
            item => Assert.Equal("tagHelperPrefix", item.InsertText),
            item => AssertDirectiveSnippet(item, "tagHelperPrefix")
        );
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task GetCompletionListAsync_ProvidesTagHelperElementCompletionItems()
    {
        // Arrange
        var documentPath = "C:/path/to/document.cshtml";
        var builder = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestTagHelper", "TestAssembly");
        builder.TagMatchingRule(rule => rule.TagName = "Test");
        builder.Metadata(TypeName("TestNamespace.TestTagHelper"));
        var tagHelper = builder.Build();
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, new[] { tagHelper });
        var codeDocument = CreateCodeDocument("<");
        codeDocument.SetTagHelperContext(tagHelperContext);
        var documentContext = TestDocumentContext.From(documentPath, codeDocument, hostDocumentVersion: 0);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 1, _defaultCompletionContext, documentContext, _clientCapabilities, existingCompletions: null, DisposalToken);

        // Assert
        Assert.Contains(completionList.Items, item => item.InsertText == "Test");
    }

    // This is more of an integration test to validate that all the pieces work together
    [Fact]
    public async Task GetCompletionListAsync_ProvidesTagHelperAttributeItems()
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
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, new[] { tagHelper });
        var codeDocument = CreateCodeDocument("<test  ");
        codeDocument.SetTagHelperContext(tagHelperContext);
        var documentContext = TestDocumentContext.From(documentPath, codeDocument, hostDocumentVersion: 0);
        var provider = new RazorCompletionListProvider(_completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 6, _defaultCompletionContext, documentContext, _clientCapabilities, existingCompletions: null, DisposalToken);

        // Assert
        Assert.Contains(completionList.Items, item => item.InsertText == "testAttribute=\"$0\"");
    }

    [Fact]
    public async Task GetCompletionListAsync_ProvidesTagHelperAttributeItems_AttributeQuotesOff()
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
        var tagHelperContext = TagHelperDocumentContext.Create(prefix: string.Empty, new[] { tagHelper });
        var codeDocument = CreateCodeDocument("<test  ");
        codeDocument.SetTagHelperContext(tagHelperContext);
        var documentContext = TestDocumentContext.From(documentPath, codeDocument, hostDocumentVersion: 0);

        // Set up a custom options monitor with desired options
        var optionsMonitor = TestRazorLSPOptionsMonitor.Create();
        await optionsMonitor.UpdateAsync(optionsMonitor.CurrentValue with { AutoInsertAttributeQuotes = false }, DisposalToken);

        var completionFactsService = new RazorCompletionFactsService(GetCompletionProviders(optionsMonitor));
        var provider = new RazorCompletionListProvider(completionFactsService, _completionListCache, LoggerFactory);

        // Act
        var completionList = await provider.GetCompletionListAsync(
            absoluteIndex: 6, _defaultCompletionContext, documentContext, _clientCapabilities, existingCompletions: null, DisposalToken);

        // Assert
        Assert.Contains(completionList.Items, item => item.InsertText == "testAttribute=$0");
    }

    private static RazorCodeDocument CreateCodeDocument(string text)
    {
        var codeDocument = TestRazorCodeDocument.CreateEmpty();
        var sourceDocument = TestRazorSourceDocument.Create(text);
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument);
        codeDocument.SetSyntaxTree(syntaxTree);
        var tagHelperDocumentContext = TagHelperDocumentContext.Create(prefix: string.Empty, Enumerable.Empty<TagHelperDescriptor>());
        codeDocument.SetTagHelperContext(tagHelperDocumentContext);
        return codeDocument;
    }

    private static void AssertDirectiveSnippet(CompletionItem completionItem, string directive)
    {
        Assert.StartsWith(directive, completionItem.InsertText);
        Assert.Equal(DirectiveCompletionItemProvider.s_singleLineDirectiveSnippets[directive].InsertText, completionItem.InsertText);
        Assert.Equal(CompletionItemKind.Snippet, completionItem.Kind);
    }
}

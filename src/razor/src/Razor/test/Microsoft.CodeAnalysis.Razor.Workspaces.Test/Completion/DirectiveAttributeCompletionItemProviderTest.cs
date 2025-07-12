// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.IntegrationTests;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.Completion;

public class DirectiveAttributeCompletionItemProviderTest : RazorToolingIntegrationTestBase
{
    private readonly DirectiveAttributeCompletionItemProvider _provider;
    private readonly TagHelperDocumentContext _defaultTagHelperContext;
    private readonly RazorCompletionOptions _defaultRazorCompletionOptions;
    internal override RazorFileKind? FileKind => RazorFileKind.Component;
    internal override bool UseTwoPhaseCompilation => true;

    public DirectiveAttributeCompletionItemProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _provider = new DirectiveAttributeCompletionItemProvider();

        // Most of these completions rely on stuff in the web namespace.
        ImportItems.Add(CreateProjectItem(
            "_Imports.razor",
            "@using Microsoft.AspNetCore.Components.Web"));

        var codeDocument = GetCodeDocument(string.Empty);
        _defaultTagHelperContext = codeDocument.GetRequiredTagHelperContext();
        _defaultRazorCompletionOptions = new RazorCompletionOptions(SnippetsSupported: true, AutoInsertAttributeQuotes: true, CommitElementsWithSpace: true);
    }

    private RazorCodeDocument GetCodeDocument(string content)
    {
        var result = CompileToCSharp(content, throwOnFailure: false);
        return result.CodeDocument;
    }

    [Fact]
    public void GetCompletionItems_OnNonAttributeArea_ReturnsEmptyCollection()
    {
        // Arrange
        var context = CreateRazorCompletionContext("<in$$put @  />");

        // Act
        var completions = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetCompletionItems_OnDirectiveAttributeParameter_ReturnsEmptyCollection()
    {
        // Arrange
        var context = CreateRazorCompletionContext("<input @bind:f$$o  />");

        // Act
        var completions = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetCompletionItems_OnDirectiveAttributeName_bind_ReturnsCompletions()
    {
        // Arrange
        var context = CreateRazorCompletionContext("<input @$$  />");

        // Act
        var completions = _provider.GetCompletionItems(context);

        // Assert
        AssertContains(completions, "bind", "@bind", ["=", ":"]);
    }

    [Fact]
    public void GetCompletionItems_OnDirectiveAttributeName_attributes_ReturnsCompletions()
    {
        // Arrange
        var context = CreateRazorCompletionContext("<input @$$  />");

        // Act
        var completions = _provider.GetCompletionItems(context);

        // Assert
        AssertContains(completions, "attributes", "@attributes", ["="]);
    }

    [Fact]
    public void GetCompletionItems_AttributeAreaEndOfSelfClosingTag_ReturnsEmptyCollection()
    {
        // Arrange
        var context = CreateRazorCompletionContext("<input @bind:fo $$ />");

        // Act
        var completions = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetCompletionItems_AttributeAreaEndOfOpeningTag_ReturnsEmptyCollection()
    {
        // Arrange
        var context = CreateRazorCompletionContext("<input @bind:fo $$  ></input>");

        // Act
        var completions = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetCompletionItems_ExistingAttribute_LeadingEdge_ReturnsEmptyCollection()
    {
        // Arrange
        var context = CreateRazorCompletionContext("<input $$src=\"xyz\" />");

        // Act
        var completions = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetCompletionItems_ExistingAttribute_TrailingEdge_ReturnsEmptyCollection()
    {
        // Arrange
        var context = CreateRazorCompletionContext("<input src=\"xyz$$\" />");

        // Act
        var completions = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetCompletionItems_ExistingAttribute_Partial_ReturnsEmptyCollection()
    {
        // Arrange
        var context = CreateRazorCompletionContext("<svg xml:$$ ></svg>");

        // Act
        var completions = _provider.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetAttributeCompletions_NoDescriptorsForTag_ReturnsEmptyCollection()
    {
        // Arrange
        var owner = GetOwner("<foobarbaz @bin$$></foobarbar>");
        var documentContext = TagHelperDocumentContext.Create(string.Empty, tagHelpers: []);

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner!, "@bin", "foobarbaz", [], documentContext, _defaultRazorCompletionOptions);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetAttributeCompletions_NoDirectiveAttributesForTag_ReturnsEmptyCollection()
    {
        // Arrange
        var descriptor = TagHelperDescriptorBuilder.Create("CatchAll", "TestAssembly");
        descriptor.BoundAttributeDescriptor(boundAttribute => boundAttribute.Name = "Test");
        descriptor.TagMatchingRule(rule => rule.RequireTagName("*"));
        var documentContext = TagHelperDocumentContext.Create(string.Empty, [descriptor.Build()]);
        var owner = GetOwner("<input @bin$$></input>");

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner, "@bin", "input", [], documentContext, _defaultRazorCompletionOptions);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetAttributeCompletions_SelectedDirectiveAttribute_IsIncludedInCompletions()
    {
        // Arrange
        var attributeNames = ImmutableArray.Create("@bind");
        var owner = GetOwner("<input @bind$$></input>");

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner, "@bind", "input", attributeNames, _defaultTagHelperContext, _defaultRazorCompletionOptions);

        // Assert
        AssertContains(completions, "bind=\"$0\"", "@bind", ["=", ":"]);
    }

    [Fact]
    public void GetAttributeCompletions_NonIndexer_ReturnsCompletion()
    {
        // Arrange
        var owner = GetOwner("<input @$$></input>");

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner, "@", "input", [], _defaultTagHelperContext, _defaultRazorCompletionOptions);

        // Assert
        AssertContains(completions, "bind=\"$0\"", "@bind", ["=", ":"]);
    }

    [Fact]
    public void GetAttributeCompletions_WithNoAutoQuotesOption_ReturnsNonQuotedSnippet()
    {
        // Arrange
        var owner = GetOwner("<input @$$></input>");
        var noAutoQuotesRazorCompletionOptions = new RazorCompletionOptions(SnippetsSupported: true, AutoInsertAttributeQuotes: false, CommitElementsWithSpace: true);

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner, "@", "input", [], _defaultTagHelperContext, noAutoQuotesRazorCompletionOptions);

        // Assert
        AssertContains(completions, "bind=$0", "@bind", ["=", ":"]);
    }

    [Fact]
    public void GetAttributeCompletions_WithNoSnippetsOption_ReturnsNoSnippets()
    {
        // Arrange
        var owner = GetOwner("<input @$$></input>");
        var noAutoQuotesRazorCompletionOptions = new RazorCompletionOptions(SnippetsSupported: false, AutoInsertAttributeQuotes: true, CommitElementsWithSpace: true);

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner, "@", "input", [], _defaultTagHelperContext, noAutoQuotesRazorCompletionOptions);

        // Assert
        AssertContains(completions, "bind", "@bind", ["=", ":"]);
    }

    [Fact]
    public void GetAttributeCompletions_ExistingAttrubteWithValue_ReturnsNoSnippets()
    {
        // Arrange
        var owner = GetOwner("<input @bi$$nd=\"foo\"></input>");
        var noAutoQuotesRazorCompletionOptions = new RazorCompletionOptions(SnippetsSupported: false, AutoInsertAttributeQuotes: true, CommitElementsWithSpace: true);

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner, "@", "input", [], _defaultTagHelperContext, noAutoQuotesRazorCompletionOptions);

        // Assert
        AssertContains(completions, "bind", "@bind", ["=", ":"]);
    }

    [Fact]
    public void GetAttributeCompletions_Indexer_ReturnsCompletion()
    {
        // Arrange
        var owner = GetOwner("<input @$$></input>");

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner, "@", "input", [], _defaultTagHelperContext, _defaultRazorCompletionOptions);

        // Assert
        AssertContains(completions, "bind-", "@bind-...", []);
    }

    [Fact]
    public void GetAttributeCompletions_BaseDirectiveAttributeAlreadyExists_IncludesBaseAttribute()
    {
        // Arrange
        var attributeNames = ImmutableArray.Create("@bind", "@");
        var owner = GetOwner("<input @$$></input>");

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner, "@", "input", attributeNames, _defaultTagHelperContext, _defaultRazorCompletionOptions);

        // Assert
        AssertContains(completions, "bind=\"$0\"", "@bind", ["=", ":"]);
    }

    [Fact]
    public void GetAttributeCompletions_BaseDirectiveAttributeAndParameterVariationsExist_ExcludesCompletion()
    {
        // Arrange
        var attributeNames = ImmutableArray.Create(
            "@bind",
            "@bind:format",
            "@bind:event",
            "@bind:culture",
            "@bind:get",
            "@bind:set",
            "@bind:after",
            "@");
        var owner = GetOwner("<input @$$></input>");

        // Act
        var completions = DirectiveAttributeCompletionItemProvider.GetAttributeCompletions(owner, "@", "input", attributeNames, _defaultTagHelperContext, _defaultRazorCompletionOptions);

        // Assert
        AssertDoesNotContain(completions, "bind", "@bind");
    }

    private static void AssertContains(ImmutableArray<RazorCompletionItem> completions, string insertText, string displayText, ImmutableArray<string> commitCharacters)
    {
        displayText ??= insertText;

        Assert.Contains(completions, completion =>
            insertText == completion.InsertText &&
            displayText == completion.DisplayText &&
            commitCharacters.SequenceEqual(completion.CommitCharacters.Select(c => c.Character)) &&
            RazorCompletionItemKind.DirectiveAttribute == completion.Kind);
    }

    private static void AssertDoesNotContain(IReadOnlyList<RazorCompletionItem> completions, string insertText, string displayText)
    {
        displayText ??= insertText;

        Assert.DoesNotContain(completions, completion => insertText == completion.InsertText &&
               displayText == completion.DisplayText &&
               RazorCompletionItemKind.DirectiveAttribute == completion.Kind);
    }

    private RazorCompletionContext CreateRazorCompletionContext(TestCode testCode)
    {
        var codeDocument = GetCodeDocument(testCode.Text);
        var syntaxTree = codeDocument.GetRequiredSyntaxTree();
        var tagHelperContext = codeDocument.GetRequiredTagHelperContext();

        var owner = syntaxTree.Root.FindInnermostNode(testCode.Position, includeWhitespace: true, walkMarkersBack: true);
        owner = AbstractRazorCompletionFactsService.AdjustSyntaxNodeForWordBoundary(owner, testCode.Position);

        return new RazorCompletionContext(testCode.Position, owner, syntaxTree, tagHelperContext);
    }

    private RazorSyntaxNode GetOwner(string testCodeText)
    {
        return CreateRazorCompletionContext(testCodeText).Owner!;
    }
}

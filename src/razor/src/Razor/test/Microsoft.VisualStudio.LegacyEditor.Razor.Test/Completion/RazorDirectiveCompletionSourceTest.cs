// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Span = Microsoft.VisualStudio.Text.Span;
using WorkspacesSR = Microsoft.CodeAnalysis.Razor.Workspaces.Resources.SR;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

public class RazorDirectiveCompletionSourceTest(ITestOutputHelper testOutput) : VisualStudioTestBase(testOutput)
{
    private static readonly ImmutableArray<DirectiveDescriptor> s_defaultDirectives = ImmutableArray.Create(
    [
        CSharpCodeParser.AddTagHelperDirectiveDescriptor,
        CSharpCodeParser.RemoveTagHelperDirectiveDescriptor,
        CSharpCodeParser.TagHelperPrefixDirectiveDescriptor,
        CSharpCodeParser.UsingDirectiveDescriptor
    ]);

    private readonly IRazorCompletionFactsService _completionFactsService = new LegacyRazorCompletionFactsService();

    [UIFact]
    public async Task GetCompletionContextAsync_DoesNotProvideCompletionsPriorToParseResults()
    {
        // Arrange
        var text = "@validCompletion";
        var parserMock = new StrictMock<IVisualStudioRazorParser>();
        parserMock
            .Setup(p => p.GetLatestCodeDocumentAsync(It.IsAny<ITextSnapshot>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null); // CodeDocument will be null faking a parser without a parse.
        var completionSource = new RazorDirectiveCompletionSource(parserMock.Object, _completionFactsService);
        var documentSnapshot = new StringTextSnapshot(text);
        var textBuffer = new TestTextBuffer(documentSnapshot, VsMocks.ContentTypes.LegacyRazorCore);
        var triggerLocation = new SnapshotPoint(documentSnapshot, 4);
        var applicableSpan = new SnapshotSpan(documentSnapshot, new Span(1, text.Length - 1 /* validCompletion */));

        // Act
        var completionContext = await completionSource.GetCompletionContextAsync(
            session: null!,
            new CompletionTrigger(CompletionTriggerReason.Invoke, triggerLocation.Snapshot),
            triggerLocation,
            applicableSpan,
            DisposalToken);

        // Assert
        Assert.Empty(completionContext.ItemList);
    }

    [UIFact]
    public async Task GetCompletionContextAsync_DoesNotProvideCompletionsWhenNotAtCompletionPoint()
    {
        // Arrange
        var text = "@(NotValidCompletionLocation)";
        var parser = CreateParser(text);
        var completionSource = new RazorDirectiveCompletionSource(parser, _completionFactsService);
        var documentSnapshot = new StringTextSnapshot(text);
        var textBuffer = new TestTextBuffer(documentSnapshot, VsMocks.ContentTypes.LegacyRazorCore);
        var triggerLocation = new SnapshotPoint(documentSnapshot, 4);
        var applicableSpan = new SnapshotSpan(documentSnapshot, new Span(2, text.Length - 3 /* @() */));

        // Act
        var completionContext = await Task.Run(
            () => completionSource.GetCompletionContextAsync(
                session: null!,
                new CompletionTrigger(CompletionTriggerReason.Invoke, triggerLocation.Snapshot),
                triggerLocation,
                applicableSpan,
                DisposalToken));

        // Assert
        Assert.Empty(completionContext.ItemList);
    }

    // This is more of an integration level test validating the end-to-end completion flow.
    [UITheory]
    [InlineData("@")]
    [InlineData("@\r\n")]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/4547")]
    public async Task GetCompletionContextAsync_ProvidesCompletionsWhenAtCompletionPoint(string text)
    {
        // Arrange
        var parser = CreateParser(text, SectionDirective.Directive);
        var completionSource = new RazorDirectiveCompletionSource(parser, _completionFactsService);
        var documentSnapshot = new StringTextSnapshot(text);
        var textBuffer = new TestTextBuffer(documentSnapshot, VsMocks.ContentTypes.LegacyRazorCore);
        var triggerLocation = new SnapshotPoint(documentSnapshot, 1);
        var applicableSpan = new SnapshotSpan(documentSnapshot, new Span(1, 0));

        // Act
        var completionContext = await Task.Run(
            () => completionSource.GetCompletionContextAsync(
                null!,
                new CompletionTrigger(CompletionTriggerReason.Invoke, triggerLocation.Snapshot),
                triggerLocation,
                applicableSpan,
                DisposalToken));

        // Assert
        Assert.Collection(
            completionContext.ItemList,
            item => AssertRazorCompletionItem(SectionDirective.Directive, item, completionSource),
            item => AssertRazorCompletionItem(s_defaultDirectives[0], item, completionSource, isSnippet: false),
            item => AssertRazorCompletionItem(s_defaultDirectives[0], item, completionSource, isSnippet: true),
            item => AssertRazorCompletionItem(s_defaultDirectives[1], item, completionSource, isSnippet: false),
            item => AssertRazorCompletionItem(s_defaultDirectives[1], item, completionSource, isSnippet: true),
            item => AssertRazorCompletionItem(s_defaultDirectives[2], item, completionSource, isSnippet: false),
            item => AssertRazorCompletionItem(s_defaultDirectives[2], item, completionSource, isSnippet: true),
            item => AssertRazorCompletionItem(s_defaultDirectives[3], item, completionSource, isSnippet: false),
            item => AssertRazorCompletionItem(s_defaultDirectives[3], item, completionSource, isSnippet: true));
    }

    [Fact]
    public async Task GetDescriptionAsync_AddsDirectiveDescriptionIfPropertyExists()
    {
        // Arrange
        var completionItem = new CompletionItem("TestDirective", StrictMock.Of<IAsyncCompletionSource>());
        var expectedDescription = new DirectiveCompletionDescription("The expected description");
        completionItem.Properties.AddProperty(RazorDirectiveCompletionSource.DescriptionKey, expectedDescription);
        var completionSource = new RazorDirectiveCompletionSource(StrictMock.Of<IVisualStudioRazorParser>(), _completionFactsService);

        // Act
        var descriptionObject = await completionSource.GetDescriptionAsync(null!, completionItem, DisposalToken);

        // Assert
        var description = Assert.IsType<string>(descriptionObject);
        Assert.Equal(expectedDescription.Description, description);
    }

    [Fact]
    public async Task GetDescriptionAsync_DoesNotAddDescriptionWhenPropertyAbsent()
    {
        // Arrange
        var completionItem = new CompletionItem("TestDirective", StrictMock.Of<IAsyncCompletionSource>());
        var completionSource = new RazorDirectiveCompletionSource(StrictMock.Of<IVisualStudioRazorParser>(), _completionFactsService);

        // Act
        var descriptionObject = await completionSource.GetDescriptionAsync(null!, completionItem, DisposalToken);

        // Assert
        var description = Assert.IsType<string>(descriptionObject);
        Assert.Equal(string.Empty, description);
    }

    private static void AssertRazorCompletionItem(string completionDisplayText, DirectiveDescriptor directive, CompletionItem item, IAsyncCompletionSource source, bool isSnippet = false)
    {
        Assert.Equal(item.DisplayText, completionDisplayText);
        Assert.Equal(item.FilterText, completionDisplayText);

        if (isSnippet)
        {
            Assert.StartsWith(directive.Directive, item.InsertText);
            Assert.Equal(item.InsertText, DirectiveCompletionItemProvider.SingleLineDirectiveSnippets[directive.Directive].InsertText);
        }
        else
        {
            Assert.Equal(item.InsertText, directive.Directive);
        }

        Assert.Same(item.Source, source);
        Assert.True(item.Properties.TryGetProperty<DirectiveCompletionDescription>(RazorDirectiveCompletionSource.DescriptionKey, out var actualDescription));

        var description = isSnippet ? "@" + DirectiveCompletionItemProvider.SingleLineDirectiveSnippets[directive.Directive].DisplayText
                         + Environment.NewLine
                         + WorkspacesSR.DirectiveSnippetDescription
                         : directive.Description;
        Assert.Equal(description, actualDescription.Description);

        AssertRazorCompletionItemDefaults(item);
    }

    private static void AssertRazorCompletionItem(DirectiveDescriptor directive, CompletionItem item, IAsyncCompletionSource source, bool isSnippet = false)
    {
        var expectedDisplayText = isSnippet ? directive.Directive + " directive ..." : directive.Directive;
        AssertRazorCompletionItem(expectedDisplayText, directive, item, source, isSnippet: isSnippet);
    }

    private static void AssertRazorCompletionItemDefaults(CompletionItem item)
    {
        Assert.Equal(item.Icon.ImageId.Guid, RazorDirectiveCompletionSource.DirectiveImageGlyph.ImageId.Guid);
        var filter = Assert.Single(item.Filters);
        Assert.Same(RazorDirectiveCompletionSource.DirectiveCompletionFilters[0], filter);
        Assert.Equal(string.Empty, item.Suffix);
        Assert.Equal(item.DisplayText, item.SortText);
        Assert.Empty(item.AttributeIcons);
    }

    private static IVisualStudioRazorParser CreateParser(string text, params DirectiveDescriptor[] directives)
    {
        var source = RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Default);
        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Default.WithDirectives([.. directives]));

        var syntaxTree = RazorSyntaxTree.Parse(source, codeDocument.ParserOptions);
        codeDocument.SetSyntaxTree(syntaxTree);

        codeDocument.SetTagHelperContext(TagHelperDocumentContext.GetOrCreate(tagHelpers: []));

        var parserMock = new StrictMock<IVisualStudioRazorParser>();
        parserMock
            .Setup(p => p.GetLatestCodeDocumentAsync(It.IsAny<ITextSnapshot>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);

        return parserMock.Object;
    }
}

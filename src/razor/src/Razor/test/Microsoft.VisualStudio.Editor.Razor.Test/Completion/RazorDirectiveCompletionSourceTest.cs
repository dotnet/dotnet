// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Span = Microsoft.VisualStudio.Text.Span;
using WorkspacesSR = Microsoft.CodeAnalysis.Razor.Workspaces.Resources.SR;

namespace Microsoft.VisualStudio.Editor.Razor.Completion;

public class RazorDirectiveCompletionSourceTest : ProjectSnapshotManagerDispatcherTestBase
{
    private static readonly IReadOnlyList<DirectiveDescriptor> s_defaultDirectives = new[]
    {
        CSharpCodeParser.AddTagHelperDirectiveDescriptor,
        CSharpCodeParser.RemoveTagHelperDirectiveDescriptor,
        CSharpCodeParser.TagHelperPrefixDirectiveDescriptor,
    };

    private readonly IRazorCompletionFactsService _completionFactsService;

    public RazorDirectiveCompletionSourceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _completionFactsService = new RazorCompletionFactsService(new[] { new DirectiveCompletionItemProvider() });
    }

    [UIFact]
    public async Task GetCompletionContextAsync_DoesNotProvideCompletionsPriorToParseResults()
    {
        // Arrange
        var text = "@validCompletion";
        var parser = new Mock<VisualStudioRazorParser>(MockBehavior.Strict);
        parser
            .Setup(p => p.GetLatestCodeDocumentAsync(It.IsAny<ITextSnapshot>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null); // CodeDocument will be null faking a parser without a parse.
        var completionSource = new RazorDirectiveCompletionSource(parser.Object, _completionFactsService);
        var documentSnapshot = new StringTextSnapshot(text);
        var triggerLocation = new SnapshotPoint(documentSnapshot, 4);
        var applicableSpan = new SnapshotSpan(documentSnapshot, new Span(1, text.Length - 1 /* validCompletion */));

        // Act
        var completionContext = await Task.Run(
            () => completionSource.GetCompletionContextAsync(
                session: null,
                new CompletionTrigger(CompletionTriggerReason.Invoke, triggerLocation.Snapshot),
                triggerLocation,
                applicableSpan,
                DisposalToken));

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
        var triggerLocation = new SnapshotPoint(documentSnapshot, 4);
        var applicableSpan = new SnapshotSpan(documentSnapshot, new Span(2, text.Length - 3 /* @() */));

        // Act
        var completionContext = await Task.Run(
            () => completionSource.GetCompletionContextAsync(
                session: null,
                new CompletionTrigger(CompletionTriggerReason.Invoke, triggerLocation.Snapshot),
                triggerLocation,
                applicableSpan,
                DisposalToken));

        // Assert
        Assert.Empty(completionContext.ItemList);
    }

    // This is more of an integration level test validating the end-to-end completion flow.
    [UIFact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/4547")]
    public async Task GetCompletionContextAsync_ProvidesCompletionsWhenAtCompletionPoint()
    {
        // Arrange
        var text = "@";
        var parser = CreateParser(text, SectionDirective.Directive);
        var completionSource = new RazorDirectiveCompletionSource(parser, _completionFactsService);
        var documentSnapshot = new StringTextSnapshot(text);
        var triggerLocation = new SnapshotPoint(documentSnapshot, 1);
        var applicableSpan = new SnapshotSpan(documentSnapshot, new Span(1, 0));

        // Act
        var completionContext = await Task.Run(
            () => completionSource.GetCompletionContextAsync(
                null,
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
            item => AssertRazorCompletionItem(s_defaultDirectives[2], item, completionSource, isSnippet: true));
    }

    [Fact]
    public async Task GetDescriptionAsync_AddsDirectiveDescriptionIfPropertyExists()
    {
        // Arrange
        var completionItem = new CompletionItem("TestDirective", Mock.Of<IAsyncCompletionSource>(MockBehavior.Strict));
        var expectedDescription = new DirectiveCompletionDescription("The expected description");
        completionItem.Properties.AddProperty(RazorDirectiveCompletionSource.DescriptionKey, expectedDescription);
        var completionSource = new RazorDirectiveCompletionSource(Mock.Of<VisualStudioRazorParser>(MockBehavior.Strict), _completionFactsService);

        // Act
        var descriptionObject = await completionSource.GetDescriptionAsync(null, completionItem, DisposalToken);

        // Assert
        var description = Assert.IsType<string>(descriptionObject);
        Assert.Equal(expectedDescription.Description, description);
    }

    [Fact]
    public async Task GetDescriptionAsync_DoesNotAddDescriptionWhenPropertyAbsent()
    {
        // Arrange
        var completionItem = new CompletionItem("TestDirective", Mock.Of<IAsyncCompletionSource>(MockBehavior.Strict));
        var completionSource = new RazorDirectiveCompletionSource(Mock.Of<VisualStudioRazorParser>(MockBehavior.Strict), _completionFactsService);

        // Act
        var descriptionObject = await completionSource.GetDescriptionAsync(null, completionItem, DisposalToken);

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
            Assert.Equal(item.InsertText, DirectiveCompletionItemProvider.s_singleLineDirectiveSnippets[directive.Directive].InsertText);
        }
        else
        {
            Assert.Equal(item.InsertText, directive.Directive);
        }

        Assert.Same(item.Source, source);
        Assert.True(item.Properties.TryGetProperty<DirectiveCompletionDescription>(RazorDirectiveCompletionSource.DescriptionKey, out var actualDescription));

        var description = isSnippet ? "@" + DirectiveCompletionItemProvider.s_singleLineDirectiveSnippets[directive.Directive].DisplayText
                         + Environment.NewLine
                         + WorkspacesSR.DirectiveSnippetDescription
                         : directive.Description;
        Assert.Equal(description, actualDescription.Description);

        AssertRazorCompletionItemDefaults(item);
    }

    private static void AssertRazorCompletionItem(DirectiveDescriptor directive, CompletionItem item, IAsyncCompletionSource source, bool isSnippet = false)
    {
        var expectedDisplayText = isSnippet ? directive.Directive + " ..." : directive.Directive;
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

    private static VisualStudioRazorParser CreateParser(string text, params DirectiveDescriptor[] directives)
    {
        var syntaxTree = CreateSyntaxTree(text, directives);
        var codeDocument = RazorCodeDocument.Create(RazorSourceDocument.Create(text, RazorSourceDocumentProperties.Default));
        codeDocument.SetSyntaxTree(syntaxTree);
        codeDocument.SetTagHelperContext(TagHelperDocumentContext.Create(prefix: null, Enumerable.Empty<TagHelperDescriptor>()));
        var parser = new Mock<VisualStudioRazorParser>(MockBehavior.Strict);
        parser
            .Setup(p => p.GetLatestCodeDocumentAsync(It.IsAny<ITextSnapshot>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(codeDocument);

        return parser.Object;
    }

    private static RazorSyntaxTree CreateSyntaxTree(string text, params DirectiveDescriptor[] directives)
    {
        var sourceDocument = TestRazorSourceDocument.Create(text);
        var options = RazorParserOptions.Create(builder =>
        {
            foreach (var directive in directives)
            {
                builder.Directives.Add(directive);
            }
        });
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument, options);
        return syntaxTree;
    }
}

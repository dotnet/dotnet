// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.Editor.Razor;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

public class TagHelperCompletionProviderTest : TagHelperServiceTestBase
{
    public TagHelperCompletionProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    private TagHelperCompletionProvider CreateTagHelperCompletionProvider()
        => new(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());

    [Fact]
    public void GetNearestAncestorTagInfo_MarkupElement()
    {
        // Arrange
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <p><$$strong></strong></p>
                """,
            isRazorFile: false);
        var element = context.Owner.FirstAncestorOrSelf<MarkupElementSyntax>();
        var service = new TagHelperFactsService();

        // Act
        var (ancestorName, ancestorIsTagHelper) = service.GetNearestAncestorTagInfo(element.Ancestors());

        // Assert
        Assert.Equal("p", ancestorName);
        Assert.False(ancestorIsTagHelper);
    }

    [Fact]
    public void GetNearestAncestorTagInfo_TagHelperElement()
    {
        // Arrange
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test1><$$test2></test2></test1>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);
        var element = context.Owner.FirstAncestorOrSelf<MarkupTagHelperElementSyntax>();
        var service = new TagHelperFactsService();

        // Act
        var (ancestorName, ancestorIsTagHelper) = service.GetNearestAncestorTagInfo(element.Ancestors());

        // Assert
        Assert.Equal("test1", ancestorName);
        Assert.True(ancestorIsTagHelper);
    }

    [Fact]
    public void GetCompletionAt_AtEmptyTagName_ReturnsCompletions()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <$$
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(
            completions,
            completion => Assert.Equal("test1", completion.InsertText),
            completion => Assert.Equal("test2", completion.InsertText));
    }

    [Fact]
    public void GetCompletionAt_OutsideOfTagName_DoesNotReturnCompletions()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <br $$/>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetCompletionAt_OutsideOfTagName_InsideCSharp_DoesNotReturnCompletions()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly

                @if (true)
                {
                    <br $$/>
                }
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetCompletionAt_SelfClosingTag_NotAtEndOfName_DoesNotReturnCompletions()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <c $$ />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }


    [Fact]
    public void GetCompletionAt_SelfClosingTag_ReturnsCompletions()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <c$$ />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertTest1Test2Completions(completions);
    }

    [Fact]
    public void GetCompletionAt_SelfClosingTag_InsideCSharp_ReturnsCompletions()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly

                @if (true)
                {
                    <c$$ />
                }
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertTest1Test2Completions(completions);
    }

    [Fact]
    public void GetCompletionAt_MalformedElement()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                </t$$
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertTest1Test2Completions(completions);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6134")]
    public void GetCompletionAt_AtHtmlElementNameEdge_ReturnsCompletions()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <br$$ />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        // Both "test1" and "test2" technically should not be here, but in real-world scenarios they will be filtered by the IDE
        AssertTest1Test2Completions(completions);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6134")]
    public void GetCompletionAt_AtTagHelperElementNameEdge_ReturnsCompletions()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test1$$ />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        // "test2" technically should not be here, but in real-world scenarios it will be filtered by the IDE
        AssertTest1Test2Completions(completions);
    }

    [Fact]
    public void GetCompletionAt_AtAttributeEdge_IntAttribute_ReturnsCompletions()
    {
        // Arrange
        var service = new TagHelperCompletionProvider(RazorTagHelperCompletionService, HtmlFactsService, TagHelperFactsService, TestRazorLSPOptionsMonitor.Create());
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test1 $$/>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(
            completions,
            completion =>
            {
                Assert.Equal("bool-val", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.MinimizedAttributeCommitCharacters, completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
            },
            completion =>
            {
                Assert.Equal("int-val", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.AttributeCommitCharacters, completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
            });
    }

    [Fact]
    public void GetCompletionAt_AtAttributeEdge_IntAttribute_Snippets_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var options = new RazorCompletionOptions(SnippetsSupported: true);
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test1 $$/>
                """,
            isRazorFile: false,
            options,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(
            completions,
            completion =>
            {
                Assert.Equal("bool-val", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.MinimizedAttributeCommitCharacters, completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
                Assert.False(completion.IsSnippet);
            },
            completion =>
            {
                Assert.Equal("int-val=\"$0\"", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.AttributeSnippetCommitCharacters, completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
                Assert.True(completion.IsSnippet);
            });
    }

    [Fact]
    public void GetCompletionAt_KnownHtmlElement_ReturnsCompletions_DefaultPriority()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <title $$ mutator />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(
            completions,
            completion =>
            {
                Assert.Equal("Extra", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.MinimizedAttributeCommitCharacters, completion.CommitCharacters);
                Assert.Equal("Extra", completion.SortText);
            });
    }

    [Fact]
    public void GetCompletionAt_InBody_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2>
                    <$$
                </test2>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(
            completions,
            completion =>
            {
                Assert.Equal("test1", completion.InsertText);
            },
            completion =>
            {
                Assert.Equal("test2", completion.InsertText);
            });
    }

    [Fact]
    public void GetCompletionAt_InBody_ParentRequiring_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test1>
                    <$$
                </test1>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(
            completions,
            completion =>
            {
                Assert.Equal("test1", completion.InsertText);
            },
            completion =>
            {
                Assert.Equal("SomeChild", completion.InsertText);
            },
            completion =>
            {
                Assert.Equal("test2", completion.InsertText);
            });
    }

    [Fact]
    public void GetCompletionAt_AtAttributeEdge_BoolAttribute_ReturnsCompletionsWithout()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 $$/>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(
            completions,
            completion =>
            {
                Assert.Equal("bool-val", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.MinimizedAttributeCommitCharacters, completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
            },
            completion =>
            {
                Assert.Equal("int-val", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.AttributeCommitCharacters, completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
            });
    }

    [Fact]
    public void GetCompletionAt_AtAttributeEdge_IndexerBoolAttribute_ReturnsCompletionsWithDifferentCommitCharacters()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly");
        tagHelper.TagMatchingRule(rule => rule.TagName = "test");
        tagHelper.SetMetadata(TypeName("TestTagHelper"));
        tagHelper.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetMetadata(PropertyName("BoolVal"));
            attribute.TypeName = "System.Collections.Generic.IDictionary<System.String, System.Boolean>";
            attribute.AsDictionary("bool-val-", typeof(bool).FullName);
        });
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test $$/>
                """,
            isRazorFile: false,
            tagHelpers: ImmutableArray.Create(tagHelper.Build()));

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(
            completions,
            completion =>
            {
                Assert.Equal("bool-val", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.AttributeCommitCharacters, completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
            },
            completion =>
            {
                Assert.Equal("bool-val-", completion.InsertText);
                Assert.Empty(completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
            });
    }

    [Fact]
    public void GetCompletionAt_AtAttributeEdge_IndexerAttribute_ReturnsCompletions()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly");
        tagHelper.TagMatchingRule(rule => rule.TagName = "test");
        tagHelper.SetMetadata(TypeName("TestTagHelper"));
        tagHelper.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetMetadata(PropertyName("IntVal"));
            attribute.TypeName = "System.Collections.Generic.IDictionary<System.String, System.Int32>";
            attribute.AsDictionary("int-val-", typeof(int).FullName);
        });
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test $$/>
                """,
            isRazorFile: false,
            tagHelpers: ImmutableArray.Create(tagHelper.Build()));

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(
            completions,
            completion =>
            {
                Assert.Equal("int-val", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.AttributeCommitCharacters, completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
            },
            completion =>
            {
                Assert.Equal("int-val-", completion.InsertText);
                Assert.Empty(completion.CommitCharacters);
                Assert.Equal(CompletionSortTextHelper.HighSortPriority, completion.SortText);
            });
    }

    [Fact]
    public void GetCompletionAt_MinimizedAttributeMiddle_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 bo$$o />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertBoolIntCompletions(completions);
    }

    [Fact]
    public void GetCompletionAt_MinimizedAttributeEdge_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 un$$bound />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertBoolIntCompletions(completions);
    }

    [Fact]
    public void GetCompletionAt_MinimizedTagHelperAttributeEdge_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 bo$$ol-val />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertBoolIntCompletions(completions);
    }

    [Fact]
    public void GetCompletionAt_InHtmlAttributeName_ReturnsNoCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 cl$$ass='' />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertBoolIntCompletions(completions);
    }

    [Fact]
    public void GetCompletionAt_InTagHelperAttribute_ReturnsNoCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 in$$t-val='123' />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertBoolIntCompletions(completions);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6134")]
    public void GetCompletionAt_InPossibePartiallyWrittenTagHelper_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 int$$ />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        // "bool-var" technically should not be here, but in real-world scenarios it will be filtered by the IDE
        AssertBoolIntCompletions(completions);
    }

    [Fact]
    public void GetCompletionsAt_MalformedAttributeValueInName_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 in$$t-val='>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertBoolIntCompletions(completions);
    }

    [Fact]
    public void GetCompletionsAt_MalformedAttributeNamePrefix_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 $$int->
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertBoolIntCompletions(completions);
    }

    [Fact]
    public void GetCompletionAt_HtmlAttributeValue_DoesNotReturnCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 class='$$' />
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Empty(completions);
    }

    [Fact]
    public void GetCompletionsAt_AttributePrefix_ReturnsCompletions()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2   $$     class=''>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertBoolIntCompletions(completions);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/6724")]
    public void GetCompletionsAt_MiddleOfFullAttribute_ReturnsCompletions_NoSnippetBehaviour()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <test2 int-$$val=''>
                """,
            isRazorFile: false,
            options: new(SnippetsSupported: true),
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        Assert.Collection(completions,
            completion =>
            {
                Assert.Equal("bool-val", completion.InsertText); // bool-val will be filtered on IDE side anyway, so just check that it exists and then don't care about its properties
            },
            completion =>
            {
                Assert.Equal("int-val", completion.InsertText);
                Assert.False(completion.IsSnippet);
                Assert.Equal(TagHelperCompletionProvider.AttributeSnippetCommitCharacters, completion.CommitCharacters); // we still want `=` to be a commit character, but we don't want it to be inserted
            }
        );
    }

    [Fact]
    public void GetCompletionsAt_MiddleOfHtmlAttribute_ReturnsCompletion()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <tes$$DF></tesDF>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertTest1Test2Completions(completions);
    }

    [Fact]
    public void GetCompletionsAt_EndOfAttribute_ReturnsCompletion()
    {
        // Arrange
        var service = CreateTagHelperCompletionProvider();
        var context = CreateRazorCompletionContext(
            """
                @addTagHelper *, TestAssembly
                <tesDF$$></tesDF>
                """,
            isRazorFile: false,
            tagHelpers: DefaultTagHelpers);

        // Act
        var completions = service.GetCompletionItems(context);

        // Assert
        AssertTest1Test2Completions(completions);
    }

    private static void AssertBoolIntCompletions(IReadOnlyList<RazorCompletionItem> completions)
    {
        Assert.Collection(completions,
            completion =>
            {
                Assert.Equal("bool-val", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.MinimizedAttributeCommitCharacters, completion.CommitCharacters);
            },
            completion =>
            {
                Assert.Equal("int-val", completion.InsertText);
                Assert.Equal(TagHelperCompletionProvider.AttributeCommitCharacters, completion.CommitCharacters);
            }
        );
    }

    private static void AssertTest1Test2Completions(IReadOnlyList<RazorCompletionItem> completions)
    {
        Assert.Collection(completions,
            completion =>
            {
                Assert.Equal("test1", completion.InsertText);
            },
            completion =>
            {
                Assert.Equal("test2", completion.InsertText);
            }
        );
    }

    private static RazorCompletionContext CreateRazorCompletionContext(string markup, bool isRazorFile, RazorCompletionOptions options = default, ImmutableArray<TagHelperDescriptor> tagHelpers = default)
    {
        tagHelpers = tagHelpers.NullToEmpty();

        TestFileMarkupParser.GetPosition(markup, out var documentContent, out var position);
        var codeDocument = CreateCodeDocument(documentContent, isRazorFile, tagHelpers);
        var syntaxTree = codeDocument.GetSyntaxTree();
        var tagHelperDocumentContext = codeDocument.GetTagHelperContext();

        var queryableChange = new SourceChange(position, length: 0, newText: string.Empty);
        var owner = syntaxTree.Root.LocateOwner(queryableChange);
        return new RazorCompletionContext(position, owner, syntaxTree, tagHelperDocumentContext, Options: options);
    }
}

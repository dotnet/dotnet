// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.VisualStudio.Editor.Razor;

public class TagHelperFactsTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void GetTagHelperBinding_DoesNotAllowOptOutCharacterPrefix()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);

        // Act
        var binding = TagHelperFacts.GetTagHelperBinding(documentContext, "!a", ImmutableArray<KeyValuePair<string, string>>.Empty, parentTag: null, parentIsTagHelper: false);

        // Assert
        Assert.Null(binding);
    }

    [Fact]
    public void GetTagHelperBinding_WorksAsExpected()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule =>
                    rule
                        .RequireTagName("a")
                        .RequireAttributeDescriptor(attribute => attribute.Name("asp-for")))
                .BoundAttributeDescriptor(attribute =>
                    attribute
                        .Name("asp-for")
                        .TypeName(typeof(string).FullName)
                        .Metadata(PropertyName("AspFor")))
                .BoundAttributeDescriptor(attribute =>
                    attribute
                        .Name("asp-route")
                        .TypeName(typeof(IDictionary<string, string>).Namespace + "IDictionary<string, string>")
                        .Metadata(PropertyName("AspRoute"))
                        .AsDictionaryAttribute("asp-route-", typeof(string).FullName))
                .Build(),
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("input"))
                .BoundAttributeDescriptor(attribute =>
                    attribute
                        .Name("asp-for")
                        .TypeName(typeof(string).FullName)
                        .Metadata(PropertyName("AspFor")))
                .Build(),
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);
        var attributes = ImmutableArray.Create(
            new KeyValuePair<string, string>("asp-for", "Name"));

        // Act
        var binding = TagHelperFacts.GetTagHelperBinding(documentContext, "a", attributes, parentTag: "p", parentIsTagHelper: false);

        // Assert
        var descriptor = Assert.Single(binding.Descriptors);
        Assert.Equal(documentDescriptors[0], descriptor);
        var boundRule = Assert.Single(binding.GetBoundRules(descriptor));
        Assert.Equal(documentDescriptors[0].TagMatchingRules.First(), boundRule);
    }

    [Fact]
    public void GetBoundTagHelperAttributes_MatchesPrefixedAttributeName()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("a"))
                .BoundAttributeDescriptor(attribute =>
                    attribute
                        .Name("asp-for")
                        .TypeName(typeof(string).FullName)
                        .Metadata(PropertyName("AspFor")))
                .BoundAttributeDescriptor(attribute =>
                    attribute
                        .Name("asp-route")
                        .TypeName(typeof(IDictionary<string, string>).Namespace + "IDictionary<string, string>")
                        .Metadata(PropertyName("AspRoute"))
                        .AsDictionaryAttribute("asp-route-", typeof(string).FullName))
                .Build()
        ];
        var expectedAttributeDescriptors = new[]
        {
            documentDescriptors[0].BoundAttributes.Last()
        };
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);
        var binding = TagHelperFacts.GetTagHelperBinding(documentContext, "a", ImmutableArray<KeyValuePair<string, string>>.Empty, parentTag: null, parentIsTagHelper: false);

        // Act
        var tagHelperAttributes = TagHelperFacts.GetBoundTagHelperAttributes(documentContext, "asp-route-something", binding);

        // Assert
        Assert.Equal(expectedAttributeDescriptors, tagHelperAttributes);
    }

    [Fact]
    public void GetBoundTagHelperAttributes_MatchesAttributeName()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("input"))
                .BoundAttributeDescriptor(attribute =>
                    attribute
                        .Name("asp-for")
                        .TypeName(typeof(string).FullName)
                        .Metadata(PropertyName("AspFor")))
                .BoundAttributeDescriptor(attribute =>
                    attribute
                        .Name("asp-extra")
                        .TypeName(typeof(string).FullName)
                        .Metadata(PropertyName("AspExtra")))
                .Build()
        ];
        var expectedAttributeDescriptors = new[]
        {
            documentDescriptors[0].BoundAttributes.First()
        };
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);
        var binding = TagHelperFacts.GetTagHelperBinding(documentContext, "input", ImmutableArray<KeyValuePair<string, string>>.Empty, parentTag: null, parentIsTagHelper: false);

        // Act
        var tagHelperAttributes = TagHelperFacts.GetBoundTagHelperAttributes(documentContext, "asp-for", binding);

        // Assert
        Assert.Equal(expectedAttributeDescriptors, tagHelperAttributes);
    }

    [Fact]
    public void GetTagHelpersGivenTag_DoesNotAllowOptOutCharacterPrefix()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);

        // Act
        var descriptors = TagHelperFacts.GetTagHelpersGivenTag(documentContext, "!strong", parentTag: null);

        // Assert
        Assert.Empty(descriptors);
    }

    [Fact]
    public void GetTagHelpersGivenTag_RequiresTagName()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("strong"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);

        // Act
        var descriptors = TagHelperFacts.GetTagHelpersGivenTag(documentContext, "strong", "p");

        // Assert
        Assert.Equal<TagHelperDescriptor>(documentDescriptors, descriptors);
    }

    [Fact]
    public void GetTagHelpersGivenTag_RestrictsTagHelpersBasedOnTagName()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> expectedDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(
                    rule => rule
                        .RequireTagName("a")
                        .RequireParentTag("div"))
                .Build()
        ];
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            expectedDescriptors[0],
            TagHelperDescriptorBuilder.Create("TestType2", "TestAssembly")
                .TagMatchingRuleDescriptor(
                    rule => rule
                        .RequireTagName("strong")
                        .RequireParentTag("div"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);

        // Act
        var descriptors = TagHelperFacts.GetTagHelpersGivenTag(documentContext, "a", "div");

        // Assert
        Assert.Equal<TagHelperDescriptor>(expectedDescriptors, descriptors);
    }

    [Fact]
    public void GetTagHelpersGivenTag_RestrictsTagHelpersBasedOnTagHelperPrefix()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> expectedDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("strong"))
                .Build()
        ];
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            expectedDescriptors[0],
            TagHelperDescriptorBuilder.Create("TestType2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("thstrong"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create("th", documentDescriptors);

        // Act
        var descriptors = TagHelperFacts.GetTagHelpersGivenTag(documentContext, "thstrong", "div");

        // Assert
        Assert.Equal<TagHelperDescriptor>(expectedDescriptors, descriptors);
    }

    [Fact]
    public void GetTagHelpersGivenTag_RestrictsTagHelpersBasedOnParent()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> expectedDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(
                    rule => rule
                        .RequireTagName("strong")
                        .RequireParentTag("div"))
                .Build()
        ];
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            expectedDescriptors[0],
            TagHelperDescriptorBuilder.Create("TestType2", "TestAssembly")
                .TagMatchingRuleDescriptor(
                    rule => rule
                        .RequireTagName("strong")
                        .RequireParentTag("p"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);

        // Act
        var descriptors = TagHelperFacts.GetTagHelpersGivenTag(documentContext, "strong", "div");

        // Assert
        Assert.Equal<TagHelperDescriptor>(expectedDescriptors, descriptors);
    }

    [Fact]
    public void GetTagHelpersGivenParent_AllowsRootParentTag()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);

        // Act
        var descriptors = TagHelperFacts.GetTagHelpersGivenParent(documentContext, parentTag: null /* root */);

        // Assert
        Assert.Equal<TagHelperDescriptor>(documentDescriptors, descriptors);
    }

    [Fact]
    public void GetTagHelpersGivenParent_AllowsRootParentTagForParentRestrictedTagHelperDescriptors()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .Build(),
            TagHelperDescriptorBuilder.Create("PTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("p")
                    .RequireParentTag("body"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);

        // Act
        var descriptors = TagHelperFacts.GetTagHelpersGivenParent(documentContext, parentTag: null /* root */);

        // Assert
        var descriptor = Assert.Single(descriptors);
        Assert.Equal(documentDescriptors[0], descriptor);
    }

    [Fact]
    public void GetTagHelpersGivenParent_AllowsUnspecifiedParentTagHelpers()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);

        // Act
        var descriptors = TagHelperFacts.GetTagHelpersGivenParent(documentContext, "p");

        // Assert
        Assert.Equal<TagHelperDescriptor>(documentDescriptors, descriptors);
    }

    [Fact]
    public void GetTagHelpersGivenParent_RestrictsTagHelpersBasedOnParent()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> expectedDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestType", "TestAssembly")
                .TagMatchingRuleDescriptor(
                    rule => rule
                        .RequireTagName("p")
                        .RequireParentTag("div"))
                .Build()
        ];
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            expectedDescriptors[0],
            TagHelperDescriptorBuilder.Create("TestType2", "TestAssembly")
                .TagMatchingRuleDescriptor(
                    rule => rule
                        .RequireTagName("strong")
                        .RequireParentTag("p"))
                .Build()
        ];
        var documentContext = TagHelperDocumentContext.Create(string.Empty, documentDescriptors);

        // Act
        var descriptors = TagHelperFacts.GetTagHelpersGivenParent(documentContext, "div");

        // Assert
        Assert.Equal<TagHelperDescriptor>(expectedDescriptors, descriptors);
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor.Completion;

public class LanguageServerTagHelperCompletionServiceTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    [WorkItem("https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1452432")]
    public void GetAttributeCompletions_OnlyIndexerNamePrefix()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("FormTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form"))
                .BoundAttributeDescriptor(attribute => attribute
                    .TypeName("System.Collections.Generic.IDictionary<System.String, System.String>")
                    .Metadata(PropertyName("RouteValues"))
                    .AsDictionary("asp-route-", typeof(string).FullName))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["asp-route-..."] = [documentDescriptors[0].BoundAttributes.Last()]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            attributes: [],
            currentTagName: "form");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_BoundDictionaryAttribute_ReturnsPrefixIndexerAndFullSetter()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("FormTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("asp-all-route-data")
                    .TypeName("System.Collections.Generic.IDictionary<System.String, System.String>")
                    .Metadata(PropertyName("RouteValues"))
                    .AsDictionary("asp-route-", typeof(string).FullName))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["asp-all-route-data"] = [documentDescriptors[0].BoundAttributes.Last()],
            ["asp-route-..."] = [documentDescriptors[0].BoundAttributes.Last()]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            attributes: [],
            currentTagName: "form");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_RequiredBoundDictionaryAttribute_ReturnsPrefixIndexerAndFullSetter()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("FormTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form")
                    .RequireAttributeDescriptor(builder =>
                    {
                        builder.Name = "asp-route-";
                        builder.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
                    }))
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form")
                    .RequireAttributeDescriptor(builder => builder.Name = "asp-all-route-data"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("asp-all-route-data")
                    .TypeName("System.Collections.Generic.IDictionary<System.String, System.String>")
                    .Metadata(PropertyName("RouteValues"))
                    .AsDictionary("asp-route-", typeof(string).FullName))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["asp-all-route-data"] = [documentDescriptors[0].BoundAttributes.Last()],
            ["asp-route-..."] = [documentDescriptors[0].BoundAttributes.Last()]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            attributes: [],
            currentTagName: "form");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_DoesNotReturnCompletionsForAlreadySuppliedAttributes()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("div")
                    .RequireAttributeDescriptor(attribute => attribute.Name("repeat")))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("visible")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Visible")))
                .Build(),
            TagHelperDescriptorBuilder.Create("StyleTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("class")
                    .TypeName(typeof(string).FullName)
                    .Metadata(PropertyName("Class")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["onclick"] = [],
            ["visible"] = [documentDescriptors[0].BoundAttributes.Last()]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["onclick"],
            attributes: [
                KeyValuePair.Create("class", "something"),
                KeyValuePair.Create("repeat", "4")],
            currentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_ReturnsCompletionForAlreadySuppliedAttribute_IfCurrentAttributeMatches()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("div")
                    .RequireAttributeDescriptor(attribute => attribute.Name("repeat")))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("visible")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Visible")))
                .Build(),
            TagHelperDescriptorBuilder.Create("StyleTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("class")
                    .TypeName(typeof(string).FullName)
                    .Metadata(PropertyName("Class")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["onclick"] = [],
            ["visible"] = [documentDescriptors[0].BoundAttributes.Last()]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["onclick"],
            attributes: [
                KeyValuePair.Create("class", "something"),
                KeyValuePair.Create("repeat", "4"),
                KeyValuePair.Create("visible", "false")],
            currentTagName: "div",
            currentAttributeName: "visible");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_DoesNotReturnAlreadySuppliedAttribute_IfCurrentAttributeDoesNotMatch()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("div")
                    .RequireAttributeDescriptor(attribute => attribute.Name("repeat")))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("visible")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Visible")))
                .Build(),
            TagHelperDescriptorBuilder.Create("StyleTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("class")
                    .TypeName(typeof(string).FullName)
                    .Metadata(PropertyName("Class")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["onclick"] = []
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["onclick"],
            attributes: [
                KeyValuePair.Create("class", "something"),
                KeyValuePair.Create("repeat", "4"),
                KeyValuePair.Create("visible", "false")],
            currentTagName: "div",
            currentAttributeName: "repeat");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_PossibleDescriptorsReturnUnboundRequiredAttributesWithExistingCompletions()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("div")
                    .RequireAttributeDescriptor(attribute => attribute.Name("repeat")))
                .Build(),
            TagHelperDescriptorBuilder.Create("StyleTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("*")
                    .RequireAttributeDescriptor(attribute => attribute.Name("class")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["class"] = [],
            ["onclick"] = [],
            ["repeat"] = []
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["onclick", "class"],
            currentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_PossibleDescriptorsReturnBoundRequiredAttributesWithExistingCompletions()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("div")
                    .RequireAttributeDescriptor(attribute => attribute.Name("repeat")))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("repeat")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Repeat")))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("visible")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Visible")))
                .Build(),
            TagHelperDescriptorBuilder.Create("StyleTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("*")
                    .RequireAttributeDescriptor(attribute => attribute.Name("class")))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("class")
                    .TypeName(typeof(string).FullName)
                    .Metadata(PropertyName("Class")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["class"] = [.. documentDescriptors[1].BoundAttributes],
            ["onclick"] = [],
            ["repeat"] = [documentDescriptors[0].BoundAttributes.First()]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["onclick"],
            currentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_AppliedDescriptorsReturnAllBoundAttributesWithExistingCompletionsForSchemaTags()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("repeat")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Repeat")))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("visible")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Visible")))
                .Build(),
            TagHelperDescriptorBuilder.Create("StyleTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("*")
                    .RequireAttributeDescriptor(attribute => attribute.Name("class")))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("class")
                    .TypeName(typeof(string).FullName)
                    .Metadata(PropertyName("Class")))
                .Build(),
            TagHelperDescriptorBuilder.Create("StyleTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("visible")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Visible")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["onclick"] = [],
            ["class"] = [.. documentDescriptors[1].BoundAttributes],
            ["repeat"] = [documentDescriptors[0].BoundAttributes.First()],
            ["visible"] = [documentDescriptors[0].BoundAttributes.Last(), documentDescriptors[2].BoundAttributes.First()]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["class", "onclick"],
            currentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_AppliedTagOutputHintDescriptorsReturnBoundAttributesWithExistingCompletionsForNonSchemaTags()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("CustomTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("custom"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("repeat")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Repeat")))
                .TagOutputHint("div")
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["class"] = [],
            ["repeat"] = [.. documentDescriptors[0].BoundAttributes]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["class"],
            currentTagName: "custom");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_AppliedDescriptorsReturnBoundAttributesCompletionsForNonSchemaTags()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("CustomTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("custom"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("repeat")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Repeat")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["repeat"] = [.. documentDescriptors[0].BoundAttributes]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["class"],
            currentTagName: "custom");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_AppliedDescriptorsReturnBoundAttributesWithExistingCompletionsForSchemaTags()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("repeat")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Repeat")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["class"] = [],
            ["repeat"] = [.. documentDescriptors[0].BoundAttributes]
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["class"],
            currentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_NoDescriptorsReturnsExistingCompletions()
    {
        // Arrange
        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["class"] = [],
        });

        var completionContext = BuildAttributeCompletionContext(
            descriptors: [],
            existingCompletions: ["class"],
            currentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_NoDescriptorsForUnprefixedTagReturnsExistingCompletions()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("div")
                    .RequireAttributeDescriptor(attribute => attribute.Name("special")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["class"] = [],
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["class"],
            currentTagName: "div",
            tagHelperPrefix: "th:");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetAttributeCompletions_NoDescriptorsForTagReturnsExistingCompletions()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("MyTableTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("table")
                    .RequireAttributeDescriptor(attribute => attribute.Name("special")))
                .Build(),
        ];

        var expectedCompletions = AttributeCompletionResult.Create(new()
        {
            ["class"] = [],
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions: ["class"],
            currentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetAttributeCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_IgnoresDirectiveAttributes()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("BindAttribute", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("input"))
                .BoundAttributeDescriptor(builder =>
                {
                    builder.Name = "@bind";
                    builder.Metadata(IsDirectiveAttribute);
                })
                .TagOutputHint("table")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create([]);

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["table"],
            containingTagName: "body",
            containingParentTagName: null!);
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_FiltersFullyQualifiedElementsIfShortNameExists()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("Test"))
                .Build(),
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("TestAssembly.Test"))
                .Metadata(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch)
                .Build(),
            TagHelperDescriptorBuilder.Create("Test2TagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("Test2Assembly.Test"))
                .Metadata(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch)
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["Test"] = [documentDescriptors[0]],
            ["Test2Assembly.Test"] = [documentDescriptors[2]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            containingTagName: "body",
            containingParentTagName: null!);
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_TagOutputHintDoesNotFallThroughToSchemaCheck()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("MyTableTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("my-table"))
                .TagOutputHint("table")
                .Build(),
            TagHelperDescriptorBuilder.Create("MyTrTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("my-tr"))
                .TagOutputHint("tr")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["my-table"] = [documentDescriptors[0]]
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["table", "div"],
            containingTagName: "body",
            containingParentTagName: null!);
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_CatchAllsOnlyApplyToCompletionsStartingWithPrefix()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("CatchAllTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["th:li"] = [documentDescriptors[1], documentDescriptors[0]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["li"],
            containingTagName: "ul",
            tagHelperPrefix: "th:");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_TagHelperPrefixIsPrependedToTagHelperCompletions()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("SuperLiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("superli"))
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["th:superli"] = [documentDescriptors[0]],
            ["th:li"] = [documentDescriptors[1]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["li"],
            containingTagName: "ul",
            tagHelperPrefix: "th:");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_IsCaseSensitive()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("MyliTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("myli"))
                .SetCaseSensitive()
                .Build(),
            TagHelperDescriptorBuilder.Create("MYLITagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("MYLI"))
                .SetCaseSensitive()
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["myli"] = [documentDescriptors[0]],
            ["MYLI"] = [documentDescriptors[1]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["li"],
            containingTagName: "ul",
            tagHelperPrefix: null!);
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_HTMLSchemaTagName_IsCaseSensitive()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("LITagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("LI"))
                .SetCaseSensitive()
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .SetCaseSensitive()
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["LI"] = [documentDescriptors[0]],
            ["li"] = [documentDescriptors[1]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["li"],
            containingTagName: "ul",
            tagHelperPrefix: null!);
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_CatchAllsApplyToOnlyTagHelperCompletions()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("SuperLiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("superli"))
                .Build(),
            TagHelperDescriptorBuilder.Create("CatchAll", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["superli"] = [documentDescriptors[0], documentDescriptors[1]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["li"],
            containingTagName: "ul");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_CatchAllsApplyToNonTagHelperCompletionsIfStartsWithTagHelperPrefix()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("SuperLiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("superli"))
                .Build(),
            TagHelperDescriptorBuilder.Create("CatchAll", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["th:superli"] = [documentDescriptors[0], documentDescriptors[1]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["th:li"],
            containingTagName: "ul",
            tagHelperPrefix: "th:");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_AllowsMultiTargetingTagHelpers()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("BoldTagHelper1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("strong"))
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("b"))
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("bold"))
                .Build(),
            TagHelperDescriptorBuilder.Create("BoldTagHelper2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("strong"))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["strong"] = [documentDescriptors[0], documentDescriptors[1]],
            ["b"] = [documentDescriptors[0]],
            ["bold"] = [documentDescriptors[0]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["strong", "b", "bold"],
            containingTagName: "ul");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_CombinesDescriptorsOnExistingCompletions()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("LiTagHelper1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["li"] = [documentDescriptors[0], documentDescriptors[1]],
        });

        var completionContext = BuildElementCompletionContext(documentDescriptors, existingCompletions: ["li"], containingTagName: "ul");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_NewCompletionsForSchemaTagsNotInExistingCompletionsAreIgnored()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("SuperLiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("superli"))
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .TagOutputHint("strong")
                .Build(),
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["li"] = [documentDescriptors[1]],
            ["superli"] = [documentDescriptors[0]],
        });

        var completionContext = BuildElementCompletionContext(documentDescriptors, existingCompletions: ["li"], containingTagName: "ul");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_OutputHintIsCrossReferencedWithExistingCompletions()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .TagOutputHint("li")
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .TagOutputHint("strong")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["div"] = [documentDescriptors[0]],
            ["li"] = [documentDescriptors[1]],
        });

        var completionContext = BuildElementCompletionContext(documentDescriptors, existingCompletions: ["li"], containingTagName: "ul");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_EnsuresDescriptorsHaveSatisfiedParent()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("LiTagHelper1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li").RequireParentTag("ol"))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["li"] = [documentDescriptors[0]],
        });

        var completionContext = BuildElementCompletionContext(documentDescriptors, existingCompletions: ["li"], containingTagName: "ul");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_NoContainingParentTag_DoesNotGetCompletionForRuleWithParentTag()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("Tag1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("outer-child-tag"))
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("child-tag").RequireParentTag("parent-tag"))
                .Build(),
            TagHelperDescriptorBuilder.Create("Tag2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("parent-tag"))
                .AllowChildTag("child-tag")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["outer-child-tag"] = [documentDescriptors[0]],
            ["parent-tag"] = [documentDescriptors[1]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            containingTagName: null!,
            containingParentTagName: null!);
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_WithContainingParentTag_GetsCompletionForRuleWithParentTag()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("Tag1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("outer-child-tag"))
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("child-tag").RequireParentTag("parent-tag"))
                .Build(),
            TagHelperDescriptorBuilder.Create("Tag2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("parent-tag"))
                .AllowChildTag("child-tag")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["child-tag"] = [documentDescriptors[0]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            containingTagName: "child-tag",
            containingParentTagName: "parent-tag");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_AllowedChildrenAreIgnoredWhenAtRoot()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("CatchAll", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .AllowChildTag("b")
                .AllowChildTag("bold")
                .AllowChildTag("div")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create([]);

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            containingTagName: null!,
            containingParentTagName: null!);
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_DoesNotReturnExistingCompletionsWhenAllowedChildren()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("BoldParent", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .AllowChildTag("b")
                .AllowChildTag("bold")
                .AllowChildTag("div")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["b"] = [],
            ["bold"] = [],
            ["div"] = [documentDescriptors[0]]
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["p", "em"],
            containingTagName: "thing",
            containingParentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_CapturesAllAllowedChildTagsFromParentTagHelpers_NoneTagHelpers()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("BoldParent", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .AllowChildTag("b")
                .AllowChildTag("bold")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["b"] = [],
            ["bold"] = [],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            containingTagName: "",
            containingParentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_CapturesAllAllowedChildTagsFromParentTagHelpers_SomeTagHelpers()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("BoldParent", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .AllowChildTag("b")
                .AllowChildTag("bold")
                .AllowChildTag("div")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["b"] = [],
            ["bold"] = [],
            ["div"] = [documentDescriptors[0]]
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            containingTagName: "",
            containingParentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_CapturesAllAllowedChildTagsFromParentTagHelpers_AllTagHelpers()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("BoldParentCatchAll", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .AllowChildTag("strong")
                .AllowChildTag("div")
                .AllowChildTag("b")
                .Build(),
            TagHelperDescriptorBuilder.Create("BoldParent", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .AllowChildTag("b")
                .AllowChildTag("bold")
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["strong"] = [documentDescriptors[0]],
            ["b"] = [documentDescriptors[0]],
            ["bold"] = [documentDescriptors[0]],
            ["div"] = [documentDescriptors[0], documentDescriptors[1]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            containingTagName: "",
            containingParentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_MustSatisfyAttributeRules_WithAttributes()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("FormTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form")
                .RequireAttributeDescriptor(builder =>
                    {
                        builder.Name = "asp-route-";
                        builder.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
                    }))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["form"] = [documentDescriptors[0]]
        });

        var attributes = ImmutableArray.Create(
            KeyValuePair.Create("asp-route-id", "123"));

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["form"],
            containingTagName: "",
            containingParentTagName: "div",
            attributes: attributes);
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_MustSatisfyAttributeRules_NoAttributes()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("FormTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form")
                .RequireAttributeDescriptor(builder =>
                    {
                        builder.Name = "asp-route-";
                        builder.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
                    }))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create([]);

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: ["form"],
            containingTagName: "",
            containingParentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    [Fact]
    public void GetElementCompletions_MustSatisfyAttributeRules_NoAttributes_AllowedIfNotHtml()
    {
        // Arrange
        ImmutableArray<TagHelperDescriptor> documentDescriptors =
        [
            TagHelperDescriptorBuilder.Create("ComponentTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("component")
                .RequireAttributeDescriptor(builder =>
                    {
                        builder.Name = "type";
                        builder.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
                    }))
                .Build(),
        ];

        var expectedCompletions = ElementCompletionResult.Create(new()
        {
            ["component"] = [documentDescriptors[0]],
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: [],
            containingTagName: "",
            containingParentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    private static TagHelperCompletionService CreateTagHelperCompletionFactsService() => new();

    private static void AssertCompletionsAreEquivalent(ElementCompletionResult expected, ElementCompletionResult actual)
    {
        Assert.Equal(expected.Completions.Count, actual.Completions.Count);

        foreach (var (key, value) in expected.Completions)
        {
            var actualValue = actual.Completions[key];
            Assert.NotNull(actualValue);
            Assert.Equal(value, actualValue);
        }
    }

    private static void AssertCompletionsAreEquivalent(AttributeCompletionResult expected, AttributeCompletionResult actual)
    {
        Assert.Equal(expected.Completions.Count, actual.Completions.Count);

        foreach (var expectedCompletion in expected.Completions)
        {
            var actualValue = actual.Completions[expectedCompletion.Key];
            Assert.NotNull(actualValue);
            Assert.Equal(expectedCompletion.Value, actualValue);
        }
    }

    private static ElementCompletionContext BuildElementCompletionContext(
        ImmutableArray<TagHelperDescriptor> descriptors,
        ImmutableArray<string> existingCompletions,
        string containingTagName,
        string containingParentTagName = "body",
        bool containingParentIsTagHelper = false,
        string tagHelperPrefix = "",
        ImmutableArray<KeyValuePair<string, string>> attributes = default)
    {
        attributes = attributes.NullToEmpty();

        var documentContext = TagHelperDocumentContext.Create(tagHelperPrefix, descriptors);
        var completionContext = new ElementCompletionContext(
            documentContext,
            existingCompletions,
            containingTagName,
            attributes,
            containingParentTagName: containingParentTagName,
            containingParentIsTagHelper: containingParentIsTagHelper,
            inHTMLSchema: (tag) => tag == "strong" || tag == "b" || tag == "bold" || tag == "li" || tag == "div" || tag == "form");

        return completionContext;
    }

    private static AttributeCompletionContext BuildAttributeCompletionContext(
        ImmutableArray<TagHelperDescriptor> descriptors,
        ImmutableArray<string> existingCompletions,
        string currentTagName,
        string? currentAttributeName = null!,
        ImmutableArray<KeyValuePair<string, string>> attributes = default,
        string tagHelperPrefix = "")
    {
        attributes = attributes.NullToEmpty();

        var documentContext = TagHelperDocumentContext.Create(tagHelperPrefix, descriptors);
        var completionContext = new AttributeCompletionContext(
            documentContext,
            existingCompletions,
            currentTagName,
            currentAttributeName,
            attributes,
            currentParentTagName: "body",
            currentParentIsTagHelper: false,
            inHTMLSchema: (tag) => tag == "strong" || tag == "b" || tag == "bold" || tag == "li" || tag == "div");

        return completionContext;
    }
}

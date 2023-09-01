// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.VisualStudio.Editor.Razor;

public class LanguageServerTagHelperCompletionServiceTest(ITestOutputHelper testOutput) : TestBase(testOutput)
{
    [Fact]
    [WorkItem("https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1452432")]
    public void GetAttributeCompletions_OnlyIndexerNamePrefix()
    {
        // Arrange
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("FormTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form"))
                .BoundAttributeDescriptor(attribute => attribute
                    .TypeName("System.Collections.Generic.IDictionary<System.String, System.String>")
                    .Metadata(PropertyName("RouteValues"))
                    .AsDictionary("asp-route-", typeof(string).FullName))
                .Build(),
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["asp-route-..."] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.Last(),
            }
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            Array.Empty<string>(),
            attributes: ImmutableArray<KeyValuePair<string, string>>.Empty,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("FormTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("asp-all-route-data")
                    .TypeName("System.Collections.Generic.IDictionary<System.String, System.String>")
                    .Metadata(PropertyName("RouteValues"))
                    .AsDictionary("asp-route-", typeof(string).FullName))
                .Build(),
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["asp-all-route-data"] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.Last(),
            },
            ["asp-route-..."] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.Last(),
            }
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            Array.Empty<string>(),
            attributes: ImmutableArray<KeyValuePair<string, string>>.Empty,
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["asp-all-route-data"] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.Last(),
            },
            ["asp-route-..."] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.Last(),
            }
        });

        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            Array.Empty<string>(),
            attributes: ImmutableArray<KeyValuePair<string, string>>.Empty,
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["onclick"] = new HashSet<BoundAttributeDescriptor>(),
            ["visible"] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.Last()
            }
        });

        var existingCompletions = new[] { "onclick" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
            attributes: ImmutableArray.Create(
                KeyValuePair.Create("class", "something"),
                KeyValuePair.Create("repeat", "4")),
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["onclick"] = new HashSet<BoundAttributeDescriptor>(),
            ["visible"] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.Last()
            }
        });

        var existingCompletions = new[] { "onclick" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
            attributes: ImmutableArray.Create(
                KeyValuePair.Create("class", "something"),
                KeyValuePair.Create("repeat", "4"),
                KeyValuePair.Create("visible", "false")),
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["onclick"] = new HashSet<BoundAttributeDescriptor>()
        });

        var existingCompletions = new[] { "onclick" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
            attributes: ImmutableArray.Create(
                KeyValuePair.Create("class", "something"),
                KeyValuePair.Create("repeat", "4"),
                KeyValuePair.Create("visible", "false")),
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["class"] = new HashSet<BoundAttributeDescriptor>(),
            ["onclick"] = new HashSet<BoundAttributeDescriptor>(),
            ["repeat"] = new HashSet<BoundAttributeDescriptor>()
        });

        var existingCompletions = new[] { "onclick", "class" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["class"] = new HashSet<BoundAttributeDescriptor>(documentDescriptors[1].BoundAttributes),
            ["onclick"] = new HashSet<BoundAttributeDescriptor>(),
            ["repeat"] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.First()
            }
        });

        var existingCompletions = new[] { "onclick" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["onclick"] = new HashSet<BoundAttributeDescriptor>(),
            ["class"] = new HashSet<BoundAttributeDescriptor>(documentDescriptors[1].BoundAttributes),
            ["repeat"] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.First()
            },
            ["visible"] = new HashSet<BoundAttributeDescriptor>()
            {
                documentDescriptors[0].BoundAttributes.Last(),
                documentDescriptors[2].BoundAttributes.First(),
            }
        });

        var existingCompletions = new[] { "class", "onclick" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("CustomTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("custom"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("repeat")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Repeat")))
                .TagOutputHint("div")
                .Build(),
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["class"] = new HashSet<BoundAttributeDescriptor>(),
            ["repeat"] = new HashSet<BoundAttributeDescriptor>(documentDescriptors[0].BoundAttributes)
        });

        var existingCompletions = new[] { "class" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("CustomTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("custom"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("repeat")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Repeat")))
                .Build(),
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["repeat"] = new HashSet<BoundAttributeDescriptor>(documentDescriptors[0].BoundAttributes)
        });

        var existingCompletions = new[] { "class" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .BoundAttributeDescriptor(attribute => attribute
                    .Name("repeat")
                    .TypeName(typeof(bool).FullName)
                    .Metadata(PropertyName("Repeat")))
                .Build(),
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["class"] = new HashSet<BoundAttributeDescriptor>(),
            ["repeat"] = new HashSet<BoundAttributeDescriptor>(documentDescriptors[0].BoundAttributes)
        });

        var existingCompletions = new[] { "class" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["class"] = new HashSet<BoundAttributeDescriptor>(),
        });

        var existingCompletions = new[] { "class" };
        var completionContext = BuildAttributeCompletionContext(
            Enumerable.Empty<TagHelperDescriptor>(),
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("div")
                    .RequireAttributeDescriptor(attribute => attribute.Name("special")))
                .Build(),
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["class"] = new HashSet<BoundAttributeDescriptor>(),
        });

        var existingCompletions = new[] { "class" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("MyTableTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("table")
                    .RequireAttributeDescriptor(attribute => attribute.Name("special")))
                .Build(),
        };
        var expectedCompletions = AttributeCompletionResult.Create(new Dictionary<string, HashSet<BoundAttributeDescriptor>>()
        {
            ["class"] = new HashSet<BoundAttributeDescriptor>(),
        });

        var existingCompletions = new[] { "class" };
        var completionContext = BuildAttributeCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("BindAttribute", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("input"))
                .BoundAttributeDescriptor(builder =>
                {
                    builder.Name = "@bind";
                    builder.Metadata(IsDirectiveAttribute);
                })
                .TagOutputHint("table")
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>());

        var existingCompletions = new[] { "table" };
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
            containingTagName: "body",
            containingParentTagName: null);
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["Test"] = new HashSet<TagHelperDescriptor>() { documentDescriptors[0] },
            ["Test2Assembly.Test"] = new HashSet<TagHelperDescriptor>() { documentDescriptors[2] },
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            Array.Empty<string>(),
            containingTagName: "body",
            containingParentTagName: null);
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("MyTableTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("my-table"))
                .TagOutputHint("table")
                .Build(),
            TagHelperDescriptorBuilder.Create("MyTrTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("my-tr"))
                .TagOutputHint("tr")
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["my-table"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] }
        });

        var existingCompletions = new[] { "table", "div" };
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
            containingTagName: "body",
            containingParentTagName: null);
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("CatchAllTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["th:li"] = new HashSet<TagHelperDescriptor> { documentDescriptors[1], documentDescriptors[0] },
        });

        var existingCompletions = new[] { "li" };
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("SuperLiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("superli"))
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["th:superli"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
            ["th:li"] = new HashSet<TagHelperDescriptor> { documentDescriptors[1] },
        });

        var existingCompletions = new[] { "li" };
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("MyliTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("myli"))
                .SetCaseSensitive()
                .Build(),
            TagHelperDescriptorBuilder.Create("MYLITagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("MYLI"))
                .SetCaseSensitive()
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["myli"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
            ["MYLI"] = new HashSet<TagHelperDescriptor> { documentDescriptors[1] },
        });

        var existingCompletions = new[] { "li" };
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
            containingTagName: "ul",
            tagHelperPrefix: null);
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("LITagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("LI"))
                .SetCaseSensitive()
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .SetCaseSensitive()
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["LI"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
            ["li"] = new HashSet<TagHelperDescriptor> { documentDescriptors[1] },
        });

        var existingCompletions = new[] { "li" };
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
            containingTagName: "ul",
            tagHelperPrefix: null);
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("SuperLiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("superli"))
                .Build(),
            TagHelperDescriptorBuilder.Create("CatchAll", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["superli"] = new HashSet<TagHelperDescriptor>() { documentDescriptors[0], documentDescriptors[1] },
        });

        var existingCompletions = new[] { "li" };
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("SuperLiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("superli"))
                .Build(),
            TagHelperDescriptorBuilder.Create("CatchAll", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["th:superli"] = new HashSet<TagHelperDescriptor>() { documentDescriptors[0], documentDescriptors[1] },
        });

        var existingCompletions = new[] { "th:li" };
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("BoldTagHelper1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("strong"))
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("b"))
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("bold"))
                .Build(),
            TagHelperDescriptorBuilder.Create("BoldTagHelper2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("strong"))
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["strong"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0], documentDescriptors[1] },
            ["b"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
            ["bold"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
        });

        var existingCompletions = new[] { "strong", "b", "bold" };
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("LiTagHelper1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["li"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0], documentDescriptors[1] },
        });

        var existingCompletions = new[] { "li" };
        var completionContext = BuildElementCompletionContext(documentDescriptors, existingCompletions, containingTagName: "ul");
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["li"] = new HashSet<TagHelperDescriptor> { documentDescriptors[1] },
            ["superli"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
        });

        var existingCompletions = new[] { "li" };
        var completionContext = BuildElementCompletionContext(documentDescriptors, existingCompletions, containingTagName: "ul");
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("DivTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .TagOutputHint("li")
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .TagOutputHint("strong")
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["div"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
            ["li"] = new HashSet<TagHelperDescriptor> { documentDescriptors[1] },
        });

        var existingCompletions = new[] { "li" };
        var completionContext = BuildElementCompletionContext(documentDescriptors, existingCompletions, containingTagName: "ul");
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("LiTagHelper1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li"))
                .Build(),
            TagHelperDescriptorBuilder.Create("LiTagHelper2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("li").RequireParentTag("ol"))
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["li"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
        });

        var existingCompletions = new[] { "li" };
        var completionContext = BuildElementCompletionContext(documentDescriptors, existingCompletions, containingTagName: "ul");
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("Tag1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("outer-child-tag"))
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("child-tag").RequireParentTag("parent-tag"))
                .Build(),
            TagHelperDescriptorBuilder.Create("Tag2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("parent-tag"))
                .AllowChildTag("child-tag")
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["outer-child-tag"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
            ["parent-tag"] = new HashSet<TagHelperDescriptor> { documentDescriptors[1] },
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: Enumerable.Empty<string>(),
            containingTagName: null,
            containingParentTagName: null);
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("Tag1", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("outer-child-tag"))
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("child-tag").RequireParentTag("parent-tag"))
                .Build(),
            TagHelperDescriptorBuilder.Create("Tag2", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("parent-tag"))
                .AllowChildTag("child-tag")
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["child-tag"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
        });

        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions: Enumerable.Empty<string>(),
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("CatchAll", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("*"))
                .AllowChildTag("b")
                .AllowChildTag("bold")
                .AllowChildTag("div")
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>());

        var existingCompletions = Enumerable.Empty<string>();
        var completionContext = BuildElementCompletionContext(
            documentDescriptors,
            existingCompletions,
            containingTagName: null,
            containingParentTagName: null);
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("BoldParent", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .AllowChildTag("b")
                .AllowChildTag("bold")
                .AllowChildTag("div")
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["b"] = new HashSet<TagHelperDescriptor>(),
            ["bold"] = new HashSet<TagHelperDescriptor>(),
            ["div"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] }
        });

        var existingCompletions = new[] { "p", "em" };
        var completionContext = BuildElementCompletionContext(documentDescriptors, existingCompletions, containingTagName: "thing", containingParentTagName: "div");
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("BoldParent", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .AllowChildTag("b")
                .AllowChildTag("bold")
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["b"] = new HashSet<TagHelperDescriptor>(),
            ["bold"] = new HashSet<TagHelperDescriptor>(),
        });

        var completionContext = BuildElementCompletionContext(documentDescriptors, Enumerable.Empty<string>(), containingTagName: "", containingParentTagName: "div");
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("BoldParent", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule.RequireTagName("div"))
                .AllowChildTag("b")
                .AllowChildTag("bold")
                .AllowChildTag("div")
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["b"] = new HashSet<TagHelperDescriptor>(),
            ["bold"] = new HashSet<TagHelperDescriptor>(),
            ["div"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] }
        });

        var completionContext = BuildElementCompletionContext(documentDescriptors, Enumerable.Empty<string>(), containingTagName: "", containingParentTagName: "div");
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
        var documentDescriptors = new[]
        {
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
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["strong"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
            ["b"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
            ["bold"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] },
            ["div"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0], documentDescriptors[1] },
        });

        var completionContext = BuildElementCompletionContext(documentDescriptors, Enumerable.Empty<string>(), containingTagName: "", containingParentTagName: "div");
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("FormTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form")
                .RequireAttributeDescriptor(builder =>
                    {
                        builder.Name = "asp-route-";
                        builder.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
                    }))
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>()
        {
            ["form"] = new HashSet<TagHelperDescriptor> { documentDescriptors[0] }
        });

        var attributes = ImmutableArray.Create(
            KeyValuePair.Create("asp-route-id", "123"));

        var completionContext = BuildElementCompletionContext(documentDescriptors, Enumerable.Empty<string>(), containingTagName: "", containingParentTagName: "div", attributes: attributes);
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
        var documentDescriptors = new[]
        {
            TagHelperDescriptorBuilder.Create("FormTagHelper", "TestAssembly")
                .TagMatchingRuleDescriptor(rule => rule
                    .RequireTagName("form")
                .RequireAttributeDescriptor(builder =>
                    {
                        builder.Name = "asp-route-";
                        builder.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
                    }))
                .Build(),
        };
        var expectedCompletions = ElementCompletionResult.Create(new Dictionary<string, HashSet<TagHelperDescriptor>>());

        var completionContext = BuildElementCompletionContext(documentDescriptors, Enumerable.Empty<string>(), containingTagName: "", containingParentTagName: "div");
        var service = CreateTagHelperCompletionFactsService();

        // Act
        var completions = service.GetElementCompletions(completionContext);

        // Assert
        AssertCompletionsAreEquivalent(expectedCompletions, completions);
    }

    private static LanguageServerTagHelperCompletionService CreateTagHelperCompletionFactsService()
    {
        var tagHelperFactsService = new TagHelperFactsService();
        var completionFactService = new LanguageServerTagHelperCompletionService(tagHelperFactsService);

        return completionFactService;
    }

    private static void AssertCompletionsAreEquivalent(ElementCompletionResult expected, ElementCompletionResult actual)
    {
        Assert.Equal(expected.Completions.Count, actual.Completions.Count);

        foreach (var expectedCompletion in expected.Completions)
        {
            var actualValue = actual.Completions[expectedCompletion.Key];
            Assert.NotNull(actualValue);
            Assert.Equal(expectedCompletion.Value, actualValue, TagHelperDescriptorComparer.Default);
        }
    }

    private static void AssertCompletionsAreEquivalent(AttributeCompletionResult expected, AttributeCompletionResult actual)
    {
        Assert.Equal(expected.Completions.Count, actual.Completions.Count);

        foreach (var expectedCompletion in expected.Completions)
        {
            var actualValue = actual.Completions[expectedCompletion.Key];
            Assert.NotNull(actualValue);
            Assert.Equal(expectedCompletion.Value, actualValue, BoundAttributeDescriptorComparer.Default);
        }
    }

    private static ElementCompletionContext BuildElementCompletionContext(
        IEnumerable<TagHelperDescriptor> descriptors,
        IEnumerable<string> existingCompletions,
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
            inHTMLSchema: (tag) => tag == "strong" || tag == "b" || tag == "bold" || tag == "li" || tag == "div");

        return completionContext;
    }

    private static AttributeCompletionContext BuildAttributeCompletionContext(
        IEnumerable<TagHelperDescriptor> descriptors,
        IEnumerable<string> existingCompletions,
        string currentTagName,
        string currentAttributeName = null,
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

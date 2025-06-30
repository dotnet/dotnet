// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Xunit;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

public class DefaultTagHelperDescriptorFactoryTest : TagHelperDescriptorProviderTestBase
{
    public DefaultTagHelperDescriptorFactoryTest() : base(AdditionalCode)
    {
        Compilation = BaseCompilation;
    }

    private Compilation Compilation { get; }

    public static TheoryData RequiredAttributeParserErrorData
    {
        get
        {
            return new TheoryData<string, Action<RequiredAttributeDescriptorBuilder>[]>
                {
                    {
                        "name,",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_CouldNotFindMatchingEndBrace("name,")),
                        }
                    },
                    {
                        " ",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name(string.Empty)
                                .AddDiagnostic(AspNetCore.Razor.Language.RazorDiagnosticFactory.CreateTagHelper_InvalidTargetedAttributeNameNullOrWhitespace()),
                        }
                    },
                    {
                        "n@me",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("n@me")
                                .AddDiagnostic(AspNetCore.Razor.Language.RazorDiagnosticFactory.CreateTagHelper_InvalidTargetedAttributeName("n@me", '@')),
                        }
                    },
                    {
                        "name extra",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_InvalidRequiredAttributeCharacter('e', "name extra")),
                        }
                    },
                    {
                        "[[ ",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("[")
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_CouldNotFindMatchingEndBrace("[[ ")),
                        }
                    },
                    {
                        "[ ",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("")
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_CouldNotFindMatchingEndBrace("[ ")),
                        }
                    },
                    {
                        "[name='unended]",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .ValueComparison(RequiredAttributeValueComparison.FullMatch)
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_InvalidRequiredAttributeMismatchedQuotes('\'', "[name='unended]")),
                        }
                    },
                    {
                        "[name='unended",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .ValueComparison(RequiredAttributeValueComparison.FullMatch)
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_InvalidRequiredAttributeMismatchedQuotes('\'', "[name='unended")),
                        }
                    },
                    {
                        "[name",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_CouldNotFindMatchingEndBrace("[name")),
                        }
                    },
                    {
                        "[ ]",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name(string.Empty)
                                .AddDiagnostic(AspNetCore.Razor.Language.RazorDiagnosticFactory.CreateTagHelper_InvalidTargetedAttributeNameNullOrWhitespace()),
                        }
                    },
                    {
                        "[n@me]",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("n@me")
                                .AddDiagnostic(AspNetCore.Razor.Language.RazorDiagnosticFactory.CreateTagHelper_InvalidTargetedAttributeName("n@me", '@')),
                        }
                    },
                    {
                        "[name@]",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name@")
                                .AddDiagnostic(AspNetCore.Razor.Language.RazorDiagnosticFactory.CreateTagHelper_InvalidTargetedAttributeName("name@", '@')),
                        }
                    },
                    {
                        "[name^]",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_PartialRequiredAttributeOperator('^', "[name^]")),
                        }
                    },
                    {
                        "[name='value'",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .Value("value", RequiredAttributeValueComparison.FullMatch)
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_CouldNotFindMatchingEndBrace("[name='value'")),
                        }
                    },
                    {
                        "[name ",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_CouldNotFindMatchingEndBrace("[name ")),
                        }
                    },
                    {
                        "[name extra]",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_InvalidRequiredAttributeOperator('e', "[name extra]")),
                        }
                    },
                    {
                        "[name=value ",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .Value("value", RequiredAttributeValueComparison.FullMatch)
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_CouldNotFindMatchingEndBrace("[name=value ")),
                        }
                    },
                };
        }
    }

    [Theory]
    [MemberData(nameof(RequiredAttributeParserErrorData))]
    public void RequiredAttributeParser_ParsesRequiredAttributesAndLogsDiagnosticsCorrectly(
        string requiredAttributes,
        IEnumerable<Action<RequiredAttributeDescriptorBuilder>> configureBuilders)
    {
        // Arrange
        var tagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        var ruleBuilder = new TagMatchingRuleDescriptorBuilder(tagHelperBuilder);

        var expectedRules = new List<RequiredAttributeDescriptor>();
        foreach (var configureBuilder in configureBuilders)
        {
            var builder = new RequiredAttributeDescriptorBuilder(ruleBuilder);
            configureBuilder(builder);

            expectedRules.Add(builder.Build());
        }

        // Act
        RequiredAttributeParser.AddRequiredAttributes(requiredAttributes, ruleBuilder);

        // Assert
        var descriptors = ruleBuilder.Build().Attributes;
        Assert.Equal(expectedRules, descriptors);
    }

    public static TheoryData RequiredAttributeParserData
    {
        get
        {
            Func<string, RequiredAttributeNameComparison, Action<RequiredAttributeDescriptorBuilder>> plain =
                (name, nameComparison) => builder => builder.Name(name, nameComparison);

            Func<string, string, RequiredAttributeValueComparison, Action<RequiredAttributeDescriptorBuilder>> css =
                (name, value, valueComparison) => builder => builder
                    .Name(name)
                    .Value(value, valueComparison);

            return new TheoryData<string, IEnumerable<Action<RequiredAttributeDescriptorBuilder>>>
                {
                    { null, Enumerable.Empty<Action<RequiredAttributeDescriptorBuilder>>() },
                    { string.Empty, Enumerable.Empty<Action<RequiredAttributeDescriptorBuilder>>() },
                    { "name", new[] { plain("name", RequiredAttributeNameComparison.FullMatch) } },
                    { "name-*", new[] { plain("name-", RequiredAttributeNameComparison.PrefixMatch) } },
                    { "  name-*   ", new[] { plain("name-", RequiredAttributeNameComparison.PrefixMatch) } },
                    {
                        "asp-route-*,valid  ,  name-*   ,extra",
                        new[]
                        {
                            plain("asp-route-", RequiredAttributeNameComparison.PrefixMatch),
                            plain("valid", RequiredAttributeNameComparison.FullMatch),
                            plain("name-", RequiredAttributeNameComparison.PrefixMatch),
                            plain("extra", RequiredAttributeNameComparison.FullMatch),
                        }
                    },
                    { "[name]", new[] { css("name", null, RequiredAttributeValueComparison.None) } },
                    { "[ name ]", new[] { css("name", null, RequiredAttributeValueComparison.None) } },
                    { " [ name ] ", new[] { css("name", null, RequiredAttributeValueComparison.None) } },
                    { "[name=]", new[] { css("name", "", RequiredAttributeValueComparison.FullMatch) } },
                    { "[name='']", new[] { css("name", "", RequiredAttributeValueComparison.FullMatch) } },
                    { "[name ^=]", new[] { css("name", "", RequiredAttributeValueComparison.PrefixMatch) } },
                    { "[name=hello]", new[] { css("name", "hello", RequiredAttributeValueComparison.FullMatch) } },
                    { "[name= hello]", new[] { css("name", "hello", RequiredAttributeValueComparison.FullMatch) } },
                    { "[name='hello']", new[] { css("name", "hello", RequiredAttributeValueComparison.FullMatch) } },
                    { "[name=\"hello\"]", new[] { css("name", "hello", RequiredAttributeValueComparison.FullMatch) } },
                    { " [ name  $= \" hello\" ]  ", new[] { css("name", " hello", RequiredAttributeValueComparison.SuffixMatch) } },
                    {
                        "[name=\"hello\"],[other^=something ], [val = 'cool']",
                        new[]
                        {
                            css("name", "hello", RequiredAttributeValueComparison.FullMatch),
                            css("other", "something", RequiredAttributeValueComparison.PrefixMatch),
                            css("val", "cool", RequiredAttributeValueComparison.FullMatch) }
                    },
                    {
                        "asp-route-*,[name=\"hello\"],valid  ,[other^=something ],   name-*   ,[val = 'cool'],extra",
                        new[]
                        {
                            plain("asp-route-", RequiredAttributeNameComparison.PrefixMatch),
                            css("name", "hello", RequiredAttributeValueComparison.FullMatch),
                            plain("valid", RequiredAttributeNameComparison.FullMatch),
                            css("other", "something", RequiredAttributeValueComparison.PrefixMatch),
                            plain("name-", RequiredAttributeNameComparison.PrefixMatch),
                            css("val", "cool", RequiredAttributeValueComparison.FullMatch),
                            plain("extra", RequiredAttributeNameComparison.FullMatch),
                        }
                    },
                };
        }
    }

    [Theory]
    [MemberData(nameof(RequiredAttributeParserData))]
    public void RequiredAttributeParser_ParsesRequiredAttributesCorrectly(
        string requiredAttributes,
        IEnumerable<Action<RequiredAttributeDescriptorBuilder>> configureBuilders)
    {
        // Arrange
        var tagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        var ruleBuilder = new TagMatchingRuleDescriptorBuilder(tagHelperBuilder);

        var expectedRules = new List<RequiredAttributeDescriptor>();
        foreach (var configureBuilder in configureBuilders)
        {
            var builder = new RequiredAttributeDescriptorBuilder(ruleBuilder);
            configureBuilder(builder);

            expectedRules.Add(builder.Build());
        }

        // Act
        RequiredAttributeParser.AddRequiredAttributes(requiredAttributes, ruleBuilder);

        // Assert
        var descriptors = ruleBuilder.Build().Attributes;
        Assert.Equal(expectedRules, descriptors);
    }

    public static TheoryData IsEnumData
    {
        get
        {
            // tagHelperType, expectedDescriptor
            return new TheoryData<string, TagHelperDescriptor>
            {
                {
                    "TestNamespace.EnumTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.EnumTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "EnumTagHelper"))
                        .TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName("enum"))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("non-enum-property")
                                .Metadata(PropertyName("NonEnumProperty"))
                                .TypeName(typeof(int).FullName))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("enum-property")
                                .Metadata(PropertyName("EnumProperty"))
                                .TypeName("TestNamespace.CustomEnum")
                                .AsEnum())
                        .Build()
                },
                {
                    "TestNamespace.MultiEnumTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.MultiEnumTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "MultiEnumTagHelper"))
                        .TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName("p"))
                        .TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName("input"))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("non-enum-property")
                                .Metadata(PropertyName("NonEnumProperty"))
                                .TypeName(typeof(int).FullName))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("enum-property")
                                .Metadata(PropertyName("EnumProperty"))
                                .TypeName("TestNamespace.CustomEnum")
                                .AsEnum())
                        .Build()
                },
                {
                    "TestNamespace.NestedEnumTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.NestedEnumTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "NestedEnumTagHelper"))
                        .TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName("nested-enum"))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("nested-enum-property")
                                .Metadata(PropertyName("NestedEnumProperty"))
                                .TypeName("TestNamespace.NestedEnumTagHelper.NestedEnum")
                                .AsEnum())
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("non-enum-property")
                                .Metadata(PropertyName("NonEnumProperty"))
                                .TypeName(typeof(int).FullName))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("enum-property")
                                .Metadata(PropertyName("EnumProperty"))
                                .TypeName("TestNamespace.CustomEnum")
                                .AsEnum())
                        .Build()
                },
            };
        }
    }

    [Theory]
    [MemberData(nameof(IsEnumData))]
    public void CreateDescriptor_IsEnumIsSetCorrectly(
        string tagHelperTypeFullName,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperTypeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    public static TheoryData RequiredParentData
    {
        get
        {
            // tagHelperType, expectedDescriptor
            return new TheoryData<string, TagHelperDescriptor>
            {
                {
                    "TestNamespace.RequiredParentTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.RequiredParentTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "RequiredParentTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("input").RequireParentTag("div"))
                        .Build()
                },
                {
                    "TestNamespace.MultiSpecifiedRequiredParentTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.MultiSpecifiedRequiredParentTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "MultiSpecifiedRequiredParentTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("p").RequireParentTag("div"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("input").RequireParentTag("section"))
                        .Build()
                },
                {
                    "TestNamespace.MultiWithUnspecifiedRequiredParentTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.MultiWithUnspecifiedRequiredParentTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "MultiWithUnspecifiedRequiredParentTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("p"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("input").RequireParentTag("div"))
                        .Build()
                },
            };
        }
    }

    [Theory]
    [MemberData(nameof(RequiredParentData))]
    public void CreateDescriptor_CreatesDesignTimeDescriptorsWithRequiredParent(
        string tagHelperTypeFullName,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperTypeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    private static KeyValuePair<string, string>[] GetMetadata(string @namespace, string name)
    {
        var fullName = $"{@namespace}.{name}";

        return new[]
        {
            TypeName(fullName),
            TypeNamespace(@namespace),
            TypeNameIdentifier(name)
        };
    }

    public static TheoryData RestrictChildrenData
    {
        get
        {
            // tagHelperType, expectedDescriptor
            return new TheoryData<string, TagHelperDescriptor>
            {
                {
                    "TestNamespace.RestrictChildrenTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.RestrictChildrenTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "RestrictChildrenTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("restrict-children"))
                        .AllowChildTag("p")
                        .Build()
                },
                {
                    "TestNamespace.DoubleRestrictChildrenTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.DoubleRestrictChildrenTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "DoubleRestrictChildrenTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("double-restrict-children"))
                        .AllowChildTag("p")
                        .AllowChildTag("strong")
                        .Build()
                },
                {
                    "TestNamespace.MultiTargetRestrictChildrenTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.MultiTargetRestrictChildrenTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "MultiTargetRestrictChildrenTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("p"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("div"))
                        .AllowChildTag("p")
                        .AllowChildTag("strong")
                        .Build()
                },
            };
        }
    }


    [Theory]
    [MemberData(nameof(RestrictChildrenData))]
    public void CreateDescriptor_CreatesDescriptorsWithAllowedChildren(
        string tagHelperTypeFullName,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperTypeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    public static TheoryData TagStructureData
    {
        get
        {
            // tagHelperType, expectedDescriptor
            return new TheoryData<string, TagHelperDescriptor>
            {
                {
                    "TestNamespace.TagStructureTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.TagStructureTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "TagStructureTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder
                            .RequireTagName("input")
                            .RequireTagStructure(TagStructure.WithoutEndTag))
                        .Build()
                },
                {
                    "TestNamespace.MultiSpecifiedTagStructureTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.MultiSpecifiedTagStructureTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "MultiSpecifiedTagStructureTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder
                            .RequireTagName("p")
                            .RequireTagStructure(TagStructure.NormalOrSelfClosing))
                        .TagMatchingRuleDescriptor(builder => builder
                            .RequireTagName("input")
                            .RequireTagStructure(TagStructure.WithoutEndTag))
                        .Build()
                },
                {
                    "TestNamespace.MultiWithUnspecifiedTagStructureTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace.MultiWithUnspecifiedTagStructureTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "MultiWithUnspecifiedTagStructureTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder
                            .RequireTagName("p"))
                        .TagMatchingRuleDescriptor(builder => builder
                            .RequireTagName("input")
                            .RequireTagStructure(TagStructure.WithoutEndTag))
                        .Build()
                },
            };
        }
    }

    [Theory]
    [MemberData(nameof(TagStructureData))]
    public void CreateDescriptor_CreatesDesignTimeDescriptorsWithTagStructure(
        string tagHelperTypeFullName,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperTypeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    public static TheoryData EditorBrowsableData
    {
        get
        {
            // tagHelperType, designTime, expectedDescriptor
            return new TheoryData<string, bool, TagHelperDescriptor>
                {
                    {
                        "TestNamespace.InheritedEditorBrowsableTagHelper",
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "inherited-editor-browsable",
                            typeName: "TestNamespace.InheritedEditorBrowsableTagHelper",
                            assemblyName: AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "InheritedEditorBrowsableTagHelper",
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName("Property"))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    { "TestNamespace.EditorBrowsableTagHelper", true, null },
                    {
                        "TestNamespace.EditorBrowsableTagHelper",
                        false,
                        CreateTagHelperDescriptor(
                            tagName: "editor-browsable",
                            typeName: "TestNamespace.EditorBrowsableTagHelper",
                            assemblyName: AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "EditorBrowsableTagHelper",
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName("Property"))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        "TestNamespace.HiddenPropertyEditorBrowsableTagHelper",
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "hidden-property-editor-browsable",
                            typeName: "TestNamespace.HiddenPropertyEditorBrowsableTagHelper",
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "HiddenPropertyEditorBrowsableTagHelper",
                            assemblyName: AssemblyName)
                    },
                    {
                        "TestNamespace.HiddenPropertyEditorBrowsableTagHelper",
                        false,
                        CreateTagHelperDescriptor(
                            tagName: "hidden-property-editor-browsable",
                            typeName: "TestNamespace.HiddenPropertyEditorBrowsableTagHelper",
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "HiddenPropertyEditorBrowsableTagHelper",
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName("Property"))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        "TestNamespace.OverriddenEditorBrowsableTagHelper",
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "overridden-editor-browsable",
                            typeName: "TestNamespace.OverriddenEditorBrowsableTagHelper",
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "OverriddenEditorBrowsableTagHelper",
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName("Property"))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        "TestNamespace.MultiPropertyEditorBrowsableTagHelper",
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "multi-property-editor-browsable",
                            typeName: "TestNamespace.MultiPropertyEditorBrowsableTagHelper",
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "MultiPropertyEditorBrowsableTagHelper",
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property2")
                                    .Metadata(PropertyName("Property2"))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        "TestNamespace.MultiPropertyEditorBrowsableTagHelper",
                        false,
                        CreateTagHelperDescriptor(
                            tagName: "multi-property-editor-browsable",
                            typeName: "TestNamespace.MultiPropertyEditorBrowsableTagHelper",
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "MultiPropertyEditorBrowsableTagHelper",
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName("Property"))
                                    .TypeName(typeof(int).FullName),
                                builder => builder
                                    .Name("property2")
                                    .Metadata(PropertyName("Property2"))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        "TestNamespace.OverriddenPropertyEditorBrowsableTagHelper",
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "overridden-property-editor-browsable",
                            typeName: "TestNamespace.OverriddenPropertyEditorBrowsableTagHelper",
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "OverriddenPropertyEditorBrowsableTagHelper",
                            assemblyName: AssemblyName)
                    },
                    {
                        "TestNamespace.OverriddenPropertyEditorBrowsableTagHelper",
                        false,
                        CreateTagHelperDescriptor(
                            tagName: "overridden-property-editor-browsable",
                            typeName: "TestNamespace.OverriddenPropertyEditorBrowsableTagHelper",
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "OverriddenPropertyEditorBrowsableTagHelper",
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property2")
                                    .Metadata(PropertyName("Property2"))
                                    .TypeName(typeof(int).FullName),
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName("Property"))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        "TestNamespace.DefaultEditorBrowsableTagHelper",
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "default-editor-browsable",
                            typeName: "TestNamespace.DefaultEditorBrowsableTagHelper",
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "DefaultEditorBrowsableTagHelper",
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName("Property"))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    { "TestNamespace.MultiEditorBrowsableTagHelper", true, null }
                };
        }
    }

    [Theory]
    [MemberData(nameof(EditorBrowsableData))]
    public void CreateDescriptor_UnderstandsEditorBrowsableAttribute(
        string tagHelperTypeFullName,
        bool designTime,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, designTime, designTime);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperTypeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    public static TheoryData AttributeTargetData
    {
        get
        {
            var attributes = Enumerable.Empty<BoundAttributeDescriptor>();

            // tagHelperType, expectedDescriptor
            return new TheoryData<string, TagHelperDescriptor>
                {
                    {
                        "TestNamespace.AttributeTargetingTagHelper",
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            "TestNamespace.AttributeTargetingTagHelper",
                            AssemblyName,
                            "TestNamespace",
                            "AttributeTargetingTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("class")),
                            })
                    },
                    {
                        "TestNamespace.MultiAttributeTargetingTagHelper",
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            "TestNamespace.MultiAttributeTargetingTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "MultiAttributeTargetingTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder =>
                                {
                                    builder
                                        .RequireAttributeDescriptor(attribute => attribute.Name("class"))
                                        .RequireAttributeDescriptor(attribute => attribute.Name("style"));
                                },
                            })
                    },
                    {
                        "TestNamespace.MultiAttributeAttributeTargetingTagHelper",
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            "TestNamespace.MultiAttributeAttributeTargetingTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "MultiAttributeAttributeTargetingTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("custom")),
                                builder =>
                                {
                                    builder
                                        .RequireAttributeDescriptor(attribute => attribute.Name("class"))
                                        .RequireAttributeDescriptor(attribute => attribute.Name("style"));
                                },
                            })
                    },
                    {
                        "TestNamespace.InheritedAttributeTargetingTagHelper",
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            "TestNamespace.InheritedAttributeTargetingTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "InheritedAttributeTargetingTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("style")),
                            })
                    },
                    {
                        "TestNamespace.RequiredAttributeTagHelper",
                        CreateTagHelperDescriptor(
                            "input",
                            "TestNamespace.RequiredAttributeTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "RequiredAttributeTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("class")),
                            })
                    },
                    {
                        "TestNamespace.InheritedRequiredAttributeTagHelper",
                        CreateTagHelperDescriptor(
                            "div",
                            "TestNamespace.InheritedRequiredAttributeTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "InheritedRequiredAttributeTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("class")),
                            })
                    },
                    {
                        "TestNamespace.MultiAttributeRequiredAttributeTagHelper",
                        CreateTagHelperDescriptor(
                            "div",
                            "TestNamespace.MultiAttributeRequiredAttributeTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "MultiAttributeRequiredAttributeTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder
                                    .RequireTagName("div")
                                    .RequireAttributeDescriptor(attribute => attribute.Name("class")),
                                builder => builder
                                    .RequireTagName("input")
                                    .RequireAttributeDescriptor(attribute => attribute.Name("class")),
                            })
                    },
                    {
                        "TestNamespace.MultiAttributeSameTagRequiredAttributeTagHelper",
                        CreateTagHelperDescriptor(
                            "input",
                            "TestNamespace.MultiAttributeSameTagRequiredAttributeTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "MultiAttributeSameTagRequiredAttributeTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("style")),
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("class")),
                            })
                    },
                    {
                        "TestNamespace.MultiRequiredAttributeTagHelper",
                        CreateTagHelperDescriptor(
                            "input",
                            "TestNamespace.MultiRequiredAttributeTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "MultiRequiredAttributeTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder
                                    .RequireAttributeDescriptor(attribute => attribute.Name("class"))
                                    .RequireAttributeDescriptor(attribute => attribute.Name("style")),
                            })
                    },
                    {
                        "TestNamespace.MultiTagMultiRequiredAttributeTagHelper",
                        CreateTagHelperDescriptor(
                            "div",
                            "TestNamespace.MultiTagMultiRequiredAttributeTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "MultiTagMultiRequiredAttributeTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder
                                    .RequireTagName("div")
                                    .RequireAttributeDescriptor(attribute => attribute.Name("class"))
                                    .RequireAttributeDescriptor(attribute => attribute.Name("style")),
                                builder => builder
                                    .RequireTagName("input")
                                    .RequireAttributeDescriptor(attribute => attribute.Name("class"))
                                    .RequireAttributeDescriptor(attribute => attribute.Name("style")),
                            })
                    },
                    {
                        "TestNamespace.AttributeWildcardTargetingTagHelper",
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            "TestNamespace.AttributeWildcardTargetingTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "AttributeWildcardTargetingTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder
                                    .RequireAttributeDescriptor(attribute => attribute
                                        .Name("class", RequiredAttributeNameComparison.PrefixMatch)),
                            })
                    },
                    {
                        "TestNamespace.MultiAttributeWildcardTargetingTagHelper",
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            "TestNamespace.MultiAttributeWildcardTargetingTagHelper",
                            AssemblyName,
                            typeNamespace: "TestNamespace",
                            typeNameIdentifier: "MultiAttributeWildcardTargetingTagHelper",
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder
                                    .RequireAttributeDescriptor(attribute => attribute
                                        .Name("class", RequiredAttributeNameComparison.PrefixMatch))
                                    .RequireAttributeDescriptor(attribute => attribute
                                        .Name("style", RequiredAttributeNameComparison.PrefixMatch)),
                            })
                    },
                };
        }
    }

    [Theory]
    [MemberData(nameof(AttributeTargetData))]
    public void CreateDescriptor_ReturnsExpectedDescriptors(
        string tagHelperTypeFullName,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperTypeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    public static TheoryData HtmlCaseData
    {
        get
        {
            // tagHelperType, expectedTagName, expectedAttributeName
            return new TheoryData<string, string, string>
            {
                { "TestNamespace.SingleAttributeTagHelper", "single-attribute", "int-attribute" },
                { "TestNamespace.ALLCAPSTAGHELPER", "allcaps", "allcapsattribute" },
                { "TestNamespace.CAPSOnOUTSIDETagHelper", "caps-on-outside", "caps-on-outsideattribute" },
                { "TestNamespace.capsONInsideTagHelper", "caps-on-inside", "caps-on-insideattribute" },
                { "TestNamespace.One1Two2Three3TagHelper", "one1-two2-three3", "one1-two2-three3-attribute" },
                { "TestNamespace.ONE1TWO2THREE3TagHelper", "one1two2three3", "one1two2three3-attribute" },
                { "TestNamespace.First_Second_ThirdHiTagHelper", "first_second_third-hi", "first_second_third-attribute" },
                { "TestNamespace.UNSuffixedCLASS", "un-suffixed-class", "un-suffixed-attribute" },
            };
        }
    }

    [Theory]
    [MemberData(nameof(HtmlCaseData))]
    public void CreateDescriptor_HtmlCasesTagNameAndAttributeName(
        string tagHelperTypeFullName,
        string expectedTagName,
        string expectedAttributeName)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperTypeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        var rule = Assert.Single(descriptor.TagMatchingRules);
        Assert.Equal(expectedTagName, rule.TagName, StringComparer.Ordinal);
        var attributeDescriptor = Assert.Single(descriptor.BoundAttributes);
        Assert.Equal(expectedAttributeName, attributeDescriptor.Name);
    }

    [Fact]
    public void CreateDescriptor_OverridesAttributeNameFromAttribute()
    {
        // Arrange
        var expectedDescriptor =
            CreateTagHelperDescriptor(
                "overridden-attribute",
                "TestNamespace.OverriddenAttributeTagHelper",
                AssemblyName,
                typeNamespace: "TestNamespace",
                typeNameIdentifier: "OverriddenAttributeTagHelper",
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("SomethingElse")
                        .Metadata(PropertyName("ValidAttribute1"))
                        .TypeName(typeof(string).FullName),
                    builder => builder
                        .Name("Something-Else")
                        .Metadata(PropertyName("ValidAttribute2"))
                        .TypeName(typeof(string).FullName),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.OverriddenAttributeTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_DoesNotInheritOverridenAttributeName()
    {
        // Arrange
        var expectedDescriptor =
            CreateTagHelperDescriptor(
                "inherited-overridden-attribute",
                "TestNamespace.InheritedOverriddenAttributeTagHelper",
                AssemblyName,
                typeNamespace: "TestNamespace",
                typeNameIdentifier: "InheritedOverriddenAttributeTagHelper",
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("valid-attribute1")
                        .Metadata(PropertyName("ValidAttribute1"))
                        .TypeName(typeof(string).FullName),
                    builder => builder
                        .Name("Something-Else")
                        .Metadata(PropertyName("ValidAttribute2"))
                        .TypeName(typeof(string).FullName),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.InheritedOverriddenAttributeTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_AllowsOverriddenAttributeNameOnUnimplementedVirtual()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
                "inherited-not-overridden-attribute",
                "TestNamespace.InheritedNotOverriddenAttributeTagHelper",
                AssemblyName,
                typeNamespace: "TestNamespace",
                typeNameIdentifier: "InheritedNotOverriddenAttributeTagHelper",
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("SomethingElse")
                        .Metadata(PropertyName("ValidAttribute1"))
                        .TypeName(typeof(string).FullName),
                    builder => builder
                        .Name("Something-Else")
                        .Metadata(PropertyName("ValidAttribute2"))
                        .TypeName(typeof(string).FullName),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.InheritedNotOverriddenAttributeTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_BuildsDescriptorsWithInheritedProperties()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            "inherited-single-attribute",
            "TestNamespace.InheritedSingleAttributeTagHelper",
            AssemblyName,
            typeNamespace: "TestNamespace",
            typeNameIdentifier: "InheritedSingleAttributeTagHelper",
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("int-attribute")
                    .Metadata(PropertyName("IntAttribute"))
                    .TypeName(typeof(int).FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.InheritedSingleAttributeTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_BuildsDescriptorsWithConventionNames()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            "single-attribute",
            "TestNamespace.SingleAttributeTagHelper",
            AssemblyName,
            typeNamespace: "TestNamespace",
            typeNameIdentifier: "SingleAttributeTagHelper",
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("int-attribute")
                    .Metadata(PropertyName("IntAttribute"))
                    .TypeName(typeof(int).FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.SingleAttributeTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_OnlyAcceptsPropertiesWithGetAndSet()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            "missing-accessor",
            "TestNamespace.MissingAccessorTagHelper",
            AssemblyName,
            typeNamespace: "TestNamespace",
            typeNameIdentifier: "MissingAccessorTagHelper",
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("valid-attribute")
                    .Metadata(PropertyName("ValidAttribute"))
                    .TypeName(typeof(string).FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.MissingAccessorTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_OnlyAcceptsPropertiesWithPublicGetAndSet()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            "non-public-accessor",
            "TestNamespace.NonPublicAccessorTagHelper",
            AssemblyName,
            typeNamespace: "TestNamespace",
            typeNameIdentifier: "NonPublicAccessorTagHelper",
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("valid-attribute")
                    .Metadata(PropertyName("ValidAttribute"))
                    .TypeName(typeof(string).FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.NonPublicAccessorTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_DoesNotIncludePropertiesWithNotBound()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            "not-bound-attribute",
            "TestNamespace.NotBoundAttributeTagHelper",
            AssemblyName,
            typeNamespace: "TestNamespace",
            typeNameIdentifier: "NotBoundAttributeTagHelper",
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("bound-property")
                    .Metadata(PropertyName("BoundProperty"))
                    .TypeName(typeof(object).FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.NotBoundAttributeTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_ResolvesMultipleTagHelperDescriptorsFromSingleType()
    {
        // Arrange
        var expectedDescriptor =
            CreateTagHelperDescriptor(
                string.Empty,
                "TestNamespace.MultiTagTagHelper",
                AssemblyName,
                typeNamespace: "TestNamespace",
                typeNameIdentifier: "MultiTagTagHelper",
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("valid-attribute")
                        .Metadata(PropertyName("ValidAttribute"))
                        .TypeName(typeof(string).FullName),
                },
                new Action<TagMatchingRuleDescriptorBuilder>[]
                {
                    builder => builder.RequireTagName("p"),
                    builder => builder.RequireTagName("div"),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.MultiTagTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_DoesNotResolveInheritedTagNames()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
                "inherited-multi-tag",
                "TestNamespace.InheritedMultiTagTagHelper",
                AssemblyName,
                typeNamespace: "TestNamespace",
                typeNameIdentifier: "InheritedMultiTagTagHelper",
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("valid-attribute")
                        .Metadata(PropertyName("ValidAttribute"))
                        .TypeName(typeof(string).FullName),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.InheritedMultiTagTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_IgnoresDuplicateTagNamesFromAttribute()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            string.Empty,
            "TestNamespace.DuplicateTagNameTagHelper",
            AssemblyName,
            typeNamespace: "TestNamespace",
            typeNameIdentifier: "DuplicateTagNameTagHelper",
            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
            {
                    builder => builder.RequireTagName("p"),
                    builder => builder.RequireTagName("div"),
            });

        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.DuplicateTagNameTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_OverridesTagNameFromAttribute()
    {
        // Arrange
        var expectedDescriptor =
            CreateTagHelperDescriptor(
                "data-condition",
                "TestNamespace.OverrideNameTagHelper",
                AssemblyName,
                typeNamespace: "TestNamespace",
                typeNameIdentifier: "OverrideNameTagHelper");
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName("TestNamespace.OverrideNameTagHelper");

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    // name, expectedErrorMessages
    public static TheoryData<string, string[]> InvalidNameData
    {
        get
        {
            Func<string, string, string> onNameError =
                (invalidText, invalidCharacter) => $"Tag helpers cannot target tag name '{invalidText}' because it contains a '{invalidCharacter}' character.";
            var whitespaceErrorString = "Targeted tag name cannot be null or whitespace.";

            var data = GetInvalidNameOrPrefixData(onNameError, whitespaceErrorString, onDataError: null);
            data.Add(string.Empty, new[] { whitespaceErrorString });

            return data;
        }
    }

    [Theory]
    [MemberData(nameof(InvalidNameData))]
    public void CreateDescriptor_CreatesErrorOnInvalidNames(
        string name, string[] expectedErrorMessages)
    {
        // Arrange
        name = name.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"");
        var text = $$"""
            [Microsoft.AspNetCore.Razor.TagHelpers.HtmlTargetElementAttribute("{{name}}")]
            public class DynamicTestTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
            {
            }
            """;
        var syntaxTree = Parse(text);
        var compilation = Compilation.AddSyntaxTrees(syntaxTree);
        var tagHelperType = compilation.GetTypeByMetadataName("DynamicTestTagHelper");
        var attribute = tagHelperType.GetAttributes().Single();
        var factory = new DefaultTagHelperDescriptorFactory(compilation, includeDocumentation: false, excludeHidden: false);

        // Act
        var descriptor = factory.CreateDescriptor(tagHelperType);

        // Assert
        var rule = Assert.Single(descriptor.TagMatchingRules);
        var errorMessages = rule.GetAllDiagnostics().Select(diagnostic => diagnostic.GetMessage(CultureInfo.CurrentCulture)).ToArray();
        Assert.Equal(expectedErrorMessages.Length, errorMessages.Length);
        for (var i = 0; i < expectedErrorMessages.Length; i++)
        {
            Assert.Equal(expectedErrorMessages[i], errorMessages[i], StringComparer.Ordinal);
        }
    }

    public static TheoryData ValidNameData
    {
        get
        {
            // name, expectedNames
            return new TheoryData<string, IEnumerable<string>>
                {
                    { "p", new[] { "p" } },
                    { " p", new[] { "p" } },
                    { "p ", new[] { "p" } },
                    { " p ", new[] { "p" } },
                    { "p,div", new[] { "p", "div" } },
                    { " p,div", new[] { "p", "div" } },
                    { "p ,div", new[] { "p", "div" } },
                    { " p ,div", new[] { "p", "div" } },
                    { "p, div", new[] { "p", "div" } },
                    { "p,div ", new[] { "p", "div" } },
                    { "p, div ", new[] { "p", "div" } },
                    { " p, div ", new[] { "p", "div" } },
                    { " p , div ", new[] { "p", "div" } },
                };
        }
    }

    public static TheoryData InvalidTagHelperAttributeDescriptorData
    {
        get
        {
            var invalidBoundAttributeBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "InvalidBoundAttribute", "Test");
            invalidBoundAttributeBuilder.Metadata(TypeName("TestNamespace.InvalidBoundAttribute"));

            // type, expectedAttributeDescriptors
            return new TheoryData<string, IEnumerable<BoundAttributeDescriptor>>
            {
                {
                    "TestNamespace.InvalidBoundAttribute",
                    new[]
                    {
                        CreateAttributeFor("TestNamespace.InvalidBoundAttribute", attribute =>
                        {
                            attribute
                                .Name("data-something")
                                .Metadata(PropertyName("DataSomething"))
                                .TypeName(typeof(string).FullName);
                        }),
                    }
                },
                {
                    "TestNamespace.InvalidBoundAttributeWithValid",
                    new[]
                    {
                        CreateAttributeFor("TestNamespace.InvalidBoundAttributeWithValid", attribute =>
                        {
                            attribute
                                .Name("data-something")
                                .Metadata(PropertyName("DataSomething"))
                                .TypeName(typeof(string).FullName); ;
                        }),
                        CreateAttributeFor("TestNamespace.InvalidBoundAttributeWithValid", attribute =>
                        {
                            attribute
                            .Name("int-attribute")
                            .Metadata(PropertyName("IntAttribute"))
                            .TypeName(typeof(int).FullName);
                        }),
                    }
                },
                {
                    "TestNamespace.OverriddenInvalidBoundAttributeWithValid",
                    new[]
                    {
                        CreateAttributeFor("TestNamespace.OverriddenInvalidBoundAttributeWithValid", attribute =>
                        {
                            attribute
                            .Name("valid-something")
                            .Metadata(PropertyName("DataSomething"))
                            .TypeName(typeof(string).FullName);
                        }),
                    }
                },
                {
                    "TestNamespace.OverriddenValidBoundAttributeWithInvalid",
                    new[]
                    {
                        CreateAttributeFor("TestNamespace.OverriddenValidBoundAttributeWithInvalid", attribute =>
                        {
                            attribute
                            .Name("data-something")
                            .Metadata(PropertyName("ValidSomething"))
                            .TypeName(typeof(string).FullName);
                        }),
                    }
                },
                {
                    "TestNamespace.OverriddenValidBoundAttributeWithInvalidUpperCase",
                    new[]
                    {
                        CreateAttributeFor("TestNamespace.OverriddenValidBoundAttributeWithInvalidUpperCase", attribute =>
                        {
                            attribute
                            .Name("DATA-SOMETHING")
                            .Metadata(PropertyName("ValidSomething"))
                            .TypeName(typeof(string).FullName);
                        }),
                    }
                },
            };
        }
    }

    [Theory]
    [MemberData(nameof(InvalidTagHelperAttributeDescriptorData))]
    public void CreateDescriptor_DoesNotAllowDataDashAttributes(
        string typeFullName,
        IEnumerable<BoundAttributeDescriptor> expectedAttributeDescriptors)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedAttributeDescriptors, descriptor.BoundAttributes);

        var id = AspNetCore.Razor.Language.RazorDiagnosticFactory.TagHelper_InvalidBoundAttributeNameStartsWith.Id;
        foreach (var attribute in descriptor.BoundAttributes.Where(a => a.Name.StartsWith("data-", StringComparison.OrdinalIgnoreCase)))
        {
            var diagnostic = Assert.Single(attribute.Diagnostics);
            Assert.Equal(id, diagnostic.Id);
        }
    }

    public static TheoryData<string> ValidAttributeNameData
    {
        get
        {
            return new TheoryData<string>
                {
                    "data",
                    "dataa-",
                    "ValidName",
                    "valid-name",
                    "--valid--name--",
                    ",,--__..oddly.valid::;;",
                };
        }
    }

    [Theory]
    [MemberData(nameof(ValidAttributeNameData))]
    public void CreateDescriptor_WithValidAttributeName_HasNoErrors(string name)
    {
        // Arrange
        var text = $$"""
            public class DynamicTestTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
            {
                [Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute("{{name}}")]
                public string SomeAttribute { get; set; }
            }
            """;
        var syntaxTree = Parse(text);
        var compilation = Compilation.AddSyntaxTrees(syntaxTree);
        var tagHelperType = compilation.GetTypeByMetadataName("DynamicTestTagHelper");
        var factory = new DefaultTagHelperDescriptorFactory(compilation, includeDocumentation: false, excludeHidden: false);

        // Act
        var descriptor = factory.CreateDescriptor(tagHelperType);

        // Assert
        Assert.False(descriptor.HasErrors);
    }

    public static TheoryData<string> ValidAttributePrefixData
    {
        get
        {
            return new TheoryData<string>
                {
                    string.Empty,
                    "data",
                    "dataa-",
                    "ValidName",
                    "valid-name",
                    "--valid--name--",
                    ",,--__..oddly.valid::;;",
                };
        }
    }

    [Theory]
    [MemberData(nameof(ValidAttributePrefixData))]
    public void CreateDescriptor_WithValidAttributePrefix_HasNoErrors(string prefix)
    {
        // Arrange
        var text = $$"""
            public class DynamicTestTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
            {
                [Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute(DictionaryAttributePrefix = "{{prefix}}")]
                public System.Collections.Generic.IDictionary<string, int> SomeAttribute { get; set; }
            }
            """;
        var syntaxTree = Parse(text);
        var compilation = Compilation.AddSyntaxTrees(syntaxTree);
        var tagHelperType = compilation.GetTypeByMetadataName("DynamicTestTagHelper");
        var factory = new DefaultTagHelperDescriptorFactory(compilation, includeDocumentation: false, excludeHidden: false);

        // Act
        var descriptor = factory.CreateDescriptor(tagHelperType);

        // Assert
        Assert.False(descriptor.HasErrors);
    }

    // name, expectedErrorMessages
    public static TheoryData<string, string[]> InvalidAttributeNameData
    {
        get
        {
            Func<string, string, string> onNameError = (invalidText, invalidCharacter) =>
                "Invalid tag helper bound property 'string DynamicTestTagHelper.InvalidProperty' on tag helper 'DynamicTestTagHelper'. Tag helpers " +
                $"cannot bind to HTML attributes with name '{invalidText}' because the name contains a '{invalidCharacter}' character.";
            var whitespaceErrorString =
                "Invalid tag helper bound property 'string DynamicTestTagHelper.InvalidProperty' on tag helper 'DynamicTestTagHelper'. Tag helpers cannot " +
                "bind to HTML attributes with a null or empty name.";
            Func<string, string> onDataError = invalidText =>
            "Invalid tag helper bound property 'string DynamicTestTagHelper.InvalidProperty' on tag helper 'DynamicTestTagHelper'. Tag helpers cannot bind " +
            $"to HTML attributes with name '{invalidText}' because the name starts with 'data-'.";

            return GetInvalidNameOrPrefixData(onNameError, whitespaceErrorString, onDataError);
        }
    }

    [Theory]
    [MemberData(nameof(InvalidAttributeNameData))]
    public void CreateDescriptor_WithInvalidAttributeName_HasErrors(string name, string[] expectedErrorMessages)
    {
        // Arrange
        name = name.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"");
        var text = $$"""
            public class DynamicTestTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
            {
                [Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute("{{name}}")]
                public string InvalidProperty { get; set; }
            }
            """;
        var syntaxTree = Parse(text);
        var compilation = Compilation.AddSyntaxTrees(syntaxTree);
        var tagHelperType = compilation.GetTypeByMetadataName("DynamicTestTagHelper");
        var factory = new DefaultTagHelperDescriptorFactory(compilation, includeDocumentation: false, excludeHidden: false);

        // Act
        var descriptor = factory.CreateDescriptor(tagHelperType);

        // Assert
        var errorMessages = descriptor.GetAllDiagnostics().Select(diagnostic => diagnostic.GetMessage(CultureInfo.CurrentCulture));
        Assert.Equal(expectedErrorMessages, errorMessages);
    }

    // prefix, expectedErrorMessages
    public static TheoryData<string, string[]> InvalidAttributePrefixData
    {
        get
        {
            Func<string, string, string> onPrefixError = (invalidText, invalidCharacter) =>
                "Invalid tag helper bound property 'System.Collections.Generic.IDictionary<System.String, System.Int32> DynamicTestTagHelper.InvalidProperty' " +
                "on tag helper 'DynamicTestTagHelper'. Tag helpers " +
                $"cannot bind to HTML attributes with prefix '{invalidText}' because the prefix contains a '{invalidCharacter}' character.";
            var whitespaceErrorString =
                "Invalid tag helper bound property 'System.Collections.Generic.IDictionary<System.String, System.Int32> DynamicTestTagHelper.InvalidProperty' " +
                "on tag helper 'DynamicTestTagHelper'. Tag helpers cannot bind to HTML attributes with a null or empty name.";
            Func<string, string> onDataError = invalidText =>
                "Invalid tag helper bound property 'System.Collections.Generic.IDictionary<System.String, System.Int32> DynamicTestTagHelper.InvalidProperty' " +
                "on tag helper 'DynamicTestTagHelper'. Tag helpers cannot bind to HTML attributes " +
                $"with prefix '{invalidText}' because the prefix starts with 'data-'.";

            return GetInvalidNameOrPrefixData(onPrefixError, whitespaceErrorString, onDataError);
        }
    }

    [Theory]
    [MemberData(nameof(InvalidAttributePrefixData))]
    public void CreateDescriptor_WithInvalidAttributePrefix_HasErrors(string prefix, string[] expectedErrorMessages)
    {
        // Arrange
        prefix = prefix.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"");
        var text = $$"""
            public class DynamicTestTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
            {
                [Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute(DictionaryAttributePrefix = "{{prefix}}")]
                public System.Collections.Generic.IDictionary<System.String, System.Int32> InvalidProperty { get; set; }
            }
            """;
        var syntaxTree = Parse(text);
        var compilation = Compilation.AddSyntaxTrees(syntaxTree);
        var tagHelperType = compilation.GetTypeByMetadataName("DynamicTestTagHelper");
        var factory = new DefaultTagHelperDescriptorFactory(compilation, includeDocumentation: false, excludeHidden: false);

        // Act
        var descriptor = factory.CreateDescriptor(tagHelperType);

        // Assert
        var errorMessages = descriptor.GetAllDiagnostics().Select(diagnostic => diagnostic.GetMessage(CultureInfo.CurrentCulture));
        Assert.Equal(expectedErrorMessages, errorMessages);
    }

    public static TheoryData<string, string[]> InvalidRestrictChildrenNameData
    {
        get
        {
            var nullOrWhiteSpaceError =
                AspNetCore.Razor.Language.Resources.FormatTagHelper_InvalidRestrictedChildNullOrWhitespace("DynamicTestTagHelper");

            return GetInvalidNameOrPrefixData(
                onNameError: (invalidInput, invalidCharacter) =>
                    AspNetCore.Razor.Language.Resources.FormatTagHelper_InvalidRestrictedChild(
                        "DynamicTestTagHelper",
                        invalidInput,
                        invalidCharacter),
                whitespaceErrorString: nullOrWhiteSpaceError,
                onDataError: null);
        }
    }

    [Theory]
    [MemberData(nameof(InvalidRestrictChildrenNameData))]
    public void CreateDescriptor_WithInvalidAllowedChildren_HasErrors(string name, string[] expectedErrorMessages)
    {
        // Arrange
        name = name.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"");
        var text = $$"""
            [Microsoft.AspNetCore.Razor.TagHelpers.RestrictChildrenAttribute("{{name}}")]
            public class DynamicTestTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
            {
            }
            """;
        var syntaxTree = Parse(text);
        var compilation = Compilation.AddSyntaxTrees(syntaxTree);
        var tagHelperType = compilation.GetTypeByMetadataName("DynamicTestTagHelper");
        var factory = new DefaultTagHelperDescriptorFactory(compilation, includeDocumentation: false, excludeHidden: false);

        // Act
        var descriptor = factory.CreateDescriptor(tagHelperType);

        // Assert
        var errorMessages = descriptor.GetAllDiagnostics().Select(diagnostic => diagnostic.GetMessage(CultureInfo.CurrentCulture));
        Assert.Equal(expectedErrorMessages, errorMessages);
    }

    public static TheoryData<string, string[]> InvalidParentTagData
    {
        get
        {
            var nullOrWhiteSpaceError =
                AspNetCore.Razor.Language.Resources.TagHelper_InvalidTargetedParentTagNameNullOrWhitespace;

            return GetInvalidNameOrPrefixData(
                onNameError: (invalidInput, invalidCharacter) =>
                    AspNetCore.Razor.Language.Resources.FormatTagHelper_InvalidTargetedParentTagName(
                        invalidInput,
                        invalidCharacter),
                whitespaceErrorString: nullOrWhiteSpaceError,
                onDataError: null);
        }
    }

    [Theory]
    [MemberData(nameof(InvalidParentTagData))]
    public void CreateDescriptor_WithInvalidParentTag_HasErrors(string name, string[] expectedErrorMessages)
    {
        // Arrange
        name = name.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\"", "\\\"");
        var text = $$"""
            [Microsoft.AspNetCore.Razor.TagHelpers.HtmlTargetElementAttribute(ParentTag = "{{name}}")]
            public class DynamicTestTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
            {
            }
            """;
        var syntaxTree = Parse(text);
        var compilation = Compilation.AddSyntaxTrees(syntaxTree);
        var tagHelperType = compilation.GetTypeByMetadataName("DynamicTestTagHelper");
        var factory = new DefaultTagHelperDescriptorFactory(compilation, includeDocumentation: false, excludeHidden: false);

        // Act
        var descriptor = factory.CreateDescriptor(tagHelperType);

        // Assert
        var errorMessages = descriptor.GetAllDiagnostics().Select(diagnostic => diagnostic.GetMessage(CultureInfo.CurrentCulture));
        Assert.Equal(expectedErrorMessages, errorMessages);
    }

    [Fact]
    public void CreateDescriptor_BuildsDescriptorsFromSimpleTypes()
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(Enumerable).FullName);
        var expectedDescriptor =
            CreateTagHelperDescriptor(
                "enumerable",
                "System.Linq.Enumerable",
                typeSymbol.ContainingAssembly.Identity.Name,
                typeSymbol.ContainingNamespace.ToDisplayString(),
                typeSymbol.Name);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    public static TheoryData TagHelperWithPrefixData
    {
        get
        {
            var dictionaryNamespace = typeof(IDictionary<,>).FullName;
            dictionaryNamespace = dictionaryNamespace.Substring(0, dictionaryNamespace.IndexOf('`'));

            // tagHelperType, expectedAttributeDescriptors, expectedDiagnostics
            return new TheoryData<string, IEnumerable<BoundAttributeDescriptor>, IEnumerable<RazorDiagnostic>>
                {
                    {
                        "TestNamespace.DefaultValidHtmlAttributePrefix",
                        new[]
                        {
                            CreateAttributeFor("TestNamespace.DefaultValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("dictionary-property")
                                    .Metadata(PropertyName("DictionaryProperty"))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.String>")
                                    .AsDictionaryAttribute("dictionary-property-", typeof(string).FullName);
                            }),
                        },
                        Enumerable.Empty<RazorDiagnostic>()
                    },
                    {
                        "TestNamespace.SingleValidHtmlAttributePrefix",
                        new[]
                        {
                            CreateAttributeFor("TestNamespace.SingleValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name")
                                    .Metadata(PropertyName("DictionaryProperty"))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.String>")
                                    .AsDictionaryAttribute("valid-name-", typeof(string).FullName);
                            }),
                        },
                        Enumerable.Empty<RazorDiagnostic>()
                    },
                    {
                        "TestNamespace.MultipleValidHtmlAttributePrefix",
                        new[]
                        {
                            CreateAttributeFor("TestNamespace.MultipleValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name1")
                                    .Metadata(PropertyName("DictionaryProperty"))
                                    .TypeName($"{typeof(Dictionary<,>).Namespace}.Dictionary<System.String, System.Object>")
                                    .AsDictionaryAttribute("valid-prefix1-", typeof(object).FullName);
                            }),
                            CreateAttributeFor("TestNamespace.MultipleValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name2")
                                    .Metadata(PropertyName("DictionarySubclassProperty"))
                                    .TypeName("TestNamespace.DictionarySubclass")
                                    .AsDictionaryAttribute("valid-prefix2-", typeof(string).FullName);
                            }),
                            CreateAttributeFor("TestNamespace.MultipleValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name3")
                                    .Metadata(PropertyName("DictionaryWithoutParameterlessConstructorProperty"))
                                    .TypeName("TestNamespace.DictionaryWithoutParameterlessConstructor")
                                    .AsDictionaryAttribute("valid-prefix3-", typeof(string).FullName);
                            }),
                            CreateAttributeFor("TestNamespace.MultipleValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name4")
                                    .Metadata(PropertyName("GenericDictionarySubclassProperty"))
                                    .TypeName("TestNamespace.GenericDictionarySubclass<System.Object>")
                                    .AsDictionaryAttribute("valid-prefix4-", typeof(object).FullName);
                            }),
                            CreateAttributeFor("TestNamespace.MultipleValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name5")
                                    .Metadata(PropertyName("SortedDictionaryProperty"))
                                    .TypeName(typeof(SortedDictionary<string, int>).Namespace + ".SortedDictionary<System.String, System.Int32>")
                                    .AsDictionaryAttribute("valid-prefix5-", typeof(int).FullName);
                            }),
                            CreateAttributeFor("TestNamespace.MultipleValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name6")
                                    .Metadata(PropertyName("StringProperty"))
                                    .TypeName(typeof(string).FullName);
                            }),
                            CreateAttributeFor("TestNamespace.MultipleValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Metadata(PropertyName("GetOnlyDictionaryProperty"))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.Int32>")
                                    .AsDictionaryAttribute("get-only-dictionary-property-", typeof(int).FullName);
                            }),
                            CreateAttributeFor("TestNamespace.MultipleValidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Metadata(PropertyName("GetOnlyDictionaryPropertyWithAttributePrefix"))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.String>")
                                    .AsDictionaryAttribute("valid-prefix6", typeof(string).FullName);
                            }),
                        },
                        Enumerable.Empty<RazorDiagnostic>()
                    },
                    {
                        "TestNamespace.SingleInvalidHtmlAttributePrefix",
                        new[]
                        {
                            CreateAttributeFor("TestNamespace.SingleInvalidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name")
                                    .Metadata(PropertyName("StringProperty"))
                                    .TypeName(typeof(string).FullName)
                                    .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                        "TestNamespace.SingleInvalidHtmlAttributePrefix",
                                        "StringProperty"));
                            }),
                        },
                        new[]
                        {
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                "TestNamespace.SingleInvalidHtmlAttributePrefix",
                                "StringProperty")
                        }
                    },
                    {
                        "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                        new[]
                        {
                            CreateAttributeFor("TestNamespace.MultipleInvalidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name1")
                                    .Metadata(PropertyName("LongProperty"))
                                    .TypeName(typeof(long).FullName);
                            }),
                            CreateAttributeFor("TestNamespace.MultipleInvalidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name2")
                                    .Metadata(PropertyName("DictionaryOfIntProperty"))
                                    .TypeName($"{typeof(Dictionary<,>).Namespace}.Dictionary<System.Int32, System.String>")
                                    .AsDictionaryAttribute("valid-prefix2-", typeof(string).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                            "DictionaryOfIntProperty"));
                            }),
                            CreateAttributeFor("TestNamespace.MultipleInvalidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name3")
                                    .Metadata(PropertyName("ReadOnlyDictionaryProperty"))
                                    .TypeName($"{typeof(IReadOnlyDictionary<,>).Namespace}.IReadOnlyDictionary<System.String, System.Object>")
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                            "ReadOnlyDictionaryProperty"));
                            }),
                            CreateAttributeFor("TestNamespace.MultipleInvalidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name4")
                                    .Metadata(PropertyName("IntProperty"))
                                    .TypeName(typeof(int).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                            "IntProperty"));
                            }),
                            CreateAttributeFor("TestNamespace.MultipleInvalidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Name("valid-name5")
                                    .Metadata(PropertyName("DictionaryOfIntSubclassProperty"))
                                    .TypeName("TestNamespace.DictionaryOfIntSubclass")
                                    .AsDictionaryAttribute("valid-prefix5-", typeof(string).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                            "DictionaryOfIntSubclassProperty"));
                            }),
                            CreateAttributeFor("TestNamespace.MultipleInvalidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Metadata(PropertyName("GetOnlyDictionaryAttributePrefix"))
                                    .TypeName($"{dictionaryNamespace}<System.Int32, System.String>")
                                    .AsDictionaryAttribute("valid-prefix6", typeof(string).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                            "GetOnlyDictionaryAttributePrefix"));
                            }),
                            CreateAttributeFor("TestNamespace.MultipleInvalidHtmlAttributePrefix", attribute =>
                            {
                                attribute
                                    .Metadata(PropertyName("GetOnlyDictionaryPropertyWithAttributeName"))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.Object>")
                                    .AsDictionaryAttribute("invalid-name7-", typeof(object).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNull(
                                            "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                            "GetOnlyDictionaryPropertyWithAttributeName"));
                            }),
                        },
                        new[]
                        {
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                "DictionaryOfIntProperty"),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                "ReadOnlyDictionaryProperty"),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                "IntProperty"),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                "DictionaryOfIntSubclassProperty"),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                "GetOnlyDictionaryAttributePrefix"),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNull(
                                "TestNamespace.MultipleInvalidHtmlAttributePrefix",
                                "GetOnlyDictionaryPropertyWithAttributeName"),
                        }
                    },
                };
        }
    }

    [Theory]
    [MemberData(nameof(TagHelperWithPrefixData))]
    public void CreateDescriptor_WithPrefixes_ReturnsExpectedAttributeDescriptors(
        string tagHelperTypeFullName,
        IEnumerable<BoundAttributeDescriptor> expectedAttributeDescriptors,
        IEnumerable<RazorDiagnostic> expectedDiagnostics)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperTypeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedAttributeDescriptors, descriptor.BoundAttributes);
        Assert.Equal(expectedDiagnostics, descriptor.GetAllDiagnostics());
    }

    public static TheoryData TagOutputHintData
    {
        get
        {
            // tagHelperType, expectedDescriptor
            return new TheoryData<string, TagHelperDescriptor>
            {
                {
                    "TestNamespace.MultipleDescriptorTagHelperWithOutputElementHint",
                    TagHelperDescriptorBuilder.Create("TestNamespace.MultipleDescriptorTagHelperWithOutputElementHint", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace", "MultipleDescriptorTagHelperWithOutputElementHint"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("a"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("p"))
                        .TagOutputHint("div")
                        .Build()
                },
                {
                    "TestNamespace2.InheritedOutputElementHintTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace2.InheritedOutputElementHintTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace2", "InheritedOutputElementHintTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("inherited-output-element-hint"))
                        .Build()
                },
                {
                    "TestNamespace2.OutputElementHintTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace2.OutputElementHintTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace2", "OutputElementHintTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("output-element-hint"))
                        .TagOutputHint("hinted-value")
                        .Build()
                },
                {
                    "TestNamespace2.OverriddenOutputElementHintTagHelper",
                    TagHelperDescriptorBuilder.Create("TestNamespace2.OverriddenOutputElementHintTagHelper", AssemblyName)
                        .Metadata(GetMetadata("TestNamespace2", "OverriddenOutputElementHintTagHelper"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("overridden-output-element-hint"))
                        .TagOutputHint("overridden")
                        .Build()
                },
            };
        }
    }

    [Theory]
    [MemberData(nameof(TagOutputHintData))]
    public void CreateDescriptor_CreatesDescriptorsWithOutputElementHint(
        string tagHelperTypeFullName,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperTypeFullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_CapturesDocumentationOnTagHelperClass()
    {
        // Arrange
        var syntaxTree = Parse(@"
        using Microsoft.AspNetCore.Razor.TagHelpers;

        /// <summary>
        /// The summary for <see cref=""DocumentedTagHelper""/>.
        /// </summary>
        /// <remarks>
        /// Inherits from <see cref=""TagHelper""/>.
        /// </remarks>
        public class DocumentedTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
        {
        }");
        var compilation = Compilation.AddSyntaxTrees(syntaxTree);
        var factory = new DefaultTagHelperDescriptorFactory(compilation, includeDocumentation: true, excludeHidden: false);
        var typeSymbol = compilation.GetTypeByMetadataName("DocumentedTagHelper");
        var expectedDocumentation =
@"<member name=""T:DocumentedTagHelper"">
    <summary>
    The summary for <see cref=""T:DocumentedTagHelper""/>.
    </summary>
    <remarks>
    Inherits from <see cref=""T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper""/>.
    </remarks>
</member>
";

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDocumentation, descriptor.Documentation);
    }

    [Fact]
    public void CreateDescriptor_CapturesDocumentationOnTagHelperProperties()
    {
        // Arrange
        var syntaxTree = Parse(@"
        using System.Collections.Generic;

        public class DocumentedTagHelper : Microsoft.spNetCore.Razor.TagHelpers.TagHelper
        {
            /// <summary>
            /// This <see cref=""SummaryProperty""/> is of type <see cref=""string""/>.
            /// </summary>
            public string SummaryProperty { get; set; }

            /// <remarks>
            /// The <see cref=""SummaryProperty""/> may be <c>null</c>.
            /// </remarks>
            public int RemarksProperty { get; set; }

            /// <summary>
            /// This is a complex <see cref=""List{bool}""/>.
            /// </summary>
            /// <remarks>
            /// <see cref=""SummaryProperty""/><see cref=""RemarksProperty""/>
            /// </remarks>
            public List<bool> RemarksAndSummaryProperty { get; set; }
        }");
        var compilation = Compilation.AddSyntaxTrees(syntaxTree);
        var factory = new DefaultTagHelperDescriptorFactory(compilation, includeDocumentation: true, excludeHidden: false);
        var typeSymbol = compilation.GetTypeByMetadataName("DocumentedTagHelper");
        var expectedDocumentations = new[]
        {

@"<member name=""P:DocumentedTagHelper.SummaryProperty"">
    <summary>
    This <see cref=""P:DocumentedTagHelper.SummaryProperty""/> is of type <see cref=""T:System.String""/>.
    </summary>
</member>
",
@"<member name=""P:DocumentedTagHelper.RemarksProperty"">
    <remarks>
    The <see cref=""P:DocumentedTagHelper.SummaryProperty""/> may be <c>null</c>.
    </remarks>
</member>
",
@"<member name=""P:DocumentedTagHelper.RemarksAndSummaryProperty"">
    <summary>
    This is a complex <see cref=""T:System.Collections.Generic.List`1""/>.
    </summary>
    <remarks>
    <see cref=""P:DocumentedTagHelper.SummaryProperty""/><see cref=""P:DocumentedTagHelper.RemarksProperty""/>
    </remarks>
</member>
",
                    };

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        var documentations = descriptor.BoundAttributes.Select(boundAttribute => boundAttribute.Documentation);
        Assert.Equal(expectedDocumentations, documentations);
    }

    private static TheoryData<string, string[]> GetInvalidNameOrPrefixData(
        Func<string, string, string> onNameError,
        string whitespaceErrorString,
        Func<string, string> onDataError)
    {
        // name, expectedErrorMessages
        var data = new TheoryData<string, string[]>
            {
                { "!", new[] {  onNameError("!", "!") } },
                { "hello!", new[] { onNameError("hello!", "!") } },
                { "!hello", new[] { onNameError("!hello", "!") } },
                { "he!lo", new[] { onNameError("he!lo", "!") } },
                { "!he!lo!", new[] { onNameError("!he!lo!", "!") } },
                { "@", new[] { onNameError("@", "@") } },
                { "hello@", new[] { onNameError("hello@", "@") } },
                { "@hello", new[] { onNameError("@hello", "@") } },
                { "he@lo", new[] { onNameError("he@lo", "@") } },
                { "@he@lo@", new[] { onNameError("@he@lo@", "@") } },
                { "/", new[] { onNameError("/", "/") } },
                { "hello/", new[] { onNameError("hello/", "/") } },
                { "/hello", new[] { onNameError("/hello", "/") } },
                { "he/lo", new[] { onNameError("he/lo", "/") } },
                { "/he/lo/", new[] { onNameError("/he/lo/", "/") } },
                { "<", new[] { onNameError("<", "<") } },
                { "hello<", new[] { onNameError("hello<", "<") } },
                { "<hello", new[] { onNameError("<hello", "<") } },
                { "he<lo", new[] { onNameError("he<lo", "<") } },
                { "<he<lo<", new[] { onNameError("<he<lo<", "<") } },
                { "?", new[] { onNameError("?", "?") } },
                { "hello?", new[] { onNameError("hello?", "?") } },
                { "?hello", new[] { onNameError("?hello", "?") } },
                { "he?lo", new[] { onNameError("he?lo", "?") } },
                { "?he?lo?", new[] { onNameError("?he?lo?", "?") } },
                { "[", new[] { onNameError("[", "[") } },
                { "hello[", new[] { onNameError("hello[", "[") } },
                { "[hello", new[] { onNameError("[hello", "[") } },
                { "he[lo", new[] { onNameError("he[lo", "[") } },
                { "[he[lo[", new[] { onNameError("[he[lo[", "[") } },
                { ">", new[] { onNameError(">", ">") } },
                { "hello>", new[] { onNameError("hello>", ">") } },
                { ">hello", new[] { onNameError(">hello", ">") } },
                { "he>lo", new[] { onNameError("he>lo", ">") } },
                { ">he>lo>", new[] { onNameError(">he>lo>", ">") } },
                { "]", new[] { onNameError("]", "]") } },
                { "hello]", new[] { onNameError("hello]", "]") } },
                { "]hello", new[] { onNameError("]hello", "]") } },
                { "he]lo", new[] { onNameError("he]lo", "]") } },
                { "]he]lo]", new[] { onNameError("]he]lo]", "]") } },
                { "=", new[] { onNameError("=", "=") } },
                { "hello=", new[] { onNameError("hello=", "=") } },
                { "=hello", new[] { onNameError("=hello", "=") } },
                { "he=lo", new[] { onNameError("he=lo", "=") } },
                { "=he=lo=", new[] { onNameError("=he=lo=", "=") } },
                { "\"", new[] { onNameError("\"", "\"") } },
                { "hello\"", new[] { onNameError("hello\"", "\"") } },
                { "\"hello", new[] { onNameError("\"hello", "\"") } },
                { "he\"lo", new[] { onNameError("he\"lo", "\"") } },
                { "\"he\"lo\"", new[] { onNameError("\"he\"lo\"", "\"") } },
                { "'", new[] { onNameError("'", "'") } },
                { "hello'", new[] { onNameError("hello'", "'") } },
                { "'hello", new[] { onNameError("'hello", "'") } },
                { "he'lo", new[] { onNameError("he'lo", "'") } },
                { "'he'lo'", new[] { onNameError("'he'lo'", "'") } },
                { "hello*", new[] { onNameError("hello*", "*") } },
                { "*hello", new[] { onNameError("*hello", "*") } },
                { "he*lo", new[] { onNameError("he*lo", "*") } },
                { "*he*lo*", new[] { onNameError("*he*lo*", "*") } },
                { Environment.NewLine, new[] { whitespaceErrorString } },
                { "\t", new[] { whitespaceErrorString } },
                { " \t ", new[] { whitespaceErrorString } },
                { " ", new[] { whitespaceErrorString } },
                { Environment.NewLine + " ", new[] { whitespaceErrorString } },
                {
                    "! \t\r\n@/<>?[]=\"'*",
                    new[]
                    {
                        onNameError("! \t\r\n@/<>?[]=\"'*", "!"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", " "),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "\t"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "\r"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "\n"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "@"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "/"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "<"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", ">"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "?"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "["),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "]"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "="),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "\""),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "'"),
                        onNameError("! \t\r\n@/<>?[]=\"'*", "*"),
                    }
                },
                {
                    "! \tv\ra\nl@i/d<>?[]=\"'*",
                    new[]
                    {
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "!"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", " "),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "\t"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "\r"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "\n"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "@"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "/"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "<"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", ">"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "?"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "["),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "]"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "="),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "\""),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "'"),
                        onNameError("! \tv\ra\nl@i/d<>?[]=\"'*", "*"),
                    }
                },
            };

        if (onDataError != null)
        {
            data.Add("data-", new[] { onDataError("data-") });
            data.Add("data-something", new[] { onDataError("data-something") });
            data.Add("Data-Something", new[] { onDataError("Data-Something") });
            data.Add("DATA-SOMETHING", new[] { onDataError("DATA-SOMETHING") });
        }

        return data;
    }

    protected static TagHelperDescriptor CreateTagHelperDescriptor(
        string tagName,
        string typeName,
        string assemblyName,
        string typeNamespace,
        string typeNameIdentifier,
        IEnumerable<Action<BoundAttributeDescriptorBuilder>> attributes = null,
        IEnumerable<Action<TagMatchingRuleDescriptorBuilder>> ruleBuilders = null)
    {
        var builder = TagHelperDescriptorBuilder.Create(typeName, assemblyName);

        builder.SetMetadata(
            RuntimeName(TagHelperConventions.DefaultKind),
            TypeName(typeName),
            TypeNamespace(typeNamespace),
            TypeNameIdentifier(typeNameIdentifier));

        if (attributes != null)
        {
            foreach (var attributeBuilder in attributes)
            {
                builder.BoundAttributeDescriptor(attributeBuilder);
            }
        }

        if (ruleBuilders != null)
        {
            foreach (var ruleBuilder in ruleBuilders)
            {
                builder.TagMatchingRuleDescriptor(innerRuleBuilder =>
                {
                    innerRuleBuilder.RequireTagName(tagName);
                    ruleBuilder(innerRuleBuilder);
                });
            }
        }
        else
        {
            builder.TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName(tagName));
        }

        var descriptor = builder.Build();

        return descriptor;
    }

    private static BoundAttributeDescriptor CreateAttributeFor(string tagHelperTypeFullName, Action<BoundAttributeDescriptorBuilder> configure)
    {
        var tagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, tagHelperTypeFullName.Split('.')[^1], "Test");
        tagHelperBuilder.Metadata(TypeName(tagHelperTypeFullName));

        var attributeBuilder = new BoundAttributeDescriptorBuilder(tagHelperBuilder);
        configure(attributeBuilder);
        return attributeBuilder.Build();
    }

    private const string AdditionalCode =
        """
        namespace TestNamespace2
        {
            [Microsoft.AspNetCore.Razor.TagHelpers.OutputElementHint("hinted-value")]
            public class OutputElementHintTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
            {
            }

            public class InheritedOutputElementHintTagHelper : OutputElementHintTagHelper
            {
            }

            [Microsoft.AspNetCore.Razor.TagHelpers.OutputElementHint("overridden")]
            public class OverriddenOutputElementHintTagHelper : OutputElementHintTagHelper
            {
            }
        }
        """;
}

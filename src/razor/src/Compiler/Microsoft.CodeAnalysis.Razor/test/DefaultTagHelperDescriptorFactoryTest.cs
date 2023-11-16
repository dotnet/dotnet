// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.Workspaces.Test;
using Xunit;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

public class DefaultTagHelperDescriptorFactoryTest
{
    private static readonly Assembly _assembly = typeof(DefaultTagHelperDescriptorFactoryTest).GetTypeInfo().Assembly;

    protected static readonly AssemblyName TagHelperDescriptorFactoryTestAssembly = _assembly.GetName();

    protected static readonly string AssemblyName = TagHelperDescriptorFactoryTestAssembly.Name;

    private static Compilation Compilation { get; } = TestCompilation.Create(_assembly);

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
                                .ValueComparisonMode(RequiredAttributeDescriptor.ValueComparisonMode.FullMatch)
                                .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_InvalidRequiredAttributeMismatchedQuotes('\'', "[name='unended]")),
                        }
                    },
                    {
                        "[name='unended",
                        new Action<RequiredAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("name")
                                .ValueComparisonMode(RequiredAttributeDescriptor.ValueComparisonMode.FullMatch)
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
                                .Value("value")
                                .ValueComparisonMode(RequiredAttributeDescriptor.ValueComparisonMode.FullMatch)
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
                                .Value("value")
                                .ValueComparisonMode(RequiredAttributeDescriptor.ValueComparisonMode.FullMatch)
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
            Func<string, RequiredAttributeDescriptor.NameComparisonMode, Action<RequiredAttributeDescriptorBuilder>> plain =
                (name, nameComparison) => (builder) => builder
                    .Name(name)
                    .NameComparisonMode(nameComparison);

            Func<string, string, RequiredAttributeDescriptor.ValueComparisonMode, Action<RequiredAttributeDescriptorBuilder>> css =
                (name, value, valueComparison) => (builder) => builder
                    .Name(name)
                    .Value(value)
                    .ValueComparisonMode(valueComparison);

            return new TheoryData<string, IEnumerable<Action<RequiredAttributeDescriptorBuilder>>>
                {
                    { null, Enumerable.Empty<Action<RequiredAttributeDescriptorBuilder>>() },
                    { string.Empty, Enumerable.Empty<Action<RequiredAttributeDescriptorBuilder>>() },
                    { "name", new[] { plain("name", RequiredAttributeDescriptor.NameComparisonMode.FullMatch) } },
                    { "name-*", new[] { plain("name-", RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch) } },
                    { "  name-*   ", new[] { plain("name-", RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch) } },
                    {
                        "asp-route-*,valid  ,  name-*   ,extra",
                        new[]
                        {
                            plain("asp-route-", RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch),
                            plain("valid", RequiredAttributeDescriptor.NameComparisonMode.FullMatch),
                            plain("name-", RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch),
                            plain("extra", RequiredAttributeDescriptor.NameComparisonMode.FullMatch),
                        }
                    },
                    { "[name]", new[] { css("name", null, RequiredAttributeDescriptor.ValueComparisonMode.None) } },
                    { "[ name ]", new[] { css("name", null, RequiredAttributeDescriptor.ValueComparisonMode.None) } },
                    { " [ name ] ", new[] { css("name", null, RequiredAttributeDescriptor.ValueComparisonMode.None) } },
                    { "[name=]", new[] { css("name", "", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch) } },
                    { "[name='']", new[] { css("name", "", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch) } },
                    { "[name ^=]", new[] { css("name", "", RequiredAttributeDescriptor.ValueComparisonMode.PrefixMatch) } },
                    { "[name=hello]", new[] { css("name", "hello", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch) } },
                    { "[name= hello]", new[] { css("name", "hello", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch) } },
                    { "[name='hello']", new[] { css("name", "hello", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch) } },
                    { "[name=\"hello\"]", new[] { css("name", "hello", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch) } },
                    { " [ name  $= \" hello\" ]  ", new[] { css("name", " hello", RequiredAttributeDescriptor.ValueComparisonMode.SuffixMatch) } },
                    {
                        "[name=\"hello\"],[other^=something ], [val = 'cool']",
                        new[]
                        {
                            css("name", "hello", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch),
                            css("other", "something", RequiredAttributeDescriptor.ValueComparisonMode.PrefixMatch),
                            css("val", "cool", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch) }
                    },
                    {
                        "asp-route-*,[name=\"hello\"],valid  ,[other^=something ],   name-*   ,[val = 'cool'],extra",
                        new[]
                        {
                            plain("asp-route-", RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch),
                            css("name", "hello", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch),
                            plain("valid", RequiredAttributeDescriptor.NameComparisonMode.FullMatch),
                            css("other", "something", RequiredAttributeDescriptor.ValueComparisonMode.PrefixMatch),
                            plain("name-", RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch),
                            css("val", "cool", RequiredAttributeDescriptor.ValueComparisonMode.FullMatch),
                            plain("extra", RequiredAttributeDescriptor.NameComparisonMode.FullMatch),
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
            return new TheoryData<Type, TagHelperDescriptor>
            {
                {
                    typeof(EnumTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(EnumTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<EnumTagHelper>())
                        .TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName("enum"))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("non-enum-property")
                                .Metadata(PropertyName(nameof(EnumTagHelper.NonEnumProperty)))
                                .TypeName(typeof(int).FullName))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("enum-property")
                                .Metadata(PropertyName(nameof(EnumTagHelper.EnumProperty)))
                                .TypeName(typeof(CustomEnum).FullName)
                                .AsEnum())
                        .Build()
                },
                {
                    typeof(MultiEnumTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(MultiEnumTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<MultiEnumTagHelper>())
                        .TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName("p"))
                        .TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName("input"))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("non-enum-property")
                                .Metadata(PropertyName(nameof(MultiEnumTagHelper.NonEnumProperty)))
                                .TypeName(typeof(int).FullName))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("enum-property")
                                .Metadata(PropertyName(nameof(MultiEnumTagHelper.EnumProperty)))
                                .TypeName(typeof(CustomEnum).FullName)
                                .AsEnum())
                        .Build()
                },
                {
                    typeof(NestedEnumTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(NestedEnumTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<NestedEnumTagHelper>())
                        .TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName("nested-enum"))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("nested-enum-property")
                                .Metadata(PropertyName(nameof(NestedEnumTagHelper.NestedEnumProperty)))
                                .TypeName($"{typeof(NestedEnumTagHelper).FullName}.{nameof(NestedEnumTagHelper.NestedEnum)}")
                                .AsEnum())
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("non-enum-property")
                                .Metadata(PropertyName(nameof(NestedEnumTagHelper.NonEnumProperty)))
                                .TypeName(typeof(int).FullName))
                        .BoundAttributeDescriptor(builder =>
                            builder
                                .Name("enum-property")
                                .Metadata(PropertyName(nameof(NestedEnumTagHelper.EnumProperty)))
                                .TypeName(typeof(CustomEnum).FullName)
                                .AsEnum())
                        .Build()
                },
            };
        }
    }

    [Theory]
    [MemberData(nameof(IsEnumData))]
    public void CreateDescriptor_IsEnumIsSetCorrectly(
        Type tagHelperType,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperType.FullName);

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
            return new TheoryData<Type, TagHelperDescriptor>
            {
                {
                    typeof(RequiredParentTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(RequiredParentTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<RequiredParentTagHelper>())
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("input").RequireParentTag("div"))
                        .Build()
                },
                {
                    typeof(MultiSpecifiedRequiredParentTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(MultiSpecifiedRequiredParentTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<MultiSpecifiedRequiredParentTagHelper>())
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("p").RequireParentTag("div"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("input").RequireParentTag("section"))
                        .Build()
                },
                {
                    typeof(MultiWithUnspecifiedRequiredParentTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(MultiWithUnspecifiedRequiredParentTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<MultiWithUnspecifiedRequiredParentTagHelper>())
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
        Type tagHelperType,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperType.FullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    private static KeyValuePair<string, string>[] GetMetadata<T>()
    {
        var type = typeof(T);
        var name = type.Name;
        var fullName = type.FullName;

        return new[]
        {
            TypeName(fullName),
            TypeNamespace(fullName[..(fullName.Length - name.Length - 1)]),
            TypeNameIdentifier(name)
        };
    }

    public static TheoryData RestrictChildrenData
    {
        get
        {
            // tagHelperType, expectedDescriptor
            return new TheoryData<Type, TagHelperDescriptor>
            {
                {
                    typeof(RestrictChildrenTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(RestrictChildrenTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<RestrictChildrenTagHelper>())
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("restrict-children"))
                        .AllowChildTag("p")
                        .Build()
                },
                {
                    typeof(DoubleRestrictChildrenTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(DoubleRestrictChildrenTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<DoubleRestrictChildrenTagHelper>())
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("double-restrict-children"))
                        .AllowChildTag("p")
                        .AllowChildTag("strong")
                        .Build()
                },
                {
                    typeof(MultiTargetRestrictChildrenTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(MultiTargetRestrictChildrenTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<MultiTargetRestrictChildrenTagHelper>())
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
        Type tagHelperType,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperType.FullName);

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
            return new TheoryData<Type, TagHelperDescriptor>
            {
                {
                    typeof(TagStructureTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(TagStructureTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<TagStructureTagHelper>())
                        .TagMatchingRuleDescriptor(builder => builder
                            .RequireTagName("input")
                            .RequireTagStructure(TagStructure.WithoutEndTag))
                        .Build()
                },
                {
                    typeof(MultiSpecifiedTagStructureTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(MultiSpecifiedTagStructureTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<MultiSpecifiedTagStructureTagHelper>())
                        .TagMatchingRuleDescriptor(builder => builder
                            .RequireTagName("p")
                            .RequireTagStructure(TagStructure.NormalOrSelfClosing))
                        .TagMatchingRuleDescriptor(builder => builder
                            .RequireTagName("input")
                            .RequireTagStructure(TagStructure.WithoutEndTag))
                        .Build()
                },
                {
                    typeof(MultiWithUnspecifiedTagStructureTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(MultiWithUnspecifiedTagStructureTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<MultiWithUnspecifiedTagStructureTagHelper>())
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
        Type tagHelperType,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperType.FullName);

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
            return new TheoryData<Type, bool, TagHelperDescriptor>
                {
                    {
                        typeof(InheritedEditorBrowsableTagHelper),
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "inherited-editor-browsable",
                            typeName: typeof(InheritedEditorBrowsableTagHelper).FullName,
                            assemblyName: AssemblyName,
                            typeNamespace: typeof(InheritedEditorBrowsableTagHelper).FullName.Substring(0, typeof(InheritedEditorBrowsableTagHelper).FullName.Length - nameof(InheritedEditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(InheritedEditorBrowsableTagHelper),
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName(nameof(InheritedEditorBrowsableTagHelper.Property)))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    { typeof(EditorBrowsableTagHelper), true, null },
                    {
                        typeof(EditorBrowsableTagHelper),
                        false,
                        CreateTagHelperDescriptor(
                            tagName: "editor-browsable",
                            typeName: typeof(EditorBrowsableTagHelper).FullName,
                            assemblyName: AssemblyName,
                            typeNamespace: typeof(EditorBrowsableTagHelper).FullName.Substring(0, typeof(EditorBrowsableTagHelper).FullName.Length - nameof(EditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(EditorBrowsableTagHelper),
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName(nameof(EditorBrowsableTagHelper.Property)))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        typeof(HiddenPropertyEditorBrowsableTagHelper),
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "hidden-property-editor-browsable",
                            typeName: typeof(HiddenPropertyEditorBrowsableTagHelper).FullName,
                            typeNamespace: typeof(HiddenPropertyEditorBrowsableTagHelper).FullName.Substring(0, typeof(HiddenPropertyEditorBrowsableTagHelper).FullName.Length - nameof(HiddenPropertyEditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(HiddenPropertyEditorBrowsableTagHelper),
                            assemblyName: AssemblyName)
                    },
                    {
                        typeof(HiddenPropertyEditorBrowsableTagHelper),
                        false,
                        CreateTagHelperDescriptor(
                            tagName: "hidden-property-editor-browsable",
                            typeName: typeof(HiddenPropertyEditorBrowsableTagHelper).FullName,
                            typeNamespace: typeof(HiddenPropertyEditorBrowsableTagHelper).FullName.Substring(0, typeof(HiddenPropertyEditorBrowsableTagHelper).FullName.Length - nameof(HiddenPropertyEditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(HiddenPropertyEditorBrowsableTagHelper),
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName(nameof(HiddenPropertyEditorBrowsableTagHelper.Property)))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        typeof(OverriddenEditorBrowsableTagHelper),
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "overridden-editor-browsable",
                            typeName: typeof(OverriddenEditorBrowsableTagHelper).FullName,
                            typeNamespace: typeof(OverriddenEditorBrowsableTagHelper).FullName.Substring(0, typeof(OverriddenEditorBrowsableTagHelper).FullName.Length - nameof(OverriddenEditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(OverriddenEditorBrowsableTagHelper),
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName(nameof(OverriddenEditorBrowsableTagHelper.Property)))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        typeof(MultiPropertyEditorBrowsableTagHelper),
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "multi-property-editor-browsable",
                            typeName: typeof(MultiPropertyEditorBrowsableTagHelper).FullName,
                            typeNamespace: typeof(MultiPropertyEditorBrowsableTagHelper).FullName.Substring(0, typeof(MultiPropertyEditorBrowsableTagHelper).FullName.Length - nameof(MultiPropertyEditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(MultiPropertyEditorBrowsableTagHelper),
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property2")
                                    .Metadata(PropertyName(nameof(MultiPropertyEditorBrowsableTagHelper.Property2)))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        typeof(MultiPropertyEditorBrowsableTagHelper),
                        false,
                        CreateTagHelperDescriptor(
                            tagName: "multi-property-editor-browsable",
                            typeName: typeof(MultiPropertyEditorBrowsableTagHelper).FullName,
                            typeNamespace: typeof(MultiPropertyEditorBrowsableTagHelper).FullName.Substring(0, typeof(MultiPropertyEditorBrowsableTagHelper).FullName.Length - nameof(MultiPropertyEditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(MultiPropertyEditorBrowsableTagHelper),
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName(nameof(MultiPropertyEditorBrowsableTagHelper.Property)))
                                    .TypeName(typeof(int).FullName),
                                builder => builder
                                    .Name("property2")
                                    .Metadata(PropertyName(nameof(MultiPropertyEditorBrowsableTagHelper.Property2)))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        typeof(OverriddenPropertyEditorBrowsableTagHelper),
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "overridden-property-editor-browsable",
                            typeName: typeof(OverriddenPropertyEditorBrowsableTagHelper).FullName,
                            typeNamespace: typeof(OverriddenPropertyEditorBrowsableTagHelper).FullName.Substring(0, typeof(OverriddenPropertyEditorBrowsableTagHelper).FullName.Length - nameof(OverriddenPropertyEditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(OverriddenPropertyEditorBrowsableTagHelper),
                            assemblyName: AssemblyName)
                    },
                    {
                        typeof(OverriddenPropertyEditorBrowsableTagHelper),
                        false,
                        CreateTagHelperDescriptor(
                            tagName: "overridden-property-editor-browsable",
                            typeName: typeof(OverriddenPropertyEditorBrowsableTagHelper).FullName,
                            typeNamespace: typeof(OverriddenPropertyEditorBrowsableTagHelper).FullName.Substring(0, typeof(OverriddenPropertyEditorBrowsableTagHelper).FullName.Length - nameof(OverriddenPropertyEditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(OverriddenPropertyEditorBrowsableTagHelper),
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property2")
                                    .Metadata(PropertyName(nameof(OverriddenPropertyEditorBrowsableTagHelper.Property2)))
                                    .TypeName(typeof(int).FullName),
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName(nameof(OverriddenPropertyEditorBrowsableTagHelper.Property)))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    {
                        typeof(DefaultEditorBrowsableTagHelper),
                        true,
                        CreateTagHelperDescriptor(
                            tagName: "default-editor-browsable",
                            typeName: typeof(DefaultEditorBrowsableTagHelper).FullName,
                            typeNamespace: typeof(DefaultEditorBrowsableTagHelper).FullName.Substring(0, typeof(DefaultEditorBrowsableTagHelper).FullName.Length - nameof(DefaultEditorBrowsableTagHelper).Length -1),
                            typeNameIdentifier: nameof(DefaultEditorBrowsableTagHelper),
                            assemblyName: AssemblyName,
                            attributes: new Action<BoundAttributeDescriptorBuilder>[]
                            {
                                builder => builder
                                    .Name("property")
                                    .Metadata(PropertyName(nameof(DefaultEditorBrowsableTagHelper.Property)))
                                    .TypeName(typeof(int).FullName),
                            })
                    },
                    { typeof(MultiEditorBrowsableTagHelper), true, null }
                };
        }
    }

    [Theory]
    [MemberData(nameof(EditorBrowsableData))]
    public void CreateDescriptor_UnderstandsEditorBrowsableAttribute(
        Type tagHelperType,
        bool designTime,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, designTime, designTime);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperType.FullName);

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
            return new TheoryData<Type, TagHelperDescriptor>
                {
                    {
                        typeof(AttributeTargetingTagHelper),
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            typeof(AttributeTargetingTagHelper).FullName,
                            AssemblyName,
                            typeof(AttributeTargetingTagHelper).FullName.Substring(0, typeof(AttributeTargetingTagHelper).FullName.Length - nameof(AttributeTargetingTagHelper).Length -1),
                            nameof(AttributeTargetingTagHelper),
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("class")),
                            })
                    },
                    {
                        typeof(MultiAttributeTargetingTagHelper),
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            typeof(MultiAttributeTargetingTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(MultiAttributeTargetingTagHelper).FullName.Substring(0, typeof(MultiAttributeTargetingTagHelper).FullName.Length - nameof(MultiAttributeTargetingTagHelper).Length -1),
                            typeNameIdentifier: nameof(MultiAttributeTargetingTagHelper),
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
                        typeof(MultiAttributeAttributeTargetingTagHelper),
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            typeof(MultiAttributeAttributeTargetingTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(MultiAttributeAttributeTargetingTagHelper).FullName.Substring(0, typeof(MultiAttributeAttributeTargetingTagHelper).FullName.Length - nameof(MultiAttributeAttributeTargetingTagHelper).Length -1),
                            typeNameIdentifier: nameof(MultiAttributeAttributeTargetingTagHelper),
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
                        typeof(InheritedAttributeTargetingTagHelper),
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            typeof(InheritedAttributeTargetingTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(InheritedAttributeTargetingTagHelper).FullName.Substring(0, typeof(InheritedAttributeTargetingTagHelper).FullName.Length - nameof(InheritedAttributeTargetingTagHelper).Length -1),
                            typeNameIdentifier: nameof(InheritedAttributeTargetingTagHelper),
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("style")),
                            })
                    },
                    {
                        typeof(RequiredAttributeTagHelper),
                        CreateTagHelperDescriptor(
                            "input",
                            typeof(RequiredAttributeTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(RequiredAttributeTagHelper).FullName.Substring(0, typeof(RequiredAttributeTagHelper).FullName.Length - nameof(RequiredAttributeTagHelper).Length -1),
                            typeNameIdentifier: nameof(RequiredAttributeTagHelper),
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("class")),
                            })
                    },
                    {
                        typeof(InheritedRequiredAttributeTagHelper),
                        CreateTagHelperDescriptor(
                            "div",
                            typeof(InheritedRequiredAttributeTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(InheritedRequiredAttributeTagHelper).FullName.Substring(0, typeof(InheritedRequiredAttributeTagHelper).FullName.Length - nameof(InheritedRequiredAttributeTagHelper).Length -1),
                            typeNameIdentifier: nameof(InheritedRequiredAttributeTagHelper),
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("class")),
                            })
                    },
                    {
                        typeof(MultiAttributeRequiredAttributeTagHelper),
                        CreateTagHelperDescriptor(
                            "div",
                            typeof(MultiAttributeRequiredAttributeTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(MultiAttributeRequiredAttributeTagHelper).FullName.Substring(0, typeof(MultiAttributeRequiredAttributeTagHelper).FullName.Length - nameof(MultiAttributeRequiredAttributeTagHelper).Length -1),
                            typeNameIdentifier: nameof(MultiAttributeRequiredAttributeTagHelper),
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
                        typeof(MultiAttributeSameTagRequiredAttributeTagHelper),
                        CreateTagHelperDescriptor(
                            "input",
                            typeof(MultiAttributeSameTagRequiredAttributeTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(MultiAttributeSameTagRequiredAttributeTagHelper).FullName.Substring(0, typeof(MultiAttributeSameTagRequiredAttributeTagHelper).FullName.Length - nameof(MultiAttributeSameTagRequiredAttributeTagHelper).Length -1),
                            typeNameIdentifier: nameof(MultiAttributeSameTagRequiredAttributeTagHelper),
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("style")),
                                builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("class")),
                            })
                    },
                    {
                        typeof(MultiRequiredAttributeTagHelper),
                        CreateTagHelperDescriptor(
                            "input",
                            typeof(MultiRequiredAttributeTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(MultiRequiredAttributeTagHelper).FullName.Substring(0, typeof(MultiRequiredAttributeTagHelper).FullName.Length - nameof(MultiRequiredAttributeTagHelper).Length -1),
                            typeNameIdentifier: nameof(MultiRequiredAttributeTagHelper),
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder
                                    .RequireAttributeDescriptor(attribute => attribute.Name("class"))
                                    .RequireAttributeDescriptor(attribute => attribute.Name("style")),
                            })
                    },
                    {
                        typeof(MultiTagMultiRequiredAttributeTagHelper),
                        CreateTagHelperDescriptor(
                            "div",
                            typeof(MultiTagMultiRequiredAttributeTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(MultiTagMultiRequiredAttributeTagHelper).FullName.Substring(0, typeof(MultiTagMultiRequiredAttributeTagHelper).FullName.Length - nameof(MultiTagMultiRequiredAttributeTagHelper).Length -1),
                            typeNameIdentifier: nameof(MultiTagMultiRequiredAttributeTagHelper),
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
                        typeof(AttributeWildcardTargetingTagHelper),
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            typeof(AttributeWildcardTargetingTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(AttributeWildcardTargetingTagHelper).FullName.Substring(0, typeof(AttributeWildcardTargetingTagHelper).FullName.Length - nameof(AttributeWildcardTargetingTagHelper).Length -1),
                            typeNameIdentifier: nameof(AttributeWildcardTargetingTagHelper),
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder
                                    .RequireAttributeDescriptor(attribute => attribute
                                        .Name("class")
                                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch)),
                            })
                    },
                    {
                        typeof(MultiAttributeWildcardTargetingTagHelper),
                        CreateTagHelperDescriptor(
                            TagHelperMatchingConventions.ElementCatchAllName,
                            typeof(MultiAttributeWildcardTargetingTagHelper).FullName,
                            AssemblyName,
                            typeNamespace: typeof(MultiAttributeWildcardTargetingTagHelper).FullName.Substring(0, typeof(MultiAttributeWildcardTargetingTagHelper).FullName.Length - nameof(MultiAttributeWildcardTargetingTagHelper).Length -1),
                            typeNameIdentifier: nameof(MultiAttributeWildcardTargetingTagHelper),
                            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                            {
                                builder => builder
                                    .RequireAttributeDescriptor(attribute => attribute
                                        .Name("class")
                                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch))
                                    .RequireAttributeDescriptor(attribute => attribute
                                        .Name("style")
                                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch)),
                            })
                    },
                };
        }
    }

    [Theory]
    [MemberData(nameof(AttributeTargetData))]
    public void CreateDescriptor_ReturnsExpectedDescriptors(
        Type tagHelperType,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperType.FullName);

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
            return new TheoryData<Type, string, string>
            {
                { typeof(SingleAttributeTagHelper), "single-attribute", "int-attribute" },
                { typeof(ALLCAPSTAGHELPER), "allcaps", "allcapsattribute" },
                { typeof(CAPSOnOUTSIDETagHelper), "caps-on-outside", "caps-on-outsideattribute" },
                { typeof(capsONInsideTagHelper), "caps-on-inside", "caps-on-insideattribute" },
                { typeof(One1Two2Three3TagHelper), "one1-two2-three3", "one1-two2-three3-attribute" },
                { typeof(ONE1TWO2THREE3TagHelper), "one1two2three3", "one1two2three3-attribute" },
                { typeof(First_Second_ThirdHiTagHelper), "first_second_third-hi", "first_second_third-attribute" },
                { typeof(UNSuffixedCLASS), "un-suffixed-class", "un-suffixed-attribute" },
            };
        }
    }

    [Theory]
    [MemberData(nameof(HtmlCaseData))]
    public void CreateDescriptor_HtmlCasesTagNameAndAttributeName(
        Type tagHelperType,
        string expectedTagName,
        string expectedAttributeName)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperType.FullName);

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
        var validProperty1 = typeof(OverriddenAttributeTagHelper).GetProperty(
            nameof(OverriddenAttributeTagHelper.ValidAttribute1));
        var validProperty2 = typeof(OverriddenAttributeTagHelper).GetProperty(
            nameof(OverriddenAttributeTagHelper.ValidAttribute2));
        var expectedDescriptor =
            CreateTagHelperDescriptor(
                "overridden-attribute",
                typeof(OverriddenAttributeTagHelper).FullName,
                AssemblyName,
                typeNamespace: typeof(OverriddenAttributeTagHelper).FullName.Substring(0, typeof(OverriddenAttributeTagHelper).FullName.Length - nameof(OverriddenAttributeTagHelper).Length -1),
                typeNameIdentifier: nameof(OverriddenAttributeTagHelper),
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("SomethingElse")
                        .Metadata(PropertyName(validProperty1.Name))
                        .TypeName(validProperty1.PropertyType.FullName),
                    builder => builder
                        .Name("Something-Else")
                        .Metadata(PropertyName(validProperty2.Name))
                        .TypeName(validProperty2.PropertyType.FullName),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(OverriddenAttributeTagHelper).FullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_DoesNotInheritOverridenAttributeName()
    {
        // Arrange
        var validProperty1 = typeof(InheritedOverriddenAttributeTagHelper).GetProperty(
            nameof(InheritedOverriddenAttributeTagHelper.ValidAttribute1));
        var validProperty2 = typeof(InheritedOverriddenAttributeTagHelper).GetProperty(
            nameof(InheritedOverriddenAttributeTagHelper.ValidAttribute2));
        var expectedDescriptor =
            CreateTagHelperDescriptor(
                "inherited-overridden-attribute",
                typeof(InheritedOverriddenAttributeTagHelper).FullName,
                AssemblyName,
                typeNamespace: typeof(InheritedOverriddenAttributeTagHelper).FullName.Substring(0, typeof(InheritedOverriddenAttributeTagHelper).FullName.Length - nameof(InheritedOverriddenAttributeTagHelper).Length -1),
                typeNameIdentifier: nameof(InheritedOverriddenAttributeTagHelper),
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("valid-attribute1")
                        .Metadata(PropertyName(validProperty1.Name))
                        .TypeName(validProperty1.PropertyType.FullName),
                    builder => builder
                        .Name("Something-Else")
                        .Metadata(PropertyName(validProperty2.Name))
                        .TypeName(validProperty2.PropertyType.FullName),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(InheritedOverriddenAttributeTagHelper).FullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_AllowsOverriddenAttributeNameOnUnimplementedVirtual()
    {
        // Arrange
        var validProperty1 = typeof(InheritedNotOverriddenAttributeTagHelper).GetProperty(
            nameof(InheritedNotOverriddenAttributeTagHelper.ValidAttribute1));
        var validProperty2 = typeof(InheritedNotOverriddenAttributeTagHelper).GetProperty(
            nameof(InheritedNotOverriddenAttributeTagHelper.ValidAttribute2));

        var expectedDescriptor = CreateTagHelperDescriptor(
                "inherited-not-overridden-attribute",
                typeof(InheritedNotOverriddenAttributeTagHelper).FullName,
                AssemblyName,
                typeNamespace: typeof(InheritedNotOverriddenAttributeTagHelper).FullName.Substring(0, typeof(InheritedNotOverriddenAttributeTagHelper).FullName.Length - nameof(InheritedNotOverriddenAttributeTagHelper).Length -1),
                typeNameIdentifier: nameof(InheritedNotOverriddenAttributeTagHelper),
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("SomethingElse")
                        .Metadata(PropertyName(validProperty1.Name))
                        .TypeName(validProperty1.PropertyType.FullName),
                    builder => builder
                        .Name("Something-Else")
                        .Metadata(PropertyName(validProperty2.Name))
                        .TypeName(validProperty2.PropertyType.FullName),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(InheritedNotOverriddenAttributeTagHelper).FullName);

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
            typeof(InheritedSingleAttributeTagHelper).FullName,
            AssemblyName,
            typeNamespace: typeof(InheritedSingleAttributeTagHelper).FullName.Substring(0, typeof(InheritedSingleAttributeTagHelper).FullName.Length - nameof(InheritedSingleAttributeTagHelper).Length -1),
            typeNameIdentifier: nameof(InheritedSingleAttributeTagHelper),
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("int-attribute")
                    .Metadata(PropertyName(nameof(InheritedSingleAttributeTagHelper.IntAttribute)))
                    .TypeName(typeof(int).FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(InheritedSingleAttributeTagHelper).FullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_BuildsDescriptorsWithConventionNames()
    {
        // Arrange
        var intProperty = typeof(SingleAttributeTagHelper).GetProperty(nameof(SingleAttributeTagHelper.IntAttribute));
        var expectedDescriptor = CreateTagHelperDescriptor(
            "single-attribute",
            typeof(SingleAttributeTagHelper).FullName,
            AssemblyName,
            typeNamespace: typeof(SingleAttributeTagHelper).FullName.Substring(0, typeof(SingleAttributeTagHelper).FullName.Length - nameof(SingleAttributeTagHelper).Length -1),
            typeNameIdentifier: nameof(SingleAttributeTagHelper),
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("int-attribute")
                    .Metadata(PropertyName(intProperty.Name))
                    .TypeName(intProperty.PropertyType.FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(SingleAttributeTagHelper).FullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_OnlyAcceptsPropertiesWithGetAndSet()
    {
        // Arrange
        var validProperty = typeof(MissingAccessorTagHelper).GetProperty(
            nameof(MissingAccessorTagHelper.ValidAttribute));
        var expectedDescriptor = CreateTagHelperDescriptor(
            "missing-accessor",
            typeof(MissingAccessorTagHelper).FullName,
            AssemblyName,
            typeNamespace: typeof(MissingAccessorTagHelper).FullName.Substring(0, typeof(MissingAccessorTagHelper).FullName.Length - nameof(MissingAccessorTagHelper).Length -1),
            typeNameIdentifier: nameof(MissingAccessorTagHelper),
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("valid-attribute")
                    .Metadata(PropertyName(validProperty.Name))
                    .TypeName(validProperty.PropertyType.FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(MissingAccessorTagHelper).FullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_OnlyAcceptsPropertiesWithPublicGetAndSet()
    {
        // Arrange
        var validProperty = typeof(NonPublicAccessorTagHelper).GetProperty(
            nameof(NonPublicAccessorTagHelper.ValidAttribute));
        var expectedDescriptor = CreateTagHelperDescriptor(
            "non-public-accessor",
            typeof(NonPublicAccessorTagHelper).FullName,
            AssemblyName,
            typeNamespace: typeof(NonPublicAccessorTagHelper).FullName.Substring(0, typeof(NonPublicAccessorTagHelper).FullName.Length - nameof(NonPublicAccessorTagHelper).Length -1),
            typeNameIdentifier: nameof(NonPublicAccessorTagHelper),
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("valid-attribute")
                    .Metadata(PropertyName(validProperty.Name))
                    .TypeName(validProperty.PropertyType.FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(NonPublicAccessorTagHelper).FullName);

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
            typeof(NotBoundAttributeTagHelper).FullName,
            AssemblyName,
            typeNamespace: typeof(NotBoundAttributeTagHelper).FullName.Substring(0, typeof(NotBoundAttributeTagHelper).FullName.Length - nameof(NotBoundAttributeTagHelper).Length -1),
            typeNameIdentifier: nameof(NotBoundAttributeTagHelper),
            new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("bound-property")
                    .Metadata(PropertyName(nameof(NotBoundAttributeTagHelper.BoundProperty)))
                    .TypeName(typeof(object).FullName)
            });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(NotBoundAttributeTagHelper).FullName);

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
                typeof(MultiTagTagHelper).FullName,
                AssemblyName,
                typeNamespace: typeof(MultiTagTagHelper).FullName.Substring(0, typeof(MultiTagTagHelper).FullName.Length - nameof(MultiTagTagHelper).Length -1),
                typeNameIdentifier: nameof(MultiTagTagHelper),
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("valid-attribute")
                        .Metadata(PropertyName(nameof(MultiTagTagHelper.ValidAttribute)))
                        .TypeName(typeof(string).FullName),
                },
                new Action<TagMatchingRuleDescriptorBuilder>[]
                {
                    builder => builder.RequireTagName("p"),
                    builder => builder.RequireTagName("div"),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(MultiTagTagHelper).FullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_DoesNotResolveInheritedTagNames()
    {
        // Arrange
        var validProp = typeof(InheritedMultiTagTagHelper).GetProperty(nameof(InheritedMultiTagTagHelper.ValidAttribute));
        var expectedDescriptor = CreateTagHelperDescriptor(
                "inherited-multi-tag",
                typeof(InheritedMultiTagTagHelper).FullName,
                AssemblyName,
                typeNamespace: typeof(InheritedMultiTagTagHelper).FullName.Substring(0, typeof(InheritedMultiTagTagHelper).FullName.Length - nameof(InheritedMultiTagTagHelper).Length -1),
                typeNameIdentifier: nameof(InheritedMultiTagTagHelper),
                new Action<BoundAttributeDescriptorBuilder>[]
                {
                    builder => builder
                        .Name("valid-attribute")
                        .Metadata(PropertyName(validProp.Name))
                        .TypeName(validProp.PropertyType.FullName),
                });
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(InheritedMultiTagTagHelper).FullName);

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
            typeof(DuplicateTagNameTagHelper).FullName,
            AssemblyName,
            typeNamespace: typeof(DuplicateTagNameTagHelper).FullName.Substring(0, typeof(DuplicateTagNameTagHelper).FullName.Length - nameof(DuplicateTagNameTagHelper).Length -1),
            typeNameIdentifier: nameof(DuplicateTagNameTagHelper),
            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
            {
                    builder => builder.RequireTagName("p"),
                    builder => builder.RequireTagName("div"),
            });

        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(DuplicateTagNameTagHelper).FullName);

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
                typeof(OverrideNameTagHelper).FullName,
                AssemblyName,
                typeNamespace: typeof(OverrideNameTagHelper).FullName.Substring(0, typeof(OverrideNameTagHelper).FullName.Length - nameof(OverrideNameTagHelper).Length -1),
                typeNameIdentifier: nameof(OverrideNameTagHelper));
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(typeof(OverrideNameTagHelper).FullName);

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
            [{{typeof(AspNetCore.Razor.TagHelpers.HtmlTargetElementAttribute).FullName}}("{{name}}")]
            public class DynamicTestTagHelper : {{typeof(AspNetCore.Razor.TagHelpers.TagHelper).FullName}}
            {
            }
            """;
        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var compilation = TestCompilation.Create(_assembly, syntaxTree);
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
            var invalidBoundAttributeBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, nameof(InvalidBoundAttribute), "Test");
            invalidBoundAttributeBuilder.Metadata(TypeName(typeof(InvalidBoundAttribute).FullName));

            // type, expectedAttributeDescriptors
            return new TheoryData<Type, IEnumerable<BoundAttributeDescriptor>>
            {
                {
                    typeof(InvalidBoundAttribute),
                    new[]
                    {
                        CreateAttributeFor(typeof(InvalidBoundAttribute), attribute =>
                        {
                            attribute
                                .Name("data-something")
                                .Metadata(PropertyName(nameof(InvalidBoundAttribute.DataSomething)))
                                .TypeName(typeof(string).FullName);
                        }),
                    }
                },
                {
                    typeof(InvalidBoundAttributeWithValid),
                    new[]
                    {
                        CreateAttributeFor(typeof(InvalidBoundAttributeWithValid), attribute =>
                        {
                            attribute
                                .Name("data-something")
                                .Metadata(PropertyName(nameof(InvalidBoundAttributeWithValid.DataSomething)))
                                .TypeName(typeof(string).FullName); ;
                        }),
                        CreateAttributeFor(typeof(InvalidBoundAttributeWithValid), attribute =>
                        {
                            attribute
                            .Name("int-attribute")
                            .Metadata(PropertyName(nameof(InvalidBoundAttributeWithValid.IntAttribute)))
                            .TypeName(typeof(int).FullName);
                        }),
                    }
                },
                {
                    typeof(OverriddenInvalidBoundAttributeWithValid),
                    new[]
                    {
                        CreateAttributeFor(typeof(OverriddenInvalidBoundAttributeWithValid), attribute =>
                        {
                            attribute
                            .Name("valid-something")
                            .Metadata(PropertyName(nameof(OverriddenInvalidBoundAttributeWithValid.DataSomething)))
                            .TypeName(typeof(string).FullName);
                        }),
                    }
                },
                {
                    typeof(OverriddenValidBoundAttributeWithInvalid),
                    new[]
                    {
                        CreateAttributeFor(typeof(OverriddenValidBoundAttributeWithInvalid), attribute =>
                        {
                            attribute
                            .Name("data-something")
                            .Metadata(PropertyName(nameof(OverriddenValidBoundAttributeWithInvalid.ValidSomething)))
                            .TypeName(typeof(string).FullName);
                        }),
                    }
                },
                {
                    typeof(OverriddenValidBoundAttributeWithInvalidUpperCase),
                    new[]
                    {
                        CreateAttributeFor(typeof(OverriddenValidBoundAttributeWithInvalidUpperCase), attribute =>
                        {
                            attribute
                            .Name("DATA-SOMETHING")
                            .Metadata(PropertyName(nameof(OverriddenValidBoundAttributeWithInvalidUpperCase.ValidSomething)))
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
        Type type,
        IEnumerable<BoundAttributeDescriptor> expectedAttributeDescriptors)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(type.FullName);

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
            public class DynamicTestTagHelper : {{typeof(AspNetCore.Razor.TagHelpers.TagHelper).FullName}}
            {
                [{{typeof(AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute).FullName}}("{{name}}")]
                public string SomeAttribute { get; set; }
            }
            """;
        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var compilation = TestCompilation.Create(_assembly, syntaxTree);
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
            public class DynamicTestTagHelper : {{typeof(AspNetCore.Razor.TagHelpers.TagHelper).FullName}}
            {
                [{{typeof(AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute).FullName}}({{nameof(AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute.DictionaryAttributePrefix)}} = "{{prefix}}")]
                public System.Collections.Generic.IDictionary<string, int> SomeAttribute { get; set; }
            }
            """;
        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var compilation = TestCompilation.Create(_assembly, syntaxTree);
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
            public class DynamicTestTagHelper : {{typeof(AspNetCore.Razor.TagHelpers.TagHelper).FullName}}
            {
                [{{typeof(AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute).FullName}}("{{name}}")]
                public string InvalidProperty { get; set; }
            }
            """;
        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var compilation = TestCompilation.Create(_assembly, syntaxTree);
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
            public class DynamicTestTagHelper : {{typeof(AspNetCore.Razor.TagHelpers.TagHelper).FullName}}
            {
                [{{typeof(AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute).FullName}}({{nameof(AspNetCore.Razor.TagHelpers.HtmlAttributeNameAttribute.DictionaryAttributePrefix)}} = "{{prefix}}")]
                public System.Collections.Generic.IDictionary<System.String, System.Int32> InvalidProperty { get; set; }
            }
            """;
        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var compilation = TestCompilation.Create(_assembly, syntaxTree);
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
            [{{typeof(AspNetCore.Razor.TagHelpers.RestrictChildrenAttribute).FullName}}("{{name}}")]
            public class DynamicTestTagHelper : {{typeof(AspNetCore.Razor.TagHelpers.TagHelper).FullName}}
            {
            }
            """;
        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var compilation = TestCompilation.Create(_assembly, syntaxTree);
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
            [{{typeof(AspNetCore.Razor.TagHelpers.HtmlTargetElementAttribute).FullName}}({{nameof(AspNetCore.Razor.TagHelpers.HtmlTargetElementAttribute.ParentTag)}} = "{{name}}")]
            public class DynamicTestTagHelper : {{typeof(AspNetCore.Razor.TagHelpers.TagHelper).FullName}}
            {
            }
            """;
        var syntaxTree = CSharpSyntaxTree.ParseText(text);
        var compilation = TestCompilation.Create(_assembly, syntaxTree);
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
        var objectAssemblyName = typeof(Enumerable).GetTypeInfo().Assembly.GetName().Name;
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
            return new TheoryData<Type, IEnumerable<BoundAttributeDescriptor>, IEnumerable<RazorDiagnostic>>
                {
                    {
                        typeof(DefaultValidHtmlAttributePrefix),
                        new[]
                        {
                            CreateAttributeFor(typeof(DefaultValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("dictionary-property")
                                    .Metadata(PropertyName(nameof(DefaultValidHtmlAttributePrefix.DictionaryProperty)))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.String>")
                                    .AsDictionaryAttribute("dictionary-property-", typeof(string).FullName);
                            }),
                        },
                        Enumerable.Empty<RazorDiagnostic>()
                    },
                    {
                        typeof(SingleValidHtmlAttributePrefix),
                        new[]
                        {
                            CreateAttributeFor(typeof(SingleValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name")
                                    .Metadata(PropertyName(nameof(SingleValidHtmlAttributePrefix.DictionaryProperty)))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.String>")
                                    .AsDictionaryAttribute("valid-name-", typeof(string).FullName);
                            }),
                        },
                        Enumerable.Empty<RazorDiagnostic>()
                    },
                    {
                        typeof(MultipleValidHtmlAttributePrefix),
                        new[]
                        {
                            CreateAttributeFor(typeof(MultipleValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name1")
                                    .Metadata(PropertyName(nameof(MultipleValidHtmlAttributePrefix.DictionaryProperty)))
                                    .TypeName($"{typeof(Dictionary<,>).Namespace}.Dictionary<System.String, System.Object>")
                                    .AsDictionaryAttribute("valid-prefix1-", typeof(object).FullName);
                            }),
                            CreateAttributeFor(typeof(MultipleValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name2")
                                    .Metadata(PropertyName(nameof(MultipleValidHtmlAttributePrefix.DictionarySubclassProperty)))
                                    .TypeName(typeof(DictionarySubclass).FullName)
                                    .AsDictionaryAttribute("valid-prefix2-", typeof(string).FullName);
                            }),
                            CreateAttributeFor(typeof(MultipleValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name3")
                                    .Metadata(PropertyName(nameof(MultipleValidHtmlAttributePrefix.DictionaryWithoutParameterlessConstructorProperty)))
                                    .TypeName(typeof(DictionaryWithoutParameterlessConstructor).FullName)
                                    .AsDictionaryAttribute("valid-prefix3-", typeof(string).FullName);
                            }),
                            CreateAttributeFor(typeof(MultipleValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name4")
                                    .Metadata(PropertyName(nameof(MultipleValidHtmlAttributePrefix.GenericDictionarySubclassProperty)))
                                    .TypeName(typeof(GenericDictionarySubclass<object>).Namespace + ".GenericDictionarySubclass<System.Object>")
                                    .AsDictionaryAttribute("valid-prefix4-", typeof(object).FullName);
                            }),
                            CreateAttributeFor(typeof(MultipleValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name5")
                                    .Metadata(PropertyName(nameof(MultipleValidHtmlAttributePrefix.SortedDictionaryProperty)))
                                    .TypeName(typeof(SortedDictionary<string, int>).Namespace + ".SortedDictionary<System.String, System.Int32>")
                                    .AsDictionaryAttribute("valid-prefix5-", typeof(int).FullName);
                            }),
                            CreateAttributeFor(typeof(MultipleValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name6")
                                    .Metadata(PropertyName(nameof(MultipleValidHtmlAttributePrefix.StringProperty)))
                                    .TypeName(typeof(string).FullName);
                            }),
                            CreateAttributeFor(typeof(MultipleValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Metadata(PropertyName(nameof(MultipleValidHtmlAttributePrefix.GetOnlyDictionaryProperty)))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.Int32>")
                                    .AsDictionaryAttribute("get-only-dictionary-property-", typeof(int).FullName);
                            }),
                            CreateAttributeFor(typeof(MultipleValidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Metadata(PropertyName(nameof(MultipleValidHtmlAttributePrefix.GetOnlyDictionaryPropertyWithAttributePrefix)))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.String>")
                                    .AsDictionaryAttribute("valid-prefix6", typeof(string).FullName);
                            }),
                        },
                        Enumerable.Empty<RazorDiagnostic>()
                    },
                    {
                        typeof(SingleInvalidHtmlAttributePrefix),
                        new[]
                        {
                            CreateAttributeFor(typeof(SingleInvalidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name")
                                    .Metadata(PropertyName(nameof(SingleInvalidHtmlAttributePrefix.StringProperty)))
                                    .TypeName(typeof(string).FullName)
                                    .AddDiagnostic(RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                        typeof(SingleInvalidHtmlAttributePrefix).FullName,
                                        nameof(SingleInvalidHtmlAttributePrefix.StringProperty)));
                            }),
                        },
                        new[]
                        {
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                typeof(SingleInvalidHtmlAttributePrefix).FullName,
                                nameof(SingleInvalidHtmlAttributePrefix.StringProperty))
                        }
                    },
                    {
                        typeof(MultipleInvalidHtmlAttributePrefix),
                        new[]
                        {
                            CreateAttributeFor(typeof(MultipleInvalidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name1")
                                    .Metadata(PropertyName(nameof(MultipleInvalidHtmlAttributePrefix.LongProperty)))
                                    .TypeName(typeof(long).FullName);
                            }),
                            CreateAttributeFor(typeof(MultipleInvalidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name2")
                                    .Metadata(PropertyName(nameof(MultipleInvalidHtmlAttributePrefix.DictionaryOfIntProperty)))
                                    .TypeName($"{typeof(Dictionary<,>).Namespace}.Dictionary<System.Int32, System.String>")
                                    .AsDictionaryAttribute("valid-prefix2-", typeof(string).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                            nameof(MultipleInvalidHtmlAttributePrefix.DictionaryOfIntProperty)));
                            }),
                            CreateAttributeFor(typeof(MultipleInvalidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name3")
                                    .Metadata(PropertyName(nameof(MultipleInvalidHtmlAttributePrefix.ReadOnlyDictionaryProperty)))
                                    .TypeName($"{typeof(IReadOnlyDictionary<,>).Namespace}.IReadOnlyDictionary<System.String, System.Object>")
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                            nameof(MultipleInvalidHtmlAttributePrefix.ReadOnlyDictionaryProperty)));
                            }),
                            CreateAttributeFor(typeof(MultipleInvalidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name4")
                                    .Metadata(PropertyName(nameof(MultipleInvalidHtmlAttributePrefix.IntProperty)))
                                    .TypeName(typeof(int).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                            nameof(MultipleInvalidHtmlAttributePrefix.IntProperty)));
                            }),
                            CreateAttributeFor(typeof(MultipleInvalidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Name("valid-name5")
                                    .Metadata(PropertyName(nameof(MultipleInvalidHtmlAttributePrefix.DictionaryOfIntSubclassProperty)))
                                    .TypeName(typeof(DictionaryOfIntSubclass).FullName)
                                    .AsDictionaryAttribute("valid-prefix5-", typeof(string).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                            nameof(MultipleInvalidHtmlAttributePrefix.DictionaryOfIntSubclassProperty)));
                            }),
                            CreateAttributeFor(typeof(MultipleInvalidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Metadata(PropertyName(nameof(MultipleInvalidHtmlAttributePrefix.GetOnlyDictionaryAttributePrefix)))
                                    .TypeName($"{dictionaryNamespace}<System.Int32, System.String>")
                                    .AsDictionaryAttribute("valid-prefix6", typeof(string).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                            typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                            nameof(MultipleInvalidHtmlAttributePrefix.GetOnlyDictionaryAttributePrefix)));
                            }),
                            CreateAttributeFor(typeof(MultipleInvalidHtmlAttributePrefix), attribute =>
                            {
                                attribute
                                    .Metadata(PropertyName(nameof(MultipleInvalidHtmlAttributePrefix.GetOnlyDictionaryPropertyWithAttributeName)))
                                    .TypeName($"{dictionaryNamespace}<System.String, System.Object>")
                                    .AsDictionaryAttribute("invalid-name7-", typeof(object).FullName)
                                    .AddDiagnostic(
                                        RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNull(
                                            typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                            nameof(MultipleInvalidHtmlAttributePrefix.GetOnlyDictionaryPropertyWithAttributeName)));
                            }),
                        },
                        new[]
                        {
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.DictionaryOfIntProperty)),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.ReadOnlyDictionaryProperty)),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.IntProperty)),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.DictionaryOfIntSubclassProperty)),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.GetOnlyDictionaryAttributePrefix)),
                            RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNull(
                                typeof(MultipleInvalidHtmlAttributePrefix).FullName,
                                nameof(MultipleInvalidHtmlAttributePrefix.GetOnlyDictionaryPropertyWithAttributeName)),
                        }
                    },
                };
        }
    }

    [Theory]
    [MemberData(nameof(TagHelperWithPrefixData))]
    public void CreateDescriptor_WithPrefixes_ReturnsExpectedAttributeDescriptors(
        Type tagHelperType,
        IEnumerable<BoundAttributeDescriptor> expectedAttributeDescriptors,
        IEnumerable<RazorDiagnostic> expectedDiagnostics)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperType.FullName);

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
            return new TheoryData<Type, TagHelperDescriptor>
            {
                {
                    typeof(MultipleDescriptorTagHelperWithOutputElementHint),
                    TagHelperDescriptorBuilder.Create(typeof(MultipleDescriptorTagHelperWithOutputElementHint).FullName, AssemblyName)
                        .Metadata(GetMetadata<MultipleDescriptorTagHelperWithOutputElementHint>())
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("a"))
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("p"))
                        .TagOutputHint("div")
                        .Build()
                },
                {
                    typeof(InheritedOutputElementHintTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(InheritedOutputElementHintTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<InheritedOutputElementHintTagHelper>())
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("inherited-output-element-hint"))
                        .Build()
                },
                {
                    typeof(OutputElementHintTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(OutputElementHintTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<OutputElementHintTagHelper>())
                        .TagMatchingRuleDescriptor(builder => builder.RequireTagName("output-element-hint"))
                        .TagOutputHint("hinted-value")
                        .Build()
                },
                {
                    typeof(OverriddenOutputElementHintTagHelper),
                    TagHelperDescriptorBuilder.Create(typeof(OverriddenOutputElementHintTagHelper).FullName, AssemblyName)
                        .Metadata(GetMetadata<OverriddenOutputElementHintTagHelper>())
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
        Type tagHelperType,
        TagHelperDescriptor expectedDescriptor)
    {
        // Arrange
        var factory = new DefaultTagHelperDescriptorFactory(Compilation, includeDocumentation: false, excludeHidden: false);
        var typeSymbol = Compilation.GetTypeByMetadataName(tagHelperType.FullName);

        // Act
        var descriptor = factory.CreateDescriptor(typeSymbol);

        // Assert
        Assert.Equal(expectedDescriptor, descriptor);
    }

    [Fact]
    public void CreateDescriptor_CapturesDocumentationOnTagHelperClass()
    {
        // Arrange
        var errorSink = new ErrorSink();
        var syntaxTree = CSharpSyntaxTree.ParseText(@"
        using Microsoft.AspNetCore.Razor.TagHelpers;

        /// <summary>
        /// The summary for <see cref=""DocumentedTagHelper""/>.
        /// </summary>
        /// <remarks>
        /// Inherits from <see cref=""TagHelper""/>.
        /// </remarks>
        public class DocumentedTagHelper : " + typeof(AspNetCore.Razor.TagHelpers.TagHelper).Name + @"
        {
        }");
        var compilation = TestCompilation.Create(_assembly, syntaxTree);
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
        var errorSink = new ErrorSink();
        var syntaxTree = CSharpSyntaxTree.ParseText(@"
        using System.Collections.Generic;

        public class DocumentedTagHelper : " + typeof(AspNetCore.Razor.TagHelpers.TagHelper).FullName + @"
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
        var compilation = TestCompilation.Create(_assembly, syntaxTree);
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

    private static BoundAttributeDescriptor CreateAttributeFor(Type tagHelperType, Action<BoundAttributeDescriptorBuilder> configure)
    {
        var tagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, tagHelperType.Name, "Test");
        tagHelperBuilder.Metadata(TypeName(tagHelperType.FullName));

        var attributeBuilder = new BoundAttributeDescriptorBuilder(tagHelperBuilder, TagHelperConventions.DefaultKind);
        configure(attributeBuilder);
        return attributeBuilder.Build();
    }
}

[AspNetCore.Razor.TagHelpers.OutputElementHint("hinted-value")]
public class OutputElementHintTagHelper : AspNetCore.Razor.TagHelpers.TagHelper
{
}

public class InheritedOutputElementHintTagHelper : OutputElementHintTagHelper
{
}

[AspNetCore.Razor.TagHelpers.OutputElementHint("overridden")]
public class OverriddenOutputElementHintTagHelper : OutputElementHintTagHelper
{
}

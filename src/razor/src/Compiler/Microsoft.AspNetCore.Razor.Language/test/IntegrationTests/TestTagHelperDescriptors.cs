﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Reflection;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests;

public class TestTagHelperDescriptors
{
    public static IEnumerable<TagHelperDescriptor> SimpleTagHelperDescriptors
    {
        get
        {
            return new[]
            {
                    CreateTagHelperDescriptor(
                        tagName: "span",
                        typeName: "SpanTagHelper",
                        assemblyName: "TestAssembly"),
                    CreateTagHelperDescriptor(
                        tagName: "div",
                        typeName: "DivTagHelper",
                        assemblyName: "TestAssembly"),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "InputTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("value")
                                .Metadata(PropertyName("FooProp"))
                                .TypeName("System.String"),
                            builder => builder
                                .Name("bound")
                                .Metadata(PropertyName("BoundProp"))
                                .TypeName("System.String"),
                            builder => builder
                                .Name("age")
                                .Metadata(PropertyName("AgeProp"))
                                .TypeName("System.Int32"),
                            builder => builder
                                .Name("alive")
                                .Metadata(PropertyName("AliveProp"))
                                .TypeName("System.Boolean"),
                            builder => builder
                                .Name("tag")
                                .Metadata(PropertyName("TagProp"))
                                .TypeName("System.Object"),
                            builder => builder
                                .Name("tuple-dictionary")
                                .Metadata(PropertyName("DictionaryOfBoolAndStringTupleProperty"))
                                .TypeName(typeof(IDictionary<string, int>).Namespace + ".IDictionary<System.String, (System.Boolean, System.String)>")
                                .AsDictionaryAttribute("tuple-prefix-", typeof((bool, string)).FullName)
                        })
                };
        }
    }

    public static IEnumerable<TagHelperDescriptor> MinimizedBooleanTagHelperDescriptors
    {
        get
        {
            return new[]
            {
                    CreateTagHelperDescriptor(
                        tagName: "span",
                        typeName: "SpanTagHelper",
                        assemblyName: "TestAssembly"),
                    CreateTagHelperDescriptor(
                        tagName: "div",
                        typeName: "DivTagHelper",
                        assemblyName: "TestAssembly"),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "InputTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("value")
                                .Metadata(PropertyName("FooProp"))
                                .TypeName("System.String"),
                            builder => builder
                                .Name("bound")
                                .Metadata(PropertyName("BoundProp"))
                                .TypeName("System.Boolean"),
                            builder => builder
                                .Name("age")
                                .Metadata(PropertyName("AgeProp"))
                                .TypeName("System.Int32"),
                        })
                };
        }
    }

    public static IEnumerable<TagHelperDescriptor> CssSelectorTagHelperDescriptors
    {
        get
        {
            var inputTypePropertyInfo = typeof(TestType).GetRuntimeProperty("Type");
            var inputCheckedPropertyInfo = typeof(TestType).GetRuntimeProperty("Checked");

            return new[]
            {
                    CreateTagHelperDescriptor(
                        tagName: "a",
                        typeName: "TestNamespace.ATagHelper",
                        assemblyName: "TestAssembly",
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder
                                .RequireAttributeDescriptor(attribute => attribute
                                    .Name("href", RequiredAttributeNameComparison.FullMatch)
                                    .Value("~/", RequiredAttributeValueComparison.FullMatch)),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "a",
                        typeName: "TestNamespace.ATagHelperMultipleSelectors",
                        assemblyName: "TestAssembly",
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder
                                .RequireAttributeDescriptor(attribute => attribute
                                    .Name("href", RequiredAttributeNameComparison.FullMatch)
                                    .Value("~/", RequiredAttributeValueComparison.PrefixMatch))
                                .RequireAttributeDescriptor(attribute => attribute
                                    .Name("href", RequiredAttributeNameComparison.FullMatch)
                                    .Value("?hello=world", RequiredAttributeValueComparison.SuffixMatch)),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "TestNamespace.InputTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "type", inputTypePropertyInfo),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder
                                .RequireAttributeDescriptor(attribute => attribute
                                    .Name("type", RequiredAttributeNameComparison.FullMatch)
                                    .Value("text", RequiredAttributeValueComparison.FullMatch)),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "TestNamespace.InputTagHelper2",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "type", inputTypePropertyInfo),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder
                                .RequireAttributeDescriptor(attribute => attribute
                                    .Name("ty", RequiredAttributeNameComparison.PrefixMatch)),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "*",
                        typeName: "TestNamespace.CatchAllTagHelper",
                        assemblyName: "TestAssembly",
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder
                                .RequireAttributeDescriptor(attribute => attribute
                                    .Name("href", RequiredAttributeNameComparison.FullMatch)
                                    .Value("~/", RequiredAttributeValueComparison.PrefixMatch)),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "*",
                        typeName: "TestNamespace.CatchAllTagHelper2",
                        assemblyName: "TestAssembly",
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder
                                .RequireAttributeDescriptor(attribute => attribute
                                    .Name("type", RequiredAttributeNameComparison.FullMatch)),
                        }),
                };
        }
    }

    public static IEnumerable<TagHelperDescriptor> EnumTagHelperDescriptors
    {
        get
        {
            return new[]
            {
                CreateTagHelperDescriptor(
                    tagName: "*",
                    typeName: "TestNamespace.CatchAllTagHelper",
                    assemblyName: "TestAssembly",
                    attributes: new Action<BoundAttributeDescriptorBuilder>[]
                    {
                        builder => builder
                            .Name("catch-all")
                            .Metadata(PropertyName("CatchAll"))
                            .AsEnum()
                            .TypeName("Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestTagHelperDescriptors.MyEnum"),
                    }),
                CreateTagHelperDescriptor(
                    tagName: "input",
                    typeName: "TestNamespace.InputTagHelper",
                    assemblyName: "TestAssembly",
                    attributes: new Action<BoundAttributeDescriptorBuilder>[]
                    {
                        builder => builder
                            .Name("value")
                            .Metadata(PropertyName("Value"))
                            .AsEnum()
                            .TypeName("Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestTagHelperDescriptors.MyEnum"),
                    }),
            };
        }
    }

    public static IEnumerable<TagHelperDescriptor> SymbolBoundTagHelperDescriptors
    {
        get
        {
            return new[]
            {
                    CreateTagHelperDescriptor(
                        tagName: "*",
                        typeName: "TestNamespace.CatchAllTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("[item]")
                                .Metadata(PropertyName("ListItems"))
                                .TypeName("System.Collections.Generic.List<string>"),
                            builder => builder
                                .Name("[(item)]")
                                .Metadata(PropertyName("ArrayItems"))
                                .TypeName(typeof(string[]).FullName),
                            builder => builder
                                .Name("(click)")
                                .Metadata(PropertyName("Event1"))
                                .TypeName(typeof(Action).FullName),
                            builder => builder
                                .Name("(^click)")
                                .Metadata(PropertyName("Event2"))
                                .TypeName(typeof(Action).FullName),
                            builder => builder
                                .Name("*something")
                                .Metadata(PropertyName("StringProperty1"))
                                .TypeName(typeof(string).FullName),
                            builder => builder
                                .Name("#local")
                                .Metadata(PropertyName("StringProperty2"))
                                .TypeName(typeof(string).FullName),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("bound")),
                        }),
                };
        }
    }

    public static IEnumerable<TagHelperDescriptor> MinimizedTagHelpers_Descriptors
    {
        get
        {
            return new[]
            {
                    CreateTagHelperDescriptor(
                        tagName: "*",
                        typeName: "TestNamespace.CatchAllTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("catchall-bound-string")
                                .Metadata(PropertyName("BoundRequiredString"))
                                .TypeName(typeof(string).FullName),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("catchall-unbound-required")),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "TestNamespace.InputTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("input-bound-required-string")
                                .Metadata(PropertyName("BoundRequiredString"))
                                .TypeName(typeof(string).FullName),
                            builder => builder
                                .Name("input-bound-string")
                                .Metadata(PropertyName("BoundString"))
                                .TypeName(typeof(string).FullName),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder
                                .RequireAttributeDescriptor(attribute => attribute.Name("input-bound-required-string"))
                                .RequireAttributeDescriptor(attribute => attribute.Name("input-unbound-required")),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "div",
                        typeName: "DivTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => builder
                                .Name("boundbool")
                                .Metadata(PropertyName("BoundBoolProp"))
                                .TypeName(typeof(bool).FullName),
                            builder => builder
                                .Name("booldict")
                                .Metadata(PropertyName("BoolDictProp"))
                                .TypeName("System.Collections.Generic.IDictionary<string, bool>")
                                .AsDictionaryAttribute("booldict-prefix-", typeof(bool).FullName),
                        }),
                };
        }
    }

    public static IEnumerable<TagHelperDescriptor> DynamicAttributeTagHelpers_Descriptors
    {
        get
        {
            return new[]
            {
                CreateTagHelperDescriptor(
                    tagName: "input",
                    typeName: "TestNamespace.InputTagHelper",
                    assemblyName: "TestAssembly",
                    attributes: new Action<BoundAttributeDescriptorBuilder>[]
                    {
                        builder => builder
                            .Name("bound")
                            .Metadata(PropertyName("Bound"))
                            .TypeName(typeof(string).FullName)
                    }),
                };
        }
    }

    public static IEnumerable<TagHelperDescriptor> DuplicateTargetTagHelperDescriptors
    {
        get
        {
            var typePropertyInfo = typeof(TestType).GetRuntimeProperty("Type");
            var checkedPropertyInfo = typeof(TestType).GetRuntimeProperty("Checked");
            return new[]
            {
                    CreateTagHelperDescriptor(
                        tagName: "*",
                        typeName: "TestNamespace.CatchAllTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "type", typePropertyInfo),
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "checked", checkedPropertyInfo),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("type")),
                            builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("checked"))
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "TestNamespace.InputTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "type", typePropertyInfo),
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "checked", checkedPropertyInfo),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("type")),
                            builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("checked"))
                        })
                };
        }
    }

    public static IEnumerable<TagHelperDescriptor> AttributeTargetingTagHelperDescriptors
    {
        get
        {
            var inputTypePropertyInfo = typeof(TestType).GetRuntimeProperty("Type");
            var inputCheckedPropertyInfo = typeof(TestType).GetRuntimeProperty("Checked");
            return new[]
            {
                    CreateTagHelperDescriptor(
                        tagName: "p",
                        typeName: "TestNamespace.PTagHelper",
                        assemblyName: "TestAssembly",
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("class")),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "TestNamespace.InputTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "type", inputTypePropertyInfo),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("type")),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "TestNamespace.InputTagHelper2",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "type", inputTypePropertyInfo),
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "checked", inputCheckedPropertyInfo),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder
                                .RequireAttributeDescriptor(attribute => attribute.Name("type"))
                                .RequireAttributeDescriptor(attribute => attribute.Name("checked")),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "*",
                        typeName: "TestNamespace.CatchAllTagHelper",
                        assemblyName: "TestAssembly",
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder.RequireAttributeDescriptor(attribute => attribute.Name("catchAll")),
                        }),
                };
        }
    }

    public static IEnumerable<TagHelperDescriptor> PrefixedAttributeTagHelperDescriptors
    {
        get
        {
            return new[]
            {
                CreateTagHelperDescriptor(
                    tagName: "input",
                    typeName: "TestNamespace.InputTagHelper1",
                    assemblyName: "TestAssembly",
                    attributes: new Action<BoundAttributeDescriptorBuilder>[]
                    {
                        builder => builder
                            .Name("int-prefix-grabber")
                            .Metadata(PropertyName("IntProperty"))
                            .TypeName(typeof(int).FullName),
                        builder => builder
                            .Name("int-dictionary")
                            .Metadata(PropertyName("IntDictionaryProperty"))
                            .TypeName("System.Collections.Generic.IDictionary<string, int>")
                            .AsDictionaryAttribute("int-prefix-", typeof(int).FullName),
                        builder => builder
                            .Name("string-prefix-grabber")
                            .Metadata(PropertyName("StringProperty"))
                            .TypeName(typeof(string).FullName),
                        builder => builder
                            .Name("string-dictionary")
                            .Metadata(PropertyName("StringDictionaryProperty"))
                            .TypeName("Namespace.DictionaryWithoutParameterlessConstructor<string, string>")
                            .AsDictionaryAttribute("string-prefix-", typeof(string).FullName),
                    }),
                CreateTagHelperDescriptor(
                    tagName: "input",
                    typeName: "TestNamespace.InputTagHelper2",
                    assemblyName: "TestAssembly",
                    attributes: new Action<BoundAttributeDescriptorBuilder>[]
                    {
                        builder => builder
                            .Name("int-dictionary")
                            .Metadata(PropertyName("IntDictionaryProperty"))
                            .TypeName(typeof(int).FullName)
                            .AsDictionaryAttribute("int-prefix-", typeof(int).FullName),
                        builder => builder
                            .Name("string-dictionary")
                            .Metadata(PropertyName("StringDictionaryProperty"))
                            .TypeName("Namespace.DictionaryWithoutParameterlessConstructor<string, string>")
                            .AsDictionaryAttribute("string-prefix-", typeof(string).FullName),
                    }),
            };
        }
    }

    public static IEnumerable<TagHelperDescriptor> TagHelpersInSectionDescriptors
    {
        get
        {
            var propertyInfo = typeof(TestType).GetRuntimeProperty("BoundProperty");
            return new[]
            {
                    CreateTagHelperDescriptor(
                        tagName: "MyTagHelper",
                        typeName: "TestNamespace.MyTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "BoundProperty", propertyInfo),
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "NestedTagHelper",
                        typeName: "TestNamespace.NestedTagHelper",
                        assemblyName: "TestAssembly"),
                };
        }
    }

    public static IEnumerable<TagHelperDescriptor> DefaultPAndInputTagHelperDescriptors
    {
        get
        {
            var pAgePropertyInfo = typeof(TestType).GetRuntimeProperty("Age");
            var inputTypePropertyInfo = typeof(TestType).GetRuntimeProperty("Type");
            var checkedPropertyInfo = typeof(TestType).GetRuntimeProperty("Checked");

            return new[]
            {
                    CreateTagHelperDescriptor(
                        tagName: "p",
                        typeName: "TestNamespace.PTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "age", pAgePropertyInfo),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder.RequireTagStructure(TagStructure.NormalOrSelfClosing)
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "TestNamespace.InputTagHelper",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "type", inputTypePropertyInfo),
                        },
                        ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
                        {
                            builder => builder.RequireTagStructure(TagStructure.WithoutEndTag)
                        }),
                    CreateTagHelperDescriptor(
                        tagName: "input",
                        typeName: "TestNamespace.InputTagHelper2",
                        assemblyName: "TestAssembly",
                        attributes: new Action<BoundAttributeDescriptorBuilder>[]
                        {
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "type", inputTypePropertyInfo),
                            builder => BuildBoundAttributeDescriptorFromPropertyInfo(builder, "checked", checkedPropertyInfo),
                        }),
                };
        }
    }

    private static TagHelperDescriptor CreateTagHelperDescriptor(
        string tagName,
        string typeName,
        string assemblyName,
        IEnumerable<Action<BoundAttributeDescriptorBuilder>> attributes = null,
        IEnumerable<Action<TagMatchingRuleDescriptorBuilder>> ruleBuilders = null)
    {
        var builder = TagHelperDescriptorBuilder.Create(typeName, assemblyName);
        builder.Metadata(TypeName(typeName));

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

    private static void BuildBoundAttributeDescriptorFromPropertyInfo(
        BoundAttributeDescriptorBuilder builder,
        string name,
        PropertyInfo propertyInfo)
    {
        builder
            .Name(name)
            .Metadata(PropertyName(propertyInfo.Name))
            .TypeName(propertyInfo.PropertyType.FullName);

        if (propertyInfo.PropertyType.GetTypeInfo().IsEnum)
        {
            builder.AsEnum();
        }
    }

    private class TestType
    {
        public int Age { get; set; }

        public string Type { get; set; }

        public bool Checked { get; set; }

        public string BoundProperty { get; set; }
    }

    public static readonly string Code = """
        namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests
        {
            public class TestTagHelperDescriptors
            {
                public enum MyEnum
                {
                    MyValue,
                    MySecondValue
                }
            }
        }
        """;
}

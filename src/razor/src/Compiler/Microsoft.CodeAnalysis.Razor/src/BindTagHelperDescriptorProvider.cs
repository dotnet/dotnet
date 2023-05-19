// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor;

internal class BindTagHelperDescriptorProvider : ITagHelperDescriptorProvider
{
    private static TagHelperDescriptor s_fallbackBindTagHelper;

    // Run after the component tag helper provider, because we need to see the results.
    public int Order { get; set; } = 1000;

    public RazorEngine Engine { get; set; }

    public void Execute(TagHelperDescriptorProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // This provider returns tag helper information for 'bind' which doesn't necessarily
        // map to any real component. Bind behaviors more like a macro, which can map a single LValue to
        // both a 'value' attribute and a 'value changed' attribute.
        //
        // User types:
        //      <input type="text" @bind="@FirstName"/>
        //
        // We generate:
        //      <input type="text"
        //          value="@BindMethods.GetValue(FirstName)"
        //          onchange="@EventCallbackFactory.CreateBinder(this, __value => FirstName = __value, FirstName)"/>
        //
        // This isn't very different from code the user could write themselves - thus the pronouncement
        // that @bind is very much like a macro.
        //
        // A lot of the value that provide in this case is that the associations between the
        // elements, and the attributes aren't straightforward.
        //
        // For instance on <input type="text" /> we need to listen to 'value' and 'onchange',
        // but on <input type="checked" we need to listen to 'checked' and 'onchange'.
        //
        // We handle a few different cases here:
        //
        //  1.  When given an attribute like **anywhere**'@bind-value="@FirstName"' and '@bind-value:event="onchange"' we will
        //      generate the 'value' attribute and 'onchange' attribute.
        //
        //      We don't do any transformation or inference for this case, because the developer has
        //      told us exactly what to do. This is the *full* form of @bind, and should support any
        //      combination of element, component, and attributes.
        //
        //      This is the most general case, and is implemented with a built-in tag helper that applies
        //      to everything, and binds to a dictionary of attributes that start with @bind-.
        //
        //  2.  We also support cases like '@bind-value="@FirstName"' where we will generate the 'value'
        //      attribute and another attribute based for a changed handler based on the metadata.
        //
        //     These mappings are provided by attributes that tell us what attributes, suffixes, and
        //      elements to map.
        //
        //  3.  When given an attribute like '@bind="@FirstName"' we will generate a value and change
        //      attribute solely based on the context. We need the context of an HTML tag to know
        //      what attributes to generate.
        //
        //      Similar to case #2, this should 'just work' from the users point of view. We expect
        //      using this syntax most frequently with input elements.
        //
        //      These mappings are also provided by attributes. Primarily these are used by <input />
        //      and so we have a special case for input elements and their type attributes.
        //
        //      Additionally, our mappings tell us about cases like <input type="number" ... /> where
        //      we need to treat the value as an invariant culture value. In general the HTML5 field
        //      types use invariant culture values when interacting with the DOM, in contrast to
        //      <input type="text" ... /> which is free-form text and is most likely to be
        //      culture-sensitive.
        //
        //  4.  For components, we have a bit of a special case. We can infer a syntax that matches
        //      case #2 based on property names. So if a component provides both 'Value' and 'ValueChanged'
        //      we will turn that into an instance of bind.
        //
        // So case #1 here is the most general case. Case #2 and #3 are data-driven based on attribute data
        // we have. Case #4 is data-driven based on component definitions.
        //
        // We provide a good set of attributes that map to the HTML dom. This set is user extensible.
        var compilation = context.GetCompilation();
        if (compilation == null)
        {
            return;
        }

        var bindMethods = compilation.GetTypeByMetadataName(ComponentsApi.BindConverter.FullTypeName);
        if (bindMethods == null)
        {
            // If we can't find BindConverter, then just bail. We won't be able to compile the
            // generated code anyway.
            return;
        }

        var targetSymbol = context.Items.GetTargetSymbol();
        if (targetSymbol is not null && !SymbolEqualityComparer.Default.Equals(targetSymbol, bindMethods.ContainingAssembly))
        {
            return;
        }

        // Tag Helper definition for case #1. This is the most general case.
        context.Results.Add(GetOrCreateFallbackBindTagHelper());

        // For case #2 & #3 we have a whole bunch of attribute entries on BindMethods that we can use
        // to data-drive the definitions of these tag helpers.
        var elementBindData = GetElementBindData(compilation);

        // Case #2 & #3
        foreach (var tagHelper in CreateElementBindTagHelpers(elementBindData))
        {
            context.Results.Add(tagHelper);
        }

        // For case #4 we look at the tag helpers that were already created corresponding to components
        // and pattern match on properties.
        foreach (var tagHelper in CreateComponentBindTagHelpers(context.Results))
        {
            context.Results.Add(tagHelper);
        }
    }

    private static TagHelperDescriptor GetOrCreateFallbackBindTagHelper()
    {
        return s_fallbackBindTagHelper ??= CreateFallbackBindTagHelper();

        static TagHelperDescriptor CreateFallbackBindTagHelper()
        {
            using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
                ComponentMetadata.Bind.TagHelperKind, "Bind", ComponentsApi.AssemblyName,
                out var builder);

            builder.CaseSensitive = true;
            builder.SetDocumentation(DocumentationDescriptor.BindTagHelper_Fallback);

            builder.Metadata.Add(ComponentMetadata.SpecialKindKey, ComponentMetadata.Bind.TagHelperKind);
            builder.Metadata.Add(TagHelperMetadata.Common.ClassifyAttributesOnly, bool.TrueString);
            builder.Metadata[TagHelperMetadata.Runtime.Name] = ComponentMetadata.Bind.RuntimeName;
            builder.Metadata[ComponentMetadata.Bind.FallbackKey] = bool.TrueString;

            // WTE has a bug in 15.7p1 where a Tag Helper without a display-name that looks like
            // a C# property will crash trying to create the toolips.
            builder.SetTypeName("Microsoft.AspNetCore.Components.Bind");
            builder.SetTypeNamespace("Microsoft.AspNetCore.Components");
            builder.SetTypeNameIdentifier("Bind");

            builder.TagMatchingRule(rule =>
            {
                rule.TagName = "*";
                rule.Attribute(attribute =>
                {
                    attribute.Name = "@bind-";
                    attribute.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
                    attribute.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                });
            });

            builder.BindAttribute(attribute =>
            {
                attribute.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                attribute.SetDocumentation(DocumentationDescriptor.BindTagHelper_Fallback);

                var attributeName = "@bind-...";
                attribute.Name = attributeName;
                attribute.AsDictionary("@bind-", typeof(object).FullName);

                // WTE has a bug 15.7p1 where a Tag Helper without a display-name that looks like
                // a C# property will crash trying to create the toolips.
                attribute.SetPropertyName("Bind");
                attribute.TypeName = "System.Collections.Generic.Dictionary<string, object>";

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "format";
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Fallback_Format);

                    parameter.SetPropertyName("Format");
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "event";
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Fallback_Event, attributeName));

                    parameter.SetPropertyName("Event");
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "culture";
                    parameter.TypeName = typeof(CultureInfo).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Culture);

                    parameter.SetPropertyName("Culture");
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "get";
                    parameter.TypeName = typeof(object).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Get);

                    parameter.SetPropertyName("Get");

                    parameter.SetBindAttributeGetSet();
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "set";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Set);

                    parameter.SetPropertyName("Set");
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "after";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_After);

                    parameter.SetPropertyName("After");
                });
            });

            return builder.Build();
        }
    }

    private static ImmutableArray<ElementBindData> GetElementBindData(Compilation compilation)
    {
        var bindElement = compilation.GetTypeByMetadataName(ComponentsApi.BindElementAttribute.FullTypeName);
        var bindInputElement = compilation.GetTypeByMetadataName(ComponentsApi.BindInputElementAttribute.FullTypeName);

        if (bindElement == null || bindInputElement == null)
        {
            // This won't likely happen, but just in case.
            return ImmutableArray<ElementBindData>.Empty;
        }

        using var _ = ListPool<INamedTypeSymbol>.GetPooledObject(out var types);
        var visitor = new BindElementDataVisitor(types);

        // Visit the primary output of this compilation, as well as all references.
        visitor.Visit(compilation.Assembly);

        foreach (var reference in compilation.References)
        {
            // We ignore .netmodules here - there really isn't a case where they are used by user code
            // even though the Roslyn APIs all support them.
            if (compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
            {
                visitor.Visit(assembly);
            }
        }

        using var results = new PooledArrayBuilder<ElementBindData>();

        foreach (var type in types)
        {
            // Create helper to delay computing display names for this type when we need them.
            var displayNames = new DisplayNameHelper(type);

            // Not handling duplicates here for now since we're the primary ones extending this.
            // If we see users adding to the set of 'bind' constructs we will want to add deduplication
            // and potentially diagnostics.
            foreach (var attribute in type.GetAttributes())
            {
                var constructorArguments = attribute.ConstructorArguments;

                // We need to check the constructor argument length here, because this can show up as 0
                // if the language service fails to initialize. This is an invalid case, so skip it.
                if (constructorArguments.Length == 4 && SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, bindElement))
                {
                    var (assemblyName, typeName, namespaceName) = displayNames.GetNames();

                    results.Add(new ElementBindData(
                        assemblyName,
                        typeName,
                        namespaceName,
                        type.Name,
                        (string)constructorArguments[0].Value,
                        null,
                        (string)constructorArguments[1].Value,
                        (string)constructorArguments[2].Value,
                        (string)constructorArguments[3].Value));
                }
                else if (constructorArguments.Length == 4 && SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, bindInputElement))
                {
                    var (assemblyName, typeName, namespaceName) = displayNames.GetNames();

                    results.Add(new ElementBindData(
                        assemblyName,
                        typeName,
                        namespaceName,
                        type.Name,
                        "input",
                        (string)constructorArguments[0].Value,
                        (string)constructorArguments[1].Value,
                        (string)constructorArguments[2].Value,
                        (string)constructorArguments[3].Value));
                }
                else if (constructorArguments.Length == 6 && SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, bindInputElement))
                {
                    var (assemblyName, typeName, namespaceName) = displayNames.GetNames();

                    results.Add(new ElementBindData(
                        assemblyName,
                        typeName,
                        namespaceName,
                        type.Name,
                        "input",
                        (string)constructorArguments[0].Value,
                        (string)constructorArguments[1].Value,
                        (string)constructorArguments[2].Value,
                        (string)constructorArguments[3].Value,
                        (bool)constructorArguments[4].Value,
                        (string)constructorArguments[5].Value));
                }
            }
        }

        return results.DrainToImmutable();
    }

    /// <summary>
    ///  Helper to avoid computing various type-based names until necessary.
    /// </summary>
    private ref struct DisplayNameHelper
    {
        private readonly INamedTypeSymbol _type;
        private (string Assembly, string Type, string Namespace)? _names;

        public DisplayNameHelper(INamedTypeSymbol type)
        {
            _type = type;
        }

        public (string Assembly, string Type, string Namespace) GetNames()
        {
            _names ??= (_type.ContainingAssembly.Name, _type.ToDisplayString(), _type.ContainingNamespace.ToDisplayString());

            return _names.GetValueOrDefault();
        }
    }

    private static ImmutableArray<TagHelperDescriptor> CreateElementBindTagHelpers(ImmutableArray<ElementBindData> data)
    {
        using var results = new PooledArrayBuilder<TagHelperDescriptor>();

        foreach (var entry in data)
        {
            var name = entry.Suffix == null ? "Bind" : "Bind_" + entry.Suffix;
            var attributeName = entry.Suffix == null ? "@bind" : "@bind-" + entry.Suffix;

            var formatName = entry.Suffix == null ? "Format_" + entry.ValueAttribute : "Format_" + entry.Suffix;
            var formatAttributeName = entry.Suffix == null ? "format-" + entry.ValueAttribute : "format-" + entry.Suffix;

            var eventName = entry.Suffix == null ? "Event_" + entry.ValueAttribute : "Event_" + entry.Suffix;

            using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
                ComponentMetadata.Bind.TagHelperKind, name, ComponentsApi.AssemblyName,
                out var builder);

            builder.CaseSensitive = true;
            builder.SetDocumentation(
                DocumentationDescriptor.From(
                    DocumentationId.BindTagHelper_Element,
                    entry.ValueAttribute,
                    entry.ChangeAttribute));

            builder.Metadata.Add(ComponentMetadata.SpecialKindKey, ComponentMetadata.Bind.TagHelperKind);
            builder.Metadata.Add(TagHelperMetadata.Common.ClassifyAttributesOnly, bool.TrueString);
            builder.Metadata[TagHelperMetadata.Runtime.Name] = ComponentMetadata.Bind.RuntimeName;
            builder.Metadata[ComponentMetadata.Bind.ValueAttribute] = entry.ValueAttribute;
            builder.Metadata[ComponentMetadata.Bind.ChangeAttribute] = entry.ChangeAttribute;
            builder.Metadata[ComponentMetadata.Bind.IsInvariantCulture] = entry.IsInvariantCulture ? bool.TrueString : bool.FalseString;
            builder.Metadata[ComponentMetadata.Bind.Format] = entry.Format;

            if (entry.TypeAttribute != null)
            {
                // For entries that map to the <input /> element, we need to be able to know
                // the difference between <input /> and <input type="text" .../> for which we
                // want to use the same attributes.
                //
                // We provide a tag helper for <input /> that should match all input elements,
                // but we only want it to be used when a more specific one is used.
                //
                // Therefore we use this metadata to know which one is more specific when two
                // tag helpers match.
                builder.Metadata[ComponentMetadata.Bind.TypeAttribute] = entry.TypeAttribute;
            }

            // WTE has a bug in 15.7p1 where a Tag Helper without a display-name that looks like
            // a C# property will crash trying to create the toolips.
            builder.SetTypeName(entry.TypeName);
            builder.SetTypeNamespace(entry.TypeNamespace);
            builder.SetTypeNameIdentifier(entry.TypeNameIdentifier);

            builder.TagMatchingRule(rule =>
            {
                rule.TagName = entry.Element;
                if (entry.TypeAttribute != null)
                {
                    rule.Attribute(a =>
                    {
                        a.Name = "type";
                        a.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        a.Value = entry.TypeAttribute;
                        a.ValueComparisonMode = RequiredAttributeDescriptor.ValueComparisonMode.FullMatch;
                    });
                }

                rule.Attribute(a =>
                {
                    a.Name = attributeName;
                    a.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                    a.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                });
            });

            builder.TagMatchingRule(rule =>
            {
                rule.TagName = entry.Element;
                if (entry.TypeAttribute != null)
                {
                    rule.Attribute(a =>
                    {
                        a.Name = "type";
                        a.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        a.Value = entry.TypeAttribute;
                        a.ValueComparisonMode = RequiredAttributeDescriptor.ValueComparisonMode.FullMatch;
                    });
                }

                rule.Attribute(a =>
                {
                    a.Name = $"{attributeName}:get";
                    a.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                    a.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                });

                rule.Attribute(a =>
                {
                    a.Name = $"{attributeName}:set";
                    a.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                    a.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                });
            });

            builder.BindAttribute(a =>
            {
                a.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                a.SetDocumentation(
                    DocumentationDescriptor.From(
                        DocumentationId.BindTagHelper_Element,
                        entry.ValueAttribute,
                        entry.ChangeAttribute));

                a.Name = attributeName;
                a.TypeName = typeof(object).FullName;

                // WTE has a bug 15.7p1 where a Tag Helper without a display-name that looks like
                // a C# property will crash trying to create the toolips.
                a.SetPropertyName(name);

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "format";
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Element_Format,
                            attributeName));

                    parameter.SetPropertyName(formatName);
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "event";
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Element_Event,
                            attributeName));

                    parameter.SetPropertyName(eventName);
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "culture";
                    parameter.TypeName = typeof(CultureInfo).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Culture);

                    parameter.SetPropertyName("Culture");
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "get";
                    parameter.TypeName = typeof(object).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Get);

                    parameter.SetPropertyName("Get");
                    parameter.SetBindAttributeGetSet();
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "set";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Set);

                    parameter.SetPropertyName("Set");
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "after";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_After);

                    parameter.SetPropertyName("After");
                });
            });

            // This is no longer supported. This is just here so we can add a diagnostic later on when this matches.
            builder.BindAttribute(attribute =>
            {
                attribute.Name = formatAttributeName;
                attribute.TypeName = "System.String";
                attribute.SetDocumentation(
                    DocumentationDescriptor.From(
                        DocumentationId.BindTagHelper_Element_Format,
                        attributeName));

                // WTE has a bug 15.7p1 where a Tag Helper without a display-name that looks like
                // a C# property will crash trying to create the toolips.
                attribute.SetPropertyName(formatName);
            });

            results.Add(builder.Build());
        }

        return results.DrainToImmutable();
    }

    private static ImmutableArray<TagHelperDescriptor> CreateComponentBindTagHelpers(ICollection<TagHelperDescriptor> tagHelpers)
    {
        using var results = new PooledArrayBuilder<TagHelperDescriptor>();

        foreach (var tagHelper in tagHelpers)
        {
            if (!tagHelper.IsComponentTagHelper())
            {
                continue;
            }

            // We want to create a 'bind' tag helper everywhere we see a pair of properties like `Foo`, `FooChanged`
            // where `FooChanged` is a delegate and `Foo` is not.
            //
            // The easiest way to figure this out without a lot of backtracking is to look for `FooChanged` and then
            // try to find a matching "Foo".
            //
            // We also look for a corresponding FooExpression attribute, though its presence is optional.
            for (var i = 0; i < tagHelper.BoundAttributes.Count; i++)
            {
                var changeAttribute = tagHelper.BoundAttributes[i];
                if (!changeAttribute.Name.EndsWith("Changed", StringComparison.Ordinal) ||

                    // Allow the ValueChanged attribute to be a delegate or EventCallback<>.
                    //
                    // We assume that the Delegate or EventCallback<> has a matching type, and the C# compiler will help
                    // you figure figure it out if you did it wrongly.
                    (!changeAttribute.IsDelegateProperty() && !changeAttribute.IsEventCallbackProperty()))
                {
                    continue;
                }

                BoundAttributeDescriptor valueAttribute = null;
                BoundAttributeDescriptor expressionAttribute = null;
                var valueAttributeName = changeAttribute.Name[..^"Changed".Length];
                var expressionAttributeName = valueAttributeName + "Expression";
                for (var j = 0; j < tagHelper.BoundAttributes.Count; j++)
                {
                    if (tagHelper.BoundAttributes[j].Name == valueAttributeName)
                    {
                        valueAttribute = tagHelper.BoundAttributes[j];
                    }

                    if (tagHelper.BoundAttributes[j].Name == expressionAttributeName)
                    {
                        expressionAttribute = tagHelper.BoundAttributes[j];
                    }

                    if (valueAttribute != null && expressionAttribute != null)
                    {
                        // We found both, so we can stop looking now
                        break;
                    }
                }

                if (valueAttribute == null)
                {
                    // No matching attribute found.
                    continue;
                }

                using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
                    ComponentMetadata.Bind.TagHelperKind, tagHelper.Name, tagHelper.AssemblyName,
                    out var builder);

                builder.DisplayName = tagHelper.DisplayName;
                builder.CaseSensitive = true;
                builder.SetDocumentation(
                    DocumentationDescriptor.From(
                        DocumentationId.BindTagHelper_Component,
                        valueAttribute.Name,
                        changeAttribute.Name));

                builder.Metadata.Add(ComponentMetadata.SpecialKindKey, ComponentMetadata.Bind.TagHelperKind);
                builder.Metadata[TagHelperMetadata.Runtime.Name] = ComponentMetadata.Bind.RuntimeName;
                builder.Metadata[ComponentMetadata.Bind.ValueAttribute] = valueAttribute.Name;
                builder.Metadata[ComponentMetadata.Bind.ChangeAttribute] = changeAttribute.Name;

                if (expressionAttribute != null)
                {
                    builder.Metadata[ComponentMetadata.Bind.ExpressionAttribute] = expressionAttribute.Name;
                }

                // WTE has a bug 15.7p1 where a Tag Helper without a display-name that looks like
                // a C# property will crash trying to create the toolips.
                builder.SetTypeName(tagHelper.GetTypeName());
                builder.SetTypeNamespace(tagHelper.GetTypeNamespace());
                builder.SetTypeNameIdentifier(tagHelper.GetTypeNameIdentifier());

                // Match the component and attribute name
                builder.TagMatchingRule(rule =>
                {
                    rule.TagName = tagHelper.TagMatchingRules.Single().TagName;
                    rule.Attribute(attribute =>
                    {
                        attribute.Name = "@bind-" + valueAttribute.Name;
                        attribute.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        attribute.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                    });
                });

                builder.TagMatchingRule(rule =>
                {
                    rule.TagName = tagHelper.TagMatchingRules.Single().TagName;
                    rule.Attribute(attribute =>
                    {
                        attribute.Name = "@bind-" + valueAttribute.Name + ":get";
                        attribute.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        attribute.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                    });
                    rule.Attribute(attribute =>
                    {
                        attribute.Name = "@bind-" + valueAttribute.Name + ":set";
                        attribute.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        attribute.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                    });
                });

                builder.BindAttribute(attribute =>
                {
                    attribute.Metadata[ComponentMetadata.Common.DirectiveAttribute] = bool.TrueString;
                    attribute.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Component,
                            valueAttribute.Name,
                            changeAttribute.Name));

                    attribute.Name = "@bind-" + valueAttribute.Name;
                    attribute.TypeName = changeAttribute.TypeName;
                    attribute.IsEnum = valueAttribute.IsEnum;

                    // WTE has a bug 15.7p1 where a Tag Helper without a display-name that looks like
                    // a C# property will crash trying to create the toolips.
                    attribute.SetPropertyName(valueAttribute.GetPropertyName());

                    attribute.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "get";
                        parameter.TypeName = typeof(object).FullName;
                        parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Get);

                        parameter.SetPropertyName("Get");
                        parameter.SetBindAttributeGetSet();
                    });

                    attribute.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "set";
                        parameter.TypeName = typeof(Delegate).FullName;
                        parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Set);

                        parameter.SetPropertyName("Set");
                    });

                    attribute.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "after";
                        parameter.TypeName = typeof(Delegate).FullName;
                        parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_After);

                        parameter.SetPropertyName("After");
                    });
                });


                if (tagHelper.IsComponentFullyQualifiedNameMatch())
                {
                    builder.Metadata[ComponentMetadata.Component.NameMatchKey] = ComponentMetadata.Component.FullyQualifiedNameMatch;
                }

                results.Add(builder.Build());
            }
        }

        return results.DrainToImmutable();
    }

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    private readonly record struct ElementBindData(
        string Assembly,
        string TypeName,
        string TypeNamespace,
        string TypeNameIdentifier,
        string Element,
        string TypeAttribute,
        string Suffix,
        string ValueAttribute,
        string ChangeAttribute,
        bool IsInvariantCulture = false,
        string Format = null)
    {
        private string GetDebuggerDisplay()
        {
            return $"Element: {Element} - Suffix: {Suffix ?? "(none)"} - Type: {TypeAttribute} Value: {ValueAttribute} Change: {ChangeAttribute}";
        }
    }

    private class BindElementDataVisitor : SymbolVisitor
    {
        private readonly List<INamedTypeSymbol> _results;

        public BindElementDataVisitor(List<INamedTypeSymbol> results)
        {
            _results = results;
        }

        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            if (symbol.Name == "BindAttributes" && symbol.DeclaredAccessibility == Accessibility.Public)
            {
                _results.Add(symbol);
            }
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            foreach (var member in symbol.GetMembers())
            {
                Visit(member);
            }
        }

        public override void VisitAssembly(IAssemblySymbol symbol)
        {
            // This as a simple yet high-value optimization that excludes the vast majority of
            // assemblies that (by definition) can't contain a component.
            if (symbol.Name != null && !symbol.Name.StartsWith("System.", StringComparison.Ordinal))
            {
                Visit(symbol.GlobalNamespace);
            }
        }
    }
}

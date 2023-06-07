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
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

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

            builder.SetMetadata(
                SpecialKind(ComponentMetadata.Bind.TagHelperKind),
                MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
                RuntimeName(ComponentMetadata.Bind.RuntimeName),
                MakeTrue(ComponentMetadata.Bind.FallbackKey),
                TypeName("Microsoft.AspNetCore.Components.Bind"),
                TypeNamespace("Microsoft.AspNetCore.Components"),
                TypeNameIdentifier("Bind"));

            builder.TagMatchingRule(rule =>
            {
                rule.TagName = "*";
                rule.Attribute(attribute =>
                {
                    attribute.Name = "@bind-";
                    attribute.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch;
                    attribute.SetMetadata(Attributes.IsDirectiveAttribute);
                });
            });

            builder.BindAttribute(attribute =>
            {
                attribute.SetDocumentation(DocumentationDescriptor.BindTagHelper_Fallback);

                var attributeName = "@bind-...";
                attribute.Name = attributeName;
                attribute.AsDictionary("@bind-", typeof(object).FullName);

                attribute.SetMetadata(
                    PropertyName("Bind"),
                    IsDirectiveAttribute);

                attribute.TypeName = "System.Collections.Generic.Dictionary<string, object>";

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "format";
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Fallback_Format);

                    parameter.SetMetadata(Parameters.Format);
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "event";
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Fallback_Event, attributeName));

                    parameter.SetMetadata(Parameters.Event);
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "culture";
                    parameter.TypeName = typeof(CultureInfo).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Culture);

                    parameter.SetMetadata(Parameters.Culture);
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "get";
                    parameter.TypeName = typeof(object).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Get);

                    parameter.SetMetadata(Parameters.Get);
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "set";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Set);

                    parameter.SetMetadata(Parameters.Set);
                });

                attribute.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "after";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_After);

                    parameter.SetMetadata(Parameters.After);
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
            string name, attributeName, formatName, formatAttributeName, eventName;

            if (entry.Suffix is { } suffix)
            {
                name = "Bind_" + suffix;
                attributeName = "@bind-" + suffix;
                formatName = "Format_" + suffix;
                formatAttributeName = "format-" + suffix;
                eventName = "Event_" + suffix;
            }
            else
            {
                name = "Bind";
                attributeName = "@bind";

                suffix = entry.ValueAttribute;
                formatName = "Format_" + suffix;
                formatAttributeName = "format-" + suffix;
                eventName = "Event_" + suffix;
            }

            using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
                ComponentMetadata.Bind.TagHelperKind, name, ComponentsApi.AssemblyName,
                out var builder);

            using var metadata = builder.GetMetadataBuilder(ComponentMetadata.Bind.RuntimeName);

            builder.CaseSensitive = true;
            builder.SetDocumentation(
                DocumentationDescriptor.From(
                    DocumentationId.BindTagHelper_Element,
                    entry.ValueAttribute,
                    entry.ChangeAttribute));

            metadata.Add(SpecialKind(ComponentMetadata.Bind.TagHelperKind));
            metadata.Add(MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly));
            metadata.Add(ComponentMetadata.Bind.ValueAttribute, entry.ValueAttribute);
            metadata.Add(ComponentMetadata.Bind.ChangeAttribute, entry.ChangeAttribute);
            metadata.Add(ComponentMetadata.Bind.IsInvariantCulture, entry.IsInvariantCulture ? bool.TrueString : bool.FalseString);
            metadata.Add(ComponentMetadata.Bind.Format, entry.Format);

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
                metadata.Add(ComponentMetadata.Bind.TypeAttribute, entry.TypeAttribute);
            }

            metadata.Add(TypeName(entry.TypeName));
            metadata.Add(TypeNamespace(entry.TypeNamespace));
            metadata.Add(TypeNameIdentifier(entry.TypeNameIdentifier));

            builder.SetMetadata(metadata.Build());

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
                    a.SetMetadata(Attributes.IsDirectiveAttribute);
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
                    a.SetMetadata(Attributes.IsDirectiveAttribute);
                });

                rule.Attribute(a =>
                {
                    a.Name = $"{attributeName}:set";
                    a.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                    a.SetMetadata(Attributes.IsDirectiveAttribute);
                });
            });

            builder.BindAttribute(a =>
            {
                a.SetDocumentation(
                    DocumentationDescriptor.From(
                        DocumentationId.BindTagHelper_Element,
                        entry.ValueAttribute,
                        entry.ChangeAttribute));

                a.Name = attributeName;
                a.TypeName = typeof(object).FullName;

                a.SetMetadata(
                    IsDirectiveAttribute,
                    PropertyName(name));

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "format";
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Element_Format,
                            attributeName));

                    parameter.SetMetadata(PropertyName(formatName));
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "event";
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Element_Event,
                            attributeName));

                    parameter.SetMetadata(PropertyName(eventName));
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "culture";
                    parameter.TypeName = typeof(CultureInfo).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Culture);

                    parameter.SetMetadata(Parameters.Culture);
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "get";
                    parameter.TypeName = typeof(object).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Get);

                    parameter.SetMetadata(Parameters.Get);
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "set";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Set);

                    parameter.SetMetadata(Parameters.Set);
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "after";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_After);

                    parameter.SetMetadata(Parameters.After);
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

                attribute.SetMetadata(PropertyName(formatName));
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

                using var metadata = builder.GetMetadataBuilder(ComponentMetadata.Bind.RuntimeName);

                builder.DisplayName = tagHelper.DisplayName;
                builder.CaseSensitive = true;
                builder.SetDocumentation(
                    DocumentationDescriptor.From(
                        DocumentationId.BindTagHelper_Component,
                        valueAttribute.Name,
                        changeAttribute.Name));

                metadata.Add(SpecialKind(ComponentMetadata.Bind.TagHelperKind));
                metadata.Add(ComponentMetadata.Bind.ValueAttribute, valueAttribute.Name);
                metadata.Add(ComponentMetadata.Bind.ChangeAttribute, changeAttribute.Name);

                if (expressionAttribute != null)
                {
                    metadata.Add(ComponentMetadata.Bind.ExpressionAttribute, expressionAttribute.Name);
                }

                metadata.Add(TypeName(tagHelper.GetTypeName()));
                metadata.Add(TypeNamespace(tagHelper.GetTypeNamespace()));
                metadata.Add(TypeNameIdentifier(tagHelper.GetTypeNameIdentifier()));

                // Match the component and attribute name
                builder.TagMatchingRule(rule =>
                {
                    rule.TagName = tagHelper.TagMatchingRules.Single().TagName;
                    rule.Attribute(attribute =>
                    {
                        attribute.Name = "@bind-" + valueAttribute.Name;
                        attribute.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        attribute.SetMetadata(Attributes.IsDirectiveAttribute);
                    });
                });

                builder.TagMatchingRule(rule =>
                {
                    rule.TagName = tagHelper.TagMatchingRules.Single().TagName;
                    rule.Attribute(attribute =>
                    {
                        attribute.Name = "@bind-" + valueAttribute.Name + ":get";
                        attribute.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        attribute.SetMetadata(Attributes.IsDirectiveAttribute);
                    });
                    rule.Attribute(attribute =>
                    {
                        attribute.Name = "@bind-" + valueAttribute.Name + ":set";
                        attribute.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        attribute.SetMetadata(Attributes.IsDirectiveAttribute);
                    });
                });

                builder.BindAttribute(attribute =>
                {
                    attribute.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Component,
                            valueAttribute.Name,
                            changeAttribute.Name));

                    attribute.Name = "@bind-" + valueAttribute.Name;
                    attribute.TypeName = changeAttribute.TypeName;
                    attribute.IsEnum = valueAttribute.IsEnum;

                    attribute.SetMetadata(
                        PropertyName(valueAttribute.GetPropertyName()),
                        IsDirectiveAttribute);

                    attribute.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "get";
                        parameter.TypeName = typeof(object).FullName;
                        parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Get);

                        parameter.SetMetadata(Parameters.Get);
                    });

                    attribute.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "set";
                        parameter.TypeName = typeof(Delegate).FullName;
                        parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Set);

                        parameter.SetMetadata(Parameters.Set);
                    });

                    attribute.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "after";
                        parameter.TypeName = typeof(Delegate).FullName;
                        parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_After);

                        parameter.SetMetadata(Parameters.After);
                    });
                });

                if (tagHelper.IsComponentFullyQualifiedNameMatch())
                {
                    metadata.Add(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch);
                }

                builder.SetMetadata(metadata.Build());

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

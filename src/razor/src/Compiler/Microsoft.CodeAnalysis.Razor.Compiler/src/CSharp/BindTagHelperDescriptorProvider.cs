// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor;

// Run after the component tag helper provider, because we need to see the results.
internal sealed class BindTagHelperDescriptorProvider() : TagHelperDescriptorProviderBase(order: 1000)
{
    private static readonly Lazy<TagHelperDescriptor> s_fallbackBindTagHelper = new(CreateFallbackBindTagHelper);

    public override void Execute(TagHelperDescriptorProviderContext context, CancellationToken cancellationToken = default)
    {
        ArgHelper.ThrowIfNull(context);

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
        //      These mappings are provided by attributes that tell us what attributes, suffixes, and
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
        var compilation = context.Compilation;

        var bindMethods = compilation.GetTypeByMetadataName(ComponentsApi.BindConverter.FullTypeName);
        if (bindMethods == null)
        {
            // If we can't find BindConverter, then just bail. We won't be able to compile the
            // generated code anyway.
            return;
        }

        if (context.TargetAssembly is { } targetAssembly &&
            !SymbolEqualityComparer.Default.Equals(targetAssembly, bindMethods.ContainingAssembly))
        {
            return;
        }

        // Tag Helper definition for case #1. This is the most general case.
        context.Results.Add(s_fallbackBindTagHelper.Value);

        var bindElementAttribute = compilation.GetTypeByMetadataName(ComponentsApi.BindElementAttribute.FullTypeName);
        var bindInputElementAttribute = compilation.GetTypeByMetadataName(ComponentsApi.BindInputElementAttribute.FullTypeName);

        if (bindElementAttribute == null || bindInputElementAttribute == null)
        {
            // This won't likely happen, but just in case.
            return;
        }

        // We want to walk the compilation and its references, not the target symbol.
        var collector = new Collector(
            compilation, bindElementAttribute, bindInputElementAttribute);
        collector.Collect(context, cancellationToken);
    }

    private static TagHelperDescriptor CreateFallbackBindTagHelper()
    {
        using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
            TagHelperKind.Bind, "Bind", ComponentsApi.AssemblyName,
            out var builder);

        builder.SetTypeName(
            fullName: "Microsoft.AspNetCore.Components.Bind",
            typeNamespace: "Microsoft.AspNetCore.Components",
            typeNameIdentifier: "Bind");

        builder.CaseSensitive = true;
        builder.ClassifyAttributesOnly = true;
        builder.SetDocumentation(DocumentationDescriptor.BindTagHelper_Fallback);

        builder.SetMetadata(new BindMetadata() { IsFallback = true });

        builder.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.Attribute(attribute =>
            {
                attribute.Name = "@bind-";
                attribute.NameComparison = RequiredAttributeNameComparison.PrefixMatch;
                attribute.IsDirectiveAttribute = true;
            });
        });

        builder.BindAttribute(attribute =>
        {
            attribute.SetDocumentation(DocumentationDescriptor.BindTagHelper_Fallback);

            var attributeName = "@bind-...";
            attribute.Name = attributeName;
            attribute.AsDictionary("@bind-", typeof(object).FullName);
            attribute.IsDirectiveAttribute = true;

            attribute.PropertyName = "Bind";

            attribute.TypeName = "System.Collections.Generic.Dictionary<string, object>";

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "format";
                parameter.PropertyName = "Format";
                parameter.TypeName = typeof(string).FullName;
                parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Fallback_Format);
            });

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "event";
                parameter.PropertyName = "Event";
                parameter.TypeName = typeof(string).FullName;
                parameter.SetDocumentation(
                    DocumentationDescriptor.From(
                        DocumentationId.BindTagHelper_Fallback_Event, attributeName));
            });

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "culture";
                parameter.PropertyName = "Culture";
                parameter.TypeName = typeof(CultureInfo).FullName;
                parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Culture);
            });

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "get";
                parameter.PropertyName = "Get";
                parameter.TypeName = typeof(object).FullName;
                parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Get);
                parameter.BindAttributeGetSet = true;
            });

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "set";
                parameter.PropertyName = "Set";
                parameter.TypeName = typeof(Delegate).FullName;
                parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Set);
            });

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "after";
                parameter.PropertyName = "After";
                parameter.TypeName = typeof(Delegate).FullName;
                parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_After);
            });
        });

        return builder.Build();
    }

    private class Collector(
        Compilation compilation,
        INamedTypeSymbol bindElementAttribute,
        INamedTypeSymbol bindInputElementAttribute)
        : TagHelperCollector<Collector>(compilation, targetAssembly: null)
    {
        protected override bool IsCandidateType(INamedTypeSymbol types)
            => types.DeclaredAccessibility == Accessibility.Public &&
               types.Name == "BindAttributes";

        protected override void Collect(IAssemblySymbol assembly, ICollection<TagHelperDescriptor> results, CancellationToken cancellationToken)
        {
            // First, collect the initial set of tag helpers from this assembly. This calls
            // the Collect(INamedTypeSymbol, ...) overload below for cases #2 & #3.
            base.Collect(assembly, results, cancellationToken);

            // Then, for case #4 we look at the tag helpers that were already created corresponding to components
            // and pattern match on properties.
            using var componentBindTagHelpers = new PooledArrayBuilder<TagHelperDescriptor>(capacity: results.Count);

            foreach (var tagHelper in results)
            {
                cancellationToken.ThrowIfCancellationRequested();

                AddComponentBindTagHelpers(tagHelper, ref componentBindTagHelpers.AsRef());
            }

            foreach (var tagHelper in componentBindTagHelpers)
            {
                results.Add(tagHelper);
            }
        }

        protected override void Collect(
            INamedTypeSymbol type,
            ICollection<TagHelperDescriptor> results,
            CancellationToken cancellationToken)
        {
            // Not handling duplicates here for now since we're the primary ones extending this.
            // If we see users adding to the set of 'bind' constructs we will want to add deduplication
            // and potentially diagnostics.
            foreach (var attribute in type.GetAttributes())
            {
                var constructorArguments = attribute.ConstructorArguments;

                TagHelperDescriptor? tagHelper = null;

                // For case #2 & #3 we have a whole bunch of attribute entries on BindMethods that we can use
                // to data-drive the definitions of these tag helpers.

                // We need to check the constructor argument length here, because this can show up as 0
                // if the language service fails to initialize. This is an invalid case, so skip it.
                if (constructorArguments.Length == 4 && SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, bindElementAttribute))
                {
                    tagHelper = CreateElementBindTagHelper(
                        typeName: type.GetDefaultDisplayString(),
                        typeNamespace: type.ContainingNamespace.GetFullName(),
                        typeNameIdentifier: type.Name,
                        element: (string?)constructorArguments[0].Value,
                        typeAttribute: null,
                        suffix: (string?)constructorArguments[1].Value,
                        valueAttribute: (string?)constructorArguments[2].Value,
                        changeAttribute: (string?)constructorArguments[3].Value);
                }
                else if (constructorArguments.Length == 4 && SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, bindInputElementAttribute))
                {
                    tagHelper = CreateElementBindTagHelper(
                        typeName: type.GetDefaultDisplayString(),
                        typeNamespace: type.ContainingNamespace.GetFullName(),
                        typeNameIdentifier: type.Name,
                        element: "input",
                        typeAttribute: (string?)constructorArguments[0].Value,
                        suffix: (string?)constructorArguments[1].Value,
                        valueAttribute: (string?)constructorArguments[2].Value,
                        changeAttribute: (string?)constructorArguments[3].Value);
                }
                else if (constructorArguments.Length == 6 && SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, bindInputElementAttribute))
                {
                    tagHelper = CreateElementBindTagHelper(
                        typeName: type.GetDefaultDisplayString(),
                        typeNamespace: type.ContainingNamespace.GetFullName(),
                        typeNameIdentifier: type.Name,
                        element: "input",
                        typeAttribute: (string?)constructorArguments[0].Value,
                        suffix: (string?)constructorArguments[1].Value,
                        valueAttribute: (string?)constructorArguments[2].Value,
                        changeAttribute: (string?)constructorArguments[3].Value,
                        isInvariantCulture: (bool?)constructorArguments[4].Value ?? false,
                        format: (string?)constructorArguments[5].Value);
                }

                if (tagHelper is not null)
                {
                    results.Add(tagHelper);
                }
            }
        }

        private static TagHelperDescriptor CreateElementBindTagHelper(
            string typeName,
            string typeNamespace,
            string typeNameIdentifier,
            string? element,
            string? typeAttribute,
            string? suffix,
            string? valueAttribute,
            string? changeAttribute,
            bool isInvariantCulture = false,
            string? format = null)
        {
            string name, attributeName, formatName, formatAttributeName, eventName;

            if (suffix is { } s)
            {
                name = "Bind_" + s;
                attributeName = "@bind-" + s;
                formatName = "Format_" + s;
                formatAttributeName = "format-" + s;
                eventName = "Event_" + s;
            }
            else
            {
                name = "Bind";
                attributeName = "@bind";

                suffix = valueAttribute;
                formatName = "Format_" + suffix;
                formatAttributeName = "format-" + suffix;
                eventName = "Event_" + suffix;
            }

            using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
                TagHelperKind.Bind, name, ComponentsApi.AssemblyName,
                out var builder);

            builder.SetTypeName(typeName, typeNamespace, typeNameIdentifier);

            builder.CaseSensitive = true;
            builder.ClassifyAttributesOnly = true;
            builder.SetDocumentation(
                DocumentationDescriptor.From(
                    DocumentationId.BindTagHelper_Element,
                    valueAttribute,
                    changeAttribute));

            var metadata = new BindMetadata.Builder
            {
                ValueAttribute = valueAttribute,
                ChangeAttribute = changeAttribute,
                IsInvariantCulture = isInvariantCulture,
                Format = format
            };

            if (typeAttribute != null)
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
                metadata.TypeAttribute = typeAttribute;
            }

            builder.SetMetadata(metadata.Build());

            builder.TagMatchingRule(rule =>
            {
                rule.TagName = element;
                if (typeAttribute != null)
                {
                    rule.Attribute(a =>
                    {
                        a.Name = "type";
                        a.NameComparison = RequiredAttributeNameComparison.FullMatch;
                        a.Value = typeAttribute;
                        a.ValueComparison = RequiredAttributeValueComparison.FullMatch;
                    });
                }

                rule.Attribute(a =>
                {
                    a.Name = attributeName;
                    a.NameComparison = RequiredAttributeNameComparison.FullMatch;
                    a.IsDirectiveAttribute = true;
                });
            });

            builder.TagMatchingRule(rule =>
            {
                rule.TagName = element;
                if (typeAttribute != null)
                {
                    rule.Attribute(a =>
                    {
                        a.Name = "type";
                        a.NameComparison = RequiredAttributeNameComparison.FullMatch;
                        a.Value = typeAttribute;
                        a.ValueComparison = RequiredAttributeValueComparison.FullMatch;
                    });
                }

                rule.Attribute(a =>
                {
                    a.Name = $"{attributeName}:get";
                    a.NameComparison = RequiredAttributeNameComparison.FullMatch;
                    a.IsDirectiveAttribute = true;
                });

                rule.Attribute(a =>
                {
                    a.Name = $"{attributeName}:set";
                    a.NameComparison = RequiredAttributeNameComparison.FullMatch;
                    a.IsDirectiveAttribute = true;
                });
            });

            builder.BindAttribute(a =>
            {
                a.SetDocumentation(
                    DocumentationDescriptor.From(
                        DocumentationId.BindTagHelper_Element,
                        valueAttribute,
                        changeAttribute));

                a.Name = attributeName;
                a.TypeName = typeof(object).FullName;
                a.IsDirectiveAttribute = true;
                a.PropertyName = name;

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "format";
                    parameter.PropertyName = formatName;
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Element_Format,
                            attributeName));
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "event";
                    parameter.PropertyName = eventName;
                    parameter.TypeName = typeof(string).FullName;
                    parameter.SetDocumentation(
                        DocumentationDescriptor.From(
                            DocumentationId.BindTagHelper_Element_Event,
                            attributeName));
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "culture";
                    parameter.PropertyName = "Culture";
                    parameter.TypeName = typeof(CultureInfo).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Culture);
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "get";
                    parameter.PropertyName = "Get";
                    parameter.TypeName = typeof(object).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Get);
                    parameter.BindAttributeGetSet = true;
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "set";
                    parameter.PropertyName = "Set";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Set);
                });

                a.BindAttributeParameter(parameter =>
                {
                    parameter.Name = "after";
                    parameter.PropertyName = "After";
                    parameter.TypeName = typeof(Delegate).FullName;
                    parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_After);
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

                attribute.PropertyName = formatName;
            });

            return builder.Build();
        }

        private static void AddComponentBindTagHelpers(TagHelperDescriptor tagHelper, ref PooledArrayBuilder<TagHelperDescriptor> results)
        {
            if (tagHelper.Kind != TagHelperKind.Component)
            {
                return;
            }

            // We want to create a 'bind' tag helper everywhere we see a pair of properties like `Foo`, `FooChanged`
            // where `FooChanged` is a delegate and `Foo` is not.
            //
            // The easiest way to figure this out without a lot of backtracking is to look for `FooChanged` and then
            // try to find a matching "Foo".
            //
            // We also look for a corresponding FooExpression attribute, though its presence is optional.
            foreach (var changeAttribute in tagHelper.BoundAttributes)
            {
                if (!changeAttribute.Name.EndsWith("Changed", StringComparison.Ordinal) ||

                    // Allow the ValueChanged attribute to be a delegate or EventCallback<>.
                    //
                    // We assume that the Delegate or EventCallback<> has a matching type, and the C# compiler will help
                    // you figure figure it out if you did it wrongly.
                    (!changeAttribute.IsDelegateProperty() && !changeAttribute.IsEventCallbackProperty()))
                {
                    continue;
                }

                BoundAttributeDescriptor? valueAttribute = null;
                BoundAttributeDescriptor? expressionAttribute = null;
                var valueAttributeName = changeAttribute.Name[..^"Changed".Length];
                var expressionAttributeName = valueAttributeName + "Expression";
                foreach (var attribute in tagHelper.BoundAttributes)
                {
                    if (attribute.Name == valueAttributeName)
                    {
                        valueAttribute = attribute;
                    }

                    if (attribute.Name == expressionAttributeName)
                    {
                        expressionAttribute = attribute;
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
                    TagHelperKind.Bind, tagHelper.Name, tagHelper.AssemblyName,
                    out var builder);

                builder.SetTypeName(tagHelper.TypeNameObject);

                builder.DisplayName = tagHelper.DisplayName;
                builder.CaseSensitive = true;
                builder.SetDocumentation(
                    DocumentationDescriptor.From(
                        DocumentationId.BindTagHelper_Component,
                        valueAttribute.Name,
                        changeAttribute.Name));

                var metadata = new BindMetadata.Builder
                {
                    ValueAttribute = valueAttribute.Name,
                    ChangeAttribute = changeAttribute.Name
                };

                if (expressionAttribute != null)
                {
                    metadata.ExpressionAttribute = expressionAttribute.Name;
                }

                // Match the component and attribute name
                builder.TagMatchingRule(rule =>
                {
                    rule.TagName = tagHelper.TagMatchingRules.Single().TagName;
                    rule.Attribute(attribute =>
                    {
                        attribute.Name = "@bind-" + valueAttribute.Name;
                        attribute.NameComparison = RequiredAttributeNameComparison.FullMatch;
                        attribute.IsDirectiveAttribute = true;
                    });
                });

                builder.TagMatchingRule(rule =>
                {
                    rule.TagName = tagHelper.TagMatchingRules.Single().TagName;
                    rule.Attribute(attribute =>
                    {
                        attribute.Name = "@bind-" + valueAttribute.Name + ":get";
                        attribute.NameComparison = RequiredAttributeNameComparison.FullMatch;
                        attribute.IsDirectiveAttribute = true;
                    });
                    rule.Attribute(attribute =>
                    {
                        attribute.Name = "@bind-" + valueAttribute.Name + ":set";
                        attribute.NameComparison = RequiredAttributeNameComparison.FullMatch;
                        attribute.IsDirectiveAttribute = true;
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
                    attribute.ContainingType = valueAttribute.ContainingType;
                    attribute.IsDirectiveAttribute = true;
                    attribute.PropertyName = valueAttribute.PropertyName;

                    attribute.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "get";
                        parameter.PropertyName = "Get";
                        parameter.TypeName = typeof(object).FullName;
                        parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Get);
                        parameter.BindAttributeGetSet = true;
                    });

                    attribute.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "set";
                        parameter.PropertyName = "Set";
                        parameter.TypeName = typeof(Delegate).FullName;
                        parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_Set);
                    });

                    attribute.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "after";
                        parameter.PropertyName = "After";
                        parameter.TypeName = typeof(Delegate).FullName;
                        parameter.SetDocumentation(DocumentationDescriptor.BindTagHelper_Element_After);
                    });
                });

                if (tagHelper.IsFullyQualifiedNameMatch)
                {
                    builder.IsFullyQualifiedNameMatch = true;
                }

                builder.SetMetadata(metadata.Build());

                results.Add(builder.Build());
            }
        }
    }
}

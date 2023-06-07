// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.PooledObjects;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

internal class EventHandlerTagHelperDescriptorProvider : ITagHelperDescriptorProvider
{
    public int Order { get; set; }

    public RazorEngine Engine { get; set; }

    public void Execute(TagHelperDescriptorProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var compilation = context.GetCompilation();
        if (compilation == null)
        {
            return;
        }

        if (compilation.GetTypeByMetadataName(ComponentsApi.EventHandlerAttribute.FullTypeName) is not INamedTypeSymbol eventHandlerAttribute)
        {
            // If we can't find EventHandlerAttribute, then just bail. We won't discover anything.
            return;
        }

        var eventHandlerData = GetEventHandlerData(context, compilation, eventHandlerAttribute);

        foreach (var tagHelper in CreateEventHandlerTagHelpers(eventHandlerData))
        {
            context.Results.Add(tagHelper);
        }
    }

    private static ImmutableArray<EventHandlerData> GetEventHandlerData(TagHelperDescriptorProviderContext context, Compilation compilation, INamedTypeSymbol eventHandlerAttribute)
    {
        using var _ = ListPool<INamedTypeSymbol>.GetPooledObject(out var types);
        var visitor = new EventHandlerDataVisitor(types);

        var targetSymbol = context.Items.GetTargetSymbol();
        if (targetSymbol is not null)
        {
            visitor.Visit(targetSymbol);
        }
        else
        {
            visitor.Visit(compilation.Assembly.GlobalNamespace);
            foreach (var reference in compilation.References)
            {
                if (compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
                {
                    visitor.Visit(assembly.GlobalNamespace);
                }
            }
        }

        using var results = new PooledArrayBuilder<EventHandlerData>();

        foreach (var type in types)
        {
            // Create helper to delay computing display names for this type when we need them.
            var displayNames = new DisplayNameHelper(type);

            // Not handling duplicates here for now since we're the primary ones extending this.
            // If we see users adding to the set of event handler constructs we will want to add deduplication
            // and potentially diagnostics.
            foreach (var attribute in type.GetAttributes())
            {
                if (SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, eventHandlerAttribute))
                {
                    var enablePreventDefault = false;
                    var enableStopPropagation = false;
                    if (attribute.ConstructorArguments.Length == 4)
                    {
                        enablePreventDefault = (bool)attribute.ConstructorArguments[2].Value;
                        enableStopPropagation = (bool)attribute.ConstructorArguments[3].Value;
                    }

                    var (assemblyName, typeName, namespaceName) = displayNames.GetNames();
                    var constructorArguments = attribute.ConstructorArguments;

                    results.Add(new EventHandlerData(
                        assemblyName,
                        typeName,
                        namespaceName,
                        type.Name,
                        (string)constructorArguments[0].Value,
                        (INamedTypeSymbol)constructorArguments[1].Value,
                        enablePreventDefault,
                        enableStopPropagation));
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

    private static ImmutableArray<TagHelperDescriptor> CreateEventHandlerTagHelpers(ImmutableArray<EventHandlerData> data)
    {
        using var results = new PooledArrayBuilder<TagHelperDescriptor>();

        foreach (var entry in data)
        {
            var attributeName = "@" + entry.Attribute;
            var eventArgType = entry.EventArgsType.ToDisplayString();

            using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
                ComponentMetadata.EventHandler.TagHelperKind, entry.Attribute, ComponentsApi.AssemblyName,
                out var builder);

            builder.CaseSensitive = true;
            builder.SetDocumentation(
                DocumentationDescriptor.From(
                    DocumentationId.EventHandlerTagHelper,
                    attributeName,
                    eventArgType));

            builder.SetMetadata(
                SpecialKind(ComponentMetadata.EventHandler.TagHelperKind),
                new(ComponentMetadata.EventHandler.EventArgsType, eventArgType),
                MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
                RuntimeName(ComponentMetadata.EventHandler.RuntimeName),
                TypeName(entry.TypeName),
                TypeNamespace(entry.TypeNamespace),
                TypeNameIdentifier(entry.TypeNameIdentifier));

            builder.TagMatchingRule(rule =>
            {
                rule.TagName = "*";

                rule.Attribute(a =>
                {
                    a.Name = attributeName;
                    a.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                    a.SetMetadata(Attributes.IsDirectiveAttribute);
                });
            });

            if (entry.EnablePreventDefault)
            {
                builder.TagMatchingRule(rule =>
                {
                    rule.TagName = "*";

                    rule.Attribute(a =>
                    {
                        a.Name = attributeName + ":preventDefault";
                        a.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        a.SetMetadata(Attributes.IsDirectiveAttribute);
                    });
                });
            }

            if (entry.EnableStopPropagation)
            {
                builder.TagMatchingRule(rule =>
                {
                    rule.TagName = "*";

                    rule.Attribute(a =>
                    {
                        a.Name = attributeName + ":stopPropagation";
                        a.NameComparisonMode = RequiredAttributeDescriptor.NameComparisonMode.FullMatch;
                        a.SetMetadata(Attributes.IsDirectiveAttribute);
                    });
                });
            }

            builder.BindAttribute(a =>
            {
                a.SetDocumentation(
                    DocumentationDescriptor.From(
                        DocumentationId.EventHandlerTagHelper,
                        attributeName,
                        eventArgType));

                a.Name = attributeName;

                // We want event handler directive attributes to default to C# context.
                a.TypeName = $"Microsoft.AspNetCore.Components.EventCallback<{eventArgType}>";

                a.SetMetadata(
                    // Make this weakly typed (don't type check) - delegates have their own type-checking
                    // logic that we don't want to interfere with.
                    IsWeaklyTyped,
                    IsDirectiveAttribute,
                    PropertyName(entry.Attribute));

                if (entry.EnablePreventDefault)
                {
                    a.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "preventDefault";
                        parameter.TypeName = typeof(bool).FullName;
                        parameter.SetDocumentation(
                            DocumentationDescriptor.From(
                                DocumentationId.EventHandlerTagHelper_PreventDefault,
                                attributeName));

                        parameter.SetMetadata(Parameters.PreventDefault);
                    });
                }

                if (entry.EnableStopPropagation)
                {
                    a.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "stopPropagation";
                        parameter.TypeName = typeof(bool).FullName;
                        parameter.SetDocumentation(
                            DocumentationDescriptor.From(
                                DocumentationId.EventHandlerTagHelper_StopPropagation,
                                attributeName));

                        parameter.SetMetadata(Parameters.StopPropagation);
                    });
                }
            });

            results.Add(builder.Build());
        }

        return results.DrainToImmutable();
    }

    private readonly record struct EventHandlerData(
        string Assembly,
        string TypeName,
        string TypeNamespace,
        string TypeNameIdentifier,
        string Attribute,
        INamedTypeSymbol EventArgsType,
        bool EnablePreventDefault,
        bool EnableStopPropagation);

    private class EventHandlerDataVisitor : SymbolVisitor
    {
        private readonly List<INamedTypeSymbol> _results;

        public EventHandlerDataVisitor(List<INamedTypeSymbol> results)
        {
            _results = results;
        }

        public override void VisitNamedType(INamedTypeSymbol symbol)
        {
            if (symbol.Name == "EventHandlers" && symbol.DeclaredAccessibility == Accessibility.Public)
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

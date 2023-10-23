// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
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

        var targetSymbol = context.Items.GetTargetSymbol();

        var collector = new Collector(compilation, targetSymbol, eventHandlerAttribute);
        collector.Collect(context);
    }

    private class Collector(Compilation compilation, ISymbol targetSymbol, INamedTypeSymbol eventHandlerAttribute)
        : TagHelperCollector<Collector>(compilation, targetSymbol)
    {
        private readonly INamedTypeSymbol _eventHandlerAttribute = eventHandlerAttribute;

        protected override void Collect(ISymbol symbol, ICollection<TagHelperDescriptor> results)
        {
            using var _ = ListPool<INamedTypeSymbol>.GetPooledObject(out var types);
            var visitor = new EventHandlerDataVisitor(types);

            visitor.Visit(symbol);

            foreach (var type in types)
            {
                // Create helper to delay computing display names for this type when we need them.
                var displayNames = new DisplayNameHelper(type);

                // Not handling duplicates here for now since we're the primary ones extending this.
                // If we see users adding to the set of event handler constructs we will want to add deduplication
                // and potentially diagnostics.
                foreach (var attribute in type.GetAttributes())
                {
                    if (SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, _eventHandlerAttribute))
                    {
                        var enablePreventDefault = false;
                        var enableStopPropagation = false;
                        if (attribute.ConstructorArguments.Length == 4)
                        {
                            enablePreventDefault = (bool)attribute.ConstructorArguments[2].Value;
                            enableStopPropagation = (bool)attribute.ConstructorArguments[3].Value;
                        }

                        var (typeName, namespaceName) = displayNames.GetNames();
                        var constructorArguments = attribute.ConstructorArguments;

                        results.Add(CreateTagHelper(
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
        }

        /// <summary>
        ///  Helper to avoid computing various type-based names until necessary.
        /// </summary>
        private ref struct DisplayNameHelper(INamedTypeSymbol type)
        {
            private readonly INamedTypeSymbol _type = type;
            private (string Type, string Namespace)? _names;

            public (string Type, string Namespace) GetNames()
            {
                _names ??= (_type.ToDisplayString(), _type.ContainingNamespace.ToDisplayString());

                return _names.GetValueOrDefault();
            }
        }

        private static TagHelperDescriptor CreateTagHelper(
            string typeName,
            string typeNamespace,
            string typeNameIdentifier,
            string attribute,
            INamedTypeSymbol eventArgsType,
            bool enablePreventDefault,
            bool enableStopPropagation)
        {
            var attributeName = "@" + attribute;
            var eventArgType = eventArgsType.ToDisplayString();
            _ = TagHelperDescriptorBuilder.GetPooledInstance(
                ComponentMetadata.EventHandler.TagHelperKind, attribute, ComponentsApi.AssemblyName,
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
                TypeName(typeName),
                TypeNamespace(typeNamespace),
                TypeNameIdentifier(typeNameIdentifier));

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

            if (enablePreventDefault)
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

            if (enableStopPropagation)
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
                    PropertyName(attribute));

                if (enablePreventDefault)
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

                if (enableStopPropagation)
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

            return builder.Build();
        }

        private class EventHandlerDataVisitor(List<INamedTypeSymbol> results) : SymbolVisitor
        {
            private readonly List<INamedTypeSymbol> _results = results;

            public override void VisitNamedType(INamedTypeSymbol symbol)
            {
                if (symbol.DeclaredAccessibility == Accessibility.Public &&
                    symbol.Name == "EventHandlers")
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
                Visit(symbol.GlobalNamespace);
            }
        }
    }
}

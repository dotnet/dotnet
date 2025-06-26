// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.PooledObjects;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

internal sealed class EventHandlerTagHelperDescriptorProvider : TagHelperDescriptorProviderBase
{
    public override void Execute(TagHelperDescriptorProviderContext context)
    {
        ArgHelper.ThrowIfNull(context);

        var compilation = context.Compilation;

        if (compilation.GetTypeByMetadataName(ComponentsApi.EventHandlerAttribute.FullTypeName) is not INamedTypeSymbol eventHandlerAttribute)
        {
            // If we can't find EventHandlerAttribute, then just bail. We won't discover anything.
            return;
        }

        var targetSymbol = context.TargetSymbol;

        var collector = new Collector(compilation, targetSymbol, eventHandlerAttribute);
        collector.Collect(context);
    }

    private class Collector(Compilation compilation, ISymbol? targetSymbol, INamedTypeSymbol eventHandlerAttribute)
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
                        if (!AttributeArgs.TryGet(attribute, out var args))
                        {
                            // If this occurs, the [EventHandler] was defined incorrectly, so we can't create a tag helper.
                            continue;
                        }

                        var (typeName, namespaceName) = displayNames.GetNames();
                        results.Add(CreateTagHelper(typeName, namespaceName, type.Name, args));
                    }
                }
            }
        }

        private readonly record struct AttributeArgs(
            string Attribute,
            INamedTypeSymbol EventArgsType,
            bool EnableStopPropagation = false,
            bool EnablePreventDefault = false)
        {
            public static bool TryGet(AttributeData attribute, out AttributeArgs args)
            {
                // EventHandlerAttribute has two constructors:
                //
                // - EventHandlerAttribute(string attributeName, Type eventArgsType);
                // - EventHandlerAttribute(string attributeName, Type eventArgsType, bool enableStopPropagation, bool enablePreventDefault);

                var arguments = attribute.ConstructorArguments;

                return TryGetFromTwoArguments(arguments, out args) ||
                       TryGetFromFourArguments(arguments, out args);

                static bool TryGetFromTwoArguments(ImmutableArray<TypedConstant> arguments, out AttributeArgs args)
                {
                    // Ctor 1: EventHandlerAttribute(string attributeName, Type eventArgsType);

                    if (arguments is [
                        { Value: string attributeName },
                        { Value: INamedTypeSymbol eventArgsType }])
                    {
                        args = new(attributeName, eventArgsType);
                        return true;
                    }

                    args = default;
                    return false;
                }

                static bool TryGetFromFourArguments(ImmutableArray<TypedConstant> arguments, out AttributeArgs args)
                {
                    // Ctor 2: EventHandlerAttribute(string attributeName, Type eventArgsType, bool enableStopPropagation, bool enablePreventDefault);

                    // TODO: The enablePreventDefault and enableStopPropagation arguments are incorrectly swapped!
                    // However, they have been that way since the 4-argument constructor variant was introduced
                    // in https://github.com/dotnet/razor/commit/7635bba6ef2d3e6798d0846ceb96da6d5908e1b0.
                    // Fixing this is tracked be https://github.com/dotnet/razor/issues/10497

                    if (arguments is [
                        { Value: string attributeName },
                        { Value: INamedTypeSymbol eventArgsType },
                        { Value: bool enablePreventDefault },
                        { Value: bool enableStopPropagation }])
                    {
                        args = new(attributeName, eventArgsType, enableStopPropagation, enablePreventDefault);
                        return true;
                    }

                    args = default;
                    return false;
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
                _names ??= (_type.ToDisplayString(),
                    _type.ContainingNamespace.ToDisplayString(SymbolExtensions.FullNameTypeDisplayFormat));

                return _names.GetValueOrDefault();
            }
        }

        private static TagHelperDescriptor CreateTagHelper(
            string typeName,
            string typeNamespace,
            string typeNameIdentifier,
            AttributeArgs args)
        {
            var (attribute, eventArgsType, enableStopPropagation, enablePreventDefault) = args;

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
                    a.NameComparison = RequiredAttributeNameComparison.FullMatch;
                    a.IsDirectiveAttribute = true;
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
                        a.NameComparison = RequiredAttributeNameComparison.FullMatch;
                        a.IsDirectiveAttribute = true;
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
                        a.NameComparison = RequiredAttributeNameComparison.FullMatch;
                        a.IsDirectiveAttribute = true;
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
                        parameter.PropertyName = "PreventDefault";
                        parameter.TypeName = typeof(bool).FullName;
                        parameter.SetDocumentation(
                            DocumentationDescriptor.From(
                                DocumentationId.EventHandlerTagHelper_PreventDefault,
                                attributeName));
                    });
                }

                if (enableStopPropagation)
                {
                    a.BindAttributeParameter(parameter =>
                    {
                        parameter.Name = "stopPropagation";
                        parameter.PropertyName = "StopPropagation";
                        parameter.TypeName = typeof(bool).FullName;
                        parameter.SetDocumentation(
                            DocumentationDescriptor.From(
                                DocumentationId.EventHandlerTagHelper_StopPropagation,
                                attributeName));
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

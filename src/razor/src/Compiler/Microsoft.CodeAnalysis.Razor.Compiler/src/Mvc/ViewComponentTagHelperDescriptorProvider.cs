// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions;

public sealed class ViewComponentTagHelperDescriptorProvider : TagHelperDescriptorProviderBase
{
    public override void Execute(TagHelperDescriptorProviderContext context)
    {
        ArgHelper.ThrowIfNull(context);

        var compilation = context.Compilation;

        var vcAttribute = compilation.GetTypeByMetadataName(ViewComponentTypes.ViewComponentAttribute);
        var nonVCAttribute = compilation.GetTypeByMetadataName(ViewComponentTypes.NonViewComponentAttribute);
        if (vcAttribute == null || vcAttribute.TypeKind == TypeKind.Error)
        {
            // Could not find attributes we care about in the compilation. Nothing to do.
            return;
        }

        var factory = new ViewComponentTagHelperDescriptorFactory(compilation);
        var collector = new Collector(compilation, factory, vcAttribute, nonVCAttribute);

        collector.Collect(context);
    }

    private class Collector(
        Compilation compilation,
        ViewComponentTagHelperDescriptorFactory factory,
        INamedTypeSymbol vcAttribute,
        INamedTypeSymbol? nonVCAttribute)
        : TagHelperCollector<Collector>(compilation, targetSymbol: null)
    {
        private readonly ViewComponentTagHelperDescriptorFactory _factory = factory;
        private readonly INamedTypeSymbol _vcAttribute = vcAttribute;
        private readonly INamedTypeSymbol? _nonVCAttribute = nonVCAttribute;

        protected override void Collect(ISymbol symbol, ICollection<TagHelperDescriptor> results)
        {
            using var _ = ListPool<INamedTypeSymbol>.GetPooledObject(out var types);
            var visitor = new ViewComponentTypeVisitor(_vcAttribute, _nonVCAttribute, types);

            visitor.Visit(symbol);

            foreach (var type in types)
            {
                var descriptor = _factory.CreateDescriptor(type);

                if (descriptor != null)
                {
                    results.Add(descriptor);
                }
            }
        }
    }
}

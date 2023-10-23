// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions.Version2_X;

public sealed class ViewComponentTagHelperDescriptorProvider : RazorEngineFeatureBase, ITagHelperDescriptorProvider
{
    public int Order { get; set; }

    public void Execute(TagHelperDescriptorProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var compilation = context.GetCompilation();
        if (compilation == null)
        {
            // No compilation, nothing to do.
            return;
        }

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
        INamedTypeSymbol nonVCAttribute)
        : TagHelperCollector<Collector>(compilation, targetSymbol: null)
    {
        private readonly ViewComponentTagHelperDescriptorFactory _factory = factory;
        private readonly INamedTypeSymbol _vcAttribute = vcAttribute;
        private readonly INamedTypeSymbol _nonVCAttribute = nonVCAttribute;

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

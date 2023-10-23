// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor;

public sealed class DefaultTagHelperDescriptorProvider : RazorEngineFeatureBase, ITagHelperDescriptorProvider
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

        var tagHelperTypeSymbol = compilation.GetTypeByMetadataName(TagHelperTypes.ITagHelper);
        if (tagHelperTypeSymbol == null || tagHelperTypeSymbol.TypeKind == TypeKind.Error)
        {
            // Could not find attributes we care about in the compilation. Nothing to do.
            return;
        }

        var targetSymbol = context.Items.GetTargetSymbol();
        var factory = new DefaultTagHelperDescriptorFactory(compilation, context.IncludeDocumentation, context.ExcludeHidden);
        var collector = new Collector(compilation, targetSymbol, factory, tagHelperTypeSymbol);
        collector.Collect(context);
    }

    private class Collector(
        Compilation compilation, ISymbol targetSymbol, DefaultTagHelperDescriptorFactory factory, INamedTypeSymbol tagHelperTypeSymbol)
        : TagHelperCollector<Collector>(compilation, targetSymbol)
    {
        private readonly DefaultTagHelperDescriptorFactory _factory = factory;
        private readonly INamedTypeSymbol _tagHelperTypeSymbol = tagHelperTypeSymbol;

        protected override void Collect(ISymbol symbol, ICollection<TagHelperDescriptor> results)
        {
            using var _ = ListPool<INamedTypeSymbol>.GetPooledObject(out var types);
            var visitor = new TagHelperTypeVisitor(_tagHelperTypeSymbol, types);

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

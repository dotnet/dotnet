// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions;

public sealed class ViewComponentTagHelperDescriptorProvider : TagHelperDescriptorProviderBase
{
    public override void Execute(TagHelperDescriptorProviderContext context, CancellationToken cancellationToken = default)
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

        collector.Collect(context, cancellationToken);
    }

    private class Collector(
        Compilation compilation,
        ViewComponentTagHelperDescriptorFactory factory,
        INamedTypeSymbol vcAttribute,
        INamedTypeSymbol? nonVCAttribute)
        : TagHelperCollector<Collector>(compilation, targetAssembly: null)
    {
        private readonly ViewComponentTagHelperDescriptorFactory _factory = factory;
        private readonly INamedTypeSymbol _vcAttribute = vcAttribute;
        private readonly INamedTypeSymbol? _nonVCAttribute = nonVCAttribute;

        protected override bool IncludeNestedTypes => true;

        protected override bool IsCandidateType(INamedTypeSymbol type)
            => type.IsViewComponent(_vcAttribute, _nonVCAttribute);

        protected override void Collect(
            INamedTypeSymbol type,
            ICollection<TagHelperDescriptor> results,
            CancellationToken cancellationToken)
        {
            var descriptor = _factory.CreateDescriptor(type);

            if (descriptor != null)
            {
                results.Add(descriptor);
            }
        }
    }
}

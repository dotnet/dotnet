// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor;

public sealed class DefaultTagHelperDescriptorProvider : TagHelperDescriptorProviderBase
{
    public override void Execute(TagHelperDescriptorProviderContext context, CancellationToken cancellationToken = default)
    {
        ArgHelper.ThrowIfNull(context);

        var compilation = context.Compilation;

        var iTagHelperType = compilation.GetTypeByMetadataName(TagHelperTypes.ITagHelper);
        if (iTagHelperType == null || iTagHelperType.TypeKind == TypeKind.Error)
        {
            // Could not find attributes we care about in the compilation. Nothing to do.
            return;
        }

        var targetAssembly = context.TargetAssembly;
        var factory = new DefaultTagHelperDescriptorFactory(context.IncludeDocumentation, context.ExcludeHidden);
        var collector = new Collector(compilation, targetAssembly, factory, iTagHelperType);
        collector.Collect(context, cancellationToken);
    }

    private class Collector(
        Compilation compilation,
        IAssemblySymbol? targetAssembly,
        DefaultTagHelperDescriptorFactory factory,
        INamedTypeSymbol iTagHelperType)
        : TagHelperCollector<Collector>(compilation, targetAssembly)
    {
        private readonly DefaultTagHelperDescriptorFactory _factory = factory;
        private readonly INamedTypeSymbol _iTagHelperType = iTagHelperType;

        protected override bool IsCandidateType(INamedTypeSymbol type)
            => type.IsTagHelper(_iTagHelperType);

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

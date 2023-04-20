// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
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

        var iTagHelper = compilation.GetTypeByMetadataName(TagHelperTypes.ITagHelper);
        if (iTagHelper == null || iTagHelper.TypeKind == TypeKind.Error)
        {
            // Could not find attributes we care about in the compilation. Nothing to do.
            return;
        }

        using var _ = ListPool<INamedTypeSymbol>.GetPooledObject(out var types);
        var visitor = new TagHelperTypeVisitor(iTagHelper, types);

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
                    if (IsTagHelperAssembly(assembly))
                    {
                        visitor.Visit(assembly.GlobalNamespace);
                    }
                }
            }
        }

        var factory = new DefaultTagHelperDescriptorFactory(compilation, context.IncludeDocumentation, context.ExcludeHidden);

        foreach (var type in types)
        {
            var descriptor = factory.CreateDescriptor(type);

            if (descriptor != null)
            {
                context.Results.Add(descriptor);
            }
        }
    }

    private static bool IsTagHelperAssembly(IAssemblySymbol assembly)
    {
        return assembly.Name != null && !assembly.Name.StartsWith("System.", StringComparison.Ordinal);
    }
}

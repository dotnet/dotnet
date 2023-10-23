// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract partial class TagHelperCollector<T>
    where T : TagHelperCollector<T>
{
    // This type is generic to ensure that each descendent gets its own instance of this field.
    private static readonly ConditionalWeakTable<IAssemblySymbol, Cache> s_perAssemblyCaches = new();

    private readonly Compilation _compilation;
    private readonly ISymbol? _targetSymbol;

    protected TagHelperCollector(Compilation compilation, ISymbol? targetSymbol)
    {
        _compilation = compilation;
        _targetSymbol = targetSymbol;
    }

    private static bool MightContainTagHelpers(IAssemblySymbol assembly)
    {
        // In order to contain tag helpers, components, or anything else we might want to find,
        // the assembly must start with "Microsoft.AspNetCore." or reference an assembly that
        // starts with "Microsoft.AspNetCore."

        return assembly.Name.StartsWith("Microsoft.AspNetCore.", StringComparison.Ordinal) ||
               assembly.Modules.First().ReferencedAssemblies.Any(
                   a => a.Name.StartsWith("Microsoft.AspNetCore.", StringComparison.Ordinal));
    }

    protected abstract void Collect(ISymbol symbol, ICollection<TagHelperDescriptor> results);

    public void Collect(TagHelperDescriptorProviderContext context)
    {
        if (_targetSymbol is not null)
        {
            Collect(_targetSymbol, context.Results);
        }
        else
        {
            Collect(_compilation.Assembly.GlobalNamespace, context.Results);

            foreach (var reference in _compilation.References)
            {
                if (_compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
                {
                    if (!MightContainTagHelpers(assembly))
                    {
                        continue;
                    }

                    // Check to see if we already have tag helpers cached for this assembly
                    // and use the cached versions if we do. Roslyn shares PE assembly symbols
                    // across compilations, so this ensures that we don't produce new tag helpers
                    // for the same assemblies over and over again.

                    var cache = s_perAssemblyCaches.GetOrCreateValue(assembly);

                    var includeDocumentation = context.IncludeDocumentation;
                    var excludeHidden = context.ExcludeHidden;

                    if (!cache.TryGet(includeDocumentation, excludeHidden, out var tagHelpers))
                    {
                        using var _ = ListPool<TagHelperDescriptor>.GetPooledObject(out var referenceTagHelpers);
                        Collect(assembly.GlobalNamespace, referenceTagHelpers);

                        tagHelpers = cache.Add(referenceTagHelpers.ToArrayOrEmpty(), includeDocumentation, excludeHidden);
                    }

                    foreach (var tagHelper in tagHelpers)
                    {
                        context.Results.Add(tagHelper);
                    }
                }
            }
        }
    }
}

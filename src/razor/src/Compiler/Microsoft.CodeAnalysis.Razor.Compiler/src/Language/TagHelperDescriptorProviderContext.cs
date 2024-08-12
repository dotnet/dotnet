// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed class TagHelperDescriptorProviderContext(Compilation compilation, ISymbol? targetSymbol, ICollection<TagHelperDescriptor> results)
{
    public Compilation Compilation { get; } = compilation;
    public ISymbol? TargetSymbol { get; } = targetSymbol;
    public ICollection<TagHelperDescriptor> Results { get; } = results;

    public bool ExcludeHidden { get; init; }
    public bool IncludeDocumentation { get; init; }

    public TagHelperDescriptorProviderContext(Compilation compilation, ISymbol? targetSymbol = null)
        : this(compilation, targetSymbol, results: [])
    {
    }

    public TagHelperDescriptorProviderContext(Compilation compilation, ICollection<TagHelperDescriptor> results)
        : this(compilation, targetSymbol: null, results)
    {
    }
}

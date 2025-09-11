// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace Microsoft.CodeAnalysis.Razor.Compiler.Language.Extensions;

internal static partial class INamedTypeSymbolExtensions
{
    private static readonly ConditionalWeakTable<INamedTypeSymbol, Cache> s_instance = new();

    public static bool IsViewComponent(this INamedTypeSymbol symbol, INamedTypeSymbol viewComponentAttribute, INamedTypeSymbol? nonViewComponentAttribute)
    {
        var cache = s_instance.GetValue(symbol, static s => new Cache(s));

        return cache.IsViewComponent(viewComponentAttribute, nonViewComponentAttribute);
    }
}

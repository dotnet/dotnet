// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor;

internal static class TagHelperTargetSymbolExtensions
{
    private static readonly object TargetSymbolKey = new object();

    public static ISymbol? GetTargetSymbol(this ItemCollection items)
    {
        if (items.Count == 0 || items[TargetSymbolKey] is not ISymbol symbol)
        {
            return null;
        }

        return symbol;
    }

    public static void SetTargetSymbol(this ItemCollection items, ISymbol symbol)
    {
        items[TargetSymbolKey] = symbol;
    }
}

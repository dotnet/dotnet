// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.CodeAnalysis.Razor;

internal static class SymbolExtensions
{
    internal static readonly SymbolDisplayFormat FullNameTypeDisplayFormat =
        SymbolDisplayFormat.FullyQualifiedFormat
            .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)
            .RemoveMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    internal static bool HasFullName(this ISymbol symbol, string fullName)
    {
        return symbol.ToDisplayString(FullNameTypeDisplayFormat).Equals(fullName, StringComparison.Ordinal);
    }
}

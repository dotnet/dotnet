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

    /// <summary>
    /// Checks if <paramref name="symbol"/> has the same fully qualified name as <paramref name="fullName"/>.
    /// </summary>
    /// <remarks>
    /// This is a *very* simple implementation that doesn't handle all cases. It's only intended to be used
    /// for a small set of known types. If you add a new type to be looked up by this function you must ensure
    /// the simple check works correctly for it. 
    /// </remarks>
    /// <returns>true if the type has the specified full name</returns>
    internal static bool HasFullName(this ISymbol symbol, string fullName)
    {
        // generally we expect this call to fail, so try and match on the simple name first, before
        // checking the fully qualified name, to prevent unnecessary string allocation from ToDisplayString 
        if (symbol.Name.Length > fullName.Length)
        {
            return false;
        }

        // the simple name doesn't include generic type params, so skip over them in the full name if they are present
        var fullNameEndIndex = fullName[^1] == '>' ? fullName.LastIndexOf('<') : fullName.Length;
        var fullNameStartIndex = fullName.LastIndexOf('.', fullNameEndIndex - 1) + 1;

        if (!fullName.AsSpan(fullNameStartIndex, fullNameEndIndex - fullNameStartIndex).Equals(symbol.Name.AsSpan(), StringComparison.Ordinal))
        {
            return false;
        }

        // The simple name matched, do a fully qualified comparison
        return symbol.ToDisplayString(FullNameTypeDisplayFormat).Equals(fullName, StringComparison.Ordinal);
    }
}

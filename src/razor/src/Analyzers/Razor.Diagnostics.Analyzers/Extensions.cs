// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;

namespace Razor.Diagnostics.Analyzers;

internal static class Extensions
{
    public static Diagnostic CreateDiagnostic(this IOperation operation, DiagnosticDescriptor rule)
    {
        var location = operation.Syntax.GetLocation();

        if (!location.IsInSource)
        {
            location = Location.None;
        }

        return Diagnostic.Create(rule, location);
    }
}

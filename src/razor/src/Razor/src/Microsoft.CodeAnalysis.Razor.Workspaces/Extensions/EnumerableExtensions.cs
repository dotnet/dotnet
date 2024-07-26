// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal static class EnumerableExtensions
{
    internal static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        where T : class
        => source.Where(static t => t is not null)!;
}

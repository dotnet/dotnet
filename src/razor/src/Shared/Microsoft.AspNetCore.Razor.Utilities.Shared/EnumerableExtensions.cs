// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace System.Collections.Generic;

internal static class EnumerableExtensions
{
    public static ImmutableArray<TResult> SelectAsArray<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
    {
        if (source is IReadOnlyList<T> list)
        {
            return list.SelectAsArray(selector);
        }

        return BuildResult(source, selector);

        static ImmutableArray<TResult> BuildResult(IEnumerable<T> items, Func<T, TResult> selector)
        {
            using var results = new PooledArrayBuilder<TResult>();

            foreach (var item in items)
            {
                results.Add(selector(item));
            }

            return results.DrainToImmutable();
        }
    }
}

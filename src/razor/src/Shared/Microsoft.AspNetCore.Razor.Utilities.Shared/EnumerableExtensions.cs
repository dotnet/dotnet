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

    public static bool TryGetCount<T>(this IEnumerable<T> sequence, out int count)
    {
#if NET6_0_OR_GREATER
        return Linq.Enumerable.TryGetNonEnumeratedCount(sequence, out count);
#else
        return TryGetCount<T>((IEnumerable)sequence, out count);
#endif
    }

    public static bool TryGetCount<T>(this IEnumerable sequence, out int count)
    {
        switch (sequence)
        {
            case ICollection collection:
                count = collection.Count;
                return true;

            case ICollection<T> collection:
                count = collection.Count;
                return true;

            case IReadOnlyCollection<T> collection:
                count = collection.Count;
                return true;
        }

        count = 0;
        return false;
    }
}

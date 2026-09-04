// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Helix.JobMonitor.Parallelism;

internal static class ParallelAsync
{
    public static async Task<IReadOnlyDictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(
        IEnumerable<TSource> source,
        int parallelism,
        Func<TSource, TKey> getKey,
        Func<TSource, CancellationToken, ValueTask<TValue>> getValue,
        IEqualityComparer<TKey> comparer,
        CancellationToken cancellationToken)
        where TKey : notnull
    {
        TSource[] items = [.. source];
        // Each worker owns one unique array slot. The dictionary is constructed only after
        // Parallel.ForEachAsync completes, so neither collection is mutated concurrently.
        var values = new TValue[items.Length];

        await Parallel.ForEachAsync(
            Enumerable.Range(0, items.Length),
            new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = parallelism,
            },
            async (index, token) => values[index] = await getValue(items[index], token));

        var result = new Dictionary<TKey, TValue>(items.Length, comparer);
        for (int i = 0; i < items.Length; i++)
        {
            result.Add(getKey(items[i]), values[i]);
        }

        return result;
    }
}

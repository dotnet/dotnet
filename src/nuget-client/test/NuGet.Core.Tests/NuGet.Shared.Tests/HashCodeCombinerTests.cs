// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NuGet.Shared.Tests;

public class HashCodeCombinerTests
{
    [Fact]
    public void AddDictionary_IsOrderIndependent()
    {
        var pairs1 = new[]
        {
            new KeyValuePair<int, string>(1, "A"),
            new KeyValuePair<int, string>(2, "B")
       };

        var pairs2 = new[]
        {
            new KeyValuePair<int, string>(100, "AAAA"),
            new KeyValuePair<int, string>(200, "BBB")
       };

        Assert.Equal(Compute(pairs1), Compute(pairs1.Reverse()));

        Assert.Equal(Compute(pairs2), Compute(pairs2.Reverse()));

        Assert.NotEqual(Compute(pairs1), Compute(pairs2));

        static int Compute<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            var combiner = new HashCodeCombiner();
            combiner.AddDictionary(pairs);
            return combiner.CombinedHash;
        }
    }

    [Fact]
    public void AddUnorderedSequence_NoComparer()
    {
        var values1 = new[] { 1, 2, 3, 4 };
        var values2 = new[] { 100, 200, 300, 400 };

        Assert.Equal(Compute(values1), Compute(values1.Reverse()));

        Assert.Equal(Compute(values2), Compute(values2.Reverse()));

        Assert.NotEqual(Compute(values1), Compute(values2));

        static int Compute<T>(IEnumerable<T> pairs)
        {
            var combiner = new HashCodeCombiner();
            combiner.AddUnorderedSequence(pairs);
            return combiner.CombinedHash;
        }
    }

    [Fact]
    public void AddUnorderedSequence_DuplicateItems()
    {
        // Internally we use XOR to get unordered behaviour in an efficient manner.
        // Because an XOR of a value with itself is a no-op, we want to ensure that
        // duplicate items still produce unique hashes.

        Assert.Equal(
            Compute(new[] { 1, 1 }),
            Compute(new[] { 1, 1 }));

        Assert.NotEqual(
            Compute(new[] { 1 }),
            Compute(new[] { 1, 1 }));
        Assert.NotEqual(
            Compute(new[] { 1, 1 }),
            Compute(new[] { 1, 1, 1 }));
        Assert.NotEqual(
            Compute(new[] { 1, 1 }),
            Compute(new[] { 1, 1, 1, 1 }));

        static int Compute<T>(IEnumerable<T> pairs)
        {
            var combiner = new HashCodeCombiner();
            combiner.AddUnorderedSequence(pairs);
            return combiner.CombinedHash;
        }
    }

    [Fact]
    public void AddUnorderedSequence_CustomComparer()
    {
        var valuesUpper = new[] { "A", "B", "C", "D" };
        var valuesLower = new[] { "a", "b", "c", "d" };

        Assert.Equal(
            Compute(valuesUpper, StringComparer.Ordinal),
            Compute(valuesUpper.Reverse(), StringComparer.Ordinal));

        Assert.Equal(
            Compute(valuesUpper, StringComparer.OrdinalIgnoreCase),
            Compute(valuesUpper.Reverse(), StringComparer.OrdinalIgnoreCase));

        Assert.Equal(
            Compute(valuesLower, StringComparer.OrdinalIgnoreCase),
            Compute(valuesUpper.Reverse(), StringComparer.OrdinalIgnoreCase));

        Assert.NotEqual(
            Compute(valuesUpper, StringComparer.Ordinal),
            Compute(valuesLower, StringComparer.Ordinal));

        static int Compute<T>(IEnumerable<T> pairs, IEqualityComparer<T> comparer)
        {
            var combiner = new HashCodeCombiner();
            combiner.AddUnorderedSequence(pairs, comparer);
            return combiner.CombinedHash;
        }
    }
}

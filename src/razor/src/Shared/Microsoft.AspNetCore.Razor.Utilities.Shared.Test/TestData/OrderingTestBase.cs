// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test.TestData;

public abstract class OrderingTestBase<TOrderCollection, TOrderByCollection, TCaseConverter>
    where TOrderCollection : IEnumerable<int>
    where TOrderByCollection : IEnumerable<ValueHolder<int>>
    where TCaseConverter : IOrderingCaseConverter<TOrderCollection, TOrderByCollection>, new()
{
    private static readonly TheoryData<TOrderCollection, ImmutableArray<int>> s_orderTestData = [];
    private static readonly TheoryData<TOrderCollection, ImmutableArray<int>> s_orderTestData_OddBeforeEven = [];
    private static readonly TheoryData<TOrderCollection, ImmutableArray<int>> s_orderDescendingTestData = [];
    private static readonly TheoryData<TOrderCollection, ImmutableArray<int>> s_orderDescendingTestData_OddBeforeEven = [];
    private static readonly TheoryData<TOrderByCollection, ImmutableArray<ValueHolder<int>>> s_orderByTestData = [];
    private static readonly TheoryData<TOrderByCollection, ImmutableArray<ValueHolder<int>>> s_orderByTestData_OddBeforeEven = [];
    private static readonly TheoryData<TOrderByCollection, ImmutableArray<ValueHolder<int>>> s_orderByDescendingTestData = [];
    private static readonly TheoryData<TOrderByCollection, ImmutableArray<ValueHolder<int>>> s_orderByDescendingTestData_OddBeforeEven = [];

    protected static void AddCase(TOrderCollection collection)
    {
        s_orderTestData.Add(collection, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
        s_orderTestData_OddBeforeEven.Add(collection, [1, 3, 5, 7, 9, 2, 4, 6, 8, 10]);
        s_orderDescendingTestData.Add(collection, [10, 9, 8, 7, 6, 5, 4, 3, 2, 1]);
        s_orderDescendingTestData_OddBeforeEven.Add(collection, [10, 8, 6, 4, 2, 9, 7, 5, 3, 1]);
    }

    protected static void AddCase(TOrderByCollection collection)
    {
        s_orderByTestData.Add(collection, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
        s_orderByTestData_OddBeforeEven.Add(collection, [1, 3, 5, 7, 9, 2, 4, 6, 8, 10]);
        s_orderByDescendingTestData.Add(collection, [10, 9, 8, 7, 6, 5, 4, 3, 2, 1]);
        s_orderByDescendingTestData_OddBeforeEven.Add(collection, [10, 8, 6, 4, 2, 9, 7, 5, 3, 1]);
    }

    static OrderingTestBase()
    {
        var converter = new TCaseConverter();

        AddCase(converter.ConvertOrderCase([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]));
        AddCase(converter.ConvertOrderCase([10, 9, 8, 7, 6, 5, 4, 3, 2, 1]));
        AddCase(converter.ConvertOrderCase([1, 3, 5, 7, 9, 2, 4, 6, 8, 10]));
        AddCase(converter.ConvertOrderCase([2, 5, 8, 1, 3, 9, 7, 4, 10, 6]));
        AddCase(converter.ConvertOrderCase([6, 10, 4, 7, 9, 3, 1, 8, 5, 2]));

        AddCase(converter.ConvertOrderByCase([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]));
        AddCase(converter.ConvertOrderByCase([10, 9, 8, 7, 6, 5, 4, 3, 2, 1]));
        AddCase(converter.ConvertOrderByCase([1, 3, 5, 7, 9, 2, 4, 6, 8, 10]));
        AddCase(converter.ConvertOrderByCase([2, 5, 8, 1, 3, 9, 7, 4, 10, 6]));
        AddCase(converter.ConvertOrderByCase([6, 10, 4, 7, 9, 3, 1, 8, 5, 2]));
    }

    protected static Comparison<int> OddBeforeEven
        => (x, y) => (x % 2 != 0, y % 2 != 0) switch
        {
            (true, false) => -1,
            (false, true) => 1,
            _ => x.CompareTo(y)
        };

    public static TheoryData<TOrderCollection, ImmutableArray<int>> OrderTestData => s_orderTestData;
    public static TheoryData<TOrderCollection, ImmutableArray<int>> OrderTestData_OddBeforeEven => s_orderTestData_OddBeforeEven;
    public static TheoryData<TOrderCollection, ImmutableArray<int>> OrderDescendingTestData => s_orderDescendingTestData;
    public static TheoryData<TOrderCollection, ImmutableArray<int>> OrderDescendingTestData_OddBeforeEven => s_orderDescendingTestData_OddBeforeEven;
    public static TheoryData<TOrderByCollection, ImmutableArray<ValueHolder<int>>> OrderByTestData => s_orderByTestData;
    public static TheoryData<TOrderByCollection, ImmutableArray<ValueHolder<int>>> OrderByTestData_OddBeforeEven => s_orderByTestData_OddBeforeEven;
    public static TheoryData<TOrderByCollection, ImmutableArray<ValueHolder<int>>> OrderByDescendingTestData => s_orderByDescendingTestData;
    public static TheoryData<TOrderByCollection, ImmutableArray<ValueHolder<int>>> OrderByDescendingTestData_OddBeforeEven => s_orderByDescendingTestData_OddBeforeEven;

    protected void AssertEqual<T>(ImmutableArray<T> result, ImmutableArray<T> expected)
    {
        Assert.Equal<T>(result, expected);
    }

    protected void AssertEqual<T>(ImmutableArray<T> result, ImmutableArray<T> expected, IEqualityComparer<T> comparer)
    {
        Assert.Equal(result, expected, comparer);
    }
}

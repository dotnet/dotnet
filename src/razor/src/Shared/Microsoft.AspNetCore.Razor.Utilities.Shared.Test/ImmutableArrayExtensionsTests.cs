// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test;

public class ImmutableArrayExtensionsTests
{
    [Fact]
    public void GetMostRecentUniqueItems()
    {
        ImmutableArray<string> items =
        [
            "Hello",
            "HELLO",
            "HeLlO",
            new string([',', ' ']),
            new string([',', ' ']),
            "World",
            "WORLD",
            "WoRlD"
        ];

        var mostRecent = items.GetMostRecentUniqueItems(StringComparer.OrdinalIgnoreCase);

        Assert.Collection(mostRecent,
            s => Assert.Equal("HeLlO", s),
            s =>
            {
                // make sure it's the most recent ", "
                Assert.NotSame(items[3], s);
                Assert.Same(items[4], s);
            },
            s => Assert.Equal("WoRlD", s));
    }

    private static Comparison<int> OddBeforeEven
        => (x, y) => (x % 2 != 0, y % 2 != 0) switch
        {
            (true, false) => -1,
            (false, true) => 1,
            _ => x.CompareTo(y)
        };

    public readonly record struct ValueHolder(int Value)
    {
        public static implicit operator ValueHolder(int value)
            => new(value);
    }

    public static TheoryData<ImmutableArray<int>, ImmutableArray<int>> OrderTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        };

    public static TheoryData<ImmutableArray<int>, ImmutableArray<int>> OrderTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
        };

    public static TheoryData<ImmutableArray<int>, ImmutableArray<int>> OrderDescendingTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
        };

    public static TheoryData<ImmutableArray<int>, ImmutableArray<int>> OrderDescendingTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
        };

    public static TheoryData<ImmutableArray<ValueHolder>, ImmutableArray<ValueHolder>> OrderByTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        };

    public static TheoryData<ImmutableArray<ValueHolder>, ImmutableArray<ValueHolder>> OrderByTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
        };

    public static TheoryData<ImmutableArray<ValueHolder>, ImmutableArray<ValueHolder>> OrderByDescendingTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
        };

    public static TheoryData<ImmutableArray<ValueHolder>, ImmutableArray<ValueHolder>> OrderByDescendingTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
        };

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void OrderAsArray(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void OrderAsArray_ReadOnlyList(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var readOnlyList = (IReadOnlyList<int>)data;
        var sorted = readOnlyList.OrderAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_ReadOnlyList_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var readOnlyList = (IReadOnlyList<int>)data;
        var sorted = readOnlyList.OrderAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray_ReadOnlyList(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var readOnlyList = (IReadOnlyList<int>)data;
        var sorted = readOnlyList.OrderDescendingAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_ReadOnlyList_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var readOnlyList = (IReadOnlyList<int>)data;
        var sorted = readOnlyList.OrderDescendingAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray_ReadOnlyList(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var readOnlyList = (IReadOnlyList<ValueHolder>)data;
        var sorted = readOnlyList.OrderByAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_ReadOnlyList_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var readOnlyList = (IReadOnlyList<ValueHolder>)data;
        var sorted = readOnlyList.OrderByAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray_ReadOnlyList(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var readOnlyList = (IReadOnlyList<ValueHolder>)data;
        var sorted = readOnlyList.OrderByDescendingAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_ReadOnlyList_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var readOnlyList = (IReadOnlyList<ValueHolder>)data;
        var sorted = readOnlyList.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void OrderAsArray_Enumerable(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_Enumerable_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray_Enumerable(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderDescendingAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_Enumerable_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderDescendingAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray_Enumerable(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var enumerable = (IEnumerable<ValueHolder>)data;
        var sorted = enumerable.OrderByAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_Enumerable_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var enumerable = (IEnumerable<ValueHolder>)data;
        var sorted = enumerable.OrderByAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray_Enumerable(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var enumerable = (IEnumerable<ValueHolder>)data;
        var sorted = enumerable.OrderByDescendingAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_Enumerable_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var enumerable = (IEnumerable<ValueHolder>)data;
        var sorted = enumerable.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void ToImmutableOrdered(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrdered();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void ToImmutableOrdered_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrdered(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void ToImmutableOrderedDescending(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedDescending();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void ToImmutableOrderedDescending_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedDescending(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void ToImmutableOrderedBy(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedBy(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void ToImmutableOrderedBy_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedBy(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void ToImmutableOrderedByDescending(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedByDescending(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void ToImmutableOrderedByDescending_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedByDescending(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void DrainToImmutableOrdered(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrdered();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void DrainToImmutableOrdered_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrdered(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void DrainToImmutableOrderedDescending(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedDescending();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void DrainToImmutableOrderedDescending_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedDescending(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void DrainToImmutableOrderedBy(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedBy(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void DrainToImmutableOrderedBy_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedBy(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void DrainToImmutableOrderedByDescending(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedByDescending(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void DrainToImmutableOrderedByDescending_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedByDescending(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Fact]
    public void OrderAsArray_EmptyArrayReturnsSameArray()
    {
        var array = ImmutableCollectionsMarshal.AsArray(ImmutableArray<int>.Empty);
        var immutableArray = ImmutableArray<int>.Empty;

        immutableArray = immutableArray.OrderAsArray();
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderAsArray_SingleElementArrayReturnsSameArray()
    {
        var array = new int[] { 42 };
        var immutableArray = ImmutableCollectionsMarshal.AsImmutableArray(array);

        immutableArray = immutableArray.OrderAsArray();
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderAsArray_SortedArrayReturnsSameArray()
    {
        var values = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var immutableArray = ImmutableCollectionsMarshal.AsImmutableArray(values);

        immutableArray = immutableArray.OrderAsArray();
        Assert.Same(values, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderDescendingAsArray_EmptyArrayReturnsSameArray()
    {
        var array = ImmutableCollectionsMarshal.AsArray(ImmutableArray<int>.Empty);
        var immutableArray = ImmutableArray<int>.Empty;

        immutableArray = immutableArray.OrderDescendingAsArray();
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderDescendingAsArray_SingleElementArrayReturnsSameArray()
    {
        var array = new int[] { 42 };
        var immutableArray = ImmutableCollectionsMarshal.AsImmutableArray(array);

        immutableArray = immutableArray.OrderDescendingAsArray();
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderDescendingAsArray_SortedArrayReturnsSameArray()
    {
        var values = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        var presortedArray = ImmutableCollectionsMarshal.AsImmutableArray(values);

        presortedArray = presortedArray.OrderDescendingAsArray();
        Assert.Same(values, ImmutableCollectionsMarshal.AsArray(presortedArray));
    }

    [Fact]
    public void OrderByAsArray_EmptyArrayReturnsSameArray()
    {
        var array = ImmutableCollectionsMarshal.AsArray(ImmutableArray<ValueHolder>.Empty);
        var immutableArray = ImmutableArray<ValueHolder>.Empty;

        immutableArray = immutableArray.OrderByAsArray(static x => x.Value);
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderByAsArray_SingleElementArrayReturnsSameArray()
    {
        var array = new ValueHolder[] { 42 };
        var immutableArray = ImmutableCollectionsMarshal.AsImmutableArray(array);

        immutableArray = immutableArray.OrderByAsArray(static x => x.Value);
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderByAsArray_SortedArrayReturnsSameArray()
    {
        var values = new ValueHolder[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var presortedArray = ImmutableCollectionsMarshal.AsImmutableArray(values);

        presortedArray = presortedArray.OrderByAsArray(static x => x.Value);
        Assert.Same(values, ImmutableCollectionsMarshal.AsArray(presortedArray));
    }

    [Fact]
    public void OrderByDescendingAsArray_EmptyArrayReturnsSameArray()
    {
        var array = ImmutableCollectionsMarshal.AsArray(ImmutableArray<ValueHolder>.Empty);
        var immutableArray = ImmutableArray<ValueHolder>.Empty;

        immutableArray = immutableArray.OrderByDescendingAsArray(static x => x.Value);
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderByDescendingAsArray_SingleElementArrayReturnsSameArray()
    {
        var array = new ValueHolder[] { 42 };
        var immutableArray = ImmutableCollectionsMarshal.AsImmutableArray(array);

        immutableArray = immutableArray.OrderByDescendingAsArray(static x => x.Value);
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderByDescendingAsArray_SortedArrayReturnsSameArray()
    {
        var values = new ValueHolder[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        var presortedArray = ImmutableCollectionsMarshal.AsImmutableArray(values);

        presortedArray = presortedArray.OrderByDescendingAsArray(static x => x.Value);
        Assert.Same(values, ImmutableCollectionsMarshal.AsArray(presortedArray));
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void UnsafeOrder(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().Order();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void UnsafeOrder_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().Order(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void UnsafeOrderDescending(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderDescending();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void UnsafeOrderDescending_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderDescending(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void UnsafeOrderBy(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderBy(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void UnsafeOrderBy_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderBy(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void UnsafeOrderByDescending(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderByDescending(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void UnsafeOrderByDescending_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderByDescending(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

#if NET // Enumerable.Order(...) and Enumerable.OrderDescending(...) were introduced in .NET 7

    [Fact]
    public void OrderAsArray_IsStable()
    {
        OrderAndAssertStableSort(
            linqFunction: data => data.Order(),
            testFunction: data => data.OrderAsArray());
    }

    [Fact]
    public void OrderAsArray_Comparer_IsStable()
    {
        OrderAndAssertStableSort(
            linqFunction: data => data.Order(StringHolder.Comparer.Ordinal),
            testFunction: data => data.OrderAsArray(StringHolder.Comparer.Ordinal));

        OrderAndAssertStableSort(
            linqFunction: data => data.Order(StringHolder.Comparer.OrdinalIgnoreCase),
            testFunction: data => data.OrderAsArray(StringHolder.Comparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void OrderDescendingAsArray_IsStable()
    {
        OrderAndAssertStableSort(
            linqFunction: data => data.OrderDescending(),
            testFunction: data => data.OrderDescendingAsArray());
    }

    [Fact]
    public void OrderDescendingAsArray_Comparer_IsStable()
    {
        OrderAndAssertStableSort(
            linqFunction: data => data.OrderDescending(StringHolder.Comparer.Ordinal),
            testFunction: data => data.OrderDescendingAsArray(StringHolder.Comparer.Ordinal));

        OrderAndAssertStableSort(
            linqFunction: data => data.OrderDescending(StringHolder.Comparer.OrdinalIgnoreCase),
            testFunction: data => data.OrderDescendingAsArray(StringHolder.Comparer.OrdinalIgnoreCase));
    }

#endif

    [Fact]
    public void OrderByAsArray_IsStable()
    {
        OrderAndAssertStableSort(
            linqFunction: data => data.OrderBy(static x => x?.Text),
            testFunction: data => data.OrderByAsArray(static x => x?.Text));
    }

    [Fact]
    public void OrderByAsArray_Comparer_IsStable()
    {
        OrderAndAssertStableSort(
            linqFunction: data => data.OrderBy(static x => x?.Text, StringComparer.OrdinalIgnoreCase),
            testFunction: data => data.OrderByAsArray(static x => x?.Text, StringComparer.OrdinalIgnoreCase));

        OrderAndAssertStableSort(
            linqFunction: data => data.OrderBy(static x => x?.Text, StringComparer.OrdinalIgnoreCase),
            testFunction: data => data.OrderByAsArray(static x => x?.Text, StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void OrderByDescendingAsArray_IsStable()
    {
        OrderAndAssertStableSort(
            linqFunction: data => data.OrderByDescending(static x => x?.Text),
            testFunction: data => data.OrderByDescendingAsArray(static x => x?.Text));
    }

    [Fact]
    public void OrderByDescendingAsArray_Comparer_IsStable()
    {
        OrderAndAssertStableSort(
            linqFunction: data => data.OrderByDescending(static x => x?.Text, StringComparer.OrdinalIgnoreCase),
            testFunction: data => data.OrderByDescendingAsArray(static x => x?.Text, StringComparer.OrdinalIgnoreCase));

        OrderAndAssertStableSort(
            linqFunction: data => data.OrderByDescending(static x => x?.Text, StringComparer.OrdinalIgnoreCase),
            testFunction: data => data.OrderByDescendingAsArray(static x => x?.Text, StringComparer.OrdinalIgnoreCase));
    }

    private static void OrderAndAssertStableSort(
        Func<ImmutableArray<StringHolder?>, IEnumerable<StringHolder?>> linqFunction,
        Func<ImmutableArray<StringHolder?>, ImmutableArray<StringHolder?>> testFunction)
    {
        ImmutableArray<StringHolder?> data = [
            "All", "Your", "Base", "Are", "belong", null, "To", "Us",
            "all", "your", null, "Base", "are", "belong", "to", "us"];

        var expected = linqFunction(data);
        var actual = testFunction(data);

        Assert.Equal<StringHolder?>(expected, actual, ReferenceEquals);
    }
}

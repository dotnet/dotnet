// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
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

    public static TheoryData<ImmutableArray<int>, ImmutableArray<int>> OrderAsArrayData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        };

    [Theory]
    [MemberData(nameof(OrderAsArrayData))]
    public void OrderAsArray(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    public static TheoryData<ImmutableArray<int>, ImmutableArray<int>> OrderAsArray_OddBeforeEvenData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
        };

    [Theory]
    [MemberData(nameof(OrderAsArray_OddBeforeEvenData))]
    public void OrderAsArray_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    public static TheoryData<ImmutableArray<int>, ImmutableArray<int>> OrderDescendingAsArrayData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
        };

    [Theory]
    [MemberData(nameof(OrderAsArrayData))]
    public void OrderDescendingAsArray(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    public static TheoryData<ImmutableArray<int>, ImmutableArray<int>> OrderDescendingAsArray_OddBeforeEvenData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
        };

    [Theory]
    [MemberData(nameof(OrderDescendingAsArray_OddBeforeEvenData))]
    public void OrderDescendingAsArray_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    public readonly record struct ValueHolder(int Value)
    {
        public static implicit operator ValueHolder(int value)
            => new(value);
    }

    public static TheoryData<ImmutableArray<ValueHolder>, ImmutableArray<ValueHolder>> OrderByAsArrayData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        };

    [Theory]
    [MemberData(nameof(OrderByAsArrayData))]
    public void OrderByAsArray(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    public static TheoryData<ImmutableArray<ValueHolder>, ImmutableArray<ValueHolder>> OrderByAsArray_OddBeforeEvenData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
        };

    [Theory]
    [MemberData(nameof(OrderByAsArray_OddBeforeEvenData))]
    public void OrderByAsArray_OddBeforeEvent(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    public static TheoryData<ImmutableArray<ValueHolder>, ImmutableArray<ValueHolder>> OrderByDescendingAsArrayData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
        };

    [Theory]
    [MemberData(nameof(OrderByDescendingAsArrayData))]
    public void OrderByDescendingAsArray(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    public static TheoryData<ImmutableArray<ValueHolder>, ImmutableArray<ValueHolder>> OrderByDescendingAsArray_OddBeforeEvenData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
        };

    [Theory]
    [MemberData(nameof(OrderByDescendingAsArray_OddBeforeEvenData))]
    public void OrderByDescendingAsArray_OddBeforeEven(ImmutableArray<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
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
}

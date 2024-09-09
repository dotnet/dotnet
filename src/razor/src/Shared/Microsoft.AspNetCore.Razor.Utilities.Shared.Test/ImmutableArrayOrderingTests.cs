// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Razor.Utilities.Shared.Test.TestData;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test;

public class ImmutableArrayOrderingTests : ImmutableArrayOrderingTestBase
{
    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void OrderAsArray(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void OrderAsArray_ReadOnlyList(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var readOnlyList = (IReadOnlyList<int>)data;
        var sorted = readOnlyList.OrderAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_ReadOnlyList_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var readOnlyList = (IReadOnlyList<int>)data;
        var sorted = readOnlyList.OrderAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray_ReadOnlyList(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var readOnlyList = (IReadOnlyList<int>)data;
        var sorted = readOnlyList.OrderDescendingAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_ReadOnlyList_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var readOnlyList = (IReadOnlyList<int>)data;
        var sorted = readOnlyList.OrderDescendingAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray_ReadOnlyList(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var readOnlyList = (IReadOnlyList<ValueHolder<int>>)data;
        var sorted = readOnlyList.OrderByAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_ReadOnlyList_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var readOnlyList = (IReadOnlyList<ValueHolder<int>>)data;
        var sorted = readOnlyList.OrderByAsArray(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray_ReadOnlyList(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var readOnlyList = (IReadOnlyList<ValueHolder<int>>)data;
        var sorted = readOnlyList.OrderByDescendingAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_ReadOnlyList_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var readOnlyList = (IReadOnlyList<ValueHolder<int>>)data;
        var sorted = readOnlyList.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void OrderAsArray_Enumerable(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_Enumerable_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray_Enumerable(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderDescendingAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_Enumerable_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderDescendingAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray_Enumerable(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var enumerable = (IEnumerable<ValueHolder<int>>)data;
        var sorted = enumerable.OrderByAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_Enumerable_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var enumerable = (IEnumerable<ValueHolder<int>>)data;
        var sorted = enumerable.OrderByAsArray(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray_Enumerable(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var enumerable = (IEnumerable<ValueHolder<int>>)data;
        var sorted = enumerable.OrderByDescendingAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_Enumerable_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var enumerable = (IEnumerable<ValueHolder<int>>)data;
        var sorted = enumerable.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void ToImmutableOrdered(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrdered();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void ToImmutableOrdered_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrdered(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void ToImmutableOrderedDescending(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedDescending();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void ToImmutableOrderedDescending_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedDescending(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void ToImmutableOrderedBy(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedBy(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void ToImmutableOrderedBy_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedBy(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void ToImmutableOrderedByDescending(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedByDescending(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void ToImmutableOrderedByDescending_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.ToImmutableOrderedByDescending(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void DrainToImmutableOrdered(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrdered();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void DrainToImmutableOrdered_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrdered(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void DrainToImmutableOrderedDescending(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedDescending();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void DrainToImmutableOrderedDescending_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedDescending(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void DrainToImmutableOrderedBy(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedBy(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void DrainToImmutableOrderedBy_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedBy(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void DrainToImmutableOrderedByDescending(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedByDescending(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void DrainToImmutableOrderedByDescending_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builder = data.ToBuilder();
        var sorted = builder.DrainToImmutableOrderedByDescending(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
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
        var array = ImmutableCollectionsMarshal.AsArray(ImmutableArray<ValueHolder<int>>.Empty);
        var immutableArray = ImmutableArray<ValueHolder<int>>.Empty;

        immutableArray = immutableArray.OrderByAsArray(static x => x.Value);
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderByAsArray_SingleElementArrayReturnsSameArray()
    {
        var array = new ValueHolder<int>[] { 42 };
        var immutableArray = ImmutableCollectionsMarshal.AsImmutableArray(array);

        immutableArray = immutableArray.OrderByAsArray(static x => x.Value);
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderByAsArray_SortedArrayReturnsSameArray()
    {
        var values = new ValueHolder<int>[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var presortedArray = ImmutableCollectionsMarshal.AsImmutableArray(values);

        presortedArray = presortedArray.OrderByAsArray(static x => x.Value);
        Assert.Same(values, ImmutableCollectionsMarshal.AsArray(presortedArray));
    }

    [Fact]
    public void OrderByDescendingAsArray_EmptyArrayReturnsSameArray()
    {
        var array = ImmutableCollectionsMarshal.AsArray(ImmutableArray<ValueHolder<int>>.Empty);
        var immutableArray = ImmutableArray<ValueHolder<int>>.Empty;

        immutableArray = immutableArray.OrderByDescendingAsArray(static x => x.Value);
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderByDescendingAsArray_SingleElementArrayReturnsSameArray()
    {
        var array = new ValueHolder<int>[] { 42 };
        var immutableArray = ImmutableCollectionsMarshal.AsImmutableArray(array);

        immutableArray = immutableArray.OrderByDescendingAsArray(static x => x.Value);
        Assert.Same(array, ImmutableCollectionsMarshal.AsArray(immutableArray));
    }

    [Fact]
    public void OrderByDescendingAsArray_SortedArrayReturnsSameArray()
    {
        var values = new ValueHolder<int>[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
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
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void UnsafeOrder_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().Order(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void UnsafeOrderDescending(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderDescending();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void UnsafeOrderDescending_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderDescending(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void UnsafeOrderBy(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderBy(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void UnsafeOrderBy_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderBy(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void UnsafeOrderByDescending(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderByDescending(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void UnsafeOrderByDescending_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        // Clone array, since we're modifying it in-place.
        var sorted = ImmutableArray.Create(data.AsSpan());
        sorted.Unsafe().OrderByDescending(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
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

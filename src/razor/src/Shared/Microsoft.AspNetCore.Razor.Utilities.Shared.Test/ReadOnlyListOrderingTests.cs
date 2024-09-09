// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Utilities.Shared.Test.TestData;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test;

public class ReadOnlyListOrderingTests : ReadOnlyListOrderingTestBase
{
    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void OrderAsArray(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_OddBeforeEven(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_OddBeforeEven(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray(IReadOnlyList<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_OddBeforeEven(IReadOnlyList<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray(IReadOnlyList<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_OddBeforeEven(IReadOnlyList<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void OrderAsArray_Enumerable(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_Enumerable_OddBeforeEven(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray_Enumerable(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderDescendingAsArray();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_Enumerable_OddBeforeEven(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderDescendingAsArray(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray_Enumerable(IReadOnlyList<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var enumerable = (IEnumerable<ValueHolder<int>>)data;
        var sorted = enumerable.OrderByAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_Enumerable_OddBeforeEven(IReadOnlyList<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var enumerable = (IEnumerable<ValueHolder<int>>)data;
        var sorted = enumerable.OrderByAsArray(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray_Enumerable(IReadOnlyList<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var enumerable = (IEnumerable<ValueHolder<int>>)data;
        var sorted = enumerable.OrderByDescendingAsArray(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_Enumerable_OddBeforeEven(IReadOnlyList<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var enumerable = (IEnumerable<ValueHolder<int>>)data;
        var sorted = enumerable.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
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
        Func<IReadOnlyList<StringHolder?>, IEnumerable<StringHolder?>> linqFunction,
        Func<IReadOnlyList<StringHolder?>, ImmutableArray<StringHolder?>> testFunction)
    {
        IReadOnlyList<StringHolder?> data = [
            "All", "Your", "Base", "Are", "belong", null, "To", "Us",
            "all", "your", null, "Base", "are", "belong", "to", "us"];

        var expected = linqFunction(data);
        var actual = testFunction(data);

        Assert.Equal<StringHolder?>(expected, actual, ReferenceEquals);
    }
}

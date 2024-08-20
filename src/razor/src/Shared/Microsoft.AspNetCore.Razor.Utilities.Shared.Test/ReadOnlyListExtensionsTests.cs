// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;
using SR = Microsoft.AspNetCore.Razor.Utilities.Shared.Resources.SR;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test;

public class ReadOnlyListExtensionsTest
{
    private static Func<int, bool> IsEven => x => x % 2 == 0;
    private static Func<int, bool> IsOdd => x => x % 2 != 0;

    [Fact]
    public void Any()
    {
        var list = new List<int>();
        var readOnlyList = (IReadOnlyList<int>)list;

        Assert.False(readOnlyList.Any());

        list.Add(19);

        Assert.True(readOnlyList.Any());

        list.Add(23);

        Assert.True(readOnlyList.Any(IsOdd));

        // ... but no even numbers
        Assert.False(readOnlyList.Any(IsEven));
    }

    [Fact]
    public void All()
    {
        var list = new List<int>();
        var readOnlyList = (IReadOnlyList<int>)list;

        Assert.True(readOnlyList.All(IsEven));

        list.Add(19);

        Assert.False(readOnlyList.All(IsEven));

        list.Add(23);

        Assert.True(readOnlyList.All(IsOdd));

        list.Add(42);

        Assert.False(readOnlyList.All(IsOdd));
    }

    [Fact]
    public void FirstAndLast()
    {
        var list = new List<int>();
        var readOnlyList = (IReadOnlyList<int>)list;

        var exception1 = Assert.Throws<InvalidOperationException>(() => readOnlyList.First());
        Assert.Equal(SR.Contains_no_elements, exception1.Message);

        Assert.Equal(default, readOnlyList.FirstOrDefault());

        var exception2 = Assert.Throws<InvalidOperationException>(() => readOnlyList.Last());
        Assert.Equal(SR.Contains_no_elements, exception1.Message);

        Assert.Equal(default, readOnlyList.LastOrDefault());

        list.Add(19);

        Assert.Equal(19, readOnlyList.First());
        Assert.Equal(19, readOnlyList.FirstOrDefault());
        Assert.Equal(19, readOnlyList.Last());
        Assert.Equal(19, readOnlyList.LastOrDefault());

        list.Add(23);

        Assert.Equal(19, readOnlyList.First());
        Assert.Equal(19, readOnlyList.FirstOrDefault());
        Assert.Equal(23, readOnlyList.Last());
        Assert.Equal(23, readOnlyList.LastOrDefault());
    }

    [Fact]
    public void FirstAndLastWithPredicate()
    {
        var list = new List<int>();
        var readOnlyList = (IReadOnlyList<int>)list;

        var exception1 = Assert.Throws<InvalidOperationException>(() => readOnlyList.First(IsOdd));
        Assert.Equal(SR.Contains_no_matching_elements, exception1.Message);

        Assert.Equal(default, readOnlyList.FirstOrDefault(IsOdd));

        var exception2 = Assert.Throws<InvalidOperationException>(() => readOnlyList.Last(IsOdd));
        Assert.Equal(SR.Contains_no_matching_elements, exception2.Message);

        Assert.Equal(default, readOnlyList.LastOrDefault(IsOdd));

        list.Add(19);

        Assert.Equal(19, readOnlyList.First(IsOdd));
        Assert.Equal(19, readOnlyList.FirstOrDefault(IsOdd));
        Assert.Equal(19, readOnlyList.Last(IsOdd));
        Assert.Equal(19, readOnlyList.LastOrDefault(IsOdd));

        list.Add(23);

        Assert.Equal(19, readOnlyList.First(IsOdd));
        Assert.Equal(19, readOnlyList.FirstOrDefault(IsOdd));
        Assert.Equal(23, readOnlyList.Last(IsOdd));
        Assert.Equal(23, readOnlyList.LastOrDefault(IsOdd));

        var exception3 = Assert.Throws<InvalidOperationException>(() => readOnlyList.First(IsEven));
        Assert.Equal(SR.Contains_no_matching_elements, exception3.Message);

        Assert.Equal(default, readOnlyList.FirstOrDefault(IsEven));

        var exception4 = Assert.Throws<InvalidOperationException>(() => readOnlyList.Last(IsEven));
        Assert.Equal(SR.Contains_no_matching_elements, exception4.Message);

        Assert.Equal(default, readOnlyList.LastOrDefault(IsEven));

        list.Add(42);

        Assert.Equal(42, readOnlyList.First(IsEven));
        Assert.Equal(42, readOnlyList.FirstOrDefault(IsEven));
        Assert.Equal(42, readOnlyList.Last(IsEven));
        Assert.Equal(42, readOnlyList.LastOrDefault(IsEven));
    }

    [Fact]
    public void Single()
    {
        var list = new List<int>();
        var readOnlyList = (IReadOnlyList<int>)list;

        var exception1 = Assert.Throws<InvalidOperationException>(() => readOnlyList.Single());
        Assert.Equal(SR.Contains_no_elements, exception1.Message);
        Assert.Equal(default, readOnlyList.SingleOrDefault());

        list.Add(19);

        Assert.Equal(19, readOnlyList.Single());
        Assert.Equal(19, readOnlyList.SingleOrDefault());

        list.Add(23);

        var exception2 = Assert.Throws<InvalidOperationException>(() => readOnlyList.Single());
        Assert.Equal(SR.Contains_more_than_one_element, exception2.Message);
        var exception3 = Assert.Throws<InvalidOperationException>(() => readOnlyList.SingleOrDefault());
        Assert.Equal(SR.Contains_more_than_one_element, exception2.Message);
    }

    [Fact]
    public void SingleWithPredicate()
    {
        var list = new List<int>();
        var readOnlyList = (IReadOnlyList<int>)list;

        var exception1 = Assert.Throws<InvalidOperationException>(() => readOnlyList.Single(IsOdd));
        Assert.Equal(SR.Contains_no_matching_elements, exception1.Message);
        Assert.Equal(default, readOnlyList.SingleOrDefault(IsOdd));

        list.Add(19);

        Assert.Equal(19, readOnlyList.Single(IsOdd));
        Assert.Equal(19, readOnlyList.SingleOrDefault(IsOdd));

        list.Add(23);

        var exception2 = Assert.Throws<InvalidOperationException>(() => readOnlyList.Single(IsOdd));
        Assert.Equal(SR.Contains_more_than_one_matching_element, exception2.Message);
        var exception3 = Assert.Throws<InvalidOperationException>(() => readOnlyList.SingleOrDefault(IsOdd));
        Assert.Equal(SR.Contains_more_than_one_matching_element, exception2.Message);

        list.Add(42);

        Assert.Equal(42, readOnlyList.Single(IsEven));
        Assert.Equal(42, readOnlyList.SingleOrDefault(IsEven));
    }

    [Fact]
    public void CopyTo_ImmutableArray()
    {
        Span<int> source = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var immutableArray = ImmutableArray.Create(source);

        AssertCopyToCore(immutableArray);
    }

    [Fact]
    public void CopyTo_ImmutableArrayBuilder()
    {
        Span<int> source = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var builder = ImmutableArray.CreateBuilder<int>();
        builder.AddRange(source);

        AssertCopyToCore(builder);
    }

    [Fact]
    public void CopyTo_List()
    {
        IEnumerable<int> source = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var list = new List<int>(source);

        AssertCopyToCore(list);
    }

    [Fact]
    public void CopyTo_Array()
    {
        int[] array = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        AssertCopyToCore(array);
    }

    [Fact]
    public void CopyTo_CustomReadOnlyList()
    {
        CustomReadOnlyList custom = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        AssertCopyToCore(custom);
    }

    private static void AssertCopyToCore(IReadOnlyList<int> list)
    {
        var destination1 = new int[list.Count - 1];
        var exception = Assert.Throws<ArgumentException>(() => list.CopyTo(destination1.AsSpan()));
        Assert.StartsWith(SR.Destination_is_too_short, exception.Message);

        Span<int> destination2 = stackalloc int[list.Count];
        list.CopyTo(destination2);
        AssertElementsEqual(list, destination2);

        Span<int> destination3 = stackalloc int[list.Count + 1];
        list.CopyTo(destination3);
        AssertElementsEqual(list, destination3);

        static void AssertElementsEqual<T>(IReadOnlyList<T> list, ReadOnlySpan<T> span)
        {
            var count = list.Count;
            for (var i = 0; i < count; i++)
            {
                Assert.Equal(list[i], span[i]);
            }
        }
    }

    [CollectionBuilder(typeof(CustomReadOnlyList), "Create")]
    private sealed class CustomReadOnlyList(params ReadOnlySpan<int> values) : IReadOnlyList<int>
    {
        private readonly int[] _values = values.ToArray();

        public int this[int index] => _values[index];
        public int Count => _values.Length;

        public IEnumerator<int> GetEnumerator()
        {
            foreach (var value in _values)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static CustomReadOnlyList Create(ReadOnlySpan<int> span)
            => new(span);
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

    public static TheoryData<IReadOnlyList<int>, ImmutableArray<int>> OrderTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        };

    public static TheoryData<IReadOnlyList<int>, ImmutableArray<int>> OrderTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
        };

    public static TheoryData<IReadOnlyList<int>, ImmutableArray<int>> OrderDescendingTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
        };

    public static TheoryData<IReadOnlyList<int>, ImmutableArray<int>> OrderDescendingTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
        };

    public static TheoryData<IReadOnlyList<ValueHolder>, ImmutableArray<ValueHolder>> OrderByTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        };

    public static TheoryData<IReadOnlyList<ValueHolder>, ImmutableArray<ValueHolder>> OrderByTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
        };

    public static TheoryData<IReadOnlyList<ValueHolder>, ImmutableArray<ValueHolder>> OrderByDescendingTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
        };

    public static TheoryData<IReadOnlyList<ValueHolder>, ImmutableArray<ValueHolder>> OrderByDescendingTestData_OddBeforeEven
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
    public void OrderAsArray(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_OddBeforeEven(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_OddBeforeEven(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray(IReadOnlyList<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_OddBeforeEven(IReadOnlyList<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray(IReadOnlyList<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_OddBeforeEven(IReadOnlyList<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void OrderAsArray_Enumerable(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_Enumerable_OddBeforeEven(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray_Enumerable(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderDescendingAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_Enumerable_OddBeforeEven(IReadOnlyList<int> data, ImmutableArray<int> expected)
    {
        var enumerable = (IEnumerable<int>)data;
        var sorted = enumerable.OrderDescendingAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray_Enumerable(IReadOnlyList<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var enumerable = (IEnumerable<ValueHolder>)data;
        var sorted = enumerable.OrderByAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_Enumerable_OddBeforeEven(IReadOnlyList<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var enumerable = (IEnumerable<ValueHolder>)data;
        var sorted = enumerable.OrderByAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray_Enumerable(IReadOnlyList<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var enumerable = (IEnumerable<ValueHolder>)data;
        var sorted = enumerable.OrderByDescendingAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_Enumerable_OddBeforeEven(IReadOnlyList<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var enumerable = (IEnumerable<ValueHolder>)data;
        var sorted = enumerable.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
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

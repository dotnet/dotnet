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

public class EnumerableExtensionsTests
{
    [Fact]
    public void CopyTo_ImmutableArray()
    {
        Span<int> source = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var immutableArray = ImmutableArray.Create(source);

        AssertCopyToCore(immutableArray, immutableArray.Length);
    }

    [Fact]
    public void CopyTo_ImmutableArrayBuilder()
    {
        Span<int> source = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var builder = ImmutableArray.CreateBuilder<int>();
        builder.AddRange(source);

        AssertCopyToCore(builder, builder.Count);
    }

    [Fact]
    public void CopyTo_List()
    {
        IEnumerable<int> source = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var list = new List<int>(source);

        AssertCopyToCore(list, list.Count);
    }

    [Fact]
    public void CopyTo_HashSet()
    {
        IEnumerable<int> source = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var set = new HashSet<int>(source);

        AssertCopyToCore(set, set.Count);
    }

    [Fact]
    public void CopyTo_Array()
    {
        int[] array = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        AssertCopyToCore(array, array.Length);
    }

    [Fact]
    public void CopyTo_CustomEnumerable()
    {
        CustomEnumerable custom = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        AssertCopyToCore(custom, 10);
    }

    [Fact]
    public void CopyTo_CustomReadOnlyCollection()
    {
        CustomReadOnlyCollection custom = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        AssertCopyToCore(custom, 10);
    }

    private static void AssertCopyToCore(IEnumerable<int> sequence, int count)
    {
        var destination1 = new int[count - 1];
        var exception = Assert.Throws<ArgumentException>(() => sequence.CopyTo(destination1.AsSpan()));
        Assert.StartsWith(SR.Destination_is_too_short, exception.Message);

        Span<int> destination2 = stackalloc int[count];
        sequence.CopyTo(destination2);
        AssertElementsEqual(sequence, destination2);

        Span<int> destination3 = stackalloc int[count + 1];
        sequence.CopyTo(destination3);
        AssertElementsEqual(sequence, destination3);

        static void AssertElementsEqual<T>(IEnumerable<T> sequence, ReadOnlySpan<T> span)
        {
            var index = 0;

            foreach (var item in sequence)
            {
                Assert.Equal(item, span[index++]);
            }
        }
    }

    [CollectionBuilder(typeof(CustomEnumerable), "Create")]
    private sealed class CustomEnumerable(params ReadOnlySpan<int> values) : IEnumerable<int>
    {
        private readonly int[] _values = values.ToArray();

        public IEnumerator<int> GetEnumerator()
        {
            foreach (var value in _values)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public static CustomEnumerable Create(ReadOnlySpan<int> span)
            => new(span);
    }

    [CollectionBuilder(typeof(CustomReadOnlyCollection), "Create")]
    private sealed class CustomReadOnlyCollection(params ReadOnlySpan<int> values) : IReadOnlyCollection<int>
    {
        private readonly int[] _values = values.ToArray();

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

        public static CustomReadOnlyCollection Create(ReadOnlySpan<int> span)
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

    public static TheoryData<IEnumerable<int>, ImmutableArray<int>> OrderTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        };

    public static TheoryData<IEnumerable<int>, ImmutableArray<int>> OrderTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
        };

    public static TheoryData<IEnumerable<int>, ImmutableArray<int>> OrderDescendingTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
        };

    public static TheoryData<IEnumerable<int>, ImmutableArray<int>> OrderDescendingTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 8, 6, 4, 2, 9, 7, 5, 3, 1] },
        };

    public static TheoryData<IEnumerable<ValueHolder>, ImmutableArray<ValueHolder>> OrderByTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 2, 3, 4, 5, 6, 7, 8, 9, 10] },
        };

    public static TheoryData<IEnumerable<ValueHolder>, ImmutableArray<ValueHolder>> OrderByTestData_OddBeforeEven
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [1, 3, 5, 7, 9, 2, 4, 6, 8, 10] },
        };

    public static TheoryData<IEnumerable<ValueHolder>, ImmutableArray<ValueHolder>> OrderByDescendingTestData
        => new()
        {
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [10, 9, 8, 7, 6, 5, 4, 3, 2, 1], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [1, 3, 5, 7, 9, 2, 4, 6, 8, 10], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [2, 5, 8, 1, 3, 9, 7, 4, 10, 6], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
            { [6, 10, 4, 7, 9, 3, 1, 8, 5, 2], [10, 9, 8, 7, 6, 5, 4, 3, 2, 1] },
        };

    public static TheoryData<IEnumerable<ValueHolder>, ImmutableArray<ValueHolder>> OrderByDescendingTestData_OddBeforeEven
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
    public void OrderAsArray(IEnumerable<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void OrderAsArray_OddBeforeEven(IEnumerable<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void OrderDescendingAsArray(IEnumerable<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray();
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void OrderDescendingAsArray_OddBeforeEven(IEnumerable<int> data, ImmutableArray<int> expected)
    {
        var sorted = data.OrderDescendingAsArray(OddBeforeEven);
        Assert.Equal<int>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void OrderByAsArray(IEnumerable<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void OrderByAsArray_OddBeforeEven(IEnumerable<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByAsArray(static x => x.Value, OddBeforeEven);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void OrderByDescendingAsArray(IEnumerable<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value);
        Assert.Equal<ValueHolder>(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void OrderByDescendingAsArray_OddBeforeEven(IEnumerable<ValueHolder> data, ImmutableArray<ValueHolder> expected)
    {
        var sorted = data.OrderByDescendingAsArray(static x => x.Value, OddBeforeEven);
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
        Func<IEnumerable<StringHolder?>, IEnumerable<StringHolder?>> linqFunction,
        Func<IEnumerable<StringHolder?>, ImmutableArray<StringHolder?>> testFunction)
    {
        IEnumerable<StringHolder?> data = [
            "All", "Your", "Base", "Are", "belong", null, "To", "Us",
            "all", "your", null, "Base", "are", "belong", "to", "us"];

        var expected = linqFunction(data);
        var actual = testFunction(data);

        Assert.Equal<StringHolder?>(expected, actual, ReferenceEquals);
    }
}

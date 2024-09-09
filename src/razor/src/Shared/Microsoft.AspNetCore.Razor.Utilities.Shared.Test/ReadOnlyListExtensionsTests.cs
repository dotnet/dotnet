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
}

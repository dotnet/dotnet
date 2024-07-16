// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Xunit;
using SR = Microsoft.AspNetCore.Razor.Utilities.Shared.Resources.SR;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test.PooledObjects;

public class PooledArrayBuilderTests
{
    [Theory]
    [CombinatorialData]
    public void AddElements([CombinatorialRange(0, 8)] int count)
    {
        using var builder = new PooledArrayBuilder<int>();

        for (var i = 0; i < count; i++)
        {
            builder.Add(i);
        }

        for (var i = 0; i < count; i++)
        {
            Assert.Equal(i, builder[i]);
        }

        var result = builder.DrainToImmutable();

        for (var i = 0; i < count; i++)
        {
            Assert.Equal(i, result[i]);
        }
    }

    public static TheoryData<int, int> RemoveAtIndex_Data
    {
        get
        {
            var data = new TheoryData<int, int>();

            for (var count = 0; count < 8; count++)
            {
                for (var removeIndex = 0; removeIndex < 8; removeIndex++)
                {
                    if (removeIndex < count)
                    {
                        data.Add(count, removeIndex);
                    }
                }
            }

            return data;
        }
    }

    [Theory]
    [MemberData(nameof(RemoveAtIndex_Data))]
    public void RemoveAtIndex(int count, int removeIndex)
    {
        using var builder = new PooledArrayBuilder<int>();

        for (var i = 0; i < count; i++)
        {
            builder.Add(i);
        }

        var newCount = count;
        var newValue = removeIndex;

        // Now, remove each element at removeIndex.
        for (var i = removeIndex; i < builder.Count; i++)
        {
            builder.RemoveAt(removeIndex);
            newCount--;
            newValue++;

            Assert.Equal(newCount, builder.Count);

            // Check the values starting at removeIndex.
            for (var j = removeIndex; j < newCount; j++)
            {
                Assert.Equal(newValue + (j - removeIndex), builder[j]);
            }
        }
    }

    private static Func<int, bool> IsEven => x => x % 2 == 0;
    private static Func<int, bool> IsOdd => x => x % 2 != 0;

    [Fact]
    public void Any()
    {
        using var builder = new PooledArrayBuilder<int>();

        Assert.False(builder.Any());

        builder.Add(19);

        Assert.True(builder.Any());

        builder.Add(23);

        Assert.True(builder.Any(IsOdd));

        // ... but no even numbers
        Assert.False(builder.Any(IsEven));
    }

    [Fact]
    public void All()
    {
        using var builder = new PooledArrayBuilder<int>();

        Assert.True(builder.All(IsEven));

        builder.Add(19);

        Assert.False(builder.All(IsEven));

        builder.Add(23);

        Assert.True(builder.All(IsOdd));

        builder.Add(42);

        Assert.False(builder.All(IsOdd));
    }

    [Fact]
    public void FirstAndLast()
    {
        using var builder = new PooledArrayBuilder<int>();

        var exception1 = Assert.Throws<InvalidOperationException>(() => builder.First());
        Assert.Equal(SR.Contains_no_elements, exception1.Message);

        Assert.Equal(default, builder.FirstOrDefault());

        var exception2 = Assert.Throws<InvalidOperationException>(() => builder.Last());
        Assert.Equal(SR.Contains_no_elements, exception1.Message);

        Assert.Equal(default, builder.LastOrDefault());

        builder.Add(19);

        Assert.Equal(19, builder.First());
        Assert.Equal(19, builder.FirstOrDefault());
        Assert.Equal(19, builder.Last());
        Assert.Equal(19, builder.LastOrDefault());

        builder.Add(23);

        Assert.Equal(19, builder.First());
        Assert.Equal(19, builder.FirstOrDefault());
        Assert.Equal(23, builder.Last());
        Assert.Equal(23, builder.LastOrDefault());
    }

    [Fact]
    public void FirstAndLastWithPredicate()
    {
        using var builder = new PooledArrayBuilder<int>();

        var exception1 = Assert.Throws<InvalidOperationException>(() => builder.First(IsOdd));
        Assert.Equal(SR.Contains_no_matching_elements, exception1.Message);

        Assert.Equal(default, builder.FirstOrDefault(IsOdd));

        var exception2 = Assert.Throws<InvalidOperationException>(() => builder.Last(IsOdd));
        Assert.Equal(SR.Contains_no_matching_elements, exception2.Message);

        Assert.Equal(default, builder.LastOrDefault(IsOdd));

        builder.Add(19);

        Assert.Equal(19, builder.First(IsOdd));
        Assert.Equal(19, builder.FirstOrDefault(IsOdd));
        Assert.Equal(19, builder.Last(IsOdd));
        Assert.Equal(19, builder.LastOrDefault(IsOdd));

        builder.Add(23);

        Assert.Equal(19, builder.First(IsOdd));
        Assert.Equal(19, builder.FirstOrDefault(IsOdd));
        Assert.Equal(23, builder.Last(IsOdd));
        Assert.Equal(23, builder.LastOrDefault(IsOdd));

        var exception3 = Assert.Throws<InvalidOperationException>(() => builder.First(IsEven));
        Assert.Equal(SR.Contains_no_matching_elements, exception3.Message);

        Assert.Equal(default, builder.FirstOrDefault(IsEven));

        var exception4 = Assert.Throws<InvalidOperationException>(() => builder.Last(IsEven));
        Assert.Equal(SR.Contains_no_matching_elements, exception4.Message);

        Assert.Equal(default, builder.LastOrDefault(IsEven));

        builder.Add(42);

        Assert.Equal(42, builder.First(IsEven));
        Assert.Equal(42, builder.FirstOrDefault(IsEven));
        Assert.Equal(42, builder.Last(IsEven));
        Assert.Equal(42, builder.LastOrDefault(IsEven));
    }

    [Fact]
    public void Single()
    {
        using var builder = new PooledArrayBuilder<int>();

        var exception1 = Assert.Throws<InvalidOperationException>(() => builder.Single());
        Assert.Equal(SR.Contains_no_elements, exception1.Message);
        Assert.Equal(default, builder.SingleOrDefault());

        builder.Add(19);

        Assert.Equal(19, builder.Single());
        Assert.Equal(19, builder.SingleOrDefault());

        builder.Add(23);

        var exception2 = Assert.Throws<InvalidOperationException>(() => builder.Single());
        Assert.Equal(SR.Contains_more_than_one_element, exception2.Message);
        var exception3 = Assert.Throws<InvalidOperationException>(() => builder.SingleOrDefault());
        Assert.Equal(SR.Contains_more_than_one_element, exception2.Message);
    }

    [Fact]
    public void SingleWithPredicate()
    {
        using var builder = new PooledArrayBuilder<int>();

        var exception1 = Assert.Throws<InvalidOperationException>(() => builder.Single(IsOdd));
        Assert.Equal(SR.Contains_no_matching_elements, exception1.Message);
        Assert.Equal(default, builder.SingleOrDefault(IsOdd));

        builder.Add(19);

        Assert.Equal(19, builder.Single(IsOdd));
        Assert.Equal(19, builder.SingleOrDefault(IsOdd));

        builder.Add(23);

        var exception2 = Assert.Throws<InvalidOperationException>(() => builder.Single(IsOdd));
        Assert.Equal(SR.Contains_more_than_one_matching_element, exception2.Message);
        var exception3 = Assert.Throws<InvalidOperationException>(() => builder.SingleOrDefault(IsOdd));
        Assert.Equal(SR.Contains_more_than_one_matching_element, exception2.Message);

        builder.Add(42);

        Assert.Equal(42, builder.Single(IsEven));
        Assert.Equal(42, builder.SingleOrDefault(IsEven));
    }
}

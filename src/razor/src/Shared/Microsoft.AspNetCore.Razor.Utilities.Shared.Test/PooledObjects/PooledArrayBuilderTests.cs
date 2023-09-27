// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Xunit;

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

    public static IEnumerable<object[]> RemoveAtIndex_Data
    {
        get
        {
            for (var count = 0; count < 8; count++)
            {
                for (var removeIndex = 0; removeIndex < 8; removeIndex++)
                {
                    if (removeIndex < count)
                    {
                        yield return new object[] { count, removeIndex };
                    }
                }
            }
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
}

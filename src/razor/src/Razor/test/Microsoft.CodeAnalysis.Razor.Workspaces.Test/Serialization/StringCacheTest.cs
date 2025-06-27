// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

public class StringCacheTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void GetOrAdd_EquivalentStrings_RetrievesFirstReference()
    {
        // Arrange
        var cache = new StringCache();
        // String format to prevent them from being RefEqual
        var str1 = $"stuff {1}";
        var str2 = $"stuff {1}";
        // Sanity check that these aren't already equal
        Assert.False(ReferenceEquals(str1, str2));

        // Act
        // Force a collection
        _ = cache.GetOrAddValue(str1);
        GC.Collect();
        var result = cache.GetOrAddValue(str2);

        // Assert
        Assert.Same(result, str1);
        Assert.False(ReferenceEquals(result, str2));
    }

    [Fact]
    public void GetOrAdd_NullReturnsNull()
    {
        // Arrange
        var cache = new StringCache();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => cache.GetOrAddValue(null!));
    }

    [Fact]
    public void GetOrAdd_DisposesReleasedReferencesOnExpand()
    {
        // Arrange
        var cache = new StringCache(2);

        // Act
        StringArea();

        // Force a collection
        GC.Collect();
        var str1 = $"{1}";
        var result = cache.GetOrAddValue(str1);

        // Assert
        Assert.Equal(1, cache.ApproximateSize);
        Assert.Same(result, str1);

        void StringArea()
        {
            var first = $"{1}";
            var test = cache.GetOrAddValue(first);
            Assert.Same(first, test);
            Assert.Equal(1, cache.ApproximateSize);
            GC.KeepAlive(first);
        }
    }
}

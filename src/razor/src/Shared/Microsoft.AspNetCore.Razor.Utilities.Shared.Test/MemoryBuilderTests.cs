// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test;

public class MemoryBuilderTests
{
    [Fact]
    public void StartWithDefault()
    {
        using MemoryBuilder<int> builder = default;

        for (var i = 0; i < 1000; i++)
        {
            builder.Append(i);
        }

        var result = builder.AsMemory();

        for (var i = 0; i < 1000; i++)
        {
            Assert.Equal(i, result.Span[i]);
        }
    }

    [Fact]
    public void StartWithNew()
    {
        using MemoryBuilder<int> builder = new();

        for (var i = 0; i < 1000; i++)
        {
            builder.Append(i);
        }

        var result = builder.AsMemory();

        for (var i = 0; i < 1000; i++)
        {
            Assert.Equal(i, result.Span[i]);
        }
    }

    [Fact]
    public void StartWithInitialCapacity()
    {
        using MemoryBuilder<int> builder = new(1024);

        for (var i = 0; i < 1000; i++)
        {
            builder.Append(i);
        }

        var result = builder.AsMemory();

        for (var i = 0; i < 1000; i++)
        {
            Assert.Equal(i, result.Span[i]);
        }
    }

    [Fact]
    public void StartWithInitialArray()
    {
        using MemoryBuilder<int> builder = new(1024);

        for (var i = 0; i < 1000; i++)
        {
            builder.Append(i);
        }

        var result = builder.AsMemory();

        for (var i = 0; i < 1000; i++)
        {
            Assert.Equal(i, result.Span[i]);
        }
    }

    [Fact]
    public void AppendChunks()
    {
        using MemoryBuilder<int> builder = default;

        ReadOnlySpan<int> chunk = [1, 2, 3, 4, 5, 6, 7, 8];

        for (var i = 0; i < 1000; i++)
        {
            builder.Append(chunk);
        }

        var result = builder.AsMemory();

        for (var i = 0; i < 1000; i++)
        {
            for (var j = 0; j < chunk.Length; j++)
            {
                Assert.Equal(chunk[j], result.Span[(i * 8) + j]);
            }
        }
    }
}

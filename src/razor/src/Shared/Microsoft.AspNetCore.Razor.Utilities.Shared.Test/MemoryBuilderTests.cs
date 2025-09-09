// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
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

    [Fact]
    public void CreateString_EmptyBuilder_ReturnsEmptyString()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            var result = builder.CreateString();

            Assert.Equal(string.Empty, result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_SingleChunk_ReturnsCorrectString()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            var text = "Hello, World!";

            builder.Append(text.AsMemory());

            var result = builder.CreateString();

            Assert.Same(text, result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_SingleChunk_FromSubstring_ReturnsCorrectString()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            var originalText = "Hello, World! This is a test.";
            var substring = originalText.AsMemory(7, 5); // "World"

            builder.Append(substring);

            var result = builder.CreateString();

            Assert.Equal("World", result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_MultipleChunks_ConcatenatesCorrectly()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            builder.Append("Hello".AsMemory());
            builder.Append(", ".AsMemory());
            builder.Append("World".AsMemory());
            builder.Append("!".AsMemory());

            var result = builder.CreateString();

            Assert.Equal("Hello, World!", result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_MixedChunkTypes_ConcatenatesCorrectly()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            var fullString = "Original text here";

            // Mix of full strings and substrings
            builder.Append("Start".AsMemory());
            builder.Append(" - ".AsMemory());
            builder.Append(fullString.AsMemory(0, 8)); // "Original"
            builder.Append(" + ".AsMemory());
            builder.Append(fullString.AsMemory(9, 4)); // "text"
            builder.Append(" - End".AsMemory());

            var result = builder.CreateString();

            Assert.Equal("Start - Original + text - End", result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_EmptyChunks_HandlesCorrectly()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            builder.Append("Hello".AsMemory());
            builder.Append(ReadOnlyMemory<char>.Empty);
            builder.Append(", ".AsMemory());
            builder.Append(ReadOnlyMemory<char>.Empty);
            builder.Append("World".AsMemory());

            var result = builder.CreateString();

            Assert.Equal("Hello, World", result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_OnlyEmptyChunks_ReturnsEmptyString()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            builder.Append(ReadOnlyMemory<char>.Empty);
            builder.Append(ReadOnlyMemory<char>.Empty);
            builder.Append(ReadOnlyMemory<char>.Empty);

            var result = builder.CreateString();

            Assert.Equal(string.Empty, result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_LargeNumberOfChunks_ConcatenatesCorrectly()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            var expectedLength = 0;

            // Add 100 chunks of "X"
            for (var i = 0; i < 100; i++)
            {
                builder.Append("X".AsMemory());
                expectedLength++;
            }

            var result = builder.CreateString();

            Assert.Equal(expectedLength, result.Length);
            Assert.True(result.All(c => c == 'X'));
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_VaryingChunkSizes_ConcatenatesCorrectly()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            var chunks = new[]
            {
                "A",           // 1 char
                "BB",          // 2 chars
                "CCC",         // 3 chars
                "DDDD",        // 4 chars
                "EEEEE"        // 5 chars
            };

            foreach (var chunk in chunks)
            {
                builder.Append(chunk.AsMemory());
            }

            var result = builder.CreateString();

            Assert.Equal("ABBCCCDDDDEEEEE", result);
            Assert.Equal(15, result.Length); // 1+2+3+4+5
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_UnicodeCharacters_HandlesCorrectly()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            builder.Append("Hello ".AsMemory());
            builder.Append("🌍".AsMemory());          // Earth emoji
            builder.Append(" and ".AsMemory());
            builder.Append("🚀".AsMemory());          // Rocket emoji
            builder.Append("!".AsMemory());

            var result = builder.CreateString();

            Assert.Equal("Hello 🌍 and 🚀!", result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_SpecialCharacters_HandlesCorrectly()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            builder.Append("Line1\n".AsMemory());
            builder.Append("Line2\r\n".AsMemory());
            builder.Append("Tab\t".AsMemory());
            builder.Append("Quote\"".AsMemory());
            builder.Append("Backslash\\".AsMemory());

            var result = builder.CreateString();

            Assert.Equal("Line1\nLine2\r\nTab\tQuote\"Backslash\\", result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_SingleCharacterChunks_ConcatenatesCorrectly()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            var word = "HELLO";

            foreach (var c in word)
            {
                builder.Append(c.ToString().AsMemory());
            }

            var result = builder.CreateString();

            Assert.Equal(word, result);
        }
        finally
        {
            builder.Dispose();
        }
    }

    [Fact]
    public void CreateString_AfterMultipleOperations_WorksCorrectly()
    {
        var builder = new MemoryBuilder<ReadOnlyMemory<char>>();
        try
        {
            // First, add some content
            builder.Append("Initial".AsMemory());

            // Get initial string
            var initial = builder.CreateString();
            Assert.Equal("Initial", initial);

            // Add more content
            builder.Append(" + More".AsMemory());

            // Get final string
            var final = builder.CreateString();
            Assert.Equal("Initial + More", final);
        }
        finally
        {
            builder.Dispose();
        }
    }
}

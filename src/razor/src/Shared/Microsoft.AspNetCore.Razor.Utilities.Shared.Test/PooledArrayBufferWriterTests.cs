// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test;

public class PooledArrayBufferWriterTests
{
    [Theory]
    [InlineData(42)]
    [InlineData(256)]
    [InlineData(400)]
    [InlineData(1024)]
    [InlineData(8 * 1024)]
    public void FillBufferWriter(int arraySize)
    {
        // Create an array filled with byte data.
        Span<byte> span = new byte[arraySize];

        for (var i = 0; i < arraySize; i++)
        {
            span[i] = (byte)(i % 255);
        }

        using var bufferWriter = new PooledArrayBufferWriter<byte>();

        bufferWriter.Write(span);

        Assert.True(span.SequenceEqual(bufferWriter.WrittenMemory.Span));
    }
}

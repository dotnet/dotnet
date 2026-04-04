// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Buffers;
using OpenTelemetry.OpAmp.Client.Internal.Utils;

using Xunit;

namespace OpenTelemetry.OpAmp.Client.Tests;

public class Varint64Tests
{
    [Theory]
    [InlineData(0, new byte[] { 0x00 })]
    [InlineData(1, new byte[] { 0x01 })]
    [InlineData(127, new byte[] { 0x7F })]
    [InlineData(128, new byte[] { 0x80, 0x01 })]
    [InlineData(987654321, new byte[] { 0xB1, 0xD1, 0xF9, 0xD6, 0x03 })]
    [InlineData(ulong.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x01 })]
    public void OpAmpHeaderHelper_Encode(ulong value, byte[] expectedEncoded)
    {
        var encoded = Varint64.Encode(value);

        Assert.Equal(expectedEncoded.Length, encoded.Length);
        Assert.Equal(expectedEncoded, encoded);
    }

    [Theory]
    [InlineData(0, 1, new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [InlineData(1, 1, new byte[] { 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [InlineData(127, 1, new byte[] { 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [InlineData(128, 2, new byte[] { 0x80, 0x01, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [InlineData(987654321, 5, new byte[] { 0xB1, 0xD1, 0xF9, 0xD6, 0x03, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    [InlineData(ulong.MaxValue, 10, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x01 })]
    public void OpAmpHeaderHelper_Decode(ulong expectedValue, int expectedLength, byte[] buffer)
    {
        var sequence = new ReadOnlySequence<byte>(buffer);
        var decodeResult = Varint64.TryDecode(sequence, out int bytesRead, out ulong result, out string errorMessage);

        Assert.True(decodeResult, errorMessage);
        Assert.Equal(expectedValue, result);
        Assert.Equal(expectedLength, bytesRead);
    }
}

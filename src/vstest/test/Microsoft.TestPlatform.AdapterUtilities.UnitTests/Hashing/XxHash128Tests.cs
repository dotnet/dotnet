// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;
using System.Text;

using Microsoft.TestPlatform.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.UnitTests.Hashing;

/// <summary>
/// Pins the vendored xxHash128 implementation against vectors taken from the upstream
/// dotnet/runtime test suite (src/libraries/System.IO.Hashing/tests/XxHash128Tests.cs).
/// </summary>
/// <remarks>
/// These vectors are independent of vstest: they assert that the vendored copy still computes real
/// xxHash128. Together with XxHash128CompatibilityTests, which pins the ids vstest derives from it,
/// this makes an accidental change to test ids impossible to merge silently.
/// </remarks>
[TestClass]
public class XxHash128Tests
{
    [TestMethod]
    // Empty input.
    [DataRow("", 0L, "99aa06d3014798d86001c324468d497f")]
    // Short inputs, below the 16 byte fast path.
    [DataRow("Z", 0L, "a26f5ff5290b016c2753d05a8f320003")]
    [DataRow("fCiJ", 0L, "b6b99015d0e80f4115ac2f7581d32767")]
    [DataRow("Zx63J", 0L, "4da901296206362c5b4171745fecbf51")]
    [DataRow("SbmPVuZ", 0L, "4dfc8bc32f60ec85f2081fdcb4e7adc3")]
    // Medium inputs, crossing the 16, 32 and 64 byte boundaries.
    [DataRow("uOnyD2tQQ7v", 0L, "4fea8787fd490dda2c69900c70d19b04")]
    [DataRow("CTiZlNvCQ3UGNvemq0dPvPKqyhMn316cDcPJSqG", 0L, "6f0e1fc7c616fad1606418b9cd175bb3")]
    [DataRow("jxRWEK6LJazWynCNTJLyg8nWsWnB38nYXq6cvSIzOtW091yMZEfgHaz5O05oI59UL7m00Lw6FYyGZ1", 0L, "f689d7bd37bc5e29b3154d844b44a137")]
    // Long inputs, past 100 bytes.
    [DataRow("Mb1fTHyhvQNuCur14DwDIPky7QP9kdi9AUEcqJTeGbShRh1Qf2AB36QPbQe17mKzmfeNun1qisQzu2Y8YI4dlw44TFh0otAltgRHe6EKemPhRxUk5", 0L, "317b7c14373d61f1b54071818147144d")]
    [DataRow("1osqpwzvEYMXBhwDCKUPlSjMVRW2qy8AKv6Hp9PugG0cLhwUztcjrEb506Bm6UPmS8i4icbB8xhu92MT9hff9xuLKKZg2qkEgDWm5PILwVpT9E8KJ", 0L, "9e7a47f5585bdaaf6eb06ce894224820")]
    // Seeded.
    [DataRow("", 0x13f0L, "4a807558806f6b31eca8475b2cc08fee")]
    [DataRow("f", 0x5a7a3a6dd84a445fL, "4623628864bd3461826ed41a6e3413f3")]
    [DataRow("kLi", 0x6f3502011f621a64L, "83b9766abe82fb07f917a1f0983dac91")]
    public void Hash_OneShot_MatchesUpstreamVectors(string ascii, long seed, string expected)
    {
        byte[] input = Encoding.ASCII.GetBytes(ascii);

        byte[] actual = seed == 0
            ? XxHash128.Hash(input)
            : XxHash128.Hash(input, seed);

        Assert.AreEqual(expected, ToHex(actual), $"xxHash128 of '{ascii}' (seed {seed}) is wrong.");
    }

    [TestMethod]
    [DataRow("", 0L, "99aa06d3014798d86001c324468d497f")]
    [DataRow("Zx63J", 0L, "4da901296206362c5b4171745fecbf51")]
    [DataRow("jxRWEK6LJazWynCNTJLyg8nWsWnB38nYXq6cvSIzOtW091yMZEfgHaz5O05oI59UL7m00Lw6FYyGZ1", 0L, "f689d7bd37bc5e29b3154d844b44a137")]
    [DataRow("Mb1fTHyhvQNuCur14DwDIPky7QP9kdi9AUEcqJTeGbShRh1Qf2AB36QPbQe17mKzmfeNun1qisQzu2Y8YI4dlw44TFh0otAltgRHe6EKemPhRxUk5", 0L, "317b7c14373d61f1b54071818147144d")]
    [DataRow("kLi", 0x6f3502011f621a64L, "83b9766abe82fb07f917a1f0983dac91")]
    public void Hash_Streaming_MatchesUpstreamVectors(string ascii, long seed, string expected)
    {
        byte[] input = Encoding.ASCII.GetBytes(ascii);

        // Feed the input one byte at a time, which forces the buffering paths the one-shot overload
        // never touches. TestIdProviderXxHash128 depends on those, since it appends once per string.
        var hasher = new XxHash128(seed);
        foreach (byte b in input)
        {
            hasher.Append([b]);
        }

        Assert.AreEqual(expected, ToHex(hasher.GetCurrentHash()), $"Streaming xxHash128 of '{ascii}' (seed {seed}) is wrong.");
    }

    [TestMethod]
    public void Hash_Streaming_ChunkSizeDoesNotChangeResult()
    {
        // 1000 chars, long enough to span many internal blocks.
        string data = string.Concat(Enumerable.Repeat("abcdefghij", 100));
        byte[] bytes = Encoding.Unicode.GetBytes(data);

        string oneShot = ToHex(XxHash128.Hash(bytes));

        foreach (int chunkSize in new[] { 1, 7, 63, 64, 65, 256, 1024 })
        {
            var hasher = new XxHash128();
            for (int offset = 0; offset < bytes.Length; offset += chunkSize)
            {
                int length = Math.Min(chunkSize, bytes.Length - offset);
                byte[] chunk = new byte[length];
                Array.Copy(bytes, offset, chunk, 0, length);
                hasher.Append(chunk);
            }

            Assert.AreEqual(oneShot, ToHex(hasher.GetCurrentHash()), $"Chunk size {chunkSize} produced a different hash.");
        }
    }

    [TestMethod]
    public void Hash_Throws_WhenSourceIsNull()
        => Assert.ThrowsExactly<ArgumentNullException>(() => XxHash128.Hash(null!));

    private static string ToHex(byte[] bytes)
    {
        var builder = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("x2", CultureInfo.InvariantCulture));
        }

        return builder.ToString();
    }
}

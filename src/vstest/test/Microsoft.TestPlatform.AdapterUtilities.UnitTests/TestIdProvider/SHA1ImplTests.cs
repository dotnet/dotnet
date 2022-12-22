// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.UnitTests.TestIdProvider;

[TestClass]
public class Sha1ImplTests
{
    [TestMethod]
    public void SHA1_TestVectors_EmptyString()
    {
        SHA1_TestVector(
            string.Empty,
            "da39a3ee5e6b4b0d3255bfef95601890afd80709"
        );
    }

    [TestMethod]
    public void SHA1_TestVectors_abc()
    {
        SHA1_TestVector(
            "abc",
            "a9993e364706816aba3e25717850c26c9cd0d89d"
        );
    }

    [TestMethod]
    public void SHA1_TestVectors_448Bits()
    {
        SHA1_TestVector(
            "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq",
            "84983e441c3bd26ebaae4aa1f95129e5e54670f1"
        );
    }

    [TestMethod]
    public void SHA1_TestVectors_896Bits()
    {
        SHA1_TestVector(
            "abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmnoijklmnopjklmnopqklmnopqrlmnopqrsmnopqrstnopqrstu",
            "a49b2446a02c645bf419f995b67091253a04a259"
        );
    }

    [TestMethod]
    public void SHA1_TestVectors_1Block()
    {
        SHA1_TestRepetitionVector(
            'a',
            512 / 8
        );
    }

    [TestMethod]
    public void SHA1_ExtremelyLarge_TestVectors_500k_a()
    {
        SHA1_TestRepetitionVector(
            'a',
            500_000
        );
    }

    [TestMethod]
    public void SHA1_ExtremelyLarge_TestVectors_900k_a()
    {
        SHA1_TestRepetitionVector(
            'a',
            900_000
        );
    }

    [TestMethod]
    public void SHA1_ExtremelyLarge_TestVectors_999999_a()
    {
        SHA1_TestRepetitionVector(
            'a',
            999_999
        );
    }

    [TestMethod]
    public void SHA1_ExtremelyLarge_TestVectors_1M_a()
    {
        SHA1_TestRepetitionVector(
            'a',
            1_000_000,
            "34aa973c d4c4daa4 f61eeb2b dbad2731 6534016f"
        );
    }

    [TestMethod]
    public void SHA1_ExtremelyLarge_TestVectors_10M_a()
    {
        SHA1_TestRepetitionVector(
            'a',
            10_000_000
        );
    }

    private static void SHA1_TestVector(string message, string expected)
    {
        // Arrange
        expected = expected.Replace(" ", "").ToLowerInvariant();
        var shaHasher1 = new AdapterUtilities.TestIdProvider.Sha1Implementation();

        // Act
        var bytes = Encoding.UTF8.GetBytes(message);
        var digest1 = ToHex(shaHasher1.ComputeHash(bytes));

        // Assert
        Assert.AreEqual(expected, digest1, $"Test vector '{message}' failed!");
    }

    private static void SHA1_TestRepetitionVector(char input, int repetition, string? expected = null)
    {
        // Arrange
        var shaHasher1 = new AdapterUtilities.TestIdProvider.Sha1Implementation();
        var shaHasher2 = new AdapterUtilities.TestIdProvider.Sha1Implementation();

        var bytes = new byte[repetition];
        for (int i = 0; i < repetition; i++)
        {
            bytes[i] = (byte)input;
        }

        if (string.IsNullOrEmpty(expected))
        {
            using var hasher = System.Security.Cryptography.SHA1.Create();
            expected = ToHex(hasher.ComputeHash(bytes));
        }
        else
        {
            expected = expected!.Replace(" ", "").ToLowerInvariant();
        }

        // Act
        var digest1 = ToHex(shaHasher1.ComputeHash(bytes));
        var blocks = bytes.Length / AdapterUtilities.TestIdProvider.BlockBytes;
        byte[] block;
        for (var i = 0; i < blocks; i += 1)
        {
            block = new byte[AdapterUtilities.TestIdProvider.BlockBytes];
            Buffer.BlockCopy(bytes, i * block.Length, block, 0, block.Length);
            shaHasher2.ProcessBlock(block, 0, block.Length);
        }

        var rest = bytes.Length - blocks * AdapterUtilities.TestIdProvider.BlockBytes;
        if (rest != 0)
        {
            block = new byte[rest];
            Buffer.BlockCopy(bytes, blocks * block.Length, block, 0, block.Length);
            shaHasher2.PadMessage(ref block, block.Length);
            shaHasher2.ProcessBlock(block, 0, block.Length);
        }

        var digest2 = ToHex(shaHasher2.ProcessFinalBlock());

        // Assert
        Assert.AreEqual(expected, digest1, $"Test vector '{input}'*{repetition} failed! (normal path)");
        Assert.AreEqual(expected, digest2, $"Test vector '{input}'*{repetition} failed! (padding path)");
    }

    private static string ToHex(byte[] digest) => string.Concat(digest.Select(i => i.ToString("x2", CultureInfo.CurrentCulture)));
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.TestPlatform.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.UnitTests.Hashing;

/// <summary>
/// Covers the RFC 9562 version 8 UUID encoding applied to every generated test id.
/// </summary>
[TestClass]
public class TestIdGuidTests
{
    [TestMethod]
    public void VersionedGuidFromHash_SetsUuidVersionTo8()
    {
        byte[] hash = new byte[16];

        Guid guid = TestIdGuid.VersionedGuidFromHash(hash, hashVersion: 1);

        // The UUID version lives in the top 4 bits of byte 6 of the big-endian layout, which is the
        // first character of the third dash-separated group.
        Assert.AreEqual('8', guid.ToString("D")[14], $"Expected a version 8 UUID but got {guid}.");
    }

    [TestMethod]
    public void VersionedGuidFromHash_SetsRfc9562Variant()
    {
        byte[] hash = new byte[16];
        for (int i = 0; i < hash.Length; i++)
        {
            hash[i] = 0xFF;
        }

        Guid guid = TestIdGuid.VersionedGuidFromHash(hash, hashVersion: 1);

        // The variant lives in the top 2 bits of byte 8, which must be 0b10. That makes the first
        // character of the fourth group one of 8, 9, a or b.
        char variant = guid.ToString("D")[19];
        Assert.IsTrue(variant is '8' or '9' or 'a' or 'b', $"Expected an RFC 9562 variant but got '{variant}' in {guid}.");
    }

    [TestMethod]
    [DataRow((byte)1)]
    [DataRow((byte)2)]
    [DataRow((byte)15)]
    public void VersionedGuidFromHash_EmbedsHashVersionInTopNibble(byte hashVersion)
    {
        byte[] hash = new byte[16];

        Guid guid = TestIdGuid.VersionedGuidFromHash(hash, hashVersion);

        // The hash version lives in the top 4 bits of byte 0, which is the first character of the id.
        char expected = hashVersion.ToString("x", System.Globalization.CultureInfo.InvariantCulture)[0];
        Assert.AreEqual(expected, guid.ToString("D")[0], $"Expected hash version {hashVersion} to be encoded in {guid}.");
    }

    [TestMethod]
    public void VersionedGuidFromHash_PreservesTheRemainingHashBits()
    {
        // Every byte distinct so a reordering bug shows up.
        byte[] hash = [0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF];

        Guid guid = TestIdGuid.VersionedGuidFromHash(hash, TestIdGuid.CurrentHashVersion);

        // Bytes are laid out big-endian, so the hash is readable straight off the id, with only the
        // version, variant and hash version nibbles overwritten.
        Assert.AreEqual("10112233-4455-8677-8899-aabbccddeeff", guid.ToString("D"));
    }

    [TestMethod]
    public void CurrentHashVersion_FitsInFourBitsAndIsNotZero()
    {
        // 0 is reserved to mean "unversioned", which is what every legacy SHA1 id looks like.
        byte current = TestIdGuid.CurrentHashVersion;
        Assert.IsTrue(current is >= 1 and <= 15, $"CurrentHashVersion {current} does not fit in 4 bits.");
    }
}

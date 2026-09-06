// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;
using System.Text;

using Microsoft.TestPlatform.Hashing;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

/// <summary>
/// Wrapper class for hashing.
/// </summary>
public static class EqtHash
{
    /// <summary>
    /// Calculates a SHA1 hash of the string and copies the first 128 bits of the hash
    /// to a new Guid.
    /// </summary>
    /// <remarks>
    /// This is the algorithm test case ids are computed with by default. See
    /// <see cref="GuidFromStringXxHash128(string)"/> for the xxHash128 based successor, which is available
    /// but not yet the default.
    /// </remarks>
    public static Guid GuidFromString(string data)
    {
        TPDebug.Assert(data != null);

        // Do NOT change the algorithm ever as this will have compat implications
        // TC-TA team has a feature in VS where workitems are associated based on TestCase Ids
        // If Algorithm changes, then all the bugs/workitems filed in TFS Server against a given TestCase become unassociated if IDs change
        // Any algorithm or logic change must require a sign off from feature owners of above
        // Also, TPV2 and TPV1 must use same Algorithm until the time TPV1 is completely deleted to be on-par
        // If LUT or .Net core scenario uses TPV2 to discover, but if it uses TPV1 in Devenv, then there will be testcase matching issues
        using HashAlgorithm provider = SHA1.Create();
        byte[] hash = provider.ComputeHash(Encoding.Unicode.GetBytes(data));

        // Guid is always 16 bytes
        TPDebug.Assert(Guid.Empty.ToByteArray().Length == 16, "Expected Guid to be 16 bytes");

        byte[] toGuid = new byte[16];
        Array.Copy(hash, toGuid, 16);

        return new Guid(toGuid);
    }

    /// <summary>
    /// Calculates an xxHash128 hash of the string and turns it into an RFC 9562 version 8 UUID.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the intended successor to <see cref="GuidFromString(string)"/>. xxHash128 is a
    /// non-cryptographic hash, which is what this has always needed - the id is an identity, never a
    /// security boundary - and it is considerably faster than SHA1. Using a cryptographic hash for
    /// this also makes vstest show up in security scans that flag SHA1 regardless of how it is used.
    /// </para>
    /// <para>
    /// The resulting Guid carries the version of the hashing scheme in its top 4 bits, so ids
    /// produced by this method are distinguishable from legacy SHA1 ids and from any future
    /// scheme. See TestIdGuid.
    /// </para>
    /// <para>
    /// This deliberately does NOT produce the same value as <see cref="GuidFromString(string)"/>.
    /// Changing the id of a test is a breaking change for anything that stored it, which is why this
    /// ships available but not default: test case ids are computed with it only when a run opts in
    /// through the VSTEST_DISABLE_XXHASH128_TESTCASE_ID feature flag. The warning on
    /// <see cref="GuidFromString(string)"/> about work item association requiring sign off from the
    /// TC-TA feature owners applies to making this the default.
    /// </para>
    /// <para>
    /// The bytes are laid out exactly as MSTest lays them out, so the same input produces the same
    /// id in both, and an id produced by either is legible to the other.
    /// </para>
    /// </remarks>
    public static Guid GuidFromStringXxHash128(string data)
    {
        TPDebug.Assert(data != null);

        // An xxHash128 hash is 16 bytes, exactly the size of a Guid.
        byte[] hash = XxHash128.Hash(Encoding.Unicode.GetBytes(data));

        TPDebug.Assert(Guid.Empty.ToByteArray().Length == 16, "Expected Guid to be 16 bytes");

        return TestIdGuid.VersionedGuidFromHash(hash, TestIdGuid.CurrentHashVersion);
    }
}

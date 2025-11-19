// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

/// <summary>
/// Wrapper class for cryptographic hashing.
/// This class uses SHA1 instead of MD5 in order to conform to the FIPS standard.
/// </summary>
public static class EqtHash
{
    /// <summary>
    /// Calculates a hash of the string and copies the first 128 bits of the hash
    /// to a new Guid.
    /// </summary>
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
        byte[] hash = provider.ComputeHash(System.Text.Encoding.Unicode.GetBytes(data));

        // Guid is always 16 bytes
        TPDebug.Assert(Guid.Empty.ToByteArray().Length == 16, "Expected Guid to be 16 bytes");

        byte[] toGuid = new byte[16];
        Array.Copy(hash, toGuid, 16);

        return new Guid(toGuid);
    }
}

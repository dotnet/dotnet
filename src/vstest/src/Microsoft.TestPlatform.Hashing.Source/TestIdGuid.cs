// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace Microsoft.TestPlatform.Hashing;

/// <summary>
/// Turns a 128-bit hash into a test id <see cref="Guid"/>.
/// </summary>
/// <remarks>
/// <para>
/// The produced value is an RFC 9562 version 8 UUID (custom / implementation defined). Version 8
/// is the only version the RFC reserves for values that are not produced by one of the algorithms
/// it standardizes, which is exactly our situation: the bits come from xxHash128, not from a
/// random source, a timestamp or a namespace hash.
/// </para>
/// <para>
/// On top of the version we spend the top 4 bits of the first byte on our own
/// <c>hashVersion</c>. That way the algorithm that produced an id can be read straight off the
/// id, so a future change to the hashing is both detectable and cheap to reason about, instead
/// of being an undetectable silent break. This mirrors what MSTest does, so the two ecosystems
/// stay legible to each other.
/// </para>
/// </remarks>
internal static class TestIdGuid
{
    /// <summary>
    /// The version of the hashing scheme currently in use. Increment this whenever the algorithm
    /// or the data fed into it changes.
    /// </summary>
    /// <remarks>
    /// Only values 1-15 are representable; 0 is reserved to mean "not versioned", which is what
    /// every id produced by the legacy SHA1 scheme effectively looks like.
    /// </remarks>
    public const byte CurrentHashVersion = 1;

    /// <summary>
    /// Builds a version 8 UUID out of the first 16 bytes of <paramref name="hashBytes"/>, stamping
    /// in the UUID version, the UUID variant and our own <paramref name="hashVersion"/>.
    /// </summary>
    /// <param name="hashBytes">The hash to derive the id from. Must be at least 16 bytes. Mutated in place.</param>
    /// <param name="hashVersion">The hashing scheme version to embed. Must be between 1 and 15.</param>
    public static Guid VersionedGuidFromHash(byte[] hashBytes, byte hashVersion)
    {
        Debug.Assert(hashBytes.Length >= 16, "Expected at least 16 bytes of hash");
        Debug.Assert(hashVersion is >= 1 and <= 15, "hashVersion must fit in 4 bits and must not be 0");

        const int firstByte = 0;
        const int versionByte = 6;
        const int variantByte = 8;

        // Set the top 4 bits of the first byte to our own hash version.
        //
        // Note: the logic below operates on int32 because bitwise operators are not defined for byte
        // in C#. Casting does not affect endianness, so the byte data is always at the end of the int,
        // and the bit masks deliberately do not spell out the rest of the int - those bits are always 0.
        hashBytes[firstByte] = (byte)((hashBytes[firstByte] & 0b0000_1111) | (hashVersion << 4));

        // Set the top 4 bits of the 7th byte to 8, marking this as a version 8 UUID.
        // https://www.rfc-editor.org/rfc/rfc9562.html#name-uuid-version-8
        hashBytes[versionByte] = (byte)((hashBytes[versionByte] & 0b0000_1111) | 0b1000_0000);

        // Set the top 2 bits of the 9th byte to 0b10, the RFC 9562 variant marker.
        hashBytes[variantByte] = (byte)((hashBytes[variantByte] & 0b0011_1111) | 0b1000_0000);

        // On .NET Framework we cannot use new Guid(bytes, bigEndian: true), so construct the int and
        // short components by hand to lay the bytes out in big-endian order. Doing it this way on every
        // target framework also guarantees the same id regardless of where the test ran.
        Guid guid = new(
            (hashBytes[0] << 24) | (hashBytes[1] << 16) | (hashBytes[2] << 8) | hashBytes[3],
            (short)((hashBytes[4] << 8) | hashBytes[5]),
            (short)((hashBytes[6] << 8) | hashBytes[7]),
            hashBytes[8], hashBytes[9], hashBytes[10], hashBytes[11], hashBytes[12], hashBytes[13], hashBytes[14], hashBytes[15]);

#if NET9_0_OR_GREATER
        Debug.Assert(guid.Version == 8, "Expected a version 8 UUID");
#endif

        return guid;
    }
}

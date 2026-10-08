// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// The per-child connection secrets. The process that starts a child generates a pair and passes it
/// through the child's environment, which the operating system only lets the same user read. Each
/// side then presents the secret belonging to its role, so both ends of the connection are checked.
///
/// The child presents its secret first, and the parent answers only after the child's has matched,
/// so a process that connects without the secrets learns nothing.
/// </summary>
internal static class ConnectionSecrets
{
    private const char Separator = ':';

    /// <summary>
    /// Generates the pair carried in one environment variable: the secret the child presents,
    /// then the secret the parent presents. Each half is 256 bits of cryptographic randomness.
    /// </summary>
    internal static string GeneratePair()
        => string.Concat(GenerateOne(), Separator.ToString(), GenerateOne());

    /// <summary>
    /// Splits a generated pair. Returns <see langword="false"/> for anything malformed, so a
    /// truncated or tampered value cannot be mistaken for a usable secret.
    /// </summary>
    internal static bool TrySplit(string? pair, out string childSecret, out string parentSecret)
    {
        childSecret = string.Empty;
        parentSecret = string.Empty;
        if (pair is null)
        {
            return false;
        }

        var separator = pair.IndexOf(Separator);
        if (separator <= 0 || separator == pair.Length - 1)
        {
            return false;
        }

        childSecret = pair.Substring(0, separator);
        parentSecret = pair.Substring(separator + 1);
        return parentSecret.IndexOf(Separator) < 0;
    }

    /// <summary>
    /// Compares two secrets. A plain ordinal comparison is enough: it can return early and so reveal
    /// how many leading characters matched, but recovering a secret that way needs many timed
    /// attempts, and a wrong secret ends the run on the first one, with a fresh pair generated for
    /// the next run. Revisit this if either of those two properties changes.
    /// </summary>
    internal static bool AreEqual(string? expected, string? actual)
        => expected is not null
            && actual is not null
            && string.Equals(expected, actual, StringComparison.Ordinal);

    private static string GenerateOne()
    {
        var bytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        // Convert.ToHexString is not available on net462/netstandard2.0, so format manually.
        return BitConverter.ToString(bytes).Replace("-", string.Empty);
    }
}
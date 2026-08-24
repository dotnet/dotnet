// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Dpop;

/// <summary>
/// Component-wise URI equivalence comparison per RFC 3986 §6.2.2 syntax-based normalization,
/// ignoring query and fragment.
/// </summary>
/// <remarks>
/// <para>
/// Per RFC 3986 §6.2.2.1, the scheme and host components are compared case-insensitively
/// (and host is additionally subject to IDN normalization). The port is normalized to the
/// scheme's default when omitted (§3.2.3). The path component is compared
/// <strong>case-sensitively</strong> (§3.3 reserves case as significant). Percent-encoded
/// triplets of unreserved characters are decoded for comparison (§6.2.2.2). Query and
/// fragment components are not considered.
/// </para>
/// <para>
/// This is the comparison contract required by RFC 9449 §4.3 for DPoP <c>htu</c> validation,
/// but the implementation itself is not DPoP-specific.
/// </para>
/// </remarks>
internal static class UriComparer
{
    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="uri"/> is equivalent to
    /// <paramref name="uriToCompareTo"/> under the comparison rules described in the
    /// type-level remarks (scheme, host, port, and path; query and fragment ignored).
    /// </summary>
    /// <param name="uri">The first URI. Must be non-null and absolute.</param>
    /// <param name="uriToCompareTo">The URI string to compare against. Must parse as an absolute URI.</param>
    /// <returns>
    /// <see langword="true"/> when both URIs are absolute and equivalent;
    /// <see langword="false"/> when <paramref name="uri"/> is null or not absolute, when
    /// <paramref name="uriToCompareTo"/> is null, empty, or cannot be parsed as an
    /// absolute URI, or when any compared component (scheme, host, port, path) differs.
    /// </returns>
    internal static bool AreEquivalent(Uri uri, string uriToCompareTo)
    {
        if (uri is null || !uri.IsAbsoluteUri)
            return false;

        if (string.IsNullOrEmpty(uriToCompareTo))
            return false;

        if (!Uri.TryCreate(uriToCompareTo, UriKind.Absolute, out Uri parsed))
            return false;

        // Scheme + authority (host:port) — case-insensitive per RFC 3986 §6.2.2.1,
        // default-port-aware per §3.2.3, IDN-normalized via .NET's Uri.
        if (Uri.Compare(
                uri,
                parsed,
                UriComponents.SchemeAndServer,
                UriFormat.SafeUnescaped,
                StringComparison.OrdinalIgnoreCase) != 0)
        {
            return false;
        }

        // Path — case-sensitive per RFC 3986 §3.3 and §6.2.2.
        // SafeUnescaped normalizes percent-encoding of unreserved chars per §6.2.2.2.
        if (Uri.Compare(
                uri,
                parsed,
                UriComponents.Path,
                UriFormat.SafeUnescaped,
                StringComparison.Ordinal) != 0)
        {
            return false;
        }

        return true;
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Dpop;

/// <summary>
/// Constants for DPoP (Demonstrating Proof-of-Possession at the Application Layer) per RFC 9449.
/// </summary>
/// <remarks>
/// Claim types are in <see cref="DpopClaimTypes"/>.
/// Error codes are in <see cref="DpopErrorCodes"/>.
/// </remarks>
public static class DpopConstants
{
    /// <summary>
    /// The DPoP token type.
    /// </summary>
    public const string DpopTokenType = "DPoP";

    /// <summary>
    /// The required <c>typ</c> header value for DPoP proof JWTs.
    /// </summary>
    public const string DpopProofTokenType = "dpop+jwt";

    /// <summary>
    /// The DPoP nonce HTTP header name.
    /// </summary>
    public const string DpopNonceHeaderName = "DPoP-Nonce";

    /// <summary>
    /// The default maximum DPoP proof lifetime in seconds, measured from the <c>iat</c> claim (5 minutes).
    /// </summary>
    public const int DefaultMaxLifetimeInSeconds = 300;

    /// <summary>
    /// The default clock skew tolerance in seconds applied to DPoP proof timestamp validation (5 minutes).
    /// </summary>
    public const int DefaultClockSkewInSeconds = 300;

    /// <summary>
    /// The default maximum DPoP proof JWT size in bytes (8 KiB).
    /// Realistic proofs are well under 2 KiB; the cap bounds parse work for malformed input.
    /// </summary>
    public const int DefaultMaxProofTokenSizeInBytes = 8 * 1024;

    /// <summary>
    /// The default maximum RSA modulus size in bits permitted for DPoP proof signing keys.
    /// Bounds the cost of verifying client-controlled keys (verification cost is super-linear in modulus size).
    /// </summary>
    public const int DefaultMaxRsaKeySizeInBits = 4096;

    /// <summary>
    /// The default minimum RSA modulus size in bits permitted for DPoP proof signing keys.
    /// Matches NIST SP 800-131A guidance for asymmetric signature verification beyond 2030.
    /// </summary>
    public const int DefaultMinRsaKeySizeInBits = 2048;
}

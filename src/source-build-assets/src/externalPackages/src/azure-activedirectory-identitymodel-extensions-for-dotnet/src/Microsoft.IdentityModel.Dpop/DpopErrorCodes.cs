// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Dpop;

/// <summary>
/// RFC 9449 error codes for DPoP validation failures.
/// </summary>
public static class DpopErrorCodes
{
    /// <summary>The server requires a DPoP nonce.</summary>
    public const string UseDpopNonce = "use_dpop_nonce";

    /// <summary>The token is invalid.</summary>
    public const string InvalidToken = "invalid_token";

    /// <summary>
    /// The HTTP request itself is invalid (e.g., missing or duplicated <c>DPoP</c> header,
    /// or a malformed header value before parsing).
    /// </summary>
    /// <remarks>
    /// Provided for callers that compose their own RFC 9449 §7.1 <c>WWW-Authenticate</c>
    /// responses around the validator. <see cref="DpopProofValidator"/> does not emit this
    /// code itself because it operates on a proof string the caller has already extracted
    /// from the request — header-level shape errors are detected and signalled at the
    /// caller's request-pipeline layer.
    /// </remarks>
    public const string InvalidRequest = "invalid_request";
}

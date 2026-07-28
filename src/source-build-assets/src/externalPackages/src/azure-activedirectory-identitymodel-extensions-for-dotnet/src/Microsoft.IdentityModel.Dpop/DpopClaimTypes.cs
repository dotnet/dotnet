// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Dpop;

/// <summary>
/// DPoP-specific JWT claim type names per RFC 9449.
/// </summary>
public static class DpopClaimTypes
{
    /// <summary>The HTTP method claim.</summary>
    public const string Htm = "htm";

    /// <summary>The HTTP URI claim.</summary>
    public const string Htu = "htu";

    /// <summary>The issued-at timestamp claim.</summary>
    public const string Iat = "iat";

    /// <summary>The JWT unique identifier claim.</summary>
    public const string Jti = "jti";

    /// <summary>The access token hash claim.</summary>
    public const string Ath = "ath";

    /// <summary>The server-provided nonce claim.</summary>
    public const string Nonce = "nonce";
}

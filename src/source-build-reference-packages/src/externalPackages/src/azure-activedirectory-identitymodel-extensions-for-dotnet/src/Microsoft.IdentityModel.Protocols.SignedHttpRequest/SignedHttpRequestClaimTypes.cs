// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.JsonWebTokens;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Claim types used with SignedHttpRequest.
    /// </summary>
    public static class SignedHttpRequestClaimTypes
    {
        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string At = "at";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string Ts = "ts";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string M = "m";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string U = "u";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string P = "p";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string Q = "q";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string H = "h";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3
        /// </summary>
        public const string B = "b";

        /// <summary>
        /// Default "nonce" claim.
        /// </summary>
        public const string Nonce = JwtRegisteredClaimNames.Nonce;
    }
}

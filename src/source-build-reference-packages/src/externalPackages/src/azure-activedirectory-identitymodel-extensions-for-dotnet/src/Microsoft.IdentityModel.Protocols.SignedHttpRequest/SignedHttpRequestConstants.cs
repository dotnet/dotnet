// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Constants for SignedHttpRequest related properties.
    /// </summary>
    public static class SignedHttpRequestConstants
    {
        /// <summary>
        /// The "Authorization" header string.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7235#section-4.2</remarks>
        public const string AuthorizationHeader = "Authorization";

        /// <summary>
        /// Authorization header scheme name.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-4.1</remarks>
        public const string AuthorizationHeaderSchemeName = "PoP";

        /// <summary>
        /// SignedHttpRequest token type.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-6.1</remarks>
        public const string TokenType = "pop";
    }
}

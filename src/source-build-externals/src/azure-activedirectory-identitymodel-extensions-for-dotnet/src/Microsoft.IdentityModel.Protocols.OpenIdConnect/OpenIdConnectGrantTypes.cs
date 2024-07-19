// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Defines grant types for token requests. See: <see href="https://datatracker.ietf.org/doc/html/rfc6749"/>.
    /// </summary>
    public static class OpenIdConnectGrantTypes
    {
        /// <summary>
        /// Indicates the 'authorization_code' grant type. See: <see href="https://datatracker.ietf.org/doc/html/rfc6749#section-4.1"/>.
        /// </summary>
        public const string AuthorizationCode = "authorization_code";

        /// <summary>
        /// Indicates the 'refresh_token' grant type. See: <see href="https://datatracker.ietf.org/doc/html/rfc6749#section-6"/>.
        /// </summary>
        public const string RefreshToken = "refresh_token";

        /// <summary>
        /// Indicates the 'password' grant type. See: <see href="https://datatracker.ietf.org/doc/html/rfc6749#section-4.3"/>.
        /// </summary>
        public const string Password = "password";

        /// <summary>
        /// Indicates the 'client_credentials' grant type. See: <see href="https://datatracker.ietf.org/doc/html/rfc6749#section-4.4"/>.
        /// </summary>
        public const string ClientCredentials = "client_credentials";

        /// <summary>
        /// Indicates the 'saml2-bearer' grant type. See: <see href="https://datatracker.ietf.org/doc/html/rfc7522"/>.
        /// </summary>
        public const string Saml2Bearer = "urn:ietf:params:oauth:grant-type:saml2-bearer";

        /// <summary>
        /// Indicates the 'jwt-bearer' grant type. See: <see href="https://datatracker.ietf.org/doc/html/rfc7523"/>.
        /// </summary>
        public const string JwtBearer = "urn:ietf:params:oauth:grant-type:jwt-bearer";

        /// <summary>
        /// Indicates the 'device_code' grant type. See: <see href="https://datatracker.ietf.org/doc/html/rfc8628"/>.
        /// </summary>
        public const string DeviceCode = "urn:ietf:params:oauth:grant-type:device_code";

        /// <summary>
        /// Indicates the 'token-exchange' grant type. See: <see href="https://datatracker.ietf.org/doc/html/rfc8693"/>.
        /// </summary>
        public const string TokenExchange = "urn:ietf:params:oauth:grant-type:token-exchange";

        /// <summary>
        /// Indicates the 'ciba' grant type. See: <see href="https://openid.net/specs/openid-client-initiated-backchannel-authentication-core-1_0.html"/>.
        /// </summary>
        public const string Ciba = "urn:openid:params:grant-type:ciba";
    }
}

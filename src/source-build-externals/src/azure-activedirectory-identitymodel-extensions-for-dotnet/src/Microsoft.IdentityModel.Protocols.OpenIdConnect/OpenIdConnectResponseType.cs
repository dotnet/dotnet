// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Defines response types for OpenID Connect.
    /// </summary>
    /// <remarks>
    /// Can be used to determine the message type in an <see cref="OpenIdConnectMessage"/>.
    /// </remarks>
    public static class OpenIdConnectResponseType
    {
        /// <summary>
        /// Indicates the 'code' response type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#CodeFlowAuth"/>.
        /// For example: <see href="https://openid.net/specs/openid-connect-core-1_0.html#codeExample"/>.
        /// </summary>
        public const string Code = "code";

        /// <summary>
        /// Indicates the 'code id_token' response type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest"/>.
        /// For example: <see href="https://openid.net/specs/openid-connect-core-1_0.html#code-id_tokenExample"/>.
        /// </summary>
        public const string CodeIdToken = "code id_token";

        /// <summary>
        /// Indicates the 'code id_token token' response type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest"/>.
        /// For example: <see href="https://openid.net/specs/openid-connect-core-1_0.html#code-id_token-tokenExample"/>.
        /// </summary>
        public const string CodeIdTokenToken = "code id_token token";

        /// <summary>
        /// Indicates the 'code token' response type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest"/>.
        /// For example: <see href="https://openid.net/specs/openid-connect-core-1_0.html#code-tokenExample"/>.
        /// </summary>
        public const string CodeToken = "code token";

        /// <summary>
        /// Indicates the 'id_token' response type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest"/>.
        /// For example: <see href="https://openid.net/specs/openid-connect-core-1_0.html#id_tokenExample"/>.
        /// </summary>
        public const string IdToken = "id_token";

        /// <summary>
        /// Indicates the 'id_token token' response type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#ImplicitFlowAuth"/>.
        /// For example: <see href="https://openid.net/specs/openid-connect-core-1_0.html#id_token-tokenExample"/>.
        /// </summary>
        public const string IdTokenToken = "id_token token";

        /// <summary>
        /// Defined in the OAuth v2 Multiple Response Types 1.0 spec for completeness.
        /// See: <see href="https://openid.net/specs/oauth-v2-multiple-response-types-1_0.html#OAuthResponseTypesReg"/>.
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// Defined in the OAuth 2.0 spec for completeness.
        /// See: <see href="https://datatracker.ietf.org/doc/html/rfc6749#section-11.3.2"/>.
        /// </summary>
        public const string Token = "token";
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Response types for OpenIdConnect.
    /// </summary>
    /// <remarks>Can be used to determine the message type by consumers of an <see cref="OpenIdConnectMessage"/>.
    /// For example: OpenIdConnectMessageTests.Publics() sets <see cref="OpenIdConnectMessage.ResponseType"/>
    /// to <see cref="OpenIdConnectResponseType.CodeIdToken"/>.</remarks>
    public static class OpenIdConnectResponseType
    {
        /// <summary>
        /// Indicates 'code' type see: http://openid.net/specs/openid-connect-core-1_0.html#CodeFlowAuth.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#codeExample.
        /// </summary>
        public const string Code = "code";

        /// <summary>
        /// Indicates 'code id_token' type see: http://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#code-id_tokenExample.
        /// </summary>
        public const string CodeIdToken = "code id_token";

        /// <summary>
        /// Indicates 'code id_token token' type see: http://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#code-id_token-tokenExample.
        /// </summary>
        public const string CodeIdTokenToken = "code id_token token";

        /// <summary>
        /// Indicates 'code token' type see: http://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#code-tokenExample.
        /// </summary>
        public const string CodeToken = "code token";

        /// <summary>
        /// Indicates 'id_token' type see: http://openid.net/specs/openid-connect-core-1_0.html#HybridAuthRequest.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#id_tokenExample.
        /// </summary>
        public const string IdToken = "id_token";

        /// <summary>
        /// Indicates 'id_token token' type see: http://openid.net/specs/openid-connect-core-1_0.html#ImplicitFlowAuth.
        /// For Example: http://openid.net/specs/openid-connect-core-1_0.html#id_token-tokenExample.
        /// </summary>
        public const string IdTokenToken = "id_token token";

        /// <summary>
        /// Defined in OAuth v2 multiple response types 1.0 spec, included for completion.
        /// See: http://openid.net/specs/oauth-v2-multiple-response-types-1_0.html#OAuthResponseTypesReg.
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// Defined in OAuth 2.0 spec, included for completion.
        /// See: https://datatracker.ietf.org/doc/html/rfc6749#section-11.3.2.
        /// </summary>
        public const string Token = "token";
    }
}

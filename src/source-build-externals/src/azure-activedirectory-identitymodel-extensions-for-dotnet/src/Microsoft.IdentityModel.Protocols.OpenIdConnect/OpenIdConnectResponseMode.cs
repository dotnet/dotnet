// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Response modes for OpenIdConnect.
    /// </summary>
    /// <remarks>Can be used to determine the response mode by consumers of an <see cref="OpenIdConnectMessage"/>.
    /// For example: OpenIdConnectMessageTests.Publics() sets <see cref="OpenIdConnectMessage.ResponseMode"/>
    /// to <see cref="OpenIdConnectResponseMode.FormPost"/>.</remarks>
    public static class OpenIdConnectResponseMode
    {
        /// <summary>
        /// Indicates a Query Response see: http://openid.net/specs/openid-connect-core-1_0.html#ImplicitAuthResponse.
        /// </summary>
        public const string Query = "query";

        /// <summary>
        /// Indicates a Form Post Response see: http://openid.net/specs/openid-connect-core-1_0.html#ImplicitAuthResponse.
        /// </summary>
        public const string FormPost = "form_post";

        /// <summary>
        /// Indicates a Fragment Response see: http://openid.net/specs/openid-connect-core-1_0.html#ImplicitAuthResponse.
        /// </summary>
        public const string Fragment = "fragment";
    }
}

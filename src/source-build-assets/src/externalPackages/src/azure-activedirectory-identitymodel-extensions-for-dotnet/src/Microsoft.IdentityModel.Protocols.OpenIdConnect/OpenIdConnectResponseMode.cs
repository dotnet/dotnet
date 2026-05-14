// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Defines response modes for OpenID Connect.
    /// </summary>
    /// <remarks>
    /// Can be used to determine the response mode in an <see cref="OpenIdConnectMessage"/>.
    /// </remarks>
    public static class OpenIdConnectResponseMode
    {
        /// <summary>
        /// Indicates a Query Response. See: <see href="https://openid.net/specs/oauth-v2-multiple-response-types-1_0.html"/>.
        /// </summary>
        public const string Query = "query";

        /// <summary>
        /// Indicates a Form Post Response. See: <see href="https://openid.net/specs/oauth-v2-form-post-response-mode-1_0.html"/>.
        /// </summary>
        public const string FormPost = "form_post";

        /// <summary>
        /// Indicates a Fragment Response. See: <see href="https://openid.net/specs/oauth-v2-multiple-response-types-1_0.html"/>.
        /// </summary>
        public const string Fragment = "fragment";
    }
}

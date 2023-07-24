// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Prompt types for OpenIdConnect.
    /// </summary>
    public static class OpenIdConnectPrompt
    {
        /// <summary>
        /// Indicates 'none' prompt type see: http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// Indicates 'login' prompt type see: http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
        /// </summary>
        public const string Login = "login";

        /// <summary>
        /// Indicates 'consent' prompt type see: http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
        /// </summary>
        public const string Consent = "consent";

        /// <summary>
        /// Indicates 'select_account' prompt type see: http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
        /// </summary>
        public const string SelectAccount = "select_account";
    }
}


// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// Defines prompt types for OpenID Connect.
    /// </summary>
    public static class OpenIdConnectPrompt
    {
        /// <summary>
        /// Indicates the 'none' prompt type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest"/>.
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// Indicates the 'create' prompt type. See: <see href="https://openid.net/specs/openid-connect-prompt-create-1_0.html"/>.
        /// </summary>
        public const string Create = "create";

        /// <summary>
        /// Indicates the 'login' prompt type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest"/>.
        /// </summary>
        public const string Login = "login";

        /// <summary>
        /// Indicates the 'consent' prompt type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest"/>.
        /// </summary>
        public const string Consent = "consent";

        /// <summary>
        /// Indicates the 'select_account' prompt type. See: <see href="https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest"/>.
        /// </summary>
        public const string SelectAccount = "select_account";
    }
}

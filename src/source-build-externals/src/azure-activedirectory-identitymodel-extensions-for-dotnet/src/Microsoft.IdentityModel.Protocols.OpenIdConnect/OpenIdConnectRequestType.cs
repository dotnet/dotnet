// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    /// <summary>
    /// RequestTypes for OpenIdConnect.
    /// </summary>
    /// <remarks>Can be used to determine the message type by consumers of an <see cref="OpenIdConnectMessage"/>.
    /// For example: <see cref="OpenIdConnectMessage.CreateAuthenticationRequestUrl"/> sets <see cref="OpenIdConnectMessage.RequestType"/>
    /// to <see cref="OpenIdConnectRequestType.Authentication"/>.</remarks>
    public enum OpenIdConnectRequestType
    {
        /// <summary>
        /// Indicates an Authentication Request see: http://openid.net/specs/openid-connect-core-1_0.html#AuthRequest.
        /// </summary>
        Authentication,

        /// <summary>
        /// Indicates a Logout Request see:http://openid.net/specs/openid-connect-frontchannel-1_0.html#RPLogout.
        /// </summary>
        Logout,

        /// <summary>
        /// Indicates a Token Request see: http://openid.net/specs/openid-connect-core-1_0.html#TokenRequest.
        /// </summary>
        Token,
    }
}

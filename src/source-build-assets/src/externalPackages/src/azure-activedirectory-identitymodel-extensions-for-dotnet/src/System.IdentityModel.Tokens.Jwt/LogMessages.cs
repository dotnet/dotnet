// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// System.IdentityModel.Tokens.Jwt
// Range: 12000 - 12999

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591
        // token creation
        internal const string IDX12401 = "IDX12401: Expires: '{0}' must be after NotBefore: '{1}'.";

        // JWT messages
        //internal const string IDX12700 = "IDX12700:";
        //internal const string IDX12701 = "IDX12701:";
        internal const string IDX12706 = "IDX12706: '{0}' can only write SecurityTokens of type: '{1}', 'token' type is: '{2}'.";
        internal const string IDX12709 = "IDX12709: CanReadToken() returned false. JWT is not well formed.\nThe token needs to be in JWS or JWE Compact Serialization Format. (JWS): 'EncodedHeader.EncodedPayload.EncodedSignature'. (JWE): 'EncodedProtectedHeader.EncodedEncryptedKey.EncodedInitializationVector.EncodedCiphertext.EncodedAuthenticationTag'.";
        internal const string IDX12710 = "IDX12710: Only a single 'Actor' is supported. Found second claim of type: '{0}', value: '{1}'";
        internal const string IDX12711 = "IDX12711: actor.BootstrapContext is not a string AND actor.BootstrapContext is not a JWT";
        internal const string IDX12712 = "IDX12712: actor.BootstrapContext is null. Creating the token using actor.Claims.";
        internal const string IDX12713 = "IDX12713: Creating actor value using actor.BootstrapContext(as string)";
        internal const string IDX12714 = "IDX12714: Creating actor value using actor.BootstrapContext.rawData";
        internal const string IDX12715 = "IDX12715: Creating actor value by writing the JwtSecurityToken created from actor.BootstrapContext";
        // internal const string IDX12716 = "IDX12716:";
        internal const string IDX12720 = "IDX12720: Token string does not match the token formats: JWE (header.encryptedKey.iv.ciphertext.tag) or JWS (header.payload.signature)";
        internal const string IDX12721 = "IDX12721: Creating JwtSecurityToken: Issuer: '{0}', Audience: '{1}'";
        internal const string IDX12722 = "IDX12722: Creating security token from the header: '{0}', payload: '{1}'.";
        internal const string IDX12723 = "IDX12723: Unable to decode the payload '{0}' as Base64Url encoded string.";
        internal const string IDX12729 = "IDX12729: Unable to decode the header '{0}' as Base64Url encoded string.";
        internal const string IDX12730 = "IDX12730: Failed to create the token encryption provider.";
        internal const string IDX12735 = "IDX12735: If JwtSecurityToken.InnerToken != null, then JwtSecurityToken.Header.EncryptingCredentials must be set.";
        internal const string IDX12736 = "IDX12736: JwtSecurityToken.SigningCredentials is not supported when JwtSecurityToken.InnerToken is set.";
        internal const string IDX12737 = "IDX12737: EncryptingCredentials set on JwtSecurityToken.InnerToken is not supported.";
        internal const string IDX12738 = "IDX12738: Header.Cty != null, assuming JWS. Cty: '{0}'.";
        internal const string IDX12739 = "IDX12739: JWT has three segments but is not in proper JWS format.";
        internal const string IDX12740 = "IDX12740: JWT has five segments but is not in proper JWE format.";
        internal const string IDX12741 = "IDX12741: JWT must have three segments (JWS) or five segments (JWE).";
        internal const string IDX12742 = "IDX12742: ''{0}' cannot contain the following claims: '{1}'. These values are added by default (if necessary) during security token creation.";
#pragma warning restore 1591
    }
}

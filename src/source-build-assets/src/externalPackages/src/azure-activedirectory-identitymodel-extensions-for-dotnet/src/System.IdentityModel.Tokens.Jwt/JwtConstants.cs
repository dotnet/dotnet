// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// Constants for Json Web tokens.
    /// </summary>
    public static class JwtConstants
    {
        /// <summary>
        /// Short header type.
        /// </summary>
        public const string HeaderType = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.HeaderType;

        /// <summary>
        /// Long header type.
        /// </summary>
        public const string HeaderTypeAlt = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.HeaderTypeAlt;

        /// <summary>
        /// Short token type.
        /// </summary>
        public const string TokenType = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.TokenType;

        /// <summary>
        /// Long token type.
        /// </summary>
        public const string TokenTypeAlt = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.TokenTypeAlt;

        /// <summary>
        /// JWS - Token format: 'header.payload.signature'. Signature is optional, but '.' is required.
        /// </summary>
        public const string JsonCompactSerializationRegex = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.JsonCompactSerializationRegex;

        /// <summary>
        /// JWE - Token format: 'protectedheader.encryptedkey.iv.cyphertext.authenticationtag'.
        /// </summary>
        public const string JweCompactSerializationRegex = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.JweCompactSerializationRegex;

        /// <summary>
        /// The number of parts in a JWE token.
        /// </summary>
        internal const int JweSegmentCount = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.JweSegmentCount;

        /// <summary>
        /// The number of parts in a JWS token.
        /// </summary>
        internal const int JwsSegmentCount = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.JwsSegmentCount;

        /// <summary>
        /// The maximum number of parts in a JWT.
        /// </summary>
        internal const int MaxJwtSegmentCount = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.MaxJwtSegmentCount;

        /// <summary>
        /// JWE header alg indicating a shared symmetric key is directly used as CEK.
        /// </summary>
        public const string DirectKeyUseAlg = Microsoft.IdentityModel.JsonWebTokens.JwtConstants.DirectKeyUseAlg;
    }
}

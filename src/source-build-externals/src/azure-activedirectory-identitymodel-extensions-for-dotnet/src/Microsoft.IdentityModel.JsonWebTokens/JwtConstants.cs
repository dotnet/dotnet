// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// Constants for Json Web Tokens.
    /// </summary>
    public static class JwtConstants
    {
        /// <summary>
        /// Short header type.
        /// </summary>
        public const string HeaderType = "JWT";

        /// <summary>
        /// Long header type.
        /// </summary>
        public const string HeaderTypeAlt = "http://openid.net/specs/jwt/1.0";

        /// <summary>
        /// Short token type.
        /// </summary>
        public const string TokenType = "JWT";

        /// <summary>
        /// Long token type.
        /// </summary>
        public const string TokenTypeAlt = "urn:ietf:params:oauth:token-type:jwt";

        /// <summary>
        /// JWS - Token format: 'header.payload.signature'. Signature is optional, but '.' is required.
        /// </summary>
        public const string JsonCompactSerializationRegex = @"^[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]*$";

        /// <summary>
        /// JWE - Token format: 'protectedheader.encryptedkey.iv.cyphertext.authenticationtag'.
        /// </summary>
        public const string JweCompactSerializationRegex = @"^[A-Za-z0-9-_]+\.[A-Za-z0-9-_]*\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+\.[A-Za-z0-9-_]+$";

        /// <summary>
        /// The number of parts in a JWE token.
        /// </summary>
        public const int JweSegmentCount = 5;

        /// <summary>
        /// The number of parts in a JWS token.
        /// </summary>
        public const int JwsSegmentCount = 3;

        /// <summary>
        /// The maximum number of parts in a JWT.
        /// </summary>
        public const int MaxJwtSegmentCount = 5;

        /// <summary>
        /// JWE header alg indicating a shared symmetric key is directly used as CEK.
        /// </summary>
        public const string DirectKeyUseAlg = "dir";
    }
}

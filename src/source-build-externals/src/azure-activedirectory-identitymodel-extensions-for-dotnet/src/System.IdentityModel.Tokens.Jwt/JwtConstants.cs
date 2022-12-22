//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

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

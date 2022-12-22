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

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.SampleTests
{
    /// <summary>
    /// A class which trivially wraps Microsoft.IdentityModel token validation.
    /// </summary>
    /// <remarks>
    /// This class exists only as a trivial example of how to leverage Microsoft.IdentityModel token validation as to provide an
    /// example for how one might want to construct unit tests to confirm validation is handled correctly.
    /// <see cref="SampleTokenValidationClassTests"/> contains examples for how to leverage TestTokenCreator to validate this class.
    /// </remarks>
    class SampleTokenValidationClass
    {
        /// <summary>
        /// Initializes an instance of the <see cref="SampleTokenValidationClass"/>.
        /// </summary>
        public SampleTokenValidationClass()
        {
            JsonWebTokenHandler = new JsonWebTokenHandler();
            JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudience = "http://Default.Audience.com",
                ValidIssuer = "http://Default.Issuer.com",
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key
            };
        }

        /// <summary>
        /// Gets or sets the <see cref="TokenValidationParameters"/> used for the validation operations.
        /// </summary>
        public TokenValidationParameters TokenValidationParameters { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="JsonWebTokenHandler"/> instance used for the validation operations.
        /// </summary>
        public JsonWebTokenHandler JsonWebTokenHandler { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="JwtSecurityTokenHandler"/> instance used for the validation operations.
        /// </summary>
        public JwtSecurityTokenHandler JwtSecurityTokenHandler { get; set; }

        /// <summary>
        /// Validates the passed token using the instance's <see cref="JsonWebTokenHandler"/>.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        public void ValidateTokenShim(string token)
        {
            ValidateTokenShim(token, TokenValidationParameters);
        }

        /// <summary>
        /// Validates the passed token using the instance of the deprecated <see cref="JwtSecurityTokenHandler"/>.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        public void ValidateTokenShimWithDeprecatedModel(string token)
        {
            var result = ValidateTokenShimWithDeprecatedModel(token, TokenValidationParameters);
        }

        /// <summary>
        /// Validates the passed token using the instance's <see cref="JsonWebTokenHandler"/>.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <param name="tokenValidationParameters">
        /// The <see cref="TokenValidationParameters"/> to use instead of the instance's value.
        /// </param>
        public void ValidateTokenShim(string token, TokenValidationParameters tokenValidationParameters)
        {
            var result = JsonWebTokenHandler.ValidateToken(token, tokenValidationParameters);

            if (!result.IsValid)
            {
                throw new SampleTestTokenValidationException("Validation Issue Encountered", result.Exception);
            }
        }

        /// <summary>
        /// Validates the passed token using the instance of the deprecated <see cref="JwtSecurityTokenHandler"/>.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <param name="tokenValidationParameters">
        /// The <see cref="TokenValidationParameters"/> to use instead of the instance's value.
        /// </param>
        /// <returns>A <see cref="ClaimsPrincipal"/> representing the claims from the passed JWT.</returns>
        public ClaimsPrincipal ValidateTokenShimWithDeprecatedModel(string token, TokenValidationParameters overrideTokenValidationParameters)
        {
            try
            {
                SecurityToken validatedToken;
                return JwtSecurityTokenHandler.ValidateToken(token, overrideTokenValidationParameters, out validatedToken);
            }
            catch (Exception e)
            {
                throw new SampleTestTokenValidationException("Validation Issue Encountered", e);
            }
        }
    }
}

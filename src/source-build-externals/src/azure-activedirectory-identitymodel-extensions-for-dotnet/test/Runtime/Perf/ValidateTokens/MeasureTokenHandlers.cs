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

using System.IdentityModel.Tokens.Jwt;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml;
using Microsoft.IdentityModel.Tokens.Saml2;
using RuntimeCommon;

namespace RuntimeTests
{
    [MarkdownExporterAttribute.Default]
    [SimpleJob(RunStrategy.Throughput, launchCount: 5, warmupCount: 10, targetCount: 100)]
    public class MeasureTokenHandlers
    {
        private string _jwtToken;
        private string _saml1Token;
        private string _saml2Token;
        private TokenValidationParameters _tokenValidationParameters;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private JsonWebTokenHandler _jsonWebTokenHandler = new JsonWebTokenHandler();
        private SamlSecurityTokenHandler _saml1SecurityTokenHandler = new SamlSecurityTokenHandler();
        private Saml2SecurityTokenHandler _saml2SecurityTokenHandler = new Saml2SecurityTokenHandler();

        public MeasureTokenHandlers()
        {
            IdentityModelEventSource.ShowPII = true;
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = TestData.Audience,
                Claims = TestData.ClaimsDictionary,
                Issuer = TestData.Issuer,
                Subject = TestData.Subject,
                SigningCredentials = TestData.RsaSigningCredentials_2048Sha256
            };

            _jwtToken = _jsonWebTokenHandler.CreateToken(securityTokenDescriptor);
            _saml1Token = _saml1SecurityTokenHandler.WriteToken(_saml1SecurityTokenHandler.CreateToken(securityTokenDescriptor));
            _saml2Token = _saml2SecurityTokenHandler.WriteToken(_saml2SecurityTokenHandler.CreateToken(securityTokenDescriptor));
            _tokenValidationParameters = TestData.RsaTokenValidationParameters_2048_Public;
        }

        [Benchmark]
        public void JsonWebTokenHandlerValidateToken()
        {
            _jsonWebTokenHandler.ValidateToken(_jwtToken, _tokenValidationParameters);
        }

        [Benchmark]
        public void JwtSecurityTokenHandlerValidateToken()
        {
            _jwtSecurityTokenHandler.ValidateToken(_jwtToken, _tokenValidationParameters, out _);
        }

        [Benchmark]
        public void Saml1TokenHandlerValidateToken()
        {
            _saml1SecurityTokenHandler.ValidateToken(_saml1Token, _tokenValidationParameters, out _);
        }

        [Benchmark]
        public void Saml2TokenHandlerValidateToken()
        {
            _saml2SecurityTokenHandler.ValidateToken(_saml2Token, _tokenValidationParameters, out _);
        }
    }
}

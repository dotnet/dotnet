// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net8.0 --filter Microsoft.IdentityModel.Benchmarks.ValidateJWEAsyncTests*

    public class ValidateJWEAsyncTests
    {
        private JsonWebTokenHandler _jsonWebTokenHandler;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private SecurityTokenDescriptor _tokenDescriptor;
        private string _jwe;
        private TokenValidationParameters _validationParameters;

        [GlobalSetup]
        public void Setup()
        {
            _jsonWebTokenHandler = new JsonWebTokenHandler();
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = BenchmarkUtils.SigningCredentialsRsaSha256,
                EncryptingCredentials = BenchmarkUtils.EncryptingCredentialsAes256Sha512,
                Claims = BenchmarkUtils.Claims,
                TokenType = JsonWebTokens.JwtHeaderParameterNames.Jwk
            };

            _jwe = _jsonWebTokenHandler.CreateToken(_tokenDescriptor);
            _validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = BenchmarkUtils.SigningCredentialsRsaSha256.Key,
                TokenDecryptionKey = BenchmarkUtils.SymmetricEncryptionKey512,
                ValidAudience = BenchmarkUtils.Audience,
                ValidIssuer = BenchmarkUtils.Issuer
            };
        }

        [Benchmark]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateJWEAsync() => await _jsonWebTokenHandler.ValidateTokenAsync(_jwe, _validationParameters);

        [Benchmark]
        public ClaimsPrincipal JwtSecurityTokenHandler_ValidateJWEAsync() => _jwtSecurityTokenHandler.ValidateToken(_jwe, _validationParameters, out SecurityToken _securityToken);
    }
}

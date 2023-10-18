// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Benchmarks
{
    [Config(typeof(AntiVirusFriendlyConfig))]
    [HideColumns("Type", "Job", "WarmupCount", "LaunchCount")]
    [MemoryDiagnoser]
    public class ValidateJWEAsyncTests
    {
        private JsonWebTokenHandler _jsonWebTokenHandler;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private SecurityTokenDescriptor _tokenDescriptor;
        private string _jweFromJsonHandler;
        private string _jweFromJwtHandler;
        private TokenValidationParameters _validationParameters;

        [GlobalSetup]
        public void Setup()
        {
            _jsonWebTokenHandler = new JsonWebTokenHandler();
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
                EncryptingCredentials = KeyingMaterial.DefaultSymmetricEncryptingCreds_Aes256_Sha512_512,
                Subject = new ClaimsIdentity(Default.PayloadClaims),
                TokenType = "TokenType"
            };
            _jweFromJsonHandler = _jsonWebTokenHandler.CreateToken(_tokenDescriptor);
            _jweFromJwtHandler = _jwtSecurityTokenHandler.CreateEncodedJwt(_tokenDescriptor);
            _validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
                TokenDecryptionKey = KeyingMaterial.DefaultSymmetricSecurityKey_512,
                ValidAudience = Default.Audience,
                ValidIssuer = Default.Issuer
            };
        }

        [Benchmark]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateJWEAsync() => await _jsonWebTokenHandler.ValidateTokenAsync(_jweFromJsonHandler, _validationParameters);

        [Benchmark]
        public ClaimsPrincipal JwtSecurityTokenHandler_ValidateJWEAsync() => _jwtSecurityTokenHandler.ValidateToken(_jweFromJwtHandler, _validationParameters, out _);
    }
}

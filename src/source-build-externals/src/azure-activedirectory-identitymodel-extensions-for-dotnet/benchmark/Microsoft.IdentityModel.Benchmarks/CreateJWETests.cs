// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    [Config(typeof(AntiVirusFriendlyConfig))]
    [HideColumns("Type", "Job", "WarmupCount", "LaunchCount")]
    [MemoryDiagnoser]
    public class CreateJWETests
    {
        private JsonWebTokenHandler _jsonWebTokenHandler;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private SecurityTokenDescriptor _tokenDescriptor;

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
        }

        [Benchmark]
        public string JsonWebTokenHandler_CreateJWE() => _jsonWebTokenHandler.CreateToken(_tokenDescriptor);

        [Benchmark]
        public string JwtSecurityTokenHandler_CreateJWE() => _jwtSecurityTokenHandler.CreateEncodedJwt(_tokenDescriptor);
    }
}

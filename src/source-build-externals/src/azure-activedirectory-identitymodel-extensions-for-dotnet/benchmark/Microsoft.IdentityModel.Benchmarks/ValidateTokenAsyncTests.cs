// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.TestUtils;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Benchmarks
{
    [Config(typeof(AntiVirusFriendlyConfig))]
    [HideColumns("Type", "Job", "WarmupCount", "LaunchCount")]
    [MemoryDiagnoser]
    public class ValidateTokenAsyncTests
    {
        private JsonWebTokenHandler _jsonWebTokenHandler;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private SecurityTokenDescriptor _tokenDescriptor;
        private string _jsonWebToken;
        private TokenValidationParameters _validationParameters;

        [GlobalSetup]
        public void Setup()
        {
            _tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = BenchmarkUtils.SimpleClaims,
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            };
            _jsonWebToken = _jsonWebTokenHandler.CreateToken(_tokenDescriptor);
            _validationParameters = new TokenValidationParameters()
            {
                ValidAudience = Default.Audience,
                ValidateLifetime = true,
                ValidIssuer = Default.Issuer,
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
            };
        }

        [GlobalSetup(Targets = new[] { nameof(JsonWebTokenHandler_ValidateTokenAsync) })]
        public void JsonWebTokenSetup()
        {
            _jsonWebTokenHandler = new JsonWebTokenHandler();
            _jsonWebTokenHandler.SetDefaultTimesOnTokenCreation = false;
        }

        [Benchmark]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsync() => await _jsonWebTokenHandler.ValidateTokenAsync(_jsonWebToken, _validationParameters).ConfigureAwait(false);

        [GlobalSetup(Targets = new[] { nameof(JwtSecurityTokenHandler_ValidateTokenAsync) })]
        public void JwtSecurityTokenSetup()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _jwtSecurityTokenHandler.SetDefaultTimesOnTokenCreation = false;
        }

        [Benchmark]
        public async Task<TokenValidationResult> JwtSecurityTokenHandler_ValidateTokenAsync() => await _jwtSecurityTokenHandler.ValidateTokenAsync(_jsonWebToken, _validationParameters).ConfigureAwait(false);

    }
}

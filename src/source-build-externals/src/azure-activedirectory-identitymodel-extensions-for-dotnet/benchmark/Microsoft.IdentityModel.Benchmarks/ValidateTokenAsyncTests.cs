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
    public class ValidateTokenAsyncTests
    {
        JsonWebTokenHandler jsonWebTokenHandler;
        JwtSecurityTokenHandler jwtSecurityTokenHandler;
        SecurityTokenDescriptor tokenDescriptor;
        string jsonWebToken;
        TokenValidationParameters validationParameters;

        [GlobalSetup]
        public void Setup()
        {
            tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = BenchmarkUtils.SimpleClaims,
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            };
            jsonWebToken = jsonWebTokenHandler.CreateToken(tokenDescriptor);
            validationParameters = new TokenValidationParameters()
            {
                ValidAudience = "http://Default.Audience.com",
                ValidateLifetime = true,
                ValidIssuer = "http://Default.Issuer.com",
                IssuerSigningKey = KeyingMaterial.JsonWebKeyRsa256SigningCredentials.Key,
            };
        }

        [GlobalSetup(Targets = new[] { nameof(JsonWebTokenHandler_ValidateTokenAsync) })]
        public void JsonWebTokenSetup()
        {
            jsonWebTokenHandler = new JsonWebTokenHandler();
            jsonWebTokenHandler.SetDefaultTimesOnTokenCreation = false;
        }

        [Benchmark]
        public async Task<TokenValidationResult> JsonWebTokenHandler_ValidateTokenAsync() => await jsonWebTokenHandler.ValidateTokenAsync(jsonWebToken, validationParameters).ConfigureAwait(false);

        [GlobalSetup(Targets = new[] { nameof(JwtSecurityTokenHandler_ValidateTokenAsync) })]
        public void JwtSecurityTokenSetup()
        {
            jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.SetDefaultTimesOnTokenCreation = false;
        }

        [Benchmark]
        public async Task<TokenValidationResult> JwtSecurityTokenHandler_ValidateTokenAsync() => await jwtSecurityTokenHandler.ValidateTokenAsync(jsonWebToken, validationParameters).ConfigureAwait(false);

    }
}

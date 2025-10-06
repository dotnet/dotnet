// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IdentityModel.Tokens.Jwt;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net8.0 --filter Microsoft.IdentityModel.Benchmarks.CreateJWETests*

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
                SigningCredentials = BenchmarkUtils.SigningCredentialsRsaSha256,
                EncryptingCredentials = BenchmarkUtils.EncryptingCredentialsAes256Sha512,
                Claims = BenchmarkUtils.Claims
            };
        }

        [Benchmark]
        public string JsonWebTokenHandler_CreateJWE() => _jsonWebTokenHandler.CreateToken(_tokenDescriptor);

        [Benchmark]
        public string JwtSecurityTokenHandler_CreateJWE() => _jwtSecurityTokenHandler.CreateEncodedJwt(_tokenDescriptor);

    }
}

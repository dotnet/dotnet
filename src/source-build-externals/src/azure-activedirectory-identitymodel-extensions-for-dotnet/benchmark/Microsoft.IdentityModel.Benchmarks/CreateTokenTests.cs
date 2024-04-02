// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net8.0 --filter Microsoft.IdentityModel.Benchmarks.CreateTokenTests*

    public class CreateTokenTests
    {
        private JsonWebTokenHandler _jsonWebTokenHandler;
        private SecurityTokenDescriptor _tokenDescriptor;

        [GlobalSetup]
        public void Setup()
        {
            DateTime now = DateTime.UtcNow;
            _jsonWebTokenHandler = new JsonWebTokenHandler();
            _tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = BenchmarkUtils.Claims,
                SigningCredentials = BenchmarkUtils.SigningCredentialsRsaSha256,
            };
        }

        [Benchmark]
        public string JsonWebTokenHandler_CreateToken() => _jsonWebTokenHandler.CreateToken(_tokenDescriptor);
    }
}

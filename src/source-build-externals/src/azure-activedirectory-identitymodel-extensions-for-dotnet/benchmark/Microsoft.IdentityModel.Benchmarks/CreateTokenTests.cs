// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Claims;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    [HideColumns("Type", "Job", "WarmupCount", "LaunchCount")]
    [MemoryDiagnoser]
    public class CreateTokenTests
    {
        private JsonWebTokenHandler _jsonWebTokenHandler;
        private SecurityTokenDescriptor _tokenDescriptor;

        [GlobalSetup]
        public void Setup()
        {
            _jsonWebTokenHandler = new JsonWebTokenHandler();
            _tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Default.PayloadClaims),
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            };
        }

        [Benchmark]
        public string JsonWebTokenHandler_CreateToken() => _jsonWebTokenHandler.CreateToken(_tokenDescriptor);

    }
}

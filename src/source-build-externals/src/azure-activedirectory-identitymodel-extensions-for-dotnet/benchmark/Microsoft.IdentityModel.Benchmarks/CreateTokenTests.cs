// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Security.Claims;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    public class CreateTokenTests
    {
        JsonWebTokenHandler jsonWebTokenHandler;
        SecurityTokenDescriptor tokenDescriptor;

        [GlobalSetup]
        public void Setup()
        {
            jsonWebTokenHandler = new JsonWebTokenHandler();
            tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Default.PayloadClaims),
                SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials,
            };
        }

        [Benchmark]
        public string JsonWebTokenHandler_CreateToken() => jsonWebTokenHandler.CreateToken(tokenDescriptor);

    }
}

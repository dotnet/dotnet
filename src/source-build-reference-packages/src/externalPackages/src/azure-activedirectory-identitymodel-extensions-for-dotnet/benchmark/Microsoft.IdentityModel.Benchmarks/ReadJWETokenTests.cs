// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net8.0 --filter Microsoft.IdentityModel.Benchmarks.ReadTokenTests*

    public class ReadJWETokenTests
    {
        string _encryptedJWE;
        ReadOnlyMemory<char> _encryptedJWEAsMemory;

        [GlobalSetup]
        public void Setup()
        {
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jweTokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = BenchmarkUtils.SigningCredentialsRsaSha256,
                EncryptingCredentials = BenchmarkUtils.EncryptingCredentialsAes256Sha512,
                TokenType = JwtHeaderParameterNames.Jwk,
                Claims = BenchmarkUtils.Claims
            };

            _encryptedJWE = jsonWebTokenHandler.CreateToken(jweTokenDescriptor);
            _encryptedJWEAsMemory = _encryptedJWE.AsMemory();
        }

        [Benchmark]
        public JsonWebToken ReadJWE_FromString()
        {
            return new JsonWebToken(_encryptedJWE);
        }

        [Benchmark]
        public JsonWebToken ReadJWE_FromMemory()
        {
            return new JsonWebToken(_encryptedJWEAsMemory);
        }
    }
}

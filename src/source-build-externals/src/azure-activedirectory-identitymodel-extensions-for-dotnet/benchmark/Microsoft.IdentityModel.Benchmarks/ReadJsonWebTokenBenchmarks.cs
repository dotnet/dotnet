// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    public class ReadJsonWebTokenBenchmarks
    {
        private JsonWebTokenHandler _handler;
        private string _smallToken;
        private string _largeToken;

        [GlobalSetup]
        public void Setup()
        {
            _handler = new JsonWebTokenHandler();

            // Small token with minimal claims
            _smallToken = _handler.CreateToken(
                Default.PayloadString,
                KeyingMaterial.JsonWebKeyRsa256SigningCredentials);

            // Large token with many claims
            var claims = Default.PayloadClaims;
            for (int i = 0; i < 50; i++)
                claims.Add(new System.Security.Claims.Claim($"claim{i}", $"value{i}"));

            _largeToken = _handler.CreateToken(
                new SecurityTokenDescriptor
                {
                    Subject = new CaseSensitiveClaimsIdentity(claims),
                    SigningCredentials = KeyingMaterial.JsonWebKeyRsa256SigningCredentials
                });
        }

        [Benchmark(Baseline = true)]
        public JsonWebToken ReadJsonWebToken_String_Small()
        {
            return _handler.ReadJsonWebToken(_smallToken);
        }

        [Benchmark]
        public JsonWebToken ReadJsonWebToken_Span_Small()
        {
            return _handler.ReadJsonWebToken(_smallToken.AsMemory());
        }

        [Benchmark]
        public JsonWebToken ReadJsonWebToken_String_Large()
        {
            return _handler.ReadJsonWebToken(_largeToken);
        }

        [Benchmark]
        public JsonWebToken ReadJsonWebToken_Span_Large()
        {
            return _handler.ReadJsonWebToken(_largeToken.AsMemory());
        }
    }
}

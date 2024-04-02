// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Protocols.SignedHttpRequest;
using Microsoft.IdentityModel.Tokens;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;

namespace Microsoft.IdentityModel.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //DebugThroughTests();

#if DEBUG
            var benchmarkConfig = ManualConfig.Union(DefaultConfig.Instance, new DebugInProcessConfig()); // Allows debugging into benchmarks
#else
            var benchmarkConfig = ManualConfig.Union(DefaultConfig.Instance, new BenchmarkConfig());
#endif

            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, benchmarkConfig);
        }
        private static void DebugThroughTests()
        {
            ReadJWETokenTests readTokenTests = new ReadJWETokenTests();
            readTokenTests.Setup();
            readTokenTests.ReadJWE_FromMemory();

            AsymmetricAdapterSignatures asymmetricAdapter = new AsymmetricAdapterSignatures();
            asymmetricAdapter.Setup();
            asymmetricAdapter.SignDotnetCreatingBufferRSA();
            asymmetricAdapter.SignSpanWithArrayPoolRSA();
            asymmetricAdapter.SignSpanWithFixedBufferRSA();

            CreateJWETests createJWETests = new CreateJWETests();
            createJWETests.Setup();
            string jwe = createJWETests.JsonWebTokenHandler_CreateJWE();

            CreateSignedHttpRequestTests createSignedHttpRequestTests = new CreateSignedHttpRequestTests();
            createSignedHttpRequestTests.Setup();
            string shr = createSignedHttpRequestTests.SHRHandler_CreateSignedHttpRequest();

            CreateTokenTests createTokenTests = new CreateTokenTests();
            createTokenTests.Setup();
            string jws = createTokenTests.JsonWebTokenHandler_CreateToken();

            ValidateTokenAsyncTests validateTokenAsyncTests = new ValidateTokenAsyncTests();
            validateTokenAsyncTests.Setup();
            TokenValidationResult tokenValidationResult = validateTokenAsyncTests.JsonWebTokenHandler_ValidateTokenAsync().Result;

            ValidateSignedHttpRequestAsyncTests validateSignedHttpRequestAsyncTests = new ValidateSignedHttpRequestAsyncTests();
            validateSignedHttpRequestAsyncTests.Setup();
            SignedHttpRequestValidationResult signedHttpRequestValidationResult = validateSignedHttpRequestAsyncTests.SHRHandler_ValidateSignedHttpRequestAsync().Result;
        }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.Protocols.SignedHttpRequest;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net8.0 --filter Microsoft.IdentityModel.Benchmarks.CreateSignedHttpRequestTests*

    public class CreateSignedHttpRequestTests
    {
        private SignedHttpRequestHandler _signedHttpRequestHandler;
        private SignedHttpRequestDescriptor _signedHttpRequestDescriptor;

        [GlobalSetup]
        public void Setup()
        {
            _signedHttpRequestHandler = new SignedHttpRequestHandler();
            _signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(
                    BenchmarkUtils.CreateAccessTokenWithCnf(),
                    BenchmarkUtils.HttpRequestData,
                    BenchmarkUtils.SigningCredentialsRsaSha256,
                    new SignedHttpRequestCreationParameters()
                    {
                        CreateM = true,
                        CreateP = false,
                        CreateU = true
                    });
        }

        [Benchmark]
        public string SHRHandler_CreateSignedHttpRequest() => _signedHttpRequestHandler.CreateSignedHttpRequest(_signedHttpRequestDescriptor);
    }
}

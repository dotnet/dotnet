// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests;
using Microsoft.IdentityModel.Protocols.SignedHttpRequest;
using Microsoft.IdentityModel.Protocols;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Benchmarks
{
    [HideColumns("Type", "Job", "WarmupCount", "LaunchCount")]
    [MemoryDiagnoser]
    public class ValidateSignedHttpRequestAsyncTests
    {
        private SignedHttpRequestHandler _signedHttpRequestHandler;
        private SignedHttpRequestDescriptor _signedHttpRequestDescriptor;
        private SignedHttpRequestValidationContext _validationContext;
        private string _signedHttpRequest;

        [GlobalSetup]
        public void Setup()
        {
            var requestData = new HttpRequestData();
            _signedHttpRequestHandler = new SignedHttpRequestHandler();
            _signedHttpRequestDescriptor = new SignedHttpRequestDescriptor(
                    SignedHttpRequestTestUtils.DefaultEncodedAccessToken,
                    requestData,
                    SignedHttpRequestTestUtils.DefaultSigningCredentials,
                    new SignedHttpRequestCreationParameters()
                    {
                        CreateM = false,
                        CreateP = false,
                        CreateU = false
                    });
            _signedHttpRequest = _signedHttpRequestHandler.CreateSignedHttpRequest(_signedHttpRequestDescriptor);
            _validationContext = new SignedHttpRequestValidationContext(
                _signedHttpRequest,
                requestData,
                SignedHttpRequestTestUtils.DefaultTokenValidationParameters);
        }

        [Benchmark]
        public async Task<SignedHttpRequestValidationResult> SHRHandler_ValidateSignedHttpRequestAsync() => await _signedHttpRequestHandler.ValidateSignedHttpRequestAsync(_validationContext, CancellationToken.None);
    }
}

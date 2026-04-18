// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.IdentityModel.Protocols.SignedHttpRequest;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Benchmarks
{
    // dotnet run -c release -f net8.0 --filter Microsoft.IdentityModel.Benchmarks.ValidateSignedHttpRequestAsyncTests*

    public class ValidateSignedHttpRequestAsyncTests
    {
        private SignedHttpRequestHandler _signedHttpRequestHandler;
        private SignedHttpRequestValidationContext _validationContext;

        [GlobalSetup]
        public void Setup()
        {
            _signedHttpRequestHandler = new SignedHttpRequestHandler();
            _validationContext = new SignedHttpRequestValidationContext(
                    _signedHttpRequestHandler.CreateSignedHttpRequest(
                        new SignedHttpRequestDescriptor(
                            BenchmarkUtils.CreateAccessTokenWithCnf(),
                            BenchmarkUtils.HttpRequestData,
                            BenchmarkUtils.SigningCredentialsRsaSha256,
                            new SignedHttpRequestCreationParameters()
                            {
                                CreateM = true,
                                CreateP = false,
                                CreateU = true
                            })),
                    BenchmarkUtils.HttpRequestData,
                    new TokenValidationParameters
                    {
                        IssuerSigningKey = BenchmarkUtils.SigningCredentialsRsaSha256.Key,
                        ValidIssuer = BenchmarkUtils.Issuer,
                        ValidAudience = BenchmarkUtils.Audience,
                        TokenDecryptionKey = BenchmarkUtils.EncryptingCredentialsAes256Sha512.Key
                    },
                    new SignedHttpRequestValidationParameters
                    {
                        ValidateP = false
                    });
        }

        [Benchmark]
        public async Task<SignedHttpRequestValidationResult> SHRHandler_ValidateSignedHttpRequestAsync() => await _signedHttpRequestHandler.ValidateSignedHttpRequestAsync(_validationContext, CancellationToken.None);
    }
}

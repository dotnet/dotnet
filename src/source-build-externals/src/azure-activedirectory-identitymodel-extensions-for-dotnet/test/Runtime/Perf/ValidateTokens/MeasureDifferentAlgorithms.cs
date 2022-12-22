//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using RuntimeCommon;

namespace RuntimeTests
{
    [MarkdownExporterAttribute.Default]
    [SimpleJob(RunStrategy.Throughput, launchCount: 5, warmupCount: 2, targetCount: 5)]
    public class MeasureDifferentAlgorithms
    {
        private string _jwtTokenEcd256;
        private string _jwtTokenEcd512;
        private string _jwtTokenRsa256;
        private string _jwtTokenRsa512;
        private string _jwtTokenSymmetric256;

        private TokenValidationParameters _tokenValidationParametersEcd256;
        private TokenValidationParameters _tokenValidationParametersEcd512;
        private TokenValidationParameters _tokenValidationParametersRsa256;
        private TokenValidationParameters _tokenValidationParametersRsa512;
        private TokenValidationParameters _tokenValidationParametersSymmetric256;

        private JsonWebTokenHandler _jsonWebTokenHandler = new JsonWebTokenHandler();

        public MeasureDifferentAlgorithms()
        {
            IdentityModelEventSource.ShowPII = true;
            CryptoProviderFactory.DefaultCacheSignatureProviders = false;
            _jwtTokenEcd256 = _jsonWebTokenHandler.CreateToken(TestData.SecurityTokenDescriptor(TestData.EcdSigningCredentials_2048Sha256));
            _jwtTokenEcd512 = _jsonWebTokenHandler.CreateToken(TestData.SecurityTokenDescriptor(TestData.EcdSigningCredentials_2048Sha512));
            _jwtTokenRsa256 = _jsonWebTokenHandler.CreateToken(TestData.SecurityTokenDescriptor(TestData.RsaSigningCredentials_2048Sha256));
            _jwtTokenRsa512 = _jsonWebTokenHandler.CreateToken(TestData.SecurityTokenDescriptor(TestData.RsaSigningCredentials_2048Sha512));
            _jwtTokenSymmetric256 = _jsonWebTokenHandler.CreateToken(TestData.SecurityTokenDescriptor(TestData.SymmetricSigningCreds_256Sha256));

            _tokenValidationParametersEcd256 = TestData.TokenValidationParameters(TestData.EcdSigningCredentials_2048Sha256.Key);
            _tokenValidationParametersEcd512 = TestData.TokenValidationParameters(TestData.EcdSigningCredentials_2048Sha512.Key);
            _tokenValidationParametersRsa256 = TestData.TokenValidationParameters(TestData.RsaSigningCredentials_2048Sha512.Key);
            _tokenValidationParametersRsa512 = TestData.TokenValidationParameters(TestData.RsaSigningCredentials_2048Sha512.Key);
            _tokenValidationParametersSymmetric256 = TestData.TokenValidationParameters(TestData.SymmetricSigningCreds_256Sha256.Key);
        }

        [IterationSetup]
        public void IterationSetup()
        {
            CryptoProviderFactory.DefaultCacheSignatureProviders = false;
        }

        [Benchmark]
        public void JsonWebTokenHandlerValidateTokenEcd256()
        {
            _jsonWebTokenHandler.ValidateToken(_jwtTokenEcd256, _tokenValidationParametersEcd256);
        }

        [Benchmark]
        public void JsonWebTokenHandlerValidateTokenEcd512()
        {
            _jsonWebTokenHandler.ValidateToken(_jwtTokenEcd512, _tokenValidationParametersEcd512);
        }

        [Benchmark]
        public void JsonWebTokenHandlerValidateTokenRsa256()
        {
            _jsonWebTokenHandler.ValidateToken(_jwtTokenRsa256, _tokenValidationParametersRsa256);
        }

        [Benchmark]
        public void JsonWebTokenHandlerValidateTokenRsa512()
        {
            _jsonWebTokenHandler.ValidateToken(_jwtTokenRsa512, _tokenValidationParametersRsa512);
        }
        [Benchmark]

        public void JsonWebTokenHandlerValidateTokenSymmetric256()
        {
            _jsonWebTokenHandler.ValidateToken(_jwtTokenSymmetric256, _tokenValidationParametersSymmetric256);
        }
    }
}

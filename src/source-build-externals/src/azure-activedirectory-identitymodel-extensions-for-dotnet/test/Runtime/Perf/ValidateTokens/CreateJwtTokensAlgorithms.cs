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
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 2, targetCount: 50)]
    public class CreateJwtTokensAlgorithms
    {
        private JsonWebTokenHandler _jsonWebTokenHandler = new JsonWebTokenHandler();

        private SecurityTokenDescriptor _tokenDescriptorEcd256;
        private SecurityTokenDescriptor _tokenDescriptorEcd512;
        private SecurityTokenDescriptor _tokenDescriptorRsa256;
        private SecurityTokenDescriptor _tokenDescriptorRsa512;
        private SecurityTokenDescriptor _tokenDescriptorSymmetric256;

        public CreateJwtTokensAlgorithms()
        {
            IdentityModelEventSource.ShowPII = true;
            CryptoProviderFactory.DefaultCacheSignatureProviders = false;
            _tokenDescriptorEcd256 = TestData.SecurityTokenDescriptor(TestData.EcdSigningCredentials_2048Sha256);
            _tokenDescriptorEcd512 = TestData.SecurityTokenDescriptor(TestData.EcdSigningCredentials_2048Sha512);
            _tokenDescriptorRsa256 = TestData.SecurityTokenDescriptor(TestData.RsaSigningCredentials_2048Sha256);
            _tokenDescriptorRsa512 = TestData.SecurityTokenDescriptor(TestData.RsaSigningCredentials_2048Sha512);
            _tokenDescriptorSymmetric256 = TestData.SecurityTokenDescriptor(TestData.SymmetricSigningCreds_256Sha256);
        }

        [Benchmark]
        public void JsonWebTokenHandlerCreateTokenEcd256()
        {
            _jsonWebTokenHandler.CreateToken(_tokenDescriptorEcd256);
        }

        [Benchmark]
        public void JsonWebTokenHandlerCreateTokenEcd512()
        {
            _jsonWebTokenHandler.CreateToken(_tokenDescriptorEcd512);
        }

        [Benchmark]
        public void JsonWebTokenHandlerCreateTokenRsa256()
        {
            _jsonWebTokenHandler.CreateToken(_tokenDescriptorRsa256);
        }

        [Benchmark]
        public void JsonWebTokenHandlerCreateTokenRsa512()
        {
            _jsonWebTokenHandler.CreateToken(_tokenDescriptorRsa512);
        }
        [Benchmark]

        public void JsonWebTokenHandlerCreateTokenSymmetric256()
        {
            _jsonWebTokenHandler.CreateToken(_tokenDescriptorSymmetric256);
        }
    }
}

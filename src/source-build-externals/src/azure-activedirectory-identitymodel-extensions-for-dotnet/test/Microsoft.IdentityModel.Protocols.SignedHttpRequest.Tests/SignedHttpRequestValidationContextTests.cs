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

using System;
using Microsoft.IdentityModel.Protocols.SignedHttpRequest;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    public class SignedHttpRequestValidationContextTests
    {
        [Fact]
        public void SignedHttpRequestValidationContext()
        {
            var httpRequestData = new HttpRequestData();
            var signedHttpRequest = SignedHttpRequestTestUtils.CreateDefaultSignedHttpRequestToken(SignedHttpRequestTestUtils.DefaultSignedHttpRequestPayload.ToString()).EncodedToken;
            var tokenValidationParameters = SignedHttpRequestTestUtils.DefaultTokenValidationParameters;
            var validationParameters = new SignedHttpRequestValidationParameters();
            var callContext = new CallContext();

            Assert.Throws<ArgumentNullException>("signedHttpRequest", () => new SignedHttpRequestValidationContext(null, httpRequestData, null, validationParameters, callContext));
            Assert.Throws<ArgumentNullException>("signedHttpRequest", () => new SignedHttpRequestValidationContext(null, httpRequestData, null));
            Assert.Throws<ArgumentNullException>("signedHttpRequest", () => new SignedHttpRequestValidationContext(null, httpRequestData, null, (SignedHttpRequestValidationParameters)null));
            Assert.Throws<ArgumentNullException>("signedHttpRequest", () => new SignedHttpRequestValidationContext(null, httpRequestData, null, (CallContext)null));
            Assert.Throws<ArgumentNullException>("signedHttpRequest", () => new SignedHttpRequestValidationContext(string.Empty, httpRequestData, null, validationParameters, callContext));
            Assert.Throws<ArgumentNullException>("signedHttpRequest", () => new SignedHttpRequestValidationContext(string.Empty, httpRequestData, null));
            Assert.Throws<ArgumentNullException>("signedHttpRequest", () => new SignedHttpRequestValidationContext(string.Empty, httpRequestData, null, (SignedHttpRequestValidationParameters)null));
            Assert.Throws<ArgumentNullException>("signedHttpRequest", () => new SignedHttpRequestValidationContext(string.Empty, httpRequestData, null, (CallContext)null));
            Assert.Throws<ArgumentNullException>("httpRequestData", () => new SignedHttpRequestValidationContext(signedHttpRequest, null, null, validationParameters, callContext));
            Assert.Throws<ArgumentNullException>("httpRequestData", () => new SignedHttpRequestValidationContext(signedHttpRequest, null, null));
            Assert.Throws<ArgumentNullException>("httpRequestData", () => new SignedHttpRequestValidationContext(signedHttpRequest, null, null, (SignedHttpRequestValidationParameters)null));
            Assert.Throws<ArgumentNullException>("httpRequestData", () => new SignedHttpRequestValidationContext(signedHttpRequest, null, null, (CallContext)null));
            Assert.Throws<ArgumentNullException>("accessTokenValidationParameters", () => new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, null, validationParameters, callContext));
            Assert.Throws<ArgumentNullException>("accessTokenValidationParameters", () => new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, null));
            Assert.Throws<ArgumentNullException>("accessTokenValidationParameters", () => new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, null, (SignedHttpRequestValidationParameters)null));
            Assert.Throws<ArgumentNullException>("accessTokenValidationParameters", () => new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, null, (CallContext)null));
            Assert.Throws<ArgumentNullException>("signedHttpRequestValidationParameters", () => new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, tokenValidationParameters, (SignedHttpRequestValidationParameters)null));
            Assert.Throws<ArgumentNullException>("signedHttpRequestValidationParameters", () => new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, tokenValidationParameters, null, callContext));
            Assert.Throws<ArgumentNullException>("callContext", () => new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, tokenValidationParameters, validationParameters, null));

            // no exceptions
            var signedHttpRequestDescriptor = new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, tokenValidationParameters);
            Assert.Equal(httpRequestData, signedHttpRequestDescriptor.HttpRequestData);
            Assert.Equal(signedHttpRequest, signedHttpRequestDescriptor.SignedHttpRequest);
            Assert.Equal(tokenValidationParameters, signedHttpRequestDescriptor.AccessTokenValidationParameters);
            Assert.NotNull(signedHttpRequestDescriptor.SignedHttpRequestValidationParameters);
            Assert.NotNull(signedHttpRequestDescriptor.CallContext);

            signedHttpRequestDescriptor = new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, tokenValidationParameters, callContext);
            Assert.Equal(httpRequestData, signedHttpRequestDescriptor.HttpRequestData);
            Assert.Equal(signedHttpRequest, signedHttpRequestDescriptor.SignedHttpRequest);
            Assert.Equal(tokenValidationParameters, signedHttpRequestDescriptor.AccessTokenValidationParameters);
            Assert.Equal(callContext, signedHttpRequestDescriptor.CallContext);
            Assert.NotNull(signedHttpRequestDescriptor.SignedHttpRequestValidationParameters);
            

            signedHttpRequestDescriptor = new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, tokenValidationParameters, validationParameters);
            Assert.Equal(httpRequestData, signedHttpRequestDescriptor.HttpRequestData);
            Assert.Equal(signedHttpRequest, signedHttpRequestDescriptor.SignedHttpRequest);
            Assert.Equal(tokenValidationParameters, signedHttpRequestDescriptor.AccessTokenValidationParameters);
            Assert.Equal(validationParameters, signedHttpRequestDescriptor.SignedHttpRequestValidationParameters);
            Assert.NotNull(signedHttpRequestDescriptor.CallContext);

            signedHttpRequestDescriptor = new SignedHttpRequestValidationContext(signedHttpRequest, httpRequestData, tokenValidationParameters, validationParameters, callContext);
            Assert.Equal(httpRequestData, signedHttpRequestDescriptor.HttpRequestData);
            Assert.Equal(signedHttpRequest, signedHttpRequestDescriptor.SignedHttpRequest);
            Assert.Equal(tokenValidationParameters, signedHttpRequestDescriptor.AccessTokenValidationParameters);
            Assert.Equal(validationParameters, signedHttpRequestDescriptor.SignedHttpRequestValidationParameters);
            Assert.Equal(callContext, signedHttpRequestDescriptor.CallContext);
        }
    }
}

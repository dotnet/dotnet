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

using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Structure that wraps parameters needed for SignedHttpRequest validation.
    /// </summary>
    public class SignedHttpRequestValidationContext
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SignedHttpRequestValidationContext"/>.
        /// </summary>
        /// <param name="signedHttpRequest">SignedHttpRequest to be validated as a JWS in Compact Serialization Format.</param>
        /// <param name="httpRequestData">A structure that represents an incoming http request.</param>
        /// <param name="accessTokenValidationParameters">A <see cref="TokenValidationParameters"/> required for access token ("at") validation.</param>
        /// <remarks>Default <see cref="SignedHttpRequestValidationParameters"/> and <see cref="CallContext"/> will be created.</remarks>
        public SignedHttpRequestValidationContext(string signedHttpRequest, HttpRequestData httpRequestData, TokenValidationParameters accessTokenValidationParameters) 
            : this (signedHttpRequest, httpRequestData, accessTokenValidationParameters, new SignedHttpRequestValidationParameters(), new CallContext())
        {
        }
        /// <summary>
        /// Initializes a new instance of <see cref="SignedHttpRequestValidationContext"/>.
        /// </summary>
        /// <param name="signedHttpRequest">SignedHttpRequest to be validated encoded as a JWS in Compact Serialization Format.</param>
        /// <param name="httpRequestData">A structure that represents an incoming http request.</param>
        /// <param name="accessTokenValidationParameters">A <see cref="TokenValidationParameters"/> required for access token ("at") validation.</param>
        /// <param name="signedHttpRequestValidationParameters">A set of parameters required for validating a SignedHttpRequest.</param>
        /// <remarks>Default <see cref="CallContext"/> will be created.</remarks>
        public SignedHttpRequestValidationContext(string signedHttpRequest, HttpRequestData httpRequestData, TokenValidationParameters accessTokenValidationParameters, SignedHttpRequestValidationParameters signedHttpRequestValidationParameters)
            : this(signedHttpRequest, httpRequestData, accessTokenValidationParameters, signedHttpRequestValidationParameters, new CallContext())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SignedHttpRequestValidationContext"/>.
        /// </summary>
        /// <param name="signedHttpRequest">SignedHttpRequest to be validated encoded as a JWS in Compact Serialization Format.</param>
        /// <param name="httpRequestData">A structure that represents an incoming http request.</param>
        /// <param name="accessTokenValidationParameters">A <see cref="TokenValidationParameters"/> required for access token ("at") validation.</param>
        /// <param name="callContext">An opaque context used to store work when working with authentication artifacts.</param>
        /// <remarks>Default <see cref="SignedHttpRequestValidationParameters"/> will be created.</remarks>
        public SignedHttpRequestValidationContext(string signedHttpRequest, HttpRequestData httpRequestData, TokenValidationParameters accessTokenValidationParameters, CallContext callContext)
            : this(signedHttpRequest, httpRequestData, accessTokenValidationParameters, new SignedHttpRequestValidationParameters(), callContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SignedHttpRequestValidationContext"/>.
        /// </summary>
        /// <param name="signedHttpRequest">SignedHttpRequest to be validated encoded as a JWS in Compact Serialization Format.</param>
        /// <param name="httpRequestData">A structure that represents an incoming http request.</param>
        /// <param name="accessTokenValidationParameters">A <see cref="TokenValidationParameters"/> required for access token ("at") validation.</param>
        /// <param name="signedHttpRequestValidationParameters">A set of parameters required for validating a SignedHttpRequest.</param>
        /// <param name="callContext">An opaque context used to store work when working with authentication artifacts.</param>
        public SignedHttpRequestValidationContext(string signedHttpRequest, HttpRequestData httpRequestData, TokenValidationParameters accessTokenValidationParameters, SignedHttpRequestValidationParameters signedHttpRequestValidationParameters, CallContext callContext) 
        {
            SignedHttpRequest = !string.IsNullOrEmpty(signedHttpRequest) ? signedHttpRequest : throw LogHelper.LogArgumentNullException(nameof(signedHttpRequest));
            HttpRequestData = httpRequestData ?? throw LogHelper.LogArgumentNullException(nameof(httpRequestData));
            AccessTokenValidationParameters = accessTokenValidationParameters ?? throw LogHelper.LogArgumentNullException(nameof(accessTokenValidationParameters));
            SignedHttpRequestValidationParameters = signedHttpRequestValidationParameters ?? throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestValidationParameters));
            CallContext = callContext ?? throw LogHelper.LogArgumentNullException(nameof(callContext));
        }

        /// <summary>
        /// Gets <see cref="TokenValidationParameters"/> required for access token ("at") validation.
        /// </summary>
        public TokenValidationParameters AccessTokenValidationParameters { get; }

        /// <summary>
        /// An opaque context used to store work and logs when working with authentication artifacts. 
        /// </summary>
        public CallContext CallContext { get; }

        /// <summary>
        /// A structure that represents an incoming http request.
        /// </summary>
        public HttpRequestData HttpRequestData { get; }

        /// <summary>
        /// Gets a signed http request that is to be validated as a JWS in Compact Serialization Format.
        /// </summary>
        public string SignedHttpRequest { get; }

        /// <summary>
        /// Gets a set of parameters required for validating a SignedHttpRequest.
        /// </summary>
        public SignedHttpRequestValidationParameters SignedHttpRequestValidationParameters { get; }
    }
}

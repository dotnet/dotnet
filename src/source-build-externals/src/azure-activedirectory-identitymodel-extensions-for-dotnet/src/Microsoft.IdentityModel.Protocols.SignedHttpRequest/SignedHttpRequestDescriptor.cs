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

using System.Collections.Generic;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Structure that wraps parameters needed for SignedHttpRequest creation. 
    /// </summary>
    public class SignedHttpRequestDescriptor
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SignedHttpRequestDescriptor"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="accessToken"/> has to contain the 'cnf' claim so that PoP key can be resolved on the validation side.
        /// https://datatracker.ietf.org/doc/html/rfc7800#section-3.1
        /// Default <see cref="SignedHttpRequestCreationParameters"/> and <see cref="CallContext"/> will be created.
        /// </remarks>
        /// <param name="accessToken">An access token that contains the 'cnf' claim.</param>
        /// <param name="httpRequestData">A structure that represents an outgoing http request.</param>
        /// <param name="signingCredentials">A security key and algorithm that will be used to sign the (Signed)HttpRequest.</param>
        public SignedHttpRequestDescriptor(string accessToken, HttpRequestData httpRequestData, SigningCredentials signingCredentials) 
            : this(accessToken, httpRequestData, signingCredentials, new SignedHttpRequestCreationParameters())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SignedHttpRequestDescriptor"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="accessToken"/> has to contain the 'cnf' claim so that PoP key can be resolved on the validation side.
        /// https://datatracker.ietf.org/doc/html/rfc7800#section-3.1
        /// </remarks> 
        /// <param name="accessToken">An access token that contains the 'cnf' claim.</param>
        /// <param name="httpRequestData">A structure that represents an outgoing http request.</param>
        /// <param name="signingCredentials">A security key and algorithm that will be used to sign the (Signed)HttpRequest.</param>
        /// <param name="signedHttpRequestCreationParameters">A set of parameters required for creating a SignedHttpRequest.</param>
        public SignedHttpRequestDescriptor(string accessToken, HttpRequestData httpRequestData, SigningCredentials signingCredentials, SignedHttpRequestCreationParameters signedHttpRequestCreationParameters) 
        {
            AccessToken = !string.IsNullOrEmpty(accessToken) ? accessToken : throw LogHelper.LogArgumentNullException(nameof(accessToken));
            HttpRequestData = httpRequestData ?? throw LogHelper.LogArgumentNullException(nameof(httpRequestData));
            SigningCredentials = signingCredentials ?? throw LogHelper.LogArgumentNullException(nameof(signingCredentials));
            SignedHttpRequestCreationParameters = signedHttpRequestCreationParameters ?? throw LogHelper.LogArgumentNullException(nameof(signedHttpRequestCreationParameters));
        }

        /// <summary>
        /// Gets or sets the <see cref="Dictionary{TKey, TValue}"/> which contains any custom header claims that need to be added to the SignedHttpRequest token header.
        /// The 'alg', 'kid', and 'x5t' claims are added by default based on the provided <see cref="SigningCredentials"/> and SHOULD NOT be included in this dictionary as this
        /// will result in an exception being thrown.  
        /// </summary>
        public IDictionary<string, object> AdditionalHeaderClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dictionary{TKey, TValue}"/> which contains any custom payload claims that need to be added to the SignedHttpRequest token payload.
        /// Default SignedHttpRequest payload claims (<see cref="SignedHttpRequestClaimTypes"/>) will NOT be overwritten by <see cref="AdditionalPayloadClaims"/>.
        /// </summary>
        public IDictionary<string, object> AdditionalPayloadClaims { get; set; }

        /// <summary>
        /// Gets an access token that contains the 'cnf' claim.
        /// </summary>
        public string AccessToken { get; }

        ///<summary> Gets or sets a "cnf" claim value as a JSON string.</summary>
        /// <remarks>
        /// If <see cref="SignedHttpRequestCreationParameters.CreateCnf"/> flag is set to <c>true</c>, <see cref="CnfClaimValue"/> can be used 
        /// as a "cnf" claim value when creating a SignedHttpRequest payload.
        /// If <see cref="SignedHttpRequestCreationParameters.CreateCnf"/> flag is set to <c>true</c>, and <see cref="CnfClaimValue"/> is null or empty,
        /// a "cnf" claim value will be derived from a <see cref="SigningCredentials"/>.<see cref="SecurityKey"/>.
        /// </remarks>
        public string CnfClaimValue { get; set; }

        /// <summary>
        /// Gets or sets a custom value that will be set when creating a <see cref="SignedHttpRequestClaimTypes.Nonce"/> claim.
        /// </summary>
        /// <remarks>This value will be added to a SignedHttpRequest payload only when <see cref="SignedHttpRequestCreationParameters.CreateNonce"/> is set to <c>true</c>.</remarks>
        public string CustomNonceValue { get; set; }

        /// <summary>
        /// A structure that represents an outgoing http request.
        /// </summary>
        public HttpRequestData HttpRequestData { get; }

        /// <summary>
        /// Gets signing credentials that are used to sign a (Signed)HttpRequest.
        /// </summary>
        public SigningCredentials SigningCredentials { get; }

        /// <summary>
        /// Gets a set of parameters required for creating a SignedHttpRequest.
        /// </summary>
        public SignedHttpRequestCreationParameters SignedHttpRequestCreationParameters { get; }
    }
}

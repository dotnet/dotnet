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
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// A delegate that will be called to retrieve a collection of <see cref="SecurityKey"/>s used for the 'cnf' claim decryption.
    /// </summary>
    /// <param name="jweCnf">A 'cnf' claim represented as a <see cref="SecurityToken"/>.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns>A collection of cnf decryption keys.</returns>
    public delegate Task<IEnumerable<SecurityKey>> CnfDecryptionKeysResolverAsync(SecurityToken jweCnf, CancellationToken cancellationToken);

    /// <summary>
    /// A delegate that returns an HttpClient that will be used to retrieve a JWK Set while resolving a PoP key from a 'jku' claim.
    /// </summary>
    /// <returns>An HttpClient used to retrieve a JWK Set.</returns>
    public delegate HttpClient HttpClientProvider();

    /// <summary>
    /// A delegate that will take control over PoP key resolution, if set.
    /// </summary>
    /// <param name="validatedAccessToken">An access token ("at") that was already validated during the SignedHttpRequest validation process.</param>
    /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
    /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.></param>
    /// <returns>A resolved <see cref="SecurityKey"/>.</returns>
    public delegate Task<SecurityKey> PopKeyResolverAsync(SecurityToken validatedAccessToken, SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken);

    /// <summary>
    /// A delegate that will be called to resolve a <see cref="SecurityKey"/> from a 'cnf' claim that contains only the 'kid' claim.
    /// </summary>
    /// <param name="kid">KeyIdentifier value.</param>
    /// <param name="validatedAccessToken">An access token ("at") that was already validated during the SignedHttpRequest validation process.</param>
    /// <param name="signedHttpRequest">A SignedHttpRequest.</param> 
    /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns>A resolved <see cref="SecurityKey"/>.</returns>
    /// <remarks>https://datatracker.ietf.org/doc/html/rfc7800#section-3.4</remarks>
    public delegate Task<SecurityKey> PopKeyResolverFromKeyIdAsync(string kid, SecurityToken validatedAccessToken, SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken);

    /// <summary>
    /// A delegate that will be called to check if SignedHttpRequest is replayed, if set.
    /// </summary>
    /// <param name="signedHttpRequest">A SignedHttpRequest which contains the 'nonce' claim to validate.</param>
    /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns>Expected to throw an appropriate exception if SignedHttpRequest replay is detected.</returns>
    public delegate Task ReplayValidatorAsync(SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken);

    /// <summary>
    /// A delegate that will take control over SignedHttpRequest nonce validation, if set.
    /// </summary>
    /// <param name="key">the key use to validate server nonce.</param>
    /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
    /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns>Expected to throw an appropriate exception if SignedHttpRequest replay is detected.</returns>
    public delegate bool NonceValidatorAsync(SecurityKey key, SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken);

    /// <summary>
    /// A delegate that will take control over SignedHttpRequest signature validation, if set.
    /// </summary>
    /// <param name="popKey">A resolved PoP key.</param>
    /// <param name="signedHttpRequest">A SignedHttpRequest.</param>
    /// <param name="signedHttpRequestValidationContext">A structure that wraps parameters needed for SignedHttpRequest validation.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns>A <see cref="SecurityKey"/> used to validate a signature of the <paramref name="signedHttpRequest"/>, otherwise expected to throw an appropriate exception.</returns>
    public delegate Task<SecurityKey> SignatureValidatorAsync(SecurityKey popKey, SecurityToken signedHttpRequest, SignedHttpRequestValidationContext signedHttpRequestValidationContext, CancellationToken cancellationToken);

    /// <summary>
    /// Defines a set of parameters that are used by a <see cref="SignedHttpRequestHandler"/> when validating a SignedHttpRequest.
    /// </summary>
    public class SignedHttpRequestValidationParameters
    {
        private TimeSpan _signedHttpRequestLifetime = DefaultSignedHttpRequestLifetime;

        /// <summary>
        /// Gets or sets a value indicating whether the unsigned query parameters are accepted or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-5.1</remarks>
        public bool AcceptUnsignedQueryParameters { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the unsigned headers are accepted or not. 
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-5.1</remarks>
        public bool AcceptUnsignedHeaders { get; set; } = true;

        /// <summary>
        /// Gets or sets the claims to validate if present.
        /// </summary>
        /// <remarks>
        /// Validation will only occur if <see cref="ValidatePresentClaims"/> is set to <c>true</c>.
        /// </remarks>
        public IEnumerable<string> ClaimsToValidateWhenPresent { get; set; } = new List<string>
        {
            SignedHttpRequestClaimTypes.M,
            SignedHttpRequestClaimTypes.P
        };

        /// <summary>
        /// Gets or sets a collection of <see cref="SecurityKey"/> used for the 'cnf' claim decryption.
        /// </summary>
        public IEnumerable<SecurityKey> CnfDecryptionKeys { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="CnfDecryptionKeysResolverAsync"/> delegate.
        /// </summary>
        public CnfDecryptionKeysResolverAsync CnfDecryptionKeysResolverAsync { get; set; }

        /// <summary>
        /// Default value for the <see cref="SignedHttpRequestLifetime"/>.
        /// </summary>
        public static readonly TimeSpan DefaultSignedHttpRequestLifetime = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets or sets the <see cref="HttpClientProvider"/> delegate.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7800#section-3.5</remarks>
        public HttpClientProvider HttpClientProvider { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="NonceValidatorAsync"/> delegate.
        /// </summary>
        public NonceValidatorAsync NonceValidatorAsync { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PopKeyResolverAsync"/> delegate.
        /// </summary>
        public PopKeyResolverAsync PopKeyResolverAsync { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PopKeyResolverFromKeyIdAsync"/> delegate.
        /// </summary>
        public PopKeyResolverFromKeyIdAsync PopKeyResolverFromKeyIdAsync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether TLS is required when obtaining a JWK set using the 'jku' claim.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7800#section-3.5</remarks>
        public bool RequireHttpsForJkuResourceRetrieval { get; set; } = true;

        /// <summary>
        /// Gets or sets the signed http request lifetime.
        /// </summary>
        public TimeSpan SignedHttpRequestLifetime
        {
            get
            {
                return _signedHttpRequestLifetime;
            }

            set
            {
                if (value < TimeSpan.Zero)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value)));

                _signedHttpRequestLifetime = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ReplayValidatorAsync"/> delegate.
        /// </summary>
        public ReplayValidatorAsync ReplayValidatorAsync { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SignatureValidatorAsync"/> delegate.
        /// </summary>
        public SignatureValidatorAsync SignatureValidatorAsync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.Ts"/> claim should be validated or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks>  
        public bool ValidateTs { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.M"/> claim should be validated or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks>  
        public bool ValidateM { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.U"/> claim should be validated or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks>  
        public bool ValidateU { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.P"/> claim should be validated or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks>  
        public bool ValidateP { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.Q"/> claim should be validated or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks>  
        public bool ValidateQ { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.H"/> claim should be validated or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks>  
        public bool ValidateH { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SignedHttpRequestClaimTypes.B"/> claim should be validated or not.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/draft-ietf-oauth-signed-http-request-03#section-3</remarks>  
        public bool ValidateB { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether claims in <see cref="ClaimsToValidateWhenPresent"/> should be validated if present.
        /// </summary>
        /// <remarks>
        /// Allows for validation of a claim if present, even if the validation option for the claim is set to <c>false</c>.
        /// </remarks>
        public bool ValidatePresentClaims { get; set; } = false;
    }
}

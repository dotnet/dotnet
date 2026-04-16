// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
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
        private TokenHandler _tokenHandler = new JsonWebTokenHandler();
        private ICollection<string> _allowedDomainsForJkuRetrieval;

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
        /// Gets or sets a value indicating whether PoP key can be resolved from 'jku' claim.
        /// If you set this property to true, you must set values in <see cref="AllowedDomainsForJkuRetrieval"/>.
        /// </summary>
        /// <remarks>https://datatracker.ietf.org/doc/html/rfc7800#section-3.5</remarks>
        public bool AllowResolvingPopKeyFromJku { get; set; } = false;

        /// <summary>
        /// Gets or sets a list of allowed domains for 'jku' claim retrieval.
        /// The domains are not directly compared with the 'jku' claim. Allowed domain would be
        /// deemed valid if the host specified in the 'jku' claim ends with the domain value.
        /// </summary>
        /// <remarks>
        /// Domains should be provided in the following format:
        /// "contoso.com"
        /// "abc.fabrikam.com"
        /// </remarks>
        public ICollection<string> AllowedDomainsForJkuRetrieval => _allowedDomainsForJkuRetrieval ??
            Interlocked.CompareExchange(ref _allowedDomainsForJkuRetrieval, new List<string>(), null) ?? _allowedDomainsForJkuRetrieval;

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
        /// Gets or sets the <see cref="TokenHandler"/> to AccessToken inside the SignedHttpRequest.
        /// </summary>
        public TokenHandler TokenHandler
        {
            get
            {
                return _tokenHandler;
            }

            set
            {
                _tokenHandler = value ?? throw LogHelper.LogArgumentNullException(nameof(value));
            }
        }

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

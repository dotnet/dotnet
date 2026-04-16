// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains some information which used to create a security token.
    /// </summary>
    public class SecurityTokenDescriptor
    {
        private List<string> _audiences;

        /// <summary>
        /// Gets or sets the value of the {"": audience} claim. Will be combined with <see cref="Audiences"/> and any "Aud" claims in
        /// <see cref="Claims"/> or <see cref="Subject"/> when creating a token.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets the list audiences to include in the token's 'Aud' claim. Will be combined with <see cref="Audiences"/> and any
        /// "Aud" claims in <see cref="Claims"/> or <see cref="Subject"/> when creating a token.
        /// </summary>
        public IList<string> Audiences => _audiences ?? Interlocked.CompareExchange(ref _audiences, [], null) ?? _audiences;

        /// <summary>
        /// Defines the compression algorithm that will be used to compress the JWT token payload.
        /// </summary>
        public string CompressionAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="EncryptingCredentials"/> used to create a encrypted security token.
        /// </summary>
        public EncryptingCredentials EncryptingCredentials { get; set; }

        /// <summary>
        /// Gets or sets the value of the 'expiration' claim. This value should be in UTC.
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Gets or sets the issuer of this <see cref="SecurityTokenDescriptor"/>.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the time the security token was issued. This value should be in UTC.
        /// </summary>
        public DateTime? IssuedAt { get; set; }

        /// <summary>
        /// Gets or sets the notbefore time for the security token. This value should be in UTC.
        /// </summary>
        public DateTime? NotBefore { get; set; }

        /// <summary>
        /// Gets or sets the token type.
        /// <remarks> If provided, this will be added as the value for the 'typ' header parameter. In the case of a JWE, this will be added to both the inner (JWS) and the outer token (JWE) header. By default, the value used is 'JWT'.
        /// If <see cref="AdditionalHeaderClaims"/> also contains 'typ' header claim value, it will override the TokenType provided here.
        /// This value is used only for JWT tokens and not for SAML/SAML2 tokens</remarks>
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dictionary{TKey, TValue}"/> which represents the claims that will be used when creating a security token.
        /// If both <see cref="Claims"/> and <see cref="Subject"/> are set, the claim values in Subject will be combined with the values
        /// in Claims. The values found in Claims take precedence over those found in Subject, so any duplicate
        /// values will be overridden.
        /// </summary>
        public IDictionary<string, object> Claims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dictionary{TKey, TValue}"/> which contains any custom header claims that need to be added to the JWT token header.
        /// The 'alg', 'kid', 'x5t', 'enc', and 'zip' claims are added by default based on the <see cref="SigningCredentials"/>,
        /// <see cref="EncryptingCredentials"/>, and/or <see cref="CompressionAlgorithm"/> provided and SHOULD NOT be included in this dictionary as this
        /// will result in an exception being thrown. 
        /// <remarks> These claims are only added to the outer header (in case of a JWE).</remarks>
        /// </summary>
        public IDictionary<string, object> AdditionalHeaderClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dictionary{TKey, TValue}"/> which contains any custom header claims that need to be added to the inner JWT token header.
        /// The 'alg', 'kid', 'x5t', 'enc', and 'zip' claims are added by default based on the <see cref="SigningCredentials"/>,
        /// <see cref="EncryptingCredentials"/>, and/or <see cref="CompressionAlgorithm"/> provided and SHOULD NOT be included in this dictionary as this
        /// will result in an exception being thrown. 
        /// <remarks>
        /// For JsonWebTokenHandler, these claims are merged with <see cref="AdditionalHeaderClaims"/> while adding to the inner JWT header.
        /// </remarks>
        /// </summary>
        public IDictionary<string, object> AdditionalInnerHeaderClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SigningCredentials"/> used to create a security token.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ClaimsIdentity"/>.
        /// If both <see cref="Claims"/> and <see cref="Subject"/> are set, the claim values in Subject will be combined with the values
        /// in Claims. The values found in Claims take precedence over those found in Subject, so any duplicate
        /// values will be overridden.
        /// </summary>
        public ClaimsIdentity Subject { get; set; }
    }
}

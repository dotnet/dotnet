// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains artifacts obtained when a SecurityToken is validated.
    /// A SecurityTokenHandler returns an instance that captures the results of validating a token.
    /// </summary>
    public class TokenValidationResult
    {
        private Lazy<IDictionary<string, object>> _claims;
        private ClaimsIdentity _claimsIdentity;
        private Exception _exception;
        private bool _hasIsValidOrExceptionBeenRead = false;
        private bool _isValid = false;
        private TokenValidationParameters _validationParameters;
        private TokenHandler _tokenHandler;

        /// <summary>
        /// Creates an instance of <see cref="TokenValidationResult"/>
        /// </summary>
        public TokenValidationResult()
        {
            Initialize();
        }

        /// <summary>
        /// This ctor is used by the JsonWebTokenHandler as part of delaying creation of ClaimsIdentity.
        /// </summary>
        /// <param name="securityToken"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="validationParameters"></param>
        /// <param name="issuer"></param>
        internal TokenValidationResult(SecurityToken securityToken, TokenHandler tokenHandler, TokenValidationParameters validationParameters, string issuer)
        {
            _validationParameters = validationParameters;
            _tokenHandler = tokenHandler;
            Issuer = issuer;
            SecurityToken = securityToken;
            Initialize();
        }

        /// <summary>
        /// The <see cref="Dictionary{String, Object}"/> created from the validated security token.
        /// </summary>
        public IDictionary<string, object> Claims
        {
            get
            {
                if (!_hasIsValidOrExceptionBeenRead)
                    LogHelper.LogWarning(LogMessages.IDX10109);

                return _claims.Value;
            }
        }

        /// <summary>
        /// The <see cref="ClaimsIdentity"/> created from the validated security token.
        /// </summary>
        public ClaimsIdentity ClaimsIdentity
        {
            get
            {
                if (_claimsIdentity == null)
                    _claimsIdentity = CreateClaimsIdentity();

                return _claimsIdentity;
            }
            set
            {
                _claimsIdentity = value ?? throw LogHelper.LogArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// This call is for JWTs, SamlTokenHandler will set the ClaimsPrincipal.
        /// </summary>
        /// <returns></returns>
        private ClaimsIdentity CreateClaimsIdentity()
        {
            if (_validationParameters != null && SecurityToken != null && _tokenHandler != null && Issuer != null)
                return _tokenHandler.CreateClaimsIdentityInternal(SecurityToken, _validationParameters, Issuer);

            return null;
        }

        /// <summary>
        /// Gets or sets the <see cref="Exception"/> that occurred during validation.
        /// </summary>
        public Exception Exception
        {
            get
            {
                _hasIsValidOrExceptionBeenRead = true;
                return _exception;
            }
            set
            {
                _exception = value;
            }
        }

        private void Initialize()
        {
            _claims = new Lazy<IDictionary<string, object>>(() => TokenUtilities.CreateDictionaryFromClaims(ClaimsIdentity?.Claims));
        }

        /// <summary>
        /// Gets or sets the issuer that was found in the token.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// True if the token was successfully validated, false otherwise.
        /// </summary>
        public bool IsValid
        {
            get
            {
                _hasIsValidOrExceptionBeenRead = true;
                return _isValid;
            }
            set
            {
                _isValid = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IDictionary{String, Object}"/> that contains a collection of custom key/value pairs. This allows addition of data that could be used in custom scenarios. This uses <see cref="StringComparer.Ordinal"/> for case-sensitive comparison of keys.
        /// </summary>
        public IDictionary<string, object> PropertyBag { get; } = new Dictionary<string, object>(StringComparer.Ordinal);

        /// <summary>
        /// Gets or sets the <see cref="SecurityToken"/> that was validated.
        /// </summary>
        public SecurityToken SecurityToken { get; set; }

        /// <summary>
        /// The <see cref="SecurityToken"/> to be returned when validation fails.
        /// </summary>
        public SecurityToken TokenOnFailedValidation { get; internal set; }

        /// <summary>
        /// Gets or sets the <see cref="CallContext"/> that contains call information.
        /// </summary>
        public CallContext TokenContext { get; set; }

        /// <summary>
        /// Gets or sets the token type of the <see cref="SecurityToken"/> that was validated.
        /// When a <see cref="TokenValidationParameters.TypeValidator"/> is registered,
        /// the type returned by the delegate is used to populate this property.
        /// Otherwise, the type is resolved from the token itself, if available
        /// (e.g for a JSON Web Token, from the "typ" header). 
        /// </summary>
        public string TokenType { get; set; }
    }
}

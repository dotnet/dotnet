// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Contains artifacts obtained when a SecurityToken is validated.
    /// A SecurityTokenHandler returns an instance that captures the results of validating a token.
    /// </summary>
    public class TokenValidationResult
    {
        private readonly TokenValidationParameters _validationParameters;
        private readonly TokenHandler _tokenHandler;

        private Exception _exception;
        private bool _hasIsValidOrExceptionBeenRead = false;
        private bool _isValid = false;

        // Fields lazily initialized in a thread-safe manner. _claimsIdentity is protected by the _claimsIdentitySyncObj
        // lock, and since null is a valid initialized value, _claimsIdentityInitialized tracks whether or not it's valid.
        // _claims is constructed by reading the data from the ClaimsIdentity and is synchronized using Interlockeds
        // to ensure only one dictionary is published in the face of concurrent access (but if there's a race condition,
        // multiple dictionaries could be constructed, with only one published for all to see). Simiarly, _propertyBag
        // is initalized with Interlocked to ensure only a single instance is published in the face of concurrent use.
        // _claimsIdentityInitialized only ever transitions from false to true, and is volatile to reads/writes are not
        // reordered relative to the other operations. The rest of the objects are not because the .NET memory model
        // guarantees object writes are store releases and that reads won't be introduced.
        private volatile bool _claimsIdentityInitialized;
        private object _claimsIdentitySyncObj;
        private ClaimsIdentity _claimsIdentity;
        private Dictionary<string, object> _claims;
        private Dictionary<string, object> _propertyBag;

        /// <summary>
        /// Creates an instance of <see cref="TokenValidationResult"/>
        /// </summary>
        public TokenValidationResult()
        {
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

                if (_claims is null && ClaimsIdentity is { } ci)
                {
                    Interlocked.CompareExchange(ref _claims, TokenUtilities.CreateDictionaryFromClaims(ci.Claims), null);
                }

                return _claims;
            }
        }

        /// <summary>
        /// The <see cref="ClaimsIdentity"/> created from the validated security token.
        /// </summary>
        public ClaimsIdentity ClaimsIdentity
        {
            get
            {
                if (!_claimsIdentityInitialized)
                {
                    lock (ClaimsIdentitySyncObj)
                    {
                        return ClaimsIdentityNoLocking;
                    }
                }

                return _claimsIdentity;
            }
            set
            {
                if (value is null)
                    throw LogHelper.LogArgumentNullException(nameof(value));

                lock (ClaimsIdentitySyncObj)
                {
                    ClaimsIdentityNoLocking = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="_claimsIdentity"/> without synchronization. All accesses must either
        /// be protected or used when the caller knows access is serialized.
        /// </summary>
        internal ClaimsIdentity ClaimsIdentityNoLocking
        {
            get
            {
                if (!_claimsIdentityInitialized)
                {
                    Debug.Assert(_claimsIdentity is null);

                    if (_validationParameters != null && SecurityToken != null && _tokenHandler != null && Issuer != null)
                    {
                        _claimsIdentity = _tokenHandler.CreateClaimsIdentityInternal(SecurityToken, _validationParameters, Issuer);
                    }

                    _claimsIdentityInitialized = true;
                }

                return _claimsIdentity;
            }
            set
            {
                Debug.Assert(value is not null);
                _claimsIdentity = value;
                _claims = null;
                _claimsIdentityInitialized = true;
            }
        }

        /// <summary>Gets the object to use in <see cref="ClaimsIdentity"/> for double-checked locking.</summary>
        private object ClaimsIdentitySyncObj
        {
            get
            {
                object syncObj = _claimsIdentitySyncObj;
                if (syncObj is null)
                {
                    Interlocked.CompareExchange(ref _claimsIdentitySyncObj, new object(), null);
                    syncObj = _claimsIdentitySyncObj;
                }

                return syncObj;
            }
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
        public IDictionary<string, object> PropertyBag =>
            // Lazily-initialize the property bag in a thread-safe manner. It's ok if a race condition results
            // in multiple dictionaries being created, as long as only one is ever published and all consumers
            // see the same instance. It's a bit strange to make this thread-safe, as the resulting Dictionary
            // itself is not for writes, so multi-threaded consumption in which at least one consumer is mutating
            // the dictionary need to provide their own synchronization.
            _propertyBag ??
            Interlocked.CompareExchange(ref _propertyBag, new Dictionary<string, object>(StringComparer.Ordinal), null) ??
            _propertyBag;

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

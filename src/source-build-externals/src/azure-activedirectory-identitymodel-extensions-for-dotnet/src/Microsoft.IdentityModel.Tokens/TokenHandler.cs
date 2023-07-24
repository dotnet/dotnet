// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Logging;
using System;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Defines properties shared across all security token handlers.
    /// </summary>
    public abstract class TokenHandler
    {
        private int _defaultTokenLifetimeInMinutes = DefaultTokenLifetimeInMinutes;
        private int _maximumTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        /// <summary>
        /// Default lifetime of tokens created. When creating tokens, if 'expires', 'notbefore' or 'issuedat' are null, 
        /// then a default will be set to: issuedat = DateTime.UtcNow, notbefore = DateTime.UtcNow, expires = DateTime.UtcNow + TimeSpan.FromMinutes(TokenLifetimeInMinutes).
        /// </summary>
        /// <remarks>See: <see cref="SetDefaultTimesOnTokenCreation"/> for configuration.</remarks>
        public static readonly int DefaultTokenLifetimeInMinutes = 60;

        /// <summary>
        /// Gets and sets the maximum token size in bytes that will be processed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">'value' less than 1.</exception>
        public virtual int MaximumTokenSizeInBytes
        {
            get => _maximumTokenSizeInBytes; 
            set => _maximumTokenSizeInBytes =  (value < 1) ? throw LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), FormatInvariant(LogMessages.IDX10101, LogHelper.MarkAsNonPII(value)))) : value;
        }

        /// <summary>
        /// Gets or sets a bool that controls if token creation will set default 'exp', 'nbf' and 'iat' if not specified.
        /// </summary>
        /// <remarks>See: <see cref="TokenLifetimeInMinutes"/> for configuration.</remarks>
        [DefaultValue(true)]
        public bool SetDefaultTimesOnTokenCreation { get; set; } = true;

        /// <summary>
        /// Gets or sets the token lifetime in minutes.
        /// </summary>
        /// <remarks>Used during token creation to set the default expiration ('exp'). </remarks>
        /// <exception cref="ArgumentOutOfRangeException">'value' less than 1.</exception>
        public int TokenLifetimeInMinutes
        {
            get => _defaultTokenLifetimeInMinutes;
            set => _defaultTokenLifetimeInMinutes = (value < 1) ? throw LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), FormatInvariant(LogMessages.IDX10104, LogHelper.MarkAsNonPII(value)))) : value;
        }

        #region methods

        /// <summary>
        /// Validates a token.
        /// On a validation failure, no exception will be thrown; instead, the exception will be set in the returned TokenValidationResult.Exception property.
        /// Callers should always check the TokenValidationResult.IsValid property to verify the validity of the result.
        /// </summary>
        /// <param name="token">The token to be validated.</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>A <see cref="TokenValidationResult"/></returns>
        public virtual Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters) => throw new NotImplementedException();

        /// <summary>
        /// Validates a token.
        /// On a validation failure, no exception will be thrown; instead, the exception will be set in the returned TokenValidationResult.Exception property.
        /// Callers should always check the TokenValidationResult.IsValid property to verify the validity of the result.
        /// </summary>
        /// <param name="token">The <see cref="SecurityToken"/> to be validated.</param>
        /// <param name="validationParameters">A <see cref="TokenValidationParameters"/> required for validation.</param>
        /// <returns>A <see cref="TokenValidationResult"/></returns>
        public virtual Task<TokenValidationResult> ValidateTokenAsync(SecurityToken token, TokenValidationParameters validationParameters) => throw new NotImplementedException();

        /// <summary>
        /// Converts a string into an instance of <see cref="SecurityToken"/>.
        /// </summary>
        /// <param name="token">The string to be deserialized.</param>
        /// <exception cref="ArgumentNullException"><paramref name="token"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">'token.Length' is greater than <see cref="TokenHandler.MaximumTokenSizeInBytes"/>.</exception>
        /// <returns>A <see cref="SecurityToken"/>.</returns>
        public virtual SecurityToken ReadToken(string token) => throw new NotImplementedException();

        /// <summary>
        /// Called by base class to create a <see cref="ClaimsIdentity"/>.
        /// Currently only used by the JsonWebTokenHandler to allow for a Lazy creation.
        /// </summary>
        /// <param name="securityToken">the <see cref="SecurityToken"/> that has the Claims.</param>
        /// <param name="tokenValidationParameters">the <see cref="TokenValidationParameters"/> that was used to validate the token.</param>
        /// <param name="issuer">the 'issuer' to use by default when creating a Claim.</param>
        /// <returns>A <see cref="ClaimsIdentity"/>.</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal virtual ClaimsIdentity CreateClaimsIdentityInternal(SecurityToken securityToken, TokenValidationParameters tokenValidationParameters, string issuer) => throw new NotImplementedException();

        #endregion
    }
}

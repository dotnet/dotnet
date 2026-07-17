// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;

#nullable enable
namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <remarks>This partial class contains methods and logic related to the validation of tokens.</remarks>
    public partial class JsonWebTokenHandler : TokenHandler
    {
        /// <summary>
        /// Converts a string into an instance of <see cref="JsonWebToken"/>, returned inside of a <see cref="ValidationResult{SecurityToken, ValidationError}"/>.
        /// </summary>
        /// <param name="token">A JSON Web Token (JWT) in JWS or JWE Compact Serialization format.</param>
        /// <param name="callContext"></param>
        /// <returns>A <see cref="ValidationResult{SecurityToken, ValidationError}"/> with the <see cref="JsonWebToken"/> or a <see cref="ValidationError"/>.</returns>
        internal static ValidationResult<SecurityToken, ValidationError> ReadToken(
            string token,
#pragma warning disable CA1801 // TODO: remove pragma disable once callContext is used for logging
            CallContext? callContext)
#pragma warning restore CA1801 // TODO: remove pragma disable once callContext is used for logging
        {
            if (string.IsNullOrEmpty(token))
            {
                return ValidationError.NullParameter(
                    nameof(token),
                    ValidationError.GetCurrentStackFrame());
            }

            try
            {
                return new JsonWebToken(token);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return new ValidationError(
                    new MessageDetail(LogMessages.IDX14107),
                    ValidationFailureType.TokenReadingFailed,
                    ValidationError.GetCurrentStackFrame(),
                    ex);
            }
        }
    }
}
#nullable restore

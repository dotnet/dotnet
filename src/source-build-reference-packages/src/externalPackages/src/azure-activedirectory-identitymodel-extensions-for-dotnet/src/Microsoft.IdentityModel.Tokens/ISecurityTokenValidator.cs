// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// ISecurityTokenValidator
    /// </summary>
    public interface ISecurityTokenValidator
    {
        /// <summary>
        /// Returns true if the token can be read, false otherwise.
        /// </summary>
        bool CanReadToken(string securityToken);

        /// <summary>
        /// Returns true if a token can be validated.
        /// </summary>
        bool CanValidateToken { get; }

        /// <summary>
        /// Gets and sets the maximum size in bytes, that a will be processed.
        /// </summary>
        Int32 MaximumTokenSizeInBytes { get; set; }

        /// <summary>
        /// Validates a token passed as a string using <see cref="TokenValidationParameters"/>
        /// </summary>
        ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken);
    }
}

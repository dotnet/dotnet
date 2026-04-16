// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Validators meant to be kept internal
    /// </summary>
    internal static class InternalValidators
    {
        /// <summary>
        /// Called after signature validation has failed to avoid a metadata refresh
        /// </summary>
        internal static void ValidateAfterSignatureFailed(
            SecurityToken securityToken,
            DateTime? notBefore,
            DateTime? expires,
            IEnumerable<string> audiences,
            TokenValidationParameters validationParameters,
            BaseConfiguration configuration)
        {
            Validators.ValidateLifetime(notBefore, expires, securityToken, validationParameters);
            Validators.ValidateIssuer(securityToken.Issuer, securityToken, validationParameters, configuration);
            Validators.ValidateAudience(audiences, securityToken, validationParameters);
        }
    }
}

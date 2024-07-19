// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Security.Claims;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// AppContext switches for Microsoft.IdentityModel.Tokens and referencing packages.
    /// </summary>
    internal static class AppContextSwitches
    {
        /// <summary>
        /// Enables a fallback to the previous behavior of using <see cref="ClaimsIdentity"/> instead of <see cref="CaseSensitiveClaimsIdentity"/> globally.
        /// </summary>
        internal const string UseClaimsIdentityTypeSwitch = "Microsoft.IdentityModel.Tokens.UseClaimsIdentityType";

        internal static bool UseClaimsIdentityType() => (AppContext.TryGetSwitch(UseClaimsIdentityTypeSwitch, out bool useClaimsIdentityType) && useClaimsIdentityType);
    }
}

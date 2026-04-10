// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Facilitates the creation of <see cref="ClaimsIdentity"/> and <see cref="CaseSensitiveClaimsIdentity"/> instances based on the <see cref="AppContextSwitches.UseClaimsIdentityTypeSwitch"/>.
    /// </summary>
    internal static class ClaimsIdentityFactory
    {
        internal static ClaimsIdentity Create(IEnumerable<Claim> claims)
        {
            if (AppContextSwitches.UseClaimsIdentityType)
                return new ClaimsIdentity(claims);

            return new CaseSensitiveClaimsIdentity(claims);
        }

        internal static ClaimsIdentity Create(IEnumerable<Claim> claims, string authenticationType)
        {
            if (AppContextSwitches.UseClaimsIdentityType)
                return new ClaimsIdentity(claims, authenticationType);

            return new CaseSensitiveClaimsIdentity(claims, authenticationType);
        }

        internal static ClaimsIdentity Create(string authenticationType, string nameType, string roleType, SecurityToken securityToken)
        {
            if (AppContextSwitches.UseClaimsIdentityType)
                return new ClaimsIdentity(authenticationType: authenticationType, nameType: nameType, roleType: roleType);

            return new CaseSensitiveClaimsIdentity(authenticationType: authenticationType, nameType: nameType, roleType: roleType)
            {
                SecurityToken = securityToken,
            };
        }
    }
}

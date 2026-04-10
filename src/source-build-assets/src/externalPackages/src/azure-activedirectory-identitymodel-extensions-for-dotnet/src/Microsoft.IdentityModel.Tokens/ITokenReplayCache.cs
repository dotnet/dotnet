// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Interface that defines a simple cache for tacking replaying of security tokens.
    /// </summary>
    public interface ITokenReplayCache
    {
        /// <summary>
        /// Try to add a securityToken.
        /// </summary>
        /// <param name="securityToken">the security token to add.</param>
        /// <param name="expiresOn">the time when security token expires.</param>
        /// <returns>true if the security token was successfully added.</returns>
        bool TryAdd(string securityToken, DateTime expiresOn);

        /// <summary>
        /// Try to find securityToken
        /// </summary>
        /// <param name="securityToken">the security token to find.</param>
        /// <returns>true if the security token is found.</returns>
        bool TryFind(string securityToken);
    }
}

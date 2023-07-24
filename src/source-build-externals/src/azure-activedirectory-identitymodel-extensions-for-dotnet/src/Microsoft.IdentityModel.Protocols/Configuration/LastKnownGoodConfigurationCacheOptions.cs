// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Configuration;

namespace Microsoft.IdentityModel.Protocols.Configuration
{
    /// <summary>
    /// Specifies the LastKnownGoodConfigurationCacheOptions which can be used to configure the internal LKG configuration cache.
    /// See <see cref="EventBasedLRUCache{TKey, TValue}"/> for more details.
    /// 
    /// All fields/properties are now defined in the Microsoft.IdentityModel.Tokens.Configuration.LKGConfigurationCacheOptions class so they are more accessible from other assemblies/classes.
    /// </summary>
    public class LastKnownGoodConfigurationCacheOptions : LKGConfigurationCacheOptions
    {
        /// <inheritdoc/>
        public static readonly int DefaultLastKnownGoodConfigurationSizeLimit = LKGConfigurationCacheOptions.DefaultLKGConfigurationSizeLimit;
    }
}

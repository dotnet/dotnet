// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Comparison class for a <see cref="BaseConfiguration"/>.
    /// </summary>
    internal class BaseConfigurationComparer : IEqualityComparer<BaseConfiguration>
    {
        public bool Equals(BaseConfiguration config1, BaseConfiguration config2)
        {
            if (config1 == null && config2 == null)
                return true;
            else if (config1 == null || config2 == null)
                return false;
            else
            {
                if (config1.Issuer != config2.Issuer)
                    return false;

                if (config1.SigningKeys.Count != config2.SigningKeys.Count)
                    return false;

                foreach (var key in config1.SigningKeys)
                {
                    if (!ContainsKeyWithInternalId(config2, key.InternalId))
                        return false;
                }
            }

            return true;
        }

        private static bool ContainsKeyWithInternalId(BaseConfiguration config, string internalId)
        {
            foreach(var key in config.SigningKeys)
                if (key.InternalId == internalId)
                    return true;

            return false;
        }

        public int GetHashCode(BaseConfiguration config)
        {
            int defaultHash = string.Empty.GetHashCode();
            int hashCode = defaultHash;
            hashCode ^= string.IsNullOrEmpty(config.Issuer) ? defaultHash : config.Issuer.GetHashCode();

            foreach (var key in config.SigningKeys)
                hashCode ^= key.InternalId.GetHashCode();

            return hashCode;
        }
    }
}

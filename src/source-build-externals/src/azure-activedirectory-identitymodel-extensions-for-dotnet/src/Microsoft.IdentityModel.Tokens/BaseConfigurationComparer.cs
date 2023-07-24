// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

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
            else if (config1.Issuer == config2.Issuer && config1.SigningKeys.Count == config2.SigningKeys.Count
                     && !config1.SigningKeys.Select(x => x.InternalId).Except(config2.SigningKeys.Select(x => x.InternalId)).Any())
                return true;
            else
                return false;
        }

        public int GetHashCode(BaseConfiguration config)
        {
            int defaultHash = string.Empty.GetHashCode();
            int hashCode = defaultHash;
            hashCode ^= string.IsNullOrEmpty(config.Issuer) ? defaultHash : config.Issuer.GetHashCode();
            foreach(string internalId in config.SigningKeys.Select(x => x.InternalId))
            {
                hashCode ^= internalId.GetHashCode();
            }

            return hashCode;
        }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Specifies the CryptoProviderCacheOptions which can be used to configure the internal cryptoprovider cache.
    /// We are using our own simple LRU caching implementation across all targets. 
    /// See <see cref="EventBasedLRUCache{TKey, TValue}"/> for more details.
    /// </summary>
    public class CryptoProviderCacheOptions
    {
        private int _sizeLimit = DefaultSizeLimit;

        /// <summary>
        /// Default value for the cache size limit (<see cref="SizeLimit"/>). Default is 1000.
        /// </summary>
        public static readonly int DefaultSizeLimit = 1000;

        /// <summary>
        /// Gets or sets the size limit of the cache (in number of items). 
        /// When the cache reaches 95% of this size, 20% of the least recently used items will be evicted.
        /// </summary>
        public int SizeLimit
        {
            get
            {
                return _sizeLimit;
            }
            set
            {
                _sizeLimit = (value > 10) ? value : throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(SizeLimit), LogHelper.FormatInvariant(LogMessages.IDX10901, LogHelper.MarkAsNonPII(value))));
            }
        }
    }
}

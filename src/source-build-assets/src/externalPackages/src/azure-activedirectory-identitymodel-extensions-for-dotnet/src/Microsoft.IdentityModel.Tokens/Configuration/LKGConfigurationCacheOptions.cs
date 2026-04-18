// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Tokens.Configuration
{
    /// <summary>
    /// Defines the options which can be used to configure the internal LKG configuration cache.
    /// See <see cref="EventBasedLRUCache{TKey, TValue}"/> for more details.
    /// </summary>
    public class LKGConfigurationCacheOptions
    {
        private IEqualityComparer<BaseConfiguration> _baseConfigurationComparer = new BaseConfigurationComparer();
        private int _lastKnownGoodConfigurationSizeLimit = DefaultLKGConfigurationSizeLimit;

        /// <summary>
        /// 10 is the default size limit of the cache (in number of items) for last known good configuration.
        /// </summary>
        public static readonly int DefaultLKGConfigurationSizeLimit = 10;

        /// <summary>
        /// Gets or sets the <see cref="IEqualityComparer{T}"/> that is used to determine equality of values for <see cref="BaseConfiguration"/>.
        /// </summary>
        public IEqualityComparer<BaseConfiguration> BaseConfigurationComparer
        {
            get { return _baseConfigurationComparer; }
            set { _baseConfigurationComparer = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        /// <summary>
        /// Gets or sets the size limit of the cache (in number of items) for last known good configuration.
        /// </summary>
        public int LastKnownGoodConfigurationSizeLimit
        {
            get { return _lastKnownGoodConfigurationSizeLimit; }
            set { _lastKnownGoodConfigurationSizeLimit = (value > 0) ? value : throw new ArgumentOutOfRangeException(nameof(value)); }
        }

        /// <summary>
        /// The event queue task creation option, default to None instead of LongRunning as LongRunning will always start a task on a new thread instead of a thread from ThreadPool.
        /// </summary>
        public TaskCreationOptions TaskCreationOptions { get; set; } = TaskCreationOptions.None;

        /// <summary>
        /// Whether or not to remove expired items.
        /// </summary>
        public bool RemoveExpiredValues { get; set; } = true;
    }
}

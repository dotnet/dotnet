//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------


using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Represents a generic configuration manager.
    /// </summary>
    public abstract class BaseConfigurationManager
    {
        private TimeSpan _automaticRefreshInterval = DefaultAutomaticRefreshInterval;
        private TimeSpan _refreshInterval = DefaultRefreshInterval;
        private TimeSpan _lastKnownGoodLifetime = DefaultLastKnownGoodConfigurationLifetime;
        private BaseConfiguration _lastKnownGoodConfiguration;
        private DateTime? _lastKnownGoodConfigFirstUse = null;

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> that controls how often an automatic metadata refresh should occur.
        /// </summary>
        public TimeSpan AutomaticRefreshInterval
        {
            get { return _automaticRefreshInterval; }
            set
            {
                if (value < MinimumAutomaticRefreshInterval)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX10108, LogHelper.MarkAsNonPII(MinimumAutomaticRefreshInterval), LogHelper.MarkAsNonPII(value))));

                _automaticRefreshInterval = value;
            }
        }

        /// <summary>
        /// 12 hours is the default time interval that afterwards will obtain new configuration.
        /// </summary>
        public static readonly TimeSpan DefaultAutomaticRefreshInterval = new TimeSpan(0, 12, 0, 0);

        /// <summary>
        /// 1 hour is the default time interval that a last known good configuration will last for.
        /// </summary>
        public static readonly TimeSpan DefaultLastKnownGoodConfigurationLifetime = new TimeSpan(0, 1, 0, 0);

        /// <summary>
        /// 5 minutes is the default time interval that must pass for <see cref="RequestRefresh"/> to obtain a new configuration.
        /// </summary>
        public static readonly TimeSpan DefaultRefreshInterval = new TimeSpan(0, 0, 5, 0);

        /// <summary>
        /// Obtains an updated version of <see cref="BaseConfiguration"/> if the appropriate refresh interval has passed.
        /// This method may return a cached version of the configuration.
        /// </summary>
        /// <param name="cancel">CancellationToken</param>
        /// <returns>Configuration of type Configuration.</returns>
        /// <remarks>This method on the base class throws a <see cref="NotImplementedException"/> as it is meant to be
        /// overridden by the class that extends it.</remarks>
        public virtual Task<BaseConfiguration> GetBaseConfigurationAsync(CancellationToken cancel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The last known good configuration or LKG (a configuration retrieved in the past that we were able to successfully validate a token against).
        /// </summary>
        public BaseConfiguration LastKnownGoodConfiguration
        {
            get
            {
                // only set this value the first time the last known good configuration is used for validation
                // AND if there is actually a LKG set
                if (_lastKnownGoodConfigFirstUse == null && _lastKnownGoodConfiguration != null)
                    _lastKnownGoodConfigFirstUse = DateTime.UtcNow;

                return _lastKnownGoodConfiguration;
            }
            set
            {
                _lastKnownGoodConfiguration = value ?? throw LogHelper.LogArgumentNullException(nameof(value));
                _lastKnownGoodConfigFirstUse = null; // reset this value as a new last known good configuration was set (and has not been used yet)
            }
        }

        /// <summary>
        /// The length of time that a last known good configuration is valid for.
        /// </summary>
        public TimeSpan LastKnownGoodLifetime
        {
            get { return _lastKnownGoodLifetime; }
            set
            {
                if (value < TimeSpan.Zero)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX10110, value)));

                _lastKnownGoodLifetime = value;
            }
        }

        /// <summary>
        /// The metadata address to retrieve the configuration from.
        /// </summary>
        public string MetadataAddress { get; set; }

        /// <summary>
        /// 5 minutes is the minimum value for automatic refresh. <see cref="AutomaticRefreshInterval"/> can not be set less than this value.
        /// </summary>
        public static readonly TimeSpan MinimumAutomaticRefreshInterval = new TimeSpan(0, 0, 5, 0);

        /// <summary>
        /// 1 second is the minimum time interval that must pass for <see cref="RequestRefresh"/> to  obtain new configuration.
        /// </summary>
        public static readonly TimeSpan MinimumRefreshInterval = new TimeSpan(0, 0, 0, 1);

        /// <summary>
        /// The minimum time between retrievals, in the event that a retrieval failed, or that a refresh was explicitly requested.
        /// </summary>
        public TimeSpan RefreshInterval
        {
            get { return _refreshInterval; }
            set
            {
                if (value < MinimumRefreshInterval)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX10107, LogHelper.MarkAsNonPII(MinimumRefreshInterval), LogHelper.MarkAsNonPII(value))));

                _refreshInterval = value;
            }
        }

        /// <summary>
        /// Indicates whether the last known good feature should be used, true by default.
        /// </summary>
        public bool UseLastKnownGoodConfiguration { get; set; } = true;

        /// <summary>
        /// Indicates whether the last known good configuration is still fresh, depends on when the LKG was first used and it's lifetime.
        /// </summary>
        // The _lastKnownGoodConfiguration private variable is accessed rather than the property (LastKnownGoodConfiguration) as we do not want this access
        // to trigger a change in _lastKnownGoodConfigFirstUse.
        public bool IsLastKnownGoodValid => _lastKnownGoodConfiguration != null && (_lastKnownGoodConfigFirstUse == null || DateTime.UtcNow < _lastKnownGoodConfigFirstUse + LastKnownGoodLifetime);

        /// <summary>
        /// Indicate that the configuration may be stale (as indicated by failing to process incoming tokens).
        /// </summary>
        public abstract void RequestRefresh();
    }
}

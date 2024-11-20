// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using NuGet.Common;

namespace NuGet.Protocol
{
    /// <summary>
    /// Represents a helper class for determining if enhanced retry is enabled and what settings to use.
    /// </summary>
    internal class EnhancedHttpRetryHelper
    {
        /// <summary>
        /// The default delay in milliseconds between retries.
        /// </summary>
        public const int DefaultDelayMilliseconds = 1000;

        /// <summary>
        /// The default value indicating whether or not enhanced HTTP retry is enabled.
        /// </summary>
        public const bool DefaultEnabled = true;

        /// <summary>
        /// The default number of times to retry.
        /// </summary>
        public const int DefaultRetryCount = 6;

        /// <summary>
        /// The default value indicating whether or not to retry HTTP 429 responses.
        /// </summary>
        public const bool DefaultRetry429 = true;

        /// <summary>
        /// The default value indicating whether or not to observe Retry-After headers on responses.
        /// </summary>
        public const bool DefaultObserveRetryAfter = true;

        /// <summary>
        /// The environment variable used to change the delay value.
        /// </summary>
        public const string DelayInMillisecondsEnvironmentVariableName = "NUGET_ENHANCED_NETWORK_RETRY_DELAY_MILLISECONDS";

        /// <summary>
        /// The environment variable used to enable or disable the enhanced HTTP retry.
        /// </summary>
        public const string IsEnabledEnvironmentVariableName = "NUGET_ENABLE_ENHANCED_HTTP_RETRY";

        /// <summary>
        /// The environment variable used to change the retry value.
        /// </summary>
        public const string RetryCountEnvironmentVariableName = "NUGET_ENHANCED_MAX_NETWORK_TRY_COUNT";

        /// <summary>
        /// The environment variabled to to disable retrying HTTP 429 responses.
        /// </summary>
        public const string Retry429EnvironmentVariableName = "NUGET_RETRY_HTTP_429";

        /// <summary>
        /// The envionment variable to disable observing Retry-After responses.
        /// </summary>
        public const string ObserveRetryAfterEnvironmentVariableName = "NUGET_OBSERVE_RETRY_AFTER";

        /// <summary>
        /// The environment variable used to set maximum Retry-After delay period
        /// </summary>
        public const string MaximumRetryAfterDurationEnvironmentVariableName = "NUGET_MAX_RETRY_AFTER_DELAY_SECONDS";

        private readonly IEnvironmentVariableReader _environmentVariableReader;

        private Lazy<bool> _isEnabled;

        private Lazy<int> _retryCount;

        private Lazy<int> _delayInMilliseconds;

        private Lazy<bool> _retry429;

        private Lazy<bool> _observeRetryAfter;

        private Lazy<TimeSpan> _maxRetryAfterDelay;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedHttpRetryHelper" /> class.
        /// </summary>
        /// <param name="environmentVariableReader">A <see cref="IEnvironmentVariableReader" /> to use when reading environment variables.</param>
        /// <exception cref="ArgumentNullException"><paramref name="environmentVariableReader" /> is <see langword="null" />.</exception>
        public EnhancedHttpRetryHelper(IEnvironmentVariableReader environmentVariableReader)
        {
            _environmentVariableReader = environmentVariableReader ?? throw new ArgumentNullException(nameof(environmentVariableReader));
            _isEnabled = new Lazy<bool>(() => GetBoolFromEnvironmentVariable(IsEnabledEnvironmentVariableName, defaultValue: DefaultEnabled, _environmentVariableReader));
            _retryCount = new Lazy<int>(() => GetIntFromEnvironmentVariable(RetryCountEnvironmentVariableName, defaultValue: DefaultRetryCount, _environmentVariableReader));
            _delayInMilliseconds = new Lazy<int>(() => GetIntFromEnvironmentVariable(DelayInMillisecondsEnvironmentVariableName, defaultValue: DefaultDelayMilliseconds, _environmentVariableReader));
            _retry429 = new Lazy<bool>(() => GetBoolFromEnvironmentVariable(Retry429EnvironmentVariableName, defaultValue: true, _environmentVariableReader));
            _observeRetryAfter = new Lazy<bool>(() => GetBoolFromEnvironmentVariable(ObserveRetryAfterEnvironmentVariableName, defaultValue: DefaultObserveRetryAfter, _environmentVariableReader));
            _maxRetryAfterDelay = new Lazy<TimeSpan>(() =>
            {
                int maxRetryAfterDelay = GetIntFromEnvironmentVariable(MaximumRetryAfterDurationEnvironmentVariableName, defaultValue: (int)TimeSpan.FromHours(1).TotalSeconds, _environmentVariableReader);
                return TimeSpan.FromSeconds(maxRetryAfterDelay);
            });
        }

        /// <summary>
        /// Gets a value indicating whether or not enhanced HTTP retry should be enabled.  The default value is <see langword="true" />.
        /// </summary>
        internal bool IsEnabled => _isEnabled.Value;

        /// <summary>
        /// Gets a value indicating the maximum number of times to retry.  The default value is 6.
        /// </summary>
        internal int RetryCount => _retryCount.Value;

        /// <summary>
        /// Gets a value indicating the delay in milliseconds to wait before retrying a connection.  The default value is 1000.
        /// </summary>
        internal int DelayInMilliseconds => _delayInMilliseconds.Value;

        /// <summary>
        /// Gets a value indicating whether or not retryable HTTP 4xx responses should be retried.
        /// </summary>
        internal bool Retry429 => _retry429.Value;

        /// <summary>
        /// Gets a value indicating whether or not to observe the Retry-After header on HTTP responses.
        /// </summary>
        internal bool ObserveRetryAfter => _observeRetryAfter.Value;

        internal TimeSpan MaxRetryAfterDelay => _maxRetryAfterDelay.Value;

        /// <summary>
        /// Gets a <see cref="bool" /> value from the specified environment variable.
        /// </summary>
        /// <param name="variableName">The name of the environment variable to get the value.</param>
        /// <param name="defaultValue">The default value to return if the environment variable is not defined or is not a valid <see cref="bool" />.</param>
        /// <param name="environmentVariableReader">An <see cref="IEnvironmentVariableReader" /> to use when reading the environment variable.</param>
        /// <returns>The value of the specified as a <see cref="bool" /> if the specified environment variable is defined and is a valid value for <see cref="bool" />.</returns>
        private static bool GetBoolFromEnvironmentVariable(string variableName, bool defaultValue, IEnvironmentVariableReader environmentVariableReader)
        {
            try
            {
                if (bool.TryParse(environmentVariableReader.GetEnvironmentVariable(variableName), out bool parsedValue))
                {
                    return parsedValue;
                }
            }
            catch (Exception) { }

            return defaultValue;
        }

        /// <summary>
        /// Gets an <see cref="int" /> value from the specified environment variable.
        /// </summary>
        /// <param name="variableName">The name of the environment variable to get the value.</param>
        /// <param name="defaultValue">The default value to return if the environment variable is not defined or is not a valid <see cref="int" />.</param>
        /// <param name="environmentVariableReader">An <see cref="IEnvironmentVariableReader" /> to use when reading the environment variable.</param>
        /// <returns>The value of the specified as a <see cref="int" /> if the specified environment variable is defined and is a valid value for <see cref="int" />.</returns>
        private static int GetIntFromEnvironmentVariable(string variableName, int defaultValue, IEnvironmentVariableReader environmentVariableReader)
        {
            try
            {
                if (int.TryParse(environmentVariableReader.GetEnvironmentVariable(variableName), out int parsedValue) && parsedValue >= 0)
                {
                    return parsedValue;
                }
            }
            catch (Exception) { }

            return defaultValue;
        }
    }
}

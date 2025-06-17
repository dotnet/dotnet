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
        /// The default maximum delay in milliseconds to observe for a Retry-After header.
        /// </summary>
        public const int DefaultMaximumRetryAfterDelayInSeconds = 3600;

        /// <summary>
        /// The environment variable used to change the delay value.
        /// </summary>
        public const string DelayInMillisecondsEnvironmentVariableName = "NUGET_ENHANCED_NETWORK_RETRY_DELAY_MILLISECONDS";

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

        private Lazy<int> _retryCount;

        private Lazy<(bool, int)> _delayInMilliseconds;

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
            _retryCount = new Lazy<int>(() => GetIntFromEnvironmentVariableOrDefault(RetryCountEnvironmentVariableName, defaultValue: DefaultRetryCount, _environmentVariableReader).Item2);
            _delayInMilliseconds = new Lazy<(bool, int)>(() => GetIntFromEnvironmentVariableOrDefault(DelayInMillisecondsEnvironmentVariableName, defaultValue: DefaultDelayMilliseconds, _environmentVariableReader));
            _retry429 = new Lazy<bool>(() => GetBoolFromEnvironmentVariable(Retry429EnvironmentVariableName, defaultValue: DefaultRetry429, _environmentVariableReader));
            _observeRetryAfter = new Lazy<bool>(() => GetBoolFromEnvironmentVariable(ObserveRetryAfterEnvironmentVariableName, defaultValue: DefaultObserveRetryAfter, _environmentVariableReader));
            _maxRetryAfterDelay = new Lazy<TimeSpan>(() =>
            {
                int maxRetryAfterDelay = GetIntFromEnvironmentVariableOrDefault(MaximumRetryAfterDurationEnvironmentVariableName, defaultValue: DefaultMaximumRetryAfterDelayInSeconds, _environmentVariableReader).Item2;
                return TimeSpan.FromSeconds(maxRetryAfterDelay);
            });
        }

        /// <summary>
        /// Gets a value indicating the maximum number of times to retry.
        /// The default value is 6, see <see cref="DefaultRetryCount" />.
        /// </summary>
        internal int RetryCountOrDefault => _retryCount.Value;

        /// <summary>
        /// Gets a value indicating the delay in milliseconds to wait before retrying a connection.
        /// The default value is 1000, <see cref="DefaultDelayMilliseconds" />.
        /// </summary>
        internal int DelayInMillisecondsOrDefault => _delayInMilliseconds.Value.Item2;

        /// <summary>
        /// Gets a value indicating the delay in milliseconds to wait before retrying a connection.|
        /// Will only have a value if the environment variable is set, otherwise it will be <see langword="null" />.
        /// </summary>
        internal int? DelayInMilliseconds => _delayInMilliseconds.Value.Item1 ? _delayInMilliseconds.Value.Item2 : null;

        /// <summary>
        /// Gets a value indicating whether or not retryable HTTP 4xx responses should be retried.
        /// Default is <see langword="true" />, <see cref="DefaultObserveRetryAfter"/>.
        /// </summary>
        internal bool Retry429OrDefault => _retry429.Value;

        /// <summary>
        /// Gets a value indicating whether or not to observe the Retry-After header on HTTP responses.
        /// Default is <see langword="true" />, <see cref="DefaultObserveRetryAfter"/>.
        /// </summary>
        internal bool ObserveRetryAfterOrDefault => _observeRetryAfter.Value;

        /// <summary>
        /// Gets a value indicating the maximum delay to observe for a Retry-After header.
        /// Default is 1 hour, <see cref="DefaultMaximumRetryAfterDelayInSeconds" />.
        /// </summary>
        internal TimeSpan MaxRetryAfterDelayOrDefault => _maxRetryAfterDelay.Value;

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
        /// <returns> A tuple containing a <see cref="bool" /> indicating if the value was provided through an environment variable parsed and a valid int.
        /// the value of the specified as a <see cref="int" /> if the specified environment variable
        /// is defined and is a valid value for <see cref="int" />.</returns>
        private static (bool, int) GetIntFromEnvironmentVariableOrDefault(string variableName, int defaultValue, IEnvironmentVariableReader environmentVariableReader)
        {
            try
            {
                if (int.TryParse(environmentVariableReader.GetEnvironmentVariable(variableName), out int parsedValue) && parsedValue >= 0)
                {
                    return (true, parsedValue);
                }
            }
            catch (Exception) { }

            return (false, defaultValue);
        }
    }
}

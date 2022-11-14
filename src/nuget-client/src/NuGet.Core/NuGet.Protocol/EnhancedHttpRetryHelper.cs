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

        private readonly IEnvironmentVariableReader _environmentVariableReader;

        private bool? _isEnabled = null;

        private int? _retryCount = null;

        private int? _delayInMilliseconds = null;

        private bool? _retry429 = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedHttpRetryHelper" /> class.
        /// </summary>
        /// <param name="environmentVariableReader">A <see cref="IEnvironmentVariableReader" /> to use when reading environment variables.</param>
        /// <exception cref="ArgumentNullException"><paramref name="environmentVariableReader" /> is <c>null</c>.</exception>
        public EnhancedHttpRetryHelper(IEnvironmentVariableReader environmentVariableReader)
        {
            _environmentVariableReader = environmentVariableReader ?? throw new ArgumentNullException(nameof(environmentVariableReader));
        }

        /// <summary>
        /// Gets a value indicating whether or not enhanced HTTP retry should be enabled.  The default value is <c>true</c>.
        /// </summary>
        internal bool IsEnabled => _isEnabled ??= GetBoolFromEnvironmentVariable(IsEnabledEnvironmentVariableName, defaultValue: DefaultEnabled, _environmentVariableReader);

        /// <summary>
        /// Gets a value indicating the maximum number of times to retry.  The default value is 6.
        /// </summary>
        internal int RetryCount => _retryCount ??= GetIntFromEnvironmentVariable(RetryCountEnvironmentVariableName, defaultValue: DefaultRetryCount, _environmentVariableReader);

        /// <summary>
        /// Gets a value indicating the delay in milliseconds to wait before retrying a connection.  The default value is 1000.
        /// </summary>
        internal int DelayInMilliseconds => _delayInMilliseconds ??= GetIntFromEnvironmentVariable(DelayInMillisecondsEnvironmentVariableName, defaultValue: DefaultDelayMilliseconds, _environmentVariableReader);

        /// <summary>
        /// Gets a value indicating whether or not retryable HTTP 4xx responses should be retried.
        /// </summary>
        internal bool Retry429 => _retry429 ??= GetBoolFromEnvironmentVariable(Retry429EnvironmentVariableName, defaultValue: true, _environmentVariableReader);

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

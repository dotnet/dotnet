// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Timeout utilities.
    /// </summary>
    public static class TimeoutUtilities
    {
        /// <summary>
        /// Attempts to parse a legal timeout and returns a default timeout as a fallback.
        /// </summary>
        /// <param name="timeoutInSeconds">The requested timeout in seconds.</param>
        /// <param name="fallbackTimeout">A fallback timeout.</param>
        /// <returns>A <see cref="TimeSpan" /> object that represents a timeout interval.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="fallbackTimeout" /> is an invalid
        /// timeout.</exception>
        public static TimeSpan GetTimeout(string timeoutInSeconds, TimeSpan fallbackTimeout)
        {
            if (!IsValid(fallbackTimeout))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(fallbackTimeout),
                    fallbackTimeout,
                    Strings.Plugin_TimeoutOutOfRange);
            }

            int seconds;
            if (int.TryParse(timeoutInSeconds, out seconds))
            {
                try
                {
                    var timeout = TimeSpan.FromSeconds(seconds);

                    if (IsValid(timeout))
                    {
                        return timeout;
                    }
                }
                catch (Exception)
                {
                }
            }

            return fallbackTimeout;
        }

        /// <summary>
        /// Determines if a timeout is valid.
        /// </summary>
        /// <param name="timeout">A timeout.</param>
        /// <returns><see langword="true" /> if the timeout is valid; otherwise, <see langword="false" />.</returns>
        public static bool IsValid(TimeSpan timeout)
        {
            if (ProtocolConstants.MinTimeout <= timeout && timeout <= ProtocolConstants.MaxTimeout)
            {
                return true;
            }

            return false;
        }
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Returns the absolute DateTime or the Seconds since Unix Epoch, where Epoch is UTC 1970-01-01T0:0:0Z.
    /// </summary>
    public static class EpochTime
    {
        /// <summary>
        /// DateTime as UTV for UnixEpoch
        /// </summary>
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Gets the number of seconds from 1970-01-01T0:0:0Z as measured in UTC until the desired date/time.
        /// </summary>
        /// <param name="datetime">The DateTime to convert to seconds.</param>
        /// <remarks>If <paramref name="datetime"/> is less than Unix Epoch, returns 0.</remarks>
        /// <returns>The number of seconds since Unix Epoch.</returns>
        public static long GetIntDate(DateTime datetime)
        {
            DateTime dateTimeUtc = datetime;
            if (datetime.Kind != DateTimeKind.Utc)
            {
                dateTimeUtc = datetime.ToUniversalTime();
            }

            if (dateTimeUtc.ToUniversalTime() <= UnixEpoch)
            {
                return 0;
            }

            return (long)(dateTimeUtc - UnixEpoch).TotalSeconds;
        }

        /// <summary>
        /// Creates a <see cref="DateTime"/> from epoch time.
        /// </summary>
        /// <param name="secondsSinceUnixEpoch">The number of seconds since Unix Epoch.</param>
        /// <returns>The DateTime in UTC.</returns>
        public static DateTime DateTime(long secondsSinceUnixEpoch)
        {
            if (secondsSinceUnixEpoch <= 0)
            {
                return UnixEpoch;
            }

            if (secondsSinceUnixEpoch > TimeSpan.MaxValue.TotalSeconds)
                return DateTimeUtil.Add(UnixEpoch, TimeSpan.MaxValue).ToUniversalTime();

            return DateTimeUtil.Add(UnixEpoch, TimeSpan.FromSeconds(secondsSinceUnixEpoch)).ToUniversalTime();
        }
    }
}

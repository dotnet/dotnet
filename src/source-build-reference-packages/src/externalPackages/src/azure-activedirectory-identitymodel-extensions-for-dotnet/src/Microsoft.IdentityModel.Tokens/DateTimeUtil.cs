// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Utility class for performing operations involving <see cref="DateTime"/> and <see cref="TimeSpan"/>.
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        /// Adds a <see cref="DateTime"/> and a <see cref="TimeSpan"/>.
        /// If the resulting value exceeds <see cref="DateTime.MaxValue"/>, returns <see cref="DateTime.MaxValue"/>.
        /// If the resulting value is less than <see cref="DateTime.MinValue"/>, returns <see cref="DateTime.MinValue"/>.
        /// </summary>
        /// <param name="time">Initial <see cref="DateTime"/> value.</param>
        /// <param name="timespan"><see cref="TimeSpan"/> to add.</param>
        /// <returns>The sum of <paramref name="time"/> and <paramref name="timespan"/>, or <see cref="DateTime.MaxValue"/> if the sum exceeds it, or <see cref="DateTime.MinValue"/> if the sum is less than it.</returns>
        public static DateTime Add(DateTime time, TimeSpan timespan)
        {
            if (timespan == TimeSpan.Zero)
            {
                return time;
            }

            if (timespan > TimeSpan.Zero && DateTime.MaxValue - time <= timespan)
            {
                return GetMaxValue(time.Kind);
            }

            if (timespan < TimeSpan.Zero && DateTime.MinValue - time >= timespan)
            {
                return GetMinValue(time.Kind);
            }

            return time + timespan;
        }

        /// <summary>
        /// Gets the maximum value for a <see cref="DateTime"/> with the specified <see cref="DateTimeKind"/>.
        /// </summary>
        /// <param name="kind">The <see cref="DateTimeKind"/>.</param>
        /// <returns>The maximum <see cref="DateTime"/> value of the specified kind.</returns>
        public static DateTime GetMaxValue(DateTimeKind kind)
        {
            if (kind == DateTimeKind.Unspecified)
                return new DateTime(DateTime.MaxValue.Ticks, DateTimeKind.Utc);

            return new DateTime(DateTime.MaxValue.Ticks, kind);
        }

        /// <summary>
        /// Gets the minimum value for a <see cref="DateTime"/> with the specified <see cref="DateTimeKind"/>.
        /// </summary>
        /// <param name="kind">The <see cref="DateTimeKind"/>.</param>
        /// <returns>The minimum <see cref="DateTime"/> value of the specified kind.</returns>
        public static DateTime GetMinValue(DateTimeKind kind)
        {
            if (kind == DateTimeKind.Unspecified)
                return new DateTime(DateTime.MinValue.Ticks, DateTimeKind.Utc);

            return new DateTime(DateTime.MinValue.Ticks, kind);
        }

        /// <summary>
        /// Converts the specified <see cref="DateTime"/> to UTC if it is not already in UTC.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to convert.</param>
        /// <returns>The converted <see cref="DateTime"/> in UTC, or null if <paramref name="value"/> is null.</returns>
        public static DateTime? ToUniversalTime(DateTime? value)
        {
            if (value == null || value.Value.Kind == DateTimeKind.Utc)
                return value;

            return ToUniversalTime(value.Value);
        }

        /// <summary>
        /// Converts the specified <see cref="DateTime"/> to UTC if it is not already in UTC.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to convert.</param>
        /// <returns>The converted <see cref="DateTime"/> in UTC.</returns>
        public static DateTime ToUniversalTime(DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
                return value;

            return value.ToUniversalTime();
        }
    }
}

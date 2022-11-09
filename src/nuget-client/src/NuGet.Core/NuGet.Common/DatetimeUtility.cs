// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace NuGet.Common
{
    /// <summary>
    /// static class to provide datetime common utility apis
    /// </summary>
    public static class DatetimeUtility
    {
        /// <summary>
        /// take timespan n return in appropriate unit like ms, or seconds, or minutes, or hours
        /// </summary>
        /// <param name="time">timespan</param>
        /// <returns>A human-readable timespan string</returns>
        public static string ToReadableTimeFormat(TimeSpan time)
        {
            return ToReadableTimeFormat(time, CultureInfo.CurrentCulture);
        }

        internal static string ToReadableTimeFormat(TimeSpan time, IFormatProvider format)
        {
            // initially define as hours
            double result = time.TotalHours;

            if (time.TotalSeconds < 1)
            {
                result = time.TotalMilliseconds;
                // If less than 1 ms, show only 1 significant figure
                if (result >= 1)
                {
                    result = Math.Round(result, 0);
                }
                else if (result >= 0.1)
                {
                    result = Math.Round(result, 1);
                }

                return string.Format(format, Strings.TimeUnits_Millisecond, result);
            }
            else if (time.TotalMinutes < 1)
            {
                result = time.TotalSeconds;
                return string.Format(format, Strings.TimeUnits_Second, result);
            }
            else if (time.TotalHours < 1)
            {
                result = time.TotalMinutes;
                return string.Format(format, Strings.TimeUnits_Minute, result);
            }

            return string.Format(format, Strings.TimeUnits_Hour, result);
        }
    }
}

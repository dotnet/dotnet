namespace Microsoft.ApplicationInsights.Common.Extensions
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Provides extension methods for <see cref="DateTime"/>.
    /// </summary>
    internal static class DateTimeExtensions
    {
        /// <summary>
        /// This is a proxy method to <see cref="DateTime.ToString(string, IFormatProvider)"/>.
        /// Converts the value of the current System.DateTime object to its equivalent string representation using the specified format and CultureInfo.InvariantCulture.
        /// </summary>
        /// <param name="input">The date and time value to convert to string.</param>
        /// <param name="format">A standard or custom date and time format string.</param>
        /// <returns>A string representation of value of the current System.DateTime object as specified by format and provider.</returns>
        public static string ToInvariantString(this DateTime input, string format) => input.ToString(format, CultureInfo.InvariantCulture);
    }
}
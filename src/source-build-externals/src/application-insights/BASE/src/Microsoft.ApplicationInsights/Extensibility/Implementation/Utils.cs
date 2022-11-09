namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Various utilities.
    /// </summary>
    internal static partial class Utils
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (value == null)
            {
                return true;
            }

            return value.All(char.IsWhiteSpace);
        }

        public static void CopyDictionary<TValue>(IDictionary<string, TValue> source, IDictionary<string, TValue> target)
        {
            foreach (KeyValuePair<string, TValue> pair in source)
            {
                if (string.IsNullOrEmpty(pair.Key))
                {
                    continue;
                }

                if (!target.ContainsKey(pair.Key))
                {
                    target[pair.Key] = pair.Value;
                }
            }
        }

        /// <summary>
        /// Validates the string and if null or empty populates it with '$parameterName is a required field for $telemetryType' value.
        /// </summary>
        public static string PopulateRequiredStringValue(string value, string parameterName, string telemetryType)
        {
            if (string.IsNullOrEmpty(value))
            {
                CoreEventSource.Log.PopulateRequiredStringWithValue(parameterName, telemetryType);
                return "n/a";
            }

            return value;
        }

        /// <summary>
        /// Returns default Timespan value if not a valid Timespan.
        /// </summary>
        public static TimeSpan ValidateDuration(string value)
        {
            TimeSpan interval;
            if (!TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out interval))
            {
                CoreEventSource.Log.TelemetryIncorrectDuration();
                return TimeSpan.Zero;
            }

            return interval;
        }

        public static double SanitizeNanAndInfinity(double value)
        {
            bool valueChanged;
            return SanitizeNanAndInfinity(value, out valueChanged);
        }

        public static double SanitizeNanAndInfinity(double value, out bool valueChanged)
        {
            valueChanged = false;

            // Disallow Nan and Infinity since Breeze does not accept it
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                value = 0;
                valueChanged = true;
            }

            return value;
        }
    }
}

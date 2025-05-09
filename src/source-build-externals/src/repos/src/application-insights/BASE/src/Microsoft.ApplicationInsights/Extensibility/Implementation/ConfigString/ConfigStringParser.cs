namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ConfigString
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    using static System.FormattableString;

    /// <summary>
    /// Helper class to parse a configuration string.
    /// A configuration string is defined as a string composed of key/value pairs.
    /// (ex: "key1=value1;key2=value2;key3=value3").
    /// </summary>
    internal static class ConfigStringParser
    {
        private static readonly char[] SplitSemicolon = new char[] { ';' };

        /// <summary>
        /// Parse a given string and return a dictionary of the key/value pairs.
        /// This method will do some validation and throw exceptions if the input string does not conform to the definition of a configuration string.
        /// </summary>
        /// <param name="configString">Input string to be parsed. This string cannot be null or empty. This string will be checked for validity.</param>
        /// <returns>Returns a dictionary of Key/Value pairs. Keys are not case sensitive.</returns>
        /// <remarks>This is used by both Connection Strings and Self-Diagnostics configuration.</remarks>
        public static IDictionary<string, string> Parse(string configString)
        {
            if (configString == null)
            {
                string message = "Input cannot be null.";
                CoreEventSource.Log.ConfigurationStringParseWarning(message);
                throw new ArgumentNullException(message);
            }
            else if (string.IsNullOrWhiteSpace(configString))
            {
                string message = Invariant($"Input cannot be empty.");
                CoreEventSource.Log.ConfigurationStringParseWarning(message);
                throw new ArgumentException(message);
            }

            var keyValuePairs = configString.Split(SplitSemicolon, StringSplitOptions.RemoveEmptyEntries);
            var dictionary = new Dictionary<string, string>(keyValuePairs.Length, StringComparer.OrdinalIgnoreCase);

            foreach (var pair in keyValuePairs)
            {
                var keyAndValue = pair.Split('=');
                if (keyAndValue.Length != 2)
                {
                    string message = Invariant($"Input contains invalid delimiters and cannot be parsed. Expected example: 'key1=value1;key2=value2;key3=value3'.");
                    CoreEventSource.Log.ConfigurationStringParseWarning(message);
                    throw new ArgumentException(message);
                }

                var key = keyAndValue[0].Trim();
                var value = keyAndValue[1].Trim();

                if (dictionary.ContainsKey(key))
                {
                    string message = Invariant($"Input cannot contain duplicate keys. Duplicate key: '{key}'.");
                    CoreEventSource.Log.ConfigurationStringParseWarning(message);
                    throw new ArgumentException(message);
                }

                dictionary.Add(key, value);
            }

            return dictionary;
        }
    }
}
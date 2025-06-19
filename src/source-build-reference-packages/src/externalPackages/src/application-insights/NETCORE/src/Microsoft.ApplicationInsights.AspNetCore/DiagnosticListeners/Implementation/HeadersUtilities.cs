namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Generic functions that can be used to get and set Http headers.
    /// </summary>
    internal static class HeadersUtilities
    {
        /// <summary>
        /// Get the key value from the provided HttpHeader value that is set up as a comma-separated list of key value pairs. Each key value pair is formatted like (key)=(value).
        /// </summary>
        /// <param name="headerValues">The header values that may contain key name/value pairs.</param>
        /// <param name="keyName">The name of the key value to find in the provided header values.</param>
        /// <returns>The first key value, if it is found. If it is not found, then null.</returns>
        public static string GetHeaderKeyValue(IEnumerable<string> headerValues, string keyName)
        {
            if (headerValues != null)
            {
                foreach (string keyNameValue in headerValues)
                {
                    string[] keyNameValueParts = keyNameValue.Trim().Split('=');
                    if (keyNameValueParts.Length == 2 && keyNameValueParts[0].Trim() == keyName)
                    {
                        string value = keyNameValueParts[1].Trim();
                        return StringUtilities.EnforceMaxLength(value, InjectionGuardConstants.RequestHeaderMaxLength);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Given the provided list of header value strings, return a comma-separated list of key
        /// name/value pairs with the provided keyName and keyValue. If the initial header value
        /// string contains the key name, then the original key value should be replaced with the
        /// provided key value. If the initial header value strings don't contain the key name,
        /// then the key name/value pair should be added to the comma-separated list and returned.
        /// </summary>
        /// <param name="currentHeaders">The existing header values that the key/value pair should be added to.</param>
        /// <param name="key">The name of the key to add.</param>
        /// <param name="value">The value of the key to add.</param>
        /// <returns>The result of setting the provided key name/value pair into the provided headerValues.</returns>
        public static StringValues SetHeaderKeyValue(string[] currentHeaders, string key, string value)
        {
            if (currentHeaders != null)
            {
                for (int index = 0; index < currentHeaders.Length; index++)
                {
                    if (HeaderMatchesKey(currentHeaders[index], key))
                    {
                        currentHeaders[index] = string.Concat(key, "=", value);
                        return currentHeaders;
                    }
                }

                return StringValues.Concat(currentHeaders, string.Concat(key, "=", value));
            }
            else
            {
                return string.Concat(key, "=", value);
            }
        }

        /// <summary>
        /// Http Headers only allow Printable US-ASCII characters.
        /// Remove all other characters.
        /// </summary>
        /// <param name="input">String to be sanitized.</param>
        /// <returns>sanitized string.</returns>
        public static string SanitizeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            // US-ASCII characters (hex: 0x00 - 0x7F) (decimal: 0-127)
            // ASCII Extended characters (hex: 0x80 - 0xFF) (decimal: 128-255) (NOT ALLOWED)
            // Non-Printable ASCII characters are (hex: 0x00 - 0x1F) (decimal: 0-31) (NOT ALLOWED)
            // Printable ASCII characters are (hex: 0x20 - 0xFF) (decimal: 32-255)
            return Regex.Replace(input, @"[^\u0020-\u007F]", string.Empty);
        }

        /// <summary>
        /// Check if the header contains the key, case insensitve, ignore leading and trailing whitepsaces.
        /// </summary>
        /// <param name="headerValue">A header value that might contains key value pair.</param>
        /// <param name="key">The key to match.</param>
        /// <returns>Return true when the key matches and return false with it doens't.</returns>
        private static bool HeaderMatchesKey(string headerValue, string key)
        {
            int equalsSignIndex = headerValue.IndexOf('=');
            if (equalsSignIndex < 0)
            {
                return false;
            }

            // Skip leading whitespace
            int start;
            for (start = 0; start < equalsSignIndex; start++)
            {
                if (!char.IsWhiteSpace(headerValue[start]))
                {
                    break;
                }
            }

            if (string.CompareOrdinal(headerValue, start, key, 0, key.Length) != 0)
            {
                return false;
            }

            // Check trailing whitespace
            for (int i = start + key.Length; i < equalsSignIndex; i++)
            {
                if (!char.IsWhiteSpace(headerValue[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

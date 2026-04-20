namespace Microsoft.ApplicationInsights.Common
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
#if DEPENDENCY_COLLECTOR
    using Microsoft.ApplicationInsights.Common;
#else
    using Microsoft.ApplicationInsights.Common.Internal;
#endif

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

        public static IDictionary<string, string> GetHeaderDictionary(IEnumerable<string> headerValues)
        {
            IDictionary<string, string> result = null;

            if (headerValues != null)
            {
                result = new Dictionary<string, string>();
                foreach (string keyNameValue in headerValues)
                {
                    string[] keyNameValueParts = keyNameValue.Trim().Split('=');
                    if (keyNameValueParts.Length == 2)
                    {
                        string keyName = keyNameValueParts[0].Trim();
                        if (!result.ContainsKey(keyName))
                        {
                            string value = keyNameValueParts[1].Trim();
                            value = StringUtilities.EnforceMaxLength(value, InjectionGuardConstants.RequestHeaderMaxLength);
                            result.Add(keyName, value);
                        }
                    }
                }
            }

            if (result == null || !result.Any())
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// Given the provided list of header value strings, return a list of key name/value pairs
        /// with the provided keyName and keyValue. If the initial header value strings contains
        /// the key name, then the original key value should be replaced with the provided key
        /// value. If the initial header value strings don't contain the key name, then the key
        /// name/value pair should be added to the list and returned.
        /// </summary>
        /// <param name="headerValues">The existing header values that the key/value pair should be added to.</param>
        /// <param name="keyName">The name of the key to add.</param>
        /// <param name="keyValue">The value of the key to add.</param>
        /// <returns>The result of setting the provided key name/value pair into the provided headerValues.</returns>
        public static IEnumerable<string> UpdateHeaderWithKeyValue(IEnumerable<string> headerValues, string keyName, string keyValue)
        {
            string[] newHeaderKeyValue = new[] { string.Format(CultureInfo.InvariantCulture, "{0}={1}", keyName.Trim(), keyValue.Trim()) };
            return headerValues == null || !headerValues.Any()
                ? newHeaderKeyValue
                : headerValues
                    .Where((string headerValue) =>
                    {
                        int equalsSignIndex = headerValue.IndexOf('=');
                        return equalsSignIndex == -1 || TrimSubstring(headerValue, 0, equalsSignIndex) != keyName;
                    })
                    .Concat(newHeaderKeyValue);
        }

        /// <summary>
        /// Http Headers only allow Printable US-ASCII characters.
        /// Remove all other characters.
        /// </summary>
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
            return Regex.Replace(input, @"[^\u0020-\u007F]", string.Empty); // (hex: 0x20 - 0x7F) (decimal: 32-127)
        }

        private static string TrimSubstring(string value, int startIndex, int endIndex)
        {
            int firstNonWhitespaceIndex = -1;
            int last = -1;
            for (int firstSearchIndex = startIndex; firstSearchIndex < endIndex; ++firstSearchIndex)
            {
                if (!char.IsWhiteSpace(value[firstSearchIndex]))
                {
                    firstNonWhitespaceIndex = firstSearchIndex;

                    // Found the first non-whitespace character index, now look for the last.
                    for (int lastSearchIndex = endIndex - 1; lastSearchIndex >= startIndex; --lastSearchIndex)
                    {
                        if (!char.IsWhiteSpace(value[lastSearchIndex]))
                        {
                            last = lastSearchIndex;
                            break;
                        }
                    }

                    break;
                }
            }

            return firstNonWhitespaceIndex == -1 ? null : value.Substring(firstNonWhitespaceIndex, last - firstNonWhitespaceIndex + 1);
        }
    }
}

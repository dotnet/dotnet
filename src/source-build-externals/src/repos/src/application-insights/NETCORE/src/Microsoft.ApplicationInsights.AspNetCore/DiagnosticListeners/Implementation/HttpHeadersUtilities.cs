namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// This class contains several utility methods for working with Http Headers.
    /// </summary>
    internal static class HttpHeadersUtilities
    {
        /// <summary>
        /// Get all values of a header by name.
        /// </summary>
        /// <param name="headers">Collection of Http Headers.</param>
        /// <param name="headerName">Header name.</param>
        /// <returns>Returns a collection of values matching the header name.</returns>
        internal static IEnumerable<string> GetHeaderValues(IHeaderDictionary headers, string headerName)
        {
            IEnumerable<string> result = Enumerable.Empty<string>();
            if (headers != null)
            {
                StringValues headerValues = headers[headerName];
                if (!StringValues.IsNullOrEmpty(headerValues))
                {
                    result = headerValues.SelectMany(headerValue => headerValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }

            return result;
        }

        /// <summary>
        /// Get the Header Value matching a Name and Key.
        /// </summary>
        /// <param name="headers">Collection of Http Headers.</param>
        /// <param name="headerName">Header name.</param>
        /// <param name="keyName">Key name.</param>
        /// <returns>The first key value, if it is found. If it is not found, then null.</returns>
        internal static string GetHeaderKeyValue(IHeaderDictionary headers, string headerName, string keyName)
        {
            IEnumerable<string> headerValues = GetHeaderValues(headers, headerName);
            return HeadersUtilities.GetHeaderKeyValue(headerValues, keyName);
        }

        /// <summary>
        /// Get a key value from the Request Context header.
        /// </summary>
        /// <param name="headers">Collection of Http Headers.</param>
        /// <param name="keyName">Key name.</param>
        /// <returns>The first key value, if it is found. If it is not found, then null.</returns>
        internal static string GetRequestContextKeyValue(IHeaderDictionary headers, string keyName)
        {
            return GetHeaderKeyValue(headers, RequestResponseHeaders.RequestContextHeader, keyName);
        }

        /// <summary>
        /// Checks if a specified key exists in the Request Context Header.
        /// </summary>
        /// <param name="headers">Collection of Http Headers.</param>
        /// <param name="keyName">Key name.</param>
        /// <returns>Returns a boolean indicating if the key exists.</returns>
        internal static bool ContainsRequestContextKeyValue(IHeaderDictionary headers, string keyName)
        {
            return !string.IsNullOrEmpty(GetHeaderKeyValue(headers, RequestResponseHeaders.RequestContextHeader, keyName));
        }

        /// <summary>
        /// Sets a value on the Request Context Headers.
        /// </summary>
        /// <param name="headers">Collection of Http Headers.</param>
        /// <param name="keyName">Key name to set.</param>
        /// <param name="keyValue">Key value to set.</param>
        internal static void SetRequestContextKeyValue(IHeaderDictionary headers, string keyName, string keyValue)
        {
            SetHeaderKeyValue(headers, RequestResponseHeaders.RequestContextHeader, keyName, keyValue);
        }

        /// <summary>
        /// Sets a key value on the Http Headers.
        /// </summary>
        /// <param name="headers">Collection of Http Headers.</param>
        /// <param name="headerName">Http Header name.</param>
        /// <param name="keyName">Key name to set.</param>
        /// <param name="keyValue">Key value to set.</param>
        internal static void SetHeaderKeyValue(IHeaderDictionary headers, string headerName, string keyName, string keyValue)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            headers[headerName] = HeadersUtilities.SetHeaderKeyValue(headers[headerName], keyName, keyValue);
        }

        /// <summary>
        /// Get the values from an Http Header.
        /// </summary>
        /// <param name="headers">Collection of Http Headers.</param>
        /// <param name="headerName">Http header name.</param>
        /// <param name="maxLength">Max length of return values.</param>
        /// <param name="maxItems">Max count of return values.</param>
        /// <returns>Returns an array of the Http values.</returns>
        internal static string[] SafeGetCommaSeparatedHeaderValues(IHeaderDictionary headers, string headerName, int maxLength, int maxItems)
        {
            string[] traceStateValues = headers.GetCommaSeparatedValues(headerName);

            if (traceStateValues == null)
            {
                return null;
            }

            int length = traceStateValues.Sum(p => p.Length) + traceStateValues.Length - 1; // all values and commas
            if (length <= maxLength && traceStateValues.Length <= maxItems)
            {
                return traceStateValues;
            }

            List<string> truncated;
            if (length > maxLength)
            {
                int currentLength = 0;

                truncated = traceStateValues.TakeWhile(kvp =>
                {
                    if (currentLength + kvp.Length > maxLength)
                    {
                        return false;
                    }

                    currentLength += kvp.Length + 1; // pair and comma
                    return true;
                }).ToList();
            }
            else
            {
                truncated = traceStateValues.ToList();
            }

            // if there are more than maxItems - truncate the end
            if (truncated.Count > maxItems)
            {
                return truncated.Take(maxItems).ToArray();
            }

            return truncated.ToArray();
        }
    }
}

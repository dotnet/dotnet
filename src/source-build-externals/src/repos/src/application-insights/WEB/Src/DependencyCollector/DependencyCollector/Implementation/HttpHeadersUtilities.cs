namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using Microsoft.ApplicationInsights.Common;

    internal static class HttpHeadersUtilities
    {
        internal static IEnumerable<string> GetHeaderValues(HttpHeaders headers, string headerName)
        {
            IEnumerable<string> result;
            if (headers == null || !headers.TryGetValues(headerName, out result))
            {
                result = Enumerable.Empty<string>();
            }

            return result;
        }

        internal static string GetHeaderKeyValue(HttpHeaders headers, string headerName, string keyName)
        {
            IEnumerable<string> headerValues = GetHeaderValues(headers, headerName);
            return HeadersUtilities.GetHeaderKeyValue(headerValues, keyName);
        }

        internal static string GetRequestContextKeyValue(HttpHeaders headers, string keyName)
        {
            return GetHeaderKeyValue(headers, RequestResponseHeaders.RequestContextHeader, keyName);
        }

        internal static bool ContainsHeaderKeyValue(HttpHeaders headers, string headerName, string keyName)
        {
            return !string.IsNullOrEmpty(GetHeaderKeyValue(headers, headerName, keyName));
        }

        internal static bool ContainsRequestContextKeyValue(HttpHeaders headers, string keyName)
        {
            return ContainsHeaderKeyValue(headers, RequestResponseHeaders.RequestContextHeader, keyName);
        }

        internal static void SetRequestContextKeyValue(HttpHeaders headers, string keyName, string keyValue)
        {
            SetHeaderKeyValue(headers, RequestResponseHeaders.RequestContextHeader, keyName, keyValue);
        }

        internal static void SetHeaderKeyValue(HttpHeaders headers, string headerName, string keyName, string keyValue)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            IEnumerable<string> headerValues = GetHeaderValues(headers, headerName);
            headers.Remove(headerName);
            headers.Add(headerName, HeadersUtilities.UpdateHeaderWithKeyValue(headerValues, keyName, keyValue));
        }
    }
}

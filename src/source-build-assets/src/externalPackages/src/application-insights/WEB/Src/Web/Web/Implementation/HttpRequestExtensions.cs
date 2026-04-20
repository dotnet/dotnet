namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// HttpRequest Extensions.
    /// </summary>
    internal static partial class HttpRequestExtensions
    {
        public static HttpCookie UnvalidatedGetCookie(this HttpRequest httpRequest, string name)
        {
            return httpRequest.Unvalidated.Cookies[name];
        }

        public static string UnvalidatedGetHeader(this HttpRequest httpRequest, string headerName)
        {
            string value = httpRequest.Unvalidated.Headers[headerName];
            return StringUtilities.EnforceMaxLength(value, InjectionGuardConstants.RequestHeaderMaxLength);
        }

        public static Uri UnvalidatedGetUrl(this HttpRequest httpRequest)
        {
            try
            {
                return httpRequest.Unvalidated.Url;
            }
            catch (UriFormatException)
            {
                WebEventSource.Log.WebUriFormatException();
                return null;
            }
        }

        public static string UnvalidatedGetPath(this HttpRequest httpRequest)
        {
            return httpRequest.Unvalidated.Path;
        }

        public static NameValueCollection UnvalidatedGetHeaders(this HttpRequest httpRequest)
        {
            return httpRequest.Unvalidated.Headers;
        }

        public static string GetUserHostAddress(this HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                return null;
            }

            try
            {
                return httpRequest.UserHostAddress;
            }
            catch (ArgumentException exp)
            {
                // System.ArgumentException: Value does not fall within the expected range. Fails in IIS7, WCF OneWay.
                WebEventSource.Log.UserHostNotCollectedWarning(exp.ToInvariantString());
                return null;
            }
        }
    }
}

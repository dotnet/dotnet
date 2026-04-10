namespace Microsoft.ApplicationInsights.Tests
{
    using System.Net;

    internal static class DependencyCollectorTestHelpers
    {
        internal static string GetCookieValueFromWebRequest(HttpWebRequest webRequest, string cookieKey)
        {
            if (webRequest.CookieContainer != null)
            {
                CookieCollection collection = webRequest.CookieContainer.GetCookies(webRequest.RequestUri);
                string cookie = collection[cookieKey].ToString();
                return cookie;
            }

            return null;
        }
    }
}

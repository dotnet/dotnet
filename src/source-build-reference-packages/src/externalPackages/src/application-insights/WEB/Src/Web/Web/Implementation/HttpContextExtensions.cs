namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System.Web;

    internal static class HttpContextExtensions
    {
        public static HttpRequest GetRequest(this HttpContext context)
        {
            // HttpRequest is not available in HttContext till Application_AquireRequestState
            // But there is no way to check it; only catch HttpException
            HttpRequest result = null;
            try
            {
                result = context.Request;
            }
            catch (HttpException exp)
            {
                WebEventSource.Log.HttpRequestNotAvailable(exp.Message, exp.StackTrace);
            }

            return result;
        }

        public static HttpResponse GetResponse(this HttpContext context)
        {
            // HttpResponse is not available in HttContext till Application_AquireRequestState
            // But there is no way to check it; only catch HttpException
            HttpResponse result = null;
            try
            {
                result = context.Response;
            }
            catch (HttpException exp)
            {
                WebEventSource.Log.HttpRequestNotAvailable(exp.Message, exp.StackTrace);
            }

            return result;
        }
    }
}

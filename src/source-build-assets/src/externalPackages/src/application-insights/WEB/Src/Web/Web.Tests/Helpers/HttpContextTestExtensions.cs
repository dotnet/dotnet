namespace Microsoft.ApplicationInsights.Web.Helpers
{
    using System.Web;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    internal static class HttpContextTestExtensions
    {
        internal static RequestTelemetry WithAuthCookie(this HttpContext context, string cookieString)
        {
            var requestTelemetry = new RequestTelemetry();
            context.AddRequestCookie(
                new HttpCookie(
                    RequestTrackingConstants.WebAuthenticatedUserCookieName,
                                                    HttpUtility.UrlEncode(cookieString)))
                   .AddRequestTelemetry(requestTelemetry);
            return requestTelemetry;
        }
    }
}

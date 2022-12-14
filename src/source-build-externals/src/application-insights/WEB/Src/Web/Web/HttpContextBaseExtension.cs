namespace System.Web
{
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// HttpContextBaseExtension class provides extensions methods for accessing Web Application Insights objects.
    /// </summary>
    public static class HttpContextBaseExtension
    {        
        /// <summary>
        /// Provide access to request generated by Web Application Insights SDK.
        /// </summary>
        /// <param name="context">HttpContextBase instance.</param>
        /// <returns>Request telemetry instance or null.</returns>
        [Obsolete("Use HttpContextExtension.GetRequestTelemetry instead")]
        public static RequestTelemetry GetRequestTelemetry(this HttpContextBase context)
        {
            if (context == null || context.ApplicationInstance == null || context.ApplicationInstance.Context == null)
            {
                return null;
            }

            return context.ApplicationInstance.Context.Items[RequestTrackingConstants.RequestTelemetryItemName] as RequestTelemetry;
        }
    }
}
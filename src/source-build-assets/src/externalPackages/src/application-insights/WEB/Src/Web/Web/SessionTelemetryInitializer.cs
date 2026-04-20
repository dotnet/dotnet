namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// A telemetry initializer that will set the Session properties of Context corresponding to a RequestTelemetry object.
    /// Session is updated with properties derived from the RequestTelemetry.RequestTelemetry.Context.Session.
    /// </summary>
    public class SessionTelemetryInitializer : WebTelemetryInitializerBase
    {
        private const string WebSessionCookieName = "ai_session";
        private const int SessionCookieSessionIdIndex = 0;

        internal static void GetSessionContextFromUserCookie(HttpCookie sessionCookie, RequestTelemetry requestTelemetry)
        {
            if (sessionCookie == null)
            {
                WebEventSource.Log.WebSessionTrackingSessionCookieIsNotSpecifiedInRequest();
                return;
            }

            if (string.IsNullOrWhiteSpace(sessionCookie.Value))
            {
                WebEventSource.Log.WebSessionTrackingSessionCookieIsEmptyWarning();
                return;
            }

            var parts = sessionCookie.Value.Split('|');

            if (parts.Length > 0)
            {
                requestTelemetry.Context.Session.Id = parts[SessionCookieSessionIdIndex];
            }

            requestTelemetry.Context.Session.Id = parts[SessionCookieSessionIdIndex];
        }

        /// <summary>
        /// Implements initialization logic.
        /// </summary>
        /// <param name="platformContext">Http context.</param>
        /// <param name="requestTelemetry">Request telemetry object associated with the current request.</param>
        /// <param name="telemetry">Telemetry item to initialize.</param>
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (requestTelemetry == null)
            {
                throw new ArgumentNullException(nameof(requestTelemetry));
            }

            if (platformContext == null)
            {
                throw new ArgumentNullException(nameof(platformContext));
            }

            if (string.IsNullOrEmpty(telemetry.Context.Session.Id))
            {
                if (string.IsNullOrEmpty(requestTelemetry.Context.Session.Id))
                {
                    GetSessionContextFromUserCookie(platformContext.Request.UnvalidatedGetCookie(WebSessionCookieName), requestTelemetry);
                }

                telemetry.Context.Session.Id = requestTelemetry.Context.Session.Id;

                if (requestTelemetry.Context.Session.IsFirst.HasValue)
                {
                    telemetry.Context.Session.IsFirst = requestTelemetry.Context.Session.IsFirst;
                }
            }
        }
    }
}

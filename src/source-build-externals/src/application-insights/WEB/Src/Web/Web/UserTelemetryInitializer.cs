namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// A telemetry initializer that will set the User properties of Context corresponding to a RequestTelemetry object.
    /// User.Id are updated with properties derived from the RequestTelemetry.RequestTelemetry.Context.User.
    /// </summary>
    public class UserTelemetryInitializer : WebTelemetryInitializerBase
    {
        private const string WebUserCookieName = "ai_user";

        internal static void GetUserContextFromUserCookie(HttpCookie userCookie, RequestTelemetry requestTelemetry)
        {
            if (userCookie == null)
            {
                WebEventSource.Log.WebUserTrackingUserCookieNotAvailable();
                return;
            }

            if (string.IsNullOrEmpty(userCookie.Value))
            {
                WebEventSource.Log.WebUserTrackingUserCookieIsEmpty();
                return;
            }

            var cookieParts = userCookie.Value.Split('|');

            if (cookieParts.Length < 2)
            {
                WebEventSource.Log.WebUserTrackingUserCookieIsIncomplete(userCookie.Value);
                return;
            }

            requestTelemetry.Context.User.Id = cookieParts[0];
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

            if (string.IsNullOrEmpty(telemetry.Context.User.Id))
            {
                if (string.IsNullOrEmpty(requestTelemetry.Context.User.Id))
                {
                    GetUserContextFromUserCookie(platformContext.Request.UnvalidatedGetCookie(WebUserCookieName), requestTelemetry);
                }

                telemetry.Context.User.Id = requestTelemetry.Context.User.Id;
            }
        }
    }
}

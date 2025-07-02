namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// A telemetry initializer that will set the User properties of Context corresponding to a RequestTelemetry object.
    /// User.AccountId is updated with properties derived from the RequestTelemetry.RequestTelemetry.Context.User.
    /// </summary>
    public class AccountIdTelemetryInitializer : WebTelemetryInitializerBase
    {
        internal static void GetAuthUserContextFromUserCookie(HttpCookie authUserCookie, RequestTelemetry requestTelemetry)
        {
            if (authUserCookie == null)
            {
                // Request does not have authenticated user cookie
                WebEventSource.Log.AuthIdTrackingCookieNotAvailable();
                return;
            }

            if (string.IsNullOrEmpty(authUserCookie.Value))
            {
                // Request does not have authenticated user cookie
                WebEventSource.Log.AuthIdTrackingCookieIsEmpty();
                return;
            }

            var authUserCookieString = HttpUtility.UrlDecode(authUserCookie.Value);

            var cookieParts = authUserCookieString.Split('|');

            if (cookieParts.Length > 1)
            {
                requestTelemetry.Context.User.AccountId = cookieParts[1];
            }
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

            if (string.IsNullOrEmpty(telemetry.Context.User.AccountId))
            {
                if (string.IsNullOrEmpty(requestTelemetry.Context.User.AccountId))
                {
                    GetAuthUserContextFromUserCookie(platformContext.Request.UnvalidatedGetCookie(RequestTrackingConstants.WebAuthenticatedUserCookieName), requestTelemetry);
                }

                telemetry.Context.User.AccountId = requestTelemetry.Context.User.AccountId;
            }
        }
    }
}

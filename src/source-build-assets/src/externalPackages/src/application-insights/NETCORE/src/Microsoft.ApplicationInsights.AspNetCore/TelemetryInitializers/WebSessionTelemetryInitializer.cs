namespace Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers
{
    using System;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// This telemetry initializer sets the Context.Session properties on an ITelemetry item.
    /// </summary>
    public class WebSessionTelemetryInitializer : TelemetryInitializerBase
    {
        private const string WebSessionCookieName = "ai_session";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSessionTelemetryInitializer" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor to provide HttpContext corresponding to telemetry items.</param>
        public WebSessionTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
             : base(httpContextAccessor)
        {
        }

        /// <inheritdoc />
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (!string.IsNullOrEmpty(telemetry.Context.Session.Id))
            {
                AspNetCoreEventSource.Instance.LogWebSessionTelemetryInitializerOnInitializeTelemetrySessionIdNull();
                return;
            }

            if (requestTelemetry == null)
            {
                throw new ArgumentNullException(nameof(requestTelemetry));
            }

            if (string.IsNullOrEmpty(requestTelemetry.Context.Session.Id))
            {
                if (platformContext == null)
                {
                    throw new ArgumentNullException(nameof(platformContext));
                }

                UpdateRequestTelemetryFromPlatformContext(requestTelemetry, platformContext);
            }

            if (!string.IsNullOrEmpty(requestTelemetry.Context.Session.Id))
            {
                telemetry.Context.Session.Id = requestTelemetry.Context.Session.Id;
            }
        }

        private static void UpdateRequestTelemetryFromPlatformContext(RequestTelemetry requestTelemetry, HttpContext platformContext)
        {
            if (platformContext.Request?.Cookies != null && platformContext.Request.Cookies.ContainsKey(WebSessionCookieName))
            {
                var sessionCookieValue = platformContext.Request.Cookies[WebSessionCookieName];
                if (!string.IsNullOrEmpty(sessionCookieValue))
                {
                    var sessionCookieParts = ((string)sessionCookieValue).Split('|');
                    if (sessionCookieParts.Length > 0)
                    {
                        // Currently SessionContext takes in only SessionId.
                        // The cookies has SessionAcquisitionDate and SessionRenewDate as well that we are not picking for now.
                        requestTelemetry.Context.Session.Id = sessionCookieParts[0];
                    }
                }
            }
        }
    }
}
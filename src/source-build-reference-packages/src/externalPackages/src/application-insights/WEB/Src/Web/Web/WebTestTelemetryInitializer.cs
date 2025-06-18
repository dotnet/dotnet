namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;
    
    /// <summary>
    /// A telemetry initializer that will update the User, Session and Operation contexts if request originates from a web test.
    /// </summary>
    public class WebTestTelemetryInitializer : WebTelemetryInitializerBase
    {
        private const string GsmSource = "Application Insights Availability Monitoring";
        private const string TestRunHeader = "SyntheticTest-RunId";
        private const string TestLocationHeader = "SyntheticTest-Location";

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

            if (platformContext == null)
            {
                throw new ArgumentNullException(nameof(platformContext));
            }

            if (string.IsNullOrEmpty(telemetry.Context.Operation.SyntheticSource))
            {
                // platformContext and request != null checks are in the base class
                var request = platformContext.GetRequest();

                var runIdHeader = request.UnvalidatedGetHeader(TestRunHeader);
                var locationHeader = request.UnvalidatedGetHeader(TestLocationHeader);

                if (!string.IsNullOrEmpty(runIdHeader) &&
                    !string.IsNullOrEmpty(locationHeader))
                {
                    telemetry.Context.Operation.SyntheticSource = GsmSource;

                    // User id will be Pop location name and RunId (We cannot use just location because of sampling)
                    telemetry.Context.User.Id = locationHeader + "_" + runIdHeader;
                    telemetry.Context.Session.Id = runIdHeader;
                }
            }
        }
    }
}
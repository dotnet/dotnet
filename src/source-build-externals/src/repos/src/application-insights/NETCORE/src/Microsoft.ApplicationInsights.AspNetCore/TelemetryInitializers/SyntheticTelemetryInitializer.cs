namespace Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// This will allow to mark synthetic traffic from availability tests.
    /// </summary>
    public class SyntheticTelemetryInitializer : TelemetryInitializerBase
    {
        private const string SyntheticTestRunId = "SyntheticTest-RunId";
        private const string SyntheticTestLocation = "SyntheticTest-Location";

        private const string SyntheticSourceHeaderValue = "Application Insights Availability Monitoring";

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntheticTelemetryInitializer" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor to provide HttpContext corresponding to telemetry items.</param>
        public SyntheticTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
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

            if (string.IsNullOrEmpty(telemetry.Context.Operation.SyntheticSource))
            {
                if (platformContext == null)
                {
                    throw new ArgumentNullException(nameof(platformContext));
                }

                var runIdHeader = platformContext.Request?.Headers[SyntheticTestRunId];
                var locationHeader = platformContext.Request?.Headers[SyntheticTestLocation];

                if (!string.IsNullOrEmpty(runIdHeader) &&
                    !string.IsNullOrEmpty(locationHeader))
                {
                    telemetry.Context.Operation.SyntheticSource = SyntheticSourceHeaderValue;

                    telemetry.Context.User.Id = locationHeader + "_" + runIdHeader;
                    telemetry.Context.Session.Id = runIdHeader;
                }
            }
        }
    }
}

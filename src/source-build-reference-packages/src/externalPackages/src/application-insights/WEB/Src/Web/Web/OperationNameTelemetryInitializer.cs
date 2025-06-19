namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Web;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// A telemetry initializer that will set the NAME property of OperationContext corresponding to a TraceTelemetry object.
    /// If the telemetry object is of type RequestTelemetry, then the Name of the RequestTelemetry is updated. For all other cases,
    /// Operation.Name is updated with the name derived from the HttpContext.
    /// </summary>
    public class OperationNameTelemetryInitializer : WebTelemetryInitializerBase
    {
        /// <summary>
        /// Implements initialization logic.
        /// </summary>
        /// <param name="platformContext">Http context.</param>
        /// <param name="rootRequestTelemetry">Request telemetry object associated with the current request.</param>
        /// <param name="telemetry">Telemetry item to initialize.</param>
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry rootRequestTelemetry,  ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (rootRequestTelemetry == null)
            {
                throw new ArgumentNullException(nameof(rootRequestTelemetry));
            }

            if (platformContext == null)
            {
                throw new ArgumentNullException(nameof(platformContext));
            }

            if (string.IsNullOrEmpty(telemetry.Context.Operation.Name))
            {
                // Do not cache name because it may be too early to calculate it (e.g. traces on application start).
                // When it is too early to calculate it only that telemetry will have incorrect operation name
                string name = string.IsNullOrEmpty(rootRequestTelemetry.Name) ?
                    platformContext.CreateRequestNamePrivate() :
                    rootRequestTelemetry.Name;
                
                var telemetryType = telemetry as RequestTelemetry;

                if (telemetryType != null && string.IsNullOrEmpty(telemetryType.Name))
                {
                    telemetryType.Name = name;
                }

                telemetry.Context.Operation.Name = name;
            }
        }
    }
}

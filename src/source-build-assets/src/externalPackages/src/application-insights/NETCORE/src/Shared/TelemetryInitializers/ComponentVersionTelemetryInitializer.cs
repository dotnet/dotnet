#if AI_ASPNETCORE_WEB
namespace Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers
#else
namespace Microsoft.ApplicationInsights.WorkerService.TelemetryInitializers
#endif
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
#if AI_ASPNETCORE_WEB
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
#else
    using Microsoft.ApplicationInsights.WorkerService;
#endif
    using Microsoft.Extensions.Options;

    /// <summary>
    /// A telemetry initializer that populates telemetry.Context.Component.Version to the value read from configuration.
    /// </summary>
    public class ComponentVersionTelemetryInitializer : ITelemetryInitializer
    {
        private readonly string version;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentVersionTelemetryInitializer" /> class.
        /// </summary>
        /// <param name="options">Provides the Application Version to be added to the telemetry.</param>
        public ComponentVersionTelemetryInitializer(IOptions<ApplicationInsightsServiceOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.version = options.Value.ApplicationVersion;
        }

        /// <inheritdoc />
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (string.IsNullOrEmpty(telemetry.Context.Component.Version))
            {
                if (!string.IsNullOrEmpty(this.version))
                {
                    telemetry.Context.Component.Version = this.version;
                }
            }
        }
    }
}

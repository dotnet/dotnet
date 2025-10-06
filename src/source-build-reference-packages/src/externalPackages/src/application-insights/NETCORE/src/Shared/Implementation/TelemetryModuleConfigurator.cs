#if AI_ASPNETCORE_WEB
    namespace Microsoft.ApplicationInsights.AspNetCore
#else
    namespace Microsoft.ApplicationInsights.WorkerService
#endif
{
    using System;
    using System.Diagnostics.CodeAnalysis;

#if AI_ASPNETCORE_WEB
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
#else
    using Microsoft.ApplicationInsights.WorkerService;
#endif
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Represents method used to configure <see cref="ITelemetryModule"/> with dependency injection support.
    /// </summary>
    internal class TelemetryModuleConfigurator : ITelemetryModuleConfigurator
    {
        private readonly Action<ITelemetryModule, ApplicationInsightsServiceOptions> configure;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryModuleConfigurator"/> class.
        /// </summary>
        /// <param name="configure">The action used to configure telemetry module.</param>
        /// <param name="telemetryModuleType">The type of the telemetry module being configured.</param>
        public TelemetryModuleConfigurator(Action<ITelemetryModule, ApplicationInsightsServiceOptions> configure, Type telemetryModuleType)
        {
            this.configure = configure;
            this.TelemetryModuleType = telemetryModuleType;
        }

        /// <summary>
        /// Gets the type of <see cref="ITelemetryModule"/> to be configured.
        /// </summary>
        public Type TelemetryModuleType { get; }

        [Obsolete("Use Configure(ITelemetryModule telemetryModule, ApplicationInsightsServiceOptions options) instead.", true)]
        [SuppressMessage("Documentation Rules", "SA1600:ElementsMustBeDocumented", Justification = "This method is obsolete.")]
        public void Configure(ITelemetryModule telemetryModule)
        {
            this.configure?.Invoke(telemetryModule, null);
        }

        /// <summary>
        /// Configures telemetry module.
        /// </summary>
        /// <param name="telemetryModule">Module to be configured.</param>
        /// <param name="options">Configuration options.</param>
        public void Configure(ITelemetryModule telemetryModule, ApplicationInsightsServiceOptions options)
        {
            this.configure?.Invoke(telemetryModule, options);
        }
    }
}
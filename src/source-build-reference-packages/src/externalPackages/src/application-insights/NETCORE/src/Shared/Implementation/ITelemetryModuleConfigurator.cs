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
    public interface ITelemetryModuleConfigurator
    {
        /// <summary>
        /// Gets the type of <see cref="ITelemetryModule"/> to be configured.
        /// </summary>
        Type TelemetryModuleType { get; }

        /// <summary>
        /// Configures the given <see cref="ITelemetryModule"/>.
        /// </summary>
        [Obsolete("Use Configure(ITelemetryModule telemetryModule, ApplicationInsightsServiceOptions options) instead.")]
        [SuppressMessage("Documentation Rules", "SA1600:ElementsMustBeDocumented", Justification = "This method is obsolete.")]
        [SuppressMessage("Documentation Rules", "SA1611:ElementParametersMustBeDocumented", Justification = "This method is obsolete.")]
        void Configure(ITelemetryModule telemetryModule);

        /// <summary>
        /// Configures the given <see cref="ITelemetryModule"/>.
        /// </summary>
        /// <param name="telemetryModule">Module to be configured.</param>
        /// <param name="options">Configuration options.</param>
        void Configure(ITelemetryModule telemetryModule, ApplicationInsightsServiceOptions options);
    }
}
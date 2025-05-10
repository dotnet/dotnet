#if AI_ASPNETCORE_WEB
    namespace Microsoft.ApplicationInsights.AspNetCore
#else
    namespace Microsoft.ApplicationInsights.WorkerService
#endif
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A generic factory for telemetry processors of a given type.
    /// </summary>
    internal class TelemetryProcessorFactory : ITelemetryProcessorFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Type telemetryProcessorType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryProcessorFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="telemetryProcessorType">The type of telemetry processor to create.</param>
        public TelemetryProcessorFactory(IServiceProvider serviceProvider, Type telemetryProcessorType)
        {
            this.serviceProvider = serviceProvider;
            this.telemetryProcessorType = telemetryProcessorType;
        }

        /// <inheritdoc />
        public ITelemetryProcessor Create(ITelemetryProcessor next)
        {
            return (ITelemetryProcessor)ActivatorUtilities.CreateInstance(this.serviceProvider, this.telemetryProcessorType, next);
        }
    }
}

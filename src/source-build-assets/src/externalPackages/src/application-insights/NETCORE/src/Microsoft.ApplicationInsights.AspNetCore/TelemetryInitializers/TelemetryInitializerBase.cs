namespace Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers
{
    using System;

    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Base class for Telemetry Initializers. Provides access to HttpContext and RequestTelemetry.
    /// </summary>
    public abstract class TelemetryInitializerBase : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryInitializerBase" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor to provide HttpContext corresponding to telemetry items.</param>
        public TelemetryInitializerBase(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// TelemetryInitializerBase will retrieve the HttpContext and RequestTelemetry for the current ITelemetry and then invoke <see cref="OnInitializeTelemetry"/>.
        /// </summary>
        /// <param name="telemetry">Telemetry item to be enriched.</param>
        public void Initialize(ITelemetry telemetry)
        {
            var context = this.httpContextAccessor.HttpContext;

            if (context == null)
            {
                AspNetCoreEventSource.Instance.LogTelemetryInitializerBaseInitializeContextNull();
                return;
            }

            lock (context)
            {
                var request = context.Features.Get<RequestTelemetry>();

                if (request == null)
                {
                    AspNetCoreEventSource.Instance.LogTelemetryInitializerBaseInitializeRequestNull();
                    return;
                }

                this.OnInitializeTelemetry(context, request, telemetry);
            }
        }

        /// <summary>
        /// Abstract method provides HttpContext, RequestTelemetry for the given ITelemetry.
        /// </summary>
        /// <param name="platformContext">Current HttpContext.</param>
        /// <param name="requestTelemetry">Request telemetry from the context.</param>
        /// <param name="telemetry">Telemetry item to be enriched.</param>
        protected abstract void OnInitializeTelemetry(
            HttpContext platformContext,
            RequestTelemetry requestTelemetry,
            ITelemetry telemetry);
    }
}
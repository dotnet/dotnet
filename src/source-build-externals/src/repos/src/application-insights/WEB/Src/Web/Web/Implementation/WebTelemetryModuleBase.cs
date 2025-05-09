namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System.Web;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Base web telemetry module.
    /// </summary>
    public abstract class WebTelemetryModuleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebTelemetryModuleBase"/> class.
        /// </summary>
        protected WebTelemetryModuleBase()
        {
            this.ModuleName = this.GetType().ToString();
        }

        /// <summary>
        /// Gets the module name which is added to be used for internal tracing instead of GetType on each request to improve performance.
        /// </summary>
        internal string ModuleName { get; private set; }

        /// <summary>
        /// Post initialization Web Telemetry Module callback.
        /// </summary>
        /// <param name="requestTelemetry">An instance of request telemetry context.</param>
        /// <param name="platformContext">Platform specific context.</param>
        public virtual void OnBeginRequest(
            RequestTelemetry requestTelemetry,
            HttpContext platformContext)
        {
        }

        /// <summary>
        /// Request telemetry finalization - sending callback Web Telemetry Module callback.
        /// </summary>
        /// <param name="requestTelemetry">An instance of request telemetry context.</param>
        /// <param name="platformContext">Platform specific context.</param>
        public virtual void OnEndRequest(
            RequestTelemetry requestTelemetry,
            HttpContext platformContext)
        {
        }

        /// <summary>
        /// Http Error reporting Web Telemetry Module callback.
        /// </summary>
        /// <param name="requestTelemetry">An instance of request telemetry context.</param>
        /// <param name="platformContext">Platform specific context.</param>
        public virtual void OnError(
            RequestTelemetry requestTelemetry,
            HttpContext platformContext)
        {
        }
    }
}
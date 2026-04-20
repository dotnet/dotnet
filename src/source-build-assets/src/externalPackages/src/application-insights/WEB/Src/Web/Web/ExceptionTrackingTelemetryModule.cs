namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// Telemetry module to collect unhandled exceptions caught by http module.
    /// </summary>
    public class ExceptionTrackingTelemetryModule : ITelemetryModule, IDisposable
    {
        private readonly object lockObject = new object();

        private TelemetryClient telemetryClient;
        private bool initializationErrorReported;
        private bool isInitialized = false;
        private bool disposed = false;

        /// <summary>
        /// Gets or sets a value indicating whether automatic MVC 5 and WebAPI 2 exceptions tracking should be done.
        /// </summary>
        public bool EnableMvcAndWebApiExceptionAutoTracking { get; set; } = true;

        /// <summary>
        /// Implements on error callback of http module.
        /// </summary>
        public void OnError(HttpContext context)
        {
            if (this.telemetryClient == null)
            {
                if (!this.initializationErrorReported)
                {
                    this.initializationErrorReported = true;
                    WebEventSource.Log.InitializeHasNotBeenCalledOnModuleYetError();
                }
                else
                {
                    WebEventSource.Log.InitializeHasNotBeenCalledOnModuleYetVerbose();
                }

                return;
            }

            if (context == null)
            {
                WebEventSource.Log.NoHttpContextWarning();
                return;
            }

            var errors = context.AllErrors;

            if (errors != null && errors.Length > 0)
            {
                foreach (Exception exp in errors)
                {
                    var exceptionTelemetry = new ExceptionTelemetry(exp);

                    if (context.Response.StatusCode >= 500)
                    {
                        exceptionTelemetry.SeverityLevel = SeverityLevel.Critical;
                    }

                    this.telemetryClient.TrackException(exceptionTelemetry);
                }
            }
        }

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry configuration to use for initialization.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            if (this.EnableMvcAndWebApiExceptionAutoTracking)
            {
                if (!this.isInitialized)
                {
                    lock (this.lockObject)
                    {
                        if (!this.isInitialized)
                        {
                            this.telemetryClient = new TelemetryClient(configuration);
                            this.telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("web:");
                            ExceptionHandlersInjector.Inject(this.telemetryClient);
                            this.isInitialized = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        /// <param name="disposing">The method has been called directly or indirectly by a user's code.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.isInitialized = false;
                }

                this.disposed = true;
            }
        }
    }
}

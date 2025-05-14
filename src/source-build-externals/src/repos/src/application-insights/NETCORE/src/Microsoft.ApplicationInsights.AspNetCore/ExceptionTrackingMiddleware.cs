namespace Microsoft.ApplicationInsights.AspNetCore
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Sends telemetry about exceptions thrown by the application to the Microsoft Application Insights service.
    /// </summary>
    [Obsolete("Exceptions are automatically tracked with new RequestTrackingTelemetryModule")]
    public sealed class ExceptionTrackingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly TelemetryClient telemetryClient;
        private readonly string sdkVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTrackingMiddleware" /> class.
        /// </summary>
        /// <param name="next">A function that can process an HTTP request.</param>
        /// <param name="client">Send events, metrics and other telemetry to the Application Insights service.</param>
        public ExceptionTrackingMiddleware(RequestDelegate next, TelemetryClient client)
        {
            this.next = next;
            this.telemetryClient = client;
            this.sdkVersion = SdkVersionUtils.GetVersion();
        }

        /// <summary>
        /// Invoke the RequestDelegate with the HttpContext provided.
        /// </summary>
        /// <param name="httpContext">The HttpContext for the request.</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this.next.Invoke(httpContext).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                var exceptionTelemetry = new ExceptionTelemetry(exception);
                exceptionTelemetry.HandledAt = ExceptionHandledAt.Platform;
                exceptionTelemetry.Context.GetInternalContext().SdkVersion = this.sdkVersion;
                this.telemetryClient.Track(exceptionTelemetry);

                throw;
            }
        }
    }
}
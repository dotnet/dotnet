namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Extension methods for DependencyTelemetry.
    /// </summary>
    public static class DependencyTelemetryExtensions
    {
        /// <summary>
        /// The <see cref="HttpRequestMessage"/> are added to the dependency telemetry which may be useful
        /// to enhance DependencyTelemetry telemetry by <see cref="ITelemetryInitializer" /> implementations.
        /// Objects retrieved here are not automatically serialized and sent to the backend.
        /// </summary>
        /// <param name="telemetry"><see cref="DependencyCollector"/> to retrive operation detail from.</param>
        /// <param name="message">When this method returns, contains the <see cref="HttpRequestMessage"/> of the request, or the default value of the type if the operation failed.</param>
        /// <returns>true if the key was found; otherwise, false.</returns>
        public static bool TryGetHttpRequestOperationDetail(this DependencyTelemetry telemetry, out HttpRequestMessage message)
        {
            if (telemetry is null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (telemetry.TryGetOperationDetail(OperationDetailConstants.HttpRequestOperationDetailName, out var obj) && obj is HttpRequestMessage)
            {
                message = (HttpRequestMessage)obj;
                return true;
            }

            message = null;
            return false;
        }

        /// <summary>
        /// The <see cref="HttpResponseMessage"/> are added to the dependency telemetry which may be useful
        /// to enhance DependencyTelemetry telemetry by <see cref="ITelemetryInitializer" /> implementations.
        /// Objects retrieved here are not automatically serialized and sent to the backend.
        /// </summary>
        /// <param name="telemetry"><see cref="DependencyCollector"/> to retrive operation detail from.</param>
        /// <param name="message">When this method returns, contains the <see cref="HttpResponseMessage"/> of the request, or the default value of the type if the operation failed.</param>
        /// <returns>true if the key was found; otherwise, false.</returns>
        public static bool TryGetHttpResponseOperationDetail(this DependencyTelemetry telemetry, out HttpResponseMessage message)
        {
            if (telemetry is null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (telemetry.TryGetOperationDetail(OperationDetailConstants.HttpResponseOperationDetailName, out var obj) && obj is HttpResponseMessage)
            {
                message = (HttpResponseMessage)obj;
                return true;
            }

            message = null;
            return false;
        }

        /// <summary>
        /// The <see cref="WebHeaderCollection "/> are added to the dependency telemetry which may be useful
        /// to enhance DependencyTelemetry telemetry by <see cref="ITelemetryInitializer" /> implementations.
        /// Objects retrieved here are not automatically serialized and sent to the backend.
        /// </summary>
        /// <param name="telemetry"><see cref="DependencyCollector"/> to retrive operation detail from.</param>
        /// <param name="headers">When this method returns, contains the <see cref="WebHeaderCollection "/> of the request, or the default value of the type if the operation failed.</param>
        /// <returns>true if the key was found; otherwise, false.</returns>
        public static bool TryGetHttpResponseHeadersOperationDetail(this DependencyTelemetry telemetry, out WebHeaderCollection headers)
        {
            if (telemetry is null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (telemetry.TryGetOperationDetail(OperationDetailConstants.HttpResponseHeadersOperationDetailName, out var obj) && obj is WebHeaderCollection)
            {
                headers = (WebHeaderCollection)obj;
                return true;
            }

            headers = null;
            return false;
        }
    }
}

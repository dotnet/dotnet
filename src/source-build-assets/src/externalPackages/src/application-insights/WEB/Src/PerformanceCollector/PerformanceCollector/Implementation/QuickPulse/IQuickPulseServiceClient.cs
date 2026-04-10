namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;

    internal interface IQuickPulseServiceClient : IDisposable
    {
        /// <summary>
        /// Gets the current QPS URI.
        /// </summary>
        /// <remarks>This value may be dynamically updated with each request from within the instance.</remarks>
        Uri CurrentServiceUri { get; }

        /// <summary>
        /// Pings QPS to check if it expects data right now.
        /// </summary>
        /// <param name="instrumentationKey">InstrumentationKey for which to submit data samples.</param>
        /// <param name="timestamp">Timestamp to pass to the server.</param>
        /// <param name="configurationETag">Current configuration ETag that the client has.</param>
        /// <param name="authApiKey">Authentication API key.</param>
        /// <param name="authToken">Authorization token to be included on Http messages.</param>
        /// <param name="configurationInfo">When available, the deserialized response data received from the server.</param>
        /// <param name="servicePollingIntervalHint">When available, a hint regarding what the period should be when pinging the server going forward.</param>
        /// <returns><b>true</b> if data is expected, otherwise <b>false</b>.</returns>
        bool? Ping(
            string instrumentationKey,
            DateTimeOffset timestamp,
            string configurationETag,
            string authApiKey,
            string authToken,
            out CollectionConfigurationInfo configurationInfo,
            out TimeSpan? servicePollingIntervalHint);

        /// <summary>
        /// Submits a data samples to QPS.
        /// </summary>
        /// <param name="samples">Data samples.</param>
        /// <param name="instrumentationKey">InstrumentationKey for which to submit data samples.</param>
        /// <param name="configurationETag">Current configuration ETag that the client has.</param>
        /// <param name="authApiKey">Authentication API key.</param>
        /// <param name="authToken">Authorization token to be included on Http messages.</param>
        /// <param name="configurationInfo">When available, the deserialized response data received from the server.</param>
        /// <param name="collectionConfigurationErrors">Errors to be reported back to the server.</param>
        /// <returns><b>true</b> if the client is expected to keep sending data samples, <b>false</b> otherwise.</returns>
        bool? SubmitSamples(
            IEnumerable<QuickPulseDataSample> samples,
            string instrumentationKey,
            string configurationETag,
            string authApiKey,
            string authToken,
            out CollectionConfigurationInfo configurationInfo,
            CollectionConfigurationError[] collectionConfigurationErrors);
    }
}

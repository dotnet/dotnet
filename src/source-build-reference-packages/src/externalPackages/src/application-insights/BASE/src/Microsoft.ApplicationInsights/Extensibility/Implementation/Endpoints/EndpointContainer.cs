namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Endpoints
{
    using System;

    /// <summary>
    /// This class encapsulates the endpoint values.
    /// </summary>
    public class EndpointContainer
    {
        internal EndpointContainer(IEndpointProvider endpointProvider)
        {
            this.Ingestion = endpointProvider.GetEndpoint(EndpointName.Ingestion);
            this.Live = endpointProvider.GetEndpoint(EndpointName.Live);
            this.Profiler = endpointProvider.GetEndpoint(EndpointName.Profiler);
            this.Snapshot = endpointProvider.GetEndpoint(EndpointName.Snapshot);
        }

        /// <summary>Gets the endpoint for the Ingestion (aka Breeze) service.</summary>
        public Uri Ingestion { get; private set; }

        /// <summary>Gets the endpoint for Live Metrics (aka QuickPulse) service.</summary>
        public Uri Live { get; private set; }

        /// <summary>Gets the endpoint for the Profiler service.</summary>
        public Uri Profiler { get; private set; }

        /// <summary>Gets the endpoint for the Snapshot service.</summary>
        public Uri Snapshot { get; private set; }

        /// <summary>Gets the fully formatted endpoint for the ingestion service.</summary>
        internal string FormattedIngestionEndpoint => new Uri(this.Ingestion, "v2/track").AbsoluteUri;

        /// <summary>Gets the fully formatted endpoint for the ingestion service with support for AAD.</summary>
        internal string FormattedIngestionAADEndpoint => new Uri(this.Ingestion, "v2.1/track").AbsoluteUri;

        /// <summary>Gets the fully formatted endpoint for the application id profile service.</summary>
        /// <remarks>This returns a string without using the Uri for validation because the consuming method needs to do a string replace.</remarks>
        internal string FormattedApplicationIdEndpoint => this.Ingestion.AbsoluteUri + "api/profiles/{0}/appId";

        /// <summary>
        /// Get the Ingestion Endpoint, depending on if AAD is in use.
        /// This can be removed after we fully transition no the newer Ingestion API.
        /// </summary>
        /// <param name="enableAAD">Boolean to indicate which ingestion service to use.</param>
        /// <returns>Fully formatted endpoint for the ingestion service.</returns>
        internal string GetFormattedIngestionEndpoint(bool enableAAD) => enableAAD ? this.FormattedIngestionAADEndpoint : this.FormattedIngestionEndpoint;
    }
}

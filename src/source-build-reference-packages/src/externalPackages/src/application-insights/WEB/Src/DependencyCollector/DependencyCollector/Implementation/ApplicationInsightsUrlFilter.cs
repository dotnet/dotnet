namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Extensibility;

    internal sealed class ApplicationInsightsUrlFilter
    {
        internal const string TelemetryServiceEndpoint = "https://dc.services.visualstudio.com/v2/track";
        internal const string QuickPulseServiceEndpoint = "https://rt.services.visualstudio.com/QuickPulseService.svc";

        internal readonly Uri TelemetryServiceEndpointUri = new Uri(TelemetryServiceEndpoint);
        private readonly TelemetryConfiguration telemetryConfiguration;

        private KeyValuePair<string, string> cachedEndpointLeftPart;

        public ApplicationInsightsUrlFilter(TelemetryConfiguration telemetryConfiguration)
        {
            this.telemetryConfiguration = telemetryConfiguration;

            // cache EndpointLeftPart ahead, before first dependency call
            this.cachedEndpointLeftPart = this.GetEndpointLeftPart();
        }

        private string EndpointLeftPart
        {
            get
            {
                string currentEndpointAddressValue = null;

                // Cache AI endpoint URI
                if (this.telemetryConfiguration != null && this.telemetryConfiguration.TelemetryChannel != null)
                {
                    string endpoint = this.telemetryConfiguration.TelemetryChannel.EndpointAddress;
                    if (!string.IsNullOrEmpty(endpoint))
                    {
                        // The TelemetryChannel is used which defines the production endpoint in ApplicationInsights.config.
                        currentEndpointAddressValue = endpoint;
                    }
                }

                if (this.cachedEndpointLeftPart.Key != currentEndpointAddressValue)
                {
                    this.cachedEndpointLeftPart = this.GetEndpointLeftPart(currentEndpointAddressValue);
                }

                return this.cachedEndpointLeftPart.Value;
            }
        }

        /// <summary>
        /// Determines whether an URL is application insights URL.
        /// </summary>
        /// <param name="url">HTTP URL.</param>
        /// <returns>True if URL is application insights url, otherwise false.</returns>
        internal bool IsApplicationInsightsUrl(Uri url)
        {
            // first check that it's not active internal SDK operation 
            if (SdkInternalOperationsMonitor.IsEntered())
            {
                return true;
            }

            return this.IsApplicationInsightsUrlImpl(url?.ToString());
        }

        /// <summary>
        /// Determines whether an URL is application insights URL.
        /// </summary>
        /// <param name="url">HTTP URL.</param>
        /// <returns>True if URL is application insights url, otherwise false.</returns>
        internal bool IsApplicationInsightsUrl(string url)
        {
            // first check that it's not active internal SDK operation 
            if (SdkInternalOperationsMonitor.IsEntered())
            {
                return true;
            }

            return this.IsApplicationInsightsUrlImpl(url);
        }

        private bool IsApplicationInsightsUrlImpl(string url)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(url))
            {
                // Check if url matches default values for service endpoint/quick pulse.
                result = url.StartsWith(ApplicationInsightsUrlFilter.TelemetryServiceEndpoint, StringComparison.OrdinalIgnoreCase)
                         || url.StartsWith(ApplicationInsightsUrlFilter.QuickPulseServiceEndpoint, StringComparison.OrdinalIgnoreCase);

                if (!result)
                {
                    // Check if the url is a user-configured service endpoint.
                    var endpointUrl = this.EndpointLeftPart;
                    if (!string.IsNullOrEmpty(endpointUrl))
                    {
                        result = url.StartsWith(endpointUrl, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }

            return result;
        }

        private KeyValuePair<string, string> GetEndpointLeftPart(string currentEndpointAddressValue = null)
        {
            var uri = currentEndpointAddressValue != null ? new Uri(currentEndpointAddressValue) : this.TelemetryServiceEndpointUri;

            // we are using Authority to include the port number
            // if it is not the same as the default port of the Uri.
            // especially required for Functional Tests which host applciation
            // and telemetry service at the same host localhost but are using different ports.
            var endpointLeftPart = uri.Scheme + "://" + uri.Authority;

            return new KeyValuePair<string, string>(currentEndpointAddressValue, endpointLeftPart);
        }
    }
}

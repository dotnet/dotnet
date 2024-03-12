namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility;

    internal class ProfileServiceWrapper : IDisposable
    {
        internal readonly FailedRequestsManager FailedRequestsManager;

        private readonly HttpClient httpClient = new HttpClient();

        internal ProfileServiceWrapper()
        {
            this.FailedRequestsManager = new FailedRequestsManager();
        }

        internal ProfileServiceWrapper(TimeSpan failedRequestRetryWaitTime)
        {
            this.FailedRequestsManager = new FailedRequestsManager(failedRequestRetryWaitTime);
        }

        public string ProfileQueryEndpoint { get; set; }

        public async Task<string> FetchApplicationIdAsync(string instrumentationKey)
        {
            if (this.FailedRequestsManager.CanRetry(instrumentationKey))
            {
                try
                {
                    return await this.SendRequestAsync(instrumentationKey).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    this.FailedRequestsManager.RegisterFetchFailure(instrumentationKey, ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.httpClient.Dispose();
            }
        }

        /// <summary>Send HttpRequest to get config id.</summary>
        /// <remarks>This method is internal virtual so it can be moq-ed in a unit test.</remarks>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Implementation expects lower case. GUID is safe for lower case.")]
        internal virtual Task<HttpResponseMessage> GetAsync(string instrumentationKey)
        {
            Uri applicationIdEndpoint = this.GetApplicationIdEndPointUri(instrumentationKey.ToLowerInvariant());
            return this.httpClient.GetAsync(applicationIdEndpoint);
        }

        /// <summary>
        /// Strips off any relative path at the end of the base URI and then appends the known relative path to get the Application Id uri.
        /// </summary>
        /// <param name="instrumentationKey">AI resource's Instrumentation Key.</param>
        /// <returns>Computed Uri.</returns>
        internal Uri GetApplicationIdEndPointUri(string instrumentationKey)
        {
            return new Uri(string.Format(CultureInfo.InvariantCulture, this.ProfileQueryEndpoint ?? Constants.ProfileQueryEndpoint, instrumentationKey));
        }

        /// <summary>
        /// Retrieves the Application Id given the Instrumentation Key.
        /// </summary>
        /// <param name="instrumentationKey">Instrumentation key for which Application Id is to be retrieved.</param>
        /// <returns>Task to resolve Application Id.</returns>
        private async Task<string> SendRequestAsync(string instrumentationKey)
        {
            try
            {
                SdkInternalOperationsMonitor.Enter();

                var resultMessage = await this.GetAsync(instrumentationKey).ConfigureAwait(false);
                if (resultMessage.IsSuccessStatusCode)
                {
                    return await resultMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                else
                {
                    this.FailedRequestsManager.RegisterFetchFailure(instrumentationKey, resultMessage.StatusCode);
                    return null;
                }
            }
            finally
            {
                SdkInternalOperationsMonitor.Exit();
            }
        }
    }
}

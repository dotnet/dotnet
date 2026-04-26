namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// This <see cref="ApplicationInsightsApplicationIdProvider"/> will query the Application Insights' Breeze endpoint to lookup an Application Id based on Instrumentation Key.
    /// This will cache lookup results to prevent repeat queries.
    /// This will rely on the <see cref="ProfileServiceWrapper" /> and <see cref="FailedRequestsManager" /> to record failed requests and block additional failing requests.
    /// </summary>
    public sealed class ApplicationInsightsApplicationIdProvider : IApplicationIdProvider, IDisposable
    {
        internal ConcurrentDictionary<string, bool> FetchTasks = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// Max number of Application Ids to cache.
        /// </summary>
        private const int MAXSIZE = 100;

        private readonly ProfileServiceWrapper applicationIdProvider;

        private ConcurrentDictionary<string, string> knownApplicationIds = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Initialize a new instance of the <see cref="ApplicationInsightsApplicationIdProvider"/> class.
        /// </summary>
        public ApplicationInsightsApplicationIdProvider()
        {
            this.applicationIdProvider = new ProfileServiceWrapper();
        }

        /// <summary>
        /// Unit Test Only! Initializes a new instance of the <see cref="ApplicationInsightsApplicationIdProvider" /> class and accepts mocks for fetching Application Id.
        /// </summary>
        internal ApplicationInsightsApplicationIdProvider(ProfileServiceWrapper profileServiceWrapper)
        {
            this.applicationIdProvider = profileServiceWrapper;
        }

        /// <summary>
        /// Gets or sets the endpoint that is to be used to get the Application Insights resource's profile (Application Id etc.). 
        /// Default value is "https://dc.services.visualstudio.com/api/profiles/{0}/appId". If this is overwritten, MUST include the '{0}' for string replacement!.
        /// </summary>
        public string ProfileQueryEndpoint
        {
            get { return this.applicationIdProvider.ProfileQueryEndpoint; }
            set { this.applicationIdProvider.ProfileQueryEndpoint = value; }
        }

        /// <summary>
        /// Disposes resources.
        /// </summary>
        public void Dispose()
        {
            this.applicationIdProvider.Dispose();
        }

        /// <summary>
        /// Retrieves the Application Id corresponding to a given Instrumentation Key.
        /// </summary>
        /// <param name="instrumentationKey">Instrumentation Key string.</param>
        /// <param name="applicationId">Application Id corresponding to the provided Instrumentation Key. Returns NULL if a match was not found.</param>
        /// <returns>TRUE if Application Id was successfully retrieved, FALSE otherwise.</returns>
        public bool TryGetApplicationId(string instrumentationKey, out string applicationId)
        {
            applicationId = null;

            if (string.IsNullOrEmpty(instrumentationKey))
            {
                return false;
            }
            else if (this.knownApplicationIds.TryGetValue(instrumentationKey, out applicationId))
            {
                return true;
            }
            else
            {
                this.FetchApplicationId(instrumentationKey);
                return false;
            }
        }

        /// <summary>
        /// Unit Test Only! Informs tests to wait for a fetch task to complete.
        /// </summary>
        /// <returns>TRUE if fetch task is still in progress, FALSE otherwise.</returns>
        internal bool IsFetchAppInProgress(string instrumentationKey)
        {
            return this.FetchTasks.ContainsKey(instrumentationKey);
        }

        private void FetchApplicationId(string instrumentationKey)
        {
            if (this.FetchTasks.TryAdd(instrumentationKey, true))
            {
                // Simplistic cleanup to guard against this becoming a memory hog.
                if (this.knownApplicationIds.Keys.Count >= MAXSIZE)
                {
                    this.knownApplicationIds.Clear();
                }

                // add this task to the thread pool. 
                // We don't care when it finishes, but we don't want to block the thread.
                Task.Run(() => this.applicationIdProvider.FetchApplicationIdAsync(instrumentationKey))
                    .ContinueWith((applicationIdTask) =>
                    {
                        this.FormatAndAddToDictionary(instrumentationKey, applicationIdTask.Result);

                        this.FetchTasks.TryRemove(instrumentationKey, out bool ignoreValue);
                    })
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Format and store an Instrumentation Key and Application Id pair into the dictionary of known Application Ids.
        /// </summary>
        /// <param name="instrumentationKey">Instrumentation Key is expected to be a Guid string.</param>
        /// <param name="applicationId">Application Id is expected to be a Guid string. </param>
        private void FormatAndAddToDictionary(string instrumentationKey, string applicationId)
        {
            if (!string.IsNullOrEmpty(instrumentationKey) && !string.IsNullOrEmpty(applicationId))
            {
                this.knownApplicationIds.TryAdd(instrumentationKey, ApplicationIdHelper.ApplyFormatting(applicationId));
            }
        }
    }
}

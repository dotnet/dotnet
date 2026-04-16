namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Authentication;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;

    internal class QuickPulseCollectionStateManager
    {
        private readonly IQuickPulseServiceClient serviceClient;

        private readonly Clock timeProvider;

        private readonly QuickPulseTimings timings;

        private readonly Action onStartCollection;

        private readonly Action onStopCollection;

        private readonly Func<IList<QuickPulseDataSample>> onSubmitSamples;

        private readonly Action<IList<QuickPulseDataSample>> onReturnFailedSamples;

        private readonly Func<CollectionConfigurationInfo, CollectionConfigurationError[]> onUpdatedConfiguration;

        private readonly Action<Uri> onUpdatedServiceEndpoint;

        private readonly TimeSpan coolDownTimeout;

        private readonly List<CollectionConfigurationError> collectionConfigurationErrors = new List<CollectionConfigurationError>();

        private readonly TelemetryConfiguration telemetryConfiguration;

        private DateTimeOffset lastSuccessfulPing;

        private DateTimeOffset lastSuccessfulSubmit;

        private bool isCollectingData;

        private bool firstStateUpdate = true;

        private string currentConfigurationETag = string.Empty;

        private TimeSpan? latestServicePollingIntervalHint = null;

        public QuickPulseCollectionStateManager(
            TelemetryConfiguration telemetryConfiguration,
            IQuickPulseServiceClient serviceClient,
            Clock timeProvider,
            QuickPulseTimings timings,
            Action onStartCollection,
            Action onStopCollection,
            Func<IList<QuickPulseDataSample>> onSubmitSamples,
            Action<IList<QuickPulseDataSample>> onReturnFailedSamples,
            Func<CollectionConfigurationInfo, CollectionConfigurationError[]> onUpdatedConfiguration,
            Action<Uri> onUpdatedServiceEndpoint)
        {
            this.telemetryConfiguration = telemetryConfiguration ?? throw new ArgumentNullException(nameof(telemetryConfiguration));
            this.serviceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
            this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            this.timings = timings ?? throw new ArgumentNullException(nameof(timings));
            this.onStartCollection = onStartCollection ?? throw new ArgumentNullException(nameof(onStartCollection));
            this.onStopCollection = onStopCollection ?? throw new ArgumentNullException(nameof(onStopCollection));
            this.onSubmitSamples = onSubmitSamples ?? throw new ArgumentNullException(nameof(onSubmitSamples));
            this.onReturnFailedSamples = onReturnFailedSamples ?? throw new ArgumentNullException(nameof(onReturnFailedSamples));
            this.onUpdatedConfiguration = onUpdatedConfiguration ?? throw new ArgumentNullException(nameof(onUpdatedConfiguration));
            this.onUpdatedServiceEndpoint = onUpdatedServiceEndpoint ?? throw new ArgumentNullException(nameof(onUpdatedServiceEndpoint));

            this.coolDownTimeout = TimeSpan.FromMilliseconds(timings.CollectionInterval.TotalMilliseconds / 20);
        }

        public bool IsCollectingData
        {
            get
            {
                return this.isCollectingData;
            }

            private set
            {
                if (value != this.isCollectingData)
                {
                    // state transition
                    this.ResetLastSuccessful();
                }

                this.isCollectingData = value;
            }
        }

        public TimeSpan UpdateState(string instrumentationKey, string authApiKey)
        {
            if (string.IsNullOrWhiteSpace(instrumentationKey))
            {
                return this.timings.ServicePollingInterval;
            }

            if (this.firstStateUpdate)
            {
                this.ResetLastSuccessful();

                this.firstStateUpdate = false;
            }

            AuthToken authToken = default;
            if (this.telemetryConfiguration.CredentialEnvelope != null)
            {
                authToken = this.telemetryConfiguration.CredentialEnvelope.GetToken();
                if (authToken == default)
                {
                    // If a credential has been set on the configuration and we fail to get a token, do net send.
                    QuickPulseEventSource.Log.FailedToGetAuthToken();
                    return this.DetermineBackOffs();
                }
            }

            CollectionConfigurationInfo configurationInfo;
            if (this.IsCollectingData)
            {
                // we are currently collecting
                IList<QuickPulseDataSample> dataSamplesToSubmit = this.onSubmitSamples();

                if (!dataSamplesToSubmit.Any())
                {
                    // no samples to submit, do nothing
                    return this.DetermineBackOffs();
                }
                else
                {
                    // we have samples
                    if (dataSamplesToSubmit.Any(sample => sample.CollectionConfigurationAccumulator.GetRef() != 0))
                    {
                        // some samples are still being processed, wait a little to give them a chance to finish
                        Task.Delay(this.coolDownTimeout).GetAwaiter().GetResult();

                        bool allCooledDown =
                            dataSamplesToSubmit.All(sample => sample.CollectionConfigurationAccumulator.GetRef() == 0);

                        QuickPulseEventSource.Log.CollectionConfigurationSampleCooldownEvent(allCooledDown);
                    }
                }

                bool? keepCollecting = this.serviceClient.SubmitSamples(
                    dataSamplesToSubmit,
                    instrumentationKey,
                    this.currentConfigurationETag,
                    authApiKey,
                    authToken.Token,
                    out configurationInfo,
                    this.collectionConfigurationErrors.ToArray());

                QuickPulseEventSource.Log.SampleSubmittedEvent(this.currentConfigurationETag, configurationInfo?.ETag, keepCollecting.ToString());

                switch (keepCollecting)
                {
                    case null:
                        // the request has failed, so we need to return the samples back to the submitter
                        this.onReturnFailedSamples(dataSamplesToSubmit);
                        break;

                    case true:
                        // the service wants us to keep collecting
                        this.UpdateConfiguration(configurationInfo);
                        break;

                    case false:
                        // the service wants us to stop collection
                        this.onStopCollection();
                        break;
                }

                this.lastSuccessfulSubmit = keepCollecting.HasValue ? this.timeProvider.UtcNow : this.lastSuccessfulSubmit;
                this.IsCollectingData = keepCollecting ?? this.IsCollectingData;
            }
            else
            {
                // we are currently idle and pinging the service waiting for it to ask us to start collecting data
                bool? startCollection = this.serviceClient.Ping(
                    instrumentationKey,
                    this.timeProvider.UtcNow,
                    this.currentConfigurationETag,
                    authApiKey,
                    authToken.Token,
                    out configurationInfo,
                    out TimeSpan? servicePollingIntervalHint);

                this.latestServicePollingIntervalHint = servicePollingIntervalHint ?? this.latestServicePollingIntervalHint;

                QuickPulseEventSource.Log.PingSentEvent(this.currentConfigurationETag, configurationInfo?.ETag, startCollection.ToString());

                switch (startCollection)
                {
                    case null:
                        // the request has failed
                        break;

                    case true:
                        // the service wants us to start collection now
                        this.UpdateConfiguration(configurationInfo);
                        this.onStartCollection();
                        break;

                    case false:
                        // the service wants us to remain idle and keep pinging
                        break;
                }

                this.lastSuccessfulPing = startCollection.HasValue ? this.timeProvider.UtcNow : this.lastSuccessfulPing;
                this.IsCollectingData = startCollection ?? this.IsCollectingData;
            }

            this.onUpdatedServiceEndpoint?.Invoke(this.serviceClient.CurrentServiceUri);

            return this.DetermineBackOffs();
        }

        private void UpdateConfiguration(CollectionConfigurationInfo configurationInfo)
        {
            // we only get here if Etag in the header is different from the current one, but we still want to check if Etag in the body is also different
            if (configurationInfo != null && !string.Equals(configurationInfo.ETag, this.currentConfigurationETag, StringComparison.Ordinal))
            {
                QuickPulseEventSource.Log.CollectionConfigurationUpdating(this.currentConfigurationETag, configurationInfo.ETag, string.Empty);

                this.collectionConfigurationErrors.Clear();

                CollectionConfigurationError[] errors = null;
                try
                {
                    errors = this.onUpdatedConfiguration?.Invoke(configurationInfo);
                }
                catch (Exception e)
                {
                    QuickPulseEventSource.Log.CollectionConfigurationUpdateFailed(this.currentConfigurationETag, configurationInfo.ETag, e.ToInvariantString(), string.Empty);

                    this.collectionConfigurationErrors.Add(
                        CollectionConfigurationError.CreateError(
                            CollectionConfigurationErrorType.CollectionConfigurationFailureToCreateUnexpected,
                            string.Format(CultureInfo.InvariantCulture, "Unexpected error applying configuration. ETag: {0}", configurationInfo.ETag ?? string.Empty),
                            e,
                            Tuple.Create("ETag", configurationInfo.ETag)));
                }

                if (errors != null)
                {
                    this.collectionConfigurationErrors.AddRange(errors);
                }

                this.currentConfigurationETag = configurationInfo.ETag;
            }
        }

        private TimeSpan DetermineBackOffs()
        {
            if (this.IsCollectingData)
            {
                TimeSpan timeSinceLastSuccessfulSubmit = this.timeProvider.UtcNow - this.lastSuccessfulSubmit;
                if (timeSinceLastSuccessfulSubmit < this.timings.TimeToCollectionBackOff)
                {
                    return this.timings.CollectionInterval;
                }

                QuickPulseEventSource.Log.TroubleshootingMessageEvent("Collection is failing. Back off.");

                // we have been failing to send samples for a while
                this.onStopCollection();
                this.IsCollectingData = false;

                // we're going back to idling and pinging, but we need to back off immediately
                this.lastSuccessfulPing = DateTimeOffset.MinValue;

                return this.timings.ServicePollingBackedOffInterval;
            }
            else
            {
                TimeSpan timeSinceLastSuccessfulPing = this.timeProvider.UtcNow - this.lastSuccessfulPing;

                return timeSinceLastSuccessfulPing < this.timings.TimeToServicePollingBackOff
                           ? this.latestServicePollingIntervalHint ?? this.timings.ServicePollingInterval
                           : this.timings.ServicePollingBackedOffInterval;
            }
        }

        private void ResetLastSuccessful()
        {
            this.lastSuccessfulPing = this.timeProvider.UtcNow;
            this.lastSuccessfulSubmit = this.timeProvider.UtcNow;
        }
    }
}

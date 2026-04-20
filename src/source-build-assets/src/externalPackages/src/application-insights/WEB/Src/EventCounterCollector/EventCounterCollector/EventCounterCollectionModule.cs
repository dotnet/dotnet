namespace Microsoft.ApplicationInsights.Extensibility.EventCounterCollector
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    /// <summary>
    /// Telemetry module for collecting EventCounters.
    /// https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.Tracing/documentation/EventCounterTutorial.md.
    /// </summary>
    public class EventCounterCollectionModule : ITelemetryModule, IDisposable
    {
        // 60 sec is hardcoded and not allowed for user customization except unit tests as backend expects 1 min aggregation.
        // TODO: Need to revisit if this should be changed.               
        private readonly int refreshInternalInSecs = 60;

        /// <summary>
        /// TelemetryClient used to send data.
        /// </summary>        
        private TelemetryClient client = null;        
        private EventCounterListener eventCounterListener;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCounterCollectionModule"/> class.
        /// </summary>
        public EventCounterCollectionModule()
        {
            this.Counters = new List<EventCounterCollectionRequest>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCounterCollectionModule"/> class.
        /// </summary>
        internal EventCounterCollectionModule(int refreshIntervalInSecs) : this()
        {
            this.refreshInternalInSecs = refreshIntervalInSecs;
        }

        /// <summary>
        /// Gets the list of counter names to collect. Each should have the name of EventSource publishing the counter, and counter name.
        /// </summary>
        public IList<EventCounterCollectionRequest> Counters { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the eventsource name and metric name are stored separately.
        /// </summary>
        public bool UseEventSourceNameAsMetricsNamespace { get; set; }

        /// <summary>Gets a value indicating whether this module has been initialized.</summary>
        internal bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }        

        /// <summary>
        /// Initialize method is called after all configuration properties have been loaded from the configuration.
        /// </summary>
        public void Initialize(TelemetryConfiguration configuration)
        {
            try
            {                
                if (!this.IsInitialized)
                {
                    EventCounterCollectorEventSource.Log.ModuleIsBeingInitializedEvent(this.Counters?.Count ?? 0);

                    if (this.Counters.Count <= 0)
                    {
                        EventCounterCollectorEventSource.Log.EventCounterCollectorNoCounterConfigured();
                    }

                    this.client = new TelemetryClient(configuration);                    
                    this.client.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("evtc:");
                    this.eventCounterListener = new EventCounterListener(this.client, this.Counters, this.refreshInternalInSecs, this.UseEventSourceNameAsMetricsNamespace);
                    this.IsInitialized = true;
                    EventCounterCollectorEventSource.Log.ModuleInitializedSuccess();
                }
            }
            catch (Exception ex)
            {
                EventCounterCollectorEventSource.Log.EventCounterCollectorError("Initialization", ex.Message);
            }                        
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
                    if (this.eventCounterListener != null)
                    {
                        this.eventCounterListener.Dispose();
                    }
                }

                this.disposed = true;
            }
        }
    }
}
namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.StandardPerfCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.WebAppPerfCollector;

    using Timer = Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.Timer.Timer;

    /// <summary>
    /// Telemetry module for collecting performance counters.
    /// </summary>
    public sealed class PerformanceCollectorModule : ITelemetryModule, IDisposable
    {
        private readonly object lockObject = new object();

        private readonly List<PerformanceCounterCollectionRequest> defaultCounters = new List<PerformanceCounterCollectionRequest>();

        private readonly IPerformanceCollector collector;

        /// <summary>
        /// Determines how often we re-register performance counters.
        /// </summary>
        /// <remarks>Re-registration is needed because IIS starts reporting on different counter instances depending
        /// on worker processes starting and termination.</remarks>
        private readonly TimeSpan registrationPeriod = TimeSpan.FromMinutes(5);

        /// <summary>
        /// The idea behind this flag is the following:
        /// - If customer will never set any counters to the list of default counters - default counters will be used
        /// - If customer added a counter to the list - default counters will not be populated
        /// - If customer accessed the collection of set empty collection in config - default counters will not be populated
        /// All this complicated logic is for the backward compatibility reasons only.
        /// </summary>
        private bool defaultCountersInitialized = false;

        /// <summary>
        /// Determines how often collection takes place.
        /// </summary>
        private TimeSpan collectionPeriod = TimeSpan.FromSeconds(60);

        /// <summary>
        /// The timestamp of last performance counter registration.
        /// </summary>
        private DateTime lastRefreshTimestamp = DateTime.MinValue;

        /// <summary>
        /// Communication sink for performance data.
        /// </summary>
        /// <remarks>
        /// TelemetryContext is thread-safe.
        /// </remarks>
        private TelemetryClient client = null;

        // Used in unit tests only
        private TelemetryConfiguration telemetryConfiguration;

        /// <summary>
        /// Timer to schedule performance collection.
        /// </summary>
        private Timer timer = null;

        /// <summary>Gets a value indicating whether this module has been initialized.</summary>
        internal bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCollectorModule"/> class.
        /// </summary>
        public PerformanceCollectorModule()
        {
            this.Counters = new List<PerformanceCounterCollectionRequest>();

            this.collector = PerformanceCounterUtility.GetPerformanceCollector();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceCollectorModule"/> class.
        /// </summary>
        /// <remarks>Unit tests only.</remarks>
        internal PerformanceCollectorModule(IPerformanceCollector collectorMock) : this()
        {
            this.collector = collectorMock;
        }

        /// <summary>
        /// Gets a list of counters to collect.
        /// </summary>
        /// <remarks>Loaded from configuration.</remarks>
        public IList<PerformanceCounterCollectionRequest> Counters { get; private set; }

        /// <summary>
        /// Gets a list of default counters to collect.
        /// </summary>
        /// <remarks>Loaded from configuration.</remarks>
        public IList<PerformanceCounterCollectionRequest> DefaultCounters
        {
            get
            {
                this.defaultCountersInitialized = true;
                return this.defaultCounters;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether performance counters should be collected under IIS Express.
        /// </summary>
        /// <remarks>Loaded from configuration.</remarks>
        public bool EnableIISExpressPerformanceCounters { get; set; }

        internal TimeSpan CollectionPeriod
        {
            get
            {
                return this.collectionPeriod;
            }

            set
            {
                this.collectionPeriod = value;
            }
        }

        /// <summary>
        /// Initialize method is called after all configuration properties have been loaded from the configuration.
        /// </summary>
        public void Initialize(TelemetryConfiguration configuration)
        {
            if (!this.IsInitialized)
            {
                lock (this.lockObject)
                {
                    if (!this.IsInitialized)
                    {
                        PerformanceCollectorEventSource.Log.ModuleIsBeingInitializedEvent(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "Custom counters count: '{0}'",
                                Counters?.Count ?? 0));

                        if (configuration == null)
                        {
                            throw new ArgumentNullException(nameof(configuration));
                        }

                        if (!this.defaultCountersInitialized)
                        {
                            // The following are the counters support in all cases.
                            this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\Process(??APP_WIN32_PROC??)\% Processor Time", @"\Process(??APP_WIN32_PROC??)\% Processor Time"));
                            this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized", @"\Process(??APP_WIN32_PROC??)\% Processor Time Normalized"));
                            this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\Process(??APP_WIN32_PROC??)\Private Bytes", @"\Process(??APP_WIN32_PROC??)\Private Bytes"));

#if NET452         // The following are Asp.Net specific counters.
                            this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests/Sec", @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests/Sec"));
                            this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec", @"\.NET CLR Exceptions(??APP_CLR_PROC??)\# of Exceps Thrown / sec"));
                            this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time", @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Request Execution Time"));
                            this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue", @"\ASP.NET Applications(??APP_W3SVC_PROC??)\Requests In Application Queue"));
#endif

                            if (this.collector.GetType().Name.Equals("WebAppPerformanceCollector", StringComparison.OrdinalIgnoreCase) || this.collector.GetType().Name.Equals("StandardPerformanceCollector", StringComparison.OrdinalIgnoreCase))
                            {
                                // The systemwide Memory counter is enabled in WebApps.
                                this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\Memory\Available Bytes", @"\Memory\Available Bytes"));
                                this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\Process(??APP_WIN32_PROC??)\IO Data Bytes/sec", @"\Process(??APP_WIN32_PROC??)\IO Data Bytes/sec"));
                            }                                                      
                                                        
                            if (this.collector.GetType().Name.Equals("StandardPerformanceCollector", StringComparison.OrdinalIgnoreCase))
                            {
                                // Only time total CPU counter is available is if we are using StandardPerformanceCollector.
                                this.DefaultCounters.Add(new PerformanceCounterCollectionRequest(@"\Processor(_Total)\% Processor Time", @"\Processor(_Total)\% Processor Time"));
                            }
                        }

                        if (!this.EnableIISExpressPerformanceCounters && IsRunningUnderIisExpress())
                        {
                            PerformanceCollectorEventSource.Log.RunningUnderIisExpress();
                            return;
                        }

                        this.telemetryConfiguration = configuration;
                        this.client = new TelemetryClient(configuration);
                        this.client.Context.GetInternalContext().SdkVersion = Common.SdkVersionUtils.GetSdkVersion(PerformanceCounterUtility.SDKVersionPrefix());

                        this.lastRefreshTimestamp = DateTime.MinValue;

                        this.timer = new Timer(this.TimerCallback);

                        // schedule the first tick
                        this.timer.ScheduleNextTick(this.collectionPeriod);
                        this.IsInitialized = true;
                    }
                }
            }
        }
        
        /// <summary>
        /// Disposes resources allocated by this type.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static bool IsRunningUnderIisExpress()
        {
            var iisExpressProcessName = "iisexpress";

            try
            {
                return Process.GetCurrentProcess().ProcessName.IndexOf(iisExpressProcessName, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch (Exception e)
            {
                // we are unable to determine if we're running under IIS Express, assume we are not
                PerformanceCollectorEventSource.Log.UnknownErrorEvent(
                    string.Format(CultureInfo.InvariantCulture, "Unable to get process name. {0}", e.ToInvariantString()));

                return false;
            }
        }

        /// <summary>
        /// Dispose implementation.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.timer != null)
                {
                    this.timer.Dispose();
                    this.timer = null;
                }

                if (this.collector != null && this.collector is IDisposable)
                {
                    ((IDisposable)this.collector).Dispose();
                }
            }
        }

        private void TimerCallback(object state)
        {
            try
            {
                SdkInternalOperationsMonitor.Enter();

                PerformanceCollectorEventSource.Log.CounterCollectionAttemptEvent();

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                this.EnsurePerformanceCountersRegistered();

                var results =
                    this.collector.Collect(
                        (counterName, e) =>
                        PerformanceCollectorEventSource.Log.CounterReadingFailedEvent(e.ToString(), counterName))
                        .ToList();

                stopwatch.Stop();

                PerformanceCollectorEventSource.Log.CounterCollectionSuccessEvent(
#pragma warning disable CA1829 // Use Length/Count property instead of Count() when available
                    results.LongCount(),
#pragma warning restore CA1829 // Use Length/Count property instead of Count() when available
                    stopwatch.ElapsedMilliseconds);

                foreach (var result in results)
                {
                    var telemetry = CreateTelemetry(result.Item1, result.Item2);
                    try
                    {
                        this.client.Track(telemetry);
                    }
                    catch (InvalidOperationException e)
                    {
                        PerformanceCollectorEventSource.Log.TelemetrySendFailedEvent(e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                PerformanceCollectorEventSource.Log.UnknownErrorEvent(e.ToString());
            }
            finally
            {
                if (this.timer != null)
                {
                    this.timer.ScheduleNextTick(this.collectionPeriod);
                }

                SdkInternalOperationsMonitor.Exit();
            }
        }

        /// <summary>
        /// Binds processes to performance counters instance names and adds performance counters to the collection.
        /// </summary>
        /// <remarks>This operation is expensive, but must be done periodically to account for IIS changing instance names
        /// of the counters it reports Web Sites on as worker processes start and terminate.</remarks>
        private void EnsurePerformanceCountersRegistered()
        {
            if (DateTime.Now - this.lastRefreshTimestamp < this.registrationPeriod)
            {
                // re-registration period hasn't elapsed yet, do nothing
                return;
            }
           
            PerformanceCounterUtility.InvalidatePlaceholderCache();

            if (this.lastRefreshTimestamp == DateTime.MinValue)
            {
                // this is the initial registration, register everything
                this.ProcessCustomCounters();

                string error;
                var errors = new List<string>();

                foreach (PerformanceCounterCollectionRequest req in this.DefaultCounters.Union(this.Counters))
                {
                    this.collector.RegisterCounter(
                        req.PerformanceCounter,
                        req.ReportAs,
                        out error,
                        false);

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        errors.Add(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "Counter {0}: {1}",
                                req.PerformanceCounter,
                                error));
                    }
                }

                if (errors.Any())
                {
                    // send out the unified error message
                    PerformanceCollectorEventSource.Log.CounterCheckConfigurationEvent(
                        errors.Count.ToString(CultureInfo.InvariantCulture),
                        string.Join(Environment.NewLine, errors));
                }
            }
            else
            {
                // this is a periodic refresh
                this.collector.RefreshCounters();
            }

            // as per MSDN, we need to wait at least 1s before proceeding with counter collection
            Thread.Sleep(TimeSpan.FromSeconds(1));

            this.lastRefreshTimestamp = DateTime.Now;
        }

        private void ProcessCustomCounters()
        {
            // remove duplicate counters
            this.Counters = this.Counters.GroupBy(req => req.PerformanceCounter).Select(g => g.First()).ToList();

            // assign and sanitize reportsAs
            foreach (PerformanceCounterCollectionRequest req in this.Counters)
            {
                // Keep replacing '\' for backcompat
                req.ReportAs = string.IsNullOrWhiteSpace(req.ReportAs)
                    ? req.PerformanceCounter.Trim('\\').Replace(@"\", @" - ")
                    : req.ReportAs;
            }
        }

        /// <summary>
        /// Creates a metric telemetry associated with the PerformanceCounterData, with the respective float value.
        /// </summary>
        /// <param name="pc">PerformanceCounterData for which we are generating the telemetry.</param>
        /// <param name="value">The metric value for the respective performance counter data.</param>
        /// <returns>Metric telemetry object associated with the specific counter.</returns>
        private static MetricTelemetry CreateTelemetry(PerformanceCounterData pc, double value)
        {
            var metricName = !string.IsNullOrWhiteSpace(pc.ReportAs)
                                 ? pc.ReportAs
                                 : string.Format(
                                     CultureInfo.InvariantCulture,
                                     "{0} - {1}",
                                     pc.PerformanceCounter.CategoryName,
                                     pc.PerformanceCounter.CounterName);

            var metricTelemetry = new MetricTelemetry()
            {
                Name = metricName,
                Count = 1,
                Sum = value,
                Min = value,
                Max = value,
                StandardDeviation = 0,
            };

            metricTelemetry.Properties.Add("CounterInstanceName", pc.PerformanceCounter.InstanceName);
            metricTelemetry.Properties.Add("CustomPerfCounter", "true");

            return metricTelemetry;
        }
    }
}
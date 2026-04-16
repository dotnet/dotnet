namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib;
#if NETSTANDARD2_0
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.XPlatform;
#endif

    /// <summary>
    /// Telemetry module for collecting QuickPulse data.
    /// </summary>
    public sealed class QuickPulseTelemetryModule : ITelemetryModule, IDisposable
    {
#if NETSTANDARD2_0
        internal static IQuickPulseModuleScheduler ModuleScheduler = QuickPulseTaskModuleScheduler.Instance;
#else
        internal static IQuickPulseModuleScheduler ModuleScheduler = QuickPulseThreadModuleScheduler.Instance;
#endif

        internal readonly LinkedList<IQuickPulseTelemetryProcessor> TelemetryProcessors = new LinkedList<IQuickPulseTelemetryProcessor>();

        internal IQuickPulseServiceClient ServiceClient;

        private const int MaxSampleStorageSize = 10;

        private const int TopCpuCount = 5;

        private readonly object moduleInitializationLock = new object();

        private readonly object telemetryProcessorsLock = new object();

        private readonly object collectedSamplesLock = new object();

        private readonly object performanceCollectorUpdateLock = new object();

        private readonly LinkedList<QuickPulseDataSample> collectedSamples = new LinkedList<QuickPulseDataSample>();

        private TelemetryConfiguration config;

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "This object self-disposes with this class's Dispose method.")]
        private IQuickPulseModuleSchedulerHandle collectionThread;

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "This object self-disposes with this class's Dispose method.")]
        private IQuickPulseModuleSchedulerHandle stateThread;

        private Clock timeProvider;

        private QuickPulseTimings timings;

        /// <summary>Gets a value indicating whether this module has been initialized.</summary>
        internal bool IsInitialized { get; private set; } = false;

        private QuickPulseCollectionTimeSlotManager collectionTimeSlotManager = null;

        private IQuickPulseDataAccumulatorManager dataAccumulatorManager = null;

        private QuickPulseCollectionStateManager stateManager = null;

        private IPerformanceCollector performanceCollector = null;

        private IQuickPulseTopCpuCollector topCpuCollector = null;

        private CollectionConfiguration collectionConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickPulseTelemetryModule"/> class.
        /// </summary>
        public QuickPulseTelemetryModule()
        {
            this.ServerId = Environment.MachineName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickPulseTelemetryModule"/> class. Internal constructor for unit tests only.
        /// </summary>
        /// <param name="collectionTimeSlotManager">Collection time slot manager.</param>
        /// <param name="dataAccumulatorManager">Data hub to sink QuickPulse data to.</param>
        /// <param name="serviceClient">QPS service client.</param>
        /// <param name="performanceCollector">Performance counter collector.</param>
        /// <param name="topCpuCollector">Top N CPU collector.</param>
        /// <param name="timings">Timings for the module.</param>
        internal QuickPulseTelemetryModule(
            QuickPulseCollectionTimeSlotManager collectionTimeSlotManager,
            QuickPulseDataAccumulatorManager dataAccumulatorManager,
            IQuickPulseServiceClient serviceClient,
            IPerformanceCollector performanceCollector,
            IQuickPulseTopCpuCollector topCpuCollector,
            QuickPulseTimings timings)
            : this()
        {
            this.collectionTimeSlotManager = collectionTimeSlotManager;
            this.dataAccumulatorManager = dataAccumulatorManager;
            this.ServiceClient = serviceClient;
            this.performanceCollector = performanceCollector;
            this.topCpuCollector = topCpuCollector;
            this.timings = timings;
        }

        /// <summary>
        /// Gets or sets the QuickPulse service endpoint to send to.
        /// </summary>
        /// <remarks>Loaded from configuration.</remarks>
        public string QuickPulseServiceEndpoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether full telemetry items collection is disabled.
        /// </summary>
        /// <remarks>Loaded from configuration.</remarks>
        public bool DisableFullTelemetryItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether top CPU processes collection is disabled.
        /// </summary>
        /// <remarks>Loaded from configuration.</remarks>
        public bool DisableTopCpuProcesses { get; set; }

        /// <summary>
        /// Gets or sets the authentication API key.
        /// Authentication API key is created in the Azure Portal for an application and ensures secure distribution of
        /// the collection configuration when using QuickPulse.
        /// </summary>
        /// <remarks>Loaded from configuration.</remarks>
        public string AuthenticationApiKey { get; set; }

        /// <summary>
        /// Gets or sets QuickPulse ServiceId that is used to distinguish servers.
        /// </summary>
        /// <remarks>Loaded from configuration and defaults to Environment.MachineName.</remarks>
        public string ServerId { get; set; }

        /// <summary>
        /// Disposes resources allocated by this type.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initialize method is called after all configuration properties have been loaded from the configuration.
        /// </summary>
        /// <param name="configuration">TelemetryConfiguration passed to the module.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            if (!this.IsInitialized)
            {
                lock (this.moduleInitializationLock)
                {
                    if (!this.IsInitialized)
                    {
                        QuickPulseEventSource.Log.ModuleIsBeingInitializedEvent(
                            this.QuickPulseServiceEndpoint,
                            this.DisableFullTelemetryItems,
                            this.DisableTopCpuProcesses,
                            this.AuthenticationApiKey);

                        QuickPulseEventSource.Log.TroubleshootingMessageEvent("Validating configuration...");
                        ValidateConfiguration(configuration);
                        this.config = configuration;

                        QuickPulseEventSource.Log.TroubleshootingMessageEvent("Initializing members...");
                        this.collectionTimeSlotManager = this.collectionTimeSlotManager ?? new QuickPulseCollectionTimeSlotManager();

                        this.performanceCollector = this.performanceCollector ?? PerformanceCounterUtility.GetPerformanceCollector();

                        this.timeProvider = this.timeProvider ?? new Clock();
                        this.topCpuCollector = this.topCpuCollector
                                               ?? new QuickPulseTopCpuCollector(this.timeProvider, new QuickPulseProcessProvider(PerfLib.GetPerfLib()));
                        this.timings = this.timings ?? QuickPulseTimings.Default;

                        CollectionConfigurationError[] errors;
                        this.collectionConfiguration = new CollectionConfiguration(
                            new CollectionConfigurationInfo() { ETag = string.Empty },
                            out errors,
                            this.timeProvider);
                        this.dataAccumulatorManager = this.dataAccumulatorManager ?? new QuickPulseDataAccumulatorManager(this.collectionConfiguration);

                        this.InitializeServiceClient(configuration);
                        
                        this.stateManager = new QuickPulseCollectionStateManager(
                            configuration,
                            this.ServiceClient,
                            this.timeProvider,
                            this.timings,
                            this.OnStartCollection,
                            this.OnStopCollection,
                            this.OnSubmitSamples,
                            this.OnReturnFailedSamples,
                            this.OnUpdatedConfiguration,
                            this.OnUpdatedServiceEndpoint);

                        this.CreateStateThread();

                        this.IsInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Registers an instance of type <see cref="QuickPulseTelemetryProcessor"/> with this module.
        /// </summary>
        /// <remarks>This call is only necessary when the module is created in code and not in configuration.</remarks>
        /// <param name="telemetryProcessor">QuickPulseTelemetryProcessor instance to be registered with the module.</param>
        public void RegisterTelemetryProcessor(ITelemetryProcessor telemetryProcessor)
        {
            var quickPulseTelemetryProcessor = telemetryProcessor as IQuickPulseTelemetryProcessor;
            if (quickPulseTelemetryProcessor == null)
            {
                throw new ArgumentNullException(nameof(telemetryProcessor), @"The argument must be of type QuickPulseTelemetryProcessor");
            }

            lock (this.telemetryProcessorsLock)
            {
                const int MaxTelemetryProcessorCount = 100;
                if (!this.TelemetryProcessors.Contains(quickPulseTelemetryProcessor))
                {
                    this.TelemetryProcessors.AddLast(quickPulseTelemetryProcessor);

                    if (this.TelemetryProcessors.Count > MaxTelemetryProcessorCount)
                    {
                        this.TelemetryProcessors.RemoveFirst();
                    }

                    if (this.ServiceClient != null)
                    {
                        quickPulseTelemetryProcessor.ServiceEndpoint = this.ServiceClient.CurrentServiceUri;
                    }

                    QuickPulseEventSource.Log.ProcessorRegistered(this.TelemetryProcessors.Count.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "Argument exceptions are valid.")]
        private static void ValidateConfiguration(TelemetryConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (configuration.TelemetryProcessors == null)
            {
                throw new ArgumentNullException(nameof(configuration.TelemetryProcessors));
            }            
        }

        private static CloudContext GetCloudContext(TelemetryConfiguration configuration)
        {
            // we need to initialize an item to get instance information
            var fakeItem = new EventTelemetry();

            try
            {
                new TelemetryClient(configuration).Initialize(fakeItem);
            }
            catch (Exception)
            {
                // we don't care what happened there
            }

            return fakeItem.Context?.Cloud;
        }

        private static string GetStreamId()
        {
            return Guid.NewGuid().ToStringInvariant("N");
        }

        private static QuickPulseDataSample CreateDataSample(
            QuickPulseDataAccumulator accumulator,
            IEnumerable<Tuple<PerformanceCounterData, double>> perfData,
            IEnumerable<Tuple<string, int>> topCpuData,
            bool topCpuDataAccessDenied)
        {
            return new QuickPulseDataSample(
                accumulator,
                perfData.ToDictionary(tuple => tuple.Item1.ReportAs, tuple => tuple),
                topCpuData,
                topCpuDataAccessDenied);
        }

        private void UpdatePerformanceCollector(IEnumerable<Tuple<string, string>> performanceCountersToCollect, out CollectionConfigurationError[] errors)
        {
            // all counters that need to be collected according to the new configuration - remove duplicates
            List<Tuple<string, string>> countersToCollect =
                performanceCountersToCollect.GroupBy(counter => counter.Item1, StringComparer.Ordinal)
                    .Select(group => group.First())
                    .Concat(
                        QuickPulseDefaults.DefaultCountersToCollect.Select(defaultCounter => Tuple.Create(defaultCounter.Value, defaultCounter.Value)))
                    .ToList();

            lock (this.performanceCollectorUpdateLock)
            {
                List<PerformanceCounterData> countersCurrentlyCollected = this.performanceCollector.PerformanceCounters.ToList();

                IEnumerable<Tuple<string, string>> countersToRemove =
                    countersCurrentlyCollected.Where(
                        counter => !countersToCollect.Any(c => string.Equals(c.Item1, counter.ReportAs, StringComparison.Ordinal)))
                        .Select(counter => Tuple.Create(counter.ReportAs, counter.OriginalString));

                IEnumerable<Tuple<string, string>> countersToAdd =
                    countersToCollect.Where(
                        counter => !countersCurrentlyCollected.Any(c => string.Equals(c.ReportAs, counter.Item1, StringComparison.Ordinal)));

                // remove counters that should no longer be collected
                foreach (var counter in countersToRemove)
                {
                    this.performanceCollector.RemoveCounter(counter.Item2, counter.Item1);
                }

                var errorsList = new List<CollectionConfigurationError>();

                // add counters that should now be collected
                foreach (var counter in countersToAdd)
                {
                    try
                    {
                        string error;
                        this.performanceCollector.RegisterCounter(counter.Item2, counter.Item1, out error, true);

                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            errorsList.Add(
                                CollectionConfigurationError.CreateError(
                                    CollectionConfigurationErrorType.PerformanceCounterParsing,
                                    string.Format(CultureInfo.InvariantCulture, "Error parsing performance counter: '{0}'. {1}", counter, error),
                                    null,
                                    Tuple.Create("MetricId", counter.Item1)));

                            QuickPulseEventSource.Log.CounterParsingFailedEvent(error, counter.Item2);
                            continue;
                        }

                        QuickPulseEventSource.Log.CounterRegisteredEvent(counter.Item2);
                    }
                    catch (Exception e)
                    {
                        errorsList.Add(
                            CollectionConfigurationError.CreateError(
                                CollectionConfigurationErrorType.PerformanceCounterUnexpected,
                                string.Format(CultureInfo.InvariantCulture, "Unexpected error processing counter '{0}': {1}", counter, e.Message),
                                e,
                                Tuple.Create("MetricId", counter.Item1)));
                        QuickPulseEventSource.Log.CounterRegistrationFailedEvent(e.Message, counter.Item2);
                    }
                }

                errors = errorsList.ToArray();
            }
        }

        private void CreateStateThread()
        {
            this.stateThread = QuickPulseTelemetryModule.ModuleScheduler.Execute(this.StateThreadWorker);
        }

        private void InitializeServiceClient(TelemetryConfiguration configuration)
        {
            if (this.ServiceClient != null)
            {
                // service client has been passed through a constructor, we don't need to do anything
                return;
            }

            Uri serviceEndpointUri;
            if (string.IsNullOrWhiteSpace(this.QuickPulseServiceEndpoint))
            {
                // endpoint is not explicitly specified, use the Endpoint from the TelemetryConfiguration (ex: https://rt.services.visualstudio.com/QuickPulseService.svc)
                serviceEndpointUri = new Uri(configuration.EndpointContainer.Live, "QuickPulseService.svc");
            }
            else
            {
                // endpoint appears to have been specified in configuration, try using it
                try
                {
                    serviceEndpointUri = new Uri(this.QuickPulseServiceEndpoint);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Error initializing QuickPulse module. QPS endpoint is not a correct URI: '{0}'",
                            this.QuickPulseServiceEndpoint),
                        e);
                }
            }

            // create the default production implementation of the service client with the best service endpoint we could get
            CloudContext cloudContext = GetCloudContext(configuration);
            string instanceName = string.IsNullOrWhiteSpace(cloudContext?.RoleInstance) ? Environment.MachineName : cloudContext.RoleInstance;
            string roleName = cloudContext?.RoleName ?? string.Empty;
            string streamId = GetStreamId();
            var assemblyVersion = Common.SdkVersionUtils.GetSdkVersion(null);
            bool isWebApp = PerformanceCounterUtility.IsWebAppRunningInAzure();
            int? processorCount = PerformanceCounterUtility.GetProcessorCount();
            this.ServiceClient = new QuickPulseServiceClient(
                serviceEndpointUri,
                instanceName,
                roleName,
                streamId,
                this.ServerId,
                assemblyVersion,
                this.timeProvider,
                isWebApp,
                processorCount ?? 0);

            // TelemetryConfigurationFactory will initialize Modules after Processors. Need to update the processor with the correct service endpoint.
            foreach (var processor in this.TelemetryProcessors)
            {
                processor.ServiceEndpoint = serviceEndpointUri;
            }

            QuickPulseEventSource.Log.TroubleshootingMessageEvent(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Service client initialized. Endpoint: '{0}', instance name: '{1}', assembly version: '{2}'",
                    serviceEndpointUri,
                    instanceName,
                    assemblyVersion));
        }

        private void StateThreadWorker(CancellationToken cancellationToken)
        {
            SdkInternalOperationsMonitor.Enter();

            var stopwatch = new Stopwatch();
            TimeSpan? timeToNextUpdate = null;

            while (true)
            {
                var currentCallbackStarted = this.timeProvider.UtcNow;

                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        SdkInternalOperationsMonitor.Exit();

                        return;
                    }

                    stopwatch.Restart();

                    timeToNextUpdate = this.stateManager.UpdateState(this.config.InstrumentationKey, this.AuthenticationApiKey);

                    QuickPulseEventSource.Log.StateTimerTickFinishedEvent(stopwatch.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    QuickPulseEventSource.Log.UnknownErrorEvent(e.ToInvariantString());
                }

                // the catastrophic fallback is for the case when we've catastrophically failed some place above
                timeToNextUpdate = timeToNextUpdate ?? this.timings.CatastrophicFailureTimeout;

                // try to factor in the time spend in this tick when scheduling the next one so that the average period is close to the intended
                TimeSpan timeSpentInThisTick = this.timeProvider.UtcNow - currentCallbackStarted;
                TimeSpan timeLeftUntilNextTick = timeToNextUpdate.Value - timeSpentInThisTick;
                timeLeftUntilNextTick = timeLeftUntilNextTick > TimeSpan.Zero ? timeLeftUntilNextTick : TimeSpan.Zero;

                try
                {
                    Task.Delay(timeLeftUntilNextTick, cancellationToken).GetAwaiter().GetResult();
                }
                catch (OperationCanceledException)
                {
                }
            }
        }

        private void CollectionThreadWorker(CancellationToken cancellationToken)
        {
            SdkInternalOperationsMonitor.Enter();

            var stopwatch = new Stopwatch();

            this.InitializeCollectionThread();

            while (true)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        this.CloseCollectionThread();

                        SdkInternalOperationsMonitor.Exit();

                        return;
                    }

                    stopwatch.Restart();

                    this.CollectData();

                    QuickPulseEventSource.Log.CollectionTimerTickFinishedEvent(stopwatch.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    QuickPulseEventSource.Log.UnknownErrorEvent(e.ToInvariantString());
                }

                DateTimeOffset nextTick = this.collectionTimeSlotManager.GetNextCollectionTimeSlot(this.timeProvider.UtcNow);
                TimeSpan timeLeftUntilNextTick = nextTick - this.timeProvider.UtcNow;
                timeLeftUntilNextTick = timeLeftUntilNextTick > TimeSpan.Zero ? timeLeftUntilNextTick : TimeSpan.Zero;

                try
                {
                    Task.Delay(timeLeftUntilNextTick, cancellationToken).GetAwaiter().GetResult();
                }
                catch (OperationCanceledException)
                {
                }
            }
        }
        
        private void InitializeCollectionThread()
        {
            try
            {
                this.topCpuCollector.Initialize();
            }
            catch (Exception e)
            {
                // whatever happened, don't bring the thread down
                QuickPulseEventSource.Log.UnknownErrorEvent(e.ToInvariantString());
            }
        }

        private void CloseCollectionThread()
        {
            try
            {
                this.topCpuCollector.Close();
            }
            catch (Exception e)
            {
                // whatever happened, don't bring the thread down
                QuickPulseEventSource.Log.UnknownErrorEvent(e.ToInvariantString());
            }
        }

        private void CollectData()
        {
            var sample = this.CollectSample();

            this.StoreSample(sample);
        }

        private void StoreSample(QuickPulseDataSample sample)
        {
            lock (this.collectedSamplesLock)
            {
                QuickPulseEventSource.Log.SampleStoredEvent(this.collectedSamples.Count + 1);

                this.collectedSamples.AddLast(sample);

                while (this.collectedSamples.Count > MaxSampleStorageSize)
                {
                    this.collectedSamples.RemoveFirst();
                }
            }
        }

        private QuickPulseDataSample CollectSample()
        {
            // For AI data, all we have to do is lock the current accumulator in
            // use the latest collection configuration info set by the state thread to create the new accumulator
            QuickPulseDataAccumulator completeAccumulator = this.dataAccumulatorManager.CompleteCurrentDataAccumulator(this.collectionConfiguration);

            // For performance collection, we have to read perf samples from Windows
            List<Tuple<PerformanceCounterData, double>> perfData;
            lock (this.performanceCollectorUpdateLock)
            {
                perfData =
                    this.performanceCollector.Collect(
                        (counterName, e) => QuickPulseEventSource.Log.CounterReadingFailedEvent(e.ToString(), counterName)).ToList();
            }

            // For top N CPU, we have to get data from the provider
            IEnumerable<Tuple<string, int>> topCpuData = this.DisableTopCpuProcesses
                                                             ? Enumerable.Empty<Tuple<string, int>>()
                                                             : this.topCpuCollector.GetTopProcessesByCpu(TopCpuCount);

            return CreateDataSample(completeAccumulator, perfData, topCpuData, this.topCpuCollector.AccessDenied);
        }

#region Callbacks from the state manager

        private void OnStartCollection()
        {
            QuickPulseEventSource.Log.TroubleshootingMessageEvent("Starting collection...");

            this.EndCollectionThread();

            CollectionConfigurationError[] errors;
            this.UpdatePerformanceCollector(this.collectionConfiguration.PerformanceCounters, out errors);

            this.dataAccumulatorManager.CompleteCurrentDataAccumulator(this.collectionConfiguration);

            lock (this.telemetryProcessorsLock)
            {
                foreach (var telemetryProcessor in this.TelemetryProcessors)
                {
                    telemetryProcessor.StartCollection(
                        this.dataAccumulatorManager,
                        this.ServiceClient.CurrentServiceUri,
                        this.config,
                        this.DisableFullTelemetryItems);
                }
            }

            this.CreateCollectionThread();
        }

        private void CreateCollectionThread()
        {
            this.collectionThread = QuickPulseTelemetryModule.ModuleScheduler.Execute(this.CollectionThreadWorker);
        }

        private void EndCollectionThread()
        {
            Interlocked.Exchange(ref this.collectionThread, null)?.Stop(wait: false);
        }

        private void OnStopCollection()
        {
            QuickPulseEventSource.Log.TroubleshootingMessageEvent("Stopping collection...");

            this.EndCollectionThread();

            lock (this.telemetryProcessorsLock)
            {
                foreach (var telemetryProcessor in this.TelemetryProcessors)
                {
                    telemetryProcessor.StopCollection();
                }
            }

            lock (this.collectedSamplesLock)
            {
                this.collectedSamples.Clear();
            }
        }

        private IList<QuickPulseDataSample> OnSubmitSamples()
        {
            IList<QuickPulseDataSample> samples;

            lock (this.collectedSamplesLock)
            {
                samples = this.collectedSamples.ToList();

                this.collectedSamples.Clear();
            }

            return samples;
        }

        private void OnReturnFailedSamples(IList<QuickPulseDataSample> samples)
        {
            // append the samples that failed to get sent out back to the beginning of the list
            // these will be pushed out as newer samples arrive, so we'll never get more than a certain number
            // even if the network is lagging behind 
            QuickPulseEventSource.Log.TroubleshootingMessageEvent("Returning samples...");

            lock (this.collectedSamplesLock)
            {
                foreach (var sample in samples)
                {
                    this.collectedSamples.AddFirst(sample);
                }

                while (this.collectedSamples.Count > MaxSampleStorageSize)
                {
                    this.collectedSamples.RemoveFirst();
                }
            }
        }

        private CollectionConfigurationError[] OnUpdatedConfiguration(CollectionConfigurationInfo configurationInfo)
        {
            // we need to preserve the current quota for each document stream that still exists in the new configuration
            CollectionConfigurationError[] errorsConfig;

            lock (this.telemetryProcessorsLock)
            {
                foreach (IQuickPulseTelemetryProcessor telemetryProcessor in this.TelemetryProcessors)
                {
                    telemetryProcessor.UpdateGlobalQuotas(this.timeProvider, configurationInfo.QuotaInfo);
                }
            }

            var newCollectionConfiguration = new CollectionConfiguration(configurationInfo, out errorsConfig, this.timeProvider, this.collectionConfiguration?.DocumentStreams);

            // the next accumulator that gets swapped in on the collection thread will be initialized with the new collection configuration
            Interlocked.Exchange(ref this.collectionConfiguration, newCollectionConfiguration);

            CollectionConfigurationError[] errorsPerformanceCounters;
            this.UpdatePerformanceCollector(newCollectionConfiguration.PerformanceCounters, out errorsPerformanceCounters);

            return errorsConfig.Concat(errorsPerformanceCounters).ToArray();
        }

        private void OnUpdatedServiceEndpoint(Uri newServiceEndpoint)
        {
            QuickPulseEventSource.Log.TroubleshootingMessageEvent("Service endpoint updated.");
            
            lock (this.telemetryProcessorsLock)
            {
                foreach (var telemetryProcessor in this.TelemetryProcessors)
                {
                    telemetryProcessor.ServiceEndpoint = newServiceEndpoint;
                }
            }
        }
        #endregion

        /// <summary>
        /// Dispose implementation.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Interlocked.Exchange(ref this.stateThread, null)?.Stop(wait: true);
                Interlocked.Exchange(ref this.collectionThread, null)?.Stop(wait: true);

                if (this.ServiceClient != null)
                {
                    this.ServiceClient.Dispose();
                }
            }
        }
    }
}
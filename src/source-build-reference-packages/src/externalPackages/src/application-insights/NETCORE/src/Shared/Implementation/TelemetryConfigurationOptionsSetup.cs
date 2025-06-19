namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

#if AI_ASPNETCORE_WEB
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
#else
    using Microsoft.ApplicationInsights.WorkerService;
    using Microsoft.ApplicationInsights.WorkerService.Implementation.Tracing;
#endif
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
    using Microsoft.ApplicationInsights.WindowsServer;
    using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Initializes TelemetryConfiguration based on values in <see cref="ApplicationInsightsServiceOptions"/>
    /// and registered <see cref="ITelemetryInitializer"/>s and <see cref="ITelemetryModule"/>s.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "This class is instantiated by Dependency Injection.")]
    internal class TelemetryConfigurationOptionsSetup : IConfigureOptions<TelemetryConfiguration>
    {
        private readonly ApplicationInsightsServiceOptions applicationInsightsServiceOptions;
        private readonly IEnumerable<ITelemetryInitializer> initializers;
        private readonly IEnumerable<ITelemetryModule> modules;
        private readonly ITelemetryChannel telemetryChannel;
        private readonly IEnumerable<ITelemetryProcessorFactory> telemetryProcessorFactories;
        private readonly IEnumerable<ITelemetryModuleConfigurator> telemetryModuleConfigurators;
        private readonly IApplicationIdProvider applicationIdProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryConfigurationOptionsSetup"/> class.
        /// </summary>
        /// <param name="serviceProvider">Instance of IServiceProvider.</param>
        /// <param name="applicationInsightsServiceOptions">Instance of IOptions&lt;ApplicationInsightsServiceOptions>.</param>
        /// <param name="initializers">Instance of IEnumerable&lt;ITelemetryInitializer>.</param>
        /// <param name="modules">Instance of IEnumerable&lt;ITelemetryModule>.</param>
        /// <param name="telemetryProcessorFactories">Instance of IEnumerable&lt;ITelemetryProcessorFactory>.</param>
        /// <param name="telemetryModuleConfigurators">Instance of IEnumerable&lt;ITelemetryModuleConfigurator>.</param>
        public TelemetryConfigurationOptionsSetup(
            IServiceProvider serviceProvider,
            IOptions<ApplicationInsightsServiceOptions> applicationInsightsServiceOptions,
            IEnumerable<ITelemetryInitializer> initializers,
            IEnumerable<ITelemetryModule> modules,
            IEnumerable<ITelemetryProcessorFactory> telemetryProcessorFactories,
            IEnumerable<ITelemetryModuleConfigurator> telemetryModuleConfigurators)
        {
            this.applicationInsightsServiceOptions = applicationInsightsServiceOptions.Value;
            this.initializers = initializers;
            this.modules = modules;
            this.telemetryProcessorFactories = telemetryProcessorFactories;
            this.telemetryModuleConfigurators = telemetryModuleConfigurators;
            this.telemetryChannel = serviceProvider.GetService<ITelemetryChannel>();
            this.applicationIdProvider = serviceProvider.GetService<IApplicationIdProvider>();
        }

        /// <inheritdoc />
        public void Configure(TelemetryConfiguration configuration)
        {
            try
            {
#pragma warning disable CS0618 // Type or member is obsolete
                if (this.applicationInsightsServiceOptions.InstrumentationKey != null)
                {
                    configuration.InstrumentationKey = this.applicationInsightsServiceOptions.InstrumentationKey;
                }
#pragma warning restore CS0618 // Type or member is obsolete

                if (this.telemetryModuleConfigurators.Any())
                {
                    foreach (ITelemetryModuleConfigurator telemetryModuleConfigurator in this.telemetryModuleConfigurators)
                    {
                        ITelemetryModule telemetryModule = this.modules.FirstOrDefault((module) => module.GetType() == telemetryModuleConfigurator.TelemetryModuleType);
                        if (telemetryModule != null)
                        {
                            telemetryModuleConfigurator.Configure(telemetryModule, this.applicationInsightsServiceOptions);
                        }
                        else
                        {
#if AI_ASPNETCORE_WEB
                            AspNetCoreEventSource.Instance.UnableToFindModuleToConfigure(telemetryModuleConfigurator.TelemetryModuleType.ToString());
#else
                            WorkerServiceEventSource.Instance.UnableToFindModuleToConfigure(telemetryModuleConfigurator.TelemetryModuleType.ToString());
#endif
                        }
                    }
                }

                if (this.telemetryProcessorFactories.Any())
                {
                    foreach (ITelemetryProcessorFactory processorFactory in this.telemetryProcessorFactories)
                    {
                        configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder.Use(processorFactory.Create);
                    }
                }

                // Fallback to default channel (InMemoryChannel) created by base sdk if no channel is found in DI
                configuration.TelemetryChannel = this.telemetryChannel ?? configuration.TelemetryChannel;
                (configuration.TelemetryChannel as ITelemetryModule)?.Initialize(configuration);

                this.AddAutoCollectedMetricExtractor(configuration);
                this.AddQuickPulse(configuration);
                this.AddSampling(configuration);
                this.DisableHeartBeatIfConfigured();

                configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder.Build();
                configuration.TelemetryProcessorChainBuilder.Build();

                if (this.applicationInsightsServiceOptions.DeveloperMode != null)
                {
                    configuration.TelemetryChannel.DeveloperMode = this.applicationInsightsServiceOptions.DeveloperMode;
                }

                if (this.applicationInsightsServiceOptions.EndpointAddress != null)
                {
                    configuration.TelemetryChannel.EndpointAddress = this.applicationInsightsServiceOptions.EndpointAddress;
                }

                // Need to set connection string before calling Initialize() on the Modules and Processors.
                if (this.applicationInsightsServiceOptions.ConnectionString != null)
                {
                    configuration.ConnectionString = this.applicationInsightsServiceOptions.ConnectionString;
                }

                foreach (ITelemetryInitializer initializer in this.initializers)
                {
                    configuration.TelemetryInitializers.Add(initializer);
                }

                // Find the DiagnosticsTelemetryModule, this is needed to initialize AzureInstanceMetadataTelemetryModule and AppServicesHeartbeatTelemetryModule.
                DiagnosticsTelemetryModule diagModule =
                    (this.applicationInsightsServiceOptions.EnableDiagnosticsTelemetryModule && this.applicationInsightsServiceOptions.EnableHeartbeat)
                    ? this.modules.OfType<DiagnosticsTelemetryModule>().FirstOrDefault()
                    : null;

                // Checks ApplicationInsightsServiceOptions and disable TelemetryModules if explicitly disabled.
                foreach (ITelemetryModule module in this.modules)
                {
                    // If any of the modules are disabled explicitly using aioptions,
                    // do not initialize them and dispose if disposable.
                    // The option of not adding a module to DI if disabled
                    // cannot be done to maintain backward compatibility.
                    // So this approach of adding all modules to DI, but selectively
                    // disable those modules which user has disabled is chosen.

                    if (module is DiagnosticsTelemetryModule)
                    {
                        if (!this.applicationInsightsServiceOptions.EnableDiagnosticsTelemetryModule)
                        {
                            DisposeIfDisposable(module);
                            continue;
                        }
                    }

                    // DependencyTrackingTelemetryModule
                    if (module is DependencyTrackingTelemetryModule)
                    {
                        if (!this.applicationInsightsServiceOptions.EnableDependencyTrackingTelemetryModule)
                        {
                            DisposeIfDisposable(module);
                            continue;
                        }
                    }

#if AI_ASPNETCORE_WEB
                    // RequestTrackingTelemetryModule
                    if (module is RequestTrackingTelemetryModule)
                    {
                        if (!this.applicationInsightsServiceOptions.EnableRequestTrackingTelemetryModule)
                        {
                            DisposeIfDisposable(module);
                            continue;
                        }
                    }
#endif

                    // EventCounterCollectionModule
                    if (module is EventCounterCollectionModule)
                    {
                        if (!this.applicationInsightsServiceOptions.EnableEventCounterCollectionModule)
                        {
                            DisposeIfDisposable(module);
                            continue;
                        }
                    }

                    // PerformanceCollectorModule
                    if (module is PerformanceCollectorModule)
                    {
                        if (!this.applicationInsightsServiceOptions.EnablePerformanceCounterCollectionModule)
                        {
                            DisposeIfDisposable(module);
                            continue;
                        }
                    }

                    // AppServicesHeartbeatTelemetryModule
                    if (module is AppServicesHeartbeatTelemetryModule appServicesHeartbeatTelemetryModule)
                    {
                        if (!this.applicationInsightsServiceOptions.EnableAppServicesHeartbeatTelemetryModule)
                        {
                            DisposeIfDisposable(module);
                            continue;
                        }
                        else if (diagModule != null)
                        {
                            // diagModule is set to Null above if (applicationInsightsServiceOptions.EnableDiagnosticsTelemetryModule || this.applicationInsightsServiceOptions.EnableHeartbeat) == false.
                            appServicesHeartbeatTelemetryModule.HeartbeatPropertyManager = diagModule;
                        }
                    }

                    // AzureInstanceMetadataTelemetryModule
                    if (module is AzureInstanceMetadataTelemetryModule azureInstanceMetadataTelemetryModule)
                    {
                        if (!this.applicationInsightsServiceOptions.EnableAzureInstanceMetadataTelemetryModule)
                        {
                            DisposeIfDisposable(module);
                            continue;
                        }
                        else if (diagModule != null)
                        {
                            // diagModule is set to Null above if (applicationInsightsServiceOptions.EnableDiagnosticsTelemetryModule || this.applicationInsightsServiceOptions.EnableHeartbeat) == false.
                            azureInstanceMetadataTelemetryModule.HeartbeatPropertyManager = diagModule;
                        }
                    }

                    // QuickPulseTelemetryModule
                    if (module is QuickPulseTelemetryModule)
                    {
                        if (!this.applicationInsightsServiceOptions.EnableQuickPulseMetricStream)
                        {
                            DisposeIfDisposable(module);
                            continue;
                        }
                    }

                    try
                    {
                        module.Initialize(configuration);
                    }
                    catch (Exception ex)
                    {
#if AI_ASPNETCORE_WEB
                        AspNetCoreEventSource.Instance.TelemetryModuleInitialziationSetupFailure(module.GetType().FullName, ex.ToInvariantString());
#else
                        WorkerServiceEventSource.Instance.TelemetryModuleInitialziationSetupFailure(module.GetType().FullName, ex.ToInvariantString());
#endif
                    }
                }

                foreach (ITelemetryProcessor processor in configuration.TelemetryProcessors)
                {
                    if (processor is ITelemetryModule module)
                    {
                        module.Initialize(configuration);
                    }
                }

                // Microsoft.ApplicationInsights.DependencyCollector.DependencyTrackingTelemetryModule depends on this nullable configuration to support Correlation.
                configuration.ApplicationIdProvider = this.applicationIdProvider;

#if AI_ASPNETCORE_WEB
                AspNetCoreEventSource.Instance.LogInformational("Successfully configured TelemetryConfiguration.");
#else
                WorkerServiceEventSource.Instance.LogInformational("Successfully configured TelemetryConfiguration.");
#endif
            }
            catch (Exception ex)
            {
#if AI_ASPNETCORE_WEB
                AspNetCoreEventSource.Instance.TelemetryConfigurationSetupFailure(ex.ToInvariantString());
#else
                WorkerServiceEventSource.Instance.TelemetryConfigurationSetupFailure(ex.ToInvariantString());
#endif
            }
        }

        private static void DisposeIfDisposable(ITelemetryModule module)
        {
            if (module is IDisposable)
            {
                (module as IDisposable).Dispose();
            }
        }

        private void AddQuickPulse(TelemetryConfiguration configuration)
        {
            if (this.applicationInsightsServiceOptions.EnableQuickPulseMetricStream)
            {
                if (this.modules.FirstOrDefault((module) => module.GetType() == typeof(QuickPulseTelemetryModule)) is QuickPulseTelemetryModule quickPulseModule)
                {
                    QuickPulseTelemetryProcessor processor = null;
                    configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder.Use((next) =>
                    {
                        processor = new QuickPulseTelemetryProcessor(next);
                        quickPulseModule.RegisterTelemetryProcessor(processor);
                        return processor;
                    });
                }
                else
                {
#if AI_ASPNETCORE_WEB
                    AspNetCoreEventSource.Instance.UnableToFindModuleToConfigure("QuickPulseTelemetryModule");
#else
                    WorkerServiceEventSource.Instance.UnableToFindModuleToConfigure("QuickPulseTelemetryModule");
#endif
                }
            }
        }

        private void AddSampling(TelemetryConfiguration configuration)
        {
            if (this.applicationInsightsServiceOptions.EnableAdaptiveSampling)
            {
                AdaptiveSamplingPercentageEvaluatedCallback samplingCallback = (ratePerSecond, currentPercentage, newPercentage, isChanged, estimatorSettings) =>
                {
                    if (isChanged)
                    {
                        configuration.SetLastObservedSamplingPercentage(SamplingTelemetryItemTypes.Request, newPercentage);
                    }
                };

                SamplingPercentageEstimatorSettings settings = new SamplingPercentageEstimatorSettings();
                settings.MaxTelemetryItemsPerSecond = 5;
                configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder.UseAdaptiveSampling(settings, samplingCallback, excludedTypes: "Event");
                configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder.UseAdaptiveSampling(5, includedTypes: "Event");
            }
        }

        private void AddAutoCollectedMetricExtractor(TelemetryConfiguration configuration)
        {
            if (this.applicationInsightsServiceOptions.AddAutoCollectedMetricExtractor)
            {
                configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder.Use(next => new AutocollectedMetricsExtractor(next));
            }
        }

        private void DisableHeartBeatIfConfigured()
        {
            // Disable heartbeat if user sets it (by default it is on)
            if (!this.applicationInsightsServiceOptions.EnableHeartbeat)
            {
                foreach (var module in this.modules)
                {
                    if (module is IHeartbeatPropertyManager hbeatMan)
                    {
                        hbeatMan.IsHeartbeatEnabled = false;
                    }
                }
            }
        }
    }
}
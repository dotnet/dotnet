namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using Microsoft.ApplicationInsights;
#if AI_ASPNETCORE_WEB
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
#endif

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;

    using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
    using Microsoft.ApplicationInsights.WindowsServer;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;

#if AI_ASPNETCORE_WORKER
    using Microsoft.ApplicationInsights.WorkerService;
    using Microsoft.ApplicationInsights.WorkerService.Implementation.Tracing;
    using Microsoft.ApplicationInsights.WorkerService.TelemetryInitializers;
#endif

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Memory;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> that allow adding Application Insights services to application.
    /// </summary>
    public static partial class ApplicationInsightsExtensions
    {
        private const string VersionKeyFromConfig = "version";
        private const string InstrumentationKeyFromConfig = "ApplicationInsights:InstrumentationKey";
        private const string ConnectionStringFromConfig = "ApplicationInsights:ConnectionString";
        private const string DeveloperModeFromConfig = "ApplicationInsights:TelemetryChannel:DeveloperMode";
        private const string EndpointAddressFromConfig = "ApplicationInsights:TelemetryChannel:EndpointAddress";

        private const string InstrumentationKeyForWebSites = "APPINSIGHTS_INSTRUMENTATIONKEY";
        private const string ConnectionStringEnvironmentVariable = "APPLICATIONINSIGHTS_CONNECTION_STRING";
        private const string DeveloperModeForWebSites = "APPINSIGHTS_DEVELOPER_MODE";
        private const string EndpointAddressForWebSites = "APPINSIGHTS_ENDPOINTADDRESS";

        private const string ApplicationInsightsSectionFromConfig = "ApplicationInsights";
        private const string TelemetryChannelSectionFromConfig = "ApplicationInsights:TelemetryChannel";

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used in NetStandard2.0 build.")]
        private const string EventSourceNameForSystemRuntime = "System.Runtime";

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used in NetStandard2.0 build.")]
        private const string EventSourceNameForAspNetCoreHosting = "Microsoft.AspNetCore.Hosting";

        /// <summary>
        /// Adds an Application Insights Telemetry Processor into a service collection via a <see cref="ITelemetryProcessorFactory"/>.
        /// </summary>
        /// <typeparam name="T">Type of the telemetry processor to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        public static IServiceCollection AddApplicationInsightsTelemetryProcessor<T>(this IServiceCollection services)
            where T : ITelemetryProcessor
        {
            return services.AddSingleton<ITelemetryProcessorFactory>(serviceProvider =>
                new TelemetryProcessorFactory(serviceProvider, typeof(T)));
        }

        /// <summary>
        /// Adds an Application Insights Telemetry Processor into a service collection via a <see cref="ITelemetryProcessorFactory"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="telemetryProcessorType">Type of the telemetry processor to add.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="telemetryProcessorType"/> argument is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="telemetryProcessorType"/> type does not implement <see cref="ITelemetryProcessor"/>.</exception>
        public static IServiceCollection AddApplicationInsightsTelemetryProcessor(this IServiceCollection services, Type telemetryProcessorType)
        {
            if (telemetryProcessorType == null)
            {
                throw new ArgumentNullException(nameof(telemetryProcessorType));
            }

            if (!telemetryProcessorType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ITelemetryProcessor)))
            {
                throw new ArgumentException(nameof(telemetryProcessorType) + "does not implement ITelemetryProcessor.");
            }

            return services.AddSingleton<ITelemetryProcessorFactory>(serviceProvider =>
                new TelemetryProcessorFactory(serviceProvider, telemetryProcessorType));
        }

        /// <summary>
        /// Extension method to provide configuration logic for application insights telemetry module.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="configModule">Action used to configure the module.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        [Obsolete("Use ConfigureTelemetryModule overload that accepts ApplicationInsightsServiceOptions.")]
        public static IServiceCollection ConfigureTelemetryModule<T>(this IServiceCollection services, Action<T> configModule)
            where T : ITelemetryModule
        {
            if (configModule == null)
            {
                throw new ArgumentNullException(nameof(configModule));
            }

            return services.AddSingleton(
                typeof(ITelemetryModuleConfigurator),
                new TelemetryModuleConfigurator((config, options) => configModule((T)config), typeof(T)));
        }

        /// <summary>
        /// Extension method to provide configuration logic for application insights telemetry module.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="configModule">Action used to configure the module.</param>
        /// <returns>
        /// The <see cref="IServiceCollection"/>.
        /// </returns>
        public static IServiceCollection ConfigureTelemetryModule<T>(
            this IServiceCollection services,
            Action<T, ApplicationInsightsServiceOptions> configModule)
            where T : ITelemetryModule
        {
            if (configModule == null)
            {
                throw new ArgumentNullException(nameof(configModule));
            }

            return services.AddSingleton(
                typeof(ITelemetryModuleConfigurator),
                new TelemetryModuleConfigurator((config, options) => configModule((T)config, options), typeof(T)));
        }

        /// <summary>
        /// Adds Application Insight specific configuration properties to <see cref="IConfigurationBuilder"/>.
        /// </summary>
        /// <param name="configurationSourceRoot">The <see cref="IConfigurationBuilder"/> instance.</param>
        /// <param name="developerMode">Enables or disables developer mode.</param>
        /// <param name="endpointAddress">Sets telemetry endpoint address.</param>
        /// <param name="instrumentationKey">Sets instrumentation key.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        [Obsolete("InstrumentationKey based global ingestion is being deprecated. Use the AddApplicationInsightsSettings() overload which accepts string ConnectionString. See https://github.com/microsoft/ApplicationInsights-dotnet/issues/2560 for more details.")]
        [SuppressMessage("ApiDesign", "RS0026:Do not add multiple public overloads with optional parameters", Justification = "We made a mistake here, but we can't remove it from the public API now.")]
        public static IConfigurationBuilder AddApplicationInsightsSettings(this IConfigurationBuilder configurationSourceRoot,  bool? developerMode = null, string endpointAddress = null, string instrumentationKey = null)
            => configurationSourceRoot.AddApplicationInsightsSettings(connectionString: null, developerMode: developerMode, endpointAddress: endpointAddress, instrumentationKey: instrumentationKey);

        /// <summary>
        /// Adds Application Insight specific configuration properties to <see cref="IConfigurationBuilder"/>.
        /// </summary>
        /// <param name="configurationSourceRoot">The <see cref="IConfigurationBuilder"/> instance.</param>
        /// <param name="connectionString">Sets connection string.</param>
        /// <param name="developerMode">Enables or disables developer mode.</param>
        /// <param name="endpointAddress">Sets telemetry endpoint address.</param>
        /// <param name="instrumentationKey">Sets instrumentation key.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        [SuppressMessage("ApiDesign", "RS0026:Do not add multiple public overloads with optional parameters", Justification = "We made a mistake here, but we can't remove it from the public API now.")]
        public static IConfigurationBuilder AddApplicationInsightsSettings(
            this IConfigurationBuilder configurationSourceRoot,
            string connectionString,
            bool? developerMode = null,
            string endpointAddress = null,
            string instrumentationKey = null)
        {
            if (configurationSourceRoot == null)
            {
                throw new ArgumentNullException(nameof(configurationSourceRoot));
            }

            var telemetryConfigValues = new List<KeyValuePair<string, string>>();

            bool wasAnythingSet = false;

            if (developerMode != null)
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(
                    DeveloperModeForWebSites,
                    developerMode.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                wasAnythingSet = true;
            }

            if (connectionString != null)
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(ConnectionStringEnvironmentVariable, connectionString));
                wasAnythingSet = true;
            }

            if (instrumentationKey != null)
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(InstrumentationKeyForWebSites, instrumentationKey));
                wasAnythingSet = true;
            }

            if (endpointAddress != null)
            {
                telemetryConfigValues.Add(new KeyValuePair<string, string>(EndpointAddressForWebSites, endpointAddress));
                wasAnythingSet = true;
            }

            if (wasAnythingSet)
            {
                configurationSourceRoot.Add(new MemoryConfigurationSource() { InitialData = telemetryConfigValues });
            }

            return configurationSourceRoot;
        }

        /// <summary>
        /// Read configuration from appSettings.json, appsettings.{env.EnvironmentName}.json,
        /// IConfiguation used in an application and EnvironmentVariables.
        /// Bind configuration to ApplicationInsightsServiceOptions.
        /// Values can also be read from environment variables to support azure web sites configuration.
        /// </summary>
        /// <param name="config">Configuration to read variables from.</param>
        /// <param name="serviceOptions">Telemetry configuration to populate.</param>
        internal static void AddTelemetryConfiguration(
            IConfiguration config,
            ApplicationInsightsServiceOptions serviceOptions)
        {
            try
            {
                config.GetSection(ApplicationInsightsSectionFromConfig).Bind(serviceOptions);
                config.GetSection(TelemetryChannelSectionFromConfig).Bind(serviceOptions);

                if (config.TryGetValue(primaryKey: ConnectionStringEnvironmentVariable, backupKey: ConnectionStringFromConfig, value: out string connectionStringValue))
                {
                    serviceOptions.ConnectionString = connectionStringValue;
                }

                if (config.TryGetValue(primaryKey: InstrumentationKeyForWebSites, backupKey: InstrumentationKeyFromConfig, value: out string instrumentationKey))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    serviceOptions.InstrumentationKey = instrumentationKey;
#pragma warning restore CS0618 // Type or member is obsolete
                }

                if (config.TryGetValue(primaryKey: DeveloperModeForWebSites, backupKey: DeveloperModeFromConfig, value: out string developerModeValue))
                {
                    if (bool.TryParse(developerModeValue, out bool developerMode))
                    {
                        serviceOptions.DeveloperMode = developerMode;
                    }
                }

                if (config.TryGetValue(primaryKey: EndpointAddressForWebSites, backupKey: EndpointAddressFromConfig, value: out string endpointAddress))
                {
                    serviceOptions.EndpointAddress = endpointAddress;
                }

                if (config.TryGetValue(primaryKey: VersionKeyFromConfig, value: out string version))
                {
                    serviceOptions.ApplicationVersion = version;
                }
            }
            catch (Exception ex)
            {
#if AI_ASPNETCORE_WEB
                AspNetCoreEventSource.Instance.LogError(ex.ToInvariantString());
#else
                WorkerServiceEventSource.Instance.LogError(ex.ToInvariantString());
#endif
            }
        }

        /// <summary>
        /// The AddSingleton method will not check if a class has already been added as an ImplementationType.
        /// This extension method is to encapsulate those checks.
        /// </summary>
        /// <remarks>
        /// Must check all three properties to avoid duplicates or null ref exceptions.
        /// </remarks>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        internal static void AddSingletonIfNotExists<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            if (!services.Any(o => o.ImplementationFactory == null && typeof(TImplementation).IsAssignableFrom(o.ImplementationType ?? o.ImplementationInstance.GetType())))
            {
                services.AddSingleton<TService, TImplementation>();
            }
        }

        private static bool TryGetValue(this IConfiguration config, string primaryKey, out string value, string backupKey = null)
        {
            value = config[primaryKey];

            if (backupKey != null && string.IsNullOrWhiteSpace(value))
            {
                value = config[backupKey];
            }

            return !string.IsNullOrWhiteSpace(value);
        }

        private static bool IsApplicationInsightsAdded(IServiceCollection services)
        {
            // We treat TelemetryClient as a marker that AI services were added to service collection
            return services.Any(service => service.ServiceType == typeof(TelemetryClient));
        }

        private static void AddCommonInitializers(IServiceCollection services)
        {
#if AI_ASPNETCORE_WEB
            services.AddSingleton<ITelemetryInitializer, Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers.DomainNameRoleInstanceTelemetryInitializer>();
#else
            services.AddSingleton<ITelemetryInitializer, Microsoft.ApplicationInsights.WorkerService.TelemetryInitializers.DomainNameRoleInstanceTelemetryInitializer>();
#endif
            services.AddSingleton<ITelemetryInitializer, HttpDependenciesParsingTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, ComponentVersionTelemetryInitializer>();
        }

        private static void AddCommonTelemetryModules(IServiceCollection services)
        {
            // Previously users were encouraged to manually add the DiagnosticsTelemetryModule.
            services.AddSingletonIfNotExists<ITelemetryModule, DiagnosticsTelemetryModule>();

            // These modules add properties to Heartbeat and expect the DiagnosticsTelemetryModule to be configured in DI.
            services.AddSingleton<ITelemetryModule, AppServicesHeartbeatTelemetryModule>();
            services.AddSingleton<ITelemetryModule, AzureInstanceMetadataTelemetryModule>();

            services.AddSingleton<ITelemetryModule, PerformanceCollectorModule>();
            services.AddSingleton<ITelemetryModule, QuickPulseTelemetryModule>();

            AddAndConfigureDependencyTracking(services);
            services.AddSingleton<ITelemetryModule, EventCounterCollectionModule>();
        }

        private static void AddTelemetryChannel(IServiceCollection services)
        {
            services.TryAddSingleton<ITelemetryChannel, ServerTelemetryChannel>();
        }

        private static void AddDefaultApplicationIdProvider(IServiceCollection services)
        {
            services.TryAddSingleton<IApplicationIdProvider, ApplicationInsightsApplicationIdProvider>();
        }

        private static void AddTelemetryConfigAndClient(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton<IOptions<TelemetryConfiguration>, TelemetryConfigurationOptions>();
            services.AddSingleton<IConfigureOptions<TelemetryConfiguration>, TelemetryConfigurationOptionsSetup>();
            services.AddSingleton<TelemetryConfiguration>(provider =>
                provider.GetService<IOptions<TelemetryConfiguration>>().Value);
            services.AddSingleton<TelemetryClient>();
        }

        private static void AddAndConfigureDependencyTracking(IServiceCollection services)
        {
            services.AddSingleton<ITelemetryModule, DependencyTrackingTelemetryModule>();

            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
            {
                if (o.EnableDependencyTrackingTelemetryModule)
                {
                    module.EnableLegacyCorrelationHeadersInjection =
                       o.DependencyCollectionOptions.EnableLegacyCorrelationHeadersInjection;

                    var excludedDomains = module.ExcludeComponentCorrelationHttpHeadersOnDomains;
                    excludedDomains.Add("core.windows.net");
                    excludedDomains.Add("core.chinacloudapi.cn");
                    excludedDomains.Add("core.cloudapi.de");
                    excludedDomains.Add("core.usgovcloudapi.net");

                    if (module.EnableLegacyCorrelationHeadersInjection)
                    {
                        excludedDomains.Add("localhost");
                        excludedDomains.Add("127.0.0.1");
                    }

                    var includedActivities = module.IncludeDiagnosticSourceActivities;
                    includedActivities.Add("Microsoft.Azure.EventHubs");
                    includedActivities.Add("Microsoft.Azure.ServiceBus");
                }
            });
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "services parameter is used in only NetStandard 2.0 build.")]
        private static void AddApplicationInsightsLoggerProvider(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddApplicationInsights();

                // The default behavior is to capture only logs above Warning level from all categories.
                // This can achieved with this code level filter -> loggingBuilder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>("",LogLevel.Warning);
                // However, this will make it impossible to override this behavior from Configuration like below using appsettings.json:
                // {
                //   "Logging": {
                //     "ApplicationInsights": {
                //       "LogLevel": {
                //         "": "Error"
                //       }
                //     }
                //   },
                //   ...
                // }
                // The reason is as both rules will match the filter, the last one added wins.
                // To ensure that the default filter is in the beginning of filter rules, so that user override from Configuration will always win,
                // we add code filter rule to the 0th position as below.
                loggingBuilder.Services.Configure<LoggerFilterOptions>(
                    options => options.Rules.Insert(
                        0,
                        new LoggerFilterRule(
                            "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider",
                            null,
                            LogLevel.Warning,
                            null)));
            });
        }
    }
}

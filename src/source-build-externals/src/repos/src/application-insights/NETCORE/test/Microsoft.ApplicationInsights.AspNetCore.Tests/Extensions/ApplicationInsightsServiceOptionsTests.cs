using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
#if NETCOREAPP
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
#endif
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.WindowsServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Test;

using Xunit;

namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Extensions
{
    /// <summary>
    /// Test class for <see cref="ApplicationInsightsServiceOptions"/>.
    /// </summary>
    /// <remarks>
    /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions. 
    /// <param name="configType">
    /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetryWorkerService() which reads IConfiguration from user application automatically.
    /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetryWorkerService(configuration) where IConfiguration object is supplied by caller.
    /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file. 
    /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it. 
    /// </param>
    /// <param name="isEnable">Sets the value for property EnableEventCounterCollectionModule.</param>
    /// </remarks>
    public class ApplicationInsightsServiceOptionsTests
    {
        private static IServiceProvider TestShim(string configType, bool isEnabled, Action<ApplicationInsightsServiceOptions, bool> testConfig, Action<IServiceCollection> servicesConfig = null)
        {
            // ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = null;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "content", "config-all-settings-" + isEnabled.ToString().ToLower() + ".json");

            if (configType == "Code")
            {
                filePath = null;

                // This will set the property defined in the test.
                serviceOptions = o => { testConfig(o, isEnabled); };
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsWorker(
                jsonPath: filePath,
                serviceOptions: serviceOptions,
                servicesConfig: servicesConfig,
                useDefaultConfig: configType == "DefaultConfiguration" ? true : false);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Get telemetry client to trigger TelemetryConfig setup.
            var tc = serviceProvider.GetService<TelemetryClient>();

            // Verify that Modules were added to DI.
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            Assert.NotNull(modules);

            return serviceProvider;
        }

        private static ServiceCollection CreateServicesAndAddApplicationinsightsWorker(string jsonPath, Action<ApplicationInsightsServiceOptions> serviceOptions = null, Action<IServiceCollection> servicesConfig = null, bool useDefaultConfig = true)
        {
            IConfigurationRoot config;
            var services = new ServiceCollection()
                .AddSingleton<IHostingEnvironment>(EnvironmentHelper.GetIHostingEnvironment())
                .AddSingleton<DiagnosticListener>(new DiagnosticListener("TestListener"));

            if (jsonPath != null)
            {
                var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), jsonPath);
                Console.WriteLine("json:" + jsonFullPath);
                try
                {
                    config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(jsonFullPath).Build();
                }
                catch (Exception)
                {
                    throw new Exception("Unable to build with json:" + jsonFullPath);
                }
            }
            else
            {
                var configBuilder = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), true)
                    .AddEnvironmentVariables();
                config = configBuilder.Build();
            }

            if (useDefaultConfig)
            {
                services.AddSingleton<IConfiguration>(config);
                services.AddApplicationInsightsTelemetry();
            }
            else
            {
                services.AddApplicationInsightsTelemetry(config);
            }

            servicesConfig?.Invoke(services);

            if (serviceOptions != null)
            {
                services.Configure(serviceOptions);
            }

            return (ServiceCollection)services;
        }

        /// <summary>
        /// User could enable or disable TelemetryConfiguration.Active by setting EnableAppServicesHeartbeatTelemetryModule.
        /// </summary>
        /// /// <summary>
        /// This SDK previously had a hidden dependency on TelemetryConfiguration.Active.
        /// We've removed that, but users may have taken a dependency on the former behavior.
        /// This test verifies that users can enable "backwards compat".
        /// Enabling this will copy the AspNetCore config to the TC.Active static instance.
        /// </summary>
        [Theory]

        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableTelemetryConfigurationActive(string configType, bool isEnable)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            // Dispose .Active to force a new .Active to be created during this test.
            TelemetryConfiguration.Active.Dispose();

            // IMPORTANT: This is the same ikey specified in the config files that will be used for this test.
            string testString = "22222222-2222-3333-4444-555555555555";
            var testTelemetryInitializer = new FakeTelemetryInitializer();

            IServiceProvider serviceProvider = TestShim(
                configType: configType,
                isEnabled: isEnable,
                testConfig: (o, b) =>
                {
                    o.EnableActiveTelemetryConfigurationSetup = b;
                    o.InstrumentationKey = testString;
                },
                servicesConfig: (services) => services.AddSingleton<ITelemetryInitializer>(testTelemetryInitializer)
                );

            // TelemetryConfiguration from DI should have custom set InstrumentationKey and TelemetryInitializer
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(testString, telemetryConfiguration.InstrumentationKey);
            Assert.Same(testTelemetryInitializer, telemetryConfiguration.TelemetryInitializers.OfType<FakeTelemetryInitializer>().Single());

            // TelemetryConfiguration.Active will only have custom set InstrumentationKey if .Active was enabled.
            Assert.Equal(testString.Equals(TelemetryConfiguration.Active.InstrumentationKey), isEnable);
            
            // TelemetryConfiguration.Active will only have custom TelemetryInitializer if .Active was enabled
            var activeTelemetryInitializer = TelemetryConfiguration.Active.TelemetryInitializers.OfType<FakeTelemetryInitializer>().SingleOrDefault();
            if (isEnable)
            {
                Assert.Same(testTelemetryInitializer, activeTelemetryInitializer);
            }
            else
            {
                Assert.Null(activeTelemetryInitializer);
            }

#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// User could enable or disable PerformanceCounterCollectionModule by setting EnablePerformanceCounterCollectionModule.
        /// </summary>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisablePerfCollectorModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnablePerformanceCounterCollectionModule = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<PerformanceCollectorModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }

#if NETCOREAPP
        /// <summary>
        /// User could enable or disable EventCounterCollectionModule by setting EnableEventCounterCollectionModule.
        /// </summary>
        [Theory]

        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]

        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableEventCounterCollectorModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableEventCounterCollectionModule = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<EventCounterCollectionModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }
#endif

        /// <summary>
        /// User could enable or disable DependencyTrackingTelemetryModule by setting EnableDependencyTrackingTelemetryModule.
        /// </summary>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableDependencyCollectorModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableDependencyTrackingTelemetryModule = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<DependencyTrackingTelemetryModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }

        /// <summary>
        /// User could enable or disable QuickPulseCollectorModule by setting EnableQuickPulseMetricStream.
        /// </summary>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableQuickPulseCollectorModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableQuickPulseMetricStream = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<QuickPulseTelemetryModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }

        /// <summary>
        /// User could enable or disable AzureInstanceMetadataModule by setting EnableAzureInstanceMetadataTelemetryModule.
        /// </summary>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableAzureInstanceMetadataModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableAzureInstanceMetadataTelemetryModule = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<AzureInstanceMetadataTelemetryModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }

        /// <summary>
        /// User could enable or disable RequestTrackingTelemetryModule by setting EnableRequestTrackingTelemetryModule.
        /// </summary>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableRequestCounterCollectorModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableRequestTrackingTelemetryModule = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<RequestTrackingTelemetryModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }

        /// <summary>
        /// User could enable or disable AppServiceHeartbeatModule by setting EnableAppServicesHeartbeatTelemetryModule.
        /// </summary>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableAppServiceHeartbeatModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableAppServicesHeartbeatTelemetryModule = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<AppServicesHeartbeatTelemetryModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }

        /// <summary>
        /// User could enable or disable <see cref="DiagnosticsTelemetryModule"/> by setting <see cref="ApplicationInsightsServiceOptions.EnableDiagnosticsTelemetryModule"/>.
        /// </summary>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableDiagnosticsTelemetryModule(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable, testConfig: (o, b) => o.EnableDiagnosticsTelemetryModule = b);

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<DiagnosticsTelemetryModule>().Single();
            Assert.Equal(isEnable, module.IsInitialized);
        }

        /// <summary>
        /// User could enable or disable the Heartbeat feature by setting <see cref="ApplicationInsightsServiceOptions.EnableHeartbeat"/>.
        /// </summary>
        /// <remarks>
        /// Config file tests are not valid in this test because they set ALL settings to either TRUE/FALSE.
        /// This test is specifically evaluating what happens when the DiagnosticsTelemetryModule is enabled, but the Heartbeat feature is disabled.
        /// </remarks>
        [Theory]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableHeartbeatFeature(string configType, bool isEnable)
        {
            IServiceProvider serviceProvider = TestShim(configType: configType, isEnabled: isEnable,
                testConfig: (o, b) => {
                    o.EnableDiagnosticsTelemetryModule = true;
                    o.EnableHeartbeat = b;
                });

            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var module = modules.OfType<DiagnosticsTelemetryModule>().Single();
            Assert.True(module.IsInitialized, "module was not initialized");
            Assert.Equal(isEnable, module.IsHeartbeatEnabled);
        }
    }
}
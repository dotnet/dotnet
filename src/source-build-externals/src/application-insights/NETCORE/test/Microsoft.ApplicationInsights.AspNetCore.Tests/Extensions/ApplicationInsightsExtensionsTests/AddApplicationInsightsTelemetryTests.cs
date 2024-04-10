using Xunit;

namespace Microsoft.Extensions.DependencyInjection.Test
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Logging;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.Logging;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.AspNetCore.Tests;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;
#if NETCOREAPP
    using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
#endif
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
    using Microsoft.ApplicationInsights.Extensibility.W3C;
    using Microsoft.ApplicationInsights.WindowsServer;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public class AddApplicationInsightsTelemetryTests : BaseTestClass
    {
        [Theory]
        [InlineData(typeof(ITelemetryInitializer), typeof(ApplicationInsights.AspNetCore.TelemetryInitializers.DomainNameRoleInstanceTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(AzureAppServiceRoleNameFromHostNameHeaderInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(ComponentVersionTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(ClientIpHeaderTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(OperationNameTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(SyntheticTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(WebSessionTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(WebUserTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(HttpDependenciesParsingTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryConfiguration), null, ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryClient), typeof(TelemetryClient), ServiceLifetime.Singleton)]
        public static void RegistersExpectedServices(Type serviceType, Type implementationType, ServiceLifetime lifecycle)
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null);
            ServiceDescriptor service = services.Single(s => s.ServiceType == serviceType && s.ImplementationType == implementationType);
            Assert.Equal(lifecycle, service.Lifetime);
        }

        [Theory]
        [InlineData(typeof(ITelemetryInitializer), typeof(ApplicationInsights.AspNetCore.TelemetryInitializers.DomainNameRoleInstanceTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(AzureAppServiceRoleNameFromHostNameHeaderInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(ComponentVersionTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(ClientIpHeaderTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(OperationNameTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(SyntheticTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(WebSessionTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(WebUserTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(ITelemetryInitializer), typeof(HttpDependenciesParsingTelemetryInitializer), ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryConfiguration), null, ServiceLifetime.Singleton)]
        [InlineData(typeof(TelemetryClient), typeof(TelemetryClient), ServiceLifetime.Singleton)]
        public static void RegistersExpectedServicesOnlyOnce(Type serviceType, Type implementationType, ServiceLifetime lifecycle)
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetry();
            ServiceDescriptor service = services.Single(s => s.ServiceType == serviceType && s.ImplementationType == implementationType);
            Assert.Equal(lifecycle, service.Lifetime);
        }

        [Fact]
        public static void DoesNotThrowWithoutInstrumentationKey()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null);
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatCreatesDefaultInstance()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Contains(telemetryConfiguration.TelemetryInitializers, t => t is OperationNameTelemetryInitializer);
        }

        /// <summary>
        /// Tests that the instrumentation key configuration can be read from a JSON file by the configuration factory.            
        /// </summary>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetry() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(false)]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsInstrumentationKeyFromConfiguration(bool useDefaultConfig)
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-instrumentation-key.json"), null, null, true, useDefaultConfig);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            if (useDefaultConfig)
            {
                Assert.Equal(InstrumentationKeyInAppSettings, telemetryConfiguration.InstrumentationKey);
            }
            else
            {
                Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            }
        }

        /// <summary>
        /// Tests that the connection string can be read from a JSON file by the configuration factory.            
        /// </summary>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetry() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Trait", "ConnectionString")]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsConnectionStringFromConfiguration(bool useDefaultConfig)
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-connection-string.json"), null, null, true, useDefaultConfig);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(TestConnectionString, telemetryConfiguration.ConnectionString);
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            Assert.Equal("http://127.0.0.1/", telemetryConfiguration.EndpointContainer.Ingestion.AbsoluteUri);
        }

        /// <summary>
        /// Tests that the connection string can be read from a JSON file by the configuration factory.            
        /// This config has both a connection string and an instrumentation key. It is expected to use the ikey from the connection string.
        /// </summary>
        [Fact]
        [Trait("Trait", "ConnectionString")]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsConnectionStringAndInstrumentationKeyFromConfiguration()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-connection-string-and-instrumentation-key.json"), null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(TestConnectionString, telemetryConfiguration.ConnectionString);
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            Assert.Equal("http://127.0.0.1/", telemetryConfiguration.EndpointContainer.Ingestion.AbsoluteUri);
        }

        /// <summary>
        /// Tests that the Active configuration singleton is updated, but another instance of telemetry configuration is created for dependency injection.
        /// ASP.NET Core developers should always use Dependency Injection instead of static singleton approach. 
        /// See Microsoft/ApplicationInsights-dotnet#613
        /// </summary>
        [Fact]
        public static void ConfigurationFactoryMethodUpdatesTheActiveConfigurationSingletonByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-instrumentation-key.json"), null, null, true, false);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            TelemetryConfiguration telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
        }

        /// <summary>
        /// We determine if Active telemetry needs to be configured based on the assumptions that 'default' configuration
        // created by base SDK has single preset ITelemetryInitializer. If it ever changes, change TelemetryConfigurationOptions.IsActiveConfigured method as well.
        /// </summary>
        [Fact]
        public static void DefaultTelemetryConfigurationHasOneTelemetryInitializer()
        {
            //
            var defaultConfig = TelemetryConfiguration.CreateDefault();
            Assert.Equal(1, defaultConfig.TelemetryInitializers.Count);
        }

        /// <summary>
        /// Tests that the developer mode can be read from a JSON file by the configuration factory.
        /// </summary>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetry() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsDeveloperModeFromConfiguration(bool useDefaultConfig)
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-developer-mode.json"), null, null, true, useDefaultConfig);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.True(telemetryConfiguration.TelemetryChannel.DeveloperMode);
        }

        /// <summary>
        /// Tests that the endpoint address can be read from a JSON file by the configuration factory.
        /// </summary>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetry() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(false)]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsEndpointAddressFromConfiguration(bool useDefaultConfig)
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(Path.Combine("content", "config-endpoint-address.json"), null, null, true, useDefaultConfig);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            if (useDefaultConfig)
            {
                // Endpoint comes from appSettings 
                Assert.Equal("http://hosthere/v2/track/", telemetryConfiguration.TelemetryChannel.EndpointAddress);
            }
            else
            {
                Assert.Equal("http://localhost:1234/v2/track/", telemetryConfiguration.TelemetryChannel.EndpointAddress);
            }
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsInstrumentationKeyFromEnvironment()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryChannel>(new InMemoryChannel());
            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", TestInstrumentationKey);
            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            try
            {
                services.AddApplicationInsightsTelemetry(config);

                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);
            }
        }

        /// <summary>
        /// Validates that while using services.AddApplicationInsightsTelemetry(); ikey is read from
        /// Environment
        /// </summary>
        [Fact]
        [Trait("Trait", "ConnectionString")]
        public static void AddApplicationInsightsTelemetry_ReadsConnectionString_FromEnvironment()
        {
            var services = GetServiceCollectionWithContextAccessor();
            Environment.SetEnvironmentVariable(ConnectionStringEnvironmentVariable, TestConnectionString);
            try
            {
                services.AddApplicationInsightsTelemetry();
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(TestConnectionString, telemetryConfiguration.ConnectionString);
                Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
                Assert.Equal("http://127.0.0.1/", telemetryConfiguration.EndpointContainer.Ingestion.AbsoluteUri);
            }
            finally
            {
                Environment.SetEnvironmentVariable(ConnectionStringEnvironmentVariable, null);
            }
        }

        /// <summary>
        /// Validates that while using services.AddApplicationInsightsTelemetry(); ikey is read from
        /// Environment
        /// </summary>
        [Fact]
        public static void AddApplicationInsightsTelemetryReadsInstrumentationKeyFromEnvironment()
        {
            var services = GetServiceCollectionWithContextAccessor();
            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", TestInstrumentationKey);
            try
            {
                services.AddApplicationInsightsTelemetry();
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);
            }
        }

        /// <summary>
        /// Validates that while using services.AddApplicationInsightsTelemetry(ikey), supplied ikey is
        /// used instead of one from Environment
        /// </summary>
        [Fact]
        public static void AddApplicationInsightsTelemetryDoesNotReadInstrumentationKeyFromEnvironmentIfSupplied()
        {
            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", TestInstrumentationKey);
            var services = GetServiceCollectionWithContextAccessor();
            string ikeyExpected = Guid.NewGuid().ToString();

            try
            {
                services.AddApplicationInsightsTelemetry(ikeyExpected);
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(ikeyExpected, telemetryConfiguration.InstrumentationKey);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);
            }
        }

        /// <summary>
        /// Validates that while using services.AddApplicationInsightsTelemetry(); reads ikey/other settings from
        /// appsettings.json
        /// </summary>
        [Fact]
        public static void AddApplicationInsightsTelemetryReadsInstrumentationKeyFromDefaultAppsettingsFile()
        {
            string ikeyExpected = Guid.NewGuid().ToString();
            string hostExpected = "http://ainewhost/v2/track/";
            string text = File.ReadAllText("appsettings.json");
            string originalText = text;
            try
            {
                text = text.Replace(InstrumentationKeyInAppSettings, ikeyExpected);
                text = text.Replace("http://hosthere/v2/track/", hostExpected);
                File.WriteAllText("appsettings.json", text);

                var services = GetServiceCollectionWithContextAccessor();
                services.AddApplicationInsightsTelemetry();
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(ikeyExpected, telemetryConfiguration.InstrumentationKey);
                Assert.Equal(hostExpected, telemetryConfiguration.TelemetryChannel.EndpointAddress);
            }
            finally
            {
                File.WriteAllText("appsettings.json", originalText);
            }
        }

        /// <summary>
        /// Validates that while using services.AddApplicationInsightsTelemetry(ikey), supplied ikey is
        /// used instead of one from appsettings.json
        /// </summary>
        [Fact]
        public static void AddApplicationInsightsTelemetryDoesNotReadInstrumentationKeyFromDefaultAppsettingsIfSupplied()
        {
            string suppliedIKey = "suppliedikey";
            string ikey = Guid.NewGuid().ToString();
            string text = File.ReadAllText("appsettings.json");
            try
            {
                text = text.Replace(InstrumentationKeyInAppSettings, ikey);
                File.WriteAllText("appsettings.json", text);

                var services = GetServiceCollectionWithContextAccessor();
                services.AddApplicationInsightsTelemetry(suppliedIKey);
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(suppliedIKey, telemetryConfiguration.InstrumentationKey);
            }
            finally
            {
                text = text.Replace(ikey, InstrumentationKeyInAppSettings);
                File.WriteAllText("appsettings.json", text);
            }
        }

        /// <summary>
        /// Validates that while using services.AddApplicationInsightsTelemetry(ApplicationInsightsServiceOptions), supplied ikey is
        /// used instead of one from appsettings.json
        /// </summary>
        [Fact]
        public static void AddApplicationInsightsTelemetryDoesNotReadInstrumentationKeyFromDefaultAppsettingsIfSuppliedViaOptions()
        {
            string suppliedIKey = "suppliedikey";
            var options = new ApplicationInsightsServiceOptions() { InstrumentationKey = suppliedIKey };
            string ikey = Guid.NewGuid().ToString();
            string text = File.ReadAllText("appsettings.json");
            try
            {
                text = text.Replace(InstrumentationKeyInAppSettings, ikey);
                File.WriteAllText("appsettings.json", text);

                var services = GetServiceCollectionWithContextAccessor();
                services.AddApplicationInsightsTelemetry(options);
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(suppliedIKey, telemetryConfiguration.InstrumentationKey);
            }
            finally
            {
                text = text.Replace(ikey, InstrumentationKeyInAppSettings);
                File.WriteAllText("appsettings.json", text);
            }
        }

        /// <summary>
        /// Validates that while using services.AddApplicationInsightsTelemetry(ApplicationInsightsServiceOptions), with null ikey
        /// and endpoint, ikey and endpoint from AppSettings.Json is NOT overwritten with the null/empty ones from
        /// ApplicationInsightsServiceOptions
        /// </summary>
        [Fact]
        public static void AddApplicationInsightsTelemetryDoesNotOverrideEmptyInstrumentationKeyFromAiOptions()
        {
            // Create new options, which will be default have null ikey and endpoint.
            var options = new ApplicationInsightsServiceOptions();
            string ikey = Guid.NewGuid().ToString();
            string text = File.ReadAllText("appsettings.json");
            try
            {
                text = text.Replace(InstrumentationKeyInAppSettings, ikey);
                text = text.Replace("hosthere", "newhost");
                File.WriteAllText("appsettings.json", text);

                var services = GetServiceCollectionWithContextAccessor();
                services.AddApplicationInsightsTelemetry(options);
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal(ikey, telemetryConfiguration.InstrumentationKey);
                Assert.Equal("http://newhost/v2/track/", telemetryConfiguration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
            }
            finally
            {
                text = text.Replace(ikey, InstrumentationKeyInAppSettings);
                text = text.Replace("newhost", "hosthere");
                File.WriteAllText("appsettings.json", text);
            }
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsDeveloperModeFromEnvironment()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryChannel>(new InMemoryChannel());
            Environment.SetEnvironmentVariable("APPINSIGHTS_DEVELOPER_MODE", "true");
            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            try
            {
                services.AddApplicationInsightsTelemetry(config);

                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.True(telemetryConfiguration.TelemetryChannel.DeveloperMode);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_DEVELOPER_MODE", null);
            }
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsEndpointAddressFromEnvironment()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryChannel>(new InMemoryChannel());
            Environment.SetEnvironmentVariable("APPINSIGHTS_ENDPOINTADDRESS", "http://localhost:1234/v2/track/");
            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            try
            {
                services.AddApplicationInsightsTelemetry(config);

                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
                Assert.Equal("http://localhost:1234/v2/track/", telemetryConfiguration.TelemetryChannel.EndpointAddress);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_ENDPOINTADDRESS", null);
            }
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesItWithTelemetryInitializersFromContainer()
        {
            var telemetryInitializer = new FakeTelemetryInitializer();
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryInitializer>(telemetryInitializer);
            services.AddSingleton<ITelemetryChannel>(new InMemoryChannel());

            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Contains(telemetryInitializer, telemetryConfiguration.TelemetryInitializers);
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesItWithTelemetryChannelFromContainer()
        {
            var telemetryChannel = new FakeTelemetryChannel();
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryChannel>(telemetryChannel);

            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Same(telemetryChannel, telemetryConfiguration.TelemetryChannel);
        }

        [Fact]
        public static void DoesNotOverrideDefaultTelemetryChannelIfTelemetryChannelServiceIsNotRegistered()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.NotNull(telemetryConfiguration.TelemetryChannel);
        }

        [Fact]
        public static void RegistersTelemetryClientToGetTelemetryConfigurationFromContainerAndNotGlobalInstance()
        {
            ITelemetry sentTelemetry = null;
            var telemetryChannel = new FakeTelemetryChannel { OnSend = telemetry => sentTelemetry = telemetry };
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetTelemetryConfiguration();
            configuration.InstrumentationKey = Guid.NewGuid().ToString();
            ITelemetryChannel oldChannel = configuration.TelemetryChannel;
            try
            {
                configuration.TelemetryChannel = telemetryChannel;

                var telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();
                telemetryClient.TrackEvent("myevent");

                // We want to check that configuration from container was used but configuration is a private field so we check instrumentation key instead.
                Assert.Equal(configuration.InstrumentationKey, sentTelemetry.Context.InstrumentationKey);
            }
            finally
            {
                configuration.TelemetryChannel = oldChannel;
            }
        }

        [Fact]
        public static void AddApplicationInsightsTelemetryDoesNotThrowOnNullServiceOptions()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
        }

        [Fact]
        public static void AppApplicationInsightsTelemetryFromApplicationInsightsServiceOptionsCopiesAllSettings()
        {
            ServiceCollection services = GetServiceCollectionWithContextAccessor();
            ApplicationInsightsServiceOptions options = new ApplicationInsightsServiceOptions()
            {
                ApplicationVersion = "test",
                DeveloperMode = true,
                EnableAdaptiveSampling = false,
                EnableAuthenticationTrackingJavaScript = false,
                EnableDebugLogger = true,
                EnableQuickPulseMetricStream = false,
                EndpointAddress = "http://test",
                EnableHeartbeat = false,
                InstrumentationKey = "test",
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000"
            };
            services.AddApplicationInsightsTelemetry(options);
            ApplicationInsightsServiceOptions servicesOptions = null;
            services.Configure((ApplicationInsightsServiceOptions o) =>
            {
                servicesOptions = o;
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            TelemetryConfiguration telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            Type optionsType = typeof(ApplicationInsightsServiceOptions);
            PropertyInfo[] properties = optionsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Assert.True(properties.Length > 0);
            foreach (PropertyInfo property in properties)
            {
                Assert.Equal(property.GetValue(options).ToString(), property.GetValue(servicesOptions).ToString());
            }
        }


        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesItWithModulesFromContainer()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null, null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            Assert.NotNull(modules);

#if NETCOREAPP
            // Developer Note: Expected modules:
            //      RequestTrackingTelemetryModule, PerformanceCollectorModule, AppServicesHeartbeatTelemetryModule, AzureInstanceMetadataTelemetryModule, 
            //      QuickPulseTelemetryModule, DiagnosticsTelemetryModule, DependencyTrackingTelemetryModule, EventCollectorCollectionModule
            Assert.Equal(8, modules.Count());
#else
            Assert.Equal(7, modules.Count());
#endif

            var perfCounterModule = modules.OfType<PerformanceCollectorModule>().Single();
            Assert.NotNull(perfCounterModule);

#if NETCOREAPP
            var eventCounterModule = modules.OfType<EventCounterCollectionModule>().Single();
            Assert.NotNull(eventCounterModule);
#endif


            var dependencyModuleDescriptor = modules.OfType<DependencyTrackingTelemetryModule>().Single();
            Assert.NotNull(dependencyModuleDescriptor);

            var reqModuleDescriptor = modules.OfType<RequestTrackingTelemetryModule>().Single();
            Assert.NotNull(reqModuleDescriptor);

            var appServiceHeartBeatModuleDescriptor = modules.OfType<AppServicesHeartbeatTelemetryModule>().Single();
            Assert.NotNull(appServiceHeartBeatModuleDescriptor);

            var azureMetadataHeartBeatModuleDescriptor = modules.OfType<AzureInstanceMetadataTelemetryModule>().Single();
            Assert.NotNull(azureMetadataHeartBeatModuleDescriptor);

            var quickPulseModuleDescriptor = modules.OfType<QuickPulseTelemetryModule>().Single();
            Assert.NotNull(quickPulseModuleDescriptor);
        }

#if NETCOREAPP
        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesEventCounterCollectorWithDefaultListOfCounters()
        {
            //ARRANGE
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null, null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var modules = serviceProvider.GetServices<ITelemetryModule>();

            //ACT

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var eventCounterModule = modules.OfType<EventCounterCollectionModule>().Single();

            // VALIDATE
            // By default, no counters are collected.
            Assert.Equal(0, eventCounterModule.Counters.Count);
        }
#endif


        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesDependencyCollectorWithDefaultValues()
        {
            //ARRANGE
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, null, null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var modules = serviceProvider.GetServices<ITelemetryModule>();

            //ACT

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var dependencyModule = modules.OfType<DependencyTrackingTelemetryModule>().Single();

            //VALIDATE
            Assert.Equal(4, dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Count);
            Assert.False(dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Contains("localhost"));
            Assert.False(dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Contains("127.0.0.1"));
        }

        /// <summary>
        /// User could enable or disable LegacyCorrelationHeadersInjection of DependencyCollectorOptions.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions. 
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetry() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file. 
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it. 
        /// </param>
        /// <param name="isEnable">Sets the value for property EnableLegacyCorrelationHeadersInjection.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesDependencyCollectorWithCustomValues(string configType, bool isEnable)
        {
            // ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = null;
            var filePath = Path.Combine("content", "config-req-dep-settings-" + isEnable.ToString().ToLower() + ".json");

            if (configType == "Code")
            {
                serviceOptions = o => { o.DependencyCollectionOptions.EnableLegacyCorrelationHeadersInjection = isEnable; };
                filePath = null;
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, serviceOptions, true, configType == "DefaultConfiguration" ? true : false);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var modules = serviceProvider.GetServices<ITelemetryModule>();

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            var dependencyModule = modules.OfType<DependencyTrackingTelemetryModule>().Single();
            // Get telemetry client to trigger TelemetryConfig setup.
            var tc = serviceProvider.GetService<TelemetryClient>();

            // VALIDATE
            Assert.Equal(isEnable ? 6 : 4, dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Count);
            Assert.Equal(isEnable, dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Contains("localhost") ? true : false);
            Assert.Equal(isEnable, dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Contains("127.0.0.1") ? true : false);
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatPopulatesItWithTelemetryProcessorFactoriesFromContainer()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddApplicationInsightsTelemetryProcessor<FakeTelemetryProcessor>();

            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            // TP added via AddApplicationInsightsTelemetryProcessor is added to the default sink.
            FakeTelemetryProcessor telemetryProcessor = telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors.OfType<FakeTelemetryProcessor>().FirstOrDefault();
            Assert.NotNull(telemetryProcessor);
            Assert.True(telemetryProcessor.IsInitialized);
        }

        [Fact]
        public static void AddApplicationInsightsTelemetryProcessorWithNullTelemetryProcessorTypeThrows()
        {
            var services = GetServiceCollectionWithContextAccessor();
            Assert.Throws<ArgumentNullException>(() => services.AddApplicationInsightsTelemetryProcessor(null));
        }

        [Fact]
        public static void AddApplicationInsightsTelemetryProcessorWithNonTelemetryProcessorTypeThrows()
        {
            var services = GetServiceCollectionWithContextAccessor();
            Assert.Throws<ArgumentException>(() => services.AddApplicationInsightsTelemetryProcessor(typeof(string)));
            Assert.Throws<ArgumentException>(() => services.AddApplicationInsightsTelemetryProcessor(typeof(ITelemetryProcessor)));
        }

        [Fact]
        public static void AddApplicationInsightsTelemetryProcessorWithImportingConstructor()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddApplicationInsightsTelemetryProcessor<FakeTelemetryProcessorWithImportingConstructor>();
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            // TP added via AddApplicationInsightsTelemetryProcessor is added to the default sink.
            FakeTelemetryProcessorWithImportingConstructor telemetryProcessor = telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors.OfType<FakeTelemetryProcessorWithImportingConstructor>().FirstOrDefault();
            Assert.NotNull(telemetryProcessor);
            Assert.Same(serviceProvider.GetService<IHostingEnvironment>(), telemetryProcessor.HostingEnvironment);
        }

        [Fact]
        public static void ConfigureApplicationInsightsTelemetryModuleWorks()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryModule, TestTelemetryModule>();

            //ACT
            services.ConfigureTelemetryModule<TestTelemetryModule>
            ((module, o) => module.CustomProperty = "mycustomvalue");
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var testTelemetryModule = modules.OfType<TestTelemetryModule>().Single();

            //The module should be initialized and configured as instructed.
            Assert.NotNull(testTelemetryModule);
            Assert.Equal("mycustomvalue", testTelemetryModule.CustomProperty);
            Assert.True(testTelemetryModule.IsInitialized);
        }

        [Fact]
        /// <summary>
        /// We've added the DiagnosticsTelemetryModule to the default TelemetryModules in AspNetCore DI.
        /// During setup, we expect this module to be discovered and set on the other Heartbeat TelemetryModules.
        /// </summary>
        public static void VerifyIfHeartbeatPropertyManagerSetOnOtherModules_Default()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();

            //ACT
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var count = modules.OfType<DiagnosticsTelemetryModule>().Count();
            Assert.Equal(1, count);

            var appServicesHeartbeatTelemetryModule = modules.OfType<AppServicesHeartbeatTelemetryModule>().Single();
            var hpm1 = appServicesHeartbeatTelemetryModule.HeartbeatPropertyManager;
            Assert.NotNull(hpm1);

            var azureInstanceMetadataTelemetryModule = modules.OfType<AzureInstanceMetadataTelemetryModule>().Single();
            var hpm2 = azureInstanceMetadataTelemetryModule.HeartbeatPropertyManager;
            Assert.NotNull(hpm2);

            Assert.Same(hpm1, hpm2);
        }

        [Fact]
        /// <summary>
        /// A user can configure an instance of DiagnosticsTelemetryModule.
        /// During setup, we expect this module to be discovered and set on the other Heartbeat TelemetryModules.
        /// </summary>
        public static void VerifyIfHeartbeatPropertyManagerSetOnOtherModules_UserDefinedInstance()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();

            // VERIFY THAT A USER CAN SPECIFY THEIR OWN INSTANCE
            var testValue = TimeSpan.FromDays(9);
            var diagnosticsTelemetryModule = new DiagnosticsTelemetryModule { HeartbeatInterval = testValue };
            services.AddSingleton<ITelemetryModule>(diagnosticsTelemetryModule);

            //ACT
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var count = modules.OfType<DiagnosticsTelemetryModule>().Count();
            Assert.Equal(1, count);

            var appServicesHeartbeatTelemetryModule = modules.OfType<AppServicesHeartbeatTelemetryModule>().Single();
            var hpm1 = appServicesHeartbeatTelemetryModule.HeartbeatPropertyManager;
            Assert.NotNull(hpm1);
            Assert.Same(diagnosticsTelemetryModule, hpm1);
            Assert.Equal(testValue, hpm1.HeartbeatInterval);

            var azureInstanceMetadataTelemetryModule = modules.OfType<AzureInstanceMetadataTelemetryModule>().Single();
            var hpm2 = azureInstanceMetadataTelemetryModule.HeartbeatPropertyManager;
            Assert.NotNull(hpm2);
            Assert.Same(diagnosticsTelemetryModule, hpm2);
            Assert.Equal(testValue, hpm2.HeartbeatInterval);
        }


        [Fact]
        /// <summary>
        /// A user can configure an instance of DiagnosticsTelemetryModule.
        /// During setup, we expect this module to be discovered and set on the other Heartbeat TelemetryModules.
        /// </summary>
        public static void VerifyIfHeartbeatPropertyManagerSetOnOtherModules_UserDefinedType()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();

            // VERIFY THAT A USER CAN SPECIFY THEIR OWN TYPE
            services.AddSingleton<ITelemetryModule, DiagnosticsTelemetryModule>();

            //act
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var count = modules.OfType<DiagnosticsTelemetryModule>().Count();
            Assert.Equal(1, count);

            var appServicesHeartbeatTelemetryModule = modules.OfType<AppServicesHeartbeatTelemetryModule>().Single();
            var hpm1 = appServicesHeartbeatTelemetryModule.HeartbeatPropertyManager;
            Assert.NotNull(hpm1);

            var azureInstanceMetadataTelemetryModule = modules.OfType<AzureInstanceMetadataTelemetryModule>().Single();
            var hpm2 = azureInstanceMetadataTelemetryModule.HeartbeatPropertyManager;
            Assert.NotNull(hpm2);

            Assert.Same(hpm1, hpm2);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        /// <summary>
        /// Previously we encouraged users to add the DiagnosticsTelemetryModule manually.
        /// Users could have added this either as an INSTANCE or as a TYPE.
        /// We don't want to add it a second time so need to confirm that we catch both cases.
        /// </summary>
        public static void TestingAddDiagnosticsTelemetryModule(bool manualAddInstance, bool manualAddType)
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();

            if (manualAddInstance)
            {
                services.AddSingleton<ITelemetryModule>(new DiagnosticsTelemetryModule());
            }
            else if (manualAddType)
            {
                services.AddSingleton<ITelemetryModule, DiagnosticsTelemetryModule>();
            }

            //ACT
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var count = modules.OfType<DiagnosticsTelemetryModule>().Count();

            Assert.Equal(1, count);
        }


        [Fact]
        public static void ConfigureApplicationInsightsTelemetryModuleWorksWithOptions()
        {
            //ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = options => options.ApplicationVersion = "123";
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryModule, TestTelemetryModule>();

            //ACT
            services.ConfigureTelemetryModule<TestTelemetryModule>
                ((module, o) => module.CustomProperty = o.ApplicationVersion);
            services.AddApplicationInsightsTelemetry(serviceOptions);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var testTelemetryModule = modules.OfType<TestTelemetryModule>().Single();

            //The module should be initialized and configured as instructed.
            Assert.NotNull(testTelemetryModule);
            Assert.Equal("123", testTelemetryModule.CustomProperty);
            Assert.True(testTelemetryModule.IsInitialized);
        }

        [Fact]
        public static void ConfigureApplicationInsightsTelemetryModuleWorksWithoutOptions()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryModule, TestTelemetryModule>();

            //ACT
            services.ConfigureTelemetryModule<TestTelemetryModule>
                ((module, o) => module.CustomProperty = "mycustomproperty");

            services.AddApplicationInsightsTelemetry();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var testTelemetryModule = modules.OfType<TestTelemetryModule>().Single();

            //The module should be initialized and configured as instructed.
            Assert.NotNull(testTelemetryModule);
            Assert.Equal("mycustomproperty", testTelemetryModule.CustomProperty);
            Assert.True(testTelemetryModule.IsInitialized);
        }

        [Fact]
        public static void ConfigureRequestTrackingTelemetryDefaultOptions()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();

            //ACT
            services.AddApplicationInsightsTelemetry();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var requestTrackingModule = (RequestTrackingTelemetryModule)serviceProvider.GetServices<ITelemetryModule>().FirstOrDefault(x => x.GetType()
                                                                                                            == typeof(RequestTrackingTelemetryModule));

            Assert.True(requestTrackingModule.CollectionOptions.InjectResponseHeaders);
            Assert.False(requestTrackingModule.CollectionOptions.TrackExceptions);
        }

        /// <summary>
        /// User could enable or disable RequestCollectionOptions by setting InjectResponseHeaders, TrackExceptions and EnableW3CDistributedTracing.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions. 
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetry() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file. 
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it. 
        /// </param>
        /// <param name="isEnable">Sets the value for property InjectResponseHeaders, TrackExceptions and EnableW3CDistributedTracing.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void ConfigureRequestTrackingTelemetryCustomOptions(string configType, bool isEnable)
        {
            // ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = null;
            var filePath = Path.Combine("content", "config-req-dep-settings-" + isEnable.ToString().ToLower() + ".json");

            if (configType == "Code")
            {
                serviceOptions = o =>
                {
                    o.RequestCollectionOptions.InjectResponseHeaders = isEnable;
                    o.RequestCollectionOptions.TrackExceptions = isEnable;
                    // o.RequestCollectionOptions.EnableW3CDistributedTracing = isEnable; // Obsolete
                };
                filePath = null;
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, serviceOptions, true, configType == "DefaultConfiguration" ? true : false);

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            var requestTrackingModule = (RequestTrackingTelemetryModule)serviceProvider
                .GetServices<ITelemetryModule>().FirstOrDefault(x => x.GetType() == typeof(RequestTrackingTelemetryModule));

            Assert.Equal(isEnable, requestTrackingModule.CollectionOptions.InjectResponseHeaders);
            Assert.Equal(isEnable, requestTrackingModule.CollectionOptions.TrackExceptions);
            // Assert.Equal(isEnable, requestTrackingModule.CollectionOptions.EnableW3CDistributedTracing); // Obsolete
        }

        [Fact]
        public static void ConfigureApplicationInsightsTelemetryModuleThrowsIfConfigureIsNull()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryModule, TestTelemetryModule>();

            //ACT and VALIDATE
            Assert.Throws<ArgumentNullException>(() => services.ConfigureTelemetryModule<TestTelemetryModule>((Action<TestTelemetryModule, ApplicationInsightsServiceOptions>)null));

#pragma warning disable CS0618 // Type or member is obsolete
            Assert.Throws<ArgumentNullException>(() => services.ConfigureTelemetryModule<TestTelemetryModule>((Action<TestTelemetryModule>)null));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Fact]
        public static void ConfigureApplicationInsightsTelemetryModuleDoesNotThrowIfModuleNotFound()
        {
            //ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            // Intentionally NOT adding the module
            // services.AddSingleton<ITelemetryModule, TestTelemetryModule>();

            //ACT
            services.ConfigureTelemetryModule<TestTelemetryModule>
            ((module, options) => module.CustomProperty = "mycustomvalue");
            services.AddApplicationInsightsTelemetry(new ConfigurationBuilder().Build());
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Requesting TelemetryConfiguration from services trigger constructing the TelemetryConfiguration
            // which in turn trigger configuration of all modules.
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            //VALIDATE
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            var testTelemetryModule = modules.OfType<TestTelemetryModule>().FirstOrDefault();

            // No exceptions thrown here.
            Assert.Null(testTelemetryModule);
        }

        [Fact]
        public static void AddsAddaptiveSamplingServiceToTheConfigurationByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var adaptiveSamplingProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<AdaptiveSamplingTelemetryProcessor>(telemetryConfiguration);

            // There will be 2 separate SamplingTelemetryProcessors - one for Events, and other for everything else.
            Assert.Equal(2, adaptiveSamplingProcessorCount);
        }

        /// <summary>
        /// User could enable or disable sampling by setting EnableAdaptiveSampling.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions. 
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetry() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file. 
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it. 
        /// </param>
        /// <param name="isEnable">Sets the value for property EnableAdaptiveSampling.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void DoesNotAddSamplingToConfigurationIfExplicitlyControlledThroughParameter(string configType, bool isEnable)
        {
            // ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = null;
            var filePath = Path.Combine("content", "config-all-settings-" + isEnable.ToString().ToLower() + ".json");

            if (configType == "Code")
            {
                serviceOptions = o => { o.EnableAdaptiveSampling = isEnable; };
                filePath = null;
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, serviceOptions, true, configType == "DefaultConfiguration" ? true : false);

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var qpProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<AdaptiveSamplingTelemetryProcessor>(telemetryConfiguration);
            // There will be 2 separate SamplingTelemetryProcessors - one for Events, and other for everything else.
            Assert.Equal(isEnable ? 2 : 0, qpProcessorCount);
        }

        [Fact]
        public static void AddsAddaptiveSamplingServiceToTheConfigurationWithServiceOptions()
        {
            Action<ApplicationInsightsServiceOptions> serviceOptions = options => options.EnableAdaptiveSampling = true;
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", serviceOptions, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var adaptiveSamplingProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<AdaptiveSamplingTelemetryProcessor>(telemetryConfiguration);
            // There will be 2 separate SamplingTelemetryProcessors - one for Events, and other for everything else.
            Assert.Equal(2, adaptiveSamplingProcessorCount);
        }

        [Fact]
        public static void AddsServerTelemetryChannelByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(typeof(ServerTelemetryChannel), telemetryConfiguration.TelemetryChannel.GetType());
        }

        [Fact]
        [Trait("Trait", "ConnectionString")]
        public static void AddApplicationInsightsSettings_SetsConnectionString()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryChannel>(new InMemoryChannel());
            var config = new ConfigurationBuilder().AddApplicationInsightsSettings(connectionString: TestConnectionString).Build();
            services.AddApplicationInsightsTelemetry(config);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(TestConnectionString, telemetryConfiguration.ConnectionString);
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
            Assert.Equal("http://127.0.0.1/", telemetryConfiguration.EndpointContainer.Ingestion.AbsoluteUri);
        }

        [Fact]
        [Trait("Trait", "Endpoints")]
        public static void DoesNotOverWriteExistingChannel()
        {
            var testEndpoint = "http://localhost:1234/v2/track/";

            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryChannel, InMemoryChannel>();
            var config = new ConfigurationBuilder().AddApplicationInsightsSettings(endpointAddress: testEndpoint).Build();
            services.AddApplicationInsightsTelemetry(config);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(typeof(InMemoryChannel), telemetryConfiguration.TelemetryChannel.GetType());
            Assert.Equal(testEndpoint, telemetryConfiguration.TelemetryChannel.EndpointAddress);
        }

        [Fact]
        public static void FallbacktoDefaultChannelWhenNoChannelFoundInDI()
        {
            var testEndpoint = "http://localhost:1234/v2/track/";

            // ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            var config = new ConfigurationBuilder().AddApplicationInsightsSettings(endpointAddress: testEndpoint).Build();
            services.AddApplicationInsightsTelemetry(config);

            // Remove all ITelemetryChannel to simulate scenario where customer remove all channel from DI but forgot to add new one.
            // This should not crash application startup, and should fall back to default channel supplied by base sdk.
            for (var i = services.Count - 1; i >= 0; i--)
            {
                var descriptor = services[i];
                if (descriptor.ServiceType == typeof(ITelemetryChannel))
                {
                    services.RemoveAt(i);
                }
            }

            // VERIFY that default channel is configured when nothing is present in DI
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(typeof(InMemoryChannel), telemetryConfiguration.TelemetryChannel.GetType());
            Assert.Equal(testEndpoint, telemetryConfiguration.TelemetryChannel.EndpointAddress);
        }

        [Fact]
        public static void VerifyNoExceptionWhenAppIdProviderNotFoundInDI()
        {
            // ARRANGE
            var services = GetServiceCollectionWithContextAccessor();
            var config = new ConfigurationBuilder().AddApplicationInsightsSettings(endpointAddress: "http://localhost:1234/v2/track/").Build();
            services.AddApplicationInsightsTelemetry(config);

            for (var i = services.Count - 1; i >= 0; i--)
            {
                var descriptor = services[i];
                if (descriptor.ServiceType == typeof(IApplicationIdProvider))
                {
                    services.RemoveAt(i);
                }
            }

            // ACT
            IServiceProvider serviceProvider = services.BuildServiceProvider();


            // VERIFY
            var requestTrackingModule = serviceProvider.GetServices<ITelemetryModule>().FirstOrDefault(x => x.GetType()
                == typeof(RequestTrackingTelemetryModule));

            Assert.NotNull(requestTrackingModule); // this verifies the instance was created without exception
        }

        [Fact]
        public static void VerifyUserCanOverrideAppIdProvider()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<IApplicationIdProvider, MockApplicationIdProvider>(); // assume user tries to define own implementation
            var config = new ConfigurationBuilder().AddApplicationInsightsSettings(endpointAddress: "http://localhost:1234/v2/track/").Build();
            services.AddApplicationInsightsTelemetry(config);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var applicationIdProvider = serviceProvider.GetRequiredService<IApplicationIdProvider>();

            Assert.Equal(typeof(MockApplicationIdProvider), applicationIdProvider.GetType());
        }


        [Fact]
        public static void ValidatesThatOnlyPassThroughProcessorIsAddedToCommonPipeline()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            services.AddSingleton<ITelemetryChannel, InMemoryChannel>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

            // All default TelemetryProcessors are expected to be on the default sink. There should be
            // none on the main pipeline except the PassThrough.

            var qpProcessorCount = GetTelemetryProcessorsCountInConfiguration<QuickPulseTelemetryProcessor>(telemetryConfiguration);
            Assert.Equal(0, qpProcessorCount);

            var metricExtractorProcessorCount = GetTelemetryProcessorsCountInConfiguration<AutocollectedMetricsExtractor>(telemetryConfiguration);
            Assert.Equal(0, metricExtractorProcessorCount);

            var samplingProcessorCount = GetTelemetryProcessorsCountInConfiguration<AdaptiveSamplingTelemetryProcessor>(telemetryConfiguration);
            Assert.Equal(0, samplingProcessorCount);

            var passThroughProcessorCount = telemetryConfiguration.TelemetryProcessors.Count;
            Assert.Equal(1, passThroughProcessorCount);

            Assert.Equal("PassThroughProcessor", telemetryConfiguration.TelemetryProcessors[0].GetType().Name);
        }

        [Fact]
        public static void AddsQuickPulseProcessorToTheConfigurationByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            services.AddSingleton<ITelemetryChannel, InMemoryChannel>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var qpProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<QuickPulseTelemetryProcessor>(telemetryConfiguration);
            Assert.Equal(1, qpProcessorCount);
        }

        [Fact]
        public static void AddsAutoCollectedMetricsExtractorProcessorToTheConfigurationByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            services.AddSingleton<ITelemetryChannel, InMemoryChannel>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var metricExtractorProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<AutocollectedMetricsExtractor>(telemetryConfiguration);
            Assert.Equal(1, metricExtractorProcessorCount);
        }

        /// <summary>
        /// User could enable or disable auto collected metrics by setting AddAutoCollectedMetricExtractor.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions. 
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetry() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file. 
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it. 
        /// </param>
        /// <param name="isEnable">Sets the value for property AddAutoCollectedMetricExtractor.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void DoesNotAddAutoCollectedMetricsExtractorToConfigurationIfExplicitlyControlledThroughParameter(string configType, bool isEnable)
        {
            // ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = null;
            var filePath = Path.Combine("content", "config-all-settings-" + isEnable.ToString().ToLower() + ".json");

            if (configType == "Code")
            {
                serviceOptions = o => { o.AddAutoCollectedMetricExtractor = isEnable; };
                filePath = null;
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, serviceOptions, true, configType == "DefaultConfiguration" ? true : false);

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var metricExtractorProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<AutocollectedMetricsExtractor>(telemetryConfiguration);
            Assert.Equal(isEnable ? 1 : 0, metricExtractorProcessorCount);
        }

        [Fact]
        public static void DoesNotAddQuickPulseProcessorToConfigurationIfExplicitlyControlledThroughParameter()
        {
            Action<ApplicationInsightsServiceOptions> serviceOptions = options => options.EnableQuickPulseMetricStream = false;
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", serviceOptions, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var qpProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<QuickPulseTelemetryProcessor>(telemetryConfiguration);
            Assert.Equal(0, qpProcessorCount);
        }

        /// <summary>
        /// User could enable or disable AuthenticationTrackingJavaScript by setting EnableAuthenticationTrackingJavaScript.
        /// This configuration can be read from a JSON file by the configuration factory or through code by passing ApplicationInsightsServiceOptions. 
        /// </summary>
        /// <param name="configType">
        /// DefaultConfiguration - calls services.AddApplicationInsightsTelemetry() which reads IConfiguration from user application automatically.
        /// SuppliedConfiguration - invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// Code - Caller creates an instance of ApplicationInsightsServiceOptions and passes it. This option overrides all configuration being used in JSON file. 
        /// There is a special case where NULL values in these properties - InstrumentationKey, ConnectionString, EndpointAddress and DeveloperMode are overwritten. We check IConfiguration object to see if these properties have values, if values are present then we override it. 
        /// </param>
        /// <param name="isEnable">Sets the value for property EnableAuthenticationTrackingJavaScript.</param>
        [Theory]
        [InlineData("DefaultConfiguration", true)]
        [InlineData("DefaultConfiguration", false)]
        [InlineData("SuppliedConfiguration", true)]
        [InlineData("SuppliedConfiguration", false)]
        [InlineData("Code", true)]
        [InlineData("Code", false)]
        public static void UserCanEnableAndDisableAuthenticationTrackingJavaScript(string configType, bool isEnable)
        {
            // ARRANGE
            Action<ApplicationInsightsServiceOptions> serviceOptions = null;
            var filePath = Path.Combine("content", "config-all-settings-" + isEnable.ToString().ToLower() + ".json");

            if (configType == "Code")
            {
                serviceOptions = o => { o.EnableAuthenticationTrackingJavaScript = isEnable; };
                filePath = null;
            }

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, serviceOptions, true, configType == "DefaultConfiguration" ? true : false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // VALIDATE
            // Get telemetry client to trigger TelemetryConfig setup.
            var tc = serviceProvider.GetService<TelemetryClient>();

            Type javaScriptSnippetType = typeof(JavaScriptSnippet);
            var javaScriptSnippet = serviceProvider.GetService<JavaScriptSnippet>();
            // Get the JavaScriptSnippet private field value for enableAuthSnippet.
            FieldInfo enableAuthSnippetField = javaScriptSnippetType.GetField("enableAuthSnippet", BindingFlags.NonPublic | BindingFlags.Instance);
            // JavaScriptSnippet.enableAuthSnippet is set to true when EnableAuthenticationTrackingJavaScript is enabled, else it is set to false.
            Assert.Equal(isEnable, (bool)enableAuthSnippetField.GetValue(javaScriptSnippet));
        }

        [Fact]
        public static void AddsQuickPulseProcessorToTheConfigurationWithServiceOptions()
        {
            Action<ApplicationInsightsServiceOptions> serviceOptions = options => options.EnableQuickPulseMetricStream = true;
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", serviceOptions, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var qpProcessorCount = GetTelemetryProcessorsCountInConfigurationDefaultSink<QuickPulseTelemetryProcessor>(telemetryConfiguration);
            Assert.Equal(1, qpProcessorCount);
        }

        [Fact]
        public static void AddsHeartbeatModulesToTheConfigurationByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/", null, false);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            var modules = serviceProvider.GetServices<ITelemetryModule>();
            Assert.NotNull(modules.OfType<AppServicesHeartbeatTelemetryModule>().Single());
            Assert.NotNull(modules.OfType<AzureInstanceMetadataTelemetryModule>().Single());
        }

        [Fact]
        public static void W3CIsEnabledByDefault()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/");
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();

#pragma warning disable CS0618 // Type or member is obsolete
            Assert.DoesNotContain(telemetryConfiguration.TelemetryInitializers, t => t is W3COperationCorrelationTelemetryInitializer);
#pragma warning restore CS0618 // Type or member is obsolete

            var modules = serviceProvider.GetServices<ITelemetryModule>().ToList();

            var requestTracking = modules.OfType<RequestTrackingTelemetryModule>().ToList();
            var dependencyTracking = modules.OfType<DependencyTrackingTelemetryModule>().ToList();
            Assert.Single(requestTracking);
            Assert.Single(dependencyTracking);

            Assert.True(Activity.DefaultIdFormat == ActivityIdFormat.W3C);
            Assert.True(Activity.ForceDefaultIdFormat);
        }

        private static int GetTelemetryProcessorsCountInConfiguration<T>(TelemetryConfiguration telemetryConfiguration)
        {
            return telemetryConfiguration.TelemetryProcessors.Where(processor => processor.GetType() == typeof(T)).Count();
        }

        private static int GetTelemetryProcessorsCountInConfigurationDefaultSink<T>(TelemetryConfiguration telemetryConfiguration)
        {
            return telemetryConfiguration.DefaultTelemetrySink.TelemetryProcessors.Where(processor => processor.GetType() == typeof(T)).Count();
        }

        [Fact]
        public static void LoggerCallbackIsInvoked()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ApplicationInsightsLoggerEvents>();
            var serviceProvider = services.BuildServiceProvider();

            var loggerProvider = new MockLoggingFactory();

            bool firstLoggerCallback = false;
            bool secondLoggerCallback = false;

#pragma warning disable CS0618 // Type or member is obsolete
            loggerProvider.AddApplicationInsights(serviceProvider, (s, level) => true, () => firstLoggerCallback = true);
            loggerProvider.AddApplicationInsights(serviceProvider, (s, level) => true, () => secondLoggerCallback = true);
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.True(firstLoggerCallback);
            Assert.False(secondLoggerCallback);
        }

        [Fact]
        public static void NullLoggerCallbackAlowed()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ApplicationInsightsLoggerEvents>();
            var serviceProvider = services.BuildServiceProvider();

            var loggerProvider = new MockLoggingFactory();

#pragma warning disable CS0618 // Type or member is obsolete
            loggerProvider.AddApplicationInsights(serviceProvider, (s, level) => true, null);
            loggerProvider.AddApplicationInsights(serviceProvider, (s, level) => true, null);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Creates two copies of ApplicationInsightsServiceOptions. First object is created by calling services.AddApplicationInsightsTelemetry() or services.AddApplicationInsightsTelemetry(config).
        /// Second object is created directly from configuration file without using any of SDK functionality.
        /// Compares ApplicationInsightsServiceOptions object from dependency container and one created directly from configuration. 
        /// This proves all that SDK read configuration successfully from configuration file. 
        /// Properties from appSettings.json, appsettings.{env.EnvironmentName}.json and Environmental Variables are read if no IConfiguration is supplied or used in an application.
        /// </summary>
        /// <param name="readFromAppSettings">If this is set, read value from appsettings.json, else from passed file.</param>
        /// <param name="useDefaultConfig">
        /// Calls services.AddApplicationInsightsTelemetry() when the value is true and reads IConfiguration from user application automatically.
        /// Else, it invokes services.AddApplicationInsightsTelemetry(configuration) where IConfiguration object is supplied by caller.
        /// </param>
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public static void ReadsSettingsFromDefaultAndSuppliedConfiguration(bool readFromAppSettings, bool useDefaultConfig)
        {
            // ARRANGE
            IConfigurationBuilder configBuilder = null;
            var fileName = "config-all-default.json";

            // ACT
            var services = CreateServicesAndAddApplicationinsightsTelemetry(
                readFromAppSettings ? null : Path.Combine("content", fileName),
                null, null, true, useDefaultConfig);

            // VALIDATE

            // Generate config and don't pass to services
            // this is directly generated from config file 
            // which could be used to validate the data from dependency container

            if (!readFromAppSettings)
            {
                configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(Path.Combine("content", fileName));
                if (useDefaultConfig)
                {
                    configBuilder.AddJsonFile("appsettings.json", false);
                }
            }
            else
            {
                configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false);
            }

            var config = configBuilder.Build();

            // Compare ApplicationInsightsServiceOptions from dependency container and configuration
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            // ApplicationInsightsServiceOptions from dependency container
            var servicesOptions = serviceProvider.GetRequiredService<IOptions<ApplicationInsightsServiceOptions>>().Value;

            // Create ApplicationInsightsServiceOptions from configuration for validation.
            var aiOptions = new ApplicationInsightsServiceOptions();
            config.GetSection("ApplicationInsights").Bind(aiOptions);
            config.GetSection("ApplicationInsights:TelemetryChannel").Bind(aiOptions);

            Type optionsType = typeof(ApplicationInsightsServiceOptions);
            PropertyInfo[] properties = optionsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Assert.True(properties.Length > 0);
            foreach (PropertyInfo property in properties)
            {
                Assert.Equal(property.GetValue(aiOptions)?.ToString(), property.GetValue(servicesOptions)?.ToString());
            }
        }

        [Fact]
        public static void ReadsSettingsFromDefaultConfigurationWithEnvOverridingConfig()
        {
            // Host.CreateDefaultBuilder() in .NET Core 3.0  adds appsetting.json and env variable
            // to configuration and is made available for constructor injection.
            // this test validates that SDK reads settings from this configuration by default
            // and gives priority to the ENV variables than the one from config.

            // ARRANGE
            Environment.SetEnvironmentVariable(InstrumentationKeyEnvironmentVariable, TestInstrumentationKey);
            Environment.SetEnvironmentVariable(ConnectionStringEnvironmentVariable, TestConnectionString);
            Environment.SetEnvironmentVariable(TestEndPointEnvironmentVariable, TestEndPoint);
            Environment.SetEnvironmentVariable(DeveloperModeEnvironmentVariable, "true");

            try
            {
                var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), "content", "config-all-default.json");

                // This config will have ikey,endpoint from json and env. ENV one is expected to win.
                var config = new ConfigurationBuilder().AddJsonFile(jsonFullPath).AddEnvironmentVariables().Build();
                var services = GetServiceCollectionWithContextAccessor();

                // This line mimics the default behavior by CreateDefaultBuilder
                services.AddSingleton<IConfiguration>(config);

                // ACT             
                services.AddApplicationInsightsTelemetry();

                // VALIDATE
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
                Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
                Assert.Equal(TestConnectionString, telemetryConfiguration.ConnectionString);
                Assert.Equal(TestEndPoint, telemetryConfiguration.TelemetryChannel.EndpointAddress);
                Assert.True(telemetryConfiguration.TelemetryChannel.DeveloperMode);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);
                Environment.SetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING", null);
                Environment.SetEnvironmentVariable("APPINSIGHTS_ENDPOINTADDRESS", null);
                Environment.SetEnvironmentVariable("APPINSIGHTS_DEVELOPER_MODE", null);
            }
        }

        [Fact]
        public static void VerifiesIkeyProvidedInAddApplicationInsightsAlwaysWinsOverOtherOptions()
        {
            // ARRANGE
            Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", TestInstrumentationKey);
            try
            {
                var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), "content", "config-instrumentation-key.json");

                // This config will have ikey,endpoint from json and env. But the one
                // user explicitly provider is expected to win.
                var config = new ConfigurationBuilder().AddJsonFile(jsonFullPath).AddEnvironmentVariables().Build();
                var services = GetServiceCollectionWithContextAccessor();

                // This line mimics the default behavior by CreateDefaultBuilder
                services.AddSingleton<IConfiguration>(config);

                // ACT             
                services.AddApplicationInsightsTelemetry("userkey");

                // VALIDATE
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
                Assert.Equal("userkey", telemetryConfiguration.InstrumentationKey);
            }
            finally
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", null);
            }
        }

        [Fact]
        public static void VerifiesIkeyProvidedInAppSettingsWinsOverOtherConfigurationOptions()
        {
            // ARRANGE
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "content", "config-instrumentation-key.json");

            // ACT
            // Calls services.AddApplicationInsightsTelemetry(), which by default reads from appSettings.json
            var services = CreateServicesAndAddApplicationinsightsTelemetry(filePath, null, null, true, true);

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
            Assert.Equal(InstrumentationKeyInAppSettings, telemetryConfiguration.InstrumentationKey);
        }

        [Fact]
        public static void ReadsFromAppSettingsIfNoSettingsFoundInDefaultConfiguration()
        {
            // Host.CreateDefaultBuilder() in .NET Core 3.0  adds appsetting.json and env variable
            // to configuration and is made available for constructor injection.
            // This test validates that SDK does not throw any error if it cannot find 
            // application insights configuration in default IConfiguration.
            // ARRANGE
            var jsonFullPath = Path.Combine(Directory.GetCurrentDirectory(), "content", "sample-appsettings_dontexist.json");
            var config = new ConfigurationBuilder().AddJsonFile(jsonFullPath, true).Build();
            var services = GetServiceCollectionWithContextAccessor();
            // This line mimics the default behavior by CreateDefaultBuilder
            services.AddSingleton<IConfiguration>(config);

            // ACT             
            services.AddApplicationInsightsTelemetry();

            // VALIDATE
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetRequiredService<TelemetryConfiguration>();
            // Create a configuration from appSettings.json for validation.
            var appSettingsConfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false).Build();

            Assert.Equal(appSettingsConfig["ApplicationInsights:InstrumentationKey"], telemetryConfiguration.InstrumentationKey);
        }
    }
}

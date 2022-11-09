using Xunit;

namespace Microsoft.Extensions.DependencyInjection.Test
{
    using System;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;

    using Microsoft.Extensions.Configuration;

    public class AddApplicationInsightsSettings : BaseTestClass
    {
        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsInstrumentationKeyFromSettings()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryChannel>(new InMemoryChannel());
            var config = new ConfigurationBuilder().AddApplicationInsightsSettings(instrumentationKey: TestInstrumentationKey).Build();
            services.AddApplicationInsightsTelemetry(config);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal(TestInstrumentationKey, telemetryConfiguration.InstrumentationKey);
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsDeveloperModeFromSettings()
        {
            var services = GetServiceCollectionWithContextAccessor();
            services.AddSingleton<ITelemetryChannel>(new InMemoryChannel());
            var config = new ConfigurationBuilder().AddApplicationInsightsSettings(developerMode: true).Build();
            services.AddApplicationInsightsTelemetry(config);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.True(telemetryConfiguration.TelemetryChannel.DeveloperMode);
        }

        [Fact]
        public static void RegistersTelemetryConfigurationFactoryMethodThatReadsEndpointAddressFromSettings()
        {
            var services = CreateServicesAndAddApplicationinsightsTelemetry(null, "http://localhost:1234/v2/track/");
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var telemetryConfiguration = serviceProvider.GetTelemetryConfiguration();
            Assert.Equal("http://localhost:1234/v2/track/", telemetryConfiguration.TelemetryChannel.EndpointAddress);
        }

        /// <summary>
        /// Sanity check to validate that node name and roleinstance are populated
        /// </summary>
        [Fact]
        public static void SanityCheckRoleInstance()
        {
            // ARRANGE
            string expected = Environment.MachineName;
            var services = GetServiceCollectionWithContextAccessor();
            services.AddApplicationInsightsTelemetry();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Request TC from DI which would be made with the default TelemetryConfiguration which should 
            // contain the telemetry initializer capable of populate node name and role instance name.
            var tc = serviceProvider.GetRequiredService<TelemetryClient>();
            var mockItem = new EventTelemetry();

            // ACT
            // This is expected to run all TI and populate the node name and role instance.
            tc.Initialize(mockItem);

            // VERIFY                
            Assert.Contains(expected, mockItem.Context.Cloud.RoleInstance, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}

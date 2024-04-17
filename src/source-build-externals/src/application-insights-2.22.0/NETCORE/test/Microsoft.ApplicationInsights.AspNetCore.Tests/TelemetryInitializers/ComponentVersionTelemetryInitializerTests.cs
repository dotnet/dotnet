using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.ApplicationInsights.AspNetCore.Tests.TelemetryInitializers
{
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.DataContracts;
    using Xunit;

    public class ComponentVersionTelemetryInitializerTests
    {

        [Fact]
        public void InitializeDoesNotThrowIfHttpContextIsUnavailable()
        {
            var initializer = new ComponentVersionTelemetryInitializer(this.BuildConfigurationWithVersion());
            initializer.Initialize(new RequestTelemetry());
        }

        [Fact]
        public void InitializeDoesNotThrowIfRequestTelemetryIsUnavailable()
        {
            var initializer = new ComponentVersionTelemetryInitializer(this.BuildConfigurationWithVersion());
            initializer.Initialize(new RequestTelemetry());
        }

        [Fact]
        public void InitializeAssignsVersionToTelemetry()
        {
            var initializer = new ComponentVersionTelemetryInitializer(this.BuildConfigurationWithVersion());
            var telemetry = new RequestTelemetry();
            initializer.Initialize(telemetry);
            Assert.NotNull(telemetry.Context.Component.Version);
        }

        [Fact]
        public void InitializeDoesNotOverrideExistingVersion()
        {
            var initializer = new ComponentVersionTelemetryInitializer(this.BuildConfigurationWithVersion());
            var telemetry = new RequestTelemetry();
            telemetry.Context.Component.Version = "TestVersion";
            initializer.Initialize(telemetry);

            Assert.Equal("TestVersion", telemetry.Context.Component.Version);
        }

        [Fact]
        public void InitializeDoesNotThrowIfVersionDoesNotExist()
        {
            var initializer = new ComponentVersionTelemetryInitializer(this.BuildConfigurationWithVersion(null));
            var telemetry = new RequestTelemetry();
            initializer.Initialize(telemetry);
        }

        private IOptions<ApplicationInsightsServiceOptions> BuildConfigurationWithVersion(string versions = "1.0.0")
        {
            return new OptionsWrapper<ApplicationInsightsServiceOptions>(new ApplicationInsightsServiceOptions()
            {
                ApplicationVersion = versions
            });
        }
    }
}

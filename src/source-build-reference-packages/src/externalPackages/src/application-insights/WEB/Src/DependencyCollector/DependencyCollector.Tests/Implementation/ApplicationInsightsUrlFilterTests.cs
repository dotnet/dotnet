namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ApplicationInsightsUrlFilterTests
    {
        [TestMethod]
        public void IsApplicationInsightsUrlReturnsTrueForTelemetryServiceEndpoint()
        {
            using (TelemetryConfiguration configuration = this.CreateStubTelemetryConfiguration())
            {
                string url = "https://dc.services.visualstudio.com/v2/track";
                ApplicationInsightsUrlFilter urlFilter = new ApplicationInsightsUrlFilter(configuration);
                Assert.IsTrue(urlFilter.IsApplicationInsightsUrl(url));
            }
        }

        [TestMethod]
        public void IsApplicationInsightsUrlReturnsTrueForQuickPulseServiceEndpoint()
        {
            using (TelemetryConfiguration configuration = this.CreateStubTelemetryConfiguration())
            {
                string url = "https://rt.services.visualstudio.com/QuickPulseService.svc";
                ApplicationInsightsUrlFilter urlFilter = new ApplicationInsightsUrlFilter(configuration);
                Assert.IsTrue(urlFilter.IsApplicationInsightsUrl(url));
            }
        }

        [TestMethod]
        public void IsApplicationInsightsUrlReturnsTrueForTelemetryChannelEndpointAddress()
        {
            using (TelemetryConfiguration configuration = this.CreateStubTelemetryConfiguration())
            {
                string url = "https://endpointaddress";
                ApplicationInsightsUrlFilter urlFilter = new ApplicationInsightsUrlFilter(configuration);
                Assert.IsTrue(urlFilter.IsApplicationInsightsUrl(url));
            }
        }

        [TestMethod]
        public void IsApplicationInsightsUrlReturnsFalseForNullOrEmptyUrl()
        {
            using (TelemetryConfiguration configuration = this.CreateStubTelemetryConfiguration())
            {
                string url = null;
                ApplicationInsightsUrlFilter urlFilter = new ApplicationInsightsUrlFilter(configuration);
                Assert.IsFalse(urlFilter.IsApplicationInsightsUrl(url));
                url = string.Empty;
                Assert.IsFalse(urlFilter.IsApplicationInsightsUrl(url));
            }
        }

        [TestMethod]
        public void IsApplicationInsightsUrlReturnsFalseIfTelemetryChannelIsNull()
        {
            using (TelemetryConfiguration configuration = this.CreateStubTelemetryConfiguration())
            {
                configuration.TelemetryChannel = null;
                string url = "https://something.local";
                ApplicationInsightsUrlFilter urlFilter = new ApplicationInsightsUrlFilter(configuration);
                Assert.IsFalse(urlFilter.IsApplicationInsightsUrl(url));
            }
        }

        [TestMethod]
        public void IsApplicationInsightsUrlReturnsTrueForTelemetryServiceEndpointIfTelemetryChannelIsNull()
        {
            using (TelemetryConfiguration configuration = this.CreateStubTelemetryConfiguration())
            {
                configuration.TelemetryChannel = null;
                string url = "https://dc.services.visualstudio.com/v2/track";
                ApplicationInsightsUrlFilter urlFilter = new ApplicationInsightsUrlFilter(configuration);
                Assert.IsTrue(urlFilter.IsApplicationInsightsUrl(url));
            }
        }

        private TelemetryConfiguration CreateStubTelemetryConfiguration()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            configuration.TelemetryChannel = new StubTelemetryChannel { EndpointAddress = "https://endpointaddress" };
            configuration.InstrumentationKey = Guid.NewGuid().ToString();
            return configuration;
        }
    }
}

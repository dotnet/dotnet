namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.TestFramework;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Assert = Xunit.Assert;

    [TestClass]
    public class ExceptionTrackingTelemetryModuleTests
    {
        private TelemetryConfiguration configuration;
        private ConcurrentQueue<ITelemetry> sendItems;

        [TestInitialize]
        public void TestInit()
        {
            this.sendItems = new ConcurrentQueue<ITelemetry>();
            var stubTelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sendItems.Enqueue(item) };
            this.configuration = new TelemetryConfiguration
            {
                InstrumentationKey = Guid.NewGuid().ToString(),
                TelemetryChannel = stubTelemetryChannel
            };
        }

        [TestMethod]
        public void OnErrorTracksExceptionsThatArePresentInHttpContext()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();
            var exception1 = new Exception("1");
            platformContext.AddError(exception1);
            platformContext.AddError(new Exception("2"));

            using (var module = new ExceptionTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.OnError(platformContext);
            }

            Assert.Equal(2, this.sendItems.Count);
            Assert.Equal(exception1, ((ExceptionTelemetry)this.sendItems.First()).Exception);
        }

        [TestMethod]
        public void OnErrorSetsSeverityToCriticalForRequestWithStatusCode500()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();
            platformContext.Response.StatusCode = 500;
            platformContext.AddError(new Exception());

            using (var module = new ExceptionTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.OnError(platformContext);
            }

            Assert.Equal(SeverityLevel.Critical, ((ExceptionTelemetry)this.sendItems.First()).SeverityLevel);
        }

        [TestMethod]
        public void OnErrorDoesNotThrowOnNullContext()
        {
            using (var module = new ExceptionTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.OnError(null); // is not supposed to throw
            }
        }
    }
}

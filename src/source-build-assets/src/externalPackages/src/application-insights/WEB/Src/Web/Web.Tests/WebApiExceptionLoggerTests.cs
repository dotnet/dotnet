namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WebApiExceptionLoggerTests
    {
        private ConcurrentQueue<ITelemetry> sentTelemetry;
        private TelemetryConfiguration configuration;

        [TestInitialize]
        public void TestInit()
        {
            GlobalConfiguration.Configuration.Services.Clear(typeof(IExceptionLogger));
            this.sentTelemetry = new ConcurrentQueue<ITelemetry>();

            var stubTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = t =>
                {
                    if (t is ExceptionTelemetry telemetry)
                    {
                        this.sentTelemetry.Enqueue(telemetry);
                    }
                }
            };

            this.configuration = new TelemetryConfiguration
            {
                InstrumentationKey = Guid.NewGuid().ToString(),
                TelemetryChannel = stubTelemetryChannel
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            while (this.sentTelemetry.TryDequeue(out var _))
            {
            }

            GlobalConfiguration.Configuration.Services.Clear(typeof(IExceptionLogger));
        }

        [TestMethod]
        public void WebApiExceptionLoggerIsInjectedAndTracksException()
        {
            Assert.IsFalse(GlobalConfiguration.Configuration.Services.GetServices(typeof(IExceptionLogger)).Any());

            using (var exceptionModule = new ExceptionTrackingTelemetryModule())
            {
                exceptionModule.Initialize(this.configuration);

                var webApiExceptionLoggers = GlobalConfiguration.Configuration.Services.GetServices(typeof(IExceptionLogger)).ToList();
                Assert.AreEqual(1, webApiExceptionLoggers.Count);

                var logger = (ExceptionLogger)webApiExceptionLoggers[0];
                Assert.IsNotNull(logger);

                var exception = new Exception("test");
                var exceptionContext = new ExceptionLoggerContext(new ExceptionContext(exception, new ExceptionContextCatchBlock("catch block name", true, false)));
                logger.Log(exceptionContext);

                Assert.AreEqual(1, this.sentTelemetry.Count);

                var trackedException = (ExceptionTelemetry)this.sentTelemetry.Single();
                Assert.IsNotNull(trackedException);
                Assert.AreEqual(exception, trackedException.Exception);
            }
        }

        [TestMethod]
        public void WebApiExceptionLoggerIsNotInjectedIfAnotherInjectionDetected()
        {
            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new WebApiAutoInjectedLogger());
            Assert.AreEqual(1, GlobalConfiguration.Configuration.Services.GetServices(typeof(IExceptionLogger)).Count());

            using (var exceptionModule = new ExceptionTrackingTelemetryModule())
            {
                exceptionModule.Initialize(this.configuration);

                var loggers = GlobalConfiguration.Configuration.Services.GetServices(typeof(IExceptionLogger)).ToList();
                Assert.AreEqual(1, loggers.Count);
                Assert.IsInstanceOfType(loggers.Single(), typeof(WebApiAutoInjectedLogger));
            }
        }

        private class WebApiAutoInjectedLogger : ExceptionLogger
        {
            public const bool IsAutoInjected = true;
        }
    }
}

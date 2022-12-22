namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MvcExceptionHandlerTests
    {
        private ConcurrentQueue<ExceptionTelemetry> sentTelemetry;
        private TelemetryConfiguration configuration;

        [TestInitialize]
        public void TestInit()
        {
            GlobalFilters.Filters.Clear();
            this.sentTelemetry = new ConcurrentQueue<ExceptionTelemetry>();

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

            GlobalFilters.Filters.Clear();
        }

        [TestMethod]
        public void MvcExceptionFilterIsInjectedAndTracksException()
        {
            using (var exceptionModule = new ExceptionTrackingTelemetryModule())
            {
                exceptionModule.Initialize(this.configuration);

                var mvcExceptionFilters = GlobalFilters.Filters;
                Assert.AreEqual(1, mvcExceptionFilters.Count);

                var handleExceptionFilter = (HandleErrorAttribute)mvcExceptionFilters.Single().Instance;
                Assert.IsNotNull(handleExceptionFilter);

                var exception = new Exception("test");
                var controllerCtx = HttpModuleHelper.GetFakeControllerContext(isCustomErrorEnabled: true);
                handleExceptionFilter.OnException(new ExceptionContext(controllerCtx, exception));

                Assert.AreEqual(1, this.sentTelemetry.Count);

                var trackedException = this.sentTelemetry.Single();
                Assert.IsNotNull(trackedException);
                Assert.AreEqual(exception, trackedException.Exception);
            }
        }

        [TestMethod]
        public void MvcExceptionFilterIsNotInjectedIsInjectionIsDisabled()
        {
            using (var exceptionModule = new ExceptionTrackingTelemetryModule())
            {
                exceptionModule.EnableMvcAndWebApiExceptionAutoTracking = false;
                exceptionModule.Initialize(this.configuration);

                Assert.IsFalse(GlobalFilters.Filters.Any());
            }
        }

        [TestMethod]
        public void MvcExceptionLoggerIsNotInjectedIfAnotherInjectionDetected()
        {
            GlobalFilters.Filters.Add(new MvcAutoInjectedFilter());
            Assert.AreEqual(1, GlobalFilters.Filters.Count);

            using (var exceptionModule = new ExceptionTrackingTelemetryModule())
            {
                exceptionModule.Initialize(this.configuration);

                var filters = GlobalFilters.Filters;
                Assert.AreEqual(1, filters.Count);
                Assert.IsInstanceOfType(filters.Single().Instance, typeof(MvcAutoInjectedFilter));
            }
        }

        [TestMethod]
        public void MvcExceptionFilterNoopIfCustomErrorsIsFalse()
        {
            using (var exceptionModule = new ExceptionTrackingTelemetryModule())
            {
                exceptionModule.Initialize(this.configuration);

                var mvcExceptionFilters = GlobalFilters.Filters;
                Assert.AreEqual(1, mvcExceptionFilters.Count);

                var handleExceptionFilter = (HandleErrorAttribute)mvcExceptionFilters.Single().Instance;
                Assert.IsNotNull(handleExceptionFilter);

                var exception = new Exception("test");
                var controllerCtx = HttpModuleHelper.GetFakeControllerContext(isCustomErrorEnabled: false);
                handleExceptionFilter.OnException(new ExceptionContext(controllerCtx, exception));

                Assert.IsFalse(this.sentTelemetry.Any());
            }
        }

        [TestMethod]
        public void MvcExceptionFilterNoopIfExceptionIsNull()
        {
            using (var exceptionModule = new ExceptionTrackingTelemetryModule())
            {
                exceptionModule.Initialize(this.configuration);

                var mvcExceptionFilters = GlobalFilters.Filters;
                Assert.AreEqual(1, mvcExceptionFilters.Count);

                var handleExceptionFilter = (HandleErrorAttribute)mvcExceptionFilters.Single().Instance;
                Assert.IsNotNull(handleExceptionFilter);

                var controllerCtx = HttpModuleHelper.GetFakeControllerContext(isCustomErrorEnabled: true);
                var exceptionContext = new ExceptionContext(controllerCtx, new Exception());
                exceptionContext.Exception = null;
                handleExceptionFilter.OnException(exceptionContext);

                Assert.IsFalse(this.sentTelemetry.Any());
            }
        }

        private class MvcAutoInjectedFilter : HandleErrorAttribute
        {
            public const bool IsAutoInjected = true;
        }
    }
}

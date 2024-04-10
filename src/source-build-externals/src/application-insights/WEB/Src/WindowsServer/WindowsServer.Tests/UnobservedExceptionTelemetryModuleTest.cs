namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Assert = Xunit.Assert;

    [TestClass]
    public class UnobservedExceptionTelemetryModuleTest
    {
        private TelemetryConfiguration moduleConfiguration;
        private IList<ITelemetry> items;

        [TestInitialize]
        public void TestInitialize()
        {
            this.items = new List<ITelemetry>();

            var moduleChannel = new StubTelemetryChannel
            {
                OnSend = telemetry => this.items.Add(telemetry),
                EndpointAddress = "http://test.com"
            };

            this.moduleConfiguration = new TelemetryConfiguration
            {
                TelemetryChannel = moduleChannel,
                InstrumentationKey = "MyKey",
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.moduleConfiguration = null;
            this.items.Clear();
        }

        [TestMethod]
        public void TrackedExceptionsHaveMessageFromException()
        {
            EventHandler<UnobservedTaskExceptionEventArgs> handler = null;
            using (var module = new UnobservedExceptionTelemetryModule(
                h => handler = h,
                _ => { }))
            {
                module.Initialize(this.moduleConfiguration);
                handler.Invoke(null, new UnobservedTaskExceptionEventArgs(new AggregateException("Test")));
            }

            Assert.Equal("Test", ((ExceptionTelemetry)this.items[0]).Exception.Message);
        }

        [TestMethod]
        public void TrackedExceptionsHavePrefixUsedForTelemetry()
        {
            string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(typeof(UnobservedExceptionTelemetryModule), prefix: "unobs:");

            EventHandler<UnobservedTaskExceptionEventArgs> handler = null;
            using (var module = new UnobservedExceptionTelemetryModule(
                h => handler = h,
                _ => { }))
            {
                module.Initialize(this.moduleConfiguration);
                handler.Invoke(null, new UnobservedTaskExceptionEventArgs(new AggregateException(string.Empty)));
            }

            Assert.Equal(expectedVersion, this.items[0].Context.GetInternalContext().SdkVersion);
        }

        [TestMethod]
        public void InitializeCallsRegister()
        {
            EventHandler<UnobservedTaskExceptionEventArgs> handler = null;
            using (var module = new UnobservedExceptionTelemetryModule(
                h => handler = h,
                _ => { }))
            {
                module.Initialize(this.moduleConfiguration);
            }

            Assert.NotNull(handler);
        }

        [TestMethod]
        public void InitializeCallsRegisterOnce()
        {
            int count = 0;

            using (var module = new UnobservedExceptionTelemetryModule(
                _ => ++count,
                _ => { }))
            {
                module.Initialize(this.moduleConfiguration);
            }

            Assert.Equal(1, count);
        }

        [TestMethod]
        [Timeout(5000)]
        public void InitializeCallsRegisterOnceThreadSafe()
        {
            int count = 0;

            Task[] tasks = new Task[50];

            using (var module = new UnobservedExceptionTelemetryModule(
                _ => ++count,
                _ => { }))
            {
                for (int i = 0; i < 50; ++i)
                {
                    tasks[i] = System.Threading.Tasks.Task.Run(() => module.Initialize(this.moduleConfiguration));
                }

                System.Threading.Tasks.Task.WhenAll(tasks).Wait();
            }

            Assert.Equal(1, count);
        }

        [TestMethod]
        public void DisposeCallsUnregister()
        {
            EventHandler<UnobservedTaskExceptionEventArgs> handler = null;
            using (var module = new UnobservedExceptionTelemetryModule(
                _ => { },
                h => handler = h))
            {
                module.Initialize(this.moduleConfiguration);
            }

            Assert.NotNull(handler);
        }
    }
}

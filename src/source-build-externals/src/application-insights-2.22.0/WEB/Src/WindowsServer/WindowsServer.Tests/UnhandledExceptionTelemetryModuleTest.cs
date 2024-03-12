#if NETFRAMEWORK
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
    using TaskEx = System.Threading.Tasks.Task;

    [TestClass]
    public class UnhandledExceptionTelemetryModuleTest
    {
        private StubTelemetryChannel moduleChannel;
        private IList<ITelemetry> items;

        [TestInitialize]
        public void TestInitialize()
        {
            this.items = new List<ITelemetry>();

            this.moduleChannel = new StubTelemetryChannel { OnSend = telemetry => this.items.Add(telemetry) };

            TelemetryConfiguration.Active.TelemetryChannel = new StubTelemetryChannel
            {
                EndpointAddress = "http://test.com"
            };
            TelemetryConfiguration.Active.InstrumentationKey = "MyKey";
            TelemetryConfiguration.Active.TelemetryInitializers.Clear();            
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.moduleChannel = null;
            this.items.Clear();
        }

        [TestMethod]
        public void EndpointAddressFromConfigurationActiveIsUsedForSending()
        {
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                this.moduleChannel))
            {
                handler.Invoke(null, new UnhandledExceptionEventArgs(null, true));
            }

            Assert.Equal("http://test.com", this.moduleChannel.EndpointAddress);
        }

        [TestMethod]
        public void TelemetryInitializersFromConfigurationActiveAreUsedForSending()
        {
            bool called = false;
            var telemetryInitializer = new StubTelemetryInitializer { OnInitialize = item => called = true };

            TelemetryConfiguration.Active.TelemetryInitializers.Add(telemetryInitializer);
            
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                new InMemoryChannel()))
            {
                handler.Invoke(null, new UnhandledExceptionEventArgs(null, true));
            }

            Assert.True(called);
        }

        [TestMethod]
        public void InstrumentationKeyCanBeOverridenInCodeAfterModuleIsCreated()
        {
            // This scenario is important for CloudApps where everything exept iKey comes from ai.config
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                this.moduleChannel))
            {
                TelemetryConfiguration.Active.InstrumentationKey = "MyKey2";
                handler.Invoke(null, new UnhandledExceptionEventArgs(null, true));
            }

            Assert.Equal("MyKey2", this.items[0].Context.InstrumentationKey);
        }

        [TestMethod]
        public void TrackedExceptionsHavePrefixUsedForTelemetry()
        {
            string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(typeof(UnhandledExceptionTelemetryModule), prefix: "unhnd:");
            
            UnhandledExceptionEventHandler handler = null;
            using (new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                this.moduleChannel))
            {
                handler.Invoke(null, new UnhandledExceptionEventArgs(null, true));
            }

            Assert.Equal(expectedVersion, this.items[0].Context.GetInternalContext().SdkVersion);
        }

        [TestMethod]
        public void TrackedExceptionsHaveCorrectMessage()
        {
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                this.moduleChannel))
            {
                handler.Invoke(null, new UnhandledExceptionEventArgs(new ApplicationException("Test"), true));
            }

            Assert.Equal("Test", ((ExceptionTelemetry)this.items[0]).Exception.Message);
        }

        [TestMethod]
        public void ModuleConstructorCallsRegister()
        {
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                this.moduleChannel))
            {
            }

            Assert.NotNull(handler);
        }

        [TestMethod]
        public void DisposeCallsUnregister()
        {
            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                _ => { },
                h => handler = h,
                this.moduleChannel))
            {
            }

            Assert.NotNull(handler);
        }

        [TestMethod]
        public void DisposeDestructsChannel()
        {
            bool called = false;
            this.moduleChannel.OnDispose = () => called = true;

            using (var module = new UnhandledExceptionTelemetryModule(
                _ => { },
                _ => { },
                this.moduleChannel))
            {
            }

            Assert.True(called);
        }

        [TestMethod]
        public void FlushIsCalledToBeSureDataIsSent()
        {
            bool called = false;
            this.moduleChannel.OnFlush = () => called = true;

            UnhandledExceptionEventHandler handler = null;
            using (var module = new UnhandledExceptionTelemetryModule(
                h => handler = h,
                _ => { },
                this.moduleChannel))
            {
                handler.Invoke(null, new UnhandledExceptionEventArgs(null, true));
            }

            Assert.True(called);
        }
    }
}
#endif
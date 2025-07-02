namespace Microsoft.ApplicationInsights.Tests
{
#if NET452
    using System;
    using System.Reflection;

    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// DependencyTrackingTelemetryModule .Net 4.5 specific tests. 
    /// </summary>
    public partial class DependencyTrackingTelemetryModuleTest
    {
        private const string IKey = "F8474271-D231-45B6-8DD4-D344C309AE69";
        private const string FakeProfileApiEndpoint = "http://www.microsoft.com";

        [TestMethod]
        public void EventSourceInstrumentationDisabledWhenProfilerInstrumentationEnabled()
        {
            using (var module = new TestableDependencyTrackingTelemetryModule())
            {
                module.OnIsProfilerAvailable = () => true;

                module.Initialize(TelemetryConfiguration.CreateDefault());

                var f1 = typeof(DependencyTrackingTelemetryModule).GetField("httpEventListener", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNull(f1.GetValue(module));

                var f2 = typeof(DependencyTrackingTelemetryModule).GetField("sqlEventListener", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNull(f2.GetValue(module));
            }
        }

        [TestMethod]
        public void EventSourceInstrumentationEnabledWhenProfilerInstrumentationDisabled()
        {
            using (var module = new TestableDependencyTrackingTelemetryModule())
            {
                module.OnIsProfilerAvailable = () => false;

                module.Initialize(TelemetryConfiguration.CreateDefault());

                var f1 = typeof(DependencyTrackingTelemetryModule).GetField("httpEventListener", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNotNull(f1.GetValue(module));

                var f2 = typeof(DependencyTrackingTelemetryModule).GetField("sqlEventListener", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNotNull(f2.GetValue(module));
            }
        }

        [TestMethod]
        public void EventSourceInstrumentationEnabledWhenProfilerFailsToAttach()
        {
            using (var module = new TestableDependencyTrackingTelemetryModule())
            {
                module.OnIsProfilerAvailable = () => true;
                module.OnInitializeForRuntimeProfiler = () => { throw new Exception(); };
                DependencyTableStore.Instance.IsProfilerActivated = false;

                module.Initialize(TelemetryConfiguration.CreateDefault());

                var f1 = typeof(DependencyTrackingTelemetryModule).GetField("httpEventListener", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNotNull(f1.GetValue(module));

                var f2 = typeof(DependencyTrackingTelemetryModule).GetField("sqlEventListener", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNotNull(f2.GetValue(module));

                Assert.IsFalse(DependencyTableStore.Instance.IsProfilerActivated);
            }
        }

        [TestMethod]
        public void RequestIdInjectionInW3CModeIsTrueByDefault()
        {
            using (var module = new DependencyTrackingTelemetryModule())
            {
                Assert.IsTrue(module.EnableRequestIdHeaderInjectionInW3CMode);
            }
        }

        [TestMethod]
        public void DependencyTrackingTelemetryModuleIsNotInitializedTwiceToPreventProfilerAttachFailure()
        {
            using (var module = new DependencyTrackingTelemetryModule())
            {
                PrivateObject privateObject = new PrivateObject(module);

                module.Initialize(TelemetryConfiguration.CreateDefault());
                object config1 = privateObject.GetField("telemetryConfiguration");

                module.Initialize(TelemetryConfiguration.CreateDefault());
                object config2 = privateObject.GetField("telemetryConfiguration");

                Assert.AreSame(config1, config2);
            }
        }

        internal class TestableDependencyTrackingTelemetryModule : DependencyTrackingTelemetryModule
        {
            public TestableDependencyTrackingTelemetryModule()
            {
                this.OnInitializeForRuntimeProfiler = () => { };
                this.OnIsProfilerAvailable = () => true;
            }

            public Action OnInitializeForRuntimeProfiler { get; set; }

            public Func<bool> OnIsProfilerAvailable { get; set; }

            internal override void InitializeForRuntimeProfiler()
            {
                this.OnInitializeForRuntimeProfiler();
            }

            internal override bool IsProfilerAvailable()
            {
                return this.OnIsProfilerAvailable();
            }
        }
    }
#endif
}

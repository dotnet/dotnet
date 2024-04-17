namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks;

    [TestClass]
    public class HeartbeatTests
    {
        [TestMethod]
        public void InitializeHealthHeartbeatDoesntThrow()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);
            }
        }

        [TestMethod]
        public void InitializeHealthHeartbeatTwiceDoesntFail()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);
                hbeat.Initialize(configuration: null);
            }
        }

        [TestMethod]
        public void InitializeHealthHeartbeatDefaultsAreSetProperly()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);
                Assert.IsTrue(hbeat.ExcludedHeartbeatProperties == null || hbeat.ExcludedHeartbeatProperties.Count() == 0);
                Assert.AreEqual(hbeat.HeartbeatInterval, HeartbeatProvider.DefaultHeartbeatInterval);
            }
        }

        [TestMethod]
        public void InitializeHealthHeartbeatWithNonDefaultInterval()
        {
            TimeSpan userSetInterval = TimeSpan.FromMilliseconds(HeartbeatProvider.MinimumHeartbeatInterval.TotalMilliseconds + 7852.0);

            using (var initializedModule = new DiagnosticsTelemetryModule())
            {
                // set the interval via the IHeartbeatPropertyManager interface
                IHeartbeatPropertyManager hbeat = initializedModule;
                Assert.AreNotEqual(userSetInterval, hbeat.HeartbeatInterval);
                hbeat.HeartbeatInterval = userSetInterval;

                // initialize the DiagnosticsTelemetryModule, and check that the interval is still intact
                initializedModule.Initialize(new TelemetryConfiguration());
                Assert.AreEqual(userSetInterval, hbeat.HeartbeatInterval);
            }
        }

        [TestMethod]
        public void InitializeHeartbeatWithZeroIntervalIsSetToMinimum()
        {
            using (var hbeat = new HeartbeatProvider() { HeartbeatInterval = TimeSpan.FromMilliseconds(0) })
            {
                hbeat.Initialize(configuration: null);
                Assert.AreEqual(hbeat.HeartbeatInterval, HeartbeatProvider.MinimumHeartbeatInterval);
            }
        }

        [TestMethod]
        public void SetHeartbeatWithSmallerThanMinimumIntervalIsSetToMinimum()
        {
            var tooSmallInterval = TimeSpan.FromMilliseconds(HeartbeatProvider.MinimumHeartbeatInterval.TotalMilliseconds / 2);
            using (var hbeat = new HeartbeatProvider() { HeartbeatInterval = tooSmallInterval })
            {
                hbeat.Initialize(configuration: null);
                Assert.AreEqual(hbeat.HeartbeatInterval, HeartbeatProvider.MinimumHeartbeatInterval);
            }
        }

        [TestMethod]
        public void CanExtendHeartbeatPayload()
        {

            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: new TelemetryConfiguration());

                try
                {
                    Assert.IsTrue(hbeat.AddHeartbeatProperty("test01", "this is a value", true));
                }
                catch (Exception e)
                {
                    Assert.Fail(string.Format(CultureInfo.CurrentCulture, "Registration of a heartbeat payload provider throws exception '{0}", e.ToInvariantString()));
                }
            }
        }

        [TestMethod]
        public void InitializationOfTelemetryClientDoesntResetHeartbeat()
        {
            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();
            TelemetryClient client = new TelemetryClient(configuration);

            bool origIsEnabled = true;
            string origExcludedHbProvider = "Nonsense-Test";
            TimeSpan origInterval = TimeSpan.MaxValue;
            TimeSpan setInterval = TimeSpan.MaxValue;

            foreach (var module in TelemetryModules.Instance.Modules)
            {
                if (module is IHeartbeatPropertyManager hbeatMan)
                {
                    origIsEnabled = hbeatMan.IsHeartbeatEnabled;
                    hbeatMan.IsHeartbeatEnabled = !origIsEnabled;

                    Assert.IsFalse(hbeatMan.ExcludedHeartbeatPropertyProviders.Contains(origExcludedHbProvider));
                    hbeatMan.ExcludedHeartbeatPropertyProviders.Add(origExcludedHbProvider);


                    hbeatMan.ExcludedHeartbeatProperties.Add("Test01");

                    origInterval = hbeatMan.HeartbeatInterval;
                    setInterval = origInterval + TimeSpan.FromMinutes(2);
                    hbeatMan.HeartbeatInterval = setInterval;
                }
            }

            TelemetryClient client2 = new TelemetryClient(configuration);

            foreach (var module in TelemetryModules.Instance.Modules)
            {
                if (module is IHeartbeatPropertyManager hbeatMan)
                {
                    Assert.AreNotEqual(hbeatMan.IsHeartbeatEnabled, origIsEnabled);
                    Assert.IsTrue(hbeatMan.ExcludedHeartbeatPropertyProviders.Contains(origExcludedHbProvider));
                    Assert.IsTrue(hbeatMan.ExcludedHeartbeatProperties.Contains("Test01"));
                    Assert.AreNotEqual(hbeatMan.HeartbeatInterval, origInterval);
                    Assert.AreEqual(hbeatMan.HeartbeatInterval, setInterval);
                }
            }

        }

        [TestMethod]
        public void IsHeartbeatEnabledByDefault()
        {
            using (var initializedModule = new DiagnosticsTelemetryModule())
            {
                // test that the heartbeat is enabled by default
                IHeartbeatPropertyManager hbeat = initializedModule;
                Assert.IsTrue(hbeat.IsHeartbeatEnabled);

                // initialize the DiagnosticsTelemetryModule, and check that heartbeats are still enabled
                initializedModule.Initialize(new TelemetryConfiguration());
                Assert.IsTrue(hbeat.IsHeartbeatEnabled);
            }
        }

        [TestMethod]
        public void CanDisableHeartbeatPriorToInitialize()
        {
            using (var initializedModule = new DiagnosticsTelemetryModule())
            {
                // disable the heartbeat at construction time but before initialize
                // (this simulates the flow of disabling the heartbeat via config)
                IHeartbeatPropertyManager hbeat = initializedModule;
                hbeat.IsHeartbeatEnabled = false;

                // initialize the DiagnosticsTelemetryModule, and check that heartbeats are still disabled
                initializedModule.Initialize(new TelemetryConfiguration());
                Assert.IsFalse(hbeat.IsHeartbeatEnabled);

                // dig into the heartbeat provider itself to ensure this is indeed disabled
                Assert.IsFalse(initializedModule.HeartbeatProvider.IsHeartbeatEnabled);
            }
        }

        [TestMethod]
        public void CanDisableHeartbeatPropertyProviderPriorToInitialize()
        {
            using (var initializedModule = new DiagnosticsTelemetryModule())
            {
                // disable the heartbeat property provider 'Base' at construction time but before initialize
                // (this simulates the flow of disabling the heartbeat via config)
                IHeartbeatPropertyManager hbeat = initializedModule;
                IHeartbeatDefaultPayloadProvider hbeatPropProvider = HeartbeatDefaultPayload.DefaultPayloadProviders.First();
                hbeat.ExcludedHeartbeatPropertyProviders.Add(hbeatPropProvider.Name);

                // dig into the heartbeat provider itself to ensure this is indeed disabled
                Assert.IsTrue(initializedModule.HeartbeatProvider.ExcludedHeartbeatPropertyProviders.Contains(hbeatPropProvider.Name));

                // initialize the DiagnosticsTelemetryModule, and check that heartbeats are still disabled
                initializedModule.Initialize(new TelemetryConfiguration());

                // ensure this heartbeat property provider is still disabled after init
                Assert.IsTrue(initializedModule.HeartbeatProvider.ExcludedHeartbeatPropertyProviders.Contains(hbeatPropProvider.Name));
            }
        }

        [TestMethod]
        public void DefaultHeartbeatPropertyProviderSendsNoFieldWhenDisabled()
        {
            var mockHbeat = new HeartbeatProviderMock();
            IHeartbeatDefaultPayloadProvider baseDefaultProvider = HeartbeatDefaultPayload.DefaultPayloadProviders.First();
            var hbeatPayload = HeartbeatDefaultPayload.PopulateDefaultPayload(new string[] { }, new string[] { baseDefaultProvider.Name }, mockHbeat);
            hbeatPayload.GetAwaiter().GetResult();

            // for each heartbeat property recorded into the heartbeat provider, none should be a registered keyword on 
            // this base default heartbeat property provider.
            foreach (string payloadPropertyName in mockHbeat.HeartbeatProperties.Keys)
            {
                Assert.IsFalse(baseDefaultProvider.IsKeyword(payloadPropertyName));
            }
        }

        [TestMethod]
        public void DiagnosticsTelemetryModuleCreatesHeartbeatModule()
        {
            using (var diagModule = new DiagnosticsTelemetryModule())
            {
                diagModule.Initialize(new TelemetryConfiguration());
                Assert.IsNotNull(diagModule.HeartbeatProvider);
            }
        }

        [TestMethod]
        public void HeartbeatPayloadContainsDataByDefault()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);
                var hbeatPayloadData = hbeat.GatherData();
                Assert.IsNotNull(hbeatPayloadData);
            }
        }

        [TestMethod]
        public void HeartbeatPayloadContainsUserSpecifiedData()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                string testerKey = "tester123";
                Assert.IsTrue(hbeat.AddHeartbeatProperty(testerKey, "test", true));
                hbeat.Initialize(configuration: null);

                MetricTelemetry payload = (MetricTelemetry)hbeat.GatherData();

                Assert.IsTrue(payload.Properties.Any(
                    a => a.Key.Equals(testerKey, StringComparison.OrdinalIgnoreCase)),
                    "Provided custom payload provider to heartbeat but never received any messages with its content");
            }
        }

        [TestMethod]
        public void HeartbeatPayloadContainsOnlyAllowedDefaultPayloadFields()
        {
            var assemblyDefFields = new BaseDefaultHeartbeatPropertyProvider();

            var allDefaultFields = assemblyDefFields.DefaultFields;

            using (var hbeat = new HeartbeatProvider())
            {
                List<string> disableHbProps = new List<string>();

                for (int i = 0; i < allDefaultFields.Count(); ++i)
                {
                    if (i % 2 == 0)
                    {
                        disableHbProps.Add(allDefaultFields[i]);
                        hbeat.ExcludedHeartbeatProperties.Add(allDefaultFields[i]);
                    }
                }

                hbeat.Initialize(configuration: null);

                var sentHeartBeat = (MetricTelemetry)hbeat.GatherData();

                Assert.IsNotNull(sentHeartBeat);

                foreach (var kvp in sentHeartBeat.Properties)
                {
                    Assert.IsFalse(disableHbProps.Contains(kvp.Key), 
                        string.Format(CultureInfo.CurrentCulture, "Dissallowed field '{0}' found in payload", kvp.Key));
                }
            }
        }

        [TestMethod]
        public void HeartbeatMetricIsZeroForNoFailureConditionPresent()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);
                var msg = (MetricTelemetry)hbeat.GatherData();
                Assert.IsFalse(msg.Sum > 0.0);
            }
        }

        [TestMethod]
        public void HeartbeatMetricIsNonZeroWhenFailureConditionPresent()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);
                string testerKey = "tester123";
                hbeat.AddHeartbeatProperty(testerKey, "test", false);

                var msg = (MetricTelemetry)hbeat.GatherData();
                Assert.IsTrue(msg.Sum >= 1.0);
            }
        }

        [TestMethod]
        public void HeartbeatMetricCountAccountsForAllFailures()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);

                var msg = (MetricTelemetry)hbeat.GatherData();
                Assert.IsTrue(msg.Sum == 0.0);

                hbeat.AddHeartbeatProperty("tester01", "test failure 1", false);
                hbeat.AddHeartbeatProperty("tester02", "test failure 2", false);
                msg = (MetricTelemetry)hbeat.GatherData();

                Assert.IsTrue(msg.Sum == 2.0);
            }
        }

        [TestMethod]
        public void SentHeartbeatContainsExpectedDefaultFields()
        {
            using (var hbeatMock = new HeartbeatProviderMock())
            {
                var baseHbeatProps = new BaseDefaultHeartbeatPropertyProvider().DefaultFields;

                var taskWaiter = HeartbeatDefaultPayload.PopulateDefaultPayload(new string[] { }, new string[] { }, hbeatMock).ConfigureAwait(false);
                Assert.IsTrue(taskWaiter.GetAwaiter().GetResult()); // no await for tests

                foreach (string fieldName in baseHbeatProps)
                {
                    Assert.IsTrue(hbeatMock.HeartbeatProperties.ContainsKey(fieldName));
                }
            }
        }

        [TestMethod]
        public void HeartbeatProviderDoesNotAllowDuplicatePropertyName()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);

                Assert.IsTrue(hbeat.AddHeartbeatProperty("test01", "some test value", true));
                Assert.IsFalse(hbeat.AddHeartbeatProperty("test01", "some other test value", true));
            }
        }

        [TestMethod]
        public void CannotSetPayloadExtensionWithoutAddingItFirst()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);

                Assert.IsFalse(hbeat.SetHeartbeatProperty("test01", "some other test value", true));
                Assert.IsTrue(hbeat.AddHeartbeatProperty("test01", "some test value", true));
                Assert.IsTrue(hbeat.SetHeartbeatProperty("test01", "some other test value", true));
            }
        }

        [TestMethod]
        public void CannotSetValueOfDefaultPayloadProperties()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);
                BaseDefaultHeartbeatPropertyProvider defFields = new BaseDefaultHeartbeatPropertyProvider();
                
                foreach (string key in defFields.DefaultFields)
                {
                    Assert.IsFalse(hbeat.SetHeartbeatProperty(key, "test", true));
                }
            }
        }

        [TestMethod]
        public void CannotAddPayloadItemNamedOfDefaultPayloadProperties()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);
                BaseDefaultHeartbeatPropertyProvider defFields = new BaseDefaultHeartbeatPropertyProvider();
                
                foreach (string key in defFields.DefaultFields)
                {
                    Assert.IsFalse(hbeat.AddHeartbeatProperty(key, "test", true));
                }
            }
        }

        [TestMethod]
        public void EnsureAllTargetFrameworksRepresented()
        {
            BaseDefaultHeartbeatPropertyProvider bp = new BaseDefaultHeartbeatPropertyProvider();
            var noAwaiterForTests = bp.SetDefaultPayload(new string[] { }, new HeartbeatProviderMock()).ConfigureAwait(false);
            Assert.IsTrue(noAwaiterForTests.GetAwaiter().GetResult());
            // this is enough to ensure we've hit all of our target framework paths (unless a new one has been added, the purpose of this test)
        }

        [TestMethod]
        public void CanSetHealthHeartbeatPayloadValueWithoutHealthyFlag()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);

                string key = "setValueTest";

                Assert.IsTrue(hbeat.AddHeartbeatProperty(key, "value01", true));
                Assert.IsTrue(hbeat.SetHeartbeatProperty(key, "value02"));
                var msg = (MetricTelemetry)hbeat.GatherData();
                
                Assert.IsNotNull(msg);
                Assert.IsTrue(msg.Properties.ContainsKey(key));
                Assert.IsTrue(msg.Properties[key].Equals("value02", StringComparison.Ordinal));
            }
        }

        [TestMethod]
        public void CanSetHealthHeartbeatPayloadHealthIndicatorWithoutSettingValue()
        {
            using (var hbeat = new HeartbeatProvider())
            {
                hbeat.Initialize(configuration: null);

                string key = "healthSettingTest";

                Assert.IsTrue(hbeat.AddHeartbeatProperty(key, "value01", true));
                Assert.IsTrue(hbeat.SetHeartbeatProperty(key, null, false));
                var msg = (MetricTelemetry)hbeat.GatherData();
                
                Assert.IsNotNull(msg);
                Assert.IsTrue(msg.Properties.ContainsKey(key));
                Assert.IsTrue(msg.Properties[key].Equals("value01", StringComparison.Ordinal));
                Assert.IsTrue(msg.Sum == 1.0); // one false message in payload only
            }
        }

        [TestMethod]
        public void HandleUnknownDefaultProperty()
        {
            var defProps = new BaseDefaultHeartbeatPropertyProvider();
            string testKey = "TestProp";
            defProps.DefaultFields.Add(testKey);
            using (var hbeat = new HeartbeatProvider())
            {
                var waitForProps = defProps.SetDefaultPayload(new string[] { }, hbeat).ConfigureAwait(false);
                Assert.IsTrue(waitForProps.GetAwaiter().GetResult());
                var heartbeat = (MetricTelemetry)hbeat.GatherData();
                Assert.IsTrue(heartbeat.Properties.ContainsKey(testKey));
                Assert.IsFalse(string.IsNullOrEmpty(heartbeat.Properties[testKey]));
            }
        }

        [TestMethod]
        public void CanOverrideDefaultHeartbeatValuesInternally()
        {
            using (var hbeat = (IHeartbeatProvider)new HeartbeatProvider())
            {
                var baseProps = new BaseDefaultHeartbeatPropertyProvider();
                var defaultFieldName = baseProps.DefaultFields[0];
                Assert.IsTrue(hbeat.AddHeartbeatProperty(defaultFieldName, true, "test", true));
                Assert.IsFalse(hbeat.AddHeartbeatProperty(defaultFieldName, true, "test", true));
                Assert.IsTrue(hbeat.SetHeartbeatProperty(defaultFieldName, true, "test-1", false));
                Assert.IsFalse(hbeat.SetHeartbeatProperty(defaultFieldName, false, "test-2", false));
            }
        }
    }
}

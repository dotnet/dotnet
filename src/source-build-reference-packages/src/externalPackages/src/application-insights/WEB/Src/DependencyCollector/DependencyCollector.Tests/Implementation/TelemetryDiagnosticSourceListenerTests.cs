namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TelemetryDiagnosticSourceListenerTests
    {
        #region Fields

        private TelemetryConfiguration configuration;
        private List<ITelemetry> sentItems;

        #endregion Fields

        #region TestInitialize

        [TestInitialize]
        public void TestInitialize()
        {
            this.configuration = new TelemetryConfiguration();
            this.sentItems = new List<ITelemetry>();
            this.configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sentItems.Add(item), EndpointAddress = "https://dc.services.visualstudio.com/v2/track" };
            this.configuration.InstrumentationKey = Guid.NewGuid().ToString();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Activity.Current = null;
        }

        #endregion TestInitiliaze

        #region Subscribtion tests

        [TestMethod]
        public void TelemetryDiagnosticSourceListenerOnCreatedListener()
        {
            var inclusionList = new[] { "Test.A" }.ToList();
            using (DiagnosticListener listener = new DiagnosticListener("Test.A"))
            using (var dl = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                dl.Subscribe();
                Assert.IsTrue(listener.IsEnabled(), "There is a subscriber for a new diagnostic source");
            }
        }

        [TestMethod]
        public void TelemetryDiagnosticSourceListenerCapturesAllActivitiesByDefault()
        {
            var inclusionList = new[] { "Test.A" }.ToList();
            using (var dl = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                dl.Subscribe();
                using (var listener = new DiagnosticListener("Test.A"))
                {
                    Activity activity = new Activity("Test.A.Client.Monitoring");

                    Assert.IsTrue(listener.IsEnabled(), "There is a subscriber for a new diagnostic source");
                    Assert.IsTrue(listener.IsEnabled(activity.OperationName),
                        "There is a subscriber for a new activity");
                    Assert.IsTrue(
                        listener.IsEnabled(
                            activity.OperationName + TelemetryDiagnosticSourceListener.ActivityStopNameSuffix),
                        "There is a subscriber for new activity Stop event");
                    Assert.IsFalse(
                        listener.IsEnabled(activity.OperationName +
                                           TelemetryDiagnosticSourceListener.ActivityStartNameSuffix),
                        "There are no subscribers for new activity Start event");

                    int sentCountBefore = this.sentItems.Count;

                    listener.StartActivity(activity, null);
                    Assert.AreEqual(sentCountBefore, this.sentItems.Count,
                        "No telemetry item should be sent on activity start");

                    listener.StopActivity(activity, null);
                    Assert.AreEqual(sentCountBefore + 1, this.sentItems.Count,
                        "One new telemetry item should be sent on activity stop");

                    DependencyTelemetry telemetryItem = this.sentItems.Last() as DependencyTelemetry;
                    Assert.IsNotNull(telemetryItem, "Dependency telemetry item should be sent");
                    Assert.AreEqual(activity.OperationName, telemetryItem.Name);
                }
            }
        }

        [TestMethod]
        public void TelemetryDiagnosticSourceListenerIgnoresNotIncludedSources()
        {
            var inclusionList = new[] { "Test.B" }.ToList();
            using (var listenerA = new DiagnosticListener("Test.A"))
            using (var dl = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                dl.Subscribe();
                // Diagnostic Source A is ignored
                Activity activityA = new Activity("Test.A.Client.Monitoring");

                Assert.IsFalse(listenerA.IsEnabled(), "There are no subscribers for excluded diagnostic source A");
                Assert.IsFalse(listenerA.IsEnabled(activityA.OperationName), "There are no subscribers for activity A");

                int sentCountBefore = this.sentItems.Count;

                listenerA.StartActivity(activityA, null);
                Assert.AreEqual(sentCountBefore, this.sentItems.Count, "No telemetry item should be sent on activity start");

                listenerA.StopActivity(activityA, null);
                Assert.AreEqual(sentCountBefore, this.sentItems.Count, "No telemetry item should be sent on activity stop");

                // Diagnostic Source B is still captured
                using (var listenerB = new DiagnosticListener("Test.B"))
                {
                    Activity activityB = new Activity("Test.B.Client.Monitoring");

                    Assert.IsTrue(listenerB.IsEnabled(), "There is a subscriber for diagnostic source B");
                    Assert.IsTrue(listenerB.IsEnabled(activityB.OperationName), "There is a subscriber for activity B");

                    listenerB.StartActivity(activityB, null);
                    Assert.AreEqual(sentCountBefore, this.sentItems.Count,
                        "No telemetry item should be sent on activity start");

                    listenerB.StopActivity(activityB, null);
                    Assert.AreEqual(sentCountBefore + 1, this.sentItems.Count,
                        "One new telemetry item should be sent on activity B stop");

                    DependencyTelemetry telemetryItem = this.sentItems.Last() as DependencyTelemetry;
                    Assert.IsNotNull(telemetryItem, "Dependency telemetry item should be sent");
                    Assert.AreEqual(telemetryItem.Name, activityB.OperationName);
                }
            }
        }

        [TestMethod]
        public void TelemetryDiagnosticSourceListenerIgnoresNotIncludedActivities()
        {
            var inclusionList = new[] { "Test.A:Test.A.Client.Monitoring" }.ToList();
            using (var listener = new DiagnosticListener("Test.A"))
            using (var dl = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                dl.Subscribe();

                // Diagnostic Source is not ignored
                Assert.IsTrue(listener.IsEnabled(), "There is a subscriber for diagnostic source");

                // Activity1 is ignored per exclusion
                Activity activity1 = new Activity("Test.A.Activity1");
                Assert.IsFalse(listener.IsEnabled(activity1.OperationName), "There are no subscribers for activity 1");

                int sentCountBefore = this.sentItems.Count;

                listener.StartActivity(activity1, null);
                Assert.AreEqual(sentCountBefore, this.sentItems.Count, "No telemetry item should be sent on activity 1 start");

                listener.StopActivity(activity1, null);
                Assert.AreEqual(sentCountBefore, this.sentItems.Count, "No telemetry item should be sent on activity 1 stop");

                // Activity2 is ignored per exclusion
                Activity activity2 = new Activity("Test.A.Activity2");
                Assert.IsFalse(listener.IsEnabled(activity2.OperationName), "There are no subscribers for activity 2");

                listener.StartActivity(activity2, null);
                Assert.AreEqual(sentCountBefore, this.sentItems.Count, "No telemetry item should be sent on activity 2 start");

                listener.StopActivity(activity2, null);
                Assert.AreEqual(sentCountBefore, this.sentItems.Count, "No telemetry item should be sent on activity 2 stop");

                // non-excluded activity from same diagnostic source is captured
                Activity activity = new Activity("Test.A.Client.Monitoring");
                Assert.IsTrue(listener.IsEnabled(activity.OperationName), "There is a subscriber for activity");

                listener.StartActivity(activity, null);
                Assert.AreEqual(sentCountBefore, this.sentItems.Count, "No telemetry item should be sent on activity start");

                listener.StopActivity(activity, null);
                Assert.AreEqual(sentCountBefore + 1, this.sentItems.Count, "One new telemetry item should be sent on activity stop");

                DependencyTelemetry telemetryItem = this.sentItems.Last() as DependencyTelemetry;
                Assert.IsNotNull(telemetryItem, "Dependency telemetry item should be sent");
                Assert.AreEqual(telemetryItem.Name, activity.OperationName);
            }
        }

        #endregion Subscribtion tests

        #region Collection tests

        [TestMethod]
        public void TelemetryDiagnosticSourceListenerCollectsTelemetryFromRawActivity()
        {
            var inclusionList = new[] { "Test.A" }.ToList();

            using (var listener = new DiagnosticListener("Test.A"))
            using (var dl = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                dl.Subscribe();

                // generic example
                var tags = new Dictionary<string, string>()
                {
                    ["error"] = "true",
                    ["peer.hostname"] = "test.example.com",
                    ["custom.tag"] = "test"
                };

                DependencyTelemetry telemetryItem = this.CollectDependencyTelemetryFromActivity(listener, tags);

                Assert.AreEqual(telemetryItem.Name, "Test.A.Client.Monitoring"); // Activity name
                Assert.AreEqual(telemetryItem.Type, listener.Name);
                Assert.IsTrue(string.IsNullOrEmpty(telemetryItem.Data));
                Assert.AreEqual(telemetryItem.Target, tags["peer.hostname"]);
                Assert.AreEqual(telemetryItem.Success, false);
                Assert.IsTrue(telemetryItem.Properties.ContainsKey("custom.tag"));
                Assert.AreEqual(telemetryItem.Properties["custom.tag"], tags["custom.tag"]);
            }
        }

        [TestMethod]
        public void TelemetryDiagnosticSourceListenerInitializedWithDependencyModule()
        {
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Test.A");
                module.Initialize(this.configuration);

                using (var listener = new DiagnosticListener("Test.A"))
                {
                    // generic example
                    var tags = new Dictionary<string, string>
                    {
                        ["error"] = "true",
                        ["peer.hostname"] = "test.example.com",
                        ["custom.tag"] = "test"
                    };

                    DependencyTelemetry telemetryItem = this.CollectDependencyTelemetryFromActivity(listener, tags);

                    Assert.AreEqual(telemetryItem.Name, "Test.A.Client.Monitoring"); // Activity name
                    Assert.AreEqual(telemetryItem.Type, listener.Name);
                    Assert.IsTrue(string.IsNullOrEmpty(telemetryItem.Data));
                    Assert.AreEqual(telemetryItem.Target, tags["peer.hostname"]);
                    Assert.AreEqual(telemetryItem.Success, false);
                    Assert.IsTrue(telemetryItem.Properties.ContainsKey("custom.tag"));
                    Assert.AreEqual(telemetryItem.Properties["custom.tag"], tags["custom.tag"]);
                }
            }
        }

        [TestMethod]
        public void TelemetryDiagnosticSourceListenerCollectsTelemetryFromRawActivityWithoutParent()
        {
            var inclusionList = new List<string> { "Test.A" };
            using (var listener = new DiagnosticListener("Test.A"))
            using (var dl = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                dl.Subscribe();

                var activity = new Activity("Test.A.Activity1");

                listener.StartActivity(activity, null);
                listener.StopActivity(activity, null);

                var telemetryItem = this.sentItems.Last() as DependencyTelemetry;

                Assert.AreEqual(activity.SpanId.ToHexString(), telemetryItem.Id);
                Assert.AreEqual(activity.TraceId.ToHexString(), telemetryItem.Context.Operation.Id);
                Assert.AreEqual(null, telemetryItem.Context.Operation.ParentId);
            }
        }

        [TestMethod]
        public void TelemetryDiagnosticSourceListenerCollectsTelemetryFromRawActivityWithParent()
        {
            var inclusionList = new List<string> { "Test.A" };
            using (var listener = new DiagnosticListener("Test.A"))
            using (var dl = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                dl.Subscribe();

                var parentActivity = new Activity("Parent").Start();
                var activity = new Activity("Test.A.Activity1");

                listener.StartActivity(activity, null);
                listener.StopActivity(activity, null);

                var telemetryItem = this.sentItems.Last() as DependencyTelemetry;

                Assert.AreEqual(activity.SpanId.ToHexString(), telemetryItem.Id);
                Assert.AreEqual(activity.TraceId.ToHexString(), telemetryItem.Context.Operation.Id);
                Assert.AreEqual(activity.ParentSpanId.ToHexString(), telemetryItem.Context.Operation.ParentId);
            }
        }

        #endregion Collection tests

        #region Custom handlers

        [TestMethod]
        public void TelemetryDiagnosticSourceListenerCallsCustomHandlersWhenEnabled()
        {
            var inclusionList = new[] { "Test.A:Send", "Test.B" }.ToList();
            using (var telemetryListener = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                telemetryListener.Subscribe();
                TestableEventHandler ahandler = new TestableEventHandler();
                TestableEventHandler bhandler = new TestableEventHandler();
                telemetryListener.RegisterHandler("Test.A", ahandler);
                telemetryListener.RegisterHandler("Test.B", bhandler);

                using (var listenerA = new DiagnosticListener("Test.A"))
                using (var listenerB = new DiagnosticListener("Test.B"))
                {
                    this.DoOperation(listenerA, "Send");
                    this.DoOperation(listenerA, "Receive");
                    this.DoOperation(listenerB, "Any");
                }

                Assert.AreEqual(1, ahandler.EventCalls.Count);

                var testASend = ahandler.EventCalls[0];
                Assert.AreEqual("Test.A", testASend.Item1);
                Assert.AreEqual("Send.Stop", testASend.Item2.Key);
                Assert.IsNull(testASend.Item2.Value);

                Assert.AreEqual(1, bhandler.EventCalls.Count);
                var testBAny = bhandler.EventCalls[0];
                Assert.AreEqual("Test.B", testBAny.Item1);
                Assert.AreEqual("Any.Stop", testBAny.Item2.Key);
                Assert.IsNull(testBAny.Item2.Value);
            }
        }

        #endregion

        #region MultiHosts

        [TestMethod]
        public void MultiHost_OnlyOneListnerTracksTelemetry()
        {
            var inclusionList = new[] { "Test.A" }.ToList();

            using (var listener = new DiagnosticListener("Test.A"))
            using (var dl1 = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            using (var dl2 = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                dl1.Subscribe();
                dl2.Subscribe();

                Activity activity = new Activity("Test.A.Client.Monitoring");
                listener.StartActivity(activity, null);
                listener.StopActivity(activity, null);

                Assert.AreEqual(1, this.sentItems.Count(t => t is DependencyTelemetry));
            }
        }

        [TestMethod]
        public void MultiHost_TwoActiveAndOneIsDisposedStillTracksTelemetry()
        {
            var inclusionList = new[] { "Test.A" }.ToList();

            var dl1 = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList);
            dl1.Subscribe();

            using (var listener = new DiagnosticListener("Test.A"))
            using (var dl2 = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
            {
                dl2.Subscribe();

                Activity activity = new Activity("Test.A.Client.Monitoring");
                listener.StartActivity(activity, null);
                listener.StopActivity(activity, null);

                Assert.AreEqual(1, this.sentItems.Count(t => t is DependencyTelemetry));

                dl1.Dispose();

                activity = new Activity("Test.A.Client.Monitoring");
                listener.StartActivity(activity, null);
                listener.StopActivity(activity, null);

                Assert.AreEqual(2, this.sentItems.Count(t => t is DependencyTelemetry));
            }
        }

        [TestMethod]
        public void MultiHost_OneListnerThenAnotherTracksTelemetry()
        {
            var inclusionList = new[] { "Test.A" }.ToList();

            using (var listener = new DiagnosticListener("Test.A"))
            {
                using (var dl = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
                {
                    dl.Subscribe();

                    Activity activity = new Activity("Test.A.Client.Monitoring");
                    listener.StartActivity(activity, null);
                    listener.StopActivity(activity, null);

                    Assert.AreEqual(1, this.sentItems.Count(t => t is DependencyTelemetry));
                }

                using (var dl = new TelemetryDiagnosticSourceListener(this.configuration, inclusionList))
                {
                    dl.Subscribe();

                    Activity activity = new Activity("Test.A.Client.Monitoring");
                    listener.StartActivity(activity, null);
                    listener.StopActivity(activity, null);

                    Assert.AreEqual(2, this.sentItems.Count(t => t is DependencyTelemetry));
                }
            }
        }

        #endregion

        private void DoOperation(DiagnosticListener listener, string activityName)
        {
            Activity activity = null;

            if (listener.IsEnabled(activityName))
            {
                activity = new Activity(activityName);
                if (listener.IsEnabled(activityName + ".Start"))
                {
                    listener.StartActivity(activity, null);
                }
                else
                {
                    activity.Start();
                }
            }

            if (activity != null)
            {
                listener.StopActivity(activity, null);
            }
        }

        private DependencyTelemetry CollectDependencyTelemetryFromActivity(DiagnosticListener listener, Dictionary<string, string> tags)
        {
            Activity activity = new Activity("Test.A.Client.Monitoring");

            foreach (var tag in tags)
            {
                activity.AddTag(tag.Key, tag.Value);
            }

            listener.StartActivity(activity, null);
            listener.StopActivity(activity, null);

            DependencyTelemetry telemetryItem = this.sentItems.Last() as DependencyTelemetry;
            return telemetryItem;
        }

        private class TestableEventHandler : IDiagnosticEventHandler
        {
            public readonly List<Tuple<string, KeyValuePair<string, object>>> EventCalls = new List<Tuple<string, KeyValuePair<string, object>>>();

            public void OnEvent(KeyValuePair<string, object> evnt, DiagnosticListener diagnosticListener)
            {
                this.EventCalls.Add(new Tuple<string, KeyValuePair<string, object>>(diagnosticListener.Name, evnt));
            }

            public bool IsEventEnabled(string evnt, object arg1, object arg2)
            {
                return !evnt.EndsWith("Start");
            }
        }
    }
}

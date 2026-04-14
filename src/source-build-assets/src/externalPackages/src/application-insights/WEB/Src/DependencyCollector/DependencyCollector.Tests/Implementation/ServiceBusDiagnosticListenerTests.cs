namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ServiceBusDiagnosticListenerTests
    {
        private TelemetryConfiguration configuration;
        private List<ITelemetry> sentItems;

        [TestInitialize]
        public void TestInitialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            this.configuration = new TelemetryConfiguration();
            this.sentItems = new List<ITelemetry>();
            this.configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sentItems.Add(item), EndpointAddress = "https://dc.services.visualstudio.com/v2/track" };
            this.configuration.InstrumentationKey = Guid.NewGuid().ToString();
            this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
        }

        [TestCleanup]
        public void CleanUp()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void DiagnosticEventWithoutActivityIsIgnored()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                listener.Write(
                    "Microsoft.Azure.ServiceBus.Send.Stop",
                    new
                    {
                        Entity = "queueName",
                        Endpoint = new Uri("sb://queuename.myservicebus.com/")
                    });

                Assert.IsFalse(this.sentItems.Any());
            }
        }

        [TestMethod]
        public void ServiceBusSendHanding()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                parentActivity.TraceStateString = "state=some";
                var telemetry = this.TrackOperation<DependencyTelemetry>(listener,
                    "Microsoft.Azure.ServiceBus.Send", 
                    TaskStatus.RanToCompletion,
                    null,
                    () => sendActivity = Activity.Current);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, telemetry.Type);
                Assert.AreEqual("sb://queuename.myservicebus.com/ | queueName", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.AreEqual(parentActivity.SpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(parentActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.AreEqual("v1", telemetry.Properties["k1"]);
                Assert.AreEqual("messageId", telemetry.Properties["MessageId"]);

                Assert.IsTrue(telemetry.Properties.TryGetValue("tracestate", out var tracestate));
                Assert.AreEqual("state=some", tracestate);
            }
        }

        [TestMethod]
        public void ServiceBusSendHandingW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                var telemetry = this.TrackOperation<DependencyTelemetry>(listener,
                    "Microsoft.Azure.ServiceBus.Send", TaskStatus.RanToCompletion);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, telemetry.Type);
                Assert.AreEqual("sb://queuename.myservicebus.com/ | queueName", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.AreEqual(parentActivity.Id, telemetry.Context.Operation.ParentId);
                Assert.AreEqual(parentActivity.RootId, telemetry.Context.Operation.Id);

                Assert.AreEqual("v1", telemetry.Properties["k1"]);
                Assert.AreEqual("messageId", telemetry.Properties["MessageId"]);
            }
        }

        [TestMethod]
        public void ServiceBusSendHandingWithoutParent()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                var telemetry = this.TrackOperation<DependencyTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Send",
                    TaskStatus.RanToCompletion,
                    null,
                    () => sendActivity = Activity.Current);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, telemetry.Type);
                Assert.AreEqual("sb://queuename.myservicebus.com/ | queueName", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.AreEqual("messageId", telemetry.Properties["MessageId"]);
            }
        }

        [TestMethod]
        public void ServiceBusBadStatusHanding()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                var telemetry = this.TrackOperation<DependencyTelemetry>(listener,
                    "Microsoft.Azure.ServiceBus.Send",
                    TaskStatus.Faulted,
                    null,
                    () => sendActivity = Activity.Current);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("Send", telemetry.Name);
                Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, telemetry.Type);
                Assert.AreEqual("sb://queuename.myservicebus.com/ | queueName", telemetry.Target);
                Assert.IsFalse(telemetry.Success.Value);

                Assert.AreEqual(parentActivity.SpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(parentActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.AreEqual("v1", telemetry.Properties["k1"]);
                Assert.AreEqual("messageId", telemetry.Properties["MessageId"]);
            }
        }

        [TestMethod]
        public void ServiceBusProcessHanding()
        {
            var tc = new TelemetryClient(this.configuration);

            Activity messageActivity = null;
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                var parent = new Activity("foo").AddBaggage("k1", "v1").Start();
                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process",
                    TaskStatus.RanToCompletion,
                    operation: TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
                    requestTelemetry.Source);
                Assert.IsTrue(requestTelemetry.Success.Value);

                Assert.AreEqual(messageActivity.ParentSpanId.ToHexString(), requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual(messageActivity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
                Assert.AreEqual(messageActivity.SpanId.ToHexString(), requestTelemetry.Id);

                Assert.AreEqual("v1", requestTelemetry.Properties["k1"]);

                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            }
        }

        [TestMethod]
        public void ServiceBusProcessHandingW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            var tc = new TelemetryClient(this.configuration);

            Activity messageActivity = null;
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process",
                    TaskStatus.RanToCompletion,
                    operation: TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
                    requestTelemetry.Source);
                Assert.IsTrue(requestTelemetry.Success.Value);

                Assert.AreEqual(parentActivity.Id, requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual(parentActivity.RootId, requestTelemetry.Context.Operation.Id);
                Assert.AreEqual(messageActivity.Id, requestTelemetry.Id);
                Assert.AreEqual("v1", requestTelemetry.Properties["k1"]);

                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            }
        }

        [TestMethod]
        public void ServiceBusProcessHandingExternalHierarchicalParent()
        {
            var tc = new TelemetryClient(this.configuration);

            Activity messageActivity = null;
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process", 
                    TaskStatus.RanToCompletion, 
                    "|hierarchical-parent.",
                    TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
                    requestTelemetry.Source);
                Assert.IsTrue(requestTelemetry.Success.Value);

                Assert.IsTrue(requestTelemetry.Properties.TryGetValue("ai_legacyRootId", out var legacyRoot));
                Assert.AreEqual("hierarchical-parent", legacyRoot);
                Assert.AreEqual("|hierarchical-parent.", requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual(messageActivity.SpanId.ToHexString(), requestTelemetry.Id);
                Assert.AreEqual(messageActivity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            }
        }

        [TestMethod]
        public void ServiceBusProcessHandingExternalHierarchicalW3CCompatibleParent()
        {
            var tc = new TelemetryClient(this.configuration);

            Activity messageActivity = null;
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                var parentId = $"|{ActivityTraceId.CreateRandom().ToHexString()}.";
                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process",
                    TaskStatus.RanToCompletion,
                    parentId,
                    TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
                    requestTelemetry.Source);
                Assert.IsTrue(requestTelemetry.Success.Value);

                Assert.IsFalse(requestTelemetry.Properties.TryGetValue("ai_legacyRootId", out _));
                Assert.AreEqual(parentId, requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual(messageActivity.SpanId.ToHexString(), requestTelemetry.Id);
                Assert.AreEqual(messageActivity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);

                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            }
        }

        [TestMethod]
        public void ServiceBusProcessHandingExternalMalformedParent()
        {
            var tc = new TelemetryClient(this.configuration);

            Activity messageActivity = null;
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process",
                    TaskStatus.RanToCompletion,
                    "malformed-parent",
                    TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
                    requestTelemetry.Source);
                Assert.IsTrue(requestTelemetry.Success.Value);

                Assert.IsTrue(requestTelemetry.Properties.TryGetValue("ai_legacyRootId", out var legacyRoot));
                Assert.AreEqual("malformed-parent", legacyRoot);
                Assert.AreEqual("malformed-parent", requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual(messageActivity.SpanId.ToHexString(), requestTelemetry.Id);
                Assert.AreEqual(messageActivity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            }
        }

        [TestMethod]
        public void ServiceBusProcessHandingWithoutParent()
        {
            var tc = new TelemetryClient(this.configuration);
            Activity messageActivity = null;
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var module = new DependencyTrackingTelemetryModule())
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process",
                    TaskStatus.RanToCompletion,
                    operation: TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
                    requestTelemetry.Source);
                Assert.IsTrue(requestTelemetry.Success.Value);

                Assert.AreEqual(messageActivity.TraceId.ToHexString(), requestTelemetry.Context.Operation.Id);
                Assert.IsNull(requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual(messageActivity.SpanId.ToHexString(), requestTelemetry.Id);

                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            }
        }

        [TestMethod]
        public void ServiceBusProcessHandingExternalParentW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            var tc = new TelemetryClient(this.configuration);
            Activity messageActivity = null;
            void TrackTraceDuringProcessing()
            {
                messageActivity = Activity.Current;
                tc.TrackTrace("trace");
            }

            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                var requestTelemetry = this.TrackOperation<RequestTelemetry>(
                    listener,
                    "Microsoft.Azure.ServiceBus.Process",
                    TaskStatus.RanToCompletion,
                    "parent",
                    TrackTraceDuringProcessing);

                Assert.IsNotNull(requestTelemetry);
                Assert.AreEqual("Process", requestTelemetry.Name);
                Assert.AreEqual(
                    $"type:{RemoteDependencyConstants.AzureServiceBus} | name:queueName | endpoint:sb://queuename.myservicebus.com/",
                    requestTelemetry.Source);
                Assert.IsTrue(requestTelemetry.Success.Value);

                Assert.AreEqual("parent", requestTelemetry.Context.Operation.ParentId);
                Assert.AreEqual("parent", requestTelemetry.Context.Operation.Id);
                Assert.AreEqual(messageActivity.Id, requestTelemetry.Id);

                Assert.AreEqual("messageId", requestTelemetry.Properties["MessageId"]);

                var traceTelemetry = this.sentItems.OfType<TraceTelemetry>();
                Assert.AreEqual(1, traceTelemetry.Count());

                Assert.AreEqual(requestTelemetry.Context.Operation.Id, traceTelemetry.Single().Context.Operation.Id);
                Assert.AreEqual(requestTelemetry.Id, traceTelemetry.Single().Context.Operation.ParentId);
            }
        }

        [TestMethod]
        public void ServiceBusExceptionsAreIgnored()
        {
            using (var listener = new DiagnosticListener("Microsoft.Azure.ServiceBus"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
                module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
                module.Initialize(this.configuration);

                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                if (listener.IsEnabled("Microsoft.Azure.ServiceBus.Exception"))
                {
                    listener.Write("Microsoft.Azure.ServiceBus.Exception", new
                    {
                        Exception = new Exception("123")
                    });
                }

                Assert.IsFalse(this.sentItems.Any());
            }
        }

        private T TrackOperation<T>(
            DiagnosticListener listener,
            string activityName, 
            TaskStatus status, 
            string parentId = null,
            Action operation = null) where T : OperationTelemetry
        {
            Activity activity = null;

            if (listener.IsEnabled(activityName))
            {
                activity = new Activity(activityName);
                activity.AddTag("MessageId", "messageId");
                if (Activity.Current == null && parentId != null)
                {
                    activity.SetParentId(parentId);
                }

                if (listener.IsEnabled(activityName + ".Start"))
                {
                    listener.StartActivity(activity, new { Entity = "queueName", Endpoint = new Uri("sb://queuename.myservicebus.com/") });
                }
                else
                {
                    activity.Start();
                }
            }

            operation?.Invoke();

            if (activity != null)
            {
                listener.StopActivity(activity, new { Entity = "queueName", Endpoint = new Uri("sb://queuename.myservicebus.com/"), Status = status });
                return this.sentItems.OfType<T>().Last();
            }

            return default(T);
        }
    }
}
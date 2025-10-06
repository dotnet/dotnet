namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Net.Http;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;

    [TestClass]
    public class AzureSdkDiagnosticListenerTest
    {
        private static readonly DateTimeOffset EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        private TelemetryConfiguration configuration;
        private List<ITelemetry> sentItems;
        private static readonly JsonSerializerSettings JsonSettingThrowOnError = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error,
            ReferenceLoopHandling = ReferenceLoopHandling.Error,
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
        };

        [TestInitialize]
        public void TestInitialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            this.configuration = new TelemetryConfiguration();
            this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.sentItems = new List<ITelemetry>();
            this.configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sentItems.Add(item), EndpointAddress = "https://dc.services.visualstudio.com/v2/track" };
            this.configuration.InstrumentationKey = Guid.NewGuid().ToString();
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
        public void AzureSdkListenerDoesNotThrowAfterInitialization()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                // Dispose config after initialize
                // If AzureSdkListener attempted to create
                // new TelemetryClient after initialize, it'd throw.
                this.configuration.Dispose();

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "client");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var listenerAnother = new DiagnosticListener("Azure.AnotherClient");
                var listenerYetAnother = new DiagnosticListener("Azure.YetAnotherClient");

                Assert.AreEqual(0, this.sentItems.Count);
            }
        }

        [TestMethod]
        public void AzureClientSpansNotCollectedWhenDisabled()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.EnableAzureSdkTelemetryListener = false;
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                Assert.AreEqual(0, this.sentItems.Count);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollected()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = null;
                Activity parentActivity = new Activity("parent").AddBaggage("k1", "v1").Start();
                parentActivity.TraceStateString = "state=some";
                var telemetry = this.TrackOperation<DependencyTelemetry>(
                    listener,
                    "Azure.SomeClient.Send",
                    null,
                    () => sendActivity = Activity.Current);

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("InProc", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.AreEqual(sendActivity.ParentSpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.AreEqual("v1", telemetry.Properties["k1"]);

                Assert.IsTrue(telemetry.Properties.TryGetValue("tracestate", out var tracestate));
                Assert.AreEqual("state=some", tracestate);
            }
        }


        [TestMethod]
        public void AzureClientSpansAreCollectedMultipleDiagnosticSourcesSameName()
        {
            using (var listener1 = new DiagnosticListener("Azure.SomeClient"))
            using (var listener2 = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var telemetry1 = this.TrackOperation<DependencyTelemetry>(
                    listener1,
                    "Azure.SomeClient.Send",
                    null,
                    () => { });

                Assert.IsNotNull(telemetry1);
                Assert.AreEqual("SomeClient.Send", telemetry1.Name);

                var telemetry2 = this.TrackOperation<DependencyTelemetry>(
                    listener2,
                    "Azure.SomeClient.Send",
                    null,
                    () => { });

                Assert.IsNotNull(telemetry2);
                Assert.AreEqual("SomeClient.Send", telemetry2.Name);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedClientKind()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "client");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual(string.Empty, telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedProducerKind()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "producer");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("Queue Message", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedInternalKind()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "internal");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("InProc", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedProducerKindWithComponentEventHubs()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "producer");
                sendActivity.AddTag("component", "eventhubs");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("Queue Message | Azure Event Hubs", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedInternalKindWithComponentEventHubs()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "internal");
                sendActivity.AddTag("component", "eventhubs");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("InProc | Azure Event Hubs", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedProducerKindWithComponent()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "producer");
                sendActivity.AddTag("component", "foo");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("Queue Message | foo", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedInternalKindWithComponent()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "internal");
                sendActivity.AddTag("component", "foo");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("InProc | foo", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedProducerKindWithAzNamespaceEventHubs()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "producer");
                sendActivity.AddTag("az.namespace", "Microsoft.EventHub");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("Queue Message | Azure Event Hubs", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedProducerKindWithAzNamespaceEventHubsAndAttributes()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "producer");
                sendActivity.AddTag("az.namespace", "Microsoft.EventHub");
                sendActivity.AddTag("peer.address", "amqps://eventHub.servicebus.windows.net/");
                sendActivity.AddTag("message_bus.destination", "queueName");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("Queue Message | Azure Event Hubs", telemetry.Type);
                Assert.AreEqual("amqps://eventHub.servicebus.windows.net/queueName", telemetry.Target);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedInternalKindWithAzNamespaceEventHubs()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "internal");
                sendActivity.AddTag("az.namespace", "Microsoft.EventHub");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("InProc | Azure Event Hubs", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedProducerKindWithAzNamespace()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "producer");
                sendActivity.AddTag("az.namespace", "foo");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("Queue Message | foo", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedInternalKindWithAzNamespace()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "internal");
                sendActivity.AddTag("az.namespace", "foo");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("InProc | foo", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedServerKind()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "server");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as RequestTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
                Assert.IsFalse(telemetry.Metrics.Any());
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedServerKindEventHubs()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "server");
                sendActivity.AddTag("az.namespace", "Microsoft.EventHub");
                sendActivity.AddTag("peer.address", "amqps://eventHub.servicebus.windows.net");
                sendActivity.AddTag("message_bus.destination", "queueName");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as RequestTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("amqps://eventHub.servicebus.windows.net/queueName", telemetry.Source);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
                Assert.IsFalse(telemetry.Metrics.Any());
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedConsumerKindEventHubs()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "consumer");
                sendActivity.AddTag("az.namespace", "Microsoft.EventHub");
                sendActivity.AddTag("peer.address", "amqps://eventHub.servicebus.windows.net/");
                sendActivity.AddTag("message_bus.destination", "queueName");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as RequestTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("amqps://eventHub.servicebus.windows.net/queueName", telemetry.Source);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
                Assert.IsFalse(telemetry.Metrics.Any());
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedLinks()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var sendActivity = new Activity("Azure.SomeClient.Send");
                var link0TraceId = "70545f717a9aa6a490d820438b9d2bf6";
                var link1TraceId = "c5aa06717eef0c4592af26323ade92f7";
                var link0SpanId = "8b0b2fb40c84e64a";
                var link1SpanId = "3a69ce690411bb4f";

                var payload = new PayloadWithLinks
                {
                    Links = new List<Activity>
                    {
                        new Activity("link0").SetParentId($"00-{link0TraceId}-{link0SpanId}-01"),
                        new Activity("link1").SetParentId($"00-{link1TraceId}-{link1SpanId}-01"),
                    },
                };

                listener.StartActivity(sendActivity, payload);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                // does not throw
                Assert.IsTrue(telemetry.Properties.TryGetValue("_MS.links", out var linksStr));
                var actualLinks = JsonConvert.DeserializeObject<ApplicationInsightsLink[]>(linksStr, JsonSettingThrowOnError);

                Assert.IsNotNull(actualLinks);
                Assert.AreEqual(2, actualLinks.Length);

                Assert.AreEqual(link0TraceId, actualLinks[0].OperationId);
                Assert.AreEqual(link1TraceId, actualLinks[1].OperationId);

                Assert.AreEqual(link0SpanId, actualLinks[0].Id);
                Assert.AreEqual(link1SpanId, actualLinks[1].Id);

                Assert.AreEqual($"[{{\"operation_Id\":\"{link0TraceId}\",\"id\":\"{link0SpanId}\"}},{{\"operation_Id\":\"{link1TraceId}\",\"id\":\"{link1SpanId}\"}}]", linksStr);
                Assert.IsFalse(telemetry.Metrics.Any());
            }
        }

        [TestMethod]
        public void AzureServerSpansAreCollectedLinks()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "server");

                var link0TraceId = "70545f717a9aa6a490d820438b9d2bf6";
                var link1TraceId = "c5aa06717eef0c4592af26323ade92f7";
                var link0SpanId = "8b0b2fb40c84e64a";
                var link1SpanId = "3a69ce690411bb4f";

                var payload = new PayloadWithLinks
                {
                    Links = new List<Activity>
                    {
                        new Activity("link0").SetParentId($"00-{link0TraceId}-{link0SpanId}-01"),
                        new Activity("link1").SetParentId($"00-{link1TraceId}-{link1SpanId}-01"),
                    },
                };

                listener.StartActivity(sendActivity, payload);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as RequestTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                // does not throw
                Assert.IsTrue(telemetry.Properties.TryGetValue("_MS.links", out var linksStr));
                var actualLinks = JsonConvert.DeserializeObject<ApplicationInsightsLink[]>(linksStr, JsonSettingThrowOnError);

                Assert.IsNotNull(actualLinks);
                Assert.AreEqual(2, actualLinks.Length);

                Assert.AreEqual(link0TraceId, actualLinks[0].OperationId);
                Assert.AreEqual(link1TraceId, actualLinks[1].OperationId);

                Assert.AreEqual(link0SpanId, actualLinks[0].Id);
                Assert.AreEqual(link1SpanId, actualLinks[1].Id);

                Assert.AreEqual($"[{{\"operation_Id\":\"{link0TraceId}\",\"id\":\"{link0SpanId}\"}},{{\"operation_Id\":\"{link1TraceId}\",\"id\":\"{link1SpanId}\"}}]", linksStr);
                Assert.IsFalse(telemetry.Metrics.Any());
            }
        }

        [TestMethod]
        public void AzureConsumerSpansAreCollectedLinksAndTimeInQueue()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "consumer");

                long enqueued0 = ToUnixTimeStamp(DateTimeOffset.UtcNow.AddMilliseconds(-100));
                long enqueued1 = ToUnixTimeStamp(DateTimeOffset.UtcNow.AddMilliseconds(-200));
                long enqueued2 = ToUnixTimeStamp(DateTimeOffset.UtcNow.AddMilliseconds(-300));
                var payload = new PayloadWithLinks
                {
                    Links = new List<Activity>
                    {
                        CreateRandomLink(enqueued0),
                        CreateRandomLink(enqueued1),
                        CreateRandomLink(enqueued2),
                    },
                };

                listener.StartActivity(sendActivity, payload);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as RequestTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.AreEqual(1, telemetry.Metrics.Count);
                Assert.IsTrue(telemetry.Metrics.TryGetValue("timeSinceEnqueued", out var timeInQueue));

                var startTimeEpoch = ToUnixTimeStamp(sendActivity.StartTimeUtc);
                long expectedTimeInQueue = ((startTimeEpoch - enqueued0) +
                                          (startTimeEpoch - enqueued1) +
                                          (startTimeEpoch - enqueued2)) / 3; // avg diff with request start time across links

                Assert.AreEqual(expectedTimeInQueue, timeInQueue);
            }
        }

        [TestMethod]
        public void AzureConsumerSpansAreCollectedLinksAndTimeInQueueInvalid()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "consumer");

                var link = new Activity("foo").SetParentId(ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom());
                link.AddTag("enqueuedTime", "not long");

                var payload = new PayloadWithLinks { Links = new List<Activity> { link, }, };

                listener.StartActivity(sendActivity, payload);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as RequestTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.IsFalse(telemetry.Metrics.Any());
            }
        }

        [TestMethod]
        public void AzureConsumerSpansAreCollectedLinksAndTimeInQueueNegative()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var sendActivity = new Activity("Azure.SomeClient.Send");
                sendActivity.AddTag("kind", "consumer");

                long enqueued0 = ToUnixTimeStamp(DateTimeOffset.UtcNow.AddMilliseconds(-100));
                long enqueued1 = ToUnixTimeStamp(DateTimeOffset.UtcNow.AddMilliseconds(-200));
                long enqueued2 = ToUnixTimeStamp(DateTimeOffset.UtcNow.AddMilliseconds(300)); // ignored
                var payload = new PayloadWithLinks
                {
                    Links = new List<Activity>
                    {
                        CreateRandomLink(enqueued0),
                        CreateRandomLink(enqueued1),
                        CreateRandomLink(enqueued2),
                    },
                };

                listener.StartActivity(sendActivity, payload);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as RequestTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);

                Assert.AreEqual(1, telemetry.Metrics.Count);
                Assert.IsTrue(telemetry.Metrics.TryGetValue("timeSinceEnqueued", out var timeInQueue));

                var startTimeEpoch = ToUnixTimeStamp(sendActivity.StartTimeUtc);
                long expectedTimeInQueue = ((startTimeEpoch - enqueued0) +
                                          (startTimeEpoch - enqueued1)) / 3; // avg diff with request start time across links ignoring outliers (negative diff)

                Assert.AreEqual(expectedTimeInQueue, timeInQueue);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreMarkedAsFailed()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new InvalidOperationException();
                Activity sendActivity = null;

                var telemetry = this.TrackOperation<DependencyTelemetry>(
                    listener,
                    "Azure.SomeClient.Send",
                    null,
                    () =>
                    {
                        sendActivity = Activity.Current;
                        listener.Write("Azure.SomeClient.Send.Exception", exception);
                    });

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.IsFalse(telemetry.Success.Value);

                Assert.AreEqual(exception.ToInvariantString(), telemetry.Properties["Error"]);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedForHttp()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity httpActivity = new Activity("Azure.SomeClient.Http.Request")
                    .AddTag("http.method", "PATCH")
                    .AddTag("http.url", "http://host:8080/path?query#fragment")
                    .AddTag("requestId", "client-request-id");

                var payload = new HttpRequestMessage();
                listener.StartActivity(httpActivity, payload);
                httpActivity
                    .AddTag("http.status_code", "206")
                    .AddTag("serviceRequestId", "service-request-id");

                listener.StopActivity(httpActivity, payload);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("PATCH /path", telemetry.Name);
                Assert.AreEqual("host:8080", telemetry.Target);
                Assert.AreEqual("http://host:8080/path?query#fragment", telemetry.Data);
                Assert.AreEqual("206", telemetry.ResultCode);
                Assert.AreEqual("Http", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);
                Assert.AreEqual("client-request-id", telemetry.Properties["ClientRequestId"]);
                Assert.AreEqual("service-request-id", telemetry.Properties["ServerRequestId"]);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(httpActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(httpActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedForHttpNotSuccessResponse()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity httpActivity = new Activity("Azure.SomeClient.Http.Request")
                    .AddTag("http.method", "PATCH")
                    .AddTag("http.url", "http://host/path?query#fragment")
                    .AddTag("otel.status_code", "ERROR");

                var payload = new HttpRequestMessage();
                listener.StartActivity(httpActivity, payload);
                httpActivity.AddTag("http.status_code", "503");

                listener.StopActivity(httpActivity, payload);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("PATCH /path", telemetry.Name);
                Assert.AreEqual("host", telemetry.Target);
                Assert.AreEqual("http://host/path?query#fragment", telemetry.Data);
                Assert.AreEqual("503", telemetry.ResultCode);
                Assert.AreEqual("Http", telemetry.Type);
                Assert.IsFalse(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(httpActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(httpActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedForHttpNotSuccessResponseAndNoStatusCode()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity httpActivity = new Activity("Azure.SomeClient.Http.Request")
                    .AddTag("http.method", "PATCH")
                    .AddTag("http.url", "http://host/path?query#fragment");

                var payload = new HttpRequestMessage();
                listener.StartActivity(httpActivity, payload);
                httpActivity.AddTag("http.status_code", "503");

                listener.StopActivity(httpActivity, payload);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("PATCH /path", telemetry.Name);
                Assert.AreEqual("host", telemetry.Target);
                Assert.AreEqual("http://host/path?query#fragment", telemetry.Data);
                Assert.AreEqual("503", telemetry.ResultCode);
                Assert.AreEqual("Http", telemetry.Type);
                Assert.IsFalse(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(httpActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(httpActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedAndHttpStatusCodeIsIgnoredWithExplicitStatusCode()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity httpActivity = new Activity("Azure.SomeClient.Http.Request")
                    .AddTag("http.method", "PATCH")
                    .AddTag("http.url", "http://host/path?query#fragment")
                    .AddTag("otel.status_code", "UNSET");

                var payload = new HttpRequestMessage();
                listener.StartActivity(httpActivity, payload);
                httpActivity.AddTag("http.status_code", "503");

                listener.StopActivity(httpActivity, payload);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;
                Assert.IsTrue(telemetry.Success.Value);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedForHttpException()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new InvalidOperationException();
                Activity httpActivity = new Activity("Azure.SomeClient.Http.Request")
                    .AddTag("http.method", "PATCH")
                    .AddTag("http.url", "http://host/path?query#fragment");

                listener.StartActivity(httpActivity, null);
                listener.Write("Azure.SomeClient.Send.Exception", exception);

                listener.StopActivity(httpActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("PATCH /path", telemetry.Name);
                Assert.AreEqual("host", telemetry.Target);
                Assert.AreEqual("http://host/path?query#fragment", telemetry.Data);
                Assert.IsNull(telemetry.ResultCode);
                Assert.AreEqual("Http", telemetry.Type);
                Assert.IsFalse(telemetry.Success.Value);
                Assert.AreEqual(exception.ToInvariantString(), telemetry.Properties["Error"]);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(httpActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(httpActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedForEventHubs()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new InvalidOperationException();
                Activity sendActivity = new Activity("Azure.SomeClient.Send")
                    .AddTag("peer.address", "amqps://eventHub.servicebus.windows.net")
                    .AddTag("message_bus.destination", "queueName")
                    .AddTag("kind", "client")
                    .AddTag("component", "eventhubs");

                listener.StartActivity(sendActivity, null);
                listener.Write("Azure.SomeClient.Send.Exception", exception);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("amqps://eventHub.servicebus.windows.net/queueName", telemetry.Target);
                Assert.AreEqual(string.Empty, telemetry.Data);
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual("Azure Event Hubs", telemetry.Type);
                Assert.IsFalse(telemetry.Success.Value);
                Assert.AreEqual(exception.ToInvariantString(), telemetry.Properties["Error"]);
                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedForEventHubsMessages()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new InvalidOperationException();
                Activity sendActivity = new Activity("Azure.SomeClient.Send")
                    .AddTag("peer.address", "amqps://eventHub.servicebus.windows.net")
                    .AddTag("message_bus.destination", "queueName")
                    .AddTag("kind", "producer")
                    .AddTag("component", "eventhubs");

                listener.StartActivity(sendActivity, null);
                listener.Write("Azure.SomeClient.Send.Exception", exception);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("amqps://eventHub.servicebus.windows.net/queueName", telemetry.Target);
                Assert.AreEqual(string.Empty, telemetry.Data);
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual("Queue Message | Azure Event Hubs", telemetry.Type);
                Assert.IsFalse(telemetry.Success.Value);
                Assert.AreEqual(exception.ToInvariantString(), telemetry.Properties["Error"]);
                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [TestMethod]
        public void AzureClientSpansAreCollectedForEventHubsException()
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Send")
                    .AddTag("peer.address", "amqps://eventHub.servicebus.windows.net")
                    .AddTag("message_bus.destination", "queueName")
                    .AddTag("kind", "client")
                    .AddTag("component", "eventhubs");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Send", telemetry.Name);
                Assert.AreEqual("amqps://eventHub.servicebus.windows.net/queueName", telemetry.Target);
                Assert.AreEqual(string.Empty, telemetry.Data);
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual("Azure Event Hubs", telemetry.Type);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
            }
        }

        [DataRow("producer")]
        [DataRow("client")]
        [DataTestMethod]
        public void AzureServiceBusSpansAreCollectedAsDependency(string kind)
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Method")
                    .AddTag("kind", kind)
                    .AddTag("az.namespace", "Microsoft.ServiceBus")
                    .AddTag("component", "servicebus")
                    .AddTag("peer.address", "amqps://my.servicebus.windows.net/")
                    .AddTag("message_bus.destination", "queueName");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as DependencyTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Method", telemetry.Name);
                if (kind == "producer")
                {
                    Assert.AreEqual("Queue Message | Azure Service Bus", telemetry.Type);
                }
                else
                {
                    Assert.AreEqual("Azure Service Bus", telemetry.Type);
                }

                Assert.IsTrue(telemetry.Success.Value);
                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.StartTimeUtc, telemetry.Timestamp);
                Assert.AreEqual(sendActivity.Duration, telemetry.Duration);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
                Assert.AreEqual("amqps://my.servicebus.windows.net/queueName", telemetry.Target);
            }
        }

        [DataRow("server")]
        [DataRow("consumer")]
        [DataTestMethod]
        public void AzureServiceBusSpansAreCollectedAsRequest(string kind)
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.SomeClient.Process")
                    .AddTag("kind", kind)
                    .AddTag("az.namespace", "Microsoft.ServiceBus")
                    .AddTag("component", "servicebus")
                    .AddTag("peer.address", "amqps://my.servicebus.windows.net")
                    .AddTag("message_bus.destination", "queueName");

                listener.StartActivity(sendActivity, null);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last() as RequestTelemetry;

                Assert.IsNotNull(telemetry);
                Assert.AreEqual("SomeClient.Process", telemetry.Name);
                Assert.AreEqual("amqps://my.servicebus.windows.net/queueName", telemetry.Source);
                Assert.IsTrue(telemetry.Success.Value);

                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.StartTimeUtc, telemetry.Timestamp);
                Assert.AreEqual(sendActivity.Duration, telemetry.Duration);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), telemetry.Id);
                Assert.IsFalse(telemetry.Metrics.Any());
            }
        }

        [DataRow("producer")]
        [DataRow("client")]
        [DataRow("server")]
        [DataRow("consumer")]
        [DataTestMethod]
        public void AzureServiceBusSpansAreCollectedError(string kind)
        {
            using (var listener = new DiagnosticListener("Azure.SomeClient"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new InvalidOperationException();
                Activity sendActivity = new Activity("Azure.SomeClient.Method")
                    .AddTag("peer.address", "amqps://my.servicebus.windows.net")
                    .AddTag("message_bus.destination", "queueName")
                    .AddTag("kind", kind)
                    .AddTag("az.namespace", "Microsoft.ServiceBus");

                listener.StartActivity(sendActivity, null);
                listener.Write("Azure.SomeClient.Send.Exception", exception);
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last();

                Assert.IsNotNull(telemetry);
                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);

                OperationTelemetry operation = telemetry as OperationTelemetry;
                Assert.IsFalse(operation.Success.Value);
                Assert.AreEqual(exception.ToInvariantString(), operation.Properties["Error"]);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), operation.Id);
                Assert.AreEqual("SomeClient.Method", operation.Name);

                if (kind == "producer" || kind == "client" || kind == "internal")
                {
                    Assert.IsTrue(telemetry is DependencyTelemetry);
                    DependencyTelemetry dependency = telemetry as DependencyTelemetry;
                    Assert.AreEqual(string.Empty, dependency.Data);
                    Assert.AreEqual(string.Empty, dependency.ResultCode);
                    Assert.AreEqual("amqps://my.servicebus.windows.net/queueName", dependency.Target);
                    if (kind == "producer")
                    {
                        Assert.AreEqual("Queue Message | Azure Service Bus", dependency.Type);
                    }
                    else
                    {
                        Assert.AreEqual("Azure Service Bus", dependency.Type);
                    }
                }
                else
                {
                    Assert.IsTrue(telemetry is RequestTelemetry);
                    RequestTelemetry request = telemetry as RequestTelemetry;
                    Assert.AreEqual(string.Empty, request.ResponseCode);
                    Assert.AreEqual("amqps://my.servicebus.windows.net/queueName", request.Source);
                }
            }
        }

        [TestMethod]
        public void AzureCosmosDbSpansAreCollected()
        {
            using (var listener = new DiagnosticListener("Azure.Cosmos"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new InvalidOperationException();
                Activity sendActivity = new Activity("Azure.Cosmos.ReadItems")
                    .AddTag("net.peer.name", "my.documents.azure.com")
                    .AddTag("db.name", "database")
                    .AddTag("db.operation", "ReadItems")
                    .AddTag("db.cosmosdb.container", "container")
                    .AddTag("kind", "client")
                    .AddTag("az.namespace", "Microsoft.DocumentDB");

                listener.StartActivity(sendActivity, null);
                listener.Write("Azure.Cosmos.ReadItems.Exception", exception);
                sendActivity.AddTag("db.cosmosdb.status_code", "503");
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last();

                Assert.IsNotNull(telemetry);
                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);

                DependencyTelemetry dependency = telemetry as DependencyTelemetry;
                Assert.IsFalse(dependency.Success.Value);
                Assert.AreEqual(exception.ToInvariantString(), dependency.Properties["Error"]);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), dependency.Id);
                Assert.AreEqual("container | ReadItems", dependency.Name);
                Assert.AreEqual("my.documents.azure.com | database", dependency.Target);
                Assert.AreEqual("503", dependency.ResultCode);
                Assert.AreEqual("Microsoft.DocumentDB", dependency.Type);
                Assert.IsTrue(String.IsNullOrEmpty(dependency.Data));

                Assert.IsTrue(dependency.Properties.ContainsKey("db.name"));
                Assert.IsTrue(dependency.Properties.ContainsKey("db.operation"));
                Assert.IsTrue(dependency.Properties.ContainsKey("net.peer.name"));
                Assert.IsTrue(dependency.Properties.ContainsKey("db.cosmosdb.container"));
                Assert.AreEqual("container", dependency.Properties["db.cosmosdb.container"]);
                Assert.AreEqual("database", dependency.Properties["db.name"]);
                Assert.AreEqual("ReadItems", dependency.Properties["db.operation"]);
                Assert.AreEqual("my.documents.azure.com", dependency.Properties["net.peer.name"]);
            }
        }

        [TestMethod]
        public void AzureCosmosDbSpansErrorsAreCollected()
        {
            using (var listener = new DiagnosticListener("Azure.Cosmos"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new InvalidOperationException();
                Activity sendActivity = new Activity("Azure.Cosmos.ReadItems")
                    .AddTag("net.peer.name", "my.documents.azure.com")
                    .AddTag("db.name", "database")
                    .AddTag("db.operation", "ReadItems")
                    .AddTag("db.cosmosdb.container", "container")
                    .AddTag("kind", "client")
                    .AddTag("az.namespace", "Microsoft.DocumentDB");

                listener.StartActivity(sendActivity, null);

                sendActivity.AddTag("db.cosmosdb.status_code", "200");
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last();

                Assert.IsNotNull(telemetry);
                Assert.IsNull(telemetry.Context.Operation.ParentId);
                Assert.AreEqual(sendActivity.TraceId.ToHexString(), telemetry.Context.Operation.Id);

                DependencyTelemetry dependency = telemetry as DependencyTelemetry;
                Assert.IsTrue(dependency.Success.Value);
                Assert.AreEqual(sendActivity.SpanId.ToHexString(), dependency.Id);
                Assert.AreEqual("container | ReadItems", dependency.Name);
                Assert.AreEqual("my.documents.azure.com | database", dependency.Target);
                Assert.AreEqual("200", dependency.ResultCode);
                Assert.AreEqual("Microsoft.DocumentDB", dependency.Type);
            }
        }

        [TestMethod]
        public void AzureCosmosDbSpansAreCollectedWithExtraAttributes()
        {
            using (var listener = new DiagnosticListener("Azure.Cosmos"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new InvalidOperationException();
                Activity sendActivity = new Activity("Azure.Cosmos.ReadItems")
                    .AddTag("kind", "client")
                    .AddTag("net.peer.name", "my.documents.azure.com")
                    .AddTag("db.name", "database")
                    .AddTag("db.operation", "ReadItems")
                    .AddTag("db.cosmosdb.container", "container")
                    .AddTag("db.cosmosdb.retry_count", "2")
                    .AddTag("db.cosmosdb.connection_mode", "Direct")
                    .AddTag("db.cosmosdb.item_count", "42")
                    .AddTag("db.cosmosdb.request_charge", "0.123")
                    .AddTag("foo", "bar")
                    .AddTag("az.namespace", "Microsoft.DocumentDB");

                listener.StartActivity(sendActivity, null);
                sendActivity.AddTag("db.cosmosdb.status_code", "503");
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last();
                DependencyTelemetry dependency = telemetry as DependencyTelemetry;
                Assert.AreEqual("container | ReadItems", dependency.Name);
                Assert.AreEqual("my.documents.azure.com | database", dependency.Target);
                Assert.AreEqual("503", dependency.ResultCode);
                Assert.AreEqual("Microsoft.DocumentDB", dependency.Type);
                Assert.AreEqual("2", dependency.Properties["db.cosmosdb.retry_count"]);
                Assert.AreEqual("0.123", dependency.Properties["db.cosmosdb.request_charge"]);
                Assert.AreEqual("Direct", dependency.Properties["db.cosmosdb.connection_mode"]);
                Assert.AreEqual("42", dependency.Properties["db.cosmosdb.item_count"]);

                Assert.AreEqual("container", dependency.Properties["db.cosmosdb.container"]);
                Assert.AreEqual("database", dependency.Properties["db.name"]);
                Assert.AreEqual("ReadItems", dependency.Properties["db.operation"]);
                Assert.AreEqual("my.documents.azure.com", dependency.Properties["net.peer.name"]);
                Assert.IsFalse(dependency.Properties.ContainsKey("foo"));
                Assert.IsFalse(dependency.Properties.ContainsKey("db.system"));
            }
        }

        [TestMethod]
        public void AzureCosmosDbInternalSpansHaveInProcType()
        {
            using (var listener = new DiagnosticListener("Azure.Cosmos"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.Cosmos.ReadItems")
                    .AddTag("kind", "internal")
                    .AddTag("net.peer.name", "my.documents.azure.com")
                    .AddTag("db.name", "database")
                    .AddTag("db.operation", "ReadItems")
                    .AddTag("db.cosmosdb.container", "container")
                    .AddTag("az.namespace", "Microsoft.DocumentDB");

                listener.StartActivity(sendActivity, null);
                sendActivity.AddTag("db.cosmosdb.status_code", "200");
                listener.StopActivity(sendActivity, null);

                var telemetry = this.sentItems.Last();
                DependencyTelemetry dependency = telemetry as DependencyTelemetry;
                Assert.AreEqual("container | ReadItems", dependency.Name);
                Assert.AreEqual("my.documents.azure.com | database", dependency.Target);
                Assert.AreEqual("200", dependency.ResultCode);
                Assert.AreEqual("InProc | Microsoft.DocumentDB", dependency.Type);
                Assert.AreEqual("database", dependency.Properties["db.name"]);
                Assert.AreEqual("ReadItems", dependency.Properties["db.operation"]);
                Assert.AreEqual("my.documents.azure.com", dependency.Properties["net.peer.name"]);
            }
        }

        [TestMethod]
        public void AzureCosmosDbRequestsAreNotCollected()
        {
            using (var operations = new DiagnosticListener("Azure.Cosmos.Operation"))
            using (var requests = new DiagnosticListener("Azure.Cosmos.Request"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                Activity sendActivity = new Activity("Azure.Cosmos.Operation.ReadItems")
                    .AddTag("kind", "client")
                    .AddTag("net.peer.name", "my.documents.azure.com")
                    .AddTag("db.name", "database")
                    .AddTag("db.operation", "ReadItems")
                    .AddTag("db.cosmosdb.container", "container")
                    .AddTag("az.namespace", "Microsoft.DocumentDB");

                operations.StartActivity(sendActivity, null);

                Activity rntbdCall = new Activity("Azure.Cosmos.Request.Read")
                                    .AddTag("kind", "client")
                                    .AddTag("net.peer.name", "my.documents.azure.com")
                                    .AddTag("db.name", "database")
                                    .AddTag("db.operation", "ReadItems")
                                    .AddTag("db.cosmosdb.container", "container")
                                    .AddTag("az.namespace", "Microsoft.DocumentDB");
                requests.StartActivity(rntbdCall, null);

                rntbdCall.AddTag("db.cosmosdb.status_code", "200");
                sendActivity.AddTag("db.cosmosdb.status_code", "200");
                
                requests.StopActivity(rntbdCall, null);
                operations.StopActivity(sendActivity, null);

                Assert.AreEqual(1, this.sentItems.Count);
                var operation = this.sentItems.Last();
                DependencyTelemetry dependency = operation as DependencyTelemetry;
                Assert.AreEqual("container | ReadItems", dependency.Name);
                Assert.AreEqual("ReadItems", dependency.Properties["db.operation"]);
            }
        }

#if !NET452
        [TestMethod]
        public void AzureCosmosDbSpansAreCollectedWithLogs()
        {
            // .NET452 does not support setting custom EventSource and Cosmos DB SDK does not have targets below net461
            // so no point in testing net452.
            // but we still need to test .NET framework versions that support setting event source name
            // but fallback to depednencycollector dependency net452 target 
            using (var listener = new DiagnosticListener("Azure.Cosmos"))
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new InvalidOperationException();
                Activity sendActivity = new Activity("Azure.Cosmos.ReadItems")
                    .AddTag("kind", "client")
                    .AddTag("net.peer.name", "my.documents.azure.com")
                    .AddTag("db.name", "database")
                    .AddTag("db.operation", "ReadItems")
                    .AddTag("db.cosmosdb.container", "container")
                    .AddTag("az.namespace", "Microsoft.DocumentDB");

                listener.StartActivity(sendActivity, null);

                CosmosDbEventSource.Singleton.RecordVerbose("verbose message - ignored");
                CosmosDbEventSource.Singleton.RecordInfo("info message");
                CosmosDbEventSource.Singleton.RecordWarn("warn message");
                CosmosDbEventSource.Singleton.RecordError("error message");
                CosmosDbEventSource.Singleton.RecordWarnNoMessage("payload only");
                CosmosDbEventSource.Singleton.RecordWarnNoMessageTwoArguments("payload1", "payload2");
                CosmosDbEventSource.Singleton.RecordWarnNoMessageTwoArguments("payload1", null);
                listener.StopActivity(sendActivity, null);

                var dependency = this.sentItems.Single(t => t is DependencyTelemetry) as DependencyTelemetry;
                var logs = this.sentItems
                    .Where(t => t is TraceTelemetry)
                    .Select(t => t as TraceTelemetry)
                    .ToList();

                Assert.IsTrue(dependency.Success.Value);
                Assert.IsTrue(String.IsNullOrEmpty(dependency.ResultCode));
                Assert.AreEqual(6, logs.Count);

                Assert.AreEqual("info message", logs[0].Message);
                Assert.AreEqual("warn message", logs[1].Message);
                Assert.AreEqual("error message", logs[2].Message);
                Assert.AreEqual("payload only", logs[3].Message);
                Assert.AreEqual("payload1, payload2", logs[4].Message);
                Assert.AreEqual("payload1, ", logs[5].Message);

                Assert.AreEqual(SeverityLevel.Information, logs[0].SeverityLevel);
                Assert.AreEqual(SeverityLevel.Warning, logs[1].SeverityLevel);
                Assert.AreEqual(SeverityLevel.Error, logs[2].SeverityLevel);
                
                Assert.AreEqual(dependency.Id, logs[0].Context.Operation.ParentId);
                Assert.AreEqual(dependency.Id, logs[1].Context.Operation.ParentId);
                Assert.AreEqual(dependency.Id, logs[2].Context.Operation.ParentId);

                Assert.AreEqual(dependency.Context.Operation.Id, logs[0].Context.Operation.Id);
                Assert.AreEqual(dependency.Context.Operation.Id, logs[1].Context.Operation.Id);
                Assert.AreEqual(dependency.Context.Operation.Id, logs[2].Context.Operation.Id);

                Assert.AreEqual("3", logs[0].Properties["EventId"]);
                Assert.AreEqual("2", logs[1].Properties["EventId"]);
                Assert.AreEqual("1", logs[2].Properties["EventId"]);

#if NET5_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                // DependencyCollector has net452 and netstandard20 targets
                // test targets that falls back to net452 dependency would not have EventName available
                // because EventSource on .NET 4.5.2 does not support it
                Assert.AreEqual("RecordInfo", logs[0].Properties["EventName"]);
                Assert.AreEqual("RecordWarn", logs[1].Properties["EventName"]);
                Assert.AreEqual("RecordError", logs[2].Properties["EventName"]);
#endif
            }
        }
#endif

        private T TrackOperation<T>(
            DiagnosticListener listener,
            string activityName,
            string parentId = null,
            Action operation = null) where T : OperationTelemetry
        {
            Activity activity = null;
            int itemCountBefore = this.sentItems.Count;

            if (listener.IsEnabled(activityName))
            {
                activity = new Activity(activityName);

                if (Activity.Current == null && parentId != null)
                {
                    activity.SetParentId(parentId);
                }

                listener.StartActivity(activity, null);
            }

            operation?.Invoke();

            if (activity != null)
            {
                listener.StopActivity(activity, null);

                // a single new telemetry item was addedAssert.AreEqual(itemCountBefore + 1, this.sentItems.Count);
                return this.sentItems.Last() as T;
            }

            // no new telemetry items were added
            Assert.AreEqual(itemCountBefore, this.sentItems.Count);
            return null;
        }

        private Activity CreateRandomLink(long enqueuedTimeMs)
        {
            var activity = new Activity("foo").SetParentId(ActivityTraceId.CreateRandom(), ActivitySpanId.CreateRandom(), ActivityTraceFlags.None);
            activity.AddTag("enqueuedTime", enqueuedTimeMs.ToString());

            return activity;
        }

        private long ToUnixTimeStamp(DateTimeOffset datetime)
        {
#if NET452
           return (long)(datetime - EpochStart).TotalMilliseconds;
#else
           return datetime.ToUnixTimeMilliseconds();
#endif
        }

        private class PayloadWithLinks
        {
            public IEnumerable<Activity> Links { get; set; }
        }

        private class ApplicationInsightsLink
        {
            [JsonProperty("operation_Id")]
            public string OperationId { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
        }

#if !NET452
        class CosmosDbEventSource : EventSource
        {
            private CosmosDbEventSource()
                : base("Azure-Cosmos-Operation-Request-Diagnostics")
            {
            }

            public static CosmosDbEventSource Singleton { get; } = new CosmosDbEventSource();

            [Event(1, Level = EventLevel.Error, Message = "{0}")]
            public void RecordError(string diagnostics)
            {
                this.WriteEvent(1, diagnostics);
            }

            [Event(2, Level = EventLevel.Warning, Message = "{0}")]
            public void RecordWarn(string diagnostics)
            {
                this.WriteEvent(2, diagnostics);
            }

            [Event(3, Level = EventLevel.Informational, Message = "{0}")]
            public void RecordInfo(string diagnostics)
            {
                this.WriteEvent(3, diagnostics);
            }

            [Event(4, Level = EventLevel.Warning)]
            public void RecordWarnNoMessage(string diagnostics)
            {
                this.WriteEvent(4, diagnostics);
            }

            [Event(5, Level = EventLevel.Warning)]
            public void RecordWarnNoMessageTwoArguments(string diagnostics1, string diagnostics2)
            {
                this.WriteEvent(5, diagnostics1, diagnostics2);
            }


            [Event(6, Level = EventLevel.Verbose, Message = "{0}")]
            public void RecordVerbose(string diagnostics)
            {
                this.WriteEvent(6, diagnostics);
            }
        }
#endif
    }
}
//-----------------------------------------------------------------------
// <copyright file="EtwTelemetryModuleTests.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwTelemetryCollector.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.EtwCollector;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Tracing.Tests;
    using Microsoft.Diagnostics.Tracing.Session;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static System.Globalization.CultureInfo;

    [TestClass]
    public sealed class EtwTelemetryModuleTests : IDisposable
    {
        private const int NoEventSourcesConfiguredEventId = 1;
        private const int ModuleInitializationFailedEventId = 3;
        private const int AccessDeniedEventId = 4;

        private readonly AdapterHelper adapterHelper = new AdapterHelper();
        private static bool isTestEnvGood;

        public void Dispose()
        {
            this.adapterHelper.Dispose();
        }

        private TelemetryConfiguration GetTestTelemetryConfiguration(bool resetChannel = true)
        {
            var configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = this.adapterHelper.InstrumentationKey;
            if (resetChannel)
            {
                configuration.TelemetryChannel = this.adapterHelper.Channel.Reset();
            }
            else
            {
                configuration.TelemetryChannel = this.adapterHelper.Channel;
            }

            return configuration;
        }

        [ClassInitialize]
#pragma warning disable CA1801 // Review unused parameters
        public static void InitializeClass(TestContext testContext)
#pragma warning restore CA1801 // Review unused parameters
        {
            // Only users with administrative privileges, users in the Performance Log Users group,
            // and services running as LocalSystem, LocalService, or NetworkService can enable trace providers
            bool? isElevated = TraceEventSession.IsElevated();
            if (isElevated.HasValue && isElevated.Value)
            {
                EtwTelemetryModuleTests.isTestEnvGood = true;
                return;
            }

            foreach (IdentityReference group in WindowsIdentity.GetCurrent().Groups)
            {
                string groupName = group.Translate(typeof(NTAccount)).Value;
                if (groupName.Equals(@"BuiltIn\Performance Log Users", StringComparison.InvariantCultureIgnoreCase))
                {
                    EtwTelemetryModuleTests.isTestEnvGood = true;
                    return;
                }
            }


        }

        [TestInitialize]
        public void InitializeTest()
        {
            if (!EtwTelemetryModuleTests.isTestEnvGood)
            {
                Assert.Inconclusive("Test environment is not fit. Possible not enough permission to enable providers for ETW.");
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void DefaultConstructorExists()
        {
            using (EtwTelemetryModule module = new EtwTelemetryModule())
            {
                Assert.IsNotNull(module, "There has to be a default constructor, which has no parameter.");
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void InitializeFailedWhenConfigurationIsNull()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            using (EtwTelemetryModule module = new EtwTelemetryModule())
            {
                module.Initialize(null);
                Assert.AreEqual(1, listener.EventsReceived.Count);
                Assert.AreEqual(ModuleInitializationFailedEventId, listener.EventsReceived[0].EventId);
                Assert.AreEqual("Argument configuration is required. The initialization is terminated.", listener.EventsReceived[0].Payload[1].ToString());
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void InitializeFailedWhenDisposed()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            {
                EtwTelemetryModule module = new EtwTelemetryModule();
                module.Dispose();
                module.Initialize(GetTestTelemetryConfiguration());

                Assert.AreEqual(1, listener.EventsReceived.Count);
                Assert.AreEqual(ModuleInitializationFailedEventId, listener.EventsReceived[0].EventId);
                Assert.AreEqual("Can't initialize a module that is disposed. The initialization is terminated.", listener.EventsReceived[0].Payload[1].ToString());
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void InitializeFailedWhenSourceIsNotSpecified()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            using (TraceEventSessionMock traceEventSession = new TraceEventSessionMock(false))
            using (EtwTelemetryModule module = new EtwTelemetryModule(() => traceEventSession))
            {
                module.Initialize(GetTestTelemetryConfiguration());
                Assert.AreEqual(1, listener.EventsReceived.Count);
                Assert.AreEqual(NoEventSourcesConfiguredEventId, listener.EventsReceived[0].EventId);
                Assert.AreEqual("EtwTelemetryModule", listener.EventsReceived[0].Payload[1].ToString());
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void InitializeFailedWhenAccessDenied()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            using (TraceEventSessionMock traceEventSession = new TraceEventSessionMock(true))
            using (EtwTelemetryModule module = new EtwTelemetryModule(() => traceEventSession))
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = "Test Provider",
                    Level = Diagnostics.Tracing.TraceEventLevel.Always
                });
                module.Initialize(GetTestTelemetryConfiguration());
                // There will be 2 events because we also enable TPL EventSource to get hierarchical activity IDs.
                Assert.AreEqual(2, listener.EventsReceived.Count);
                Assert.AreEqual(AccessDeniedEventId, listener.EventsReceived[0].EventId);
                Assert.AreEqual("Access Denied.", listener.EventsReceived[0].Payload[1].ToString());
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void InitializeSucceed()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            using (TraceEventSessionMock traceEventSession = new TraceEventSessionMock(false))
            using (EtwTelemetryModule module = new EtwTelemetryModule(() => traceEventSession))
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = "Test Provider",
                    Level = Diagnostics.Tracing.TraceEventLevel.Always
                });
                module.Initialize(GetTestTelemetryConfiguration());
                Assert.AreEqual(0, listener.EventsReceived.Count);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void ProviderEnabledByName()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            using (TraceEventSessionMock traceEventSession = new TraceEventSessionMock(false))
            using (EtwTelemetryModule module = new EtwTelemetryModule(() => traceEventSession))
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = "Test Provider",
                    Level = Diagnostics.Tracing.TraceEventLevel.Always
                });
                module.Initialize(GetTestTelemetryConfiguration());
                Assert.AreEqual(1, traceEventSession.EnabledProviderNames.Count);
                Assert.AreEqual("Test Provider", traceEventSession.EnabledProviderNames[0]);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void ProviderEnabledByGuid()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            using (TraceEventSessionMock traceEventSession = new TraceEventSessionMock(false))
            using (EtwTelemetryModule module = new EtwTelemetryModule(() => traceEventSession))
            {
                Guid guid = Guid.NewGuid();
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderGuid = guid,
                    Level = Diagnostics.Tracing.TraceEventLevel.Always
                });
                module.Initialize(GetTestTelemetryConfiguration());
                Assert.AreEqual(2, traceEventSession.EnabledProviderGuids.Count);
                // First enabled provider is the TPL provider
                Assert.AreEqual(guid.ToString(), traceEventSession.EnabledProviderGuids[1].ToString());
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public void ProviderNotEnabledByEmptyGuid()
        {
            using (EventSourceModuleDiagnosticListener listener = new EventSourceModuleDiagnosticListener())
            using (TraceEventSessionMock traceEventSession = new TraceEventSessionMock(false))
            using (EtwTelemetryModule module = new EtwTelemetryModule(() => traceEventSession))
            {
                Guid guid = Guid.Empty;
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderGuid = guid,
                    Level = Diagnostics.Tracing.TraceEventLevel.Always
                });
                module.Initialize(GetTestTelemetryConfiguration());
                Assert.IsFalse(traceEventSession.EnabledProviderGuids.Any(g => Guid.Empty.Equals(g)));
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task ReportSingleEvent()
        {
            using (EtwTelemetryModule module = new EtwTelemetryModule())
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = TestProvider.ProviderName
                });
                module.Initialize(GetTestTelemetryConfiguration());

                TestProvider.Log.Info("Hello!");
                int expectedEventCount = 2;
                await WaitForItems(adapterHelper.Channel, expectedEventCount).ConfigureAwait(false);

                // The very 1st event is for the manifest.
                Assert.AreEqual(expectedEventCount, this.adapterHelper.Channel.SentItems.Length);
                TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems[1];
                Assert.AreEqual("Hello!", telemetry.Message);
            }
        }


        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task ReportMultipleEvents()
        {
            using (EtwTelemetryModule module = new EtwTelemetryModule())
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = TestProvider.ProviderName
                });
                module.Initialize(GetTestTelemetryConfiguration());
                TestProvider.Log.Info("Hello!");
                TestProvider.Log.Info("World!");

                int expectedEventCount = 3;
                await WaitForItems(adapterHelper.Channel, expectedEventCount).ConfigureAwait(false);

                // The very 1st event is for the manifest.
                Assert.AreEqual(expectedEventCount, this.adapterHelper.Channel.SentItems.Length);
                TraceTelemetry hello = (TraceTelemetry)this.adapterHelper.Channel.SentItems[1];
                TraceTelemetry world = (TraceTelemetry)this.adapterHelper.Channel.SentItems[2];
                Assert.AreEqual("Hello!", hello.Message);
                Assert.AreEqual("World!", world.Message);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task ReportsAllProperties()
        {
            using (var module = new EtwTelemetryModule())
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = TestProvider.ProviderName
                });
                module.Initialize(GetTestTelemetryConfiguration());

                Guid eventId = new Guid("30ba9220-89a4-41e4-987c-9e27ade44b74");
                Guid activityId = new Guid("0724a028-27d7-40a9-a299-acf79ff0db94");
                EventSource.SetCurrentThreadActivityId(activityId);
                TestProvider.Log.Complex(eventId);

                // There's going to be a delay around 2000ms before the events reaches the channel.
                int expectedEventCount = 2;
                await this.WaitForItems(this.adapterHelper.Channel, expectedEventCount).ConfigureAwait(false);

                Assert.AreEqual(expectedEventCount, this.adapterHelper.Channel.SentItems.Length);
                TraceTelemetry actual = (TraceTelemetry)this.adapterHelper.Channel.SentItems[1];
                TraceTelemetry expected = new TraceTelemetry("Blah blah", SeverityLevel.Verbose);

                Assert.AreEqual("Blah blah", actual.Message);
                Assert.AreEqual(SeverityLevel.Verbose, actual.SeverityLevel);
                Assert.AreEqual(eventId.ToString(), actual.Properties["uniqueId"]);
                Assert.AreEqual(TestProvider.ComplexEventId.ToString(InvariantCulture), actual.Properties["EventId"]);
                Assert.AreEqual(nameof(TestProvider.Complex) + "/Extension", actual.Properties["EventName"]);
                Assert.AreEqual(activityId.ToString(), actual.Properties["ActivityID"]);
                Assert.AreEqual("0x8000F00000000001", actual.Properties["Keywords"]);
                Assert.AreEqual(((int)EventChannel.Debug).ToString(InvariantCulture), actual.Properties["Channel"]);
                Assert.AreEqual("Extension", actual.Properties["Opcode"]);
                Assert.AreEqual("0x00000020", actual.Properties["Task"]);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task ReportSeverityLevel()
        {
            //bool isCalled = false;
            // using (TestProvider provider = new TestProvider())
            using (EtwTelemetryModule module = new EtwTelemetryModule())
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = TestProvider.ProviderName
                });
                module.Initialize(GetTestTelemetryConfiguration());
                TestProvider.Log.Info("Hello!");
                TestProvider.Log.Warning(1, 2);

                int expectedEventCount = 3;
                await this.WaitForItems(this.adapterHelper.Channel, expectedEventCount).ConfigureAwait(false);

                // The very 1st event is for the manifest.
                Assert.AreEqual(expectedEventCount, this.adapterHelper.Channel.SentItems.Length);
                Assert.AreEqual(SeverityLevel.Information, ((TraceTelemetry)this.adapterHelper.Channel.SentItems[1]).SeverityLevel);
                Assert.AreEqual(SeverityLevel.Warning, ((TraceTelemetry)this.adapterHelper.Channel.SentItems[2]).SeverityLevel);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task HandlesDuplicatePropertyNames()
        {
            using (var module = new EtwTelemetryModule())
            {
                module.Sources.Add(new EtwListeningRequest()
                {
                    ProviderName = TestProvider.ProviderName
                });
                module.Initialize(GetTestTelemetryConfiguration());

                TestProvider.Log.Tricky(7, "TrickyEvent", "Actual message");

                int expectedEventCount = 2;
                await this.WaitForItems(this.adapterHelper.Channel, expectedEventCount).ConfigureAwait(false);

                Assert.AreEqual(expectedEventCount, this.adapterHelper.Channel.SentItems.Length);
                TraceTelemetry telemetry = (TraceTelemetry)this.adapterHelper.Channel.SentItems[1];

                Assert.AreEqual("Manifest message", telemetry.Message);
                Assert.AreEqual(SeverityLevel.Information, telemetry.SeverityLevel);
                Assert.AreEqual("Actual message", telemetry.Properties["Message"]);
                Assert.AreEqual("7", telemetry.Properties["EventId"]);
                Assert.AreEqual("Tricky", telemetry.Properties["EventName"]);
                Assert.AreEqual("7", telemetry.Properties[telemetry.Properties.Keys.First(key => key.StartsWith("EventId", StringComparison.Ordinal) && !string.Equals(key, "EventId", StringComparison.Ordinal))]);
                Assert.AreEqual("TrickyEvent", telemetry.Properties[telemetry.Properties.Keys.First(key => key.StartsWith("EventName", StringComparison.Ordinal) && !string.Equals(key, "EventName", StringComparison.Ordinal))]);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task ReactsToConfigurationChanges()
        {
            using (var module = new EtwTelemetryModule())
            {
                var listeningRequest = new EtwListeningRequest();
                listeningRequest.ProviderName = TestProvider.ProviderName;
                module.Sources.Add(listeningRequest);

                module.Initialize(GetTestTelemetryConfiguration());

                TestProvider.Log.Info("Hey!");
                TestProvider.Log.Warning(1, 2);
                await this.WaitForItems(this.adapterHelper.Channel, 3).ConfigureAwait(false);

                // Now request reporting events only with certain keywords
                listeningRequest.Keywords = (ulong)TestProvider.Keywords.NonRoutine;
                module.Initialize(GetTestTelemetryConfiguration(resetChannel: false));
                await Task.Delay(500).ConfigureAwait(false);

                TestProvider.Log.Info("Hey again!");
                TestProvider.Log.Warning(3, 4);
                await this.WaitForItems(this.adapterHelper.Channel, 5).ConfigureAwait(false);

                List<TraceTelemetry> expectedTelemetry = new List<TraceTelemetry>();
                TraceTelemetry traceTelemetry = new TraceTelemetry("Hey!", SeverityLevel.Information);
                traceTelemetry.Properties["information"] = "Hey!";
                expectedTelemetry.Add(traceTelemetry);
                traceTelemetry = new TraceTelemetry("Warning!", SeverityLevel.Warning);
                traceTelemetry.Properties["i1"] = 1.ToString(InvariantCulture);
                traceTelemetry.Properties["i2"] = 2.ToString(InvariantCulture);
                expectedTelemetry.Add(traceTelemetry);
                // Note that second informational event is not expected
                traceTelemetry = new TraceTelemetry("Warning!", SeverityLevel.Warning);
                traceTelemetry.Properties["i1"] = 3.ToString(InvariantCulture);
                traceTelemetry.Properties["i2"] = 4.ToString(InvariantCulture);
                expectedTelemetry.Add(traceTelemetry);

                CollectionAssert.AreEqual(
                    expectedTelemetry,
                    this.adapterHelper.Channel.SentItems.Where(item => !((TraceTelemetry)item).Properties["EventId"].Equals("65534", StringComparison.Ordinal)).ToList(),
                    new TraceTelemetryComparer(),
                    "Reported events are not what was expected");
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task DoNotReportTplEvents()
        {
            using (var module = new EtwTelemetryModule())
            {
                module.Initialize(GetTestTelemetryConfiguration());

                for (int i = 0; i < 10; i += 2)
                {
                    Parallel.For(0, 2, (idx) =>
                    {
                        PerformActivityAsync(i + idx).GetAwaiter().GetResult();
                    });

                }

                // Wait 2 seconds to see if any events arrive asynchronously through the ETW module.
                // This is a long time but unfortunately there is no good way to make ETW infrastructure "go faster"
                // and we want to make sure that no TPL events sneak through.
                await this.WaitForItems(this.adapterHelper.Channel, 1, TimeSpan.FromSeconds(2)).ConfigureAwait(false);

                Assert.AreEqual(0, this.adapterHelper.Channel.SentItems.Length);
            }
        }

        [TestMethod]
        [TestCategory("EtwTelemetryModule")]
        public async Task ReportsHierarchicalActivities()
        {
            using (var module = new EtwTelemetryModule())
            {
                var listeningRequest = new EtwListeningRequest();
                listeningRequest.ProviderName = TestProvider.ProviderName;
                module.Sources.Add(listeningRequest);

                module.Initialize(GetTestTelemetryConfiguration());

                for (int i = 0; i < 6; i += 2)
                {
                    Parallel.For(0, 2, (idx) =>
                    {
                        PerformActivityAsync(i + idx).GetAwaiter().GetResult();
                    });
                }

                await this.WaitForItems(this.adapterHelper.Channel, 12).ConfigureAwait(false);

                ITelemetry[] capturedItems = this.adapterHelper.Channel.SentItems;
                TraceTelemetry requestStopEvent = capturedItems.OfType<TraceTelemetry>().FirstOrDefault((i) =>
                    i.Properties.TryGetValue("EventName", out string eventName)
                    && string.Equals(eventName, "Request/Stop", StringComparison.Ordinal));
                Assert.IsNotNull(requestStopEvent, "Request/Stop event not found");
                Assert.IsTrue(requestStopEvent.Properties.TryGetValue("ActivityID", out string activityID), "Event does not have ActivityID property");
                Assert.IsTrue(activityID.StartsWith("//", StringComparison.Ordinal), "The activity ID is not a hierarchical one");
            }
        }

        private Task PerformActivityAsync(int requestId)
        {
            return Task.Run(async () =>
            {
                TestProvider.Log.RequestStart(requestId);
                await Task.Delay(50).ConfigureAwait(false);
                TestProvider.Log.RequestStop(requestId);
            });
        }

        /// <summary>
        /// Wait until we caputred <paramref name="count"/> telemetry items or timeout in <see cref="CustomTelemetryChannel" />.
        /// </summary>
        /// <param name="channel">Custom telemtry channel.</param>
        /// <param name="count">Number of telemetry items to wait for</param>
        /// <param name="timeout">Timeout for waiting on the desired number of items.</param>
        /// <returns>A task, which fulfills when given number of items is captured.</returns>
        private async Task WaitForItems(CustomTelemetryChannel channel, int count = 1, TimeSpan? timeout = null)
        {
            // Use 30 seconds by default in case the expected event didn't arrive to avoid hanging on the test execution.
            timeout = timeout ?? TimeSpan.FromSeconds(30);
            DateTime start = DateTime.Now;
            int? itemsCaptured;

            do
            {
                if (DateTime.Now - start > timeout)
                {
                    // Exceeded allocated time.
                    return;
                }

                itemsCaptured = await channel.WaitForItemsCaptured(timeout.Value).ConfigureAwait(false);
                if (itemsCaptured == null)
                {
                    // Timed out waiting for new events to arrive.
                    return;
                }
            }
            while (itemsCaptured.Value < count);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                // This clean up is there to clean up possible left over trace event sessions during the Debug of the unit tests.
                foreach (var name in TraceEventSession.GetActiveSessionNames().Where(n => n.StartsWith("ApplicationInsights-", StringComparison.Ordinal)))
                {
                    TraceEventSession.GetActiveSession(name).Stop();
                }
            }
            catch
            {
                // This should normally not happen. But if this happens, there's, unfortunately, nothing too much we can do here.
            }
        }
    }
}

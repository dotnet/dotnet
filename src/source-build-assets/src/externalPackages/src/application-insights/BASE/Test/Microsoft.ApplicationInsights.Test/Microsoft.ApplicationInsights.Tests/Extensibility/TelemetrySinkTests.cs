namespace Microsoft.ApplicationInsights.Extensibility
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class TelemetrySinkTests
    {
        [TestMethod]
        public void CommonTelemetryProcessorsAreInvoked()
        {
            var configuration = new TelemetryConfiguration();

            var sentTelemetry = new List<ITelemetry>(1);
            var channel = new StubTelemetryChannel();
            channel.OnSend = (telemetry) => sentTelemetry.Add(telemetry);
            configuration.TelemetryChannel = channel;

            var chainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = chainBuilder;
            chainBuilder.Use((next) =>
            {
                var first = new StubTelemetryProcessor(next);
                first.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenByFirst", "true");
                return first;
            });
            chainBuilder.Use((next) =>
            {
                var second = new StubTelemetryProcessor(next);
                second.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenBySecond", "true");
                return second;
            });

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");

            Assert.AreEqual(1, sentTelemetry.Count);
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByFirst"));
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenBySecond"));
        }

        [TestMethod]
        public void SinkProcessorsAreInvoked()
        {
            var configuration = new TelemetryConfiguration();

            var sentTelemetry = new List<ITelemetry>(1);
            var channel = new StubTelemetryChannel();
            channel.OnSend = (telemetry) => sentTelemetry.Add(telemetry);
            configuration.TelemetryChannel = channel;

            var chainBuilder = new TelemetryProcessorChainBuilder(configuration, configuration.DefaultTelemetrySink);
            configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder = chainBuilder;
            chainBuilder.Use((next) =>
            {
                var first = new StubTelemetryProcessor(next);
                first.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenByFirst", "true");
                return first;
            });
            chainBuilder.Use((next) =>
            {
                var second = new StubTelemetryProcessor(next);
                second.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenBySecond", "true");
                return second;
            });

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");

            Assert.IsFalse(configuration.TelemetryProcessors.OfType<StubTelemetryProcessor>().Any()); // Both processors belong to the sink, not to the common chain.
            Assert.AreEqual(1, sentTelemetry.Count);
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByFirst"));
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenBySecond"));
        }

        [TestMethod]
        public void CommonAndSinkProcessorsAreInvoked()
        {
            var configuration = new TelemetryConfiguration();

            var sentTelemetry = new List<ITelemetry>(1);
            var channel = new StubTelemetryChannel();
            channel.OnSend = (telemetry) => sentTelemetry.Add(telemetry);
            configuration.TelemetryChannel = channel;

            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;
            commonChainBuilder.Use((next) =>
            {
                var commonProcessor = new StubTelemetryProcessor(next);
                commonProcessor.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenByCommonProcessor", "true");
                return commonProcessor;
            });

            var sinkChainBuilder = new TelemetryProcessorChainBuilder(configuration, configuration.DefaultTelemetrySink);
            configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder = sinkChainBuilder;
            sinkChainBuilder.Use((next) =>
            {
                var sinkProcessor = new StubTelemetryProcessor(next);
                sinkProcessor.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenBySinkProcessor", "true");
                return sinkProcessor;
            });

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");

            Assert.AreEqual(1, sentTelemetry.Count);
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByCommonProcessor"));
            Assert.IsTrue(sentTelemetry[0].Context.GlobalProperties.ContainsKey("SeenBySinkProcessor"));
        }

        [TestMethod]
        public void ReplacingTelemetryChannelOnConfiguraitonReplacesItForDefaultSink()
        {
            var configuration = new TelemetryConfiguration();

            var firstSentTelemetry = new List<ITelemetry>(1);
            var firstChannel = new StubTelemetryChannel();
            firstChannel.OnSend = (telemetry) => firstSentTelemetry.Add(telemetry);
            configuration.TelemetryChannel = firstChannel;

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");

            Assert.AreEqual(1, firstSentTelemetry.Count);

            var secondSentTelemetry = new List<ITelemetry>(1);
            var secondChannel = new StubTelemetryChannel();
            secondChannel.OnSend = (telemetry) => secondSentTelemetry.Add(telemetry);
            configuration.TelemetryChannel = secondChannel;

            client.TrackTrace("t1");

            Assert.AreEqual(1, firstSentTelemetry.Count);
            Assert.AreEqual(1, secondSentTelemetry.Count);
        }

        [TestMethod]
        public void TelemetryIsDeliveredToMultipleSinks()
        {
            var configuration = new TelemetryConfiguration();

            var firstChannelTelemetry = new List<ITelemetry>();
            var firstChannel = new StubTelemetryChannel();
            firstChannel.OnSend = (telemetry) => firstChannelTelemetry.Add(telemetry);
            configuration.DefaultTelemetrySink.TelemetryChannel = firstChannel;
            var chainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = chainBuilder;

            var secondChannelTelemetry = new List<ITelemetry>();
            var secondChannel = new StubTelemetryChannel();
            secondChannel.OnSend = (telemetry) => secondChannelTelemetry.Add(telemetry);
            var secondSink = new TelemetrySink(configuration, secondChannel);
            configuration.TelemetrySinks.Add(secondSink);

            var thirdChannelTelemetry = new List<ITelemetry>();
            var thirdChannel = new StubTelemetryChannel();
            thirdChannel.OnSend = (telemetry) => thirdChannelTelemetry.Add(telemetry);
            var thirdSink = new TelemetrySink(configuration, thirdChannel);
            configuration.TelemetrySinks.Add(thirdSink);

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");

            Assert.AreEqual(1, firstChannelTelemetry.Count);
            Assert.AreEqual("t1", ((TraceTelemetry)firstChannelTelemetry[0]).Message);
            Assert.AreEqual(1, secondChannelTelemetry.Count);
            Assert.AreEqual("t1", ((TraceTelemetry)secondChannelTelemetry[0]).Message);
            Assert.AreEqual(1, thirdChannelTelemetry.Count);
            Assert.AreEqual("t1", ((TraceTelemetry)thirdChannelTelemetry[0]).Message);
        }

        [TestMethod]
        public void MultipleSinkTelemetryProcessorsAreInvoked()
        {
            var configuration = new TelemetryConfiguration();

            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;
            commonChainBuilder.Use((next) =>
            {
                var commonProcessor = new StubTelemetryProcessor(next);
                commonProcessor.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenByCommonProcessor", "true");
                return commonProcessor;
            });

            var firstChannelTelemetry = new List<ITelemetry>();
            var firstChannel = new StubTelemetryChannel();
            firstChannel.OnSend = (telemetry) => firstChannelTelemetry.Add(telemetry);
            configuration.DefaultTelemetrySink.TelemetryChannel = firstChannel;

            var firstSinkChainBuilder = new TelemetryProcessorChainBuilder(configuration, configuration.DefaultTelemetrySink);
            firstSinkChainBuilder.Use((next) =>
            {
                var firstSinkTelemetryProcessor = new StubTelemetryProcessor(next);
                firstSinkTelemetryProcessor.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenByFirstSinkProcessor", "true");
                return firstSinkTelemetryProcessor;
            });
            configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder = firstSinkChainBuilder;

            var secondChannelTelemetry = new List<ITelemetry>();
            var secondChannel = new StubTelemetryChannel();
            secondChannel.OnSend = (telemetry) => secondChannelTelemetry.Add(telemetry);
            var secondSink = new TelemetrySink(configuration, secondChannel);
            configuration.TelemetrySinks.Add(secondSink);

            var secondSinkChainBuilder = new TelemetryProcessorChainBuilder(configuration, secondSink);
            secondSinkChainBuilder.Use((next) =>
            {
                var secondSinkTelemetryProcessor = new StubTelemetryProcessor(next);
                secondSinkTelemetryProcessor.OnProcess = (telemetry) => telemetry.Context.GlobalProperties.Add("SeenBySecondSinkProcessor", "true");
                return secondSinkTelemetryProcessor;
            });
            secondSink.TelemetryProcessorChainBuilder = secondSinkChainBuilder;

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");

            Assert.AreEqual(1, firstChannelTelemetry.Count);
            Assert.IsTrue(firstChannelTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByCommonProcessor"));
            Assert.IsTrue(firstChannelTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByFirstSinkProcessor"));
            Assert.IsFalse(firstChannelTelemetry[0].Context.GlobalProperties.ContainsKey("SeenBySecondSinkProcessor"));

            Assert.AreEqual(1, secondChannelTelemetry.Count);
            Assert.IsTrue(secondChannelTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByCommonProcessor"));
            Assert.IsFalse(secondChannelTelemetry[0].Context.GlobalProperties.ContainsKey("SeenByFirstSinkProcessor"));
            Assert.IsTrue(secondChannelTelemetry[0].Context.GlobalProperties.ContainsKey("SeenBySecondSinkProcessor"));
        }

        [TestMethod]
        public void ConfigurationDisposesAllSinks()
        {
            var configuration = new TelemetryConfiguration();
            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;

            var firstChannel = new StubTelemetryChannel();
            bool firstChannelDisposed = false;
            firstChannel.OnDispose = () => firstChannelDisposed = true;
            configuration.DefaultTelemetrySink.TelemetryChannel = firstChannel;
            var firstSinkChainBuilder = new TelemetryProcessorChainBuilder(configuration, configuration.DefaultTelemetrySink);
            bool firstSinkTelemetryProcessorDisposed = false;
            firstSinkChainBuilder.Use((next) =>
            {
                var firstSinkTelemetryProcessor = new StubTelemetryProcessor(next);
                firstSinkTelemetryProcessor.OnDispose = () => firstSinkTelemetryProcessorDisposed = true;
                return firstSinkTelemetryProcessor;
            });
            configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder = firstSinkChainBuilder;

            var secondChannel = new StubTelemetryChannel();
            bool secondChannelDisposed = false;
            secondChannel.OnDispose = () => secondChannelDisposed = true;
            var secondSink = new TelemetrySink(configuration, secondChannel);
            var secondSinkChainBuilder = new TelemetryProcessorChainBuilder(configuration, secondSink);
            bool secondSinkTelemetryProcessorDisposed = false;
            secondSinkChainBuilder.Use((next) =>
            {
                var secondSinkTelemetryProcessor = new StubTelemetryProcessor(next);
                secondSinkTelemetryProcessor.OnDispose = () => secondSinkTelemetryProcessorDisposed = true;
                return secondSinkTelemetryProcessor;
            });
            secondSink.TelemetryProcessorChainBuilder = secondSinkChainBuilder;
            configuration.TelemetrySinks.Add(secondSink);

            var client = new TelemetryClient(configuration);
            client.TrackTrace("t1");
            configuration.Dispose();

            // We expect the channels to not be disposed (because they were created externally to sinks), but the processors should be disposed.
            Assert.IsTrue(firstSinkTelemetryProcessorDisposed);
            Assert.IsTrue(secondSinkTelemetryProcessorDisposed);
            Assert.IsFalse(firstChannelDisposed);
            Assert.IsFalse(secondChannelDisposed);

            // does not throw
            try
            {
                client.TrackTrace("t2");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        /// <summary>
        /// Ensures that all the sinks get the full copy of the telemetry context.
        /// This is a test to ensure DeepClone is copying over all the properties.
        /// </summary>
        [TestMethod]
        public void EnsureAllSinksGetFullTelemetryContext()
        {
            var configuration = new TelemetryConfiguration();
            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;

            ITelemetryChannel secondTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    Assert.AreEqual("UnitTest", telemetry.Context.Cloud.RoleName);
                    Assert.AreEqual("TestVersion", telemetry.Context.Component.Version);
                    Assert.AreEqual("TestDeviceId", telemetry.Context.Device.Id);
                    Assert.AreEqual(1234, telemetry.Context.Flags);
                    Assert.AreEqual(Guid.Empty.ToString(), telemetry.Context.InstrumentationKey);
                    Assert.AreEqual("127.0.0.1", telemetry.Context.Location.Ip);
                    Assert.AreEqual("SessionId", telemetry.Context.Session.Id);
                    Assert.AreEqual("userId", telemetry.Context.User.Id);
                    Assert.AreEqual("OpId", telemetry.Context.Operation.Id);
                }
            };

            configuration.TelemetrySinks.Add(new TelemetrySink(configuration, secondTelemetryChannel));
            configuration.TelemetryProcessorChainBuilder.Build();

            TelemetryClient telemetryClient = new TelemetryClient(configuration);

            telemetryClient.Context.Operation.Id = "OpId";
            telemetryClient.Context.Cloud.RoleName = "UnitTest";
            telemetryClient.Context.Component.Version = "TestVersion";
            telemetryClient.Context.Device.Id = "TestDeviceId";
            telemetryClient.Context.Flags = 1234;
            telemetryClient.Context.InstrumentationKey = Guid.Empty.ToString();
            telemetryClient.Context.Location.Ip = "127.0.0.1";
            telemetryClient.Context.Session.Id = "SessionId";
            telemetryClient.Context.User.Id = "userId";

            telemetryClient.TrackRequest("Request", DateTimeOffset.Now, TimeSpan.FromMilliseconds(200), "200", true);
        }

        /// <summary>
        /// Ensures all telemetry sinks get the similar (objects with same values filled in them) telemetry items.
        /// </summary>
        [TestMethod]
        public void EnsureAllTelemetrySinkItemsAreSimilarAcrossSinks()
        {
            var configuration = new TelemetryConfiguration();
            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;

            string jsonFromFirstChannel = null;
            string jsonFromSecondChannel = null;

            ITelemetryChannel firstTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    jsonFromFirstChannel = JsonConvert.SerializeObject(telemetry);
                }
            };

            ITelemetryChannel secondTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    jsonFromSecondChannel = JsonConvert.SerializeObject(telemetry);
                }
            };

            configuration.DefaultTelemetrySink.TelemetryChannel = firstTelemetryChannel;
            configuration.TelemetrySinks.Add(new TelemetrySink(configuration, secondTelemetryChannel));

            configuration.TelemetryProcessorChainBuilder.Build();

            TelemetryClient telemetryClient = new TelemetryClient(configuration);

            // Setup TelemetryContext in a way that it is filledup.
            telemetryClient.Context.Operation.Id = "OpId";
            telemetryClient.Context.Cloud.RoleName = "UnitTest";
            telemetryClient.Context.Component.Version = "TestVersion";
            telemetryClient.Context.Device.Id = "TestDeviceId";
            telemetryClient.Context.Flags = 1234;
            telemetryClient.Context.InstrumentationKey = Guid.Empty.ToString();
            telemetryClient.Context.Location.Ip = "127.0.0.1";
            telemetryClient.Context.Session.Id = "SessionId";
            telemetryClient.Context.User.Id = "userId";

            telemetryClient.TrackAvailability(
                "Availability",
                DateTimeOffset.Now,
                TimeSpan.FromMilliseconds(200),
                "Local",
                true,
                "Message",
                new Dictionary<string, string>() { { "Key", "Value" } },
                new Dictionary<string, double>() { { "Dimension1", 0.9865 } });
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);

            telemetryClient.TrackDependency(
                "HTTP",
                "Target",
                "Test",
                "https://azure",
                DateTimeOffset.Now,
                TimeSpan.FromMilliseconds(100),
                "200",
                true);
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);

            telemetryClient.TrackEvent(
                "Event",
                new Dictionary<string, string>() { { "Key", "Value" } },
                new Dictionary<string, double>() { { "Dimension1", 0.9865 } });
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);

            telemetryClient.TrackException(
                new Exception("Test"),
                new Dictionary<string, string>() { { "Key", "Value" } },
                new Dictionary<string, double>() { { "Dimension1", 0.9865 } });
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);

            telemetryClient.TrackMetric("Metric", 0.1, new Dictionary<string, string>() { { "Key", "Value" } });
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);

            telemetryClient.TrackPageView("PageView");
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);

            telemetryClient.TrackRequest(
                new RequestTelemetry("GET https://azure.com", DateTimeOffset.Now, TimeSpan.FromMilliseconds(200), "200", true)
                {
#pragma warning disable CS0618 // Type or member is obsolete. Using for testing all cases.
                    HttpMethod = "GET"
#pragma warning restore CS0618 // Type or member is obsolete. Using for testing all cases.
                });
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);

            telemetryClient.TrackTrace(
                "Message",
                SeverityLevel.Critical,
                new Dictionary<string, string>() { { "Key", "Value" } });
            Assert.AreEqual(jsonFromFirstChannel, jsonFromSecondChannel);
        }

        /// <summary>
        /// Ensure broadcast processor does not drop telemetry items.
        /// </summary>
        [TestMethod]
        public void EnsureEventsAreNotDroppedByBroadcastProcessor()
        {
            var configuration = new TelemetryConfiguration();
            var commonChainBuilder = new TelemetryProcessorChainBuilder(configuration);
            configuration.TelemetryProcessorChainBuilder = commonChainBuilder;

            ConcurrentBag<ITelemetry> itemsReceivedBySink1 = new ConcurrentBag<ITelemetry>();
            ConcurrentBag<ITelemetry> itemsReceivedBySink2 = new ConcurrentBag<ITelemetry>();

            ITelemetryChannel firstTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    itemsReceivedBySink1.Add(telemetry);
                }
            };

            ITelemetryChannel secondTelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry =>
                {
                    itemsReceivedBySink2.Add(telemetry);
                }
            };

            configuration.DefaultTelemetrySink.TelemetryChannel = firstTelemetryChannel;
            configuration.TelemetrySinks.Add(new TelemetrySink(configuration, secondTelemetryChannel));

            configuration.TelemetryProcessorChainBuilder.Build();

            TelemetryClient telemetryClient = new TelemetryClient(configuration);

            // Setup TelemetryContext in a way that it is filledup.
            telemetryClient.Context.Operation.Id = "OpId";
            telemetryClient.Context.Cloud.RoleName = "UnitTest";
            telemetryClient.Context.Component.Version = "TestVersion";
            telemetryClient.Context.Device.Id = "TestDeviceId";
            telemetryClient.Context.Flags = 1234;
            telemetryClient.Context.InstrumentationKey = Guid.Empty.ToString();
            telemetryClient.Context.Location.Ip = "127.0.0.1";
            telemetryClient.Context.Session.Id = "SessionId";
            telemetryClient.Context.User.Id = "userId";

            Parallel.ForEach(
                new int[100],
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = 100
                },
                (value) =>
                {
                    telemetryClient.TrackAvailability(
                        "Availability",
                        DateTimeOffset.Now,
                        TimeSpan.FromMilliseconds(200),
                        "Local",
                        true,
                        "Message",
                        new Dictionary<string, string>() { { "Key", "Value" } },
                        new Dictionary<string, double>() { { "Dimension1", 0.9865 } });

                    telemetryClient.TrackDependency(
                        "HTTP",
                        "Target",
                        "Test",
                        "https://azure",
                        DateTimeOffset.Now,
                        TimeSpan.FromMilliseconds(100),
                        "200",
                        true);

                    telemetryClient.TrackEvent(
                        "Event",
                        new Dictionary<string, string>() { { "Key", "Value" } },
                        new Dictionary<string, double>() { { "Dimension1", 0.9865 } });

                    telemetryClient.TrackException(
                        new Exception("Test"),
                        new Dictionary<string, string>() { { "Key", "Value" } },
                        new Dictionary<string, double>() { { "Dimension1", 0.9865 } });

                    telemetryClient.TrackMetric("Metric", 0.1, new Dictionary<string, string>() { { "Key", "Value" } });

                    telemetryClient.TrackPageView("PageView");

                    telemetryClient.TrackRequest(
                        new RequestTelemetry("GET https://azure.com", DateTimeOffset.Now, TimeSpan.FromMilliseconds(200), "200", true)
                        {
#pragma warning disable CS0618 // Type or member is obsolete. Using for testing all cases.
                            HttpMethod = "GET"
#pragma warning restore CS0618 // Type or member is obsolete. Using for testing all cases.
                        });

                    telemetryClient.TrackTrace(
                        "Message",
                        SeverityLevel.Critical,
                        new Dictionary<string, string>() { { "Key", "Value" } });

                });

            Assert.AreEqual(itemsReceivedBySink1.Count, itemsReceivedBySink2.Count);
            Assert.AreEqual(8 * 100, itemsReceivedBySink1.Count);
            Assert.AreEqual(8 * 100, itemsReceivedBySink2.Count);
        }
    }
}

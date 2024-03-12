namespace Microsoft.ApplicationInsights.WindowsServer.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    [TestCategory("WindowsOnly")] // these tests are flaky on linux builds.
    public class AdaptiveSamplingTelemetryProcessorTest
    {
        [TestMethod]
        public void AllTelemetryCapturedWhenProductionRateIsLow()
        {
            var sentTelemetry = new List<ITelemetry>();
            int itemsProduced = 0;

            using (var tc = new TelemetryConfiguration() { TelemetryChannel = new StubTelemetryChannel() })
            {
                var chainBuilder = new TelemetryProcessorChainBuilder(tc);

                // set up adaptive sampling that evaluates and changes sampling % frequently
                chainBuilder
                    .UseAdaptiveSampling(
                        new Channel.Implementation.SamplingPercentageEstimatorSettings()
                        {
                            EvaluationInterval = TimeSpan.FromSeconds(1),
                            SamplingPercentageDecreaseTimeout = TimeSpan.FromSeconds(2),
                            SamplingPercentageIncreaseTimeout = TimeSpan.FromSeconds(2),
                        },
                        this.TraceSamplingPercentageEvaluation)
                    .Use((next) => new StubTelemetryProcessor(next) { OnProcess = (t) => sentTelemetry.Add(t) });

                chainBuilder.Build();

                const int productionFrequencyMs = 1000;

                var productionTimer = new Timer(
                    (state) =>
                    {
                        tc.TelemetryProcessorChain.Process(new RequestTelemetry());
                        itemsProduced++;
                    },
                    null,
                    productionFrequencyMs,
                    productionFrequencyMs);

                Thread.Sleep(25000);
                
                // dispose timer and wait for callbacks to complete
                DisposeTimer(productionTimer);
            }

            Assert.AreEqual(itemsProduced, sentTelemetry.Count);
        }

        [TestMethod]
        public void ProactivelySampledInTelemetryCapturedWhenProactiveSamplingRateIsLowerThanTarget()
        {
            var testDurationSec = 30;
            var proactivelySampledInRatePerSec = 25;
            var targetProactiveCount = proactivelySampledInRatePerSec * testDurationSec;
            var precision = 0.2;
            var (proactivelySampledInAndSentCount, sentCount) = ProactiveSamplingTest(
                proactivelySampledInRatePerSec: proactivelySampledInRatePerSec,
                beforeSamplingRatePerSec: proactivelySampledInRatePerSec * 3,
                targetAfterSamplingRatePerSec: proactivelySampledInRatePerSec * 2,
                precision: precision,
                testDurationInSec: testDurationSec); // plus warm up 

            Trace.WriteLine($"'Ideal' proactively sampled in telemetry item count: {targetProactiveCount}");
            Trace.WriteLine($"Expected range: from {targetProactiveCount - precision * targetProactiveCount} to {targetProactiveCount + precision * targetProactiveCount}");
            Trace.WriteLine(
                $"Actual proactively sampled in  telemetry item count: {proactivelySampledInAndSentCount} ({100.0 * proactivelySampledInAndSentCount / targetProactiveCount:##.##}% of ideal)");

            // all proactively sampled in should be sent assuming we have perfect algo
            // as they happen with rate 5 items per sec and we want 10 rate of sent telemetry
            Assert.IsTrue(proactivelySampledInAndSentCount / (double)targetProactiveCount > 1 - precision,
                $"Expected {proactivelySampledInAndSentCount} to be between {targetProactiveCount} +/- {targetProactiveCount * precision}");
            Assert.IsTrue(proactivelySampledInAndSentCount / (double)targetProactiveCount < 1 + precision,
                $"Expected {proactivelySampledInAndSentCount} to be between {targetProactiveCount} +/- {targetProactiveCount * precision}");
        }

        [TestMethod]
        public void ProactivelySampledInTelemetryCapturedWhenProactiveSamplingRateIsHigherThanTarget()
        {
            var testDuration = 30;
            var beforeSamplingRate = 42;
            var proactiveRate = beforeSamplingRate - 2;

            // for some reason, sampling on Linux test machines is not very stable
            // perhaps agents are not powerful or really virtual
            // there is nothing special in id generation or sampling on Linux
            // so we are blaming test infra
            double precision = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 0.4 : 0.3;

            var (proactivelySampledInAndSentCount, sentCount) = ProactiveSamplingTest(
                proactivelySampledInRatePerSec: proactiveRate,
                beforeSamplingRatePerSec: beforeSamplingRate,
                targetAfterSamplingRatePerSec: proactiveRate / 2,
                precision: precision,
                testDurationInSec: testDuration); //plus warm up

            // most of of sent should be proactively sampled in 
            // as proactive happen with rate >> than target
            Trace.WriteLine($"'Ideal' proactively sampled in telemetry item count: {sentCount}");
            Trace.WriteLine(
                $"Expected range: from {sentCount - sentCount * precision} to {sentCount + sentCount * precision}");
            Trace.WriteLine(
                $"Actual proactively sampled in  telemetry item count: {proactivelySampledInAndSentCount} ({100.0 * proactivelySampledInAndSentCount / sentCount:##.##}% of ideal)");
            Assert.AreEqual(proactivelySampledInAndSentCount, sentCount, sentCount * precision);
        }

        public (int proactivelySampledInAndSentCount, double sentCount) ProactiveSamplingTest(
            int proactivelySampledInRatePerSec, 
            int beforeSamplingRatePerSec, 
            double targetAfterSamplingRatePerSec,
            double precision,
            int testDurationInSec)
        {
            // we'll ignore telemetry reported during first few percentage evaluations
            int warmUpInSec = 12;

            // we'll produce proactively  sampled in items and also 'normal' items with the same rate
            // but allow only proactively sampled in + a bit more

            // number of items produced should be close to target
            int targetItemCount = (int)(testDurationInSec * targetAfterSamplingRatePerSec);

            var sentTelemetry = new List<ITelemetry>();

            using (var tc = new TelemetryConfiguration() { TelemetryChannel = new StubTelemetryChannel() })
            {
                var chainBuilder = new TelemetryProcessorChainBuilder(tc);

                // set up adaptive sampling that evaluates and changes sampling % frequently
                chainBuilder
                    .UseAdaptiveSampling(
                        new Channel.Implementation.SamplingPercentageEstimatorSettings()
                        {
                            // help algo get to stabilize earlier
                            InitialSamplingPercentage = targetAfterSamplingRatePerSec / (double)beforeSamplingRatePerSec * 100,
                            MaxTelemetryItemsPerSecond = targetAfterSamplingRatePerSec,
                            EvaluationInterval = TimeSpan.FromSeconds(2),
                            SamplingPercentageDecreaseTimeout = TimeSpan.FromSeconds(4),
                            SamplingPercentageIncreaseTimeout = TimeSpan.FromSeconds(4),
                        },
                        this.TraceSamplingPercentageEvaluation)
                    .Use((next) => new StubTelemetryProcessor(next) { OnProcess = (t) => sentTelemetry.Add(t) });

                chainBuilder.Build();

                var sw = Stopwatch.StartNew();
                var productionTimer = new Timer(
                    (state) =>
                    {
                        var requests = new RequestTelemetry[beforeSamplingRatePerSec];

                        for (int i = 0; i < beforeSamplingRatePerSec; i++)
                        {
                            requests[i] = new RequestTelemetry()
                            {
                                ProactiveSamplingDecision = i < proactivelySampledInRatePerSec ? SamplingDecision.SampledIn : SamplingDecision.None
                            };

                            requests[i].Context.Operation.Id = ActivityTraceId.CreateRandom().ToHexString();
                        }

                        foreach (var request in requests)
                        {
                            if (((Stopwatch) state).Elapsed.TotalSeconds < warmUpInSec)
                            {
                                // let's ignore telemetry from first few rate evaluations - it does not make sense
                                request.Properties["ignore"] = "true";
                            }

                            tc.TelemetryProcessorChain.Process(request);
                        }
                    },
                    sw,
                    0,
                    1000);

                Thread.Sleep(TimeSpan.FromSeconds(testDurationInSec + warmUpInSec));

                // dispose timer and wait for callbacks to complete
                DisposeTimer(productionTimer);
            }

            var notIgnoredSent = sentTelemetry.Where(i => i is ISupportProperties propItem && !propItem.Properties.ContainsKey("ignore")).ToArray();

            var proactivelySampledInAndSentCount = notIgnoredSent.Count(i =>
                i is ISupportAdvancedSampling advSamplingItem &&
                advSamplingItem.ProactiveSamplingDecision == SamplingDecision.SampledIn);

            // check that normal sampling requirements still apply (we generated as much items as expected)
            Trace.WriteLine($"'Ideal' telemetry item count: {targetItemCount}");
            Trace.WriteLine($"Expected range: from {targetItemCount - precision * targetItemCount} to {targetItemCount + precision * targetItemCount}");
            Trace.WriteLine(
                $"Actual telemetry item count: {notIgnoredSent.Length} ({100.0 * notIgnoredSent.Length / targetItemCount:##.##}% of ideal)");
            Trace.WriteLine(
                $"Actual proactive sampled in and sent: {proactivelySampledInAndSentCount}");

            Assert.IsTrue(notIgnoredSent.Length / (double)targetItemCount > 1 - precision);
            Assert.IsTrue(notIgnoredSent.Length / (double)targetItemCount < 1 + precision);

            return (proactivelySampledInAndSentCount, notIgnoredSent.Length);
        }

        [TestMethod]
        public void SamplingPercentageAdjustsAccordingToConstantHighProductionRate()
        {
            var sentTelemetry = new List<ITelemetry>();
            int itemsProduced = 0;

            using (var tc = new TelemetryConfiguration() { TelemetryChannel = new StubTelemetryChannel() })
            {
                var chainBuilder = new TelemetryProcessorChainBuilder(tc);

                // set up adaptive sampling that evaluates and changes sampling % frequently
                chainBuilder
                    .UseAdaptiveSampling(
                        new Channel.Implementation.SamplingPercentageEstimatorSettings()
                        {
                            EvaluationInterval = TimeSpan.FromSeconds(1),
                            SamplingPercentageDecreaseTimeout = TimeSpan.FromSeconds(2),
                            SamplingPercentageIncreaseTimeout = TimeSpan.FromSeconds(2),
                        },
                        this.TraceSamplingPercentageEvaluation)
                    .Use((next) => new StubTelemetryProcessor(next) { OnProcess = (t) => sentTelemetry.Add(t) });

                chainBuilder.Build();

                const int productionFrequencyMs = 100;

                var productionTimer = new Timer(
                    (state) =>
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            tc.TelemetryProcessorChain.Process(new RequestTelemetry());
                            itemsProduced++;
                        }
                    },
                    null,
                    0,
                    productionFrequencyMs);

                Thread.Sleep(25000);

                // dispose timer and wait for callbacks to complete
                DisposeTimer(productionTimer);
            }

            // number of items produced should be close to target of 5/second
            int targetItemCount = 25 * 5;

            // tolrance +-
            int tolerance = targetItemCount / 2;

            Trace.WriteLine(string.Format("'Ideal' telemetry item count: {0}", targetItemCount));
            Trace.WriteLine(string.Format(
                "Expected range: from {0} to {1}",
                targetItemCount - tolerance,
                targetItemCount + tolerance));
            Trace.WriteLine(string.Format(
                "Actual telemetry item count: {0} ({1:##.##}% of ideal)",
                sentTelemetry.Count,
                100.0 * sentTelemetry.Count / targetItemCount));

            Assert.IsTrue(sentTelemetry.Count > targetItemCount - tolerance);
            Assert.IsTrue(sentTelemetry.Count < targetItemCount + tolerance);
        }

        [TestMethod]
        public void SamplingPercentageAdjustsForSpikyProductionRate()
        {
            var sentTelemetry = new List<ITelemetry>();
            int itemsProduced = 0;

            using (var tc = new TelemetryConfiguration() { TelemetryChannel = new StubTelemetryChannel() })
            {
                var chainBuilder = new TelemetryProcessorChainBuilder(tc);

                // set up adaptive sampling that evaluates and changes sampling % frequently
                chainBuilder
                    .UseAdaptiveSampling(
                        new Channel.Implementation.SamplingPercentageEstimatorSettings()
                        {
                            InitialSamplingPercentage = 5.0,
                            EvaluationInterval = TimeSpan.FromSeconds(1),
                            SamplingPercentageDecreaseTimeout = TimeSpan.FromSeconds(2),
                            SamplingPercentageIncreaseTimeout = TimeSpan.FromSeconds(10),
                        },
                        this.TraceSamplingPercentageEvaluation)
                    .Use((next) => new StubTelemetryProcessor(next) { OnProcess = (t) => sentTelemetry.Add(t) });

                chainBuilder.Build();

                const int regularProductionFrequencyMs = 100;
                const int spikeProductionFrequencyMs = 3000;

                var regularProductionTimer = new Timer(
                    (state) =>
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            tc.TelemetryProcessorChain.Process(new RequestTelemetry());
                            Interlocked.Increment(ref itemsProduced);
                        }
                    },
                    null,
                    0,
                    regularProductionFrequencyMs);

                var spikeProductionTimer = new Timer(
                    (state) =>
                    {
                        for (int i = 0; i < 200; i++)
                        {
                            tc.TelemetryProcessorChain.Process(new RequestTelemetry());
                            Interlocked.Increment(ref itemsProduced);
                        }
                    },
                    null,
                    0,
                    spikeProductionFrequencyMs);

                Thread.Sleep(30000);

                // dispose timers and wait for callbacks to complete
                DisposeTimer(regularProductionTimer);
                DisposeTimer(spikeProductionTimer);
            }

            // number of items produced should be close to target of 5/second
            int targetItemCount = 30 * 5;
            int tolerance = targetItemCount / 2;

            Trace.WriteLine(string.Format("'Ideal' telemetry item count: {0}", targetItemCount));
            Trace.WriteLine(string.Format(
                "Expected range: from {0} to {1}",
                targetItemCount - tolerance,
                targetItemCount + tolerance));
            Trace.WriteLine(string.Format(
                "Actual telemetry item count: {0} ({1:##.##}% of ideal)",
                sentTelemetry.Count,
                100.0 * sentTelemetry.Count / targetItemCount));

            Assert.IsTrue(sentTelemetry.Count > targetItemCount - tolerance);
            Assert.IsTrue(sentTelemetry.Count < targetItemCount + tolerance);
        }

        private class AdaptiveTesterMessageSink : ITelemetryProcessor
        {
            public Queue<RequestTelemetry> requests = new Queue<RequestTelemetry>();
            public Queue<EventTelemetry> events = new Queue<EventTelemetry>();

            public void Process(ITelemetry item)
            {
                if (item is RequestTelemetry req)
                {
                    requests.Enqueue(req);
                }
                else if (item is EventTelemetry evt)
                {
                    events.Enqueue(evt);
                }
            }
        }

        [TestMethod]
        public void SamplingRoutesExcludedTypes()
        {
            var unsampled = new AdaptiveTesterMessageSink();
            var sampled = new AdaptiveTesterMessageSink();
            SamplingTelemetryProcessor sampler = new SamplingTelemetryProcessor(unsampled,sampled);

            sampler.ExcludedTypes = "Request";
            sampler.SamplingPercentage = 100.0;

            sampler.Process(new RequestTelemetry());
            sampler.Process(new EventTelemetry());

            Assert.IsNotNull(sampled.events.Dequeue());
            Assert.IsNotNull(unsampled.requests.Dequeue());
        }

        [TestMethod]
        public void SamplingWontEarlyExitWhenUnsampledNextPresent()
        {
            var unsampled = new AdaptiveTesterMessageSink();
            var sampled = new AdaptiveTesterMessageSink();
            SamplingTelemetryProcessor sampler = new SamplingTelemetryProcessor(unsampled, sampled)
            {
                SamplingPercentage = 100.0
            };

            sampler.Process(new RequestTelemetry());
            Assert.IsTrue(sampled.requests.Count == 1);
            var sent = sampled.requests.Dequeue();
            Assert.IsNotNull(sent);
            var sentSample = sent as ISupportSampling;
            Assert.IsNotNull(sentSample);
            Assert.IsTrue(sentSample.SamplingPercentage.HasValue);
        }

        [TestMethod]
        public void SamplingSkipsSampledTelemetryItemProperty()
        {
            var unsampled = new AdaptiveTesterMessageSink();
            var sampled = new AdaptiveTesterMessageSink();
            SamplingTelemetryProcessor sampler = new SamplingTelemetryProcessor(unsampled, sampled)
            {
                SamplingPercentage = 100.0
            };

            var send = new RequestTelemetry();
            var sendSampled = (send as ISupportSampling);
            Assert.IsNotNull(sendSampled);
            sendSampled.SamplingPercentage = 25.0;

            sampler.Process(send);

            Assert.IsTrue(unsampled.requests.Count == 1);
            Assert.IsTrue(sampled.requests.Count == 0);
        }

        [TestMethod]
        public void AdaptiveSamplingSetsExcludedTypesOnInternalSamplingProcessor()
        {
            var tc = new TelemetryConfiguration { TelemetryChannel = new StubTelemetryChannel() };
            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.UseAdaptiveSampling(5, "request;");
            channelBuilder.Build();

            var fieldInfo = typeof(AdaptiveSamplingTelemetryProcessor).GetField("samplingProcessor", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
            SamplingTelemetryProcessor internalProcessor = (SamplingTelemetryProcessor) fieldInfo.GetValue(tc.TelemetryProcessorChain.FirstTelemetryProcessor);

            Assert.AreEqual("request;", internalProcessor.ExcludedTypes);
        }

        [TestMethod]
        public void CurrentSamplingRateResetsOnInitialSamplingRateChange()
        {
            var nextMock = new Mock<ITelemetryProcessor>();
            var next = nextMock.Object;
            var adaptiveSamplingProcessor = new AdaptiveSamplingTelemetryProcessor(
                new Channel.Implementation.SamplingPercentageEstimatorSettings
                {
                    InitialSamplingPercentage = 20,
                },
                null,
                next);

            Assert.AreEqual(20, adaptiveSamplingProcessor.InitialSamplingPercentage);
            Assert.AreEqual(100 / 20, adaptiveSamplingProcessor.SamplingPercentageEstimatorTelemetryProcessor.CurrentSamplingRate);

            // change in InitialSamplingPercentage should change the CurrentSamplingPercentage:
            adaptiveSamplingProcessor.InitialSamplingPercentage = 50;
            Assert.AreEqual(50, adaptiveSamplingProcessor.InitialSamplingPercentage);
            Assert.AreEqual(100 / 50, adaptiveSamplingProcessor.SamplingPercentageEstimatorTelemetryProcessor.CurrentSamplingRate);
        }

        [TestMethod]
        public void SettingsFromPassedInTelemetryProcessorsAreAppliedToSamplingTelemetryProcessor()
        {
            var nextMock = new Mock<ITelemetryProcessor>();
            var next = nextMock.Object;
            var adaptiveSamplingProcessor = new AdaptiveSamplingTelemetryProcessor(
                new Channel.Implementation.SamplingPercentageEstimatorSettings
                {
                    InitialSamplingPercentage = 25,
                },
                null,
                next);
            var percentageEstimatorProcessor = adaptiveSamplingProcessor.SamplingTelemetryProcessor;
            Assert.AreEqual(25, percentageEstimatorProcessor.SamplingPercentage);
        }

        private void TraceSamplingPercentageEvaluation(
            double afterSamplingTelemetryItemRatePerSecond,
            double currentSamplingPercentage,
            double newSamplingPercentage,
            bool isSamplingPercentageChanged,
            Channel.Implementation.SamplingPercentageEstimatorSettings settings)
        {
            Trace.WriteLine(string.Format(
                "[Sampling% evaluation] {0}, Eps: {1}, Current %: {2}, New %: {3}, Changed: {4}",
                DateTimeOffset.UtcNow.ToString("o"), 
                afterSamplingTelemetryItemRatePerSecond,
                currentSamplingPercentage,
                newSamplingPercentage,
                isSamplingPercentageChanged));
        }


        private void DisposeTimer(Timer timer)
        {
            // Regular Dispose() does not wait for all callbacks to complete
            // so TelemetryConfiguration could be disposed while callback still runs
            AutoResetEvent allDone = new AutoResetEvent(false);
            timer.Dispose(allDone);
            // this will wait for all callbacks to complete
            allDone.WaitOne();
        }
    }
}

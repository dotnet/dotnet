namespace Microsoft.ApplicationInsights.WindowsServer.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using System.Linq;

    [TestClass]
    public class SamplingTelemetryProcessorTest
    {
        readonly Random random = new Random();

        [TestMethod]
        public void ThrowsArgumentNullExceptionWithoutNextProcessor()
        {
            AssertEx.Throws<ArgumentNullException>(() => new SamplingTelemetryProcessor(null));
        }

        [TestMethod]
        public void DefaultSamplingRateIs100Percent()
        {
            var processor = new SamplingTelemetryProcessor(new StubTelemetryProcessor(null));

            Assert.AreEqual(processor.SamplingPercentage, 100.0);
            Assert.IsNull(processor.ProactiveSamplingPercentage);
        }

        [TestMethod]
        public void AllTelemetryIsSentWithDefaultSamplingRate()
        {
            var sentTelemetry = new List<ITelemetry>();
            var processor = new SamplingTelemetryProcessor(new StubTelemetryProcessor(null) { OnProcess = t => sentTelemetry.Add(t) });

            const int ItemsToGenerate = 100;

            for (int i = 0; i < ItemsToGenerate; i++)
            {
                processor.Process(new RequestTelemetry());
            }

            Assert.AreEqual(ItemsToGenerate, sentTelemetry.Count);
        }

        [TestMethod]
        public void TelemetryItemHasSamplingPercentageSet()
        {
            var sentTelemetry = new List<ITelemetry>();
            var processor = new SamplingTelemetryProcessor(new StubTelemetryProcessor(null) { OnProcess = t => sentTelemetry.Add(t) })
            {
                SamplingPercentage = 20
            };

            do
            {
                processor.Process(new RequestTelemetry());
            }
            while (sentTelemetry.Count == 0);

            Assert.AreEqual(20, ((ISupportSampling)sentTelemetry[0]).SamplingPercentage);
        }

        [TestMethod]
        public void EarlyExitWhenProcessingAt100Percent()
        {
            var sentTelemetry = new List<ITelemetry>();
            var processor = new SamplingTelemetryProcessor(new StubTelemetryProcessor(null) { OnProcess = t => sentTelemetry.Add(t) })
            {
                SamplingPercentage = 100.0
            };

            processor.Process(new RequestTelemetry());

            Assert.IsFalse(((ISupportSampling)sentTelemetry[0]).SamplingPercentage.HasValue);
        }

        [TestMethod]
        public void TelemetryItemSamplingIsSkippedWhenSetByUser()
        {
            var sentTelemetry = new List<ITelemetry>();
            var processor = new SamplingTelemetryProcessor(new StubTelemetryProcessor(null) { OnProcess = t => sentTelemetry.Add(t) })
            {
                SamplingPercentage = 0
            };

            var requestTelemetry = new RequestTelemetry();
            ((ISupportSampling)requestTelemetry).SamplingPercentage = 100;
            processor.Process(requestTelemetry);

            Assert.AreEqual(1, sentTelemetry.Count);
        }

        [TestMethod]
        public void DependencyTelemetryIsSubjectToSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors => telemetryProcessors.Process(new DependencyTelemetry()));
        }
        
        [TestMethod]
        public void EventTelemetryIsSubjectToSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors => telemetryProcessors.Process(new EventTelemetry("event")));
        }
        
        [TestMethod]
        public void ExceptionTelemetryIsSubjectToSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors => telemetryProcessors.Process(new ExceptionTelemetry(new Exception("exception"))));
        }

        [TestMethod]
        public void MetricTelemetryIsNotSubjectToSampling()
        {
            TelemetryTypeDoesNotSupportSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new MetricTelemetry() { Count = 1, Sum = 1.0 } );
                return 1;
            });
        }

        [TestMethod]
        public void PageViewTelemetryIsSubjectToSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors => telemetryProcessors.Process(new PageViewTelemetry("page")));
        }

#pragma warning disable 618
        [TestMethod]
        public void PerformanceCounterTelemetryIsNotSubjectToSampling()
        {
            TelemetryTypeDoesNotSupportSampling(
                telemetryProcessors =>
                {
                    telemetryProcessors.Process(new PerformanceCounterTelemetry("category", "counter", "instance", 1.0));
                    return 1;
                });
        }
#pragma warning restore 618
                
        [TestMethod]
        public void RequestTelemetryIsSubjectToSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors => telemetryProcessors.Process(new RequestTelemetry()));
        }
        
        [TestMethod]
        public void SessionStateTelemetryIsNotSubjectToSampling()
        {
            TelemetryTypeDoesNotSupportSampling(telemetryProcessors =>
            {
#pragma warning disable 618
                telemetryProcessors.Process(new SessionStateTelemetry());
#pragma warning restore 618
                return 1;
            });
        }
        
        [TestMethod]
        public void TraceTelemetryIsSubjectToSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors => telemetryProcessors.Process(new TraceTelemetry("my trace")));
        }

        [TestMethod]
        public void RequestCanBeExcludedFromSampling()
        {
            TelemetryTypeDoesNotSupportSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new RequestTelemetry());
                return 1;
            }, "request");
        }

        [TestMethod]
        public void RequestCanBeIncludedInSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new RequestTelemetry());
            },
            null,
            "request");
        }

        [TestMethod]
        public void DependencyCanBeExcludedFromSampling()
        {
            TelemetryTypeDoesNotSupportSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new DependencyTelemetry());
                return 1;
            }, 
            "dependency");
        }

        [TestMethod]
        public void DependencyCanBeIncludedInSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new DependencyTelemetry());
            },
            null,
            "dependency");
        }

        [TestMethod]
        public void EventCanBeExcludedFromSampling()
        {
            TelemetryTypeDoesNotSupportSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new EventTelemetry());
                return 1;
            }, 
            "event");
        }

        [TestMethod]
        public void EventCanBeIncludedInSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new EventTelemetry());
            },
            null,
            "event");
        }

        [TestMethod]
        public void ExceptionCanBeExcludedFromSampling()
        {
            TelemetryTypeDoesNotSupportSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new ExceptionTelemetry());
                return 1;
            }, 
            "exception");
        }

        [TestMethod]
        public void ExceptionCanBeIncludedInSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new ExceptionTelemetry());
            },
            null,
            "exception");
        }

        [TestMethod]
        public void TraceCanBeExcludedFromSampling()
        {
            TelemetryTypeDoesNotSupportSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new TraceTelemetry());
                return 1;
            }, 
            "trace");
        }

        [TestMethod]
        public void TraceCanBeIncludedInSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new TraceTelemetry());
            },
            null,
            "trace");
        }

        [TestMethod]
        public void PageViewCanBeExcludedFromSampling()
        {
            TelemetryTypeDoesNotSupportSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new PageViewTelemetry());
                return 1;
            }, 
            "pageview");
        }

        [TestMethod]
        public void PageViewCanBeIncludedInSampling()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors =>
            {
                telemetryProcessors.Process(new PageViewTelemetry());
            },
            null,
            "pageview");
        }

        [TestMethod]
        public void MultipleItemsCanBeExcludedFromSampling()
        {
            TelemetryTypeDoesNotSupportSampling(
                telemetryProcessors =>
                {
                    telemetryProcessors.Process(new PageViewTelemetry());
                    telemetryProcessors.Process(new RequestTelemetry());
                    return 2;
                }, 
                "pageview;request");
        }

        [TestMethod]
        public void MultipleItemsCanBeIncludedInSampling()
        {
            TelemetryTypeSupportsSampling(
                telemetryProcessors =>
                {
                    telemetryProcessors.Process(new PageViewTelemetry());
                    telemetryProcessors.Process(new RequestTelemetry());
                    return 2;
                },
                null,
                "pageview;request");
        }

        [TestMethod]
        public void IncludedDoNotOverrideExcludedFromSampling()
        {
            TelemetryTypeDoesNotSupportSampling(
                telemetryProcessors =>
                {
                    telemetryProcessors.Process(new PageViewTelemetry());
                    telemetryProcessors.Process(new RequestTelemetry());
                    return 2;
                },
                "pageview;request",
                "exception;request");
        }

        [TestMethod]
        public void UnknownExcludedTypesAreIgnored()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors => telemetryProcessors.Process(new TraceTelemetry("my trace")), "lala1;lala2,lala3");
        }

        [TestMethod]
        public void UnknownIncludedTypesAreIgnored()
        {
            TelemetryTypeSupportsSampling(telemetryProcessors => telemetryProcessors.Process(new TraceTelemetry("my trace")), null, "lala1;lala2,lala3");
        }

        [TestMethod]
        public void IncorrectFormatDoesNotAffectCorrectExcludedTypes()
        {
            TelemetryTypeDoesNotSupportSampling(
                telemetryProcessors =>
                {
                    telemetryProcessors.Process(new PageViewTelemetry());
                    telemetryProcessors.Process(new RequestTelemetry());
                    return 2;
                },
                ";;;;;lala1;;;;;pageview;lala2;request;;;;;");
        }

        [TestMethod]
        public void IncorrectFormatDoesNotAffectCorrectIncludedTypes()
        {
            TelemetryTypeDoesNotSupportSampling(
                telemetryProcessors =>
                {
                    telemetryProcessors.Process(new PageViewTelemetry());
                    telemetryProcessors.Process(new RequestTelemetry());
                    return 2;
                },
                null,
                ";;;;;lala1;;;;;trace;lala2;exception;;;;;");
        }

        [TestMethod]
        public void NoSamplingTracksSamplingRate()
        {
            TelemetryTypeSupportsSampling((telemetryProcessors) => { telemetryProcessors.Process(new RequestTelemetry()); return 1; },
                                          excludedTypes:      null,
                                          includedTypes:      "request",
                                          samplingPercentage: 100);
        }

        [TestMethod]
        public void ProactivelySampledOutItemIsNotSent()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryProcessorChain telemetryProcessorChainWithSampling = CreateTelemetryProcessorChainWithSampling(
                                                                                    sentTelemetry,
                                                                                    100);
            var sampledOutTelemetry = new RequestTelemetry();
            sampledOutTelemetry.ProactiveSamplingDecision = SamplingDecision.SampledOut;

            telemetryProcessorChainWithSampling.Process(sampledOutTelemetry);
            telemetryProcessorChainWithSampling.Dispose();

            Assert.AreEqual(0, sentTelemetry.Count);            
        }

        [TestMethod]
        public void ProactivelySampledOutItemIsNotSentEvenIfItIsSampledIn()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryProcessorChain telemetryProcessorChainWithSampling = CreateTelemetryProcessorChainWithSampling(
                                                                                    sentTelemetry,
                                                                                    50);
            for (int i = 0; i < 100; i++)
            {
                var sampledOutTelemetry = new RequestTelemetry();
                sampledOutTelemetry.ProactiveSamplingDecision = SamplingDecision.SampledOut;

                telemetryProcessorChainWithSampling.Process(sampledOutTelemetry);
            }

            telemetryProcessorChainWithSampling.Dispose();

            Assert.AreEqual(0, sentTelemetry.Count);
        }

        [TestMethod]
        public void ProactivelySampledOutItemThatIsLaterSampledInIsAddedToTheNextSampledInItem()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryProcessorChain telemetryProcessorChainWithSampling = CreateTelemetryProcessorChainWithSampling(
                                                                                    sentTelemetry,
                                                                                    25);

            // Get the random number of sampled out items
            var sampledOutItemsCount = this.random.Next(100) + 100;

            for (int i = 0; i < sampledOutItemsCount; i++)
            {
                var sampledOutTelemetry = new RequestTelemetry();

                // This makes those items proactively sampled out
                sampledOutTelemetry.ProactiveSamplingDecision = SamplingDecision.SampledOut;

                // This operation ID hash is lower than 25, so every item in this batch is sampled in
                sampledOutTelemetry.Context.Operation.Id = "abcdfeghijk";
                telemetryProcessorChainWithSampling.Process(sampledOutTelemetry);
            }

            // No telemetry is recorded
            Assert.AreEqual(0, sentTelemetry.Count);
            
            // Sampling Rate has changed up
            ((SamplingTelemetryProcessor)telemetryProcessorChainWithSampling.FirstTelemetryProcessor).SamplingPercentage = 50;            

            for (int i = 0; i < 100; i++)
            {
                var sampledInTelemetry = new RequestTelemetry();                
                telemetryProcessorChainWithSampling.Process(sampledInTelemetry);
            }

            telemetryProcessorChainWithSampling.Dispose();

            // The item that is sampled in will need to represent all sampled out items and itself:
            // 4 * sampledOutItemsCount + 2, where 4 is (100 / 25) and 2 is (100 / 50) as per chosen sample rates
            double expectedSamplingRate = (double)100 / (100 / 25 * sampledOutItemsCount + (100 / 50));
            Assert.AreEqual(expectedSamplingRate, ((ISupportSampling)sentTelemetry.First()).SamplingPercentage.Value, delta: 0.01);

            // Sampled out items are supposed to be cleared, no more gain up:
            sentTelemetry.RemoveAt(0);
            sentTelemetry.ForEach((item) => Assert.AreEqual(50, ((ISupportSampling)item).SamplingPercentage));
        }

        [TestMethod]
        public void ProactivelySampledInItemsAreNotGivenPriorityIfRatesAreNotSet()
        {
            var sentTelemetry = new List<ITelemetry>();
            TelemetryProcessorChain telemetryProcessorChainWithSampling = CreateTelemetryProcessorChainWithSampling(
                sentTelemetry,
                50);

            for (int i = 0; i < 1000; i++)
            {
                var item = new RequestTelemetry();
                item.Context.Operation.Id = ActivityTraceId.CreateRandom().ToHexString();

                // proactively sample in items with big score, so they should not be sampled in
                if (SamplingScoreGenerator.GetSamplingScore(item.Context.Operation.Id) > 50)
                {
                    item.ProactiveSamplingDecision = SamplingDecision.SampledIn;
                }

                telemetryProcessorChainWithSampling.Process(item);
            }

            Assert.AreEqual(0, sentTelemetry.Count(i => ((ISupportAdvancedSampling)i).ProactiveSamplingDecision == SamplingDecision.SampledIn));
        }

        [TestMethod]
        public void ProactivelySampledInItemsPassIfCurrentRateIsLowerThanExpected()
        {
            var sentTelemetry = new List<ITelemetry>();

            var tc = new TelemetryConfiguration
            {
                TelemetryChannel = new StubTelemetryChannel(),
                InstrumentationKey = Guid.NewGuid().ToString("D")
            };

            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.Use(next => new SamplingTelemetryProcessor(next)
            {
                SamplingPercentage = 50,
                ProactiveSamplingPercentage = 100
            });
            channelBuilder.Use(next => new StubTelemetryProcessor(next) { OnProcess = t => sentTelemetry.Add(t) });
            channelBuilder.Build();

            int sampledInCount = 0;
            for (int i = 0; i < 1000; i++)
            {
                var item = new RequestTelemetry();
                item.Context.Operation.Id = ActivityTraceId.CreateRandom().ToHexString();

                // sample in random items - they all should  pass through regardless of the score
                if (i % 2 == 0)
                {
                    item.ProactiveSamplingDecision = SamplingDecision.SampledIn;
                    sampledInCount++;
                }

                tc.TelemetryProcessorChain.Process(item);
            }

            // all proactively sampled in items passed through regardless of their score.
            Assert.AreEqual(sampledInCount, sentTelemetry.Count(i => ((ISupportAdvancedSampling)i).ProactiveSamplingDecision == SamplingDecision.SampledIn));
        }

        [TestMethod]
        public void ProactivelySampledInItemsPassAccordingToScoreIfCurrentRateIsHigherThanExpected()
        {
            var sentTelemetry = new List<ITelemetry>();

            var tc = new TelemetryConfiguration
            {
                TelemetryChannel = new StubTelemetryChannel(),
                InstrumentationKey = Guid.NewGuid().ToString("D")
            };

            var channelBuilder = new TelemetryProcessorChainBuilder(tc);
            channelBuilder.Use(next => new SamplingTelemetryProcessor(next)
            {
                SamplingPercentage = 50,
                ProactiveSamplingPercentage = 50
            });
            channelBuilder.Use(next => new StubTelemetryProcessor(next) { OnProcess = t => sentTelemetry.Add(t) });
            channelBuilder.Build();

            int count = 5000;
            for (int i = 0; i < count; i++)
            {
                var item = new RequestTelemetry();
                item.Context.Operation.Id = ActivityTraceId.CreateRandom().ToHexString();

                // generate a lot sampled-in items, only 1/CurrentProactiveSampledInRatioToTarget of them should pass through 
                // and SamplingPercentage of sampled-out items
                if (SamplingScoreGenerator.GetSamplingScore(item.Context.Operation.Id) < 80)
                {
                    item.ProactiveSamplingDecision = SamplingDecision.SampledIn;
                }

                tc.TelemetryProcessorChain.Process(item);
            }

            Assert.AreEqual(0, sentTelemetry.Count(i => ((ISupportAdvancedSampling)i).ProactiveSamplingDecision == SamplingDecision.None));
            Assert.AreEqual(count / 2, sentTelemetry.Count(i => ((ISupportAdvancedSampling)i).ProactiveSamplingDecision == SamplingDecision.SampledIn), count / 2 / 10);
        }

        private static void TelemetryTypeDoesNotSupportSampling(Func<TelemetryProcessorChain, int> sendAction, string excludedTypes = null, string includedTypes = null)
        {
            const int SamplingPercentage = 10;
            var sentTelemetry = new List<ITelemetry>();
            var telemetryProcessorChainWithSampling = CreateTelemetryProcessorChainWithSampling(sentTelemetry, SamplingPercentage, excludedTypes, includedTypes);

            int generatedCount = 0;
            for (int i = 0; i < 100; i++)
            {
                generatedCount += sendAction.Invoke(telemetryProcessorChainWithSampling);
            }

            Assert.AreEqual(generatedCount, sentTelemetry.Count);
        }

        private static void TelemetryTypeSupportsSampling(Action<TelemetryProcessorChain> sendAction,
                                                          string excludedTypes = null,
                                                          string includedTypes = null)
        {
            TelemetryTypeSupportsSampling((chain) => { sendAction(chain); return 1; },
                                          excludedTypes,
                                          includedTypes);
        }

        private static void TelemetryTypeSupportsSampling(Func<TelemetryProcessorChain, int> sendAction,
                                                          string excludedTypes = null,
                                                          string includedTypes = null,
                                                          int samplingPercentage = 10)
        {
            const int ItemsToGenerate = 100;
            var sentTelemetry = new List<ITelemetry>();
            TelemetryProcessorChain telemetryProcessorChainWithSampling = CreateTelemetryProcessorChainWithSampling(
                                                                                    sentTelemetry,
                                                                                    samplingPercentage,
                                                                                    excludedTypes,
                                                                                    includedTypes);

            int generatedCount = 0;
            for (int i = 0; i < ItemsToGenerate; i++)
            {
                generatedCount += sendAction.Invoke(telemetryProcessorChainWithSampling);
            }

            Assert.IsNotNull(sentTelemetry[0] as ISupportSampling);
            Assert.IsTrue(sentTelemetry.Count > 0);
            
            if (samplingPercentage == 100)
            {
                Assert.IsTrue(sentTelemetry.Count == generatedCount);
                Assert.AreEqual(null, ((ISupportSampling) sentTelemetry[0]).SamplingPercentage);
            }
            else
            {
                Assert.IsTrue(sentTelemetry.Count < generatedCount);
                Assert.AreEqual(samplingPercentage, ((ISupportSampling) sentTelemetry[0]).SamplingPercentage);
            }
            
            telemetryProcessorChainWithSampling.Dispose();
        }

        private static TelemetryProcessorChain CreateTelemetryProcessorChainWithSampling(IList<ITelemetry> sentTelemetry, double samplingPercentage, string excludedTypes = null, string includedTypes = null)
        {
            var tc = new TelemetryConfiguration
            {
                TelemetryChannel = new StubTelemetryChannel(), InstrumentationKey = Guid.NewGuid().ToString("D")
            };

            var channelBuilder = new TelemetryProcessorChainBuilder(tc);            
            channelBuilder.UseSampling(samplingPercentage, excludedTypes, includedTypes);
            channelBuilder.Use(next => new StubTelemetryProcessor(next) { OnProcess = t => sentTelemetry.Add(t) });
            
            channelBuilder.Build();

            TelemetryProcessorChain processors = tc.TelemetryProcessorChain;

            foreach (ITelemetryProcessor processor in processors.TelemetryProcessors)
            {
                if (processor is ITelemetryModule m)
                {
                    m.Initialize(tc);
                }
            }

            return processors;
        }
    }
}

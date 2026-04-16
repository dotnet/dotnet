using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Metrics.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    /// <summary />
    [TestClass]
    public class ApplicationInsightsTelemetryPipelineTests
    {
        private static SemaphoreSlim dontRunInParallelLock = new SemaphoreSlim(1);

        /// <summary />
        [TestMethod]
        public void Ctor()
        {
            dontRunInParallelLock.Wait();
            try
            {
                {
                    Assert.ThrowsException<ArgumentNullException>( () => new ApplicationInsightsTelemetryPipeline((TelemetryClient) null) );
                    Assert.ThrowsException<ArgumentNullException>( () => new ApplicationInsightsTelemetryPipeline((TelemetryConfiguration) null) );
                }
                {
                    TelemetryConfiguration defaultPipeline = TelemetryConfiguration.CreateDefault();
                    //using (defaultPipeline)
                    {
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(defaultPipeline);
                        Assert.IsNotNull(pipelineAdapter);
                    }
                }
            }
            finally
            {
                dontRunInParallelLock.Release();
            }
        }

        // Disabled pending resolution of https://github.com/Microsoft/ApplicationInsights-dotnet/issues/746
        // @ToDo: Re-enble when issue resolved.
        /// <summary />
        // [TestMethod]
        public async Task TrackAsync_SendsCorrectly()
        {
            await dontRunInParallelLock.WaitAsync();
            try
            { 
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        using (var cancelControl = new CancellationTokenSource())
                        {
                            cancelControl.Cancel();
                            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => pipelineAdapter.TrackAsync(null, cancelControl.Token));
                        }
                    }
                }
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        using (var cancelControl = new CancellationTokenSource())
                        {
                            cancelControl.Cancel();
                            await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => pipelineAdapter.TrackAsync(
                                                                                                            new MetricAggregate("mns", "mid", "BadMoniker"),
                                                                                                            cancelControl.Token));
                        }
                    }
                }
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        using (var cancelControl = new CancellationTokenSource())
                        {
                            await Assert.ThrowsExceptionAsync<ArgumentException>(() => pipelineAdapter.TrackAsync(
                                                                                                            new MetricAggregate("mns", "mid", "BadMoniker"),
                                                                                                            cancelControl.Token));
                        }
                    }
                }
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        using (var cancelControl = new CancellationTokenSource())
                        {
                            cancelControl.Cancel();
                            await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => pipelineAdapter.TrackAsync(
                                                                                    new MetricAggregate("mns", "mid", MetricConfigurations.Common.Measurement().Constants().AggregateKindMoniker),
                                                                                    cancelControl.Token));
                        }
                    }
                }
                {
                    // Force registration of the converter with the telemetry pipeline:
                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);

                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        {
                            var agregate = new MetricAggregate("NSa", "M1", MetricConfigurations.Common.Measurement().Constants().AggregateKindMoniker);
                            agregate.Data["Count"] = 1;
                            agregate.Data["Sum"] = 10;
                            await pipelineAdapter.TrackAsync(agregate, CancellationToken.None);
                        }
                        {
                            var agregate = new MetricAggregate("NSb", "M2", MetricConfigurations.Common.Measurement().Constants().AggregateKindMoniker);
                            agregate.Data["Count"] = 0;
                            agregate.Data["Sum"] = 20;
                            await pipelineAdapter.TrackAsync(agregate, CancellationToken.None);
                        }
                        {
                            var agregate = new MetricAggregate("NSc", "M3", MetricConfigurations.Common.Measurement().Constants().AggregateKindMoniker);
                            agregate.Data["Sum"] = 30;
                            await pipelineAdapter.TrackAsync(agregate, CancellationToken.None);
                        }
                        {
                            var agregate = new MetricAggregate("NSd", "M4", MetricConfigurations.Common.Measurement().Constants().AggregateKindMoniker);
                            agregate.Data["Count"] = 2.9;
                            agregate.Data["Sum"] = -40;
                            await pipelineAdapter.TrackAsync(agregate, CancellationToken.None);
                        }

                        // This is super strange. Count is changed from 0 to 1 in MetricTelemetry.Sanitize(). But it seems to happen only in Debug mode!
                        // This needs to be investigated! (@ToDo)
                        // It would indicate that we are sending different telemetry from exactly the same code depending on whether the app
                        // runs under a debugger. That wouldn't be good. (Noticed with SDK 2.3)
                        int expectedCountWhenZero = Debugger.IsAttached ? 1 : 0;

                        // On SDK 2.6.X this sometimes fails when run in parallel with all the other tests and always succeeds when run by itself.
                        // There is some concurrency issue in the Pipiline that is unrelated to the stuff being tested here!

                        Assert.IsNotNull(telemetrySentToChannel);
                        Assert.AreEqual(4, telemetrySentToChannel.Count);

                        Assert.AreEqual(1, telemetrySentToChannel.Where( (item) => (item as MetricTelemetry).Name.Equals("M1") ).Count());
                        Assert.AreEqual("NSa", (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M1") ) as MetricTelemetry).MetricNamespace);
                        Assert.AreEqual(1, (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M1") ) as MetricTelemetry).Count);
                        Assert.AreEqual(10.0, (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M1") ) as MetricTelemetry).Sum);

                        Assert.AreEqual(1, telemetrySentToChannel.Where( (item) => (item as MetricTelemetry).Name.Equals("M2") ).Count());
                        Assert.AreEqual("NSb", (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M2") ) as MetricTelemetry).MetricNamespace);
                        Assert.AreEqual(expectedCountWhenZero, (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M2") ) as MetricTelemetry).Count);
                        Assert.AreEqual(20.0, (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M2") ) as MetricTelemetry).Sum);

                        Assert.AreEqual(1, telemetrySentToChannel.Where( (item) => (item as MetricTelemetry).Name.Equals("M3") ).Count());
                        Assert.AreEqual("NSc", (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M3") ) as MetricTelemetry).MetricNamespace);
                        Assert.AreEqual(expectedCountWhenZero, (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M3") ) as MetricTelemetry).Count);
                        Assert.AreEqual(30.0, (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M3") ) as MetricTelemetry).Sum);

                        Assert.AreEqual(1, telemetrySentToChannel.Where( (item) => (item as MetricTelemetry).Name.Equals("M4") ).Count());
                        Assert.AreEqual("NSd", (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M4") ) as MetricTelemetry).MetricNamespace);
                        Assert.AreEqual(3, (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M4") ) as MetricTelemetry).Count);
                        Assert.AreEqual(-40.0, (telemetrySentToChannel.First( (item) => (item as MetricTelemetry).Name.Equals("M4") ) as MetricTelemetry).Sum);
                    }
                }
            }
            finally
            {
                dontRunInParallelLock.Release();
            }
        }

        // Disabled pending resolution of https://github.com/Microsoft/ApplicationInsights-dotnet/issues/746
        // @ToDo: Re-enble when issue resolved.
        /// <summary />
        // [TestMethod]
        public async Task TrackAsync_HandlesDifferentAggregates()
        {
            const string mockInstrumentationKey = "103CFCEC-BDA6-4EBC-B1F0-2652654DC6FD";

            await dontRunInParallelLock.WaitAsync();
            try
            {
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        var aggregate = new MetricAggregate("NS", "mid-foobar", "Something");
                        await Assert.ThrowsExceptionAsync<ArgumentException>( () => pipelineAdapter.TrackAsync(aggregate, CancellationToken.None) );

                        Assert.AreEqual(0, telemetrySentToChannel.Count);
                    }
                }
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        telemetryPipeline.InstrumentationKey = mockInstrumentationKey;
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        var aggregate = new MetricAggregate("NS", "mid-foobar", "Microsoft.Azure.Measurement");

                        // Becasue we may use the pipeline before any MetricSeriesConfigurationForMeasurement instances have been created,
                        // a converter for an aggregate with the kind miniker "Microsoft.Azure.Measurement" may not registered.
                        // Force the registration:
                        IMetricSeriesConfiguration unusedMeasurementConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);

                        // Now things should work:
                        await pipelineAdapter.TrackAsync(aggregate, CancellationToken.None);

                        Assert.AreEqual(1, telemetrySentToChannel.Count);
                        Assert.IsInstanceOfType(telemetrySentToChannel[0], typeof(MetricTelemetry));
                        MetricTelemetry metricTelemetry = (MetricTelemetry) telemetrySentToChannel[0];

                        // This is super strange. Count is changed from 0 to 1 in MetricTelemetry.Sanitize(). But it seems to happen only in Debug mode!
                        // This needs to be investigated! (@ToDo)
                        // It would indicate that we are sending different telemetry from exactly the same code depending on whether the app
                        // runs under a debugger. That wouldn't be good. (Noticed with SDK 2.3)
                        int expectedCount = Debugger.IsAttached ? 1 : 0;

                        // On SDK 2.6.X this sometimes fails when run in parallel with all the other tests and always succeeds when run by itself.
                        // There is some concurrency issue in the Pipiline that is unrelated to the stuff being tested here!
                        TestUtil.ValidateNumericAggregateValues(metricTelemetry, "NS", "mid-foobar", expectedCount, sum: 0, max: 0, min: 0, stdDev: 0);
                    
                        Assert.AreEqual(1, metricTelemetry.Properties.Count);
                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));
                        Assert.AreEqual("0", metricTelemetry.Properties[TestUtil.AggregationIntervalMonikerPropertyKey]);
                        Assert.IsTrue((metricTelemetry.Timestamp - DateTimeOffset.Now).Duration() < TimeSpan.FromMilliseconds(100));

                        Assert.AreEqual(mockInstrumentationKey, metricTelemetry.Context.InstrumentationKey);
                    }
                }
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        telemetryPipeline.InstrumentationKey = mockInstrumentationKey;
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        var aggregate = new MetricAggregate("NS", "mid-foobar", "Microsoft.Azure.Measurement");

                        aggregate.AggregationPeriodStart = new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8));
                        aggregate.AggregationPeriodDuration = TimeSpan.FromSeconds(90);

                        await pipelineAdapter.TrackAsync(aggregate, CancellationToken.None);

                        Assert.AreEqual(1, telemetrySentToChannel.Count);
                        Assert.IsInstanceOfType(telemetrySentToChannel[0], typeof(MetricTelemetry));
                        MetricTelemetry metricTelemetry = (MetricTelemetry) telemetrySentToChannel[0];

                        // This is super strange. Count is changed from 0 to 1 in MetricTelemetry.Sanitize(). But it seems to happen only in Debug mode!
                        // This needs to be investigated! (@ToDo)
                        // It would indicate that we are sending different telemetry from exactly the same code depending on whether the app
                        // runs under a debugger. That wouldn't be good. (Noticed with SDK 2.3)
                        int expectedCount = Debugger.IsAttached ? 1 : 0;
                        TestUtil.ValidateNumericAggregateValues(
                                                        metricTelemetry,
                                                        "NS",
                                                        "mid-foobar",
                                                        expectedCount,
                                                        sum:        0,
                                                        max:        0,
                                                        min:        0,
                                                        stdDev:     0,
                                                        timestamp:  new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8)),
                                                        periodMs:   "90000");
                        Assert.AreEqual(1, metricTelemetry.Properties.Count);

                        Assert.AreEqual(mockInstrumentationKey, metricTelemetry.Context.InstrumentationKey);
                        TestUtil.ValidateSdkVersionString(metricTelemetry.Context.GetInternalContext().SdkVersion);
                    }
                }
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        telemetryPipeline.InstrumentationKey = mockInstrumentationKey;
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        var aggregate = new MetricAggregate("NS", "mid-foobar", "Microsoft.Azure.Measurement");

                        aggregate.AggregationPeriodStart = new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8));
                        aggregate.AggregationPeriodDuration = TimeSpan.FromSeconds(90);

                        aggregate.Data["Foo"] = "Bar";

                        await pipelineAdapter.TrackAsync(aggregate, CancellationToken.None);

                        Assert.AreEqual(1, telemetrySentToChannel.Count);
                        Assert.IsInstanceOfType(telemetrySentToChannel[0], typeof(MetricTelemetry));
                        MetricTelemetry metricTelemetry = (MetricTelemetry) telemetrySentToChannel[0];

                        // This is super strange. Count is changed from 0 to 1 in MetricTelemetry.Sanitize(). But it seems to happen only in Debug mode!
                        // This needs to be investigated! (@ToDo)
                        // It would indicate that we are sending different telemetry from exactly the same code depending on whether the app
                        // runs under a debugger. That wouldn't be good. (Noticed with SDK 2.3)
                        int expectedCount = Debugger.IsAttached ? 1 : 0;
                        TestUtil.ValidateNumericAggregateValues(
                                                        metricTelemetry,
                                                        "NS", 
                                                        "mid-foobar",
                                                        expectedCount,
                                                        sum:        0,
                                                        max:        0,
                                                        min:        0,
                                                        stdDev:     0,
                                                        timestamp:  new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8)),
                                                        periodMs:   "90000");
                        Assert.AreEqual(1, metricTelemetry.Properties.Count);

                        Assert.AreEqual(mockInstrumentationKey, metricTelemetry.Context.InstrumentationKey);
                        TestUtil.ValidateSdkVersionString(metricTelemetry.Context.GetInternalContext().SdkVersion);
                    }
                }
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        telemetryPipeline.InstrumentationKey = mockInstrumentationKey;
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        var aggregate = new MetricAggregate("NS", "mid-foobar", "Microsoft.Azure.Measurement");

                        aggregate.AggregationPeriodStart = new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8));
                        aggregate.AggregationPeriodDuration = TimeSpan.FromSeconds(90);

                        aggregate.Data["Murr"] = "Miau";
                        aggregate.Data["Purr"] = null;
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Count] = 1.2;
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Sum] = "one";
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Min] = "2.3";
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Max] = "-4";
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.StdDev] = 5;

                        await pipelineAdapter.TrackAsync(aggregate, CancellationToken.None);

                        Assert.AreEqual(1, telemetrySentToChannel.Count);
                        Assert.IsInstanceOfType(telemetrySentToChannel[0], typeof(MetricTelemetry));
                        MetricTelemetry metricTelemetry = (MetricTelemetry) telemetrySentToChannel[0];

                        TestUtil.ValidateNumericAggregateValues(
                                                        metricTelemetry,
                                                        "NS",
                                                        "mid-foobar",
                                                        count:      1,
                                                        sum:        0,
                                                        max:        -4,
                                                        min:        2.3,
                                                        stdDev:     5,
                                                        timestamp:  new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8)),
                                                        periodMs:   "90000");
                        Assert.AreEqual(1, metricTelemetry.Properties.Count);

                        Assert.AreEqual(mockInstrumentationKey, metricTelemetry.Context.InstrumentationKey);
                        TestUtil.ValidateSdkVersionString(metricTelemetry.Context.GetInternalContext().SdkVersion);
                    }
                }
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        telemetryPipeline.InstrumentationKey = mockInstrumentationKey;
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        var aggregate = new MetricAggregate("NS", "mid-foobar", "Microsoft.Azure.Measurement");

                        aggregate.AggregationPeriodStart = new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8));
                        aggregate.AggregationPeriodDuration = TimeSpan.FromSeconds(90);

                        aggregate.Data["Murr"] = "Miau";
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Count] = -3.7;
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Sum] = "-100";
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Min] = -10000000000;
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Max] = ((double) Int32.MaxValue) + 100;
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.StdDev] = -2;

                        aggregate.Dimensions["Dim 1"] = "DV1";
                        aggregate.Dimensions["Dim 2"] = "DV2";
                        aggregate.Dimensions["Dim 3"] = "DV3";
                        aggregate.Dimensions["Dim 2"] = "DV2a";
                        aggregate.Dimensions["Dim 4"] = "";
                        aggregate.Dimensions["Dim 5"] = null;
                        aggregate.Dimensions["Dim 6"] = "  ";

                        await pipelineAdapter.TrackAsync(aggregate, CancellationToken.None);

                        Assert.AreEqual(1, telemetrySentToChannel.Count);
                        Assert.IsInstanceOfType(telemetrySentToChannel[0], typeof(MetricTelemetry));
                        MetricTelemetry metricTelemetry = (MetricTelemetry) telemetrySentToChannel[0];

                        // This is super strange. Count is changed from 0 to 1 in MetricTelemetry.Sanitize(). But it seems to happen only in Debug mode!
                        // This needs to be investigated! (@ToDo)
                        // It would indicate that we are sending different telemetry from exactly the same code depending on whether the app
                        // runs under a debugger. That wouldn't be good. (Noticed with SDK 2.3)
                        int expectedCount = Debugger.IsAttached ? 1 : -4;
                        TestUtil.ValidateNumericAggregateValues(
                                                        metricTelemetry,
                                                        "NS",
                                                        "mid-foobar",
                                                        expectedCount,
                                                        sum:        -100,
                                                        max:        ((double) Int32.MaxValue) + 100,
                                                        min:        -10000000000,
                                                        stdDev:     -2,
                                                        timestamp:  new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8)),
                                                        periodMs:   "90000");

                        string props = $"metricTelemetry.Properties[{metricTelemetry.Properties.Count}] {{ ";
                        foreach (KeyValuePair<string, string> kvp in metricTelemetry.Properties)
                        {
                            props = props + $"[\"{kvp.Key}\"]=\"{kvp.Value}\", ";
                        }
                        props = props + " }";

                        // This is another super strange case where we seem to be gettin gdifferent results depending on whether a dubugger is attached.
                        // It seems to be the same Sanitization issue as above (@ToDo)
                        if (Debugger.IsAttached)
                        {
                            Assert.AreEqual(4, metricTelemetry.Properties.Count, props);
                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 1"));
                            Assert.AreEqual("DV1", metricTelemetry.Properties["Dim 1"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 2"));
                            Assert.AreEqual("DV2a", metricTelemetry.Properties["Dim 2"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 3"));
                            Assert.AreEqual("DV3", metricTelemetry.Properties["Dim 3"]);
                        }
                        else
                        {
                            Assert.AreEqual(6, metricTelemetry.Properties.Count, props);
                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 1"));
                            Assert.AreEqual("DV1", metricTelemetry.Properties["Dim 1"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 2"));
                            Assert.AreEqual("DV2a", metricTelemetry.Properties["Dim 2"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 3"));
                            Assert.AreEqual("DV3", metricTelemetry.Properties["Dim 3"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 4"));
                            Assert.AreEqual("", metricTelemetry.Properties["Dim 4"]);

                            Assert.IsFalse(metricTelemetry.Properties.ContainsKey("Dim 5"));

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 6"));
                            Assert.AreEqual("  ", metricTelemetry.Properties["Dim 6"]);
                        }

                        Assert.AreEqual(mockInstrumentationKey, metricTelemetry.Context.InstrumentationKey);
                        TestUtil.ValidateSdkVersionString(metricTelemetry.Context.GetInternalContext().SdkVersion);
                    }
                }
                {
                    IList<ITelemetry> telemetrySentToChannel;
                    TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                    using (telemetryPipeline)
                    {
                        telemetryPipeline.InstrumentationKey = mockInstrumentationKey;
                        var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                        var aggregate = new MetricAggregate("NS", "mid-foobar", "Microsoft.Azure.Measurement");

                        aggregate.AggregationPeriodStart = new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8));
                        aggregate.AggregationPeriodDuration = TimeSpan.FromSeconds(90);

                        aggregate.Data["Murr"] = "Miau";
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Count] = -3.7;
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Sum] = null;
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Min] = -10000000000;
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Max] = ((double) Int32.MaxValue) + 100;
                        aggregate.Data[MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.StdDev] = -2;

                        aggregate.Dimensions["Dim 1"] = "DV1";
                        aggregate.Dimensions["Dim 2"] = "DV2";
                        aggregate.Dimensions["Dim 3"] = "DV3";
                        aggregate.Dimensions["Dim 2"] = "DV2a";
                        aggregate.Dimensions["Dim 4"] = "";
                        aggregate.Dimensions["Dim 5"] = null;
                        aggregate.Dimensions["Dim 6"] = "  ";
                        aggregate.Dimensions[""] = "DVb1";
                        aggregate.Dimensions[" "] = "DVb2";
                        Assert.ThrowsException<ArgumentNullException>( () => { aggregate.Dimensions[null] = "DVb2"; } );

                        aggregate.Dimensions[MetricDimensionNames.TelemetryContext.InstrumentationKey] = "Aggregate's Instrumentsion Key";
                        aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Property("Prop 1")] = "PV1";
                        aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Property("Prop 2")] = "PV2";
                        aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Property("Dim 1")] = "Dim V 1";
                        aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Operation.Id] = "OpId xx";
                        aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Operation.Name] = "OpName xx";
                        aggregate.Dimensions[MetricDimensionNames.TelemetryContext.User.UserAgent] = "UA xx";
                        aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Cloud.RoleName] = "RN xx";

                        await pipelineAdapter.TrackAsync(aggregate, CancellationToken.None);

                        Assert.AreEqual(1, telemetrySentToChannel.Count);
                        Assert.IsInstanceOfType(telemetrySentToChannel[0], typeof(MetricTelemetry));
                        MetricTelemetry metricTelemetry = (MetricTelemetry) telemetrySentToChannel[0];

                        // This is super strange. Count is changed from 0 to 1 in MetricTelemetry.Sanitize(). But it seems to happen only in Debug mode!
                        // This needs to be investigated! (@ToDo)
                        // It would indicate that we are sending different telemetry from exactly the same code depending on whether the app
                        // runs under a debugger. That wouldn't be good. (Noticed with SDK 2.3)
                        int expectedCount = Debugger.IsAttached ? 1 : -4;
                        TestUtil.ValidateNumericAggregateValues(
                                                        metricTelemetry,
                                                        "NS",
                                                        "mid-foobar",
                                                        expectedCount,
                                                        sum:        0,
                                                        max:        ((double) Int32.MaxValue) + 100,
                                                        min:        -10000000000,
                                                        stdDev:     -2,
                                                        timestamp:  new DateTimeOffset(2017, 10, 30, 0, 1, 0, TimeSpan.FromHours(8)),
                                                        periodMs:   "90000");

                        string props = $"metricTelemetry.Properties[{metricTelemetry.Properties.Count}] {{ ";
                        foreach (KeyValuePair<string, string> kvp in metricTelemetry.Properties)
                        {
                            props = props + $"[\"{kvp.Key}\"]=\"{kvp.Value}\", ";
                        }
                        props = props + " }";

                        // This is another super strange case where we seem to be gettin gdifferent results depending on whether a dubugger is attached.
                        // It seems to be the same Sanitization issue as above (@ToDo)
                        if (Debugger.IsAttached)
                        {
                            Assert.AreEqual(6, metricTelemetry.Properties.Count, props);
                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 1"));
                            Assert.AreEqual("DV1", metricTelemetry.Properties["Dim 1"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 2"));
                            Assert.AreEqual("DV2a", metricTelemetry.Properties["Dim 2"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 3"));
                            Assert.AreEqual("DV3", metricTelemetry.Properties["Dim 3"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Prop 1"));
                            Assert.AreEqual("PV1", metricTelemetry.Properties["Prop 1"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Prop 2"));
                            Assert.AreEqual("PV2", metricTelemetry.Properties["Prop 2"]);
                        }
                        else
                        {
                            Assert.AreEqual(8, metricTelemetry.Properties.Count, props);
                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 1"));
                            Assert.AreEqual("DV1", metricTelemetry.Properties["Dim 1"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 2"));
                            Assert.AreEqual("DV2a", metricTelemetry.Properties["Dim 2"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 3"));
                            Assert.AreEqual("DV3", metricTelemetry.Properties["Dim 3"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 4"));
                            Assert.AreEqual("", metricTelemetry.Properties["Dim 4"]);

                            Assert.IsFalse(metricTelemetry.Properties.ContainsKey("Dim 5"));

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 6"));
                            Assert.AreEqual("  ", metricTelemetry.Properties["Dim 6"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Prop 1"));
                            Assert.AreEqual("PV1", metricTelemetry.Properties["Prop 1"]);

                            Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Prop 2"));
                            Assert.AreEqual("PV2", metricTelemetry.Properties["Prop 2"]);
                        }

                        Assert.AreEqual("OpId xx", metricTelemetry.Context.Operation.Id);
                        Assert.AreEqual("OpName xx", metricTelemetry.Context.Operation.Name);
                        Assert.AreEqual("UA xx", metricTelemetry.Context.User.UserAgent);
                        Assert.AreEqual("RN xx", metricTelemetry.Context.Cloud.RoleName);

                        Assert.AreEqual("Aggregate's Instrumentsion Key", metricTelemetry.Context.InstrumentationKey);
                        TestUtil.ValidateSdkVersionString(metricTelemetry.Context.GetInternalContext().SdkVersion);
                    }
                }
            }
            finally
            {
                dontRunInParallelLock.Release();
            }
        }

        // Disabled pending resolution of https://github.com/Microsoft/ApplicationInsights-dotnet/issues/746
        // @ToDo: Re-enble when issue resolved.
        /// <summary />
        // [TestMethod]
        public async Task TrackAsync_CorrectlyCopiesTelemetryContextDimensions()
        {
            const string mockInstrumentationKey = "103CFCEC-BDA6-4EBC-B1F0-2652654DC6FD";

            {
                IList<ITelemetry> telemetrySentToChannel;
                TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                using (telemetryPipeline)
                {
                    telemetryPipeline.InstrumentationKey = mockInstrumentationKey;
                    var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                    var aggregate = new MetricAggregate("NS", "mid-foobar", "Microsoft.Azure.Measurement");

                    aggregate.AggregationPeriodStart = new DateTimeOffset(1492, 10, 12, 0, 0, 0, TimeSpan.Zero);

                    aggregate.Dimensions["Dim 1"] = "DV1";
                    aggregate.Dimensions["Dim 2"] = "DV2";
                    aggregate.Dimensions["Dim 3"] = "DV3";
                    aggregate.Dimensions["Dim 2"] = "DV2a";
                    aggregate.Dimensions["Dim 4"] = "";
                    aggregate.Dimensions["Dim 5"] = null;
                    aggregate.Dimensions["Dim 6"] = "  ";
                    aggregate.Dimensions[""] = "DVb1";
                    aggregate.Dimensions[" "] = "DVb2";
                    Assert.ThrowsException<ArgumentNullException>(() => { aggregate.Dimensions[null] = "DVb2"; });

                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.InstrumentationKey] = "Aggregate's Instrumentsion Key";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Property("Prop 1")] = "PV1";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Property("Prop 2")] = "PV2";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Property("Dim 1")] = "Dim V 1";

                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Cloud.RoleInstance] = "A";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Cloud.RoleName] = "B";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Component.Version] = "C";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Device.Id] = "D";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Device.Language] = "E";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Device.Model] = "F";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Device.NetworkType] = "G";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Device.OemName] = "H";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Device.OperatingSystem] = "I";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Device.ScreenResolution] = "J";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Device.Type] = "K";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Location.Ip] = "L";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Operation.CorrelationVector] = "M";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Operation.Id] = "N";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Operation.Name] = "O";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Operation.ParentId] = "P";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Operation.SyntheticSource] = "R";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Session.Id] = "S";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Session.IsFirst] = "TRUE";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.User.AccountId] = "U";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.User.AuthenticatedUserId] = "V";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.User.Id] = "W";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.User.UserAgent] = "X";

                    // Becasue we may use the pipeline before any MetricSeriesConfigurationForMeasurement instances have been created,
                    // a converter for an aggregate with the kind miniker "Microsoft.Azure.Measurement" may not registered.
                    // Force the registration:
                    IMetricSeriesConfiguration unusedMeasurementConfig = new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false);

                    // Now things should work:
                    await pipelineAdapter.TrackAsync(aggregate, CancellationToken.None);

                    Assert.AreEqual(1, telemetrySentToChannel.Count);
                    Assert.IsInstanceOfType(telemetrySentToChannel[0], typeof(MetricTelemetry));
                    MetricTelemetry metricTelemetry = (MetricTelemetry) telemetrySentToChannel[0];

                    // This is super strange. Count is changed from 0 to 1 in MetricTelemetry.Sanitize(). But it seems to happen only in Debug mode!
                    // This needs to be investigated! (@ToDo)
                    // It would indicate that we are sending different telemetry from exactly the same code depending on whether the app
                    // runs under a debugger. That wouldn't be good. (Noticed with SDK 2.3)
                    int expectedCount = Debugger.IsAttached ? 1 : 0;
                    TestUtil.ValidateNumericAggregateValues(
                                                    metricTelemetry,
                                                    "NS",
                                                    "mid-foobar",
                                                    expectedCount,
                                                    sum:    0,
                                                    max:    0,
                                                    min:    0,
                                                    stdDev: 0,
                                                    timestamp:  new DateTimeOffset(1492, 10, 12, 0, 0, 0, TimeSpan.Zero),
                                                    periodMs:   "0");

                    string props = $"metricTelemetry.Properties[{metricTelemetry.Properties.Count}] {{ ";
                    foreach (KeyValuePair<string, string> kvp in metricTelemetry.Properties)
                    {
                        props = props + $"[\"{kvp.Key}\"]=\"{kvp.Value}\", ";
                    }
                    props = props + " }";

                    // This is another super strange case where we seem to be gettin gdifferent results depending on whether a dubugger is attached.
                    // It seems to be the same Sanitization issue as above (@ToDo)
                    if (Debugger.IsAttached)
                    {
                        Assert.AreEqual(9, metricTelemetry.Properties.Count, props);
                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 1"));
                        Assert.AreEqual("DV1", metricTelemetry.Properties["Dim 1"]);

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 2"));
                        Assert.AreEqual("DV2a", metricTelemetry.Properties["Dim 2"]);

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 3"));
                        Assert.AreEqual("DV3", metricTelemetry.Properties["Dim 3"]);

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Prop 1"));
                        Assert.AreEqual("PV1", metricTelemetry.Properties["Prop 1"]);

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Prop 2"));
                        Assert.AreEqual("PV2", metricTelemetry.Properties["Prop 2"]);
                    }
                    else
                    {
                        Assert.AreEqual(11, metricTelemetry.Properties.Count, props);
                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 1"));
                        Assert.AreEqual("DV1", metricTelemetry.Properties["Dim 1"]);

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 2"));
                        Assert.AreEqual("DV2a", metricTelemetry.Properties["Dim 2"]);

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 3"));
                        Assert.AreEqual("DV3", metricTelemetry.Properties["Dim 3"]);

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 4"));
                        Assert.AreEqual("", metricTelemetry.Properties["Dim 4"]);

                        Assert.IsFalse(metricTelemetry.Properties.ContainsKey("Dim 5"));

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Dim 6"));
                        Assert.AreEqual("  ", metricTelemetry.Properties["Dim 6"]);

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Prop 1"));
                        Assert.AreEqual("PV1", metricTelemetry.Properties["Prop 1"]);

                        Assert.IsTrue(metricTelemetry.Properties.ContainsKey("Prop 2"));
                        Assert.AreEqual("PV2", metricTelemetry.Properties["Prop 2"]);
                    }

                    TestUtil.ValidateSdkVersionString(metricTelemetry.Context.GetInternalContext().SdkVersion);
                    Assert.AreEqual("Aggregate's Instrumentsion Key", metricTelemetry.Context.InstrumentationKey);

                    Assert.AreEqual("A", metricTelemetry.Context.Cloud.RoleInstance);
                    Assert.AreEqual("B", metricTelemetry.Context.Cloud.RoleName);
                    Assert.AreEqual("C", metricTelemetry.Context.Component.Version);
                    Assert.AreEqual("D", metricTelemetry.Context.Device.Id);
#pragma warning disable CS0618 // Type or member is obsolete
                    Assert.AreEqual("E", metricTelemetry.Context.Device.Language);
#pragma warning restore CS0618 // Type or member is obsolete
                    Assert.AreEqual("F", metricTelemetry.Context.Device.Model);
#pragma warning disable CS0618 // Type or member is obsolete
                    Assert.AreEqual("G", metricTelemetry.Context.Device.NetworkType);
#pragma warning restore CS0618 // Type or member is obsolete
                    Assert.AreEqual("H", metricTelemetry.Context.Device.OemName);
                    Assert.AreEqual("I", metricTelemetry.Context.Device.OperatingSystem);
#pragma warning disable CS0618 // Type or member is obsolete
                    Assert.AreEqual("J", metricTelemetry.Context.Device.ScreenResolution);
#pragma warning restore CS0618 // Type or member is obsolete
                    Assert.AreEqual("K", metricTelemetry.Context.Device.Type);
                    Assert.AreEqual("L", metricTelemetry.Context.Location.Ip);
                    Assert.AreEqual("M", metricTelemetry.Context.Operation.CorrelationVector);
                    Assert.AreEqual("N", metricTelemetry.Context.Operation.Id);
                    Assert.AreEqual("O", metricTelemetry.Context.Operation.Name);
                    Assert.AreEqual("P", metricTelemetry.Context.Operation.ParentId);
                    Assert.AreEqual("R", metricTelemetry.Context.Operation.SyntheticSource);
                    Assert.AreEqual("S", metricTelemetry.Context.Session.Id);
                    Assert.AreEqual(true, metricTelemetry.Context.Session.IsFirst);
                    Assert.AreEqual("U", metricTelemetry.Context.User.AccountId);
                    Assert.AreEqual("V", metricTelemetry.Context.User.AuthenticatedUserId);
                    Assert.AreEqual("W", metricTelemetry.Context.User.Id);
                    Assert.AreEqual("X", metricTelemetry.Context.User.UserAgent);
                }
            }
            {
                IList<ITelemetry> telemetrySentToChannel;
                TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                using (telemetryPipeline)
                {
                    telemetryPipeline.InstrumentationKey = mockInstrumentationKey;
                    var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                    var aggregate = new MetricAggregate("NS", "mid-foobar", "Microsoft.Azure.Measurement");

                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Cloud.RoleName] = "";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Device.Model] = null;
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Location.Ip] = "  ";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Session.IsFirst] = "0";

                    await pipelineAdapter.TrackAsync(aggregate, CancellationToken.None);

                    Assert.AreEqual(1, telemetrySentToChannel.Count);
                    Assert.IsInstanceOfType(telemetrySentToChannel[0], typeof(MetricTelemetry));
                    MetricTelemetry metricTelemetry = (MetricTelemetry) telemetrySentToChannel[0];

                    Assert.AreEqual(1, metricTelemetry.Properties.Count);
                    Assert.IsTrue(metricTelemetry.Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

                    TestUtil.ValidateSdkVersionString(metricTelemetry.Context.GetInternalContext().SdkVersion);

                    Assert.AreEqual(null, metricTelemetry.Context.Cloud.RoleName);
                    Assert.AreEqual(null, metricTelemetry.Context.Device.Model);
                    Assert.AreEqual("  ", metricTelemetry.Context.Location.Ip);
                    Assert.AreEqual(false, metricTelemetry.Context.Session.IsFirst);
                }
            }
            {
                IList<ITelemetry> telemetrySentToChannel;
                TelemetryConfiguration telemetryPipeline = TestUtil.CreateAITelemetryConfig(out telemetrySentToChannel);
                using (telemetryPipeline)
                {
                    telemetryPipeline.InstrumentationKey = mockInstrumentationKey;
                    var pipelineAdapter = new ApplicationInsightsTelemetryPipeline(telemetryPipeline);

                    var aggregate = new MetricAggregate("NS", "mid-foobar", "Microsoft.Azure.Measurement");
                    
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Session.Id] = "some string";
                    aggregate.Dimensions[MetricDimensionNames.TelemetryContext.Session.IsFirst] = "bad string";

                    await pipelineAdapter.TrackAsync(aggregate, CancellationToken.None);

                    Assert.AreEqual(1, telemetrySentToChannel.Count);
                    Assert.IsInstanceOfType(telemetrySentToChannel[0], typeof(MetricTelemetry));
                    MetricTelemetry metricTelemetry = (MetricTelemetry) telemetrySentToChannel[0];

                    Assert.AreEqual(1, metricTelemetry.Properties.Count);
                    Assert.IsTrue(metricTelemetry.Properties.ContainsKey(TestUtil.AggregationIntervalMonikerPropertyKey));

                    TestUtil.ValidateSdkVersionString(metricTelemetry.Context.GetInternalContext().SdkVersion);

                    Assert.AreEqual("some string", metricTelemetry.Context.Session.Id);
                    Assert.AreEqual(null, metricTelemetry.Context.Session.IsFirst);
                }
            }
        }
    }
}

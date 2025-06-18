using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Metrics.Extensibility;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Metrics.TestUtility
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1724 
    public static class TestUtil
#pragma warning restore CA1724
    {
        public const string AggregationIntervalMonikerPropertyKey = "_MS.AggregationIntervalMs";
        public const double MaxAllowedPrecisionError = 0.00001;

        //public const bool WaitForDefaultAggregationCycleCompletion = true;
        public const bool WaitForDefaultAggregationCycleCompletion = false;

        public static void AssertAreEqual<T>(T[] array1, T[] array2)
        {
            if (array1 == array2)
            {
                return;
            }

            Assert.IsNotNull(array1);
            Assert.IsNotNull(array2);

            Assert.AreEqual(array1.Length, array1.Length);

            for(int i = 0; i < array1.Length; i++)
            {
                Assert.AreEqual(array1[i], array2[i], message: $" at index {i}");
            }
        }

        public static bool AreEqual<T>(T[] array1, T[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }

            if (array1 == null)
            {
                return false;
            }

            if (array2 == null)
            {
                return false;
            }

            if (array1.Length != array1.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] == null && array2[i] == null)
                {
                    continue;
                }

                if (array1 == null)
                {
                    return false;
                }

                if (array2 == null)
                {
                    return false;
                }

                if (! array1[i].Equals(array2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static TelemetryConfiguration CreateAITelemetryConfig()
        {
            IList<ITelemetry> telemetrySentToChannel;
            return CreateAITelemetryConfig(out telemetrySentToChannel);
        }

        public static TelemetryConfiguration CreateAITelemetryConfig(out IList<ITelemetry> telemetrySentToChannel)
        {
            StubApplicationInsightsTelemetryChannel channel = new StubApplicationInsightsTelemetryChannel();
            string iKey = Guid.NewGuid().ToString("D");
            TelemetryConfiguration telemetryConfig = new TelemetryConfiguration(iKey, channel);

            var channelBuilder = new TelemetryProcessorChainBuilder(telemetryConfig);
            channelBuilder.Build();

            foreach (ITelemetryProcessor initializer in telemetryConfig.TelemetryInitializers)
            {
                ITelemetryModule m = initializer as ITelemetryModule;
                if (m != null)
                {
                    m.Initialize(telemetryConfig);
                }
            }

            foreach (ITelemetryProcessor processor in telemetryConfig.TelemetryProcessors)
            {
                ITelemetryModule m = processor as ITelemetryModule;
                if (m != null)
                {
                    m.Initialize(telemetryConfig);
                }
            }

            telemetrySentToChannel = channel.TelemetryItems;
            return telemetryConfig;
        }

        public static void ValidateNumericAggregateValues(ITelemetry aggregate, string ns, string name, int count, double sum, double max, double min, double stdDev, DateTimeOffset timestamp, string periodMs)
        {
            ValidateNumericAggregateValues(aggregate, ns, name, count, sum, max, min, stdDev);

            var metricAggregate = (MetricTelemetry) aggregate;
            Assert.AreEqual(timestamp, metricAggregate.Timestamp, "metricAggregate.Timestamp mismatch");
            Assert.AreEqual(periodMs, metricAggregate?.Properties?[TestUtil.AggregationIntervalMonikerPropertyKey], "metricAggregate.Properties[AggregationIntervalMonikerPropertyKey] mismatch");
        }

        public static void ValidateNumericAggregateValues(ITelemetry aggregate, string ns, string name, int count, double sum, double max, double min, double? stdDev)
        {
            Assert.IsNotNull(aggregate);

            MetricTelemetry metricAggregate = aggregate as MetricTelemetry;

            Assert.IsNotNull(metricAggregate);

            Assert.AreEqual(ns, metricAggregate.MetricNamespace, "metricAggregate.MetricNamespace mismatch");
            Assert.AreEqual(name, metricAggregate.Name, "metricAggregate.Name mismatch");
            Assert.AreEqual(count, metricAggregate.Count, "metricAggregate.Count mismatch");
            Assert.AreEqual(sum, metricAggregate.Sum, TestUtil.MaxAllowedPrecisionError, "metricAggregate.Sum mismatch");
            Assert.AreEqual(max, metricAggregate.Max.Value, TestUtil.MaxAllowedPrecisionError, "metricAggregate.Max mismatch");
            Assert.AreEqual(min, metricAggregate.Min.Value, TestUtil.MaxAllowedPrecisionError, "metricAggregate.Min mismatch");

            // For very large numbers we perform an approx comparison.
            if (!stdDev.HasValue)
            {
                Assert.IsNull(metricAggregate.StandardDeviation);
            }
            else
            {
                if (Math.Abs(stdDev.Value) > Int64.MaxValue)
                {
                    double expectedStdDevScale = Math.Floor(Math.Log10(Math.Abs(stdDev.Value)));
                    double actualStdDevScale = Math.Floor(Math.Log10(Math.Abs(metricAggregate.StandardDeviation.Value)));
                    Assert.AreEqual(expectedStdDevScale, actualStdDevScale, "metricAggregate.StandardDeviation (exponent) mismatch");
                    Assert.AreEqual(
                                stdDev.Value / Math.Pow(10, expectedStdDevScale),
                                metricAggregate.StandardDeviation.Value / Math.Pow(10, actualStdDevScale),
                                TestUtil.MaxAllowedPrecisionError,
                                "metricAggregate.StandardDeviation (significant part) mismatch");
                }
                else
                {
                    Assert.AreEqual(stdDev.Value, metricAggregate.StandardDeviation.Value, TestUtil.MaxAllowedPrecisionError, "metricAggregate.StandardDeviation mismatch");
                }
            }
        }

        internal static void ValidateNumericAggregateValues(MetricAggregate aggregate, string ns, string name, int count, double sum, double max, double min, double stdDev, DateTimeOffset timestamp, long periodMs, string aggKindMoniker)
        {
            ValidateNumericAggregateValues(aggregate, ns, name, count, sum, max, min, stdDev, aggKindMoniker);

            Assert.AreEqual(timestamp, aggregate.AggregationPeriodStart, "metricAggregate.Timestamp mismatch");
            Assert.AreEqual(periodMs, (long) aggregate.AggregationPeriodDuration.TotalMilliseconds, "metricAggregate.Properties[AggregationIntervalMonikerPropertyKey] mismatch");
        }

        internal static void ValidateNumericAggregateValues(MetricAggregate aggregate, string ns, string name, int count, double sum, double max, double min, double stdDev, string aggKindMoniker)
        {
            Assert.IsNotNull(aggregate);

            Assert.AreEqual(aggKindMoniker, aggregate.AggregationKindMoniker);

            Assert.AreEqual(ns, aggregate.MetricNamespace, "aggregate.MetricNamespace mismatch");
            Assert.AreEqual(name, aggregate.MetricId, "aggregate.Name mismatch");

            Assert.IsNotNull(aggregate.AggregationKindMoniker);

            if (aggregate.AggregationKindMoniker.Equals(MetricConfigurations.Common.Measurement().Constants().AggregateKindMoniker))
            {
                Assert.AreEqual(count, aggregate.Data["Count"], "aggregate.Count mismatch");
                Assert.AreEqual(sum, (double) aggregate.Data["Sum"], TestUtil.MaxAllowedPrecisionError, "aggregate.Sum mismatch");
                Assert.AreEqual(max, (double) aggregate.Data["Max"], TestUtil.MaxAllowedPrecisionError, "aggregate.Max mismatch");
                Assert.AreEqual(min, (double) aggregate.Data["Min"], TestUtil.MaxAllowedPrecisionError, "aggregate.Min mismatch");

                // For very large numbers we perform an approx comparison.
                if (Math.Abs(stdDev) > Int64.MaxValue)
                {
                    double expectedStdDevScale = Math.Floor(Math.Log10(Math.Abs(stdDev)));
                    double actualStdDevScale = Math.Floor(Math.Log10(Math.Abs((double) aggregate.Data["StdDev"])));
                    Assert.AreEqual(expectedStdDevScale, actualStdDevScale, "aggregate.StandardDeviation (exponent) mismatch");
                    Assert.AreEqual(
                                stdDev / Math.Pow(10, expectedStdDevScale),
                                ((double) aggregate.Data["StdDev"]) / Math.Pow(10, actualStdDevScale),
                                TestUtil.MaxAllowedPrecisionError,
                                "aggregate.StandardDeviation (significant part) mismatch");
                }
                else
                {
                    Assert.AreEqual(stdDev, (double) aggregate.Data["StdDev"], TestUtil.MaxAllowedPrecisionError, "aggregate.StandardDeviation mismatch");
                }
            }
            else if (aggregate.AggregationKindMoniker.Equals(MetricSeriesConfigurationForTestingAccumulatorBehavior.Constants.AggregateKindMoniker))
            {
                Assert.AreEqual(sum, (double) aggregate.Data["Sum"], TestUtil.MaxAllowedPrecisionError, "aggregate.Sum mismatch");
                Assert.AreEqual(max, (double) aggregate.Data["Max"], TestUtil.MaxAllowedPrecisionError, "aggregate.Max mismatch");
                Assert.AreEqual(min, (double) aggregate.Data["Min"], TestUtil.MaxAllowedPrecisionError, "aggregate.Min mismatch");
            }
            else
            {
                Assert.Fail($"Unexpected aggregate.AggregationKindMoniker: {aggregate.AggregationKindMoniker}");
            }
        }

        /// <summary />
        /// <param name="versionMoniker"></param>
        public static void ValidateSdkVersionString(string versionMoniker)
        {
            Assert.IsNotNull(versionMoniker);

            // Expected result example: "m-agg2:2.6.0-12552"
#if NETCOREAPP // This constant is defined for all versions of NetCore https://docs.microsoft.com/en-us/dotnet/core/tutorials/libraries#how-to-multitarget
        const string expectedPrefix = "m-agg2c:";
#else
            const string expectedPrefix = "m-agg2:";
#endif        

            string sdkRevisionComponentStr = versionMoniker.Substring(expectedPrefix.Length);
            string expectedVersion = SdkVersionUtils.GetSdkVersion(String.Empty);

            Assert.IsTrue(versionMoniker.StartsWith(expectedPrefix), $"versionMoniker: \"{versionMoniker}\"; expectedPrefix: \"{expectedPrefix}\"");
            Assert.IsTrue(versionMoniker.EndsWith(expectedVersion), $"versionMoniker: \"{versionMoniker}\"; expectedVersion: \"{expectedVersion}\"");
        }

        /// <summary>
        /// The MetricManager contains an instance of DefaultAggregationPeriodCycle which encapsulates a managed thread.
        /// That tread sleeps for most of the time, and once per minute it wakes up to cycle aggregators and send aggregates.
        /// Accordingly, it can only stop itself once per minute by checking a flag.
        /// In tests we do not really need to wait for that thread to exit, but then we will get a message like:
        /// ----
        /// System.AppDomainUnloadedException:
        ///  Attempted to access an unloaded AppDomain. This can happen if the test(s) started a thread but did not stop it.
        ///  Make sure that all the threads started by the test(s) are stopped before completion.
        /// ----
        /// However, if we wait for the completion, than each test involving the MetricManager will take a minute.
        /// So we will call this method at the end of each test involving the MetricManager and then we will use a flag to switch
        /// between waiting and not waiting. This will let us run test quickly most of the time, but we can switch the flag to get
        /// a clean test run.
        /// </summary>
        /// <param name="metricManagers"></param>
        public static void CompleteDefaultAggregationCycle(params MetricManager[] metricManagers)
        {
            if (metricManagers == null)
            {
                return;
            }

            List<Task> completionTasks = new List<Task>();
            foreach (MetricManager manager in metricManagers)
            {
                if (manager != null)
                {
                    Task cycleCompletionTask = manager.StopDefaultAggregationCycleAsync();
                    completionTasks.Add(cycleCompletionTask);
                }
            }
            

            if (WaitForDefaultAggregationCycleCompletion)
            {
#pragma warning disable CS0162 // Unreachable code detected
                Task.WhenAll(completionTasks).GetAwaiter().GetResult();
#pragma warning restore CS0162 // Unreachable code detected
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

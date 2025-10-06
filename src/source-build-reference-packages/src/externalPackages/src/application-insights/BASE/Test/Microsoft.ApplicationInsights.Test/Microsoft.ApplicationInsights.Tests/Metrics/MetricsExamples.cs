using Microsoft.ApplicationInsights.Extensibility;

namespace User.Namespace.Example01
{
    using System;

    using Microsoft.ApplicationInsights;

    using TraceSeveretyLevel = Microsoft.ApplicationInsights.DataContracts.SeverityLevel;

    /// <summary>
    /// Most simple cases are one-liners.
    /// This is all possible without even importing an additional namespace.
    /// </summary>
    public class Sample01
    {
        /// <summary />
        public static void Exec()
        {
            // *** SENDING METRICS ***

            // Recall how you send custom telemetry with Application Insights in other cases, e.g. Events.
            // The following will result in an EventTelemetry object to be sent to the cloud right away.
            using (var config = TelemetryConfiguration.CreateDefault())
            {
                TelemetryClient client = new TelemetryClient(config);
                client.TrackEvent("SomethingInterestingHappened");

                // Metrics work very similar. However, the value is not sent right away.
                // It is aggregated with other values for the same metric, and the resulting summary (aka "aggregate" is sent automatically every minute.
                // To mark this difference, we use a pattern that is similar, but different from the established TrackXxx(..) pattern that sends telemetry right away:
                client.GetMetric("CowsSold").TrackValue(42);

                // *** MEASUREMENTS AND ACCUMULATORS ***

                // We support different kinds of aggregation types. For now, we include two: Measurements and Accumulators.
                // Measurements aggregate tracked values and reduce them to {Count, Sum, Min, Max, StdDev} of all values tracked during each minute. 
                // They are particularly useful if you are measuring something like the number of items sold, the completion time of an operation, or similar.

                // Accumulators are also sent to the cloud each minute.
                // But rather than aggregating values across a time period, they aggregate values across their entire life time (or until you reset them).
                // They are particularly useful when you are counting the number of items in a data structure.

                // By default, metrics are aggregated as Measurements. Here is how you can define a metric to be aggregated as an Accumulator instead:

                // Using the Microsoft.ApplicationInsights.Metrics.Extensions package:
                // Metric itemsInDatastructure = client.GetMetric("ItemsInDatastructure", MetricConfigurations.Common.Accumulator());

                // Using a private implementation:
                Metric itemsInDatastructure = client.GetMetric(
                    "ItemsInDatastructure",
                    new Microsoft.ApplicationInsights.Metrics.MetricConfiguration(
                        1000,
                        100,
                        new Microsoft.ApplicationInsights.Metrics.TestUtility.
                            MetricSeriesConfigurationForTestingAccumulatorBehavior()));

                int itemsAdded = AddItemsToDataStructure();
                itemsInDatastructure.TrackValue(itemsAdded);

                int itemsRemoved = AddItemsToDataStructure();
                itemsInDatastructure.TrackValue(-itemsRemoved);

                // Here is how you can reset an accumulator:
                ResetDataStructure();
                itemsInDatastructure.GetAllSeries()[0].Value.ResetAggregation();

                // *** MULTI-DIMENSIONAL METRICS ***

                // The above example shows a zero-dimensional metric.
                // Metrics can also be multi-dimensional.
                // In the initial version we are supporting up to 2 dimensions, and we will add support for more in the future as needed.
                // Here is an example for a one-dimensional metric:

                Metric animalsSold = client.GetMetric("AnimalsSold", "Species");

                animalsSold.TrackValue(42, "Pigs");
                animalsSold.TrackValue(24, "Horses");

                // The values for Pigs and Horses will be aggregated separately from each other and will result in two distinct aggregates.
                // You can control the maximum number of number data series per metric (and thus your resource usage and cost).
                // The default limits are no more than 1000 total data series per metric, and no more than 100 different values per dimension.
                // We discuss elsewhere how to change them.
                // We use a common .Net pattern: TryXxx(..) to make sure that the limits are observed.
                // If the limits are already reached, Metric.TrackValue(..) will return False and the value will not be tracked. Otherwise it will return True.
                // This is particularly useful if the data for a metric originates from user input, e.g. a file:

                Tuple<int, string> countAndSpecies = ReadSpeciesFromUserInput();
                int count = countAndSpecies.Item1;
                string species = countAndSpecies.Item2;

                if (!animalsSold.TrackValue(count, species))
                {
                    client.TrackTrace(
                        $"Data series or dimension cap was reached for metric {animalsSold.Identifier.MetricId}.",
                        TraceSeveretyLevel.Error);
                }

                // You can inspect a metric object to reason about its current state. For example:
                int currentNumberOfSpecies = animalsSold.GetDimensionValues(1).Count;
            }
        }

        private static void ResetDataStructure()
        {
            // Do stuff
        }

        private static Tuple<int, string> ReadSpeciesFromUserInput()
        {
            return Tuple.Create(18, "Cows");
        }

        private static int AddItemsToDataStructure()
        {
            // Do stuff
            return 5;
        }
    }
}
// ----------- ----------- ----------- ----------- ----------- ----------- ----------- ----------- -----------
namespace User.Namespace.Example02
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Metrics;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TraceSeveretyLevel = Microsoft.ApplicationInsights.DataContracts.SeverityLevel;

    /// <summary>
    /// Importing the <c>Microsoft.ApplicationInsights.Metrics</c> namespace supports some more interesting use cases.
    /// These include:
    ///  - Configuring a metric
    ///  - Working directly with the MetricManager
    ///  
    /// In this example we cover working with MetricSeries.
    /// </summary>
    public class Sample02
    {
        /// <summary />
        public static void Exec()
        {
            // *** ACCESSING METRIC DATA SERIES ***

            // Recall that metrics can be multidimensional. For example, assume that we want to track the number of books sold by Genre and by Language.
            using (var config = TelemetryConfiguration.CreateDefault())
            {
                TelemetryClient client = new TelemetryClient(config);
                Metric booksSold = client.GetMetric("BooksSold", "Genre", "Language");
                booksSold.TrackValue(10, "Science Fiction", "English");
                booksSold.TrackValue(15, "Historic Novels", "English");
                booksSold.TrackValue(20, "Epic Tragedy", "Russian");

                // Recall from the previous example that each of the above TrackValue(..) statements will create a
                // new data series and use it to track the specified value.
                // If you use the same dimension values as before, then instead of creating a new series, the system will look up and use an existing series:

                booksSold.TrackValue(8, "Science Fiction",
                    "English"); // Now we have 18 Science Fiction books in English


                // If you use certain data series frequently you can avoid this lookup by keeping a reference to it:

                MetricSeries epicTragedyInRussianSold;
                booksSold.TryGetDataSeries(out epicTragedyInRussianSold, "Epic Tragedy", "Russian");
                epicTragedyInRussianSold.TrackValue(6); // Now we have 26 Epic Tragedies in Russian
                epicTragedyInRussianSold.TrackValue(5); // Now we have 31 Epic Tragedies in Russian

                // Notice the "Try" in TryGetDataSeries(..). Recall the previous example where we explained the TrackValue(..) pattern.
                // The same reasoning applies here.

                // So Metric is a container of one or more data series.
                // The actual data belongs a specific MetricSeries object and the Metric object is a grouping of one or more series.

                // A zero-dimensional metric has exactly one metric data series:
                Metric cowsSold = client.GetMetric("CowsSold");
                Assert.AreEqual(0, cowsSold.Identifier.DimensionsCount);

                MetricSeries cowsSoldValues;
                cowsSold.TryGetDataSeries(out cowsSoldValues);
                cowsSoldValues.TrackValue(25);

                // For zero-dimensional metrics you can also get the series in a single line:
                MetricSeries cowsSoldValues2 = cowsSold.GetAllSeries()[0].Value;

                cowsSoldValues2.TrackValue(18); // Now we have 43 cows.
                Assert.AreSame(cowsSoldValues, cowsSoldValues2, "The two series references point to the same object");

                // Note, however, that you cannot play this trick with multi-dimensional series, because GetAllSeries() does
                // not provide any guarantees about the ordering of the series it returns.

                // Multi-dimensional metrics can have more than one data series:
                MetricSeries unspecifiedBooksSold, cookbookInGermanSold;
                booksSold.TryGetDataSeries(out unspecifiedBooksSold);
                booksSold.TryGetDataSeries(out cookbookInGermanSold, "Cookbook", "German");

                // You can get the "special" zero-dimensional series from every metric, regardless of now many dimensions it has.
                // But if you specify any dimension values at all, you must specify the correct number, otherwise an exception is thrown.

                try
                {
                    MetricSeries epicTragediesSold;
                    booksSold.TryGetDataSeries(out epicTragediesSold, "Epic Tragedy");
                }
                catch (ArgumentException)
                {
                    client.TrackTrace(
                        $"This error will always happen because '{nameof(booksSold)}' has 2 dimensions, but we only specified one.",
                        TraceSeveretyLevel.Error);
                }

                // The main purpose of keeping a reference to a metric data series is to use it directly for tracking data.
                // It can improve the performance of your application, especially if you are tracking values to this series very frequently,
                // as it avoids the lookups necessary to first get the metric and then the series within the metric.

                // *** SPECIAL DIMENSION NAMES ***

                // Note that metrics do not usually respect the TelemetryContext of the TelemetryClient used to access the metric.
                // There is a detailed discussion of the reasons and workarounds in a latter example. For now, just a clarification:
                TelemetryClient specialClient = new TelemetryClient(config);
                specialClient.Context.Operation.Name = "Special Operation";
                Metric specialOpsRequestSizeStats = specialClient.GetMetric("Special Operation Request Size");
                int requestSize = GetCurrentRequestSize();
                specialOpsRequestSizeStats.TrackValue(requestSize);

                // Metric aggregates sent by the above specialOpsRequestSizeStats-metric will NOT have their Context.Operation.Name set to "Special Operation".

                // However, you can use special dimension names in order to specify TelemetryContext values. For example

                MetricSeries specialOpsRequestSize;
                client.GetMetric("Request Size", "TelemetryContext.Operation.Name")
                    .TryGetDataSeries(out specialOpsRequestSize, "Special Operation");
                specialOpsRequestSize.TrackValue(120000);

                // When the metric aggregate is sent to the Application Insights cloud endpoint, its 'Context.Operation.Name' data field
                // will be set to "Special Operation".
                // Note: the values of this special dimension will be copied into the TelemetryContext and not be used as a 'normal' dimension.
                // If you want to also keep an operation name dimension for normal metric exploration, you need to create a separate dimension
                // for that purpose:

                MetricSeries someOtherOpsRequestSize;
                client.GetMetric("Request Size", MetricDimensionNames.TelemetryContext.Operation.Name,
                        "Operation Name")
                    .TryGetDataSeries(out someOtherOpsRequestSize, "Some Other Operation", "Some Other Operation");
                someOtherOpsRequestSize.TrackValue(64000);

                // In this case, the aggregates of the someOtherOpsRequestSize-series will have a dimension "Operation Name" with the
                // value "Some Other Operation", and, in addition, their Context.Operation.Name will be set to "Some Other Operation".

                // The static class MetricDimensionNames contains a list of constants for all special dimension names.
            }
        }

        private static int GetCurrentRequestSize()
        {
            // Do stuff
            return 11000;
        }
    }
}
// ----------- ----------- ----------- ----------- ----------- ----------- ----------- ----------- -----------
namespace User.Namespace.Example02a
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Metrics;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

    /// <summary>
    /// Importing the <c>Microsoft.ApplicationInsights.Metrics</c> and <c>Microsoft.ApplicationInsights.Metrics.Extensibility</c> namespaces
    /// also supports working with MetricSeries via inspection and modification.
    /// </summary>
    public class Sample02a
    {
        /// <summary />
        public static void Exec()
        {
            using (var config = TelemetryConfiguration.CreateDefault())
            {
                TelemetryClient client = new TelemetryClient(config);

                MetricSeries epicTragedyInRussianSold;

                Metric booksSold = client.GetMetric("BooksSold", "Genre", "Language");
                booksSold.TryGetDataSeries(out epicTragedyInRussianSold, "Epic Tragedy", "Russian");

                // *** WORKING WITH THE EMITTED METRIC DATA ***

                // In addition, there are additional operations that you can perform on a series.
                // Most common of them are designed to support interactive consumption of tracked data.
                // For example, you can reset the values aggregated so far during the current aggregation period to the initial state:

                epicTragedyInRussianSold.TrackValue(42); // Now we have 42 Epic Tragedies in Russian
                epicTragedyInRussianSold.ResetAggregation(); // Now we have 0 Epic Tragedies in Russian

                // For Measurements, resetting will not make a lot of sense in most cases.
                // However, for Accumulators this may be necessary once in a while, for example when you cleared a data structure for
                // which you were counting the contained items.

                // Another powerful example for interacting with aggregated metric data is the ability to inspect the aggregation.
                // This means that your application is not just sending metric telemetry for a later inspection, but is able to use its
                // own metrics to drive its behavior.
                // For example, the following code determines the currently most popular book and displays the information:

                MetricAggregate mostPopularBookKind = null;
                foreach (KeyValuePair<string[], MetricSeries> seriesKvp in booksSold.GetAllSeries())
                {
                    MetricSeries currentBookInfo = seriesKvp.Value;
                    MetricAggregate currentBookKind = currentBookInfo.GetCurrentAggregateUnsafe();

                    if (currentBookKind == null)
                    {
                        continue;
                    }

                    if (mostPopularBookKind == null)
                    {
                        mostPopularBookKind = currentBookKind;
                    }
                    else
                    {
                        double maxSum = mostPopularBookKind.GetDataValue<double>(
                            MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Sum, 0);
                        double currentSum = currentBookKind.GetDataValue<double>(
                            MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Sum, 0);

                        if (maxSum > currentSum)
                        {
                            mostPopularBookKind = currentBookKind;
                        }
                    }
                }

                if (mostPopularBookKind != null)
                {
                    DisplayMostPopularBook(mostPopularBookKind);
                }

                // Notice the "...Unsafe" suffix in the MetricSeries.GetCurrentAggregateUnsafe() method.
                // We added it to underline the need for two important considerations when using this method:
                // a) It may return proper objects and nulls in a poorly predictable way:
                //    For performance reasons, we only create internal aggregators if there is any data to aggregate.
                //    Consider a situation where you tracked some values for a Measurement metric. So GetCurrentAggregateUnsafe()
                //    returns a valid object. At any time, the aggregation period (1 minute) could complete. The aggregate
                //    will be "snapped" and sent to the cloud. Now there is no more aggregate until you track more values during
                //    the ongoing aggregation period.
                // b) Aggregator implementations may choose to optimize their multithreaded performance in a way such that the aggregates
                //    do not always reflect the latest state. Data will be synchronized correctly before being sent to the cloud at the end
                //    of the aggregation period, but it may be lagging behind a few milliseconds at other times or it may be inconsistent.
                //    E.g., following a TrackValue(..) invocation the Count statistic of an aggregate may already be updated, but its Sum
                //    statistic may not yet be updated. These errors are small and not statistically significant. However you should use
                //    unsafe aggregates for what they are - statistical summaries, rather than exact counts.
            }
        }

        private static void DisplayMostPopularBook(MetricAggregate mostPopularBookKind)
        {
            // Do stuff
        }

        private static int AddItemsToDataStructure()
        {
            // Do stuff
            return 3;
        }

        private static string ReadSpeciesFromUserInput()
        {
            // Do stuff
            return "Chicken";
        }
    }
}
// ----------- ----------- ----------- ----------- ----------- ----------- ----------- ----------- -----------
namespace User.Namespace.Example03
{
    using System;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Metrics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TraceSeveretyLevel = Microsoft.ApplicationInsights.DataContracts.SeverityLevel;

    /// <summary>
    /// In this example we cover configuring a metric.
    /// </summary>
    public class Sample03
    {
        /// <summary />
        public static void Exec()
        {
            // *** SEPARATION OF CONCERNS: TRACKING METRICS AND CONFIGURING AGGREGATIONS ARE INDEPENDENT ***

            // Recall from an earlier example that a metric can be configured for aggregation as a Measurement or as an Accumulator.
            // A strong architectural conviction of this Metrics SDK is that metrics tracking and metrics aggregation are distinct concepts
            // that must be kept separate. This means that a metric is ALWAYS tracked in the same way:
            using (var config = TelemetryConfiguration.CreateDefault())
            {
                TelemetryClient client = new TelemetryClient(config);
                Metric anyKindOfMetric = client.GetMetric("...");

                anyKindOfMetric.TrackValue(42);

                // If you want to affect the way a metric is aggregated, you need to do this in the one place where the metric is initialized:

                Metric measurementMetric = client.GetMetric("Items Processed per Minute",
                    MetricConfigurations.Common.Measurement());

                // Using the Microsoft.ApplicationInsights.Metrics.Extensions package:
                // Metric accumulatorMetric = client.GetMetric("Items in a Data Structure", MetricConfigurations.Common.Accumulator());

                // Using a private implementation:
                Metric accumulatorMetric = client.GetMetric(
                    "Items in a Data Structure",
                    new MetricConfiguration(
                        1000,
                        100,
                        new Microsoft.ApplicationInsights.Metrics.TestUtility.
                            MetricSeriesConfigurationForTestingAccumulatorBehavior()));

                measurementMetric.TrackValue(10);
                measurementMetric.TrackValue(20);
                accumulatorMetric.TrackValue(1);
                accumulatorMetric.TrackValue(-1);

                // Note that this is an important and intentional difference to some other metric aggregation libraries
                // that declare a strongly typed metric object class for different aggregators.

                // If you prefer not to cache the metric reference, you can simply avoid specifying the metric configuration in all except the first call.
                // However, you MUST specify a configuration when you initialize the metric for the first time, or we will assume a Measurement.
                // E.g., all three of accumulatorMetric2, accumulatorMetric2a and accumulatorMetric2b below are Accumulators.
                // (In fact, they are all references to the same object.)

                // Using the Microsoft.ApplicationInsights.Metrics.Extensions package:
                //Metric accumulatorMetric2 = client.GetMetric("Items in a Data Structure 2", MetricConfigurations.Common.Accumulator());

                // Using a private implementation:
                Metric accumulatorMetric2 = client.GetMetric(
                    "Items in a Data Structure",
                    new MetricConfiguration(
                        1000,
                        100,
                        new Microsoft.ApplicationInsights.Metrics.TestUtility.
                            MetricSeriesConfigurationForTestingAccumulatorBehavior()));


                Metric accumulatorMetric2a = client.GetMetric("Items in a Data Structure 2");
                Metric accumulatorMetric2b = client.GetMetric("Items in a Data Structure 2", metricConfiguration: null);

                // On contrary, metric3 and metric3a are Measurements, because no configuration was specified during the first call:

                Metric metric3 = client.GetMetric("Metric 3");
                Metric metric3a = client.GetMetric("Metric 3", metricConfiguration: null);

                // Be careful: If you specify inconsistent metric configurations, you will get an exception:

                try
                {
                    Metric accumulatorMetric2c = client.GetMetric("Items in a Data Structure 2",
                        MetricConfigurations.Common.Measurement());
                }
                catch (ArgumentException)
                {
                    client.TrackTrace(
                        "A Metric with the specified Id and dimension names already exists, but it has a configuration"
                        + " that is different from the specified configuration. You may not change configurations once a"
                        + " metric was created for the first time. Either specify the same configuration every time, or"
                        + " specify 'null' during every invocation except the first one. 'Null' will match against any"
                        + " previously specified configuration when retrieving existing metrics, or fall back to"
                        + " MetricConfigurations.Common.Measurement() when creating new metrics.",
                        TraceSeveretyLevel.Error);
                }

                // *** CUSTOM METRIC CONFIGURATIONS ***

                // Above we have seen two fixed presets for metric configurations: MetricConfigurations.Common.Measurement() and MetricConfigurations.Common.Accumulator().
                // Both are static objects of class MetricConfiguration.
                // You can provide your own implementations of IMetricSeriesConfiguration which is used by MetricConfiguration if you
                // want to implement your own custom aggregators; that is covered elsewhere.
                // Here, let's focus on creating your own instances of MetricConfiguration to configure more options.
                // MetricConfiguration ctor takes some options on how to manage different series within the respective metric and an
                // object of class MetricSeriesConfigurationForMeasurement : IMetricSeriesConfiguration that specifies aggregation behavior for
                // each individual series of the metric:

                Metric customConfiguredMeasurement = client.GetMetric(
                    "Custom Metric 1",
                    new MetricConfiguration(
                        seriesCountLimit: 1000,
                        valuesPerDimensionLimit: 100,
                        seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false)));

                // seriesCountLimit is the max total number of series the metric can contain before TrackValue(..) and TryGetDataSeries(..) stop
                // creating new data series and start returning false.
                // valuesPerDimensionLimit limits the number of distinct values per dimension in a similar manner.
                // usePersistentAggregation specifies whether the aggregator for each time series will be replaced at the end of each aggregation cycle (false)
                // or not (true). This corresponds to the Measurement and the Accumulator aggregations respectively.
                // restrictToUInt32Values can be used to force a metric to consume non-negtive integer values only. Certain ono-negative-integer-only
                // auto-collected system metrics are stored in the cloud in an optimized, more efficient manner. Custom metrics are currently always
                // stored as doubles.

                // In fact, the above customConfiguredMeasurement is how MetricConfigurations.Common.Measurement() is defined by default.

                // If you want to change some of the above configuration values for all metrics in your application without the need to specify 
                // a custom configuration every time, you can do so by using the MetricConfigurations.Common.SetDefaultForXxx(..) methods.
                // Note that this will only affect metrics created after the change:

                Metric someMeasurement1 =
                    client.GetMetric("Some Measurement 1", MetricConfigurations.Common.Measurement());

                MetricConfigurations.Common.SetDefaultForMeasurement(
                    new MetricConfigurationForMeasurement(
                        seriesCountLimit: 10000,
                        valuesPerDimensionLimit: 5000,
                        seriesConfig: new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false)));

                Metric someMeasurement2 =
                    client.GetMetric("Some Measurement 2", MetricConfigurations.Common.Measurement());

                // someMeasurement1 has SeriesCountLimit = 1000 and ValuesPerDimensionLimit = 100.
                // someMeasurement2 has SeriesCountLimit = 10000 and ValuesPerDimensionLimit = 5000.

                try
                {
                    Metric someMeasurement1a =
                        client.GetMetric("Some Measurement 1", MetricConfigurations.Common.Measurement());
                }
                catch (ArgumentException)
                {
                    // This exception will always occur because the configuration object behind MetricConfigurations.Common.Measurement()
                    // has changed when MetricConfigurations.FutureDefaults when was modified.
                }
            }
        }
    }
}
// ----------- ----------- ----------- ----------- ----------- ----------- ----------- ----------- -----------
namespace User.Namespace.Example04
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.Metrics;
    using Microsoft.ApplicationInsights.Extensibility;
    
    /// <summary>
    /// In this example we cover working directly with the MetricManager.
    /// </summary>
    public class Sample04
    {
        /// <summary />
        public static void Exec()
        {
            // *** MANUALLY CREATING METRIC SERIES WITHOUT THE CONTEXT OF A METRIC ***

            // In previous examples we learned that a Metric is merely a grouping of one or more MetricSeries, and the actual tracking
            // is performed by the respective MetricSeries.
            // MetricSeries are managed by a class called MetricManager. The MetricManager creates all MetricSeries objects that
            // share a scope, and encapsulates the corresponding aggregation cycles. The default aggregation cycle takes care of
            // sending metrics to the cloud at regular intervals (1 minute). For that, it uses a dedicated managed background thread.
            // This model is aimed at ensuring that metrics are sent regularly even in case of thread pool starvation. However, it can
            // cost significant resources when creating too many custom metric managers (this advanced situation is discussed later).

            // The default scope for a MetricManager is an instance of the Application Insights telemetry pipeline.
            // Other scopes are discussed in later examples.
            // Recall that although in some special circumstances users can create many instances of the Application Insights telemetry
            // pipeline, the normal case is that there is single default pipeline per application, accessible via the static object
            // at TelemetryConfiguration.Active.

            // Expert users can choose to manage their metric series directly, rather than using a Metric container object.
            // In that case they will obtain metric series directly from the MetricManager:

            using (var config = TelemetryConfiguration.CreateDefault())
            {
                MetricManager metrics = config.GetMetricManager();

                MetricSeries requestSize = metrics.CreateNewSeries(
                    "Example Metrics",
                    "Size of Service Resquests",
                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

                requestSize.TrackValue(256);
                requestSize.TrackValue(314);
                requestSize.TrackValue(189);

                // Note that MetricManager.CreateNewSeries(..) will ALWAYS create a new metric series. It is your responsibility to keep a reference
                // to it so that you can access it later. If you do not want to worry about keeping that reference, just use Metric.

                // If you choose to useMetricManager directly, you can specify the dimension names and values associated with a new metric series.
                // Note how dimensions can be specified as a dictionary or as an array. On contrary to the Metric class APIs, this approach does not
                // take care of series capping and dimension capping. You need to take care of it yourself.

                MetricSeries purpleCowsSold = metrics.CreateNewSeries(
                    "Example Metrics",
                    "Animals Sold",
                    new Dictionary<string, string>() {["Species"] = "Cows", ["Color"] = "Purple"},
                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

                MetricSeries yellowHorsesSold = metrics.CreateNewSeries(
                    "Example Metrics",
                    "Animals Sold",
                    new[]
                    {
                        new KeyValuePair<string, string>("Species", "Horses"),
                        new KeyValuePair<string, string>("Color", "Yellow")
                    },
                    new MetricSeriesConfigurationForMeasurement(restrictToUInt32Values: false));

                purpleCowsSold.TrackValue(42);
                yellowHorsesSold.TrackValue(132);

                // *** FLUSHING ***

                // MetricManager also allows you to flush all your metric aggregators and send the current aggregates to the cloud without waiting
                // for the end of the ongoing aggregation period:

                config.GetMetricManager().Flush();
            }
        }
    }
}
// ----------- ----------- ----------- ----------- ----------- ----------- ----------- ----------- -----------
namespace User.Namespace.Example05
{
    using System;

    using Microsoft.ApplicationInsights;

    /// <summary>
    /// In this example we cover AggregationScope.
    /// </summary>
    public class Sample05
    {
        /// <summary />
        public static void Exec()
        {
            // *** AGGREGATION SCOPE ***

            // Previously we saw that metrics do not use the Telemetry Context of the Telemetry Client used to access them.
            // We learned that using special dimension names available as constants in MetricDimensionNames is the best workaround for this limitation.
            // Here, we discuss the reasons for the limitation and other possible workarounds.

            // Recall the problem description:
            // Metric aggregates sent by the below "Special Operation Request Size"-metric will NOT have their Context.Operation.Name set to "Special Operation".
            using (var config = TelemetryConfiguration.CreateDefault())
            {
                TelemetryClient specialClient = new TelemetryClient(config);
                specialClient.Context.Operation.Name = "Special Operation";
                specialClient.GetMetric("Special Operation Request Size").TrackValue(GetCurrentRequestSize());

                // The reason for that is that by default, metrics are aggregated at the scope of the TelemetryConfiguration pipeline and not at the scope
                // of a particular TelemetryClient. This is because of a very common pattern for Application Insights users where a TelemetryClient is created
                // for a small scope. For example:

                {
                    // ...
                    (new TelemetryClient(config)).TrackEvent("Something Interesting Happened");
                    // ...
                }

                {
                    try
                    {
                        RunSomeCode();
                    }
                    catch (Exception apEx)
                    {
                        (new TelemetryClient(config)).TrackException(apEx);
                    }
                }

                // ...and so on.
                // We wanted to support this pattern and to allow users to write code like this:

                {
                    // ...
                    (new TelemetryClient(config)).GetMetric("Temperature").TrackValue(36.6);
                    // ---
                }

                {
                    // ...
                    (new TelemetryClient(config)).GetMetric("Temperature").TrackValue(39.1);
                    // ---
                }

                // In this case the expected behavior is that these values are aggregated together into a single aggregate with Count = 2, Sum = 75.7 and so on.
                // In order to achieve that, we use a single MetricManager to create all the respective metric series. This manager is attached to the
                // TelemetryConfiguration that stands behind a TelemetryClient. This ensures that the two (new TelemetryClient()).GetMetric("Temperature") statements
                // above return the same Metric object.
                // However, if different TelemetryClient instances return the name Metric instance, then what client's Context should the Metric respect?
                // To avoid confusion, it respects none.

                // The best workaround for this circumstance was mentioned in a previous example - use the special dimension names in the MetricDimensionNames class.
                // However, sometimes it is inconvenient. For example, if you already created a cached TelemetryClient for a specific scope and set some custom 
                // Context properties. 
                // It is actually possible to create a metric that is only scoped to a single TelemetryClient instance. This will cause the creation of a special
                // MetricManager instance at the scope of that one TelemetryClient. We highly recommend using this feature with restraint, as a MetricManager can
                // use a non-trivial amount of resources, including separate aggregators for each metric series and a managed thread for sending aggregated telemetry.
                // Here how this works:

                TelemetryClient operationClient = new TelemetryClient(config);
                operationClient.Context.Operation.Name =
                    "Operation XYZ"; // This client will only send telemetry related to a specific operation.
                operationClient.InstrumentationKey =
                    "05B5093A-F137-4A68-B826-A950CB68C68F"; // This client sends telemetry to a special Application Insights component.

                Metric operationRequestSize = operationClient.GetMetric("XYZ Request Size",
                    MetricConfigurations.Common.Measurement(), MetricAggregationScope.TelemetryClient);

                int requestSize = GetCurrentRequestSize();
                operationRequestSize.TrackValue(306000);

                // Note the last parameter to GetMetric: MetricAggregationScope.TelemetryClient. This instructed the GetMetric API not to use the metric
                // manager at the TelemetryConfiguration scope, but to create and use a metric manager at the respective client's scope instead.
            }
        }

        private static void RunSomeCode()
        {
            throw new Exception();
        }

        private static int GetCurrentRequestSize()
        {
            // Do stuff
            return 11000;
        }
    }
}
// ----------- ----------- ----------- ----------- ----------- ----------- ----------- ----------- -----------
namespace User.Namespace.Example06ab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Metrics;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TraceSeveretyLevel = Microsoft.ApplicationInsights.DataContracts.SeverityLevel;
    
    /// <summary>
    /// In this example we discuss how to write unit tests that validate that metrics are sent correctly.
    /// We will consider three approaches:
    ///  a) Capturing all telemetry emitted by a method, including, but not limited to Metric Telemetry,
    ///     where a telemetry client can be injected.
    ///  b) Capturing all telemetry emitted by a method, including, but not limited to Metric Telemetry,
    ///     where the (new TelemetryClient()).TrackXxx(..) pattern in used in-line.
    ///  c) Capturing metric aggregates only using a custom aggregation cycle (see next example).
    /// </summary>
    public class Sample06ab
    {
        /// <summary />
        public static void ExecA()
        {
            // *** UNIT TESTS: CAPTURING APPLICATION INSIGHTS TELEMETRY BY INJECTING A TELEMETRY CLIENT ***

            // Here we will use a common unit test to capture and verify all Application Insights telemetry emitted by a
            // method SellPurpleDucks() of class ServiceClassA. We also assume that the class has been prepared for testing by allowing
            // to specify a telemetry client using dependency injection. The code for the class is listed below.

            // In a production application the class will probably be instantiated and called like this:
            using (var config = TelemetryConfiguration.CreateDefault())
            {
                ServiceClassA serviceA = new ServiceClassA(new TelemetryClient(config));
                serviceA.SellPurpleDucks(42);
            }

            // In a unit test you will need to create a custom telemetry configuration that routs the emitted telemetry into a
            // data structure for later inspection. There is a TestUtil class below that shows how to do that. Here is the unit test:

            {
                // Create the test pipeline and client:
                IList<ITelemetry> telemetrySentToChannel;
                TelemetryConfiguration telemetryPipeline = TestUtil.CreateApplicationInsightsTelemetryConfiguration(out telemetrySentToChannel);
                using (telemetryPipeline)
                { 
                    TelemetryClient telemetryClient = new TelemetryClient(telemetryPipeline);

                    // Invoke method being tested:
                    ServiceClassA serviceA = new ServiceClassA(telemetryClient);
                    serviceA.SellPurpleDucks(42);

                    // Make sure all telemetry is collected:
                    telemetryClient.Flush();

                    // Flushing the MetricManager is particularly important since the aggregation period of 1 minute has just started:
                    telemetryPipeline.GetMetricManager().Flush();

                    // Verify that the right telemetry was sent:
                    Assert.AreEqual(2, telemetrySentToChannel.Count);

                    TraceTelemetry[] traceItems = telemetrySentToChannel.Where( (t) => ((t != null) && (t is TraceTelemetry)) )
                                                                        .Select( (t) => ((TraceTelemetry) t) )
                                                                        .ToArray();
                    Assert.AreEqual(1, traceItems.Length);
                    Assert.AreEqual("Stuff #1 completed", traceItems[0].Message);
                    Assert.AreEqual(TraceSeveretyLevel.Information, traceItems[0].SeverityLevel);

                    MetricTelemetry[] metricItems = telemetrySentToChannel.Where( (t) => ((t != null) && (t is MetricTelemetry)) )
                                                                          .Select( (t) => ((MetricTelemetry) t) )
                                                                          .ToArray();
                    Assert.AreEqual(1, metricItems.Length);
                    Assert.AreEqual("Ducks Sold", metricItems[0].Name);
                    Assert.AreEqual(1, metricItems[0].Count);
                    Assert.AreEqual(42, metricItems[0].Sum);
                    Assert.AreEqual(42, metricItems[0].Min);
                    Assert.AreEqual(42, metricItems[0].Max);
                    Assert.AreEqual(0, metricItems[0].StandardDeviation);
                    Assert.AreEqual(2, metricItems[0].Properties.Count);
                    Assert.IsTrue(metricItems[0].Properties.ContainsKey("_MS.AggregationIntervalMs"));
                    Assert.IsTrue(metricItems[0].Properties.ContainsKey("Color"));
                    Assert.AreEqual("Purple", metricItems[0].Properties["Color"]);

                    // Note that this test requires understanding how metric dimensions and other information such as aggregation period will
                    // be serialized into the Properties of the MetricTelemetry item. We will see how to avoid diving into these low level details
                    // when we see how to unit test by capturing the metric aggregates directly using a custom aggregation cycle.
                }
            }
        }

        /// <summary />
        public static void ExecB()
        {
            // *** UNIT TESTS: CAPTURING APPLICATION INSIGHTS TELEMETRY BY SUBSTITUTING THE TELEMETRY CHANNEL ***

            // Previously we used dependency injection to provide a custom telemetry client to test a method.

            // Consider now a slightly modified class ServiceClassB that does not expect a custom telemetry client.
            // We can test it by substituting the channel used in the default telemetry pipeline. 

            // In a production application the class will probably be instantiated and called like this:

            using (var config = TelemetryConfiguration.CreateDefault())
            {
                {
                    ServiceClassB serviceB = new ServiceClassB(config);
                    serviceB.SellPurpleDucks(42);
                }
                // Here is the unit test:

                // Do not forget to set the InstrumentationKey to some value, otherwise the pipeline will not send any telemetry to the channel.
                config.InstrumentationKey = Guid.NewGuid().ToString("D");

                // This approach is more widely applicable, and does not require to prepare your code for injection of a telemetry client.
                // However, a significant drawback is that in this model different unit tests can interfere with each other via the static default
                // telemetry pipeline. Such interference may be non-trivial. E.g., for this simple test, we need to flush out all the tracked values
                // from the code that just run. This will flush out all Measurements, but not Accumulators, since they persist between flushes.
                // This can make unit testing with this method quite complex.
                config.GetMetricManager().Flush();
                (new TelemetryClient(config)).Flush();

                // Create the test pipeline and client.
                StubTelemetryChannel telemetryCollector = new StubTelemetryChannel();
                config.TelemetryChannel = telemetryCollector;
                config.InstrumentationKey = Guid.NewGuid().ToString("D");

                // Invoke method being tested:
                {
                    ServiceClassB serviceB = new ServiceClassB(config);
                    serviceB.SellPurpleDucks(42);
                }
                // Flushing the MetricManager is particularly important since the aggregation period of 1 minute has just started:
                config.GetMetricManager().Flush();

                // As mentioned, tests using this approach interfere with each other.
                // For example, when running all the examples here after each other, accumulators from previous examples are still associated with the
                // metric manager at TelemetryConfiguration.Active.GetMetricManager(). Luckily, all their names begin with "Items", so we can filter them out.

                ITelemetry[] telemetryFromThisTest = telemetryCollector.TelemetryItems
                                                                       .Where( (t) => !((t is MetricTelemetry) && ((MetricTelemetry) t).Name.StartsWith("Items")) )
                                                                       .ToArray();

                // Verify that the right telemetry was sent:

                Assert.AreEqual(2, telemetryFromThisTest.Length);

                TraceTelemetry[] traceItems = telemetryFromThisTest.Where( (t) => ((t != null) && (t is TraceTelemetry)) )
                                                                   .Select( (t) => ((TraceTelemetry) t) )
                                                                   .ToArray();
                Assert.AreEqual(1, traceItems.Length);
                Assert.AreEqual("Stuff #1 completed", traceItems[0].Message);
                Assert.AreEqual(TraceSeveretyLevel.Information, traceItems[0].SeverityLevel);

                MetricTelemetry[] metricItems = telemetryFromThisTest.Where( (t) => ((t != null) && (t is MetricTelemetry)) )
                                                                     .Select( (t) => ((MetricTelemetry) t) )
                                                                     .ToArray();
                Assert.AreEqual(1, metricItems.Length);
                Assert.AreEqual("Ducks Sold", metricItems[0].Name);
                Assert.AreEqual(1, metricItems[0].Count);
                Assert.AreEqual(42, metricItems[0].Sum);
                Assert.AreEqual(42, metricItems[0].Min);
                Assert.AreEqual(42, metricItems[0].Max);
                Assert.AreEqual(0, metricItems[0].StandardDeviation);
                Assert.AreEqual(2, metricItems[0].Properties.Count);
                Assert.IsTrue(metricItems[0].Properties.ContainsKey("_MS.AggregationIntervalMs"));
                Assert.IsTrue(metricItems[0].Properties.ContainsKey("Color"));
                Assert.AreEqual("Purple", metricItems[0].Properties["Color"]);

                // Note that this test requires understanding how metric dimensions and other information such as aggregation period will
                // be serialized into the Properties of the MetricTelemetry item. We will see how to avoid diving into these low level details
                // when we see how to unit test by capturing the metric aggregates directly.
            }
        }
    }

    internal class ServiceClassA
    {
        private TelemetryClient _telemetryClient = null;

        public ServiceClassA(TelemetryClient telemetryClient)
        {
            if (telemetryClient == null)
            {
                throw new ArgumentNullException(nameof(telemetryClient));
            }

            _telemetryClient = telemetryClient;
        }

        public void SellPurpleDucks(int count)
        {
            // Do some stuff #1...
            _telemetryClient.TrackTrace("Stuff #1 completed", TraceSeveretyLevel.Information);

            // Do more stuff...
            _telemetryClient.GetMetric("Ducks Sold", "Color").TrackValue(count, "Purple");
        }
    }

    internal class ServiceClassB
    {
        private readonly TelemetryConfiguration configuration;
        public ServiceClassB(TelemetryConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SellPurpleDucks(int count)
        {
            // Do some stuff #1...
            (new TelemetryClient(configuration)).TrackTrace("Stuff #1 completed", TraceSeveretyLevel.Information);

            // Do more stuff...
            (new TelemetryClient(configuration)).GetMetric("Ducks Sold", "Color").TrackValue(count, "Purple");
        }
    }

    internal class TestUtil
    {
        public static TelemetryConfiguration CreateApplicationInsightsTelemetryConfiguration(out IList<ITelemetry> telemetrySentToChannel)
        {
            StubTelemetryChannel channel = new StubTelemetryChannel();
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
    }

    internal class StubTelemetryChannel : ITelemetryChannel
    {
        public StubTelemetryChannel()
        {
            TelemetryItems = new List<ITelemetry>();
        }

        public bool? DeveloperMode { get; set; }

        public string EndpointAddress { get; set; }

        public IList<ITelemetry> TelemetryItems { get; }

        public void Send(ITelemetry item)
        {
            TelemetryItems.Add(item);
        }

        public void Dispose()
        {
        }

        public void Flush()
        {
        }
    }
}
// ----------- ----------- ----------- ----------- ----------- ----------- ----------- ----------- -----------
namespace User.Namespace.Example06c
{
    using System;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Metrics;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TraceSeverityLevel = Microsoft.ApplicationInsights.DataContracts.SeverityLevel;
    
    /// <summary>
    /// In this example we discuss how to write unit tests that validate that metrics are sent correctly
    /// We will consider an advanced unit test approach:
    ///  c) Capturing metric aggregates only.
    /// </summary>
    public class Sample06c
    {
        /// <summary />
        public static void ExecC()
        {
            // *** UNIT TESTS: CAPTURING METRIC AGGREGATES ***

            // Previously described test approaches intercept all application insights telemetry.
            // There are some drawbacks to such tests.
            // Most of these drawbacks represent edge cases, but they can be critical if and when they occur:
            //  - Tests using channel substitution (model b) can interfere with each other (see above), making testing overly complex.
            //  - Since all telemetry is intercepted, such testing is suitable for unit tests only, but not for some integration or
            //    production test scenarios were telemetry needs to be actually sent to the cloud.
            //  - It is not applicable in the advanced scenarios where metric aggregates are sent to a consumer other than the
            //    Application Insights cloud endpoint.
            //  - It requires understanding how MetricAggregates are serialized to MetricTelemetry items. Such serialization may
            //    change over time if new metric aggregation kinds are supported by the processing backend.
            // Here, we use a custom aggregation cycle to bypass this limitation.

            // Previously we mentioned that a MetricManager encapsulates a managed thread that drives the default aggregation cycle and sends
            // metric aggregates to the cloud every minute.
            // In fact, there are 3 aggregation cycles. Beyond the default cycle, there is a custom cycle and an additional cycle dedicated
            // specifically for QuickPulse/LiveMetrics integration. Users should not me using the QuickPulse cycle for their purposes.
            // Here we discuss the custom aggregation cycle.
            // Aggregation cycles other than Default do not add additional threads.
            // Instead, they track values into additional aggregators and allow users to pull data when desired. Thus, users have full control
            // over timing issues.

            // In the context of testing, users can use "virtual time", i.e. they can specify any timestamps in a test that
            // runs only for milliseconds, thus testing various timing scenarios.
            using (var config = TelemetryConfiguration.CreateDefault())
            {
                DateTimeOffset testStartTime = new DateTimeOffset(2017, 11, 1, 13, 0, 0, TimeSpan.FromHours(8));

                // In order to use custom aggregation cycles and other advanced metrics features, import the following namespace:
                // using Microsoft.ApplicationInsights.Metrics.Extensibility;

                // By default all non-default aggregation cycles are inactive. To activate the custom cycle, request the custom cycle aggregates:

                MetricManager defaultMetricManager = config.GetMetricManager();
                AggregationPeriodSummary lastCycle = defaultMetricManager.StartOrCycleAggregators(
                    MetricAggregationCycleKind.Custom,
                    testStartTime,
                    ExcludeAccumulatorsFromPreviousTestsFilter.Instance);

                // If the cycle was inactive so far, it will be started up and aggregation into the cycle will begin. Other cycles will be unaffected.
                // Since this was the first invocation, the received AggregationPeriodSummary is empty:

                Assert.AreEqual(0, lastCycle.NonpersistentAggregates.Count);

                // Now we can call the method being tested.

                ServiceClassC serviceC = new ServiceClassC(config);
                serviceC.SellPurpleDucks(42);

                // Now we can pull the data again. Let us pretend that 1 full "virtual" minute has passed:

                lastCycle = defaultMetricManager.StartOrCycleAggregators(
                    MetricAggregationCycleKind.Custom,
                    testStartTime.AddMinutes(1),
                    ExcludeAccumulatorsFromPreviousTestsFilter.Instance);

                // Now we can verify that metrics were tracked correctly:

                Assert.AreEqual(1, lastCycle.NonpersistentAggregates.Count, "One Measurement should be tracked");
                Assert.AreEqual(0, lastCycle.PersistentAggregates.Count, "No Accumulators should be tracked");

                Assert.AreEqual("Ducks Sold", lastCycle.NonpersistentAggregates[0].MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement().Constants().AggregateKindMoniker,
                    lastCycle.NonpersistentAggregates[0].AggregationKindMoniker);
                Assert.AreEqual(testStartTime, lastCycle.NonpersistentAggregates[0].AggregationPeriodStart);
                Assert.AreEqual(TimeSpan.FromMinutes(1),
                    lastCycle.NonpersistentAggregates[0].AggregationPeriodDuration);
                Assert.AreEqual(1, lastCycle.NonpersistentAggregates[0].Dimensions.Count);
                Assert.AreEqual("Purple", lastCycle.NonpersistentAggregates[0].Dimensions["Color"]);
                Assert.AreEqual(1,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<int>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Count, -1));
                Assert.AreEqual(42,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<double>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Sum, -1));
                Assert.AreEqual(42,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<double>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Min, -1));
                Assert.AreEqual(42,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<double>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Max, -1));
                Assert.AreEqual(0,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<double>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.StdDev, -1));

                // Note that because "Ducks Sold" is a Measurement, and because we cycled the custom aggregators, the current aggregator is now empty.
                // However, if it was an Accumulator, it would keep the values tracked thus far. To help differentiate between these two cases, Measurement-like
                // aggregates are contained within AggregationPeriodSummary.NonpersistentAggregates and  Accumulator-like aggregates are contained within
                // AggregationPeriodSummary.PersistentAggregates.

                // Let's call the tested API again, now twice:

                serviceC.SellPurpleDucks(11);
                serviceC.SellPurpleDucks(12);

                // Since we are now done, we will gracefully shut down the custom aggregation cycle. We will receive the last aggregates:

                lastCycle = defaultMetricManager.StopAggregators(MetricAggregationCycleKind.Custom,
                    testStartTime.AddMinutes(2));

                Assert.AreEqual(1, lastCycle.NonpersistentAggregates.Count,
                    "One Measurement should be tracked (with two values)");
                Assert.AreEqual(0, lastCycle.PersistentAggregates.Count, "No Accumulators should be tracked");

                Assert.AreEqual("Ducks Sold", lastCycle.NonpersistentAggregates[0].MetricId);
                Assert.AreEqual(MetricConfigurations.Common.Measurement().Constants().AggregateKindMoniker,
                    lastCycle.NonpersistentAggregates[0].AggregationKindMoniker);
                Assert.AreEqual(testStartTime.AddMinutes(1),
                    lastCycle.NonpersistentAggregates[0].AggregationPeriodStart);
                Assert.AreEqual(TimeSpan.FromMinutes(1),
                    lastCycle.NonpersistentAggregates[0].AggregationPeriodDuration);
                Assert.AreEqual(1, lastCycle.NonpersistentAggregates[0].Dimensions.Count);
                Assert.AreEqual("Purple", lastCycle.NonpersistentAggregates[0].Dimensions["Color"]);
                Assert.AreEqual(2,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<int>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Count, -1));
                Assert.AreEqual(23,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<double>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Sum, -1));
                Assert.AreEqual(11,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<double>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Min, -1));
                Assert.AreEqual(12,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<double>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.Max, -1));
                Assert.AreEqual(0.5,
                    lastCycle.NonpersistentAggregates[0].GetDataValue<double>(
                        MetricConfigurations.Common.Measurement().Constants().AggregateKindDataKeys.StdDev, -1));
            }
        }
    }

    internal class ServiceClassC
    {
        private readonly TelemetryConfiguration configuration;
        public ServiceClassC(TelemetryConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SellPurpleDucks(int count)
        {
            // Do some stuff #1...
            (new TelemetryClient(configuration)).TrackTrace("Stuff #1 completed", TraceSeverityLevel.Information);

            // Do more stuff...
            (new TelemetryClient(configuration)).GetMetric("Ducks Sold", "Color").TrackValue(count, "Purple");
        }
    }

    internal class ExcludeAccumulatorsFromPreviousTestsFilter : IMetricSeriesFilter
    {
        public static readonly ExcludeAccumulatorsFromPreviousTestsFilter Instance = new ExcludeAccumulatorsFromPreviousTestsFilter();

        public bool WillConsume(MetricSeries dataSeries, out IMetricValueFilter valueFilter)
        {
            valueFilter = null;
            return !dataSeries.MetricIdentifier.MetricId.StartsWith("Items");
        }
    }

}
// ----------- ----------- ----------- ----------- ----------- ----------- ----------- ----------- -----------
namespace Microsoft.ApplicationInsights.Metrics.Examples
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class runs all examples.
    /// </summary>
    [TestClass]
    public class RunAllMetricsExamples
    {
        /// <summary />
        [TestMethod]
        public void Example01()
        {
            User.Namespace.Example01.Sample01.Exec();
        }

        /// <summary />
        [TestMethod]
        public void Example02()
        {
            User.Namespace.Example02.Sample02.Exec();
            User.Namespace.Example02a.Sample02a.Exec();
        }

        /// <summary />
        [TestMethod]
        public void Example03()
        {
            User.Namespace.Example03.Sample03.Exec();
        }

        /// <summary />
        [TestMethod]
        public void Example04()
        {
            User.Namespace.Example04.Sample04.Exec();
        }

        /// <summary />
        [TestMethod]
        public void Example05()
        {
            User.Namespace.Example05.Sample05.Exec();
        }

        /// <summary />
        [TestMethod]
        public void Example06()
        {
            User.Namespace.Example06ab.Sample06ab.ExecA();
            User.Namespace.Example06ab.Sample06ab.ExecB();
            User.Namespace.Example06c.Sample06c.ExecC();
        }
    }
}

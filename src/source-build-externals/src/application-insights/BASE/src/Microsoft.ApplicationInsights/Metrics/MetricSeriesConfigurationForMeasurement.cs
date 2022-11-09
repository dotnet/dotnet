namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Microsoft.ApplicationInsights.Metrics.Extensibility;

#pragma warning disable CA1034 // "Do not nest type" - part of the public API and too late to change.

    /// <summary>Abstracts the configuration for a metric series aggregated using the "measurement" aggregation kind.
    /// A mear=surement is best suited for metrics describing sizes or durations.
    /// It contains the Min, Max, Sum and Count of values tracked during an aggregation period.</summary>
    public class MetricSeriesConfigurationForMeasurement : IMetricSeriesConfiguration
    {
        private readonly bool restrictToUInt32Values;
        private readonly int hashCode;

        static MetricSeriesConfigurationForMeasurement()
        {
            MetricAggregateToTelemetryPipelineConverters.Registry.Add(
                                                                    typeof(ApplicationInsightsTelemetryPipeline),
                                                                    Constants.AggregateKindMoniker,
                                                                    new MeasurementAggregateToApplicationInsightsPipelineConverter());
        }

        /// <summary>CReates a new configuration.</summary>
        /// <param name="restrictToUInt32Values">Whether only integer numbers should be tracked (used for some integer-optimized backends).</param>
        public MetricSeriesConfigurationForMeasurement(bool restrictToUInt32Values)
        {
            this.restrictToUInt32Values = restrictToUInt32Values;

            this.hashCode = Util.CombineHashCodes(this.restrictToUInt32Values.GetHashCode());
        }

        /// <summary>Gets a value indicating whether the aggregation kind used by this configuration keeps state across aggregation cycles.
        /// FOr measurements - always returns <c>false</c>.</summary>
        public bool RequiresPersistentAggregation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return false; }
        }

        /// <summary>Gets a value indicating whether only integer numbers should be tracked
        /// (used for some integer-optimized backends).</summary>
        public bool RestrictToUInt32Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return this.restrictToUInt32Values; }
        }

        /// <summary>Creates a new aggregator capable of aggregating according to this configurations.</summary>
        /// <param name="dataSeries">Metric data tie series to be aggregated.</param>
        /// <param name="aggregationCycleKind">The kind of th aggregtion cycle.</param>
        /// <returns>A new metric aggregator.</returns>
        public IMetricSeriesAggregator CreateNewAggregator(MetricSeries dataSeries, MetricAggregationCycleKind aggregationCycleKind)
        {
            IMetricSeriesAggregator aggregator = new MeasurementAggregator(this, dataSeries, aggregationCycleKind);
            return aggregator;
        }

        /// <summary>Checks whether this configuration is semantically equat to a specified configuration.</summary>
        /// <param name="obj">Some objects.</param>
        /// <returns><c>true</c> if the specified object is a configutation that is semantically equal to this configuration;
        /// <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                if (obj is MetricSeriesConfigurationForMeasurement otherConfig)
                {
                    return this.Equals(otherConfig);
                }
            }

            return false;
        }

        /// <summary>Checks whether this configuration is semantically equat to a specified configuration.</summary>
        /// <param name="other">Some configuration objects.</param>
        /// <returns><c>true</c> if the specified object is a configutation that is semantically equal to this configuration;
        /// <c>false</c> otherwise.</returns>
        public bool Equals(IMetricSeriesConfiguration other)
        {
            return this.Equals((object)other);
        }

        /// <summary>Checks whether this configuration is semantically equat to a specified configuration.</summary>
        /// <param name="other">Some configuration objects.</param>
        /// <returns><c>true</c> if the specified object is a configutation that is semantically equal to this configuration;
        /// <c>false</c> otherwise.</returns>
        public bool Equals(MetricSeriesConfigurationForMeasurement other)
        {
            if (other == null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.RestrictToUInt32Values == other.RestrictToUInt32Values;
        }

        /// <summary>Gets a hash code for this configuration.</summary>
        /// <returns>A hash code for this configuration.</returns>
        public override int GetHashCode()
        {
            return this.hashCode;
        }

        /// <summary>
        /// Groups constants used by metric aggregates produced by aggregators that are configured by metric configurations represented through
        /// instances of <see cref="MetricSeriesConfigurationForMeasurement"/>. This class cannot be instantiated. To access the constants, use the 
        /// extension method <c>MetricConfigurations.Common.Measurement().Constants()</c> or <see cref="MetricSeriesConfigurationForMeasurement.Constants"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed class AggregateKindConstants
        {
#pragma warning disable CA1822 // "Member does not access instance data and can be marked as static
            internal static readonly AggregateKindConstants Instance = new AggregateKindConstants();

            private AggregateKindConstants()
            {
            }

            /// <summary>
            /// Gets the kind moniker for aggregates produced by aggregators that are configured by metric configurations represented
            /// through instances of <see cref="MetricSeriesConfigurationForMeasurement"/>.
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Part of Public API and too late to change.")]
            public string AggregateKindMoniker
            {
                get { return Constants.AggregateKindMoniker; }
            }

            /// <summary>
            /// Gets constants used to refer to data fields contained within aggregates produced by aggregators that are configured
            /// by metric configurations represented through instances of <see cref="MetricSeriesConfigurationForMeasurement"/>.
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Part of Public API and too late to change.")]
            public DataKeysConstants AggregateKindDataKeys
            {
                get { return DataKeysConstants.Instance; }
            }

            /// <summary>
            /// Groups constants used to refer to data fields contained within aggregates produced by aggregators that are configured
            /// by metric configurations represented through instances of <see cref="MetricSeriesConfigurationForMeasurement"/>.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Never)]
            public sealed class DataKeysConstants
            {
                internal static readonly DataKeysConstants Instance = new DataKeysConstants();

                private DataKeysConstants()
                {
                }

                /// <summary>
                /// Gets the name of the Count field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Part of Public API and too late to change.")]
                public string Count
                {
                    get { return Constants.AggregateKindDataKeys.Count; }
                }

                /// <summary>
                /// Gets the name of the Sum field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Part of Public API and too late to change.")]
                public string Sum
                {
                    get { return Constants.AggregateKindDataKeys.Sum; }
                }

                /// <summary>
                /// Gets the name of the Minimum field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Part of Public API and too late to change.")]
                public string Min
                {
                    get { return Constants.AggregateKindDataKeys.Min; }
                }

                /// <summary>
                /// Gets the name of the Maximum field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Part of Public API and too late to change.")]
                public string Max
                {
                    get { return Constants.AggregateKindDataKeys.Max; }
                }

                /// <summary>
                /// Gets the name of the Standard Deviation field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Part of Public API and too late to change.")]
                public string StdDev
                {
                    get { return Constants.AggregateKindDataKeys.StdDev; }
                }
            }
        }

        /// <summary>
        /// Defines constants used my metric aggregates produced by aggregators that are configured by metric configurations represented
        /// through instances of <see cref="MetricSeriesConfigurationForMeasurement"/>.
        /// </summary>
        internal static class Constants
        {
            /// <summary>
            /// The kind moniker for aggregates produced by aggregators that are configured by metric configurations represented
            /// through instances of <see cref="MetricSeriesConfigurationForMeasurement"/>.
            /// </summary>
            public const string AggregateKindMoniker = "Microsoft.Azure.Measurement";

            /// <summary>
            /// Defines constants used to refer to data fields contained within aggregates produced by aggregators that are configured
            /// by metric configurations represented through instances of <see cref="MetricSeriesConfigurationForMeasurement"/>.
            /// </summary>
            public static class AggregateKindDataKeys
            {
                /// <summary>
                /// The name of the Count field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                public const string Count = "Count";

                /// <summary>
                /// The name of the Sum field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                public const string Sum = "Sum";

                /// <summary>
                /// The name of the Minimum field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                public const string Min = "Min";

                /// <summary>
                /// The name of the Maximum field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                public const string Max = "Max";

                /// <summary>
                /// The name of the Standard Deviation field in <see cref="MetricAggregate"/> objects produced by measurement aggregators.
                /// </summary>
                public const string StdDev = "StdDev";
            }
        }
    }
#pragma warning restore CA1034 // "Do not nest type" - part of the public API and too late to change.
}

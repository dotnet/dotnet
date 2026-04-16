namespace Microsoft.ApplicationInsights.Metrics.Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.DataContracts;

    using static System.FormattableString;

    /// <summary>Converts the Metrics-Aggregation-SDK exchange type for aggregates (<c>MetricAggregate</c>) to
    /// the Application Insights exchange type for the same (<c>MetricTelemetry</c>). This abstract base class provides
    /// common functionality between aggregates for different aggregation kinds.
    /// </summary>
    /// @PublicExposureCandidate
    internal abstract class MetricAggregateToApplicationInsightsPipelineConverterBase : IMetricAggregateToTelemetryPipelineConverter
    {
        /// <summary>Property name for storing the aggregation interval length.</summary>
        public const string AggregationIntervalMonikerPropertyKey = "_MS.AggregationIntervalMs";

        /// <summary>Gets the name for the aggregation kind sopported by this converter (e.g. <c>Microsoft.Azure.Measurement</c>).</summary>
        public abstract string AggregationKindMoniker { get; }

        /// <summary>Converts a <c>Microsoft.ApplicationInsights.Metrics.MetricAggregate</c> to
        /// a <c>Microsoft.ApplicationInsights.DataContracts.MetricTelemetry</c>. </summary>
        /// <param name="aggregate">A metric aggregate.</param>
        /// <returns>A metric telemetry item representing the aggregate.</returns>
        public object Convert(MetricAggregate aggregate)
        {
            this.ValidateAggregate(aggregate);

            MetricTelemetry telemetryItem = this.ConvertAggregateToTelemetry(aggregate);
            return telemetryItem;
        }

        /// <summary>Subclasses need to override this method to actually send the metric telemetry item's properties
        /// based on the cntents of the aggregate and the aggregation kind.</summary>
        /// <param name="telemetryItem">A metric telemetry item representing the aggregate.</param>
        /// <param name="aggregate">A metric aggregate.</param>
        protected abstract void PopulateDataValues(MetricTelemetry telemetryItem, MetricAggregate aggregate);

        private static void PopulateTelemetryContext(
                                                IDictionary<string, string> dimensions,
                                                Microsoft.ApplicationInsights.DataContracts.TelemetryContext telemetryContext,
                                                out IEnumerable<KeyValuePair<string, string>> nonContextDimensions)
        {
            if (dimensions == null)
            {
                nonContextDimensions = null;
                return;
            }

            List<KeyValuePair<string, string>> nonContextDimensionList = null;

            foreach (KeyValuePair<string, string> dimension in dimensions)
            {
                if (String.IsNullOrWhiteSpace(dimension.Key) || dimension.Value == null)
                {
                    continue;
                }

                switch (dimension.Key)
                {
                    case MetricDimensionNames.TelemetryContext.InstrumentationKey:
                        telemetryContext.InstrumentationKey = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Cloud.RoleInstance:
                        telemetryContext.Cloud.RoleInstance = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Cloud.RoleName:
                        telemetryContext.Cloud.RoleName = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Component.Version:
                        telemetryContext.Component.Version = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Device.Id:
                        telemetryContext.Device.Id = dimension.Value;
                        break;

                    #pragma warning disable CS0618  // Type or member is obsolete
                    case MetricDimensionNames.TelemetryContext.Device.Language:
                        telemetryContext.Device.Language = dimension.Value;
                        break;
                    #pragma warning restore CS0618  // Type or member is obsolete

                    case MetricDimensionNames.TelemetryContext.Device.Model:
                        telemetryContext.Device.Model = dimension.Value;
                        break;

                    #pragma warning disable CS0618  // Type or member is obsolete
                    case MetricDimensionNames.TelemetryContext.Device.NetworkType:
                        telemetryContext.Device.NetworkType = dimension.Value;
                        break;
                    #pragma warning restore CS0618  // Type or member is obsolete

                    case MetricDimensionNames.TelemetryContext.Device.OemName:
                        telemetryContext.Device.OemName = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Device.OperatingSystem:
                        telemetryContext.Device.OperatingSystem = dimension.Value;
                        break;

                    #pragma warning disable CS0618  // Type or member is obsolete
                    case MetricDimensionNames.TelemetryContext.Device.ScreenResolution:
                        telemetryContext.Device.ScreenResolution = dimension.Value;
                        break;
                    #pragma warning restore CS0618  // Type or member is obsolete

                    case MetricDimensionNames.TelemetryContext.Device.Type:
                        telemetryContext.Device.Type = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Location.Ip:
                        telemetryContext.Location.Ip = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Operation.CorrelationVector:
                        telemetryContext.Operation.CorrelationVector = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Operation.Id:
                        telemetryContext.Operation.Id = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Operation.Name:
                        telemetryContext.Operation.Name = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Operation.ParentId:
                        telemetryContext.Operation.ParentId = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Operation.SyntheticSource:
                        telemetryContext.Operation.SyntheticSource = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Session.Id:
                        telemetryContext.Session.Id = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.Session.IsFirst:
                        try
                        {
                            telemetryContext.Session.IsFirst = System.Convert.ToBoolean(dimension.Value, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            try
                            {
                                int val = System.Convert.ToInt32(dimension.Value, CultureInfo.InvariantCulture);
                                if (val == 1)
                                {
                                    telemetryContext.Session.IsFirst = true;
                                }
                                else if (val == 0)
                                {
                                    telemetryContext.Session.IsFirst = false;
                                }
                            }
                            catch
                            {
                            }
                        }

                        break;

                    case MetricDimensionNames.TelemetryContext.User.AccountId:
                        telemetryContext.User.AccountId = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.User.AuthenticatedUserId:
                        telemetryContext.User.AuthenticatedUserId = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.User.Id:
                        telemetryContext.User.Id = dimension.Value;
                        break;

                    case MetricDimensionNames.TelemetryContext.User.UserAgent:
                        telemetryContext.User.UserAgent = dimension.Value;
                        break;

                    default:
                        string dimensionName;
                        if (MetricDimensionNames.TelemetryContext.IsProperty(dimension.Key, out dimensionName))
                        {
                            telemetryContext.GlobalProperties[dimensionName] = dimension.Value;
                        }
                        else
                        {
                            if (nonContextDimensionList == null)
                            {
                                nonContextDimensionList = new List<KeyValuePair<string, string>>(dimensions.Count);
                            }

                            nonContextDimensionList.Add(dimension);
                        }

                        break;
                }
            }

            nonContextDimensions = nonContextDimensionList;
        }

        private void ValidateAggregate(MetricAggregate metricAggregate)
        {
            Util.ValidateNotNull(metricAggregate, nameof(metricAggregate));
            Util.ValidateNotNull(metricAggregate.AggregationKindMoniker, nameof(metricAggregate.AggregationKindMoniker));

            string expectedMoniker = this.AggregationKindMoniker;

            if (!metricAggregate.AggregationKindMoniker.Equals(expectedMoniker, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(Invariant($"Cannot convert the specified {metricAggregate}, because is has")
                                          + Invariant($" {nameof(metricAggregate.AggregationKindMoniker)}=\"{metricAggregate.AggregationKindMoniker}\".")
                                          + Invariant($" This converter handles \"{expectedMoniker}\"."));
            }
        }

        private MetricTelemetry ConvertAggregateToTelemetry(MetricAggregate aggregate)
        {
            MetricTelemetry telemetryItem = new MetricTelemetry();

            if (aggregate.MetricNamespace.Length > 0)
            {
                telemetryItem.MetricNamespace = aggregate.MetricNamespace;
            }

            telemetryItem.Name = aggregate.MetricId;

            // Set data values:

            this.PopulateDataValues(telemetryItem, aggregate);

            // Set timing values:

            IDictionary<string, string> props = telemetryItem.Properties;
            if (props != null)
            {
                long periodMillis = (long)aggregate.AggregationPeriodDuration.TotalMilliseconds;
                props.Add(AggregationIntervalMonikerPropertyKey, periodMillis.ToString(CultureInfo.InvariantCulture));
            }

            telemetryItem.Timestamp = aggregate.AggregationPeriodStart;

            // Populate TelemetryContext:
            IEnumerable<KeyValuePair<string, string>> nonContextDimensions;
            PopulateTelemetryContext(aggregate.Dimensions, telemetryItem.Context, out nonContextDimensions);

            // Set dimensions. We do this after the context, becasue dimensions take precedence (i.e. we potentially overwrite):
            if (nonContextDimensions != null)
            {
                foreach (KeyValuePair<string, string> nonContextDimension in nonContextDimensions)
                {
                    telemetryItem.Properties[nonContextDimension.Key] = nonContextDimension.Value;
                }
            }

            // Set SDK version moniker:

            Util.StampSdkVersionToContext(telemetryItem);

            return telemetryItem;
        }
    }
}

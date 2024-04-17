namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    internal class CalculatedMetricInfo
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember(Name = "TelemetryType")]
        public string TelemetryTypeForSerialization
        {
            get
            {
                return this.TelemetryType.ToString();
            }

            set
            {
                TelemetryType telemetryType;
                if (!Enum.TryParse(value, out telemetryType))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        string.Format(CultureInfo.InvariantCulture, "Unsupported TelemetryType value: {0}", value));
                }

                this.TelemetryType = telemetryType;
            }
        }

        public TelemetryType TelemetryType { get; set; }

        /// <summary>
        /// Gets or sets an OR-connected collection of FilterConjunctionGroupInfo objects.
        /// </summary>
        [DataMember]
        public FilterConjunctionGroupInfo[] FilterGroups { get; set; }

        [DataMember]
        public string Projection { get; set; }

        [DataMember(Name = "Aggregation")]
        public string AggregationForSerialization
        {
            get
            {
                return this.Aggregation.ToString();
            }

            set
            {
                AggregationType aggregation;
                if (!Enum.TryParse(value, out aggregation))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        string.Format(CultureInfo.InvariantCulture, "Unsupported Aggregation value: {0}", value));
                }

                this.Aggregation = aggregation;
            }
        }

        public AggregationType Aggregation { get; set; }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Id: '{0}', TelemetryType: '{1}', Projection: '{2}', Aggregation: '{3}', FilterGroups: [{4}]",
                this.Id,
                this.TelemetryType,
                this.Projection,
                this.Aggregation,
                this.FilterGroupsToString());
        }

        private string FilterGroupsToString()
        {
            if (this.FilterGroups == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Join(", ", this.FilterGroups.Select(filterGroup => filterGroup.ToString()));
            }
        }
    }
}
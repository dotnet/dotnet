namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [DataContract]
    internal class DocumentFilterConjunctionGroupInfo
    {
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
        /// Gets or sets an AND-connected group of filters.
        /// </summary>
        [DataMember]
        public FilterConjunctionGroupInfo Filters { get; set; }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "TelemetryType: '{0}', filters: '{1}'",
                this.TelemetryType,
                this.Filters?.ToString() ?? string.Empty);
        }
    }
}
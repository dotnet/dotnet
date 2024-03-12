namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to add the EventData attribute and any additional customizations to the generated type.
    /// </summary>
#if !NET452
    // .NET 4.5.2 have a custom implementation of RichPayloadEventSource
    [System.Diagnostics.Tracing.EventData(Name = "PartB_MetricData")]
#endif
    internal partial class MetricData
    {
        public MetricData DeepClone()
        {
            var other = new MetricData();
            other.ver = this.ver;

            Debug.Assert(other.metrics != null, "The constructor should have allocated metrics list");
            foreach (var metric in this.metrics)
            {
                other.metrics.Add(metric.DeepClone());
            }

            Debug.Assert(other.properties != null, "The constructor should have allocated properties dictionary");
            Utils.CopyDictionary(this.properties, other.properties);
            return other;
        }
    }
}
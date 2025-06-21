namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to add the EventData attribute and any additional customizations to the generated type.
    /// </summary>
#if !NET452
    // .NET 4.5.2 have a custom implementation of RichPayloadEventSource
    [System.Diagnostics.Tracing.EventData(Name = "PartB_DataPoint")]
#endif
    internal partial class DataPoint
    {
        public DataPoint DeepClone()
        {
            var other = new DataPoint();
            other.ns = this.ns;
            other.name = this.name;
            other.kind = this.kind;
            other.value = this.value;
            other.count = this.count;
            other.min = this.min;
            other.max = this.max;
            other.stdDev = this.stdDev;
            return other;
        }
    }
}

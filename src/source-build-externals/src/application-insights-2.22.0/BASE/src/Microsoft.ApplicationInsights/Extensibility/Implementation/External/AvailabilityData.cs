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
    [System.Diagnostics.Tracing.EventData(Name = "PartB_AvailabilityData")]
#endif
    internal partial class AvailabilityData
    {
        public AvailabilityData DeepClone()
        {
            var other = new AvailabilityData();
            other.ver = this.ver;
            other.id = this.id;
            other.name = this.name;
            other.duration = this.duration;
            other.success = this.success;
            other.runLocation = this.runLocation;
            other.message = this.message;
            Debug.Assert(other.properties != null, "The constructor should have allocated properties dictionary");
            Debug.Assert(other.measurements != null, "The constructor should have allocated the measurements dictionary");
            Utils.CopyDictionary(this.properties, other.properties);
            Utils.CopyDictionary(this.measurements, other.measurements);
            return other;
        }
    }
}

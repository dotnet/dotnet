namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class AvailabilityData : ISerializableWithWriter
    {
        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("ver", this.ver);
            serializationWriter.WriteProperty("id", this.id);
            serializationWriter.WriteProperty("name", this.name);
            serializationWriter.WriteProperty("duration", this.duration);
            serializationWriter.WriteProperty("success", this.success);
            serializationWriter.WriteProperty("runLocation", this.runLocation);
            serializationWriter.WriteProperty("message", this.message);
            serializationWriter.WriteProperty("properties", this.properties);
            serializationWriter.WriteProperty("measurements", this.measurements);
        }
    }
}

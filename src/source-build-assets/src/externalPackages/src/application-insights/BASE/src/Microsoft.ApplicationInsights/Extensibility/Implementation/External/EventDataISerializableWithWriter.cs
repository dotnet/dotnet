namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class EventData : ISerializableWithWriter
    {
        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("ver", this.ver);
            serializationWriter.WriteProperty("name", this.name);

            serializationWriter.WriteProperty("properties", this.properties);
            serializationWriter.WriteProperty("measurements", this.measurements);
        }
    }
}

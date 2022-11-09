namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class MessageData : ISerializableWithWriter
    {
        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("ver", this.ver);
            serializationWriter.WriteProperty("message", this.message);
            serializationWriter.WriteProperty("severityLevel", this.severityLevel.HasValue ? this.severityLevel.Value.ToString() : null);
            serializationWriter.WriteProperty("properties", this.properties);
            serializationWriter.WriteProperty("measurements", this.measurements);
        }
    }
}

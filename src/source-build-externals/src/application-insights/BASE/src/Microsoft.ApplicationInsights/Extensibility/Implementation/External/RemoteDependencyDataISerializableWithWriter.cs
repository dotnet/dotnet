namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class RemoteDependencyData : ISerializableWithWriter
    {
        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("ver", this.ver);
            serializationWriter.WriteProperty("name", this.name);
            serializationWriter.WriteProperty("id", this.id);
            serializationWriter.WriteProperty("data", this.data);
            serializationWriter.WriteProperty("duration", this.duration);
            serializationWriter.WriteProperty("resultCode", this.resultCode);
            serializationWriter.WriteProperty("success", this.success);
            serializationWriter.WriteProperty("type", this.type);
            serializationWriter.WriteProperty("target", this.target);
            serializationWriter.WriteProperty("properties", this.properties);
            serializationWriter.WriteProperty("measurements", this.measurements);
        }
    }
}
namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class MetricData : ISerializableWithWriter
    {
        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("ver", this.ver);            
            serializationWriter.WriteProperty("metrics", this.metrics.ToList<ISerializableWithWriter>());
            serializationWriter.WriteProperty("properties", this.properties);
        }
    }
}
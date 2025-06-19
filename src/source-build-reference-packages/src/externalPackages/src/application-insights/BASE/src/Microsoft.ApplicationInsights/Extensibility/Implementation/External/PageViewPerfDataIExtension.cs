namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class PageViewPerfData : ISerializableWithWriter
    {
        public new void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("ver", this.ver);
            serializationWriter.WriteProperty("name", this.name);
            serializationWriter.WriteProperty("url", this.url);
            serializationWriter.WriteProperty("duration", Utils.ValidateDuration(this.duration));
            serializationWriter.WriteProperty("domProcessing", Utils.ValidateDuration(this.domProcessing));
            serializationWriter.WriteProperty("perfTotal", Utils.ValidateDuration(this.perfTotal));
            serializationWriter.WriteProperty("networkConnect", Utils.ValidateDuration(this.networkConnect));
            serializationWriter.WriteProperty("sentRequest", Utils.ValidateDuration(this.sentRequest));
            serializationWriter.WriteProperty("receivedResponse", Utils.ValidateDuration(this.receivedResponse));
            serializationWriter.WriteProperty("properties", this.properties);
            serializationWriter.WriteProperty("measurements", this.measurements);
        }
    }
}
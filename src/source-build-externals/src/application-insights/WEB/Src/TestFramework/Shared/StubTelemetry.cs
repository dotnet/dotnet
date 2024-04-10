namespace Microsoft.ApplicationInsights.Web.TestFramework
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal sealed class StubTelemetry : ITelemetry, ISupportProperties
    {
        public Action<IJsonWriter> OnSerialize = jsonWriter => { };

        public Action<object> OnSendEvent = eventSourceWriter => { };

        public StubTelemetry()
        {
            this.Context = new TelemetryContext();
            this.Properties = new Dictionary<string, string>();
        }

        public DateTimeOffset Timestamp { get; set; }

        public string Sequence { get; set; }

        public TelemetryContext Context { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public IExtension Extension { get; set; }

        public void Serialize(IJsonWriter writer)
        {
            this.OnSerialize(writer);
        }

        public void Sanitize()
        {
        }

        public void SendEvent(object writer)
        {
            this.OnSendEvent(writer);
        }

        public ITelemetry DeepClone()
        {
            return this;
        }

        public void SerializeData(ISerializationWriter serializationWriter)
        {            
        }
    }
}

namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal sealed class StubSerializableTelemetry : ITelemetry, ISupportProperties, IAiSerializableTelemetry
    {
        public Action<IJsonWriter> OnSerialize = jsonWriter => { };

        public Action<object> OnSendEvent = eventSourceWriter => { };

        public StubSerializableTelemetry()
        {            
            this.Properties = new Dictionary<string, string>();
            this.Context = new TelemetryContext(this.Properties);
        }

        public DateTimeOffset Timestamp { get; set; }

        public string Sequence { get; set; }

        public TelemetryContext Context { get; set; }

        public IDictionary<string, string> Properties { get; set; }
        public IExtension Extension { get; set; }

        public string TelemetryName { get; set; } = "StubTelemetryName";

        public string BaseType => "StubTelemetryBaseType";

        public void Serialize(IJsonWriter writer)
        {
            this.OnSerialize(writer);
        }

        public void Sanitize()
        {
        }

        public ITelemetry DeepClone()
        {
            throw new NotImplementedException();
        }

        public void SendEvent(object writer)
        {
            this.OnSendEvent(writer);
        }

        public void Serialize(ISerializationWriter serializationWriter)
        {
            
        }

        public void SerializeData(ISerializationWriter serializationWriter)
        {

        }
    }
}

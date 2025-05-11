namespace Microsoft.ApplicationInsights.DataContracts
{
    using System.IO;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using JsonSerializer = Microsoft.ApplicationInsights.Extensibility.Implementation.JsonSerializer;

    internal static class TelemetryItemTestHelper
    {
        /// <summary>
        /// Serializes and deserializes the telemetry item and returns the results.
        /// </summary>
        internal static AI.TelemetryItem<TelemetryDataType> SerializeDeserializeTelemetryItem<TelemetryDataType>(ITelemetry telemetry)
        {
            byte[] b = JsonSerializer.Serialize(telemetry, compress: false);

            JsonReader reader = new JsonTextReader(new StringReader(Encoding.UTF8.GetString(b, 0, b.Length)));
            reader.DateParseHandling = DateParseHandling.None;
            JObject obj = JObject.Load(reader);
            return obj.ToObject<AI.TelemetryItem<TelemetryDataType>>();
        }
    }
}
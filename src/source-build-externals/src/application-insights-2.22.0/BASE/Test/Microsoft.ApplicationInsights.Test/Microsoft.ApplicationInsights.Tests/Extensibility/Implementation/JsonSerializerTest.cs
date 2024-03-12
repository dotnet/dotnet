namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using AI;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;    
    using System.IO;
    using System.Text;

    /// <summary>
    /// Tests for <see cref="JsonSerializer"/>
    /// </summary>
    [TestClass]
    public class JsonSerializerTest
    {
        [TestMethod]
        public void SerializeAsStringMethodSerializesATelemetryCorrectly()
        {
            var exceptionTelemetry = new ExceptionTelemetry();
            string exceptionAsJson = JsonSerializer.SerializeAsString(exceptionTelemetry);

            // Expected: {"name":"Microsoft.ApplicationInsights.Exception","time":"0001-01-01T00:00:00.0000000+00:00","data":{"baseType":"ExceptionData","baseData":{"ver":2,"handledAt":"Unhandled","exceptions":[]}}}
            // Deserialize (Validates a valid JSON string)
            JObject obj = JsonConvert.DeserializeObject<JObject>(exceptionAsJson);

            // Validates 2 random properties
            Assert.IsNotNull(exceptionAsJson);
            Assert.AreEqual(ItemType.Exception, obj["name"].ToString());
        }

        [TestMethod]
        public void SanitizesTelemetryItem()
        {
            string name = new string('Z', 10000);
            EventTelemetry t = new EventTelemetry(name);

            string exceptionAsJson = JsonSerializer.SerializeAsString(t);

            JObject obj = JsonConvert.DeserializeObject<JObject>(exceptionAsJson);
            
            Assert.AreEqual(512, obj["data"]["baseData"]["name"].ToString().Length);
        }

        [TestMethod]
        public void SanitizesTelemetryContextGlobalProperties()
        {
            var addedValueWithSizeAboveLimit = new string('V', Property.MaxValueLength + 1);
            var expectedValueWithSizeWithinLimit = new string('V', Property.MaxValueLength);

            EventTelemetry t = new EventTelemetry("myevent");
            t.Context.GlobalProperties.Add("mykey", addedValueWithSizeAboveLimit);

            string exceptionAsJson = JsonSerializer.SerializeAsString(t);

            JObject obj = JsonConvert.DeserializeObject<JObject>(exceptionAsJson);

            Assert.AreEqual(expectedValueWithSizeWithinLimit, obj["data"]["baseData"]["properties"]["mykey"]);
            
        }

        [TestMethod]
        public void SanitizesTimestampInIsoFormat()
        {
            EventTelemetry t = new EventTelemetry();
            
            string json = JsonSerializer.SerializeAsString(t);

            Assert.IsTrue(json.Contains("\"time\":\"0001-01-01T00:00:00.0000000Z\""));
        }

        [TestMethod]
        public void SerializesNotZeroFlags()
        {
            EventTelemetry t = new EventTelemetry();
            t.Context.Flags = TelemetryContext.FlagDropIdentifiers;

            string json = JsonSerializer.SerializeAsString(t);

            Assert.IsTrue(json.Contains("\"flags\":2097152"), json);
        }

        [TestMethod]
        public void IfCallConvertToArrayAndThanDeserializeYouGetSameResult()
        {
            byte[] array = JsonSerializer.ConvertToByteArray("test");
            string result = JsonSerializer.Deserialize(array);

            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void SerializesUnknownTelemetryIntoCustomEventWithProperties()
        {
            UnknownTelemetry unknownTelemetryType = new UnknownTelemetry();
            unknownTelemetryType.Properties.Add("Name", "Value");
            DateTimeOffset testTime = DateTimeOffset.Now;
            unknownTelemetryType.Timestamp = testTime;
            unknownTelemetryType.Context.GlobalProperties.Add("GlobalName", "GlobalValue");
            unknownTelemetryType.Context.User.Id = "testUser";
            int intField = 42;
            string stringField = "value";
            unknownTelemetryType.Extension = new MyTestExtension { myIntField = intField, myStringField = stringField };

            byte[] bytes = JsonSerializer.Serialize(unknownTelemetryType, compress: false);

            JsonReader reader = new JsonTextReader(new StringReader(Encoding.UTF8.GetString(bytes, 0, bytes.Length)));
            reader.DateParseHandling = DateParseHandling.None;
            JObject obj = JObject.Load(reader);

            TelemetryItem<EventTelemetry> data = obj.ToObject<TelemetryItem<EventTelemetry>>();

            Assert.AreEqual(ItemType.Event, data.name);
            Assert.AreEqual(Constants.EventNameForUnknownTelemetry, data.data.baseData.Name);
            Assert.AreEqual("testUser", data.tags["ai.user.id"]);
            Assert.IsTrue(DateTimeOffset.TryParse(data.time, out DateTimeOffset testResult));
            Assert.AreEqual(testTime, testResult);           

            Assert.IsTrue(data.data.baseData.Properties["properties.Name"] == "Value");
            Assert.IsTrue(data.data.baseData.Properties["GlobalName"] == "GlobalValue");
            Assert.IsTrue(data.data.baseData.Properties["duration"] == unknownTelemetryType.Duration.ToString());
            Assert.IsTrue(data.data.baseData.Properties["success"] == unknownTelemetryType.Success.ToString());
            Assert.IsTrue(data.data.baseData.Properties["id"] == unknownTelemetryType.Id);
            Assert.IsTrue(data.data.baseData.Properties["responseCode"] == unknownTelemetryType.ResponseCode);
            Assert.IsTrue(data.data.baseData.Properties["myIntField"] == intField.ToString());
            Assert.IsTrue(data.data.baseData.Properties["myStringField"] == stringField);
            Assert.IsTrue(data.data.baseData.Properties.Count == 8);
        }

        [TestMethod]
        public void SerializesKnownTelemetryWithExtension()
        {
            RequestTelemetry request = new RequestTelemetry();
            int intField = 42;
            string stringField = "value";
            request.Extension = new MyTestExtension { myIntField = intField, myStringField = stringField };

            byte[] bytes = JsonSerializer.Serialize(request, compress: false);

            JsonReader reader = new JsonTextReader(new StringReader(Encoding.UTF8.GetString(bytes, 0, bytes.Length)));
            reader.DateParseHandling = DateParseHandling.None;
            JObject obj = JObject.Load(reader);

            TelemetryItem<RequestTelemetry> data = obj.ToObject<TelemetryItem<RequestTelemetry>>();

            Assert.IsTrue(data.data.baseData.Properties["myIntField"] == intField.ToString());
            Assert.IsTrue(data.data.baseData.Properties["myStringField"] == stringField);            
        }

        [TestMethod]
        public void SerializesKnownTelemetryWithNullExtension()
        {
            RequestTelemetry request = new RequestTelemetry();
            request.Extension = null;

            byte[] bytes = JsonSerializer.Serialize(request, compress: false);

            JsonReader reader = new JsonTextReader(new StringReader(Encoding.UTF8.GetString(bytes, 0, bytes.Length)));
            reader.DateParseHandling = DateParseHandling.None;
            JObject obj = JObject.Load(reader);

            TelemetryItem<RequestTelemetry> data = obj.ToObject<TelemetryItem<RequestTelemetry>>();

            Assert.IsTrue(!data.data.baseData.Properties.ContainsKey("myIntField"));
            Assert.IsTrue(!data.data.baseData.Properties.ContainsKey("myStringField"));
        }
    }
}
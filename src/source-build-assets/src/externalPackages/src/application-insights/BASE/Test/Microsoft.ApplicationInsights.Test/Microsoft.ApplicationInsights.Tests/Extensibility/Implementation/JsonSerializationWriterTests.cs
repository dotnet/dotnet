namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;


    /// <summary>
    /// Tests for <see cref="JsonSerializationWriter"/>
    /// </summary>
    [TestClass]
    public class JsonSerializationWriterTests
    {
        [TestMethod]
        public void SerializeComplexObject()
        {
            var complexExtension = new ComplexExtension();            
            var mySubSubExtension1 = new MySubSubExtension() { Field3 = "Value1 for field3", Field4 = 100.00 };
            var mySubSubExtension2 = new MySubSubExtension() { Field3 = "Value2 for field3", Field4 = 200.00 };
            var mySubExtension1 = new MySubExtension() { Field1 = "Value1 for field1", Field2 = 100 , MySubSubExtension = mySubSubExtension1 };
            var mySubExtension2 = new MySubExtension() { Field1 = "Value2 for field1", Field2 = 200, MySubSubExtension = mySubSubExtension2 };
            var listExtension = new List<MySubExtension>();
            listExtension.Add(mySubExtension1);
            listExtension.Add(mySubExtension2);

            var listString = new List<string>();
            listString.Add("Item1");
            listString.Add("Item2");
            listString.Add("Item3");

            complexExtension.MyBoolField = true;
            complexExtension.MyDateTimeOffsetField = DateTimeOffset.Now;
            complexExtension.MyDoubleField = 100.10;
            complexExtension.MyIntField = 100;
            complexExtension.MyStringField = "ValueStringField";
            complexExtension.MyTimeSpanField = TimeSpan.FromSeconds(2);
            complexExtension.MySubExtensionField = mySubExtension1;
            complexExtension.MyExtensionListField = listExtension;
            complexExtension.MyStringListField = listString;

            var dicString = new Dictionary<string, string>();
            dicString.Add("Key1", "Value1");
            dicString.Add("Key2", "Value2");
            complexExtension.MyStringDictionaryField = dicString;

            var dicDouble = new Dictionary<string, double>();
            dicDouble.Add("Key1", 1000.000);
            dicDouble.Add("Key2", 2000.000);
            complexExtension.MyDoubleDictionaryField = dicDouble;

            var stringBuilder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            {
                var jsonSerializationWriter = new JsonSerializationWriter(stringWriter);
                jsonSerializationWriter.WriteStartObject();
                complexExtension.Serialize(jsonSerializationWriter);
                jsonSerializationWriter.WriteEndObject();
            }

            string actualJson = stringBuilder.ToString();
            Trace.WriteLine(actualJson);
            
            JObject obj = JsonConvert.DeserializeObject<JObject>(actualJson);
            
            Assert.IsNotNull(actualJson);            
            Assert.AreEqual("ValueStringField", obj["MyStringField"].ToString());
            Assert.AreEqual(100, int.Parse(obj["MyIntField"].ToString()));
            Assert.AreEqual(100.10, double.Parse(obj["MyDoubleField"].ToString()));
            Assert.AreEqual(true, bool.Parse(obj["MyBoolField"].ToString()));
            Assert.AreEqual(TimeSpan.FromSeconds(2), TimeSpan.Parse(obj["MyTimeSpanField"].ToString()));
            //Assert.AreEqual(DateTimeOffset., double.Parse(obj["MyDateTimeOffsetField"].ToString()));

            Assert.AreEqual("Value1 for field1",obj["MySubExtensionField"]["Field1"].ToString());
            Assert.AreEqual(100, int.Parse(obj["MySubExtensionField"]["Field2"].ToString()));

            Assert.AreEqual("Value1 for field3", obj["MySubExtensionField"]["MySubSubExtension"]["Field3"].ToString());
            Assert.AreEqual(100, int.Parse(obj["MySubExtensionField"]["MySubSubExtension"]["Field4"].ToString()));

            Assert.AreEqual("Item1", obj["MyStringListField"][0].ToString());
            Assert.AreEqual("Item2", obj["MyStringListField"][1].ToString());
            Assert.AreEqual("Item3", obj["MyStringListField"][2].ToString());

            Assert.AreEqual("Value1 for field1", obj["MyExtensionListField"][0]["Field1"].ToString());
            Assert.AreEqual(100, int.Parse(obj["MyExtensionListField"][0]["Field2"].ToString()));
            Assert.AreEqual("Value1 for field3", obj["MyExtensionListField"][0]["MySubSubExtension"]["Field3"].ToString());
            Assert.AreEqual(100, int.Parse(obj["MyExtensionListField"][0]["MySubSubExtension"]["Field4"].ToString()));

            Assert.AreEqual("Value2 for field1", obj["MyExtensionListField"][1]["Field1"].ToString());
            Assert.AreEqual(200, int.Parse(obj["MyExtensionListField"][1]["Field2"].ToString()));
            Assert.AreEqual("Value2 for field3", obj["MyExtensionListField"][1]["MySubSubExtension"]["Field3"].ToString());
            Assert.AreEqual(200, int.Parse(obj["MyExtensionListField"][1]["MySubSubExtension"]["Field4"].ToString()));

            Assert.AreEqual("Value1", obj["MyStringDictionaryField"]["Key1"].ToString());
            Assert.AreEqual("Value2", obj["MyStringDictionaryField"]["Key2"].ToString());

            Assert.AreEqual(1000, double.Parse(obj["MyDoubleDictionaryField"]["Key1"].ToString()));
            Assert.AreEqual(2000, double.Parse(obj["MyDoubleDictionaryField"]["Key2"].ToString()));

        }
    }
}
namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.TestFramework;

    [TestClass]
    public class JsonSerializationWriterTest
    {
        [TestMethod]
        public void ClassIsInternalAndNotMeantToBeAccessedByCustomers()
        {
            Assert.IsFalse(typeof(JsonSerializationWriter).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void WriteStartArrayWritesOpeningSquareBracket()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteStartArray();
                Assert.AreEqual("[", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WriteStartObjectWritesOpeningCurlyBrace()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteStartObject();
                Assert.AreEqual("{", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WriteEndArrayWritesClosingSquareBracket()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteEndArray();
                Assert.AreEqual("]", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WriteEndObjectWritesClosingCurlyBrace()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteEndObject();
                Assert.AreEqual("}", stringWriter.ToString());
            }        
        }

        [TestMethod]
        public void WriteRawValueWritesValueWithoutEscapingValue()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteRawValue(@"Test\Name");
                Assert.AreEqual(@"Test\Name", stringWriter.ToString());
            }
        }

        #region WriteProperty(string, int?)

        [TestMethod]
        public void WritePropertyIntWritesIntValueWithoutQuotationMark()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                const string Name = "name";
                const int Value = 42;
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, Value);
                Assert.AreEqual("\"" + Name + "\":" + Value, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyIntDoesNothingIfValueIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (int?)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, double?)

        [TestMethod]
        public void WritePropertyDoubleWritesDoubleValueWithoutQuotationMark()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                const string Name = "name";
                const double Value = 42.3;
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, Value);
                Assert.AreEqual("\"" + Name + "\":" + Value.ToString(CultureInfo.InvariantCulture), stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyDoubleDoesNothingIfValueIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (double?)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, TimeSpan?)

        [TestMethod]
        public void WritePropertyTimeSpanWritesTimeSpanValueWithQuotationMark()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                const string Name = "name";
                TimeSpan value = TimeSpan.FromSeconds(123);
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, value);
                Assert.AreEqual("\"" + Name + "\":\"" + value + "\"", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyTimeSpanDoesNothingIfValueIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (TimeSpan?)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, string)

        [TestMethod]
        public void WritePropertyStringWritesValueInDoubleQuotes()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                const string Name = "name";
                const string Value = "value";
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, Value);
                Assert.AreEqual("\"" + Name + "\":\"" + Value + "\"", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyStringThrowsArgumentNullExceptionForNameInputAsNull()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var writer = new JsonSerializationWriter(stringWriter);
                AssertEx.Throws<ArgumentNullException>(() => writer.WriteProperty(null, "value"));
            }
        }

        [TestMethod]
        public void WritePropertyStringDoesNothingIfValueIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (string)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyStringDoesNothingIfValueIsEmptyBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", string.Empty);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, bool?)

        [TestMethod]
        public void WritePropertyBooleanWritesValueWithoutQuotationMarks()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                const string Name = "name";
                const bool Value = true;
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, Value);
                string expectedValue = Value.ToString().ToLowerInvariant();
                Assert.AreEqual("\"" + Name + "\":" + expectedValue, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyBooleanDoesNothingIfValueIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (bool?)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyBooleanWritesFalseBecauseItIsExplicitlySet()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                const string Name = "name";
                const bool Value = false;
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, Value);
                string expectedValue = Value.ToString().ToLowerInvariant();
                Assert.AreEqual("\"" + Name + "\":" + expectedValue, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, DateTimeOffset?)

        [TestMethod]
        public void WritePropertyDateTimeOffsetWritesValueInQuotationMarksAndRoundTripDateTimeFormat()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                const string Name = "name";
                DateTimeOffset value = DateTimeOffset.UtcNow;
                new JsonSerializationWriter(stringWriter).WriteProperty(Name, value);
                string expectedValue = value.ToString("o", CultureInfo.InvariantCulture);
                Assert.AreEqual("\"" + Name + "\":\"" + expectedValue + "\"", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyDateTimeOffsetDoesNothingIfValueIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (DateTimeOffset?)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, IDictionary<string, double>)

        [TestMethod]
        public void WritePropertyIDictionaryDoubleWritesPropertyNameFollowedByValuesInCurlyBraces()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var writer = new JsonSerializationWriter(stringWriter);
                writer.WriteProperty("name", new Dictionary<string, double> { { "key1", 1 } });
                AssertEx.StartsWith("\"name\":{", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
                AssertEx.EndsWith("}", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WritePropertyIDictionaryDoubleWritesValuesWithoutDoubleQuotes()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var writer = new JsonSerializationWriter(stringWriter);
                writer.WriteProperty("name", new Dictionary<string, double> { { "key1", 1 } });
                AssertEx.Contains("\"key1\":1", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WritePropertyIDictionaryDoubleDoesNothingWhenDictionaryIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (IDictionary<string, double>)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyIDictionaryDoubleDoesNothingWhenDictionaryIsEmptyBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", new Dictionary<string, double>());
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WriteProperty(string, IDictionary<string, object>)

        [TestMethod]
        public void WritePropertyIDictionaryStringStringWritesPropertyNameFollowedByValuesInCurlyBraces()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var writer = new JsonSerializationWriter(stringWriter);
                writer.WriteProperty("name", new Dictionary<string, string> { { "key1", "1" } });
                AssertEx.StartsWith("\"name\":{", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
                AssertEx.EndsWith("}", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WritePropertyIDictionaryStringStringWritesValuesWithoutDoubleQuotes()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var writer = new JsonSerializationWriter(stringWriter);
                writer.WriteProperty("name", new Dictionary<string, string> { { "key1", "1" } });
                AssertEx.Contains("\"key1\":\"1\"", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WritePropertyIDictionaryStringStringDoesNothingWhenDictionaryIsNullBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", (IDictionary<string, string>)null);
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyIDictionaryStringStringDoesNothingWhenDictionaryIsEmptyBecauseItAssumesPropertyIsOptional()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new JsonSerializationWriter(stringWriter).WriteProperty("name", new Dictionary<string, string>());
                Assert.AreEqual(string.Empty, stringWriter.ToString());
            }
        }

        #endregion

        #region WritePropertyName

        [TestMethod]
        public void WritePropertyNameWritesPropertyNameEnclosedInDoubleQuotationMarksFollowedByColon()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                new TestableJsonSerializationWriter(stringWriter).WritePropertyName("TestProperty");
                Assert.AreEqual("\"TestProperty\":", stringWriter.ToString());
            }
        }

        [TestMethod]
        public void WritePropertyNamePrependsPropertyNameWithComaWhenCurrentObjectAlreadyHasProperties()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WritePropertyName("Property1");
                JsonSerializationWriter.WritePropertyName("Property2");
                AssertEx.Contains(",\"Property2\"", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WritePropertyNameDoesNotPrependPropertyNameWithComaWhenNewObjectWasStarted()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WritePropertyName("Property1");
                JsonSerializationWriter.WriteStartObject();
                JsonSerializationWriter.WritePropertyName("Property2");
                AssertEx.Contains("{\"Property2\"", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WritePropertyNameThrowsArgumentExceptionWhenPropertyNameIsEmptyToPreventOurOwnErrors()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new JsonSerializationWriter(stringWriter);
                AssertEx.Throws<ArgumentException>(() => JsonSerializationWriter.WritePropertyName(string.Empty));
            }
        }

        #endregion

        #region WriteString

        [TestMethod]
        public void WriteStringEscapesQuotationMark()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WriteString("Test\"Value");
                AssertEx.Contains("Test\\\"Value", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WriteStringEscapesBackslash()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WriteString("Test\\Value");
                AssertEx.Contains("Test\\\\Value", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WriteStringEscapesBackspace()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WriteString("Test\bValue");
                AssertEx.Contains("Test\\bValue", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }            
        }

        [TestMethod]
        public void WriteStringEscapesFormFeed()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WriteString("Test\fValue");
                AssertEx.Contains("Test\\fValue", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }            
        }

        [TestMethod]
        public void WriteStringEscapesNewline()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WriteProperty("name", "Test\nValue");
                AssertEx.Contains("Test\\nValue", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WriteStringEscapesCarriageReturn()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WriteProperty("name", "Test\rValue");
                AssertEx.Contains("Test\\rValue", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        [TestMethod]
        public void WriteStringEscapesHorizontalTab()
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                var JsonSerializationWriter = new TestableJsonSerializationWriter(stringWriter);
                JsonSerializationWriter.WriteProperty("name", "Test\tValue");
                AssertEx.Contains("Test\\tValue", stringWriter.ToString(), StringComparison.OrdinalIgnoreCase);
            }
        }

        #endregion

        private class TestableJsonSerializationWriter : JsonSerializationWriter
        {
            public TestableJsonSerializationWriter(TextWriter textWriter)
                : base(textWriter)
            {
            }

            public new void WriteString(string value)
            {
                base.WriteString(value);
            }
        }
    }
}

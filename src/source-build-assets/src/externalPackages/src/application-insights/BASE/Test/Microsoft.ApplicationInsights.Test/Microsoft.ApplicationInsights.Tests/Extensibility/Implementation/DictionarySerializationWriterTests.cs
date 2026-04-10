namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DictionarySerializationWriterTests
    {
        [TestMethod]
        public void WritesStringProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", "value");

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "value"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleStringProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", "value");
            dsw.WriteProperty("anotherName", "anotherValue");

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "value"},
                { "anotherName", "anotherValue"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullStringProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", (string)null);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForStringProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, "value"));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, "value"));
        }

        [TestMethod]
        public void WritesDoubleProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", 1.2);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "1.2"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleDoubleProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", 1.2);
            dsw.WriteProperty("anotherName", 2.1);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "1.2"},
                { "anotherName", "2.1"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullDoubleProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", (double?)null);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForDoubleProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, 1.2));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, 1.2));
        }


        [TestMethod]
        public void WritesIntProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", 12);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "12"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleIntProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", 12);
            dsw.WriteProperty("anotherName", 21);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "12"},
                { "anotherName", "21"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullIntProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", (int?)null);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForIntProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, 12));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, 12));
        }

        [TestMethod]
        public void WritesBoolProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", true);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "True"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleBoolProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", true);
            dsw.WriteProperty("anotherName", false);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", "True"},
                { "anotherName", "False"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullBoolProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name", (bool?)null);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForBoolProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, true));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, false));
        }

        [TestMethod]
        public void WritesTimespanProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            TimeSpan value = new TimeSpan(1, 2, 3, 4, 5);

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", value.ToString()}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleTimespanProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            TimeSpan value = new TimeSpan(1, 2, 3, 4, 5);
            TimeSpan anotherValue = new TimeSpan(5, 4, 3, 2, 1);

            dsw.WriteProperty("name", value);
            dsw.WriteProperty("anotherName", anotherValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", value.ToString()},
                { "anotherName", anotherValue.ToString()}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullTimespanProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            TimeSpan? value = null;

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForTimeSpanProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            TimeSpan value = new TimeSpan(1, 2, 3, 4, 5);

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, value));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, value));
        }

        [TestMethod]
        public void WritesTimeOffsetProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            DateTimeOffset value = new DateTimeOffset();

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", value.ToString(CultureInfo.InvariantCulture)}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }
        
        [TestMethod]
        public void WritesMultipleTimeOffsetProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            DateTimeOffset value = new DateTimeOffset();
            DateTimeOffset anotherValue = new DateTimeOffset() + new TimeSpan(1,0,0);

            dsw.WriteProperty("name", value);
            dsw.WriteProperty("anotherName", anotherValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", value.ToString(CultureInfo.InvariantCulture)},
                { "anotherName", anotherValue.ToString(CultureInfo.InvariantCulture)}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullTimeOffsetProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            DateTimeOffset? value = null;

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForDateTimeOffsetProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            DateTimeOffset value = new DateTimeOffset();

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, value));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, value));
        }

        [TestMethod]
        public void WritesListProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            List<string> value = new List<string> { "value12", "value21" };

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name0", "value12" },
                { "name1", "value21" },
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleListProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            List<string> value = new List<string> { "value12", "value21" };
            List<string> anotherValue = new List<string> { "value34", "value43" };

            dsw.WriteProperty("name", value);
            dsw.WriteProperty("anotherName", anotherValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name0", "value12" },
                { "name1", "value21" },
                { "anotherName0", "value34" },
                { "anotherName1", "value43" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullListProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            List<string> value = null;

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                {"name", null }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesListPropertyWithNullInTheList()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            List<string> value = new List<string> { "value12", null };

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name0", "value12" },
                { "name1", null },
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForListProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            List<string> value = new List<string> { "value12", "value21" };

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, value));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, value));
        }

        [TestMethod]
        public void WritesDictionaryProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Dictionary<string, string> value = new Dictionary<string, string>
            {
                { "key1", "value12" },
                { "key2", "value21" }
            };

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name.key1", "value12" },
                { "name.key2", "value21" },
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleDictionaryProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Dictionary<string, string> value = new Dictionary<string, string>
            {
                { "key1", "value12" },
                { "key2", "value21" }
            };

            Dictionary<string, string> anotherValue = new Dictionary<string, string>
            {
                { "key1", "value34" },
                { "key2", "value43" }
            };

            dsw.WriteProperty("name", value);
            dsw.WriteProperty("anotherName", anotherValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name.key1", "value12" },
                { "name.key2", "value21" },
                { "anotherName.key1", "value34" },
                { "anotherName.key2", "value43" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullDictionaryProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Dictionary<string, string> value = null;

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name", null }                
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesDictionaryPropertyWithNullInDictionary()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Dictionary<string, string> value = new Dictionary<string, string>
            {
                { "key1", null },
                { "key2", "value21" }
            };

            dsw.WriteProperty("name", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name.key1", null },
                { "name.key2", "value21" },
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForDictionaryProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Dictionary<string, string> value = new Dictionary<string, string>
            {
                { "key1", "value12" },
                { "key2", "value21" }
            };

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, value));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, value));
        }

        [TestMethod]
        public void WritesISerializableProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };

            dsw.WriteProperty("Serializable", value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "Serializable.Field3", "Value1" },
                { "Serializable.Field4", "42.42" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleISerializableProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };
            MySubSubExtension anotherValue = new MySubSubExtension { Field3 = "Value2", Field4 = 24.24 };

            dsw.WriteProperty("Serializable", value);
            dsw.WriteProperty("Serializable2", anotherValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "Serializable.Field3", "Value1" },
                { "Serializable.Field4", "42.42" },
                { "Serializable2.Field3", "Value2" },
                { "Serializable2.Field4", "24.24" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullISerializableProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();            
            MySubSubExtension value = null;

            dsw.WriteProperty("Serializable", value);            

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "Serializable", null }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesSerializablePropertyWithNullInside()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = null, Field4 = 42.42 };            

            dsw.WriteProperty("Serializable", value);            

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {                
                { "Serializable.Field3", null },
                { "Serializable.Field4", "42.42" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForISerializableProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, value));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, value));
        }

        [TestMethod]
        public void WritesListISerializableProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };
            MySubSubExtension anotherValue = new MySubSubExtension { Field3 = "Value2", Field4 = 24.24 };
            List<ISerializableWithWriter> listValue = new List<ISerializableWithWriter>
            {
                value,
                anotherValue
            };

            dsw.WriteProperty("SerializableList", listValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "SerializableList0.Field3", "Value1" },
                { "SerializableList0.Field4", "42.42" },
                { "SerializableList1.Field3", "Value2" },
                { "SerializableList1.Field4", "24.24" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleListISerializableProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };
            MySubSubExtension anotherValue = new MySubSubExtension { Field3 = "Value2", Field4 = 24.24 };
            List<ISerializableWithWriter> listValue = new List<ISerializableWithWriter>
            {
                value,
                anotherValue
            };
            List<ISerializableWithWriter> anotherListValue = new List<ISerializableWithWriter>
            {
                anotherValue,
                value
            };

            dsw.WriteProperty("SerializableList", listValue);
            dsw.WriteProperty("anotherSerializableList", anotherListValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "SerializableList0.Field3", "Value1" },
                { "SerializableList0.Field4", "42.42" },
                { "SerializableList1.Field3", "Value2" },
                { "SerializableList1.Field4", "24.24" },
                { "anotherSerializableList0.Field3", "Value2" },
                { "anotherSerializableList0.Field4", "24.24" },
                { "anotherSerializableList1.Field3", "Value1" },
                { "anotherSerializableList1.Field4", "42.42" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesNullListISerializableProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();            
            List<ISerializableWithWriter> listValue = null;

            dsw.WriteProperty("SerializableList", listValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "SerializableList", null }                
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesListISerializablePropertyWithNullInsideISerializableList()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "value", Field4 = 42.42 };
            List<ISerializableWithWriter> listValue = new List<ISerializableWithWriter>
            {
                value,
                null
            };

            dsw.WriteProperty("SerializableList", listValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "SerializableList0.Field3", "value" },
                { "SerializableList0.Field4", "42.42" },
                { "SerializableList1", null }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfNameIsNullOrEmptyForListISerializableProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };
            MySubSubExtension anotherValue = new MySubSubExtension { Field3 = "Value2", Field4 = 24.24 };
            List<ISerializableWithWriter> listValue = new List<ISerializableWithWriter>
            {
                value,
                anotherValue
            };

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, listValue));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, listValue));
        }

        [TestMethod]
        public void WritesISerializableNoNameProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };

            dsw.WriteProperty(value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { DictionarySerializationWriter.DefaultKey + "1.Field3", "Value1" },
                { DictionarySerializationWriter.DefaultKey + "1.Field4", "42.42" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultipleISerializableNoNameProperties()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = "Value1", Field4 = 42.42 };
            MySubSubExtension anotherValue = new MySubSubExtension { Field3 = "Value2", Field4 = 24.24 };

            dsw.WriteProperty(value);
            dsw.WriteProperty(anotherValue);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { DictionarySerializationWriter.DefaultKey + "1.Field3", "Value1" },
                { DictionarySerializationWriter.DefaultKey + "1.Field4", "42.42" },
                { DictionarySerializationWriter.DefaultKey + "2.Field3", "Value2" },
                { DictionarySerializationWriter.DefaultKey + "2.Field4", "24.24" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void DoesNotWriteNullISerializableNoNameProperty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = null;

            dsw.WriteProperty(value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>(); // Generated name would not indicate which object is missing, hence useless, returning empty dictionary
            
            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesISerializableNoNamePropertyWithNullInside()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            MySubSubExtension value = new MySubSubExtension { Field3 = null, Field4 = 42.42 };

            dsw.WriteProperty(value);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { DictionarySerializationWriter.DefaultKey + "1.Field3", null },
                { DictionarySerializationWriter.DefaultKey + "1.Field4", "42.42" }
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMeasurements()
        {
            Dictionary<string, double> metrics = new Dictionary<string, double>
            {
                { "Metric1", 1.2 },
                { "Metric2", 2.1 }
            };

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("Metrics", metrics);

            Dictionary<string, double> excpectedSerialization = new Dictionary<string, double>()
            // Unlike properties, the absence of metric does not indicate something
            {
                { "Metrics.Metric1", 1.2},
                { "Metrics.Metric2", 2.1}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedMeasurements);
            AssertEx.IsEmpty(dsw.AccumulatedDictionary);
        }

        [TestMethod]
        public void WritesMultipleMeasurements()
        {
            Dictionary<string, double> metrics = new Dictionary<string, double>
            {
                { "Metric1", 1.2 },
                { "Metric2", 2.1 }
            };

            Dictionary<string, double> otherMetrics = new Dictionary<string, double>
            {
                { "Metric1", 3.4 },
                { "Metric2", 4.3 }
            };

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("Metrics", metrics);
            dsw.WriteProperty("otherMetrics", otherMetrics);

            Dictionary<string, double> excpectedSerialization = new Dictionary<string, double>()
            {
                { "Metrics.Metric1", 1.2},
                { "Metrics.Metric2", 2.1},
                { "otherMetrics.Metric1", 3.4},
                { "otherMetrics.Metric2", 4.3},
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedMeasurements);
            AssertEx.IsEmpty(dsw.AccumulatedDictionary);
        }

        [TestMethod]
        public void DoesNotWriteNullMeasurements()
        {
            Dictionary<string, double> metrics = null;            

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("Metrics", metrics);

            Dictionary<string, double> excpectedSerialization = new Dictionary<string, double>();

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedMeasurements);
            AssertEx.IsEmpty(dsw.AccumulatedDictionary);
        }

        [TestMethod]
        public void ThrowsIfNameIsNullOrEmptyForMeasurements()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Dictionary<string, double> metrics = new Dictionary<string, double>
            {
                { "Metric1", 1.2 },
                { "Metric2", 2.1 }
            };

            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(null, metrics));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteProperty(string.Empty, metrics));
        }

        [TestMethod]
        public void IndentsKeyWhenWritesStartObject()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteStartObject("MyObject");
            dsw.WriteProperty("name", "value");
            dsw.WriteEndObject();

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "MyObject.name", "value"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void IndentsKeyWhenWritesStartObjectWithNoName()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteStartObject();
            dsw.WriteProperty("name", "value");
            dsw.WriteEndObject();
            
            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { DictionarySerializationWriter.DefaultObjectKey + "1.name", "value"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void IndentsKeyMultipleTimesWhenWritesMultipleStartObject()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteStartObject("First");
            dsw.WriteProperty("name", "value");
            dsw.WriteStartObject("Second");
            dsw.WriteProperty("name", "value");
            dsw.WriteEndObject();
            dsw.WriteEndObject();

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "First.name", "value"},
                { "First.Second.name", "value"}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]        
        public void ThrowsIfIndentNameIsNullOrEmpty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteStartObject(null));
            Assert.ThrowsException<ArgumentException>(() => dsw.WriteStartObject(string.Empty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowsIfIndentNameIsEmpty()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteStartObject(string.Empty);
        }

        [TestMethod]
        public void WritesComplexSerializableObject()
        {
            DictionarySerializationWriter dsw = new DictionarySerializationWriter();

            var complexExtension = new ComplexExtension();
            var mySubSubExtension1 = new MySubSubExtension() { Field3 = "Value1", Field4 = 100.00 };
            var mySubSubExtension2 = new MySubSubExtension() { Field3 = "Value2", Field4 = 200.00 };
            var mySubExtension1 = new MySubExtension() { Field1 = "Value1", Field2 = 100, MySubSubExtension = mySubSubExtension1 };
            var mySubExtension2 = new MySubExtension() { Field1 = "Value2", Field2 = 200, MySubSubExtension = mySubSubExtension2 };
            var listExtension = new List<MySubExtension>();
            listExtension.Add(mySubExtension1);
            listExtension.Add(mySubExtension2);

            var listString = new List<string>();
            listString.Add("Item1");
            listString.Add("Item2");
            listString.Add("Item3");

            complexExtension.MyBoolField = true;
            DateTimeOffset testOffsetValue = DateTimeOffset.Now;
            complexExtension.MyDateTimeOffsetField = testOffsetValue;
            complexExtension.MyDoubleField = 100.1;
            complexExtension.MyIntField = 100;
            complexExtension.MyStringField = "ValueString";
            complexExtension.MyTimeSpanField = TimeSpan.FromSeconds(2);
            complexExtension.MySubExtensionField = mySubExtension1;
            complexExtension.MyExtensionListField = listExtension;
            complexExtension.MyStringListField = listString;

            var dicString = new Dictionary<string, string>();
            dicString.Add("Item1", "Value1");
            dicString.Add("Item2", "Value2");
            complexExtension.MyStringDictionaryField = dicString;

            var dicDouble = new Dictionary<string, double>();
            dicDouble.Add("Item1", 1000.0);
            dicDouble.Add("Item2", 2000.0);
            complexExtension.MyDoubleDictionaryField = dicDouble;

            dsw.WriteProperty("name", complexExtension);

            Dictionary<string, string> excpectedSerialization = new Dictionary<string, string>()
            {
                { "name.MyStringField", "ValueString" },
                { "name.MyIntField", "100" },
                { "name.MyDoubleField", "100.1" },
                { "name.MyBoolField", "True" },
                { "name.MyTimeSpanField", TimeSpan.FromSeconds(2).ToString()},
                { "name.MyDateTimeOffsetField", testOffsetValue.ToString(CultureInfo.InvariantCulture)},
                { "name.MySubExtensionField.Field1", "Value1"},
                { "name.MySubExtensionField.Field2", "100"},
                { "name.MySubExtensionField.MySubSubExtension.Field3", "Value1"},
                { "name.MySubExtensionField.MySubSubExtension.Field4", "100"},
                { "name.MyStringListField0", "Item1"},
                { "name.MyStringListField1", "Item2"},
                { "name.MyStringListField2", "Item3"},
                { "name.MyExtensionListField0.Field1", "Value1"},
                { "name.MyExtensionListField0.Field2", "100"},
                { "name.MyExtensionListField0.MySubSubExtension.Field3", "Value1"},
                { "name.MyExtensionListField0.MySubSubExtension.Field4", "100"},
                { "name.MyExtensionListField1.Field1", "Value2"},
                { "name.MyExtensionListField1.Field2", "200"},
                { "name.MyExtensionListField1.MySubSubExtension.Field3", "Value2"},
                { "name.MyExtensionListField1.MySubSubExtension.Field4", "200"},
                { "name.MyStringDictionaryField.Item1", "Value1"},
                { "name.MyStringDictionaryField.Item2", "Value2"}
            };

            Dictionary<string, double> excpectedSerializationMetrics = new Dictionary<string, double>()
            {
                { "name.MyDoubleDictionaryField.Item1", 1000.000},
                { "name.MyDoubleDictionaryField.Item2", 2000.000}
            };

            AssertEx.AreEqual(excpectedSerialization, dsw.AccumulatedDictionary);
            AssertEx.AreEqual(excpectedSerializationMetrics, dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void DoesNowWriteDuplicateName()
        {
            TimeSpan ts = new TimeSpan(1, 2, 3);
            DateTimeOffset dto = new DateTimeOffset();
            List<string> strings = new List<string> { "ListValue", "ListAnotherValue" };
            Dictionary<string, string> dict = new Dictionary<string, string> { { "name", "value" }, { "anotherName", "anotherValue"} };
            MySubSubExtension ext = new MySubSubExtension() { Field3 = "extValue", Field4 = 1.2 };
            List<ISerializableWithWriter> extensions = new List<ISerializableWithWriter> { ext, ext };

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            dsw.WriteProperty("name.dupe", "value");
            dsw.WriteProperty("name.dupe", "value");
            dsw.WriteProperty("name.dupe", 1.2);
            dsw.WriteProperty("name.dupe", 12);
            dsw.WriteProperty("name.dupe", true);
            dsw.WriteProperty("name.dupe", ts);
            dsw.WriteProperty("name.dupe", dto);
            dsw.WriteProperty("name.dupe0", "value"); // Dupe of the list entries below
            dsw.WriteProperty("name.dupe1", "value"); // Dupe of the list entries below
            dsw.WriteProperty("name.dupe", strings);            
            dsw.WriteProperty("name.dupe.name", "value"); // Dupe of the dict entry below
            dsw.WriteProperty("name.dupe.anotherName", "value"); // Dupe of the dict entry below
            dsw.WriteProperty("name.dupe", dict);
            dsw.WriteProperty("name.dupe.Field3", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe.Field4", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe", ext);
            dsw.WriteProperty("name.dupe0.Field3", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe0.Field4", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe1.Field3", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe1.Field4", "value"); // Dupe of the extensions list entry below
            dsw.WriteProperty("name.dupe", extensions);

            dsw.WriteProperty(DictionarySerializationWriter.DefaultKey + "1.Field3", "value"); // Dupe of no-name extension entry below
            dsw.WriteProperty(DictionarySerializationWriter.DefaultKey + "1.Field4", "value"); // Dupe of no-name extension entry below
            dsw.WriteProperty(ext);

            // Setup nested default key to test no-name duplication inside WriteObject
            dsw.WriteProperty("name.dupe." + DictionarySerializationWriter.DefaultKey + "1.Field3", "value");
            dsw.WriteProperty("name.dupe." + DictionarySerializationWriter.DefaultKey + "1.Field4", "value");

            dsw.WriteStartObject("name");
            dsw.WriteProperty("dupe", "value");
            dsw.WriteProperty("dupe", 1.2);
            dsw.WriteProperty("dupe", 12);
            dsw.WriteProperty("dupe", true);
            dsw.WriteProperty("dupe", ts);
            dsw.WriteProperty("dupe", dto);
            dsw.WriteProperty("dupe", strings);
            dsw.WriteProperty("dupe", dict);
            dsw.WriteProperty("dupe", ext);
            dsw.WriteProperty("dupe", extensions);
            dsw.WriteStartObject("dupe");
            dsw.WriteProperty(ext);
            dsw.WriteEndObject(); // dupe
            dsw.WriteEndObject(); // name

            Dictionary<string, double> metrics = new Dictionary<string, double> { { "metric", 1.2 } };
            dsw.WriteProperty("name.dupe", metrics);
            dsw.WriteProperty("name.dupe", metrics);

            Dictionary<string, string> expectedProperties = new Dictionary<string, string>
            {
                { "name.dupe", "value"},
                { "name.dupe0", "value"},
                { "name.dupe1", "value"},
                { "name.dupe.name", "value"},
                { "name.dupe.anotherName", "value"},
                { "name.dupe.Field3", "value"},
                { "name.dupe.Field4", "value"},
                { "name.dupe0.Field3", "value"},
                { "name.dupe0.Field4", "value"},
                { "name.dupe1.Field3", "value"},
                { "name.dupe1.Field4", "value"},
                { DictionarySerializationWriter.DefaultKey + "1.Field3", "value"},
                { DictionarySerializationWriter.DefaultKey + "1.Field4", "value"},
                { "name.dupe." + DictionarySerializationWriter.DefaultKey + "1.Field3", "value"},
                { "name.dupe." + DictionarySerializationWriter.DefaultKey + "1.Field4", "value"}
            };

            Dictionary<string, double> expectedMetrics = new Dictionary<string, double>
            {
                { "name.dupe.metric", 1.2}
            };

            AssertEx.AreEqual(expectedProperties, dsw.AccumulatedDictionary);
            AssertEx.AreEqual(expectedMetrics, dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultiLevelObjectWithVariableDepths()
        {
            // Trying to serialize an object with back and forth depth:
            //{
            //    "name" : "value1"
            //    "depth1" : {
            //        "name" : "value2"
            //        "depth2" : {
            //            "name": "value3"
            //            "key" : "value4"
            //        }
            //        "key" : "value5",
            //        "depth2again" : {
            //            "name" : "value6"
            //            "depth3" :{
            //                "name" : "value7"
            //            }
            //            "key":"value8"
            //        }
            //        "item" : "value9"
            //    }
            //    "key" : "value10"
            //    "item" : "value11"
            //    "depth1again" : {
            //        "depth2again" : {
            //            "depth3again" : {
            //                "name" : "value12"
            //            }
            //            "key" : "value13"
            //        }
            //        "item" : "value14"
            //    }
            //}

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            
            dsw.WriteProperty("name", "value1");
            dsw.WriteStartObject("depth1");
                dsw.WriteProperty("name", "value2");
                dsw.WriteStartObject("depth2");
                    dsw.WriteProperty("name", "value3");
                    dsw.WriteProperty("key", "value4");
                dsw.WriteEndObject();
                dsw.WriteProperty("key", "value5");
                dsw.WriteStartObject("depth2again");
                    dsw.WriteProperty("name", "value6");
                    dsw.WriteStartObject("depth3");
                        dsw.WriteProperty("name", "value7");
                    dsw.WriteEndObject();
                    dsw.WriteProperty("key", "value8");
                dsw.WriteEndObject();
                dsw.WriteProperty("item", "value9");
            dsw.WriteEndObject();
            dsw.WriteProperty("key", "value10");
            dsw.WriteProperty("item", "value11");
            dsw.WriteStartObject("depth1again");
                dsw.WriteStartObject("depth2again");
                    dsw.WriteStartObject("depth3again");
                        dsw.WriteProperty("name", "value12");
                    dsw.WriteEndObject();
                    dsw.WriteProperty("key", "value13");
                dsw.WriteEndObject();
                dsw.WriteProperty("item", "value14");
            dsw.WriteEndObject();

            Dictionary<string, string> expectedDictionary = new Dictionary<string, string>
            {
                {"name", "value1" },
                {"depth1.name", "value2" },
                {"depth1.depth2.name", "value3" },
                {"depth1.depth2.key", "value4" },
                {"depth1.key", "value5" },
                {"depth1.depth2again.name", "value6" },
                {"depth1.depth2again.depth3.name", "value7" },
                {"depth1.depth2again.key", "value8" },
                {"depth1.item", "value9" },
                {"key", "value10" },
                {"item", "value11" },
                {"depth1again.depth2again.depth3again.name", "value12" },
                {"depth1again.depth2again.key", "value13" },
                {"depth1again.item", "value14" }
            };

            AssertEx.AreEqual(expectedDictionary, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }

        [TestMethod]
        public void WritesMultiLevelObjectWithVariableDepthsAndDynamicNames()
        {
            // Trying to serialize an object with back and forth depth:
            //{
            //    "name" : "value1"
            //    {
            //        "name" : "value2"
            //        {
            //            "name": "value3"
            //            "key" : "value4"
            //        }
            //        "key" : "value5",
            //        {
            //            "name" : "value6"
            //            {
            //                "name" : "value7"
            //            }
            //            "key":"value8"
            //        }
            //        "item" : "value9"
            //    }
            //    "key" : "value10"
            //    "item" : "value11"
            //    {
            //        {
            //            {
            //                "name" : "value12"
            //            }
            //            "key" : "value13"
            //        }
            //        "item" : "value14"
            //    }
            //}

            DictionarySerializationWriter dsw = new DictionarySerializationWriter();
            
            dsw.WriteProperty("name", "value1");
            dsw.WriteStartObject();
                dsw.WriteProperty("name", "value2");
                dsw.WriteStartObject();
                    dsw.WriteProperty("name", "value3");
                    dsw.WriteProperty("key", "value4");
                dsw.WriteEndObject();
                dsw.WriteProperty("key", "value5");
                dsw.WriteStartObject();
                    dsw.WriteProperty("name", "value6");
                    dsw.WriteStartObject();
                        dsw.WriteProperty("name", "value7");
                    dsw.WriteEndObject();
                    dsw.WriteProperty("key", "value8");
                dsw.WriteEndObject();
                dsw.WriteProperty("item", "value9");
            dsw.WriteEndObject();
            dsw.WriteProperty("key", "value10");
            dsw.WriteProperty("item", "value11");
            dsw.WriteStartObject();
                dsw.WriteStartObject();
                    dsw.WriteStartObject();
                        dsw.WriteProperty("name", "value12");
                    dsw.WriteEndObject();
                    dsw.WriteProperty("key", "value13");
                dsw.WriteEndObject();
                dsw.WriteProperty("item", "value14");
            dsw.WriteEndObject();
            

            Dictionary<string, string> expectedDictionary = new Dictionary<string, string>
            {
                {"name", "value1" },
                {"Obj1.name", "value2" },
                {"Obj1.Obj1.name", "value3" },
                {"Obj1.Obj1.key", "value4" },
                {"Obj1.key", "value5" },
                {"Obj1.Obj2.name", "value6" },
                {"Obj1.Obj2.Obj1.name", "value7" },
                {"Obj1.Obj2.key", "value8" },
                {"Obj1.item", "value9" },
                {"key", "value10" },
                {"item", "value11" },
                {"Obj2.Obj1.Obj1.name", "value12" },
                {"Obj2.Obj1.key", "value13" },
                {"Obj2.item", "value14" }
            };

            AssertEx.AreEqual(expectedDictionary, dsw.AccumulatedDictionary);
            AssertEx.IsEmpty(dsw.AccumulatedMeasurements);
        }
    }
}

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class MyTestExtension : IExtension
    {
        public int myIntField;
        public string myStringField;

        public IExtension DeepClone()
        {
            var other = new MyTestExtension();
            other.myIntField = this.myIntField;
            other.myStringField = this.myStringField;

            return other;
        }

        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("myIntField", myIntField);
            serializationWriter.WriteProperty("myStringField", myStringField);
        }

        public Dictionary<string, string> SerializeIntoDictionary()
        {
            return new Dictionary<string, string>{
                { "myIntField", this.myIntField.ToString()},
                { "myStringField", this.myStringField}
            };
        }
    }

    public class ComplexExtension : ISerializableWithWriter
    {
        public string MyStringField;
        public int MyIntField;
        public double MyDoubleField;
        public bool MyBoolField;
        public TimeSpan MyTimeSpanField;
        public DateTimeOffset MyDateTimeOffsetField;
        public MySubExtension MySubExtensionField;
        public IList<string> MyStringListField;
        public IList<MySubExtension> MyExtensionListField;
        public IDictionary<string, string> MyStringDictionaryField;
        public IDictionary<string, double> MyDoubleDictionaryField;

        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("MyStringField", MyStringField);
            serializationWriter.WriteProperty("MyIntField", MyIntField);
            serializationWriter.WriteProperty("MyDoubleField", MyDoubleField);
            serializationWriter.WriteProperty("MyBoolField", MyBoolField);
            serializationWriter.WriteProperty("MyTimeSpanField", MyTimeSpanField);
            serializationWriter.WriteProperty("MyDateTimeOffsetField", MyDateTimeOffsetField);
            serializationWriter.WriteProperty("MySubExtensionField", MySubExtensionField);
            serializationWriter.WriteProperty("MyStringListField", MyStringListField);
            serializationWriter.WriteProperty("MyExtensionListField", MyExtensionListField.ToList<ISerializableWithWriter>());
            serializationWriter.WriteProperty("MyStringDictionaryField", MyStringDictionaryField);
            serializationWriter.WriteProperty("MyDoubleDictionaryField", MyDoubleDictionaryField);
        }
    }

    public class MySubExtension : ISerializableWithWriter
    {
        public string Field1;
        public int Field2;
        public ISerializableWithWriter MySubSubExtension;

        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("Field1", Field1);
            serializationWriter.WriteProperty("Field2", Field2);
            serializationWriter.WriteProperty("MySubSubExtension", MySubSubExtension);
        }
    }

    public class MySubSubExtension : ISerializableWithWriter
    {
        public string Field3;
        public double Field4;

        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("Field3", Field3);
            serializationWriter.WriteProperty("Field4", Field4);
        }
    }
}

namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The interface for defining writers capable of serializing data into various formats.    
    /// </summary>
    public interface ISerializationWriter
    {
        /// <summary>
        /// Writes name and value for a string field.
        /// </summary>
        void WriteProperty(string name, string value);

        /// <summary>
        /// Writes name and value for a double field.
        /// </summary>
        void WriteProperty(string name, double? value);

        /// <summary>
        /// Writes name and value for a int field.
        /// </summary>
        void WriteProperty(string name, int? value);

        /// <summary>
        /// Writes name and value for a boolean field.
        /// </summary>
        void WriteProperty(string name, bool? value);

        /// <summary>
        /// Writes name and value for a TimeSpan field.
        /// </summary>
        void WriteProperty(string name, TimeSpan? value);

        /// <summary>
        /// Writes name and value for a DateTimeOffset field.
        /// </summary>
        void WriteProperty(string name, DateTimeOffset? value);

        /// <summary>
        /// Writes name and value for a ISerializableWithWriter field.
        /// </summary>
        void WriteProperty(string name, ISerializableWithWriter value);

        /// <summary>
        /// Writes value ISerializableWithWriter field.
        /// </summary>
        void WriteProperty(ISerializableWithWriter value);

        /// <summary>
        /// Writes name and values for a IList field of strings.
        /// </summary>
        void WriteProperty(string name, IList<string> items);

        /// <summary>
        /// Writes name and values for a IList field of objects implementing ISerializableWithWriter.
        /// </summary>
        void WriteProperty(string name, IList<ISerializableWithWriter> items);

        /// <summary>
        /// Writes name and value for a IDictionary field with string,string as key,value.
        /// </summary>
        void WriteProperty(string name, IDictionary<string, string> items);

        /// <summary>
        /// Writes name and value for a IDictionary field with string,string as key,value.
        /// </summary>
        void WriteProperty(string name, IDictionary<string, double> items);

        /// <summary>
        /// Marks beginning of a complex object.
        /// </summary>
        void WriteStartObject(string name);

        /// <summary>
        /// Marks beginning of a complex object.
        /// </summary>
        void WriteStartObject();

        /// <summary>
        /// Marks ending of a complex object.
        /// </summary>
        void WriteEndObject();
    }
}
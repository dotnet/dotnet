namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{    
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Microsoft.ApplicationInsights.Extensibility;

    internal class JsonSerializationWriter : ISerializationWriter
    {
        private readonly TextWriter textWriter;
        private bool currentObjectHasProperties;

        public JsonSerializationWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;            
        }

        /// <inheritdoc/>
        public void WriteStartObject()
        {        
            this.textWriter.Write('{');
            this.currentObjectHasProperties = false;
        }

        /// <inheritdoc/>
        public void WriteStartObject(string name)
        {
            this.WritePropertyName(name);
            this.textWriter.Write('{');
            this.currentObjectHasProperties = false;
        }        

        /// <inheritdoc/>
        public void WriteProperty(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                this.WritePropertyName(name);
                this.WriteString(value);
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, int? value)
        {
            if (value.HasValue)
            {
                this.WritePropertyName(name);
                this.textWriter.Write(value.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, bool? value)
        {
            if (value.HasValue)
            {
                this.WritePropertyName(name);
                this.textWriter.Write(value.Value ? "true" : "false");
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, double? value)
        {
            if (value.HasValue)
            {
                this.WritePropertyName(name);
                this.textWriter.Write(value.Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, TimeSpan? value)
        {
            if (value.HasValue)
            {
                this.WriteProperty(name, value.Value.ToString(string.Empty, CultureInfo.InvariantCulture));
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, DateTimeOffset? value)
        {
            if (value.HasValue)
            {
                this.WriteProperty(name, value.Value.ToString("o", CultureInfo.InvariantCulture));
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, IList<string> items)
        {
            bool commaNeeded = false;
            if (items != null && items.Count > 0)
            {
                this.WritePropertyName(name);

                this.WriteStartArray();

                foreach (var item in items)
                {
                    if (commaNeeded)
                    {
                        this.WriteComma();
                    }

                    this.WriteString(item);
                    commaNeeded = true;
                }

                this.WriteEndArray();
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, IList<ISerializableWithWriter> items)
        {
            bool commaNeeded = false;
            if (items != null && items.Count > 0)
            {
                this.WritePropertyName(name);
                this.WriteStartArray();
                foreach (var item in items)
                {                    
                    if (commaNeeded)
                    {
                        this.WriteComma();
                    }

                    this.WriteStartObject();
                    item.Serialize(this);
                    commaNeeded = true;
                    this.WriteEndObject();
                }

                this.WriteEndArray();                
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, ISerializableWithWriter value)
        {
            if (value != null)
            {
                this.WriteStartObject(name);
                value.Serialize(this);
                this.WriteEndObject();
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(ISerializableWithWriter value)
        {
            if (value != null)
            {                
                value.Serialize(this);                
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, IDictionary<string, double> values)
        {
            if (values != null && values.Count > 0)
            {
                this.WritePropertyName(name);
                this.WriteStartObject();
                foreach (KeyValuePair<string, double> item in values)
                {
                    this.WriteProperty(item.Key, item.Value);
                }

                this.WriteEndObject();
            }
        }

        /// <inheritdoc/>
        public void WriteProperty(string name, IDictionary<string, string> values)
        {
            if (values != null && values.Count > 0)
            {
                this.WritePropertyName(name);
                this.WriteStartObject();
                foreach (KeyValuePair<string, string> item in values)
                {
                    this.WriteProperty(item.Key, item.Value);
                }

                this.WriteEndObject();
            }
        }

        /// <inheritdoc/>
        public void WriteEndObject()
        {
            this.textWriter.Write('}');
        }

        internal void WritePropertyName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.Length == 0)
            {
                throw new ArgumentException($"{nameof(name)} cannot be empty", nameof(name));
            }

            if (this.currentObjectHasProperties)
            {
                this.textWriter.Write(',');
            }
            else
            {
                this.currentObjectHasProperties = true;
            }

            this.WriteString(name);
            this.textWriter.Write(':');
        }

        internal void WriteStartArray()
        {
            this.textWriter.Write('[');
        }

        internal void WriteEndArray()
        {
            this.textWriter.Write(']');
        }

        internal void WriteComma()
        {
            this.textWriter.Write(',');
        }

        internal void WriteRawValue(object value)
        {
            this.textWriter.Write(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }

        internal void WriteString(string value)
        {
            this.textWriter.Write('"');

            foreach (char c in value)
            {
                switch (c)
                {
                    case '\\':
                        this.textWriter.Write("\\\\");
                        break;
                    case '"':
                        this.textWriter.Write("\\\"");
                        break;
                    case '\n':
                        this.textWriter.Write("\\n");
                        break;
                    case '\b':
                        this.textWriter.Write("\\b");
                        break;
                    case '\f':
                        this.textWriter.Write("\\f");
                        break;
                    case '\r':
                        this.textWriter.Write("\\r");
                        break;
                    case '\t':
                        this.textWriter.Write("\\t");
                        break;
                    default:
                        if (!char.IsControl(c))
                        {
                            this.textWriter.Write(c);
                        }
                        else
                        {
                            this.textWriter.Write(@"\u");
                            this.textWriter.Write(((ushort)c).ToString("x4", CultureInfo.InvariantCulture));
                        }

                        break;
                }
            }

            this.textWriter.Write('"');
        }
    }
}
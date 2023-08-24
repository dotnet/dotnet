// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Json
{
    internal static class JsonSerializerPrimitives
    {
        internal const int MaxDepth = 2;

        /// <summary>
        /// Creates a JsonException that provides information on what went wrong
        /// </summary>
        /// <param name="reader">the <see cref="Utf8JsonReader"/>.</param>
        /// <param name="expectedType">the type the reader was expecting to find.</param>
        /// <param name="className">the name of the type being read.</param>
        /// <param name="propertyName">the property name being read.</param>
        /// <param name="innerException">inner exception if any.</param>
        /// <returns></returns>
        public static JsonException CreateJsonReaderException(
            ref Utf8JsonReader reader,
            string expectedType,
            string className,
            string propertyName,
            Exception innerException = null)
        {
            if (innerException == null)
                return new JsonException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX11020,
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(expectedType),
                        LogHelper.MarkAsNonPII(className),
                        LogHelper.MarkAsNonPII(propertyName),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed)));
            else
                return new JsonException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX11020,
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(expectedType),
                        LogHelper.MarkAsNonPII(className),
                        LogHelper.MarkAsNonPII(propertyName),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed)),
                    innerException);
        }

        public static Exception CreateJsonReaderExceptionInvalidType(ref Utf8JsonReader reader, string expectedType, string className, string propertyName)
        {
            return new JsonException(
                LogHelper.FormatInvariant(
                    LogMessages.IDX11022,
                    LogHelper.MarkAsNonPII(expectedType),
                    LogHelper.MarkAsNonPII(reader.TokenType),
                    LogHelper.MarkAsNonPII(className),
                    LogHelper.MarkAsNonPII(propertyName),
                    LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                    LogHelper.MarkAsNonPII(reader.CurrentDepth),
                    LogHelper.MarkAsNonPII(reader.BytesConsumed)));
        }

        public static JsonElement CreateJsonElement(IList<string> strings)
        {
            using (MemoryStream memoryStream = new())
            {
                Utf8JsonWriter writer = null;
                try
                {
                    writer = new(memoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    writer.WriteStartArray();

                    foreach (string str in strings)
                        writer.WriteStringValue(str);

                    writer.WriteEndArray();
                    writer.Flush();

                    Utf8JsonReader reader = new(memoryStream.GetBuffer().AsSpan(0, (int)memoryStream.Length));

#if NET6_0_OR_GREATER
                    bool ret = JsonElement.TryParseValue(ref reader, out JsonElement? jsonElement);
                    return jsonElement.Value;
#else
                    using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
                        return jsonDocument.RootElement.Clone();
#endif
                }
                finally
                {
                    writer?.Dispose();
                }
            }
        }

        public static JsonElement CreateJsonElement(string json)
        {
            Utf8JsonReader reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json).AsSpan());

#if NET6_0_OR_GREATER
            bool ret = JsonElement.TryParseValue(ref reader, out JsonElement? jsonElement);
            return jsonElement.Value;
#else
            using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
                return jsonDocument.RootElement.Clone();
#endif
        }

        #region Read
        internal static bool IsReaderAtTokenType(ref Utf8JsonReader reader, JsonTokenType tokenType, bool advanceReader)
        {
            if (reader.TokenType == JsonTokenType.None)
                reader.Read();

            if (reader.TokenType != tokenType)
                return false;

            if (advanceReader)
                reader.Read();

            return true;
        }

        internal static bool ReadBoolean(ref Utf8JsonReader reader, string propertyName, string className, bool read = false)
        {
            if (read)
                reader.Read();

            if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
                return reader.GetBoolean();

            throw LogHelper.LogExceptionMessage(
                CreateJsonReaderException(ref reader, "JsonTokenType.False or JsonTokenType.True", className, propertyName));
        }

        internal static double ReadDouble(ref Utf8JsonReader reader, string propertyName, string className, bool read = false)
        {
            if (read)
                reader.Read();

            if (reader.TokenType == JsonTokenType.Number)
            {
                try
                {
                    return reader.GetDouble();
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(
                        CreateJsonReaderException(ref reader, typeof(double).ToString(), className, propertyName, ex));
                }
            }

            throw LogHelper.LogExceptionMessage(
                CreateJsonReaderException(ref reader, "JsonTokenType.Number", className, propertyName));
        }

        internal static int ReadInt(ref Utf8JsonReader reader, string propertyName, string className, bool read = false)
        {
            if (read)
                reader.Read();

            if (reader.TokenType == JsonTokenType.Number)
            {
                try
                {
                    return reader.GetInt32();
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(
                        CreateJsonReaderException(ref reader, typeof(int).ToString(), className, propertyName, ex));
                }
            }

            throw LogHelper.LogExceptionMessage(
                CreateJsonReaderException(ref reader, "JsonTokenType.Number", className, propertyName));
        }

        internal static JsonElement ReadJsonElement(ref Utf8JsonReader reader)
        {
#if NET6_0_OR_GREATER
            JsonElement? jsonElement;
            bool ret = JsonElement.TryParseValue(ref reader, out jsonElement);
            if (ret)
                return jsonElement.Value;

            return default;
#else
            using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
                return jsonDocument.RootElement.Clone();
#endif
        }

        internal static object ReadNumber(ref Utf8JsonReader reader)
        {

            if (reader.TryGetInt32(out int i))
                return i;
            else if (reader.TryGetInt64(out long l))
                return l;
            else if (reader.TryGetDouble(out double d))
                return d;
            else if (reader.TryGetUInt32(out uint u))
                return u;
            else if (reader.TryGetUInt64(out ulong ul))
                return ul;
            else if (reader.TryGetSingle(out float f))
                return f;
            else if (reader.TryGetDecimal(out decimal m))
                return m;

            Debug.Assert(false, "expected to read a number, but none of the Utf8JsonReader.TryGet... methods returned true.");

            return ReadJsonElement(ref reader);
        }

        internal static IList<object> ReadArrayOfObjects(ref Utf8JsonReader reader, string propertyName, string className)
        {
            // returning null keeps the same logic as JsonSerialization.ReadObject
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            List<object> objects = new();
            if (!IsReaderAtTokenType(ref reader, JsonTokenType.StartArray, false))
                throw LogHelper.LogExceptionMessage(
                    CreateJsonReaderExceptionInvalidType(ref reader, "JsonTokenType.StartArray", className, propertyName));

            while (reader.Read())
            {
                if (IsReaderAtTokenType(ref reader, JsonTokenType.EndArray, false))
                    break;

                objects.Add(ReadPropertyValueAsObject(ref reader, propertyName, className));
            }

            return objects;
        }

        internal static string ReadPropertyName(ref Utf8JsonReader reader, string className, bool advanceReader)
        {
            if (reader.TokenType == JsonTokenType.None)
                reader.Read();

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw LogHelper.LogExceptionMessage(CreateJsonReaderExceptionInvalidType(ref reader, "JsonTokenType.PropertyName", string.Empty, className));

            if (advanceReader)
            {
                string propertyName = reader.GetString();
                reader.Read();
                return propertyName;
            }

            return reader.GetString();
        }

        internal static string ReadString(ref Utf8JsonReader reader, string propertyName, string className, bool read = false)
        {
            if (read)
                reader.Read();

            // returning null keeps the same logic as JsonSerialization.ReadObject
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType != JsonTokenType.String)
                throw LogHelper.LogExceptionMessage(
                    CreateJsonReaderException(ref reader, "JsonTokenType.String", className, propertyName));

            return reader.GetString();
        }

        internal static object ReadStringAsObject(ref Utf8JsonReader reader, string propertyName, string className, bool read = false)
        {
            if (read)
                reader.Read();

            // returning null keeps the same logic as JsonSerialization.ReadObject
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType != JsonTokenType.String)
                throw LogHelper.LogExceptionMessage(
                    CreateJsonReaderException(ref reader, "JsonTokenType.String", className, propertyName));

            string originalString = reader.GetString();
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                // if (reader.TryGetDateTime(out DateTime dateTimeValue))
                // has thrown on escaped chars and empty chars
                // try catch for safety
                if (DateTime.TryParse(originalString, out DateTime dateTimeValue))
                {
                    dateTimeValue = dateTimeValue.ToUniversalTime();
                    string dtUniversal = dateTimeValue.ToString("o", CultureInfo.InvariantCulture);
                    if (dtUniversal.Equals(originalString, StringComparison.Ordinal))
                        return dateTimeValue;
                }
            }
            catch(Exception)
            { }
#pragma warning restore CA1031 // Do not catch general exception types

            return originalString;
        }

        internal static ICollection<string> ReadStrings(
            ref Utf8JsonReader reader,
            ICollection<string> strings,
            string propertyName,
            string className,
            bool read = false)
        {
            if (read)
                reader.Read();

            // returning null keeps the same logic as JsonSerialization.ReadObject
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (!IsReaderAtTokenType(ref reader, JsonTokenType.StartArray, false))
                throw LogHelper.LogExceptionMessage(
                    CreateJsonReaderExceptionInvalidType(ref reader, "JsonTokenType.StartArray", className, propertyName));

            while (reader.Read())
            {
                if (IsReaderAtTokenType(ref reader, JsonTokenType.EndArray, false))
                    break;

                strings.Add(ReadString(ref reader, propertyName, className));
            }

            return strings;
        }

        internal static IList<string> ReadStrings(
            ref Utf8JsonReader reader,
            IList<string> strings,
            string propertyName,
            string className,
            bool read = false)
        {
            if (read)
                reader.Read();

            // returning null keeps the same logic as JsonSerialization.ReadObject
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (!IsReaderAtTokenType(ref reader, JsonTokenType.StartArray, false))
                throw LogHelper.LogExceptionMessage(
                    CreateJsonReaderExceptionInvalidType(ref reader, "JsonTokenType.StartArray", className, propertyName));

            while (reader.Read())
            {
                if (IsReaderAtTokenType(ref reader, JsonTokenType.EndArray, false))
                    break;

                strings.Add(ReadString(ref reader, propertyName, className));
            }

            return strings;
        }

        /// <summary>
        /// This method is called when deserializing a property value as an object.
        /// Normally we put the object into a Dictionary[string, object].
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="propertyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        internal static object ReadPropertyValueAsObject(ref Utf8JsonReader reader, string propertyName, string className)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.Number:
                    return ReadNumber(ref reader);
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.String:
                    return ReadStringAsObject(ref reader, propertyName, className);
                case JsonTokenType.StartObject:
                    return ReadJsonElement(ref reader);
                case JsonTokenType.StartArray:
                    return ReadArrayOfObjects(ref reader, propertyName, className);
                default:
                    // There is something broken here as this was called when the reader is pointing at a property.
                    // It must be a known Json type.
                    Debug.Assert(false, $"Utf8JsonReader.TokenType is not one of the expected types: False, Number, True, Null, String, StartArray, StartObject. Is: '{reader.TokenType}'.");
                    return null;
            }
        }
        #endregion

        #region Write
        public static void WriteAsJsonElement(ref Utf8JsonWriter writer, string json)
        {
            Utf8JsonReader reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json).AsSpan());

#if NET6_0_OR_GREATER
            if (JsonElement.TryParseValue(ref reader, out JsonElement? jsonElement))
                jsonElement.Value.WriteTo(writer);
#else
            using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
                jsonDocument.RootElement.WriteTo(writer);
#endif
        }

        public static void WriteObjects(ref Utf8JsonWriter writer, IDictionary<string, object> dictionary)
        {
            if (dictionary?.Count > 0)
                foreach (KeyValuePair<string, object> kvp in dictionary)
                    WriteObject(ref writer, kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Writes an 'object' as a JsonProperty.
        /// This was written to support what IdentityModel6x supported and is not meant to be a
        /// general object serializer.
        /// If a user needs to serialize a special value, then serialize the value into a JsonElement.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="depth">The current depth of recursive call for objects.
        /// Maximum is 2.</param>
        public static void WriteObject(ref Utf8JsonWriter writer, string key, object obj, int depth = 0)
        {
            if (obj is string str)
                writer.WriteString(key, str);
            else if (obj is DateTime dt)
                writer.WriteString(key, dt.ToUniversalTime());
            else if (obj is int i)
                writer.WriteNumber(key, i);
            else if (obj is bool b)
                writer.WriteBoolean(key, b);
            else if (obj is decimal d)
                writer.WriteNumber(key, d);
            else if (obj is double dub)
                writer.WriteNumber(key, dub);
            else if (obj is float f)
                writer.WriteNumber(key, f);
            else if (obj is long l)
                writer.WriteNumber(key, l);
            else if (obj is null)
                writer.WriteNull(key);
            else if (obj is List<string> strs)
            {
                writer.WriteStartArray(key);
                foreach (string item in strs)
                    writer.WriteStringValue(item);

                writer.WriteEndArray();
            }
            else if (depth < MaxDepth && obj is List<object> objs)
            {
                depth++;
                writer.WriteStartArray(key);
                foreach (object item in objs)
                    WriteObjectValue(ref writer, item, depth);

                writer.WriteEndArray();
            }
            else if (obj is IDictionary<string, string> idics)
            {
                writer.WriteStartObject(key);
                foreach (KeyValuePair<string, string> kvp in idics)
                    writer.WriteString(kvp.Key, kvp.Value);

                writer.WriteEndObject();
            }
            else if (depth < MaxDepth && obj is IDictionary<string, object> idic)
            {
                depth++;
                writer.WriteStartObject(key);
                foreach (KeyValuePair<string, object> kvp in idic)
                    WriteObject(ref writer, kvp.Key, kvp.Value, depth);

                writer.WriteEndObject();
            }
            else if (depth < MaxDepth && obj is Dictionary<string, object> dic)
            {
                depth++;
                writer.WriteStartObject(key);
                foreach (KeyValuePair<string, object> kvp in dic)
                    WriteObject(ref writer, kvp.Key, kvp.Value, depth);

                writer.WriteEndObject();
            }
            else if (obj is JsonElement j)
            {
                writer.WritePropertyName(key);
                j.WriteTo(writer);
            }
            else
            {
                writer.WriteString(key, obj.ToString());
            }
        }

        /// <summary>
        /// Writes values into an array.
        /// Assumes the writer.StartArray() has been called.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj"></param>
        /// <param name="depth">The current depth of recursive call for objects.
        /// Maximum is 2.</param>
        public static void WriteObjectValue(ref Utf8JsonWriter writer, object obj, int depth = 0)
        {
            if (obj is string str)
                writer.WriteStringValue(str);
            else if (obj is DateTime dt)
                writer.WriteStringValue(dt.ToUniversalTime());
            else if (obj is int i)
                writer.WriteNumberValue(i);
            else if (obj is bool b)
                writer.WriteBooleanValue(b);
            else if (obj is double d)
                writer.WriteNumberValue((decimal)d);
            else if (obj is decimal m)
                writer.WriteNumberValue(m);
            else if (obj is float f)
                writer.WriteNumberValue(f);
            else if (obj is long l)
                writer.WriteNumberValue(l);
            else if (obj is null)
                writer.WriteNullValue();
            else if (obj is JsonElement j)
                j.WriteTo(writer);
            else if (obj is List<string> strings)
            {
                writer.WriteStartArray();
                foreach (string strValue in strings)
                    writer.WriteStringValue(strValue);

                writer.WriteEndArray();
            }
            else if (depth < MaxDepth && obj is List<object> objs)
            {
                depth++;
                writer.WriteStartArray();
                foreach (object item in objs)
                    WriteObjectValue(ref writer, item, depth);

                writer.WriteEndArray();
            }
            else
                writer.WriteStringValue(obj.ToString());
        }

        public static void WriteStrings(ref Utf8JsonWriter writer, ReadOnlySpan<byte> propertyName, IList<string> strings)
        {
            writer.WriteStartArray(propertyName);
            foreach (string str in strings)
                writer.WriteStringValue(str);

            writer.WriteEndArray();
        }

        public static void WriteStrings(ref Utf8JsonWriter writer, ReadOnlySpan<byte> propertyName, ICollection<string> strings)
        {
            writer.WriteStartArray(propertyName);
            foreach (string str in strings)
                writer.WriteStringValue(str);

            writer.WriteEndArray();
        }

        public static void WriteStrings(ref Utf8JsonWriter writer, JsonEncodedText propertyName, IList<string> strings)
        {
            writer.WriteStartArray(propertyName);
            foreach (string str in strings)
                writer.WriteStringValue(str);

            writer.WriteEndArray();
        }
#endregion
    }
}

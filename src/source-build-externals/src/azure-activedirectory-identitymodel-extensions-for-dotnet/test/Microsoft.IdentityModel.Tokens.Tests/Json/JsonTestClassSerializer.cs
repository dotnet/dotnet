// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public interface IJsonSerializer
    {
        void Serialize(object obj, Utf8JsonWriter utf8JsonWriter);

        object Deserialize(string json, Utf8JsonReader utf8JsonReader);
    }

    public class SystemTextJsonSerializer : IJsonSerializer
    {
        public void Serialize(object obj, Utf8JsonWriter utf8JsonWriter)
        {
            JsonTestClassSerializer.Serialize(obj as JsonTestClass, utf8JsonWriter);
        }

        public object Deserialize(string json, Utf8JsonReader utf8JsonReader)
        {
            return JsonTestClassSerializer.Deserialize(ref utf8JsonReader);
        }
    }

    public static class JsonTestClassSerializer
    {
        private static string _className = typeof(JsonTestClass).FullName;

        #region Read
        public static JsonTestClass Deserialize(string json, IDictionary<Type, IJsonSerializer> readers = null)
        {
            ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(json).AsSpan();
            Utf8JsonReader reader = new Utf8JsonReader(bytes);
            return Deserialize(ref reader, readers);
        }

        /// <summary>
        /// Reads a JsonTestClass.
        /// </summary>
        /// <param name="reader">a <see cref="Utf8JsonReader"/> pointing at a StartObject.</param>
        /// <returns>A <see cref="JsonWebKeyNet8"/>.</returns>
        public static JsonTestClass Deserialize(ref Utf8JsonReader reader, IDictionary<Type, IJsonSerializer> readers = null)
        {
            JsonTestClass jsonTestClass = new JsonTestClass();

            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, false))
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                        LogMessages.IDX11022,
                        LogHelper.MarkAsNonPII("JsonTokenType.StartObject"),
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(JsonTestClass.ClassName),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed))));

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = JsonSerializerPrimitives.ReadPropertyName(ref reader, JsonTestClass.ClassName, true);
                    switch (propertyName)
                    {
                        // optional
                        // https://datatracker.ietf.org/doc/html/rfc7517#section-4.4
                        case "Boolean":
                            jsonTestClass.Boolean = JsonSerializerPrimitives.ReadBoolean(ref reader, "Boolean",_className);
                            break;
                        case "Double":
                            jsonTestClass.Double = JsonSerializerPrimitives.ReadDouble(ref reader, "Double", _className);
                            break;
                        case "Int":
                            jsonTestClass.Int = JsonSerializerPrimitives.ReadInt(ref reader, "Int", _className);
                            break;
                        case "ListObject":
                            List<object> objects = new List<object>();

                            if (ReadObjects(ref reader, objects, "ListObject", _className) == null)
                            {
                                throw LogHelper.LogExceptionMessage(
                                    new ArgumentNullException(
                                        "ListString",
                                        new JsonException(
                                            LogHelper.FormatInvariant(
                                            LogMessages.IDX11022,
                                            LogHelper.MarkAsNonPII(reader.TokenType),
                                            LogHelper.MarkAsNonPII(JsonTestClass.ClassName),
                                            LogHelper.MarkAsNonPII(propertyName),
                                            LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                                            LogHelper.MarkAsNonPII(reader.CurrentDepth),
                                            LogHelper.MarkAsNonPII(reader.BytesConsumed)))));
                            }

                            jsonTestClass.ListObject = objects;
                            break;
                        case "ListString":
                             List<string> strings = new List<string>();

                            if (JsonSerializerPrimitives.ReadStrings(ref reader, strings, "ListString", _className) == null)
                            {
                                throw LogHelper.LogExceptionMessage(
                                    new ArgumentNullException(
                                        "ListString",
                                        new JsonException(
                                            LogHelper.FormatInvariant(
                                            LogMessages.IDX11022,
                                            LogHelper.MarkAsNonPII(reader.TokenType),
                                            LogHelper.MarkAsNonPII(JsonTestClass.ClassName),
                                            LogHelper.MarkAsNonPII(propertyName),
                                            LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                                            LogHelper.MarkAsNonPII(reader.CurrentDepth),
                                            LogHelper.MarkAsNonPII(reader.BytesConsumed)))));
                            }

                            jsonTestClass.ListString = strings;
                            break;
                        case "String":
                            jsonTestClass.String = JsonSerializerPrimitives.ReadString(ref reader, "String", _className);
                            break;
                        default:
                            using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
                                jsonTestClass.AdditionalData[propertyName] = jsonDocument.RootElement.Clone();

                            reader.Read();
                            break;
                    }
                }

                if (JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndObject, true))
                    break;
            }

            return jsonTestClass;
        }
        #endregion

        #region Write

        public static string Serialize(JsonTestClass jsonTestClass, IDictionary<Type, IJsonSerializer> writers = null)
        {
            JsonWriterOptions jsonWriterOptions = new JsonWriterOptions();
            jsonWriterOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return Serialize(jsonTestClass, jsonWriterOptions, writers);
        }

        public static string Serialize(JsonTestClass jsonTestClass, JsonWriterOptions jsonWriterOptions, IDictionary<Type, IJsonSerializer> serializers = null)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Utf8JsonWriter writer = new Utf8JsonWriter(memoryStream, jsonWriterOptions))
                {
                    Serialize(jsonTestClass, writer, serializers);
                    writer.Flush();
                    return UTF8Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }

        public static void Serialize(JsonTestClass jsonTestClass, Utf8JsonWriter writer, IDictionary<Type, IJsonSerializer> serializers = null)
        {
            _ = jsonTestClass ?? throw new ArgumentNullException(nameof(jsonTestClass));
            _ = writer ?? throw new ArgumentNullException(nameof(writer));

            writer.WriteStartObject();

            if (jsonTestClass.Boolean.HasValue)
                writer.WriteBoolean("Boolean", jsonTestClass.Boolean.Value);

            if (jsonTestClass.Double.HasValue)
            {
                // need to convert to decimal to avoid some odd rounding.
                // net 6.0 does not have this issue.
                writer.WritePropertyName("Double");
                decimal decimalValue = (decimal)jsonTestClass.Double.Value;
                writer.WriteNumberValue(decimalValue);
            }

            if (jsonTestClass.Int.HasValue)
                writer.WriteNumber("Int", jsonTestClass.Int.Value);

            if (jsonTestClass.ListObject != null && jsonTestClass.ListObject.Count > 0)
            {
                writer.WriteStartArray("ListObject");
                foreach (var item in jsonTestClass.ListObject)
                {
                    if (item == null)
                        writer.WriteNullValue();
                    else
                        JsonSerializer.Serialize(writer, item);
                }

                writer.WriteEndArray();
            }

            if (jsonTestClass.ListString != null && jsonTestClass.ListString.Count > 0)
            {
                writer.WriteStartArray("ListString");

                foreach (var item in jsonTestClass.ListString)
                    writer.WriteStringValue(item);

                writer.WriteEndArray();
            }

            if (!string.IsNullOrEmpty(jsonTestClass.String))
                writer.WriteString("String", jsonTestClass.String);

            if (jsonTestClass.AdditionalData != null && jsonTestClass.AdditionalData.Count > 0)
            {
                foreach (var item in jsonTestClass.AdditionalData)
                {
                    writer.WritePropertyName(item.Key);
                    if (item.Value == null)
                    {
                        writer.WriteNullValue();
                    }
                    else if (serializers != null && item.Value != null && serializers.TryGetValue(item.Value.GetType(), out IJsonSerializer jsonSerializer))
                    {
                        jsonSerializer.Serialize(item.Value, writer);
                    }
                    else if (item.Value.GetType() == typeof(JsonElement))
                    {
                        ((JsonElement)item.Value).WriteTo(writer);
                    }
                    else if (item.Value.GetType() == typeof(JsonDocument))
                    {
                        ((JsonDocument)item.Value).WriteTo(writer);
                    }
                    else if (item.Value.GetType() == typeof(string))
                    {
                        writer.WriteStringValue((string)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(bool))
                    {
                        writer.WriteBooleanValue((bool)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(int))
                    {
                        writer.WriteNumberValue((int)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(long))
                    {
                        writer.WriteNumberValue((long)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(double))
                    {
                        writer.WriteNumberValue((double)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(decimal))
                    {
                        writer.WriteNumberValue((decimal)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(float))
                    {
                        writer.WriteNumberValue((float)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(DateTime))
                    {
                        writer.WriteStringValue((DateTime)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(DateTimeOffset))
                    {
                        writer.WriteStringValue((DateTimeOffset)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(Guid))
                    {
                        writer.WriteStringValue((Guid)item.Value);
                    }
                    else if (item.Value.GetType() == typeof(byte[]))
                    {
                        writer.WriteBase64StringValue((byte[])item.Value);
                    }
                    else if (item.Value.GetType() == typeof(Uri))
                    {
                        writer.WriteStringValue(((Uri)item.Value).OriginalString);
                    }
                    else if (item.Value.GetType() == typeof(TimeSpan))
                    {
                        writer.WriteStringValue(((TimeSpan)item.Value).ToString());
                    }
                    else
                    {
                        writer.WriteStringValue(item.Value.ToString());
                    }
                }
            }

            writer.WriteEndObject();
        }
        #endregion

        internal static IList<object> ReadObjects(ref Utf8JsonReader reader, IList<object> objects, string propertyName, string className)
        {
            _ = objects ?? throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(objects)));

            // returning null keeps the same logic as JsonSerialization.ReadObject
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartArray, false))
                throw LogHelper.LogExceptionMessage(
                    JsonSerializerPrimitives.CreateJsonReaderExceptionInvalidType(ref reader, "JsonTokenType.StartArray", className, propertyName));

            while (reader.Read())
            {
                if (JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndArray, true))
                    break;

                objects.Add(JsonSerializerPrimitives.ReadJsonElement(ref reader));
            }

            return objects;
        }
    }
}

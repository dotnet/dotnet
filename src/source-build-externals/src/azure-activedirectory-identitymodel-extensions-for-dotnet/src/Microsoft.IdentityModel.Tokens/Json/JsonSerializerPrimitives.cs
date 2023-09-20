// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        // This is not a general purpose JSON serializer. It is specifically
        // made for the use in the IdentityModel libraries. As such, we can take a
        // lower limit to both our reading and writing max depth.
        // This number is the min between System.Text.Jsons default for
        // writing and reading max depth.
        const int MaxDepth = 64;

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

        public static JsonElement CreateJsonElement(List<string> strings)
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

        internal static object CreateObjectFromJsonElement(JsonElement jsonElement, int currentDepth)
        {
            if (currentDepth >= MaxDepth)
                throw new InvalidOperationException(LogHelper.FormatInvariant(
                    LogMessages.IDX10815,
                    LogHelper.MarkAsNonPII(currentDepth),
                    LogHelper.MarkAsNonPII(MaxDepth)));

            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                if (DateTime.TryParse(jsonElement.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime dateTime))
                    return dateTime;

                return jsonElement.GetString();
            }
            else if (jsonElement.ValueKind == JsonValueKind.False)
                return false;
            else if (jsonElement.ValueKind == JsonValueKind.True)
                return true;
            else if (jsonElement.ValueKind == JsonValueKind.Number)
            {
                if (jsonElement.TryGetInt32(out int intValue))
                    return intValue;
                else if (jsonElement.TryGetInt64(out long longValue))
                    return longValue;
                else if (jsonElement.TryGetDecimal(out decimal decimalValue))
                    return decimalValue;
                else if (jsonElement.TryGetDouble(out double doubleValue))
                    return doubleValue;
                else if (jsonElement.TryGetUInt32(out uint uintValue))
                    return uintValue;
                else if (jsonElement.TryGetUInt64(out ulong ulongValue))
                    return ulongValue;
            }
            else if (jsonElement.ValueKind == JsonValueKind.Null)
                return null;
            else if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                int numItems = 0;
                foreach (JsonElement j in jsonElement.EnumerateArray())
                    numItems++;

                object[] items = new object[numItems];

                int index = 0;
                foreach (JsonElement j in jsonElement.EnumerateArray())
                {
                    items[index++] = CreateObjectFromJsonElement(j, currentDepth + 1);
                }

                return items;
            }
            else if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                int numItems = 0;
                foreach (JsonProperty property in jsonElement.EnumerateObject())
                    numItems++;

                int index = 0;
                KeyValuePair<string, object>[] kvps = new KeyValuePair<string, object>[numItems];
                foreach (JsonProperty property in jsonElement.EnumerateObject())
                {
                    kvps[index++] = new KeyValuePair<string, object>(property.Name, CreateObjectFromJsonElement(property.Value, currentDepth + 1));
                }

                return kvps;
            }

            return jsonElement.GetString();
        }

        public static bool TryCreateTypeFromJsonElement<T>(JsonElement jsonElement, out T t)
        {
            int currentDepth = 0;

            if (typeof(T) == typeof(string))
            {
                t = (T)(object)jsonElement.ToString();
                return true;
            }
            else if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                if (typeof(T) == typeof(Dictionary<string, string>))
                {
                    Dictionary<string, string> dictionary = new();
                    foreach (JsonProperty property in jsonElement.EnumerateObject())
                        if (property.Value.ValueKind == JsonValueKind.String)
                            dictionary[property.Name] = property.Value.GetString();
                        else
                            dictionary[property.Name] = property.Value.GetRawText();

                    t = (T)(object)dictionary;
                    return true;
                }
                else if (typeof(T) == typeof(Dictionary<string, string[]>))
                {
                    Dictionary<string, string[]> dictionary = new();
                    foreach (JsonProperty property in jsonElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind != JsonValueKind.Array)
                            dictionary[property.Name] = new string[] { property.Value.GetRawText() };

                        int numItems = 0;
                        foreach (JsonElement j in property.Value.EnumerateArray())
                            numItems++;

                        string[] items = new string[numItems];
                        numItems = 0;
                        foreach (JsonElement j in property.Value.EnumerateArray())
                            if (j.ValueKind == JsonValueKind.String)
                                items[numItems++] = j.GetString();
                            else
                                items[numItems++] = j.GetRawText();

                        dictionary[property.Name] = items;
                    }

                    t = (T)(object)dictionary;
                    return true;
                }
                else if (typeof(T) == typeof(Dictionary<string, List<string>>))
                {
                    Dictionary<string, List<string>> dictionary = new();
                    foreach (JsonProperty property in jsonElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind != JsonValueKind.Array)
                            dictionary[property.Name] = new List<string> { property.Value.GetRawText() };

                        List<string> items = new();
                        foreach (JsonElement j in property.Value.EnumerateArray())
                            if (j.ValueKind == JsonValueKind.String)
                                items.Add(j.GetString());
                            else
                                items.Add(j.GetRawText());

                        dictionary[property.Name] = items;
                    }

                    t = (T)(object)dictionary;
                    return true;
                }
                else if (typeof(T) == typeof(Dictionary<string, Collection<string>>))
                {
                    Dictionary<string, Collection<string>> dictionary = new();
                    foreach (JsonProperty property in jsonElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind != JsonValueKind.Array)
                            dictionary[property.Name] = new Collection<string> { property.Value.GetRawText() };

                        Collection<string> items = new();
                        foreach (JsonElement j in property.Value.EnumerateArray())
                            if (j.ValueKind == JsonValueKind.String)
                                items.Add(j.GetString());
                            else
                                items.Add(j.GetRawText());

                        dictionary[property.Name] = items;
                    }

                    t = (T)(object)dictionary;
                    return true;
                }
                else if (typeof(T) == typeof(Dictionary<string, object>))
                {
                    Dictionary<string, object> dictionary = new();
                    foreach (JsonProperty property in jsonElement.EnumerateObject())
                    {
                        dictionary[property.Name] = CreateObjectFromJsonElement(property.Value, currentDepth + 1);
                    }

                    t = (T)(object)dictionary;
                    return true;
                }
            }
            else if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                if (typeof(T) == typeof(string[]))
                {
                    int numItems = 0;
                    // is this an array of properties
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        numItems++;

                    string[] items = new string[numItems];
                    numItems = 0;
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        if (j.ValueKind == JsonValueKind.String)
                            items[numItems++] = j.GetString();
                        else
                            items[numItems++] = j.GetRawText();

                    t = (T)(object)items;
                    return true;
                }
                else if (typeof(T) == typeof(List<string>))
                {
                    List<string> items = new();
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        if (j.ValueKind == JsonValueKind.String)
                            items.Add(j.GetString());
                        else
                            items.Add(j.GetRawText());

                    t = (T)(object)items;
                    return true;
                }
                else if (typeof(T) == typeof(Collection<string>))
                {
                    Collection<string> items = new();
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        if (j.ValueKind == JsonValueKind.String)
                            items.Add(j.GetString());
                        else
                            items.Add(j.GetRawText());

                    t = (T)(object)items;
                    return true;
                }
                // we could have added an OR condition to List<string>
                // but we have set an order of preference for the return types: Collection<string> is preferred over IList<string>
                else if (typeof(T) == typeof(IList<string>))
                {
                    List<string> items = new();
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        if (j.ValueKind == JsonValueKind.String)
                            items.Add(j.GetString());
                        else
                            items.Add(j.GetRawText());

                    t = (T)(object)items;
                    return true;
                }
                // we could have added an OR condition to Collection<string>
                // but we have set an order of preference for the return types:
                // string[], List<string>, Collection<string>, IList<string>, ICollection<string>
                else if (typeof(T) == typeof(ICollection<string>))
                {
                    Collection<string> items = new();
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        if (j.ValueKind == JsonValueKind.String)
                            items.Add(j.GetString());
                        else
                            items.Add(j.GetRawText());

                    t = (T)(object)items;
                    return true;
                }
                else if (typeof(T) == typeof(object[]))
                {
                    int numItems = 0;
                    // is this an array of properties
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        numItems++;

                    object[] items = new object[numItems];
                    numItems = 0;
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                    {
                        items[numItems++] = CreateObjectFromJsonElement(j, currentDepth + 1);
                    }

                    t = (T)(object)items;
                    return true;
                }
                else if (typeof(T) == typeof(List<object>))
                {
                    List<object> items = new();
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                    {
                        items.Add(CreateObjectFromJsonElement(j, currentDepth + 1));
                    }

                    t = (T)(object)items;
                    return true;
                }
                else if (typeof(T) == typeof(Collection<object>))
                {
                    Collection<object> items = new();
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                    {
                        items.Add(CreateObjectFromJsonElement(j, currentDepth + 1));
                    }

                    t = (T)(object)items;
                    return true;
                }
                else if (typeof(T) == typeof(int[]))
                {
                    int numItems = 0;
                    // is this an array of properties
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        numItems++;

                    int[] items = new int[numItems];
                    numItems = 0;

                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        if (j.TryGetInt32(out int i))
                            items[numItems++] = i;
                        else if (int.TryParse(j.GetRawText(), out int value))
                            items[numItems++] = value;
                        else
                        {
                            t = default;
                            return false;
                        }

                    t = (T)(object)items;
                    return true;
                }
                else if (typeof(T) == typeof(long[]))
                {
                    int numItems = 0;
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                        numItems++;

                    long[] items = new long[numItems];
                    numItems = 0;
                    foreach (JsonElement j in jsonElement.EnumerateArray())
                    {
                        if (j.TryGetInt64(out long l))
                            items[numItems++] = l;
                        else if (long.TryParse(j.GetRawText(), out long value))
                            items[numItems++] = value;
                        else
                        {
                            t = default;
                            return false;
                        }
                    }

                    t = (T)(object)items;
                    return true;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                if (jsonElement.ValueKind == JsonValueKind.String)
                    t = (T)(object)jsonElement.GetString();
                else
                    t = (T)(object)jsonElement.GetRawText();

                return true;
            }

            t = default;
            return false;
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

        internal static long ReadLong(ref Utf8JsonReader reader, string propertyName, string className, bool read = false)
        {
            if (read)
                reader.Read();

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out long l))
                    return l;

                if (reader.TryGetDouble(out double d))
                    return Convert.ToInt64(d);
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                if (long.TryParse(reader.GetString(), out long value))
                    return value;

                if (double.TryParse(reader.GetString(), out double d))
                    return Convert.ToInt64(d);
            }

            throw LogHelper.LogExceptionMessage(
                CreateJsonReaderException(ref reader, "JsonTokenType.Number", className, propertyName));
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
            else if (reader.TokenType == JsonTokenType.String)
            {
                if (double.TryParse(reader.GetString(), out double value))
                    return value;
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

        internal static List<object> ReadArrayOfObjects(ref Utf8JsonReader reader, string propertyName, string className)
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

        // This is a special case for reading audiences where in 6x, we didn't add null strings to the list.
        internal static void ReadStringsSkipNulls(
            ref Utf8JsonReader reader,
            List<string> strings,
            string propertyName,
            string className)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return;

            if (!IsReaderAtTokenType(ref reader, JsonTokenType.StartArray, false))
                throw LogHelper.LogExceptionMessage(
                    CreateJsonReaderExceptionInvalidType(ref reader, "JsonTokenType.StartArray", className, propertyName));

            while (reader.Read())
            {
                if (IsReaderAtTokenType(ref reader, JsonTokenType.EndArray, false))
                    break;

                if (reader.TokenType == JsonTokenType.Null)
                    continue;

                strings.Add(ReadString(ref reader, propertyName, className));
            }

            return;
        }

        /// <summary>
        /// This method is called when deserializing a property value as an object.
        /// Normally we put the object into a Dictionary[string, object].
        /// </summary>
        /// <param name="reader">the <see cref="Utf8JsonReader"/></param>
        /// <param name="propertyName">the property name that is being read</param>
        /// <param name="className">the type that is being deserialized</param>
        /// <param name="read">if true reader.Read() will be called.</param>
        /// <returns></returns>
        internal static object ReadPropertyValueAsObject(ref Utf8JsonReader reader, string propertyName, string className, bool read = false)
        {
            if (read)
                reader.Read();

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
                    return ReadJsonElement(ref reader);
                default:
                    // There is something broken here as this was called when the reader is pointing at a property.
                    // It must be a known Json type.
                    Debug.Assert(false, $"Utf8JsonReader.TokenType is not one of the expected types: False, Number, True, Null, String, StartArray, StartObject. Is: '{reader.TokenType}'.");
                    return null;
            }
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
        public static void WriteObject(ref Utf8JsonWriter writer, string key, object obj)
        {
            if (writer.CurrentDepth >= MaxDepth)
                throw new InvalidOperationException(LogHelper.FormatInvariant(
                    LogMessages.IDX10815,
                    LogHelper.MarkAsNonPII(writer.CurrentDepth),
                    LogHelper.MarkAsNonPII(MaxDepth)));

            if (obj is null)
            {
                writer.WriteNull(key);
                return;
            }

            Type objType = obj.GetType();

            if (obj is string str)
                writer.WriteString(key, str);
            else if (obj is long l)
                writer.WriteNumber(key, l);
            else if (obj is int i)
                writer.WriteNumber(key, i);
            else if (obj is bool b)
                writer.WriteBoolean(key, b);
            else if (obj is DateTime dt)
                writer.WriteString(key, dt.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture));
            else if (typeof(IDictionary).IsAssignableFrom(objType))
            {
                IDictionary dictionary = (IDictionary)obj;
                writer.WritePropertyName(key);

                writer.WriteStartObject();
                foreach (var k in dictionary.Keys)
                    WriteObject(ref writer, k.ToString(), dictionary[k]);

                writer.WriteEndObject();
            }
            else if (typeof(IList).IsAssignableFrom(objType))
            {
                IList list = (IList)obj;
                writer.WriteStartArray(key);
                foreach (var k in list)
                    WriteObjectValue(ref writer, k);

                writer.WriteEndArray();
            }
            else if (obj is JsonElement j)
            {
                writer.WritePropertyName(key);
                j.WriteTo(writer);
            }
            else if (obj is double dub)
                writer.WriteNumber(key, dub);
            else if (obj is decimal d)
                writer.WriteNumber(key, d);
            else if (obj is float f)
                writer.WriteNumber(key, f);
            else
                throw LogHelper.LogExceptionMessage(
                    new ArgumentException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX11025,
                            LogHelper.MarkAsNonPII(objType.ToString()),
                            LogHelper.MarkAsNonPII(key))));
        }

        /// <summary>
        /// Writes values into an array.
        /// Assumes the writer.StartArray() has been called.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj"></param>
        public static void WriteObjectValue(ref Utf8JsonWriter writer, object obj)
        {
            if (writer.CurrentDepth >= MaxDepth)
                throw new InvalidOperationException(LogHelper.FormatInvariant(
                    LogMessages.IDX10815,
                    LogHelper.MarkAsNonPII(writer.CurrentDepth),
                    LogHelper.MarkAsNonPII(MaxDepth)));

            if (obj is null)
            {
                writer.WriteNullValue();
                return;
            }

            Type objType = obj.GetType();

            if (obj is string str)
                writer.WriteStringValue(str);
            else if (obj is DateTime dt)
                writer.WriteStringValue(dt.ToUniversalTime());
            else if (obj is int i)
                writer.WriteNumberValue(i);
            else if (obj is bool b)
                writer.WriteBooleanValue(b);
            else if (obj is long l)
                writer.WriteNumberValue(l);
            else if (obj is null)
                writer.WriteNullValue();
            else if (obj is double d)
                writer.WriteNumberValue((decimal)d);
            else if (obj is JsonElement j)
                j.WriteTo(writer);
            else if (typeof(IDictionary).IsAssignableFrom(objType))
            {
                IDictionary dictionary = (IDictionary)obj;
                writer.WriteStartObject();
                foreach (var k in dictionary.Keys)
                    WriteObject(ref writer, k.ToString(), dictionary[k]);

                writer.WriteEndObject();
            }
            else if (typeof(IList).IsAssignableFrom(objType))
            {
                IList list = (IList)obj;
                writer.WriteStartArray();
                foreach (var k in list)
                    WriteObjectValue(ref writer, k);

                writer.WriteEndArray();
            }
            else if (obj is decimal m)
                writer.WriteNumberValue(m);
            else if (obj is float f)
                writer.WriteNumberValue(f);
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
        #endregion
    }
}

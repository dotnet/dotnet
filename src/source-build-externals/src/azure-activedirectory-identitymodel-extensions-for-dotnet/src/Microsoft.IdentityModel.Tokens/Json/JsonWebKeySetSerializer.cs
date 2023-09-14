// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens.Json
{
    internal static class JsonWebKeySetSerializer
    {
        private static readonly byte[] _keysUtf8 = Encoding.UTF8.GetBytes(JsonWebKeySetParameterNames.Keys);

        #region Read
        public static JsonWebKeySet Read(string json, JsonWebKeySet jsonWebKeySet)
        {
            Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json).AsSpan());

            try
            {
                return Read(ref reader, jsonWebKeySet);
            }
            catch (JsonException ex)
            {
                if (ex.GetType() == typeof(JsonException))
                    throw;

                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX10805,
                            LogHelper.MarkAsNonPII(json),
                            LogHelper.MarkAsNonPII(JsonWebKey.ClassName))));
            }
        }

        /// <summary>
        /// Reads a JsonWebKey. see: https://datatracker.ietf.org/doc/html/rfc7517
        /// </summary>
        /// <param name="reader">a <see cref="Utf8JsonReader"/> pointing at a StartObject.</param>
        /// <param name="jsonWebKeySet"></param>
        /// <returns>A <see cref="JsonWebKeySet"/>.</returns>
        public static JsonWebKeySet Read(ref Utf8JsonReader reader, JsonWebKeySet jsonWebKeySet)
        {
            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, false))
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                        LogMessages.IDX11022,
                        LogHelper.MarkAsNonPII("JsonTokenType.StartObject"),
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(JsonWebKeySet.ClassName),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed))));

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.ValueTextEquals(_keysUtf8))
                    {
                        reader.Read();
                        ReadKeys(ref reader, jsonWebKeySet);
                    }
                    else
                    {
                        string propertyName = JsonSerializerPrimitives.ReadPropertyName(ref reader, JsonWebKeySet.ClassName, true);
                        if (propertyName.Equals(JsonWebKeyParameterNames.Keys, StringComparison.OrdinalIgnoreCase))
                            ReadKeys(ref reader, jsonWebKeySet);
                        else
                            jsonWebKeySet.AdditionalData[propertyName] = JsonSerializerPrimitives.ReadPropertyValueAsObject(ref reader,JsonWebKeyParameterNames.Keys, JsonWebKeySet.ClassName);
                    }
                }
                else if (JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndObject, true))
                {
                    break;
                }
            }

            return jsonWebKeySet;
        }

        public static void ReadKeys(ref Utf8JsonReader reader, JsonWebKeySet jsonWebKeySet)
        {
            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartArray, false))
                throw LogHelper.LogExceptionMessage(
                    JsonSerializerPrimitives.CreateJsonReaderExceptionInvalidType(
                        ref reader,
                        "JsonTokenType.StartArray",
                        JsonWebKeyParameterNames.KeyOps,
                        JsonWebKeySet.ClassName));

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject)
                    jsonWebKeySet.Keys.Add(JsonWebKeySerializer.Read(ref reader, new JsonWebKey()));
                else if (JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndArray, false))
                    break;
            }
        }

        #endregion

        #region Write
        public static string Write(JsonWebKeySet jsonWebKeySet)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Utf8JsonWriter writer = null;
                try
                {
                    writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    Write(ref writer, jsonWebKeySet);
                    writer.Flush();

                    return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
                finally
                {
                    writer?.Dispose();
                }
            }
        }

        /// <summary>
        /// This method will be used when reading OIDC metadata
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="jsonWebKeySet"></param>
        public static void Write(ref Utf8JsonWriter writer, JsonWebKeySet jsonWebKeySet)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(JsonWebKeyParameterUtf8Bytes.Keys);
            writer.WriteStartArray();

            foreach (JsonWebKey jsonWebKey in jsonWebKeySet.Keys)
                JsonWebKeySerializer.Write(ref writer, jsonWebKey);

            writer.WriteEndArray();

            if (jsonWebKeySet.AdditionalData.Count > 0)
                JsonSerializerPrimitives.WriteObjects(ref writer, jsonWebKeySet.AdditionalData);

            writer.WriteEndObject();
        }

        #endregion
    }
}


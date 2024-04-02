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

namespace Microsoft.IdentityModel.Tokens.Json
{
    internal static class JsonWebKeySerializer
    {
        // This is used to perform performant case-insensitive property names.
        // 6x used Newtonsoft and was case-insensitive w.r.t. property names.
        // The serializer is written to use Utf8JsonReader.ValueTextEquals(...), to match property names.
        // When we do not have a match, we check the uppercase name of the property against this table.
        // If not found, then we assume we should put the value into AdditionalData.
        // If we didn't do that, we would pay a performance penalty for those cases where there is AdditionalData
        // but otherwise the JSON properties are all lower case.
        public static HashSet<string> JsonWebKeyParameterNamesUpperCase = new HashSet<string>
        {
            "ALG",
            "CRV",
            "D",
            "DP",
            "DQ",
            "E",
            "K",
            "KEY_OPS",
            "KEYS",
            "KID",
            "KTY",
            "N",
            "OTH",
            "P",
            "Q",
            "QI",
            "USE",
            "X",
            "X5C",
            "X5T",
            "X5T#S256",
            "X5U",
            "Y"
        };

        #region Read
        public static JsonWebKey Read(string json)
        {
            return Read(json, new JsonWebKey());
        }

        public static JsonWebKey Read(string json, JsonWebKey jsonWebKey)
        {
            Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json).AsSpan());
            try
            {
                return Read(ref reader, jsonWebKey);
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
        /// <param name="jsonWebKey"></param>
        /// <returns>A <see cref="JsonWebKey"/>.</returns>
        public static JsonWebKey Read(ref Utf8JsonReader reader, JsonWebKey jsonWebKey)
        {
            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, true))
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                        LogMessages.IDX11023,
                        LogHelper.MarkAsNonPII("JsonTokenType.StartObject"),
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(JsonWebKey.ClassName),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed))));

            while(true)
            {
                #region Check property name using ValueTextEquals
                // common names are tried first
                // the JsonWebKey spec, https://datatracker.ietf.org/doc/html/rfc7517#section-4, does not require that we reject JSON with
                // duplicate member names, in strict mode, we could add logic to try a property once and throw if a duplicate shows up.
                // 6x uses the last value.
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.K))
                        jsonWebKey.K = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.K, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.E))
                        jsonWebKey.E = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.E, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.Kid))
                        jsonWebKey.Kid = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Kid, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.Kty))
                        jsonWebKey.Kty = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Kty, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.N))
                        jsonWebKey.N = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.N, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.X5c))
                        JsonSerializerPrimitives.ReadStrings(ref reader, jsonWebKey.X5c, JsonWebKeyParameterNames.X5c, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.Alg))
                        jsonWebKey.Alg = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Alg, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.Crv))
                        jsonWebKey.Crv = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Crv, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.D))
                        jsonWebKey.D = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.D, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.DP))
                        jsonWebKey.DP = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.DP, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.DQ))
                        jsonWebKey.DQ = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.DQ, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.KeyOps))
                    {
                        // the value can be null if the value is 'nill'
                        if (JsonSerializerPrimitives.ReadStrings(ref reader, jsonWebKey.KeyOps, JsonWebKeyParameterNames.KeyOps, JsonWebKey.ClassName, true) == null)
                        {
                            throw LogHelper.LogExceptionMessage(
                                new ArgumentNullException(
                                    JsonWebKeyParameterNames.KeyOps,
                                    new JsonException(
                                        LogHelper.FormatInvariant(
                                        LogMessages.IDX11022,
                                        LogHelper.MarkAsNonPII("JsonTokenType.StartArray"),
                                        LogHelper.MarkAsNonPII(reader.TokenType),
                                        LogHelper.MarkAsNonPII(JsonWebKey.ClassName),
                                        LogHelper.MarkAsNonPII(JsonWebKeyParameterNames.KeyOps),
                                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                                        LogHelper.MarkAsNonPII(reader.BytesConsumed)))));
                        }
                    }
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.Oth))
                        JsonSerializerPrimitives.ReadStrings(ref reader, jsonWebKey.Oth, JsonWebKeyParameterNames.Oth, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.P))
                        jsonWebKey.P = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.P, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.Q))
                        jsonWebKey.Q = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Q, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.QI))
                        jsonWebKey.QI = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.QI, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.Use))
                        jsonWebKey.Use = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Use, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.X))
                        jsonWebKey.X = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.X, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.X5t))
                        jsonWebKey.X5t = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.X5t, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.X5tS256))
                        jsonWebKey.X5tS256 = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.X5tS256, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.X5u))
                        jsonWebKey.X5u = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.X5u, JsonWebKey.ClassName, true);
                    else if (reader.ValueTextEquals(JsonWebKeyParameterUtf8Bytes.Y))
                        jsonWebKey.Y = JsonSerializerPrimitives.ReadString(ref reader, JsonWebKeyParameterNames.Y, JsonWebKey.ClassName, true);
                    #endregion
                    else
                    {
                        #region case-insensitive
                        // fallback to checking property names as case insensitive
                        // first check to see if the upper case property value is a valid property name if not add to AdditionalData, to avoid unnecessary string compares.
                        string propertyName = JsonSerializerPrimitives.ReadPropertyName(ref reader, JsonWebKey.ClassName, true);
                        if (!JsonWebKeyParameterNamesUpperCase.Contains(propertyName.ToUpperInvariant()))
                        {
                            jsonWebKey.AdditionalData[propertyName] = JsonSerializerPrimitives.ReadPropertyValueAsObject(ref reader, propertyName, JsonWebKey.ClassName);
                        }
                        else
                        {
                            if (propertyName.Equals(JsonWebKeyParameterNames.E, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.E = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.Kid, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.Kid = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.Kty, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.Kty = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.N, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.N = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.Use, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.Use = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.Alg, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.Alg = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.Crv, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.Crv = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.D, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.D = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.DP, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.DP = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.DQ, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.DQ = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.K, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.K = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.KeyOps, StringComparison.OrdinalIgnoreCase))
                            {
                                // the value can be null if the value is 'nill'
                                if (JsonSerializerPrimitives.ReadStrings(ref reader, jsonWebKey.KeyOps, propertyName, JsonWebKey.ClassName) == null)
                                {
                                    throw LogHelper.LogExceptionMessage(
                                        new ArgumentNullException(
                                            JsonWebKeyParameterNames.KeyOps,
                                            new JsonException(
                                                LogHelper.FormatInvariant(
                                                LogMessages.IDX11022,
                                                LogHelper.MarkAsNonPII("JsonTokenType.StartArray"),
                                                LogHelper.MarkAsNonPII(reader.TokenType),
                                                LogHelper.MarkAsNonPII(JsonWebKey.ClassName),
                                                LogHelper.MarkAsNonPII(JsonWebKeyParameterNames.KeyOps),
                                                LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                                                LogHelper.MarkAsNonPII(reader.CurrentDepth),
                                                LogHelper.MarkAsNonPII(reader.BytesConsumed)))));
                                }
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.Oth, StringComparison.OrdinalIgnoreCase))
                            {
                                JsonSerializerPrimitives.ReadStrings(ref reader, jsonWebKey.Oth, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.P, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.P = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.Q, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.Q = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.QI, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.QI = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.X, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.X = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.X5c, StringComparison.OrdinalIgnoreCase))
                            {
                                JsonSerializerPrimitives.ReadStrings(ref reader, jsonWebKey.X5c, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.X5t, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.X5t = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.X5tS256, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.X5tS256 = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.X5u, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.X5u = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                            else if (propertyName.Equals(JsonWebKeyParameterNames.Y, StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWebKey.Y = JsonSerializerPrimitives.ReadString(ref reader, propertyName, JsonWebKey.ClassName);
                            }
                        }
                        #endregion case-insensitive
                    }
                }
                // We read a JsonTokenType.StartObject above, exiting and positioning reader at next token.
                else if (JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndObject, true))
                    break;
                else if (!reader.Read())
                    break;
            }

            return jsonWebKey;
        }
        #endregion

        #region Write
        public static string Write(JsonWebKey jsonWebKey)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Utf8JsonWriter writer = null;
                try
                {
                    writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    Write(ref writer, jsonWebKey);
                    writer.Flush();

                    return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
                finally
                {
                    writer?.Dispose();
                }
            }
        }

        public static void Write(ref Utf8JsonWriter writer, JsonWebKey jsonWebKey)
        {
            _ = jsonWebKey ?? throw new ArgumentNullException(nameof(jsonWebKey));
            _ = writer ?? throw new ArgumentNullException(nameof(writer));

            writer.WriteStartObject();

            if (!string.IsNullOrEmpty(jsonWebKey.Alg))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.Alg, jsonWebKey.Alg);

            if (!string.IsNullOrEmpty(jsonWebKey.Crv))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.Crv, jsonWebKey.Crv);

            if (!string.IsNullOrEmpty(jsonWebKey.D))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.D, jsonWebKey.D);

            if (!string.IsNullOrEmpty(jsonWebKey.DP))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.DP, jsonWebKey.DP);

            if (!string.IsNullOrEmpty(jsonWebKey.DQ))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.DQ, jsonWebKey.DQ);

            if (!string.IsNullOrEmpty(jsonWebKey.E))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.E, jsonWebKey.E);

            if (!string.IsNullOrEmpty(jsonWebKey.K))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.K, jsonWebKey.K);

            if (jsonWebKey.KeyOps.Count > 0)
                JsonSerializerPrimitives.WriteStrings(ref writer, JsonWebKeyParameterUtf8Bytes.KeyOps, jsonWebKey.KeyOps);

            if (!string.IsNullOrEmpty(jsonWebKey.Kid))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.Kid, jsonWebKey.Kid);

            if (!string.IsNullOrEmpty(jsonWebKey.Kty))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.Kty, jsonWebKey.Kty);

            if (!string.IsNullOrEmpty(jsonWebKey.N))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.N, jsonWebKey.N);

            if (jsonWebKey.Oth.Count > 0)
                JsonSerializerPrimitives.WriteStrings(ref writer, JsonWebKeyParameterUtf8Bytes.Oth, jsonWebKey.Oth);

            if (!string.IsNullOrEmpty(jsonWebKey.P))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.P, jsonWebKey.P);

            if (!string.IsNullOrEmpty(jsonWebKey.Q))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.Q, jsonWebKey.Q);

            if (!string.IsNullOrEmpty(jsonWebKey.QI))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.QI, jsonWebKey.QI);

            if (!string.IsNullOrEmpty(jsonWebKey.Use))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.Use, jsonWebKey.Use);

            if (!string.IsNullOrEmpty(jsonWebKey.X))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.X, jsonWebKey.X);

            if (jsonWebKey.X5c.Count > 0)
                JsonSerializerPrimitives.WriteStrings(ref writer, JsonWebKeyParameterUtf8Bytes.X5c, jsonWebKey.X5c);

            if (!string.IsNullOrEmpty(jsonWebKey.X5t))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.X5t, jsonWebKey.X5t);

            if (!string.IsNullOrEmpty(jsonWebKey.X5tS256))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.X5tS256, jsonWebKey.X5tS256);

            if (!string.IsNullOrEmpty(jsonWebKey.X5u))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.X5u, jsonWebKey.X5u);

            if (!string.IsNullOrEmpty(jsonWebKey.Y))
                writer.WriteString(JsonWebKeyParameterUtf8Bytes.Y, jsonWebKey.Y);

            JsonSerializerPrimitives.WriteObjects(ref writer, jsonWebKey.AdditionalData);

            writer.WriteEndObject();
        }
        #endregion
    }
}


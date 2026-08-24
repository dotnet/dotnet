// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Json;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    public partial class JsonWebToken
    {
        internal JsonClaimSet CreatePayloadClaimSet(byte[] bytes, int length)
        {
            return CreatePayloadClaimSet(bytes.AsSpan(0, length));
        }

        internal JsonClaimSet CreatePayloadClaimSet(ReadOnlySpan<byte> byteSpan)
        {
            if (byteSpan.Length == 0)
                return new JsonClaimSet([]);

            Utf8JsonReader reader = new(byteSpan);
            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, true))
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                        Tokens.LogMessages.IDX11023,
                        LogHelper.MarkAsNonPII("JsonTokenType.StartObject"),
                        LogHelper.MarkAsNonPII(reader.TokenType),
                        LogHelper.MarkAsNonPII(ClassName),
                        LogHelper.MarkAsNonPII(reader.TokenStartIndex),
                        LogHelper.MarkAsNonPII(reader.CurrentDepth),
                        LogHelper.MarkAsNonPII(reader.BytesConsumed))));

            Dictionary<string, object> claims = [];
            while (true)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    ReadPayloadValue(ref reader, claims);
                }
                // We read a JsonTokenType.StartObject above, exiting and positioning reader at next token.
                else if (JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndObject, false))
                    break;
                else if (!reader.Read())
                    break;
            }

            return new JsonClaimSet(claims);
        }

        /// <summary>
        /// Reads the value of a claim in the payload and adds it to the <paramref name="claims"/> dictionary.
        /// Can be overridden to read and add custom claims.
        /// If a custom claim is read, the reader should be positioned at the next token after reading the claim.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader instance positioned at a claim name token used to read the JSON payload.</param>
        /// <param name="claims">Collection of claims that have been read from the reader.</param>
        private protected virtual void ReadPayloadValue(ref Utf8JsonReader reader, IDictionary<string, object> claims)
        {
            if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Aud))
            {
                List<string> _audiences = [];
                reader.Read();
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    JsonSerializerPrimitives.ReadStringsSkipNulls(ref reader, _audiences, JwtRegisteredClaimNames.Aud, ClassName);
                    claims[JwtRegisteredClaimNames.Aud] = _audiences;
                }
                else
                {
                    if (reader.TokenType != JsonTokenType.Null)
                    {
                        _audiences.Add(JsonSerializerPrimitives.ReadString(ref reader, JwtRegisteredClaimNames.Aud, ClassName));
                        claims[JwtRegisteredClaimNames.Aud] = _audiences[0];
                    }
                    else
                    {
                        claims[JwtRegisteredClaimNames.Aud] = _audiences;
                    }
                }
            }
            else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Azp))
            {
                claims[JwtRegisteredClaimNames.Azp] = JsonSerializerPrimitives.ReadString(ref reader, JwtRegisteredClaimNames.Azp, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Exp))
            {
                claims[JwtRegisteredClaimNames.Exp] = JsonSerializerPrimitives.ReadLong(ref reader, JwtRegisteredClaimNames.Exp, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Iat))
            {
                claims[JwtRegisteredClaimNames.Iat] = JsonSerializerPrimitives.ReadLong(ref reader, JwtRegisteredClaimNames.Iat, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Iss))
            {
                claims[JwtRegisteredClaimNames.Iss] = JsonSerializerPrimitives.ReadString(ref reader, JwtRegisteredClaimNames.Iss, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Jti))
            {
                claims[JwtRegisteredClaimNames.Jti] = JsonSerializerPrimitives.ReadString(ref reader, JwtRegisteredClaimNames.Jti, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Nbf))
            {
                claims[JwtRegisteredClaimNames.Nbf] = JsonSerializerPrimitives.ReadLong(ref reader, JwtRegisteredClaimNames.Nbf, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtPayloadUtf8Bytes.Sub))
            {
                claims[JwtRegisteredClaimNames.Sub] = JsonSerializerPrimitives.ReadStringOrNumberAsString(ref reader, JwtRegisteredClaimNames.Sub, ClassName, true);
            }
            else
            {
                string claimName = reader.GetString();

                if (TryReadJwtClaim != null)
                {
                    reader.Read(); // Move to the value
                    if (TryReadJwtClaim(ref reader, JwtSegmentType.Payload, claimName, out object claimValue))
                    {
                        claims[claimName] = claimValue;
                        reader.Read(); // Move to the next token
                    }
                    else
                    {
                        // The reader is positioned at the value token. The custom delegate did not read the value. Use our own logic.
                        claims[claimName] = JsonSerializerPrimitives.ReadPropertyValueAsObject(ref reader, claimName, JsonClaimSet.ClassName, false);
                    }
                }
                else
                {
                    // Move the reader forward to the value and read it using our own logic.
                    claims[claimName] = JsonSerializerPrimitives.ReadPropertyValueAsObject(ref reader, claimName, JsonClaimSet.ClassName, true);
                }
            }
        }
    }
}

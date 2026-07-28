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
        internal JsonClaimSet CreateHeaderClaimSet(byte[] bytes)
        {
            return CreateHeaderClaimSet(bytes.AsSpan());
        }

        internal JsonClaimSet CreateHeaderClaimSet(byte[] bytes, int length)
        {
            return CreateHeaderClaimSet(bytes.AsSpan(0, length));
        }

        internal JsonClaimSet CreateHeaderClaimSet(ReadOnlySpan<byte> byteSpan)
        {
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

            Dictionary<string, object> claims = new();
            while (true)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    ReadHeaderValue(ref reader, claims);
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
        /// Reads the value of a claim in the header and adds it to the <paramref name="claims"/> dictionary.
        /// Can be overridden to read and add custom claims.
        /// If a custom claim is read, the reader should be positioned at the next token after reading the claim.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader instance positioned at a claim name token used to read the JSON payload.</param>
        /// <param name="claims">Collection of claims that have been read from the reader.</param>
        private protected virtual void ReadHeaderValue(ref Utf8JsonReader reader, IDictionary<string, object> claims)
        {
            if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Alg))
            {
                claims[JwtHeaderParameterNames.Alg] = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Alg, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Cty))
            {
                claims[JwtHeaderParameterNames.Cty] = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Cty, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Kid))
            {
                claims[JwtHeaderParameterNames.Kid] = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Kid, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Typ))
            {
                claims[JwtHeaderParameterNames.Typ] = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Typ, ClassName, true); ;
            }
            else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.X5t))
            {
                claims[JwtHeaderParameterNames.X5t] = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.X5t, ClassName, true);
            }
            else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Zip))
            {
                claims[JwtHeaderParameterNames.Zip] = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Zip, ClassName, true);
            }
            else
            {
                string claimName = reader.GetString();

                if (TryReadJwtClaim != null)
                {
                    reader.Read(); // Move to the value
                    if (TryReadJwtClaim(ref reader, JwtSegmentType.Header, claimName, out object claimValue))
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

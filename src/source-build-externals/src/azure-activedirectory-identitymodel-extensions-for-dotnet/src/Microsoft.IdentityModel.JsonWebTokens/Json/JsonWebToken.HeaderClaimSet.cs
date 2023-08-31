// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Json;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    public partial class JsonWebToken
    {
        internal JsonClaimSet CreateHeaderClaimSet(byte[] bytes)
        {
            return CreateHeaderClaimSet(bytes, bytes.Length);
        }

        internal JsonClaimSet CreateHeaderClaimSet(byte[] bytes, int length)
        {
            Utf8JsonReader reader = new(bytes.AsSpan().Slice(0, length));
            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, false))
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
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Alg))
                    {
                        _alg = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Alg, ClassName, true);
                        claims[JwtHeaderParameterNames.Alg] = _alg;
                    }
                    else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Cty))
                    {
                        _cty = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Cty, ClassName, true);
                        claims[JwtHeaderParameterNames.Cty] = _cty;
                    }
                    else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Kid))
                    {
                        _kid = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Kid, ClassName, true);
                        claims[JwtHeaderParameterNames.Kid] = _kid;
                    }
                    else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Typ))
                    {
                        _typ = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Typ, ClassName, true);
                        claims[JwtHeaderParameterNames.Typ] = _typ;
                    }
                    else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.X5t))
                    {
                        _x5t = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.X5t, ClassName, true);
                        claims[JwtHeaderParameterNames.X5t] = _x5t;
                    }
                    else if (reader.ValueTextEquals(JwtHeaderUtf8Bytes.Zip))
                    {
                        _zip = JsonSerializerPrimitives.ReadString(ref reader, JwtHeaderParameterNames.Zip, ClassName, true);
                        claims[JwtHeaderParameterNames.Zip] = _zip;
                    }
                    else
                    {
                        string propertyName = reader.GetString();
                        claims[propertyName] = JsonSerializerPrimitives.ReadPropertyValueAsObject(ref reader, propertyName, JsonClaimSet.ClassName, true);
                    }
                }
                else if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }
            };

            return new JsonClaimSet(claims);
        }
    }
}

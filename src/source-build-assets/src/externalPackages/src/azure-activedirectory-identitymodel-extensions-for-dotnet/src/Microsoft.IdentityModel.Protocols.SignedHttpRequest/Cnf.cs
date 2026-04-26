// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Json;

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest
{
    /// <summary>
    /// Represents the Cnf Claim
    /// </summary>
    internal class Cnf
    {
        internal const string ClassName = "Microsoft.IdentityModel.Protocols.SignedHttpRequest.Cnf";

        public Cnf() { }

        public Cnf(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json).AsSpan());
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

            while (true)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.ValueTextEquals(ConfirmationClaimTypesUtf8Bytes.Jwk))
                    {
                        reader.Read();
                        JsonWebKey = JsonWebKeySerializer.Read(ref reader, new JsonWebKey());
                    }
                    else if (reader.ValueTextEquals(ConfirmationClaimTypesUtf8Bytes.Kid))
                    {
                        Kid = JsonSerializerPrimitives.ReadString(ref reader, ConfirmationClaimTypes.Kid, ClassName, true);
                    }
                    else if (reader.ValueTextEquals(ConfirmationClaimTypesUtf8Bytes.Jku))
                    {
                        Jku = JsonSerializerPrimitives.ReadString(ref reader, ConfirmationClaimTypes.Kid, ClassName, true);
                    }
                    else if (reader.ValueTextEquals(ConfirmationClaimTypesUtf8Bytes.Jwe))
                    {
                        Jwe = JsonSerializerPrimitives.ReadString(ref reader, ConfirmationClaimTypes.Jwe, ClassName, true);
                    }
                    else
                    {
                        // this is not a property we recognize, skip it.
                        reader.Skip();
                    }
                }
                // We read a JsonTokenType.StartObject above, exiting and positioning reader at next token.
                else if (JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.EndObject, true))
                    break;
                else if (!reader.Read())
                        break;
            }
        }

        [JsonPropertyName("kid")]
        public string Kid { get; set; }

        [JsonPropertyName("jwe")]
        public string Jwe { get; set; }

        [JsonPropertyName("jku")]
        public string Jku { get; set; }

        [JsonPropertyName("jwk")]
        public JsonWebKey JsonWebKey{ get; set; }
    } 
}

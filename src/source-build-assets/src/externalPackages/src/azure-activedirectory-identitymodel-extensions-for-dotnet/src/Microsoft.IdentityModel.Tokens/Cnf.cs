// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens.Json;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Represents the Cnf Claim
    /// </summary>
    public class Cnf
    {
        /// <summary>
        /// The class name used for logging and exception messages.
        /// </summary>
        internal string ClassName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cnf"/> class.
        /// </summary>
        public Cnf()
        {
            // GetType returns the runtime type, e.g. if derived,
            // it will return the derived type.
            ClassName = GetType().FullName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cnf"/> class.
        /// </summary>
        /// <param name="json">
        /// JSON representation of the Cnf Claim.
        /// </param>
        public Cnf(string json) : this()
        {
            if (string.IsNullOrEmpty(json))
                throw LogHelper.LogArgumentNullException(nameof(json));

            Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json).AsSpan());
            if (!JsonSerializerPrimitives.IsReaderAtTokenType(ref reader, JsonTokenType.StartObject, true))
                throw LogHelper.LogExceptionMessage(
                    new JsonException(
                        LogHelper.FormatInvariant(
                        LogMessages.IDX11023,
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
                    else if (reader.ValueTextEquals(ConfirmationClaimTypesUtf8Bytes.Jkt))
                    {
                        Jkt = JsonSerializerPrimitives.ReadString(ref reader, ConfirmationClaimTypes.Jkt, ClassName, true);
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

        /// <summary>
        /// The Kid property is used to specify the key ID of the public key used to verify the signature of the JWT.
        /// </summary>
        [JsonPropertyName("kid")]
        public string Kid { get; internal set; }

        /// <summary>
        /// The Jwe property is used to specify an encrypted JSON web key.
        /// </summary>
        [JsonPropertyName("jwe")]
        public string Jwe { get; internal set; }

        /// <summary>
        /// The Jku property is used to specify the location of the public key used to verify the signature of the JWT.
        /// </summary>
        [JsonPropertyName("jku")]
        public string Jku { get; internal set; }

        /// <summary>
        /// The Jkt property is used for DPoP, it is the base64url encoded JWK SHA-256 thumbprint.
        /// </summary>
        [JsonPropertyName("jkt")]
        public string Jkt { get; internal set; }

        /// <summary>
        /// The Jwk property is used to specify the public key used to verify the signature of the JWT.
        /// </summary>
        [JsonPropertyName("jwk")]
        public JsonWebKey JsonWebKey { get; internal set; }
    }
}

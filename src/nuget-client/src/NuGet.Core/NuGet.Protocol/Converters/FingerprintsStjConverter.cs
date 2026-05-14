// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Packaging.Core;

namespace NuGet.Protocol.Converters
{
    internal sealed class FingerprintsStjConverter : JsonConverter<Fingerprints>
    {
        public override Fingerprints Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var dict = new Dictionary<string, string>();
            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                // Key token: the loop condition guarantees JsonTokenType.PropertyName, so GetString() is never null.
                string key = reader.GetString()!;
                reader.Read();
                // Value token: may be null (e.g. "key": null), default to empty string.
                dict[key] = reader.GetString() ?? string.Empty;
            }

            return new Fingerprints(dict);
        }

        public override void Write(Utf8JsonWriter writer, Fingerprints value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<string, string> kvp in value)
            {
                writer.WriteString(kvp.Key, kvp.Value);
            }
            writer.WriteEndObject();
        }
    }
}

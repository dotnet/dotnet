// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuGet.Protocol.Converters
{
    internal sealed class ServiceIndexEntryStringOrArrayConverter : JsonConverter<string[]>
    {
        public override string[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return [reader.GetString()!];
            }

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                string? value = null;
                List<string>? values = null;

                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    if (reader.TokenType == JsonTokenType.String)
                    {
                        if (value is null)
                        {
                            value = reader.GetString()!;
                        }
                        else
                        {
                            (values ??= [value]).Add(reader.GetString()!);
                        }
                    }
                }

                if (value is null) { return []; }
                if (values is null) { return [value]; }
                return [.. values];
            }

            reader.Skip();
            return [];
        }

        public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
        {
            if (value.Length == 1)
            {
                writer.WriteStringValue(value[0]);
            }
            else
            {
                writer.WriteStartArray();
                foreach (var item in value)
                {
                    writer.WriteStringValue(item);
                }
                writer.WriteEndArray();
            }
        }
    }
}

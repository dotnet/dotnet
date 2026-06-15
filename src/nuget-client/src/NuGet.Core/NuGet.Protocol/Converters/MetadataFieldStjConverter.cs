// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuGet.Protocol.Converters
{
    /// <summary>
    /// Reads a JSON string or array of strings into a single comma-separated string.
    /// Equivalent to <see cref="MetadataFieldConverter"/> for System.Text.Json.
    /// </summary>
    /// <remarks>
    /// NSJ equivalent: <see cref="MetadataFieldConverter"/>.
    /// Used by: <see cref="PackageSearchMetadata.Authors"/>, <see cref="PackageSearchMetadata.Tags"/>.
    /// </remarks>
    internal sealed class MetadataFieldStjConverter : JsonConverter<string>
    {
        public override bool HandleNull => true;

        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return string.Empty;
            }

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                string? first = null;
                List<string>? rest = null;

                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    string? s = reader.TokenType == JsonTokenType.Null
                        ? null
                        : reader.CoerceScalarTokenToString();

                    if (string.IsNullOrWhiteSpace(s)) { continue; }

                    if (first is null)
                    {
                        first = s;
                    }
                    else
                    {
                        (rest ??= []).Add(s!);
                    }
                }

                if (first is null) { return string.Empty; }
                if (rest is null) { return first; }
                return first + ", " + string.Join(", ", rest);
            }

            return reader.CoerceScalarTokenToString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
            => throw new NotSupportedException();
    }
}

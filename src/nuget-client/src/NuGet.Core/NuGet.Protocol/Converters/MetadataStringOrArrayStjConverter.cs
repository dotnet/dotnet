// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuGet.Protocol.Converters
{
    /// <summary>
    /// Reads a JSON string or array of strings into a <see cref="IReadOnlyList{T}"/> of strings.
    /// Equivalent to <see cref="MetadataStringOrArrayConverter"/> for System.Text.Json.
    /// </summary>
    /// <remarks>
    /// NSJ equivalent: <see cref="MetadataStringOrArrayConverter"/>.
    /// Used by: <see cref="PackageSearchMetadata.OwnersList"/>.
    /// </remarks>
    internal sealed class MetadataStringOrArrayStjConverter : JsonConverter<IReadOnlyList<string>>
    {
        public override bool HandleNull => true;

        public override IReadOnlyList<string>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_UnexpectedJsonToken, reader.TokenType));
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                return string.IsNullOrWhiteSpace(str) ? null : [str!];
            }

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_UnexpectedJsonToken, reader.TokenType));
            }

            reader.Read();
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return Array.Empty<string>();
            }

            string? first = reader.TokenType == JsonTokenType.Null ? null : reader.CoerceScalarTokenToString();

            reader.Read();
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return first is null ? Array.Empty<string>() : new[] { first };
            }

            var values = new List<string>();
            if (first is not null)
            {
                values.Add(first);
            }

            do
            {
                if (reader.TokenType != JsonTokenType.Null)
                {
                    values.Add(reader.CoerceScalarTokenToString());
                }
            }
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray);

            return values;
        }

        public override void Write(Utf8JsonWriter writer, IReadOnlyList<string> value, JsonSerializerOptions options)
            => throw new NotSupportedException();
    }
}


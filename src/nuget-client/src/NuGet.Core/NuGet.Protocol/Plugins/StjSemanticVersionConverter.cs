// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Versioning;

namespace NuGet.Protocol.Plugins
{
    internal sealed class StjSemanticVersionConverter : JsonConverter<SemanticVersion>
    {
        public override SemanticVersion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException(string.Format(CultureInfo.CurrentCulture, Strings.Error_UnexpectedJsonToken, reader.TokenType));
            }

            var str = reader.GetString()!;
            return SemanticVersion.Parse(str);
        }

        public override void Write(Utf8JsonWriter writer, SemanticVersion value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Versioning;

namespace NuGet.Protocol.Converters
{
    /// <summary>
    /// A <see cref="JsonConverter{T}"/> to allow System.Text.Json to read/write <see cref="VersionRange"/>
    /// </summary>
    internal class VersionRangeStjConverter : JsonConverter<VersionRange>
    {
        public override VersionRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (stringValue == null)
            {
                // This is actually impossible to get to, because JsonSerializer won't call into the converter when the JSON is null
                throw new InvalidOperationException("Value for version range cannot be null");
            }

            return VersionRange.Parse(stringValue);
        }

        public override void Write(Utf8JsonWriter writer, VersionRange value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.ToNormalizedString());
            }
        }
    }
}

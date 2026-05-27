// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Frameworks;

namespace NuGet.Protocol.Converters
{
    /// <remarks>
    /// NSJ equivalent: <see cref="NuGetFrameworkConverter"/> (registered globally in <see cref="JsonExtensions.ObjectSerializationSettings"/>).
    /// Used by: <see cref="NuGetFramework"/> properties.
    /// </remarks>
    internal sealed class NuGetFrameworkStjConverter : JsonConverter<NuGetFramework>
    {
        public override bool HandleNull => true;
        public override NuGetFramework Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Delegate parsing to NuGetFramework.Parse. it handles unknown/unsupported values gracefully.
            string? value = reader.TokenType switch
            {
                JsonTokenType.Null => null,
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.True => bool.TrueString,
                JsonTokenType.False => bool.FalseString,
                JsonTokenType.Number => reader.CoerceScalarTokenToString(),
                _ => throw new JsonException(string.Format(System.Globalization.CultureInfo.CurrentCulture, Strings.Error_UnexpectedJsonToken, reader.TokenType))
            };

            return string.IsNullOrEmpty(value) ? NuGetFramework.AnyFramework : NuGetFramework.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, NuGetFramework value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.GetShortFolderName());
    }
}

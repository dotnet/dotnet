// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuGet.Protocol.Converters
{
    /// <remarks>
    /// NSJ equivalent: <see cref="SafeBoolConverter"/>.
    /// Used by: <see cref="PackageSearchMetadata.RequireLicenseAcceptance"/>.
    /// </remarks>
    internal sealed class SafeBoolStjConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                case JsonTokenType.Null:
                    return false;
                case JsonTokenType.String:
                    return bool.TryParse(reader.GetString(), out bool flag) && flag;
                case JsonTokenType.Number:
                    return reader.TryGetInt64(out long l) && l == 1;
                default:
                    reader.Skip();
                    return false;
            }
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }
    }
}

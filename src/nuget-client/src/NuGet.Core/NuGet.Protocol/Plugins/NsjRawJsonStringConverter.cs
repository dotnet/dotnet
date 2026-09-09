// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
#if NET5_0_OR_GREATER
    [RequiresUnreferencedCode("Uses Newtonsoft.Json reflection-based serialization.")]
    [RequiresDynamicCode("Uses Newtonsoft.Json reflection-based serialization.")]
#endif
    internal sealed class NsjRawJsonStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonSerializationException(
                    string.Format(CultureInfo.CurrentCulture, Strings.Error_UnexpectedJsonToken, reader.TokenType));
            }

            var obj = JObject.Load(reader);
            return obj.ToString(Formatting.None, Array.Empty<JsonConverter>());
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is string s)
            {
                JObject.Parse(s).WriteTo(writer, Array.Empty<JsonConverter>());
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}

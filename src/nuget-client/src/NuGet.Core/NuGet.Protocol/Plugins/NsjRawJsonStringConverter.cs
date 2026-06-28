// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
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
#pragma warning disable IL2026, IL3050 // WriteTo without converters is safe. See https://github.com/JamesNK/Newtonsoft.Json/blob/13.0.4/Src/Newtonsoft.Json/Linq/JToken.cs
            return obj.ToString(Formatting.None, Array.Empty<JsonConverter>());
#pragma warning restore IL2026, IL3050
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is string s)
            {
#pragma warning disable IL2026, IL3050 // WriteTo without converters is safe. See https://github.com/JamesNK/Newtonsoft.Json/blob/13.0.4/Src/Newtonsoft.Json/Linq/JToken.cs
                JObject.Parse(s).WriteTo(writer, Array.Empty<JsonConverter>());
#pragma warning restore IL2026, IL3050
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}

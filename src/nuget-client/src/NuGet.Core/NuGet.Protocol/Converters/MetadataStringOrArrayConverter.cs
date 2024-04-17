// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol
{
    internal class MetadataStringOrArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => (objectType == typeof(string) || objectType == typeof(string[]));

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.None)
            {
                return string.Empty;
            }

            if (reader.TokenType == JsonToken.String)
            {
                var str = serializer.Deserialize<string>(reader);
                return string.IsNullOrWhiteSpace(str) ? null : new string[] { str };
            }

            return serializer.Deserialize<string[]>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

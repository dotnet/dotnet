// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol
{
    public class MetadataFieldConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => (objectType == typeof(string));

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return string.Empty;
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                var array = JArray.Load(reader);
                return string.Join(", ", array.Values<string>().Where(s => !string.IsNullOrWhiteSpace(s)));
            }

            return serializer.Deserialize<string>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

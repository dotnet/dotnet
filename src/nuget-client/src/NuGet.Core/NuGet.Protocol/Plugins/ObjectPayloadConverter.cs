// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NuGet.Protocol.Plugins
{
    internal sealed class ObjectPayloadConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => throw new NotSupportedException();

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new JsonSerializationException(
                    string.Format(
                        System.Globalization.CultureInfo.CurrentCulture,
                        Strings.Error_UnexpectedJsonToken,
                        reader.TokenType));
            }

            return JObject.Load(reader);
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
            => throw new NotSupportedException();
    }
}

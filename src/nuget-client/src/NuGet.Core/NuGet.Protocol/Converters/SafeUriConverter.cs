// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol
{
    public class SafeUriConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string);
        public override bool CanRead { get { return true; } }
        public override bool CanWrite { get { return false; } }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return null;
                case JsonToken.String:
                    Uri uri;
                    if (Uri.TryCreate(reader.Value.ToString().Trim(), UriKind.Absolute, out uri))
                    {
                        return uri;
                    }
                    return null;
                default:
                    reader.Skip();
                    return null;
            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

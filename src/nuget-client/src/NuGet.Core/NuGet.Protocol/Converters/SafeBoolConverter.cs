// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Protocol
{
    public class SafeBoolConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(bool);
        public override bool CanRead { get { return true; } }
        public override bool CanWrite { get { return false; } }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return false;
                case JsonToken.Boolean:
                    return serializer.Deserialize<bool>(reader);
                case JsonToken.String:
                    bool flag;
                    if (Boolean.TryParse(reader.Value.ToString().Trim(), out flag))
                    {
                        return flag;
                    }
                    return false;
                case JsonToken.Integer:
                    return ((long)reader.Value) == 1;
                default:
                    reader.Skip();
                    return false;
            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

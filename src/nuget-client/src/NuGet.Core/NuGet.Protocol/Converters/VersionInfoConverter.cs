// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class VersionInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(VersionInfo);

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var v = JsonUtility.LoadJson(reader);
            var nugetVersion = NuGetVersion.Parse(v.Value<string>("version"));
            var count = v.Value<long?>("downloads");
            return new VersionInfo(nugetVersion, count);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

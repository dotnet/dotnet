// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NuGet.Packaging.Core;

namespace NuGet.Protocol
{
    public class FingerprintsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => (objectType == typeof(Fingerprints));

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var v = JsonUtility.LoadJson(reader);

            var dict = new Dictionary<string, string>();

            foreach (var fingerprint in v)
            {
                dict[fingerprint.Key] = fingerprint.Value.ToString();
            }

            return new Fingerprints(dict);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

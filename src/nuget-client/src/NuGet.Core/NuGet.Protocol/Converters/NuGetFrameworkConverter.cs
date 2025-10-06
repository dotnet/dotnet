// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using NuGet.Frameworks;

namespace NuGet.Protocol.Converters
{
    internal class NuGetFrameworkConverter : JsonConverter<NuGetFramework>
    {
        public override void WriteJson(JsonWriter writer, NuGetFramework value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override NuGetFramework ReadJson(JsonReader reader, Type objectType, NuGetFramework existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString();
            var framework = NuGetFramework.AnyFramework;

            if (!string.IsNullOrEmpty(value))
            {
                framework = NuGetFramework.Parse(value);
            }

            return framework;
        }
    }
}

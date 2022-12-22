// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins.Tests
{
    internal static class TestUtilities
    {
        internal static string Serialize(object value)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                JsonSerializationUtilities.Serialize(jsonWriter, value);

                return stringWriter.ToString();
            }
        }
    }
}

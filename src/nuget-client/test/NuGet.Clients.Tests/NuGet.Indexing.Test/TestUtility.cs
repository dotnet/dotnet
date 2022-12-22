// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace NuGet.Indexing.Test
{
    internal static class TestUtility
    {
        public static PackageSearchMetadata[] LoadTestResponse(string fileName)
        {
            var assembly = typeof(SearchResultsAggregatorTests).Assembly;

            var serializer = JsonSerializer.Create(JsonExtensions.ObjectSerializationSettings);

            var resourcePath = string.Join(".", typeof(SearchResultsAggregatorTests).Namespace, "compiler.resources", fileName);
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    return null;
                }

                using (var streamReader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    return serializer.Deserialize<PackageSearchMetadata[]>(jsonReader);
                }
            }
        }
    }
}

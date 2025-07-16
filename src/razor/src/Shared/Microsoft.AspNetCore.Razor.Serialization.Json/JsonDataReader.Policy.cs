// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal partial class JsonDataReader
{
    private sealed class Policy : IPooledObjectPolicy<JsonDataReader>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public JsonDataReader Create() => new();

        public bool Return(JsonDataReader dataWriter)
        {
            dataWriter._reader = null;
            return true;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal partial class JsonDataWriter
{
    private sealed class Policy : IPooledObjectPolicy<JsonDataWriter>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public JsonDataWriter Create() => new();

        public bool Return(JsonDataWriter dataWriter)
        {
            dataWriter._writer = null;
            return true;
        }
    }
}

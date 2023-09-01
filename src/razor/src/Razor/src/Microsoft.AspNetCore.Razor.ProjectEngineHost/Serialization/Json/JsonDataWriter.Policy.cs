// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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

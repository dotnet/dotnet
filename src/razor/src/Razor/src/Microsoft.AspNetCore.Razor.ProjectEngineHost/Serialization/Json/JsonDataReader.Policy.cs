// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class IncrementalHashPool
{
    private class Policy : IPooledObjectPolicy<IncrementalHash>
    {
        public static readonly Policy Instance = new();

        private Policy()
        {
        }

        public IncrementalHash Create() => IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

        public bool Return(IncrementalHash hash)
        {
            Debug.Assert(hash.AlgorithmName == HashAlgorithmName.SHA256);

            return true;
        }
    }
}

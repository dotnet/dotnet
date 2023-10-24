// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.ObjectPool;
#if NET5_0_OR_GREATER
using System.Diagnostics;
#endif

#if NET5_0_OR_GREATER
using HashAlgorithmName = System.Security.Cryptography.HashAlgorithmName;
using HashingType = System.Security.Cryptography.IncrementalHash;
#else
using HashingType = System.Security.Cryptography.SHA256;
#endif

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed partial record Checksum
{
    internal readonly ref partial struct Builder
    {
        private sealed class Policy : IPooledObjectPolicy<HashingType>
        {
            public static readonly Policy Instance = new();

            private Policy()
            {
            }

            public HashingType Create()
#if NET5_0_OR_GREATER
                => HashingType.CreateHash(HashAlgorithmName.SHA256);
#else
                => HashingType.Create();
#endif

            public bool Return(HashingType hash)
            {
#if NET5_0_OR_GREATER
                Debug.Assert(hash.AlgorithmName == HashAlgorithmName.SHA256);
#endif

                return true;
            }
        }
    }
}

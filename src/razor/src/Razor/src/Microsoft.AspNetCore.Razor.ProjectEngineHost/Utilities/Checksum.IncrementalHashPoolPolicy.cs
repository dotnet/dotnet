// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Security.Cryptography;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed partial record Checksum
{
    private class IncrementalHashPoolPolicy : IPooledObjectPolicy<IncrementalHash>
    {
        public static readonly IncrementalHashPoolPolicy Instance = new();

        private IncrementalHashPoolPolicy()
        {
        }

        public IncrementalHash Create() => IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

        public bool Return(IncrementalHash obj) => true;
    }
}

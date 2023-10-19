// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Security.Cryptography;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="IncrementalHash"/> instances.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class IncrementalHashPool
{
    public static readonly ObjectPool<IncrementalHash> Default = DefaultPool.Create(Policy.Instance);

    public static PooledObject<IncrementalHash> GetPooledObject()
        => Default.GetPooledObject();

    public static PooledObject<IncrementalHash> GetPooledObject(out IncrementalHash hash)
        => Default.GetPooledObject(out hash);
}

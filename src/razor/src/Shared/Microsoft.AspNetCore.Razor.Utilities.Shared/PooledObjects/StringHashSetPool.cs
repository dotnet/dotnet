// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="HashSet{T}"/> instances that compares strings.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class StringHashSetPool
{
    public static readonly ObjectPool<HashSet<string>> Ordinal = HashSetPool<string>.Create(StringComparer.Ordinal);
    public static readonly ObjectPool<HashSet<string>> OrdinalIgnoreCase = HashSetPool<string>.Create(StringComparer.OrdinalIgnoreCase);

    public static ObjectPool<HashSet<string>> Create(IEqualityComparer<string> comparer)
        => HashSetPool<string>.Create(comparer);

    public static PooledObject<HashSet<string>> GetPooledObject()
        => Ordinal.GetPooledObject();

    public static PooledObject<HashSet<string>> GetPooledObject(out HashSet<string> set)
        => Ordinal.GetPooledObject(out set);

    public static PooledObject<HashSet<string>> GetPooledObject(bool ignoreCase)
        => ignoreCase
            ? OrdinalIgnoreCase.GetPooledObject()
            : Ordinal.GetPooledObject();

    public static PooledObject<HashSet<string>> GetPooledObject(bool ignoreCase, out HashSet<string> set)
        => ignoreCase
            ? OrdinalIgnoreCase.GetPooledObject(out set)
            : Ordinal.GetPooledObject(out set);
}

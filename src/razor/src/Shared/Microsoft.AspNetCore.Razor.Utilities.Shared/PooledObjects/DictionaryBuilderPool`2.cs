// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="ImmutableDictionary{TKey, TValue}.Builder"/> instances.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class DictionaryBuilderPool<TKey, TValue>
    where TKey : notnull
{
    public static readonly ObjectPool<ImmutableDictionary<TKey, TValue>.Builder> Default = DefaultPool.Create(Policy.Instance);

    public static ObjectPool<ImmutableDictionary<TKey, TValue>.Builder> Create(IEqualityComparer<TKey> comparer)
        => DefaultPool.Create(new Policy(comparer));

    public static PooledObject<ImmutableDictionary<TKey, TValue>.Builder> GetPooledObject()
        => Default.GetPooledObject();

    public static PooledObject<ImmutableDictionary<TKey, TValue>.Builder> GetPooledObject(out ImmutableDictionary<TKey, TValue>.Builder builder)
        => Default.GetPooledObject(out builder);
}

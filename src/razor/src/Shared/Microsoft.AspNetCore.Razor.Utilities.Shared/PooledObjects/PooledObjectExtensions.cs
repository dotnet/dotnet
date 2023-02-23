// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static partial class PooledObjectExtensions
{
    public static PooledObject<ImmutableArray<T>.Builder> GetPooledObject<T>(this ObjectPool<ImmutableArray<T>.Builder> pool)
        => new(pool);

    public static PooledObject<ImmutableArray<T>.Builder> GetPooledObject<T>(
        this ObjectPool<ImmutableArray<T>.Builder> pool,
        out ImmutableArray<T>.Builder builder)
    {
        var pooledObject = pool.GetPooledObject();
        builder = pooledObject.Object;
        return pooledObject;
    }

    public static PooledObject<Dictionary<TKey, TValue>> GetPooledObject<TKey, TValue>(this ObjectPool<Dictionary<TKey, TValue>> pool)
        where TKey : notnull
        => new(pool);

    public static PooledObject<Dictionary<TKey, TValue>> GetPooledObject<TKey, TValue>(
        this ObjectPool<Dictionary<TKey, TValue>> pool,
        out Dictionary<TKey, TValue> map)
        where TKey : notnull
    {
        var pooledObject = pool.GetPooledObject();
        map = pooledObject.Object;
        return pooledObject;
    }

    public static PooledObject<HashSet<T>> GetPooledObject<T>(this ObjectPool<HashSet<T>> pool)
        => new(pool);

    public static PooledObject<HashSet<T>> GetPooledObject<T>(
        this ObjectPool<HashSet<T>> pool,
        out HashSet<T> set)
    {
        var pooledObject = pool.GetPooledObject();
        set = pooledObject.Object;
        return pooledObject;
    }

    public static PooledObject<List<T>> GetPooledObject<T>(this ObjectPool<List<T>> pool)
        => new(pool);

    public static PooledObject<List<T>> GetPooledObject<T>(
        this ObjectPool<List<T>> pool,
        out List<T> list)
    {
        var pooledObject = pool.GetPooledObject();
        list = pooledObject.Object;
        return pooledObject;
    }

    public static PooledObject<Stack<T>> GetPooledObject<T>(this ObjectPool<Stack<T>> pool)
        => new(pool);

    public static PooledObject<Stack<T>> GetPooledObject<T>(
        this ObjectPool<Stack<T>> pool,
        out Stack<T> stack)
    {
        var pooledObject = pool.GetPooledObject();
        stack = pooledObject.Object;
        return pooledObject;
    }

    public static PooledObject<Stopwatch> GetPooledObject(this ObjectPool<Stopwatch> pool)
        => new(pool);

    public static PooledObject<Stopwatch> GetPooledObject(
        this ObjectPool<Stopwatch> pool,
        out Stopwatch watch)
    {
        var pooledObject = pool.GetPooledObject();
        watch = pooledObject.Object;
        return pooledObject;
    }

    public static PooledObject<StringBuilder> GetPooledObject(this ObjectPool<StringBuilder> pool)
        => new(pool);

    public static PooledObject<StringBuilder> GetPooledObject(
        this ObjectPool<StringBuilder> pool,
        out StringBuilder builder)
    {
        var pooledObject = pool.GetPooledObject();
        builder = pooledObject.Object;
        return pooledObject;
    }
}

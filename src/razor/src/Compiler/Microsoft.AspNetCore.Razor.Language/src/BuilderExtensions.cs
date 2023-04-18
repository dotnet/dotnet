// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language;

internal static class BuilderExtensions
{
    public static T[] BuildAll<T, TBuilder>(this List<TBuilder> builders, ObjectPool<HashSet<T>> pool)
        where TBuilder : IBuilder<T>
    {
        if (builders.Count == 0)
        {
            return Array.Empty<T>();
        }

        if (builders.Count == 1)
        {
            return new[] { builders[0].Build() };
        }

        using var _1 = ListPool<T>.GetPooledObject(out var result);
        using var _2 = pool.GetPooledObject(out var set);

        foreach (var builder in builders)
        {
            var item = builder.Build();

            if (set.Add(item))
            {
                result.Add(item);
            }
        }

        return result.ToArray();
    }

    public static T[] BuildAllOrEmpty<T, TBuilder>(this List<TBuilder>? builders, ObjectPool<HashSet<T>> pool)
        where TBuilder : IBuilder<T>
        => builders?.BuildAll(pool) ?? Array.Empty<T>();
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static partial class SyntaxListBuilderPool
{
    public static readonly ObjectPool<SyntaxListBuilder> Default = DefaultPool.Create(Policy.Instance);

    public static PooledObject<SyntaxListBuilder> GetPooledBuilder()
        => Default.GetPooledBuilder();

    public static PooledObject<SyntaxListBuilder> GetPooledBuilder(out SyntaxListBuilder builder)
        => Default.GetPooledBuilder(out builder);

    public static PooledBuilder<T> GetPooledBuilder<T>()
        where T : SyntaxNode
        => Default.GetPooledBuilder<T>();

    public static PooledBuilder<T> GetPooledBuilder<T>(out SyntaxListBuilder<T> builder)
        where T : SyntaxNode
        => Default.GetPooledBuilder(out builder);

    public static PooledObject<SyntaxListBuilder> GetPooledBuilder(this ObjectPool<SyntaxListBuilder> pool)
        => new(pool);

    public static PooledObject<SyntaxListBuilder> GetPooledBuilder(this ObjectPool<SyntaxListBuilder> pool, out SyntaxListBuilder builder)
    {
        var pooledObject = pool.GetPooledBuilder();
        builder = pooledObject.Object;
        return pooledObject;
    }

    public static PooledBuilder<T> GetPooledBuilder<T>(this ObjectPool<SyntaxListBuilder> pool)
        where T : SyntaxNode
        => new(pool);

    public static PooledBuilder<T> GetPooledBuilder<T>(this ObjectPool<SyntaxListBuilder> pool, out SyntaxListBuilder<T> builder)
        where T : SyntaxNode
    {
        var pooledBuilder = pool.GetPooledBuilder<T>();
        builder = pooledBuilder.Builder;
        return pooledBuilder;
    }
}

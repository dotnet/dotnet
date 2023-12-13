// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Language.Syntax;

internal static partial class SyntaxListBuilderPool
{
    public struct PooledBuilder<T> : IDisposable
        where T : SyntaxNode
    {
        private readonly ObjectPool<SyntaxListBuilder> _pool;
        private SyntaxListBuilder<T>? _builder;

        public readonly SyntaxListBuilder<T> Builder => _builder.GetValueOrDefault();

        public PooledBuilder(ObjectPool<SyntaxListBuilder> pool)
            : this()
        {
            _pool = pool;
            _builder = new SyntaxListBuilder<T>(pool.Get());
        }

        public void Dispose()
        {
            if (_builder is { } obj)
            {
                _pool.Return(obj.Builder);
                _builder = null;
            }
        }
    }
}

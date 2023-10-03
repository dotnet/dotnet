// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal struct PooledArray<T> : IDisposable
{
    private readonly ArrayPool<T> _pool;
    private T[]? _array;

    // Because of how this API is intended to be used, we don't want the consumption code to have
    // to deal with Array being a nullable reference type. Instead, the guarantee is that this is
    // non-null until this is disposed.
    public readonly T[] Array => _array!;

    public PooledArray(ArrayPool<T> pool, int minimumLength)
        : this()
    {
        _pool = pool;
        _array = pool.Rent(minimumLength);
    }

    public void Dispose()
    {
        if (_array is T[] array)
        {
            _pool.Return(array);
            _array = null;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.PooledObjects;

namespace System.Buffers;

internal static class BufferExtensions
{
    public static PooledArray<T> GetPooledArray<T>(this ArrayPool<T> pool, int minimumLength, out T[] array)
    {
        var result = new PooledArray<T>(pool, minimumLength);
        array = result.Array;
        return result;
    }
}

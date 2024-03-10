// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal static class PooledArrayBuilder
{
    public static PooledArrayBuilder<T> Create<T>(ReadOnlySpan<T> source)
    {
        var pooledArray = new PooledArrayBuilder<T>(source.Length);
        pooledArray.AddRange(source);
        return pooledArray;
    }
}

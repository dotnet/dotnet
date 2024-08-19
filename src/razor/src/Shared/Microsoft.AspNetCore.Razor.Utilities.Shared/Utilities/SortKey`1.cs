// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Buffers;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal readonly record struct SortKey<T>(int Index, T Value)
{
    public static PooledArray<SortKey<T>> GetPooledArray(int minimumLength)
        => ArrayPool<SortKey<T>>.Shared.GetPooledArray(minimumLength);
}

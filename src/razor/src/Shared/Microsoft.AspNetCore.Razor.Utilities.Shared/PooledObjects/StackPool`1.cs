// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="Stack{T}"/> instances.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class StackPool<T>
{
    public static readonly ObjectPool<Stack<T>> Default = DefaultPool.Create(Policy.Instance);

    public static PooledObject<Stack<T>> GetPooledObject() => Default.GetPooledObject();

    public static PooledObject<Stack<T>> GetPooledObject(out Stack<T> stack)
        => Default.GetPooledObject(out stack);
}

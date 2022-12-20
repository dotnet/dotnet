// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

/// <summary>
/// A pool of <see cref="StringBuilder"/> instances.
/// </summary>
/// 
/// <remarks>
/// Instances originating from this pool are intended to be short-lived and are suitable
/// for temporary work. Do not return them as the results of methods or store them in fields.
/// </remarks>
internal static partial class StringBuilderPool
{
    public static readonly ObjectPool<StringBuilder> Default = DefaultPool.Create(Policy.Instance);

    public static PooledObject<StringBuilder> GetPooledObject()
        => Default.GetPooledObject();

    public static PooledObject<StringBuilder> GetPooledObject(out StringBuilder builder)
        => Default.GetPooledObject(out builder);
}

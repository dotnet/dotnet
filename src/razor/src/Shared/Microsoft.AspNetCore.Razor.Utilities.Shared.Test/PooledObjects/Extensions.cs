// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test.PooledObjects;

internal static class Extensions
{
    public static void Validate<T>(this ref readonly PooledArrayBuilder<T> builder, Action<PooledArrayBuilder<T>.TestAccessor> validator)
    {
        validator(builder.GetTestAccessor());
    }
}

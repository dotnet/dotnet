// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Microsoft.AspNetCore.Razor;

internal static class InterlockedOperations
{
    public static T Initialize<T>([NotNull] ref T? target, T value)
        where T : class
        => Interlocked.CompareExchange(ref target, value, null) ?? value;
}

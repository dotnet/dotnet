// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

#if NETFRAMEWORK || NETSTANDARD2_0

using System.Diagnostics;

namespace System.Collections.Generic;

internal static class DeconstructionExtensions
{
    [DebuggerStepThrough]
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
    {
        key = pair.Key;
        value = pair.Value;
    }
}

#endif

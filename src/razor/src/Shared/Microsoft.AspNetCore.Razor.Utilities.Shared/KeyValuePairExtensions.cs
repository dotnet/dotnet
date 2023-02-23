// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace System.Collections.Generic;

internal static class KeyValuePairExtensions
{
    /// <summary>
    /// Deconstructs a <see cref="KeyValuePair{TKey, TValue}"/> into out variables. Provides support
    /// for assignment like
    /// <code>
    /// var (k,v) = kvp;
    /// </code>
    /// </summary>
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        => (key, value) = (kvp.Key, kvp.Value);
}

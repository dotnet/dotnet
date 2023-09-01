// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#if !NETCOREAPP3_0_OR_GREATER

namespace System.Collections.Generic;

internal static class KeyValuePair
{
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) =>
        new KeyValuePair<TKey, TValue>(key, value);
}

#endif

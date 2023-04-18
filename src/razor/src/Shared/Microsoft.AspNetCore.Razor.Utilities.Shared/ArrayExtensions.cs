// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor;

internal static class ArrayExtensions
{
    public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(
        this (TKey key, TValue value)[] array, IEqualityComparer<TKey> keyComparer)
        where TKey : notnull
        => array.ToImmutableDictionary(keySelector: t => t.key, elementSelector: t => t.value, keyComparer);
}

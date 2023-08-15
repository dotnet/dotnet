// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal static class ChecksumCache
{
    private static readonly ConditionalWeakTable<object, object> s_cache = new();

    public static bool TryGetValue<T>(T value, [NotNullWhen(true)] out Checksum? checksum)
        where T : class
    {
        if (s_cache.TryGetValue(value, out var result))
        {
            checksum = (Checksum)result;
            return true;
        }

        checksum = null;
        return false;
    }

    public static Checksum GetOrCreate<T>(T value, ConditionalWeakTable<object, object>.CreateValueCallback checksumCreator)
        where T : class
        => (Checksum)s_cache.GetValue(value, checksumCreator);
}

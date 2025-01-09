// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Utilities;

internal static class StringExtensions
{
#if NETFRAMEWORK
    public static bool StartsWith(this StringSpan span, string str) => span.StartsWith(str.AsSpan(), StringComparison.Ordinal);

    public static bool EndsWith(this StringSpan span, string str) => span.EndsWith(str.AsSpan(), StringComparison.Ordinal);

    public static bool EqualsOrdinal(this StringSpan span, string str) => span.Equals(str.AsSpan(), StringComparison.Ordinal);
#endif

    public static bool StartsWithIgnoreCase(this StringSpan span, string str) => span.StartsWith(str.AsSpan(), StringComparison.OrdinalIgnoreCase);

    public static bool EndsWithIgnoreCase(this StringSpan span, string str) => span.EndsWith(str.AsSpan(), StringComparison.OrdinalIgnoreCase);

    public static bool EqualsOrdinalIgnoreCase(this StringSpan span, string str) => span.Equals(str.AsSpan(), StringComparison.OrdinalIgnoreCase);
}

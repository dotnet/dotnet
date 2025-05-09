// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence;

internal static class StringExtensions
{
    /// <inheritdoc cref="string.IsNullOrEmpty(string)"/>
    internal static bool IsNullOrEmpty([NotNullWhen(returnValue: false)] this string? s) => string.IsNullOrEmpty(s);

    internal static string? NullIfEmpty(this string? str) => IsNullOrEmpty(str) ? null : str;

    internal static Guid? NullIfEmpty(this Guid? guid) => guid == Guid.Empty ? null : guid;

    internal static Guid? NullIfEmpty(this Guid guid) => guid == Guid.Empty ? null : guid;

    internal static bool EqualsOrdinal(this StringSpan span, StringSpan str) => MemoryExtensions.Equals(span, str, StringComparison.Ordinal);

    internal static bool EqualsOrdinalIgnoreCase(this StringSpan span, StringSpan str) => MemoryExtensions.Equals(span, str, StringComparison.OrdinalIgnoreCase);

#if NETFRAMEWORK

    internal static bool EqualsOrdinal(this StringSpan span, string str) => EqualsOrdinal(span, str.AsSpan());

    internal static bool EqualsOrdinalIgnoreCase(this StringSpan span, string str) => EqualsOrdinalIgnoreCase(span, str.AsSpan());

    internal static int IndexOf(this StringSpan span, string str) => span.IndexOf(str.AsSpan());

    internal static bool StartsWith(this StringSpan span, string str) => span.StartsWith(str.AsSpan());

    internal static bool StartsWith(this StringSpan span, string str, StringComparison comparisonType) => span.StartsWith(str.AsSpan(), comparisonType);

    internal static bool StartsWith(this string str, char value) => !str.IsNullOrEmpty() && str[0] == value;

    internal static bool EndsWith(this string str, char value) => !str.IsNullOrEmpty() && str[str.Length - 1] == value;

    internal static bool Contains(this StringSpan span, char c)
    {
        return span.IndexOf(c) >= 0;
    }

    internal static bool ContainsAny(this StringSpan span, string values)
    {
        return span.IndexOfAny(values.AsSpan()) >= 0;
    }

    internal static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
    {
        key = pair.Key;
        value = pair.Value;
    }

#endif

    internal static string Concat(scoped StringSpan first, scoped StringSpan second)
    {
#if NETFRAMEWORK
        return
            first.IsEmpty ? second.ToString() :
            second.IsEmpty ? first.ToString() :
            AlwaysConcat(first, second);

        static string AlwaysConcat(StringSpan first, StringSpan second)
        {
            int newLength = first.Length + second.Length;
            Span<char> buffer = newLength <= 1024 ? stackalloc char[newLength] : new char[newLength];
            first.CopyTo(buffer);
            second.CopyTo(buffer.Slice(first.Length));
            return buffer.ToString();
        }
#else
        return string.Concat(first, second);
#endif
    }
}

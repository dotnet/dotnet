// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace vstest.ProgrammerTests.Fakes;

internal static class EnumerableExtensions
{
    public static string JoinByComma<T>(this IEnumerable<T> value)
    {
        return value.JoinBy(", ");
    }

    public static string JoinBy<T>(this IEnumerable<T> value, string delimiter)
    {
        return string.Join(delimiter, value.Select(v => v?.ToString()));
    }

    public static List<T> AsList<T>(this T value)
    {
        return new List<T> { value };
    }
}

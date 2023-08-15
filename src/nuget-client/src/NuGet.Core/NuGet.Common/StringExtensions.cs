// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Common
{
    internal static class StringExtensions
    {
        internal static string? FormatWithDoubleQuotes(this string? s)
        {
            return s == null ? s : $@"""{s}""";
        }
    }
}

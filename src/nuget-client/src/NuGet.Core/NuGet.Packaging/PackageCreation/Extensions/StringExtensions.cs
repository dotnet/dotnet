// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Packaging
{
    [Obsolete("This class will be deleted in a future version of NuGet.Packaging.")]
    internal static class StringExtensions
    {
        [Obsolete("This class will be deleted in a future version of NuGet.Packaging.")]
        public static string? SafeTrim(this string? value)
        {
            return value == null ? null : value.Trim();
        }
    }
}

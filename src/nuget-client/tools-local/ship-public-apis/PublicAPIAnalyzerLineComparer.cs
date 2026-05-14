// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Internal.Tools.ShipPublicApis
{
    internal class PublicAPIAnalyzerLineComparer : IComparer<string>
    {
        public static PublicAPIAnalyzerLineComparer Instance { get; } = new();

        public int Compare(string? x, string? y)
        {
            if (x?[0] == '~')
            {
                x = x.Substring(1);
            }

            if (y?[0] == '~')
            {
                y = y.Substring(1);
            }

            return StringComparer.Ordinal.Compare(x, y);
        }
    }
}

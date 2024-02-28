// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.CommandLine.XPlat
{
    internal class ExactNormalTableFormatStrategy : ITableFormatStrategy
    {
        private readonly string[] _normalVerbosityTableHeaderForExactMatch = { "Package ID", "Version", "Owners", "Total Downloads" };
        private readonly int[] _normalColumnsToHighlight = { 0, 2 };

        public Table CreateTable()
        {
            return new Table(_normalColumnsToHighlight, _normalVerbosityTableHeaderForExactMatch);
        }
    }
}

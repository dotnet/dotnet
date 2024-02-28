// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.CommandLine.XPlat
{
    internal class ExactDetailedTableFormatStrategy : ITableFormatStrategy
    {
        private readonly string[] _detailedVerbosityTableHeaderForExactMatch = { "Package ID", "Version", "Owners", "Total Downloads", "Vulnerable", "Deprecation", "Project URL", "Description" };
        private readonly int[] _detailedColumnsToHighlight = { 0, 2, 6, 7 };

        public Table CreateTable()
        {
            return new Table(_detailedColumnsToHighlight, _detailedVerbosityTableHeaderForExactMatch);
        }
    }
}

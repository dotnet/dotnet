// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.CommandLine.XPlat
{
    internal class ExactMinimalTableFormatStrategy : ITableFormatStrategy
    {
        private readonly string[] _minimalVerbosityTableHeaderForExactMatch = { "Package ID", "Version" };
        private readonly int[] _minimalColumnsToHighlight = { 0 };

        public Table CreateTable()
        {
            return new Table(_minimalColumnsToHighlight, _minimalVerbosityTableHeaderForExactMatch);
        }
    }
}

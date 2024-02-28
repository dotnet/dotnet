// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.CommandLine.XPlat
{
    internal class MinimalTableFormatStrategy : ITableFormatStrategy
    {
        private readonly string[] _minimalVerbosityTableHeader = { "Package ID", "Latest Version" };
        private readonly int[] _minimalColumnsToHighlight = { 0 };

        public Table CreateTable()
        {
            return new Table(_minimalColumnsToHighlight, _minimalVerbosityTableHeader);
        }
    }
}

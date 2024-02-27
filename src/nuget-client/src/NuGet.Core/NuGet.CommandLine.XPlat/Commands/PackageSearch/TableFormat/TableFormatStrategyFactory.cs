// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.CommandLine.XPlat
{
    internal class TableFormatStrategyFactory
    {
        public static ITableFormatStrategy GetStrategy(PackageSearchVerbosity verbosity, bool exactMatch)
        {
            switch (verbosity)
            {
                case PackageSearchVerbosity.Minimal:
                    return exactMatch ? new ExactMinimalTableFormatStrategy() : new MinimalTableFormatStrategy();
                case PackageSearchVerbosity.Normal:
                    return exactMatch ? new ExactNormalTableFormatStrategy() : new NormalTableFormatStrategy();
                case PackageSearchVerbosity.Detailed:
                    return exactMatch ? new ExactDetailedTableFormatStrategy() : new DetailedTableFormatStrategy();
                default:
                    throw new ArgumentException("Invalid verbosity level");
            }
        }
    }
}

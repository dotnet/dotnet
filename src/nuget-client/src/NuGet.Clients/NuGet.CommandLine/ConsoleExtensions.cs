// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.CommandLine
{
    internal static class ConsoleExtensions
    {
        public static void PrintPackageSources(this IConsole console, IReadOnlyCollection<Configuration.PackageSource> packageSources)
        {
            console.WriteLine("Feeds used:");
            foreach (var packageSource in packageSources)
            {
                console.WriteLine("  " + packageSource.Source);
            }
            console.WriteLine();
        }
    }
}

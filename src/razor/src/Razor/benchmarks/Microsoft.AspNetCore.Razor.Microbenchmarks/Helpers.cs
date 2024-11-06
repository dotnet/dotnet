// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

internal static class Helpers
{
    private static string? s_repoRootPath;
    private static string? s_testAppsPath;

    public static string GetRepoRootPath()
    {
        return s_repoRootPath ??= GetRepoRootPathCore();

        static string GetRepoRootPathCore()
        {
            var current = new DirectoryInfo(AppContext.BaseDirectory);
            while (current is not null && !File.Exists(Path.Combine(current.FullName, "Razor.sln")))
            {
                current = current.Parent;
            }

            return current?.FullName ?? throw new InvalidOperationException("Could not find Razor.sln");
        }
    }

    public static string GetTestAppsPath()
    {
        return s_testAppsPath ??= Path.Combine(GetRepoRootPath(), "src", "Razor", "benchmarks", "testapps");
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SbrpTests;

internal static class PathUtilities
{
    public static string GetRepoRoot() =>
        (string)AppContext.GetData("SbrpTests.RepoRoot")!;

    public static string GetSourceBuildRepoRoot()
    {
        var artifactsDir = (string)AppContext.GetData("SbrpTests.ArtifactsDir")!;
        return Path.Combine(artifactsDir, "sb", "src");
    }

    public static string GetSourceBuildPackagesShippingDir()
    {
        var configuration = (string)AppContext.GetData("SbrpTests.Configuration")!;
        return Path.Combine(GetSourceBuildRepoRoot(), "artifacts", "packages", configuration, "Shipping");
    }
}

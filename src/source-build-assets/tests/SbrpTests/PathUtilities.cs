// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SbrpTests;

internal static class PathUtilities
{
    public static string GetRepoRoot() => (string)AppContext.GetData("SbrpTests.RepoRoot")!;

    public static string GetArtifactsPackagesDir() => (string)AppContext.GetData("SbrpTests.ArtifactsPackagesDir")!;

    public static string GetArtifactsReferenceOnlyPackagesDir() =>
        (string)AppContext.GetData("SbrpTests.ArtifactsReferenceOnlyPackagesDir")!;

    public static string GetPackageTypeDir(PackageType type)
        => type switch
        {
            PackageType.Reference => "referencePackages",
            PackageType.Text      => "textOnlyPackages",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, $"Unknown package type '{type}'")
        };
}

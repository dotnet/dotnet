// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;

namespace Microsoft.DotNet.Tests;

public static class AssetsLoader
{
    public static string AssetsDirectory => Path.Combine(Directory.GetCurrentDirectory(), "assets");

    public static Stream OpenAssetFile(string assetPath)
    {
        return File.OpenRead(GetAssetFullPath(assetPath));
    }

    public static string GetAssetFullPath(string assetPath)
    {
        return Path.Combine(AssetsDirectory, assetPath);
    }
}

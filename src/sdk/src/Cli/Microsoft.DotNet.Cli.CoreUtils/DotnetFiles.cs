// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET

using System.Reflection;

namespace Microsoft.DotNet.Cli.Utils;

public static class DotnetFiles
{
    private static string SdkRootFolder => Path.Combine(typeof(DotnetFiles).GetTypeInfo().Assembly.Location, "..");

    private static readonly Lazy<DotnetVersionFile> s_versionFileObject =
        new(() => new DotnetVersionFile(VersionFile));

    private static readonly Lazy<SdkComponentManifest> s_componentManifest =
        new(() => new SdkComponentManifest(ComponentsFile));

    /// <summary>
    /// The SDK ships with a .version file that stores the commit information and SDK version
    /// </summary>
    public static string VersionFile => Path.GetFullPath(Path.Combine(SdkRootFolder, ".version"));

    /// <summary>
    /// In VMR builds the SDK ships a .components.json file (a copy of the VMR source-manifest.json)
    /// that records every product repository the SDK was composed from. Absent for standalone builds.
    /// </summary>
    public static string ComponentsFile => Path.GetFullPath(Path.Combine(SdkRootFolder, ".components.json"));

    public static DotnetVersionFile VersionFileObject => s_versionFileObject.Value;

    public static SdkComponentManifest ComponentManifest => s_componentManifest.Value;
}

#endif

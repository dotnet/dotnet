// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace SbrpUtilities;

public static class CommonUtilities
{
    /// <summary>
    /// Finds the release version for a component in eng/Versions.props by matching
    /// the normalized (hyphen-stripped, case-insensitive) component name against
    /// properties ending in "ReleaseVersion".
    /// </summary>
    public static string? FindReleaseVersion(string versionsPropsPath, string componentName)
    {
        XDocument versionsProps = XDocument.Load(versionsPropsPath);
        string normalizedName = componentName.Replace("-", "", StringComparison.Ordinal).ToLowerInvariant();

        return versionsProps
            .Descendants()
            .FirstOrDefault(e => e.Name.LocalName.EndsWith("ReleaseVersion", StringComparison.Ordinal)
                && e.Name.LocalName.Replace("ReleaseVersion", "", StringComparison.Ordinal)
                    .Equals(normalizedName, StringComparison.OrdinalIgnoreCase))
            ?.Value;
    }

    /// <summary>
    /// Downloads a NuGet package from the sources configured in the NuGet.config
    /// found at <paramref name="settingsRoot"/>. Returns the package stream, or null
    /// if the package was not found in any source.
    /// </summary>
    public static async Task<MemoryStream?> DownloadPackageAsync(
        string settingsRoot, string packageId, string version, CancellationToken cancellationToken = default)
    {
        ISettings settings = Settings.LoadDefaultSettings(settingsRoot);
        PackageSourceProvider sourceProvider = new(settings);
        IEnumerable<PackageSource> sources = sourceProvider.LoadPackageSources().Where(s => s.IsEnabled);
        var providers = Repository.Provider.GetCoreV3();
        NuGetVersion nugetVersion = new(version);

        using SourceCacheContext cacheContext = new();

        foreach (PackageSource source in sources)
        {
            SourceRepository repository = new(source, providers);
            MemoryStream stream = new();
            try
            {
                FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>(cancellationToken);
                if (await resource.CopyNupkgToStreamAsync(
                    packageId, nugetVersion, stream, cacheContext, NullLogger.Instance, cancellationToken))
                {
                    stream.Position = 0;
                    return stream;
                }
            }
            catch
            {
            }

            stream.Dispose();
        }

        return null;
    }

    /// <summary>
    /// Downloads a NuGet package and returns the FileVersion revision from its first DLL.
    /// Combines <see cref="DownloadPackageAsync"/> with DLL extraction and version reading.
    /// Returns null if the package could not be downloaded or contains no DLL.
    /// Also returns the full FileVersion string when successful.
    /// </summary>
    public static async Task<(int? Revision, string? FileVersion)> GetFileVersionRevisionAsync(
        string settingsRoot, string packageId, string version, CancellationToken cancellationToken = default)
    {
        using MemoryStream? packageStream = await DownloadPackageAsync(settingsRoot, packageId, version, cancellationToken);
        if (packageStream is null)
        {
            return (null, null);
        }

        using PackageArchiveReader packageReader = new(packageStream);
        string? dllItem = packageReader.GetLibItems()
            .SelectMany(group => group.Items)
            .FirstOrDefault(item => item.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));

        if (dllItem is null)
        {
            return (null, null);
        }

        string tempDll = Path.Combine(Path.GetTempPath(), $"sbrp-{Guid.NewGuid():N}.dll");
        try
        {
            using (Stream dllStream = packageReader.GetStream(dllItem))
            using (FileStream fs = File.Create(tempDll))
            {
                dllStream.CopyTo(fs);
            }

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(tempDll);
            return (versionInfo.FilePrivatePart, versionInfo.FileVersion);
        }
        finally
        {
            try
            {
                File.Delete(tempDll);
            }
            catch
            {
            }
        }
    }
}

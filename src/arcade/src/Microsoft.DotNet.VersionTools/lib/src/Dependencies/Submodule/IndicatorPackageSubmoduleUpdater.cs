// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Dependencies.BuildOutput;
using NuGet.Packaging.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.DotNet.VersionTools.Dependencies.Submodule
{
    /// <summary>
    /// Downloads a package specified from a dependency build info and updates the target git
    /// submodule to match the version inside.
    /// </summary>
    public class IndicatorPackageSubmoduleUpdater : SubmoduleUpdater
    {
        private static readonly Lazy<HttpClient> DownloadClient = new Lazy<HttpClient>();

        /// <summary>
        /// The NuGet v2 base url to use to download the indicator package, without a trailing '/'.
        /// For example, https://dotnet.myget.org/F/dotnet-core/api/v2/package.
        /// </summary>
        public string PackageDownloadBaseUrl { get; set; }

        public string IndicatorPackageId { get; }

        public IndicatorPackageSubmoduleUpdater(string indicatorPackageId)
        {
            if (string.IsNullOrEmpty(indicatorPackageId))
            {
                throw new ArgumentException(nameof(indicatorPackageId), "An indicator package must be specified.");
            }
            IndicatorPackageId = indicatorPackageId;
        }

        protected override string GetDesiredCommitHash(
            IEnumerable<IDependencyInfo> dependencyInfos,
            out IEnumerable<IDependencyInfo> usedDependencyInfos)
        {
            foreach (var info in dependencyInfos.OfType<BuildDependencyInfo>())
            {
                PackageIdentity package = info.Packages
                    .FirstOrDefault(p => p.Id == IndicatorPackageId);

                if (package == null)
                {
                    continue;
                }

                using (ZipArchive archive = DownloadPackageAsync(info, package).Result)
                {
                    ZipArchiveEntry versionTxtEntry = archive.GetEntry("version.txt");
                    if (versionTxtEntry == null)
                    {
                        Trace.TraceWarning(
                            $"Downloaded '{package}' in '{info.BuildInfo.Name}' " +
                            $"to upgrade '{Path}', but it had no version.txt file. Skipping.");
                        continue;
                    }
                    using (Stream versionTxt = versionTxtEntry.Open())
                    using (var versionTxtReader = new StreamReader(versionTxt))
                    {
                        string packageCommitHash = versionTxtReader.ReadLine();
                        Trace.TraceInformation($"Found commit '{packageCommitHash}' in versions.txt.");

                        usedDependencyInfos = new[] { info };
                        return packageCommitHash;
                    }
                }
            }

            Trace.TraceError($"Failed to find '{IndicatorPackageId}' specifying a commit in any build-info.");
            usedDependencyInfos = Enumerable.Empty<BuildDependencyInfo>();
            return null;
        }

        protected async Task<ZipArchive> DownloadPackageAsync(BuildDependencyInfo info, PackageIdentity package)
        {
            if (PackageDownloadBaseUrl == null)
            {
                // This isn't checked in the constructor because build-info may contain a download URL in the future.
                throw new InvalidOperationException(
                    $"A {nameof(PackageDownloadBaseUrl)} must be configured, " +
                    "as build-infos do not have package feed details.");
            }

            string downloadUrl = $"{PackageDownloadBaseUrl}/package/{package.Id}/{package.Version}";
            Trace.TraceInformation($"Downloading '{package}' from '{downloadUrl}'");

            HttpClient client = DownloadClient.Value;
            Stream nupkgStream = await client.GetStreamAsync(downloadUrl);

            return new ZipArchive(nupkgStream);
        }
    }
}

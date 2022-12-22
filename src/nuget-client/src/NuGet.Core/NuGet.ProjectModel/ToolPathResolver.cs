// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Frameworks;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    public class ToolPathResolver
    {
        private readonly string _packagesDirectory;
        private readonly bool _isLowercase;

        public ToolPathResolver(string packagesDirectory)
            : this(packagesDirectory, isLowercase: true)
        {
        }

        public ToolPathResolver(string packagesDirectory, bool isLowercase)
        {
            _packagesDirectory = packagesDirectory;
            _isLowercase = isLowercase;
        }

        /// <summary>
        /// Given a toolDirectory path, it returns the full assets file path
        /// </summary>
        public string GetLockFilePath(string toolDirectory)
        {
            return Path.Combine(toolDirectory, LockFileFormat.AssetsFileName);
        }

        /// <summary>
        /// Given a package id, version and framework, returns the full assets file path
        /// </summary>
        public string GetLockFilePath(string packageId, NuGetVersion version, NuGetFramework framework)
        {
            return GetLockFilePath(GetToolDirectoryPath(packageId, version, framework));
        }

        /// <summary>
        /// Given a package id, version and framework, returns the tool directory path where the assets/cache file are located for tools
        /// </summary>
        public string GetToolDirectoryPath(string packageId, NuGetVersion version, NuGetFramework framework)
        {
            var versionString = version.ToNormalizedString();
            var frameworkString = framework.GetShortFolderName();

            if (_isLowercase)
            {
                packageId = packageId.ToLowerInvariant();
                versionString = versionString.ToLowerInvariant();
                frameworkString = frameworkString.ToLowerInvariant();
            }

            var basePath = GetPackagesToolsBasePath();

            return Path.Combine(
                basePath,
                packageId,
                versionString,
                frameworkString);
        }

        /// <summary>
        /// The base path for all restored tools
        /// </summary>
        private string GetPackagesToolsBasePath()
        {
            return Path.Combine(
                _packagesDirectory,
                ".tools");
        }

        /// <summary>
        /// Returns the directory (packagesFolder/.tools/id/version for example) for the best matching version if any. 
        /// </summary>
        /// <returns></returns>
        public string GetBestToolDirectoryPath(string packageId, VersionRange versionRange, NuGetFramework framework)
        {
            var availableToolVersions = GetAvailableToolVersions(packageId);

            var bestVersion = versionRange.FindBestMatch(availableToolVersions);
            if (bestVersion == null)
            {
                return null;
            }

            return GetToolDirectoryPath(packageId, bestVersion, framework);
        }

        /// <summary>
        /// Given a package id, looks in the base tools folder and returns all the version available on disk, possibly none
        /// </summary>
        private IEnumerable<NuGetVersion> GetAvailableToolVersions(string packageId)
        {
            var availableVersions = new List<NuGetVersion>();

            var toolBasePath = Path.Combine(GetPackagesToolsBasePath(), _isLowercase ? packageId.ToLowerInvariant() : packageId);
            if (!Directory.Exists(toolBasePath))
            {
                return Enumerable.Empty<NuGetVersion>();
            }

            var versionDirectories = Directory.EnumerateDirectories(toolBasePath);

            foreach (var versionDirectory in versionDirectories)
            {
                var version = Path.GetFileName(versionDirectory);

                if (NuGetVersion.TryParse(version, out var nugetVersion))
                {
                    availableVersions.Add(nugetVersion);
                }
            }

            return availableVersions;
        }
    }
}

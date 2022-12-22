// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.Packaging
{
    /// <summary>
    /// Reads packages.config
    /// </summary>
    public class PackagesConfigReader
    {
        private readonly XDocument _doc;
        private readonly IFrameworkNameProvider _frameworkMappings;

        /// <summary>
        /// Packages.config reader
        /// </summary>
        /// <param name="xml">Packages.config XML</param>
        public PackagesConfigReader(XDocument xml)
            : this(DefaultFrameworkNameProvider.Instance, xml)
        {
        }

        /// <summary>
        /// Packages.config reader
        /// </summary>
        /// <param name="frameworkMappings">Additional target framework mappings for parsing target frameworks</param>
        /// <param name="xml">Packages.config XML</param>
        public PackagesConfigReader(IFrameworkNameProvider frameworkMappings, XDocument xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException(nameof(xml));
            }

            if (frameworkMappings == null)
            {
                throw new ArgumentNullException(nameof(frameworkMappings));
            }

            _doc = xml;
            _frameworkMappings = frameworkMappings;
        }

        /// <summary>
        /// Packages.config reader
        /// </summary>
        /// <param name="stream">Stream containing packages.config</param>
        public PackagesConfigReader(Stream stream)
            : this(stream, false)
        {
        }

        /// <summary>
        /// Packages.config reader
        /// </summary>
        /// <param name="stream">Stream containing packages.config</param>
        /// <param name="leaveStreamOpen">True will leave the stream open</param>
        public PackagesConfigReader(Stream stream, bool leaveStreamOpen)
            : this(DefaultFrameworkNameProvider.Instance, stream, leaveStreamOpen)
        {
        }

        /// <summary>
        /// Packages.config reader
        /// </summary>
        /// <param name="stream">Stream containing packages.config</param>
        /// <param name="leaveStreamOpen">True will leave the stream open</param>
        /// <param name="frameworkMappings">Additional target framework mappings for parsing target frameworks</param>
        public PackagesConfigReader(IFrameworkNameProvider frameworkMappings, Stream stream, bool leaveStreamOpen)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (frameworkMappings == null)
            {
                throw new ArgumentNullException(nameof(frameworkMappings));
            }

            _frameworkMappings = frameworkMappings;
            _doc = XDocument.Load(stream);

            if (!leaveStreamOpen)
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// Reads the minimum client version from packages.config
        /// </summary>
        /// <returns>Minimum client version or the default of 2.5.0</returns>
        public NuGetVersion GetMinClientVersion()
        {
            NuGetVersion version = null;

            var node = _doc.Root.Attribute(XName.Get(PackagesConfig.MinClientAttributeName));

            if (node == null)
            {
                version = new NuGetVersion(2, 5, 0);
            }
            else if (!NuGetVersion.TryParse(node.Value, out version))
            {
                throw new PackagesConfigReaderException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.ErrorInvalidMinClientVersion,
                    node.Value));
            }

            return version;
        }

        /// <summary>
        /// Reads all package node entries in the config.
        /// If duplicate package ids exist an exception will be thrown.
        /// </summary>
        public IEnumerable<PackageReference> GetPackages()
        {
            return GetPackages(allowDuplicatePackageIds: false);
        }

        /// <summary>
        /// Reads all package node entries in the config.
        /// </summary>
        /// <param name="allowDuplicatePackageIds">If True validation will be performed to ensure that 
        /// only one entry exists for each unique package id.</param>
        public IEnumerable<PackageReference> GetPackages(bool allowDuplicatePackageIds)
        {
            var packages = new List<PackageReference>();

            foreach (var package in _doc.Root.Elements(XName.Get(PackagesConfig.PackageNodeName)))
            {
                string id = null;
                if (!PackagesConfig.TryGetAttribute(package, "id", out id)
                    || String.IsNullOrEmpty(id))
                {
                    throw new PackagesConfigReaderException(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.ErrorNullOrEmptyPackageId));
                }

                string version = null;
                if (!PackagesConfig.TryGetAttribute(package, PackagesConfig.VersionAttributeName, out version)
                    || String.IsNullOrEmpty(version))
                {
                    throw new PackagesConfigReaderException(string.Format(
                       CultureInfo.CurrentCulture,
                       Strings.ErrorInvalidPackageVersion,
                       id,
                       version));
                }

                NuGetVersion semver = null;
                if (!NuGetVersion.TryParse(version, out semver))
                {
                    throw new PackagesConfigReaderException(string.Format(
                       CultureInfo.CurrentCulture,
                       Strings.ErrorInvalidPackageVersion,
                       id,
                       version));
                }

                string attributeValue = null;
                VersionRange allowedVersions = null;
                if (PackagesConfig.TryGetAttribute(package, PackagesConfig.allowedVersionsAttributeName, out attributeValue))
                {
                    if (!VersionRange.TryParse(attributeValue, out allowedVersions))
                    {
                        throw new PackagesConfigReaderException(string.Format(
                            CultureInfo.CurrentCulture,
                            Strings.ErrorInvalidAllowedVersions,
                            id,
                            attributeValue));
                    }
                }

                var targetFramework = NuGetFramework.UnsupportedFramework;
                if (PackagesConfig.TryGetAttribute(package, PackagesConfig.TargetFrameworkAttributeName, out attributeValue))
                {
                    targetFramework = NuGetFramework.Parse(attributeValue, _frameworkMappings);
                }

                var developmentDependency = PackagesConfig.BoolAttribute(package, PackagesConfig.developmentDependencyAttributeName);
                var requireReinstallation = PackagesConfig.BoolAttribute(package, PackagesConfig.RequireInstallAttributeName);
                var userInstalled = PackagesConfig.BoolAttribute(package, PackagesConfig.UserInstalledAttributeName, true);

                var entry = new PackageReference(new PackageIdentity(id, semver), targetFramework, userInstalled, developmentDependency, requireReinstallation, allowedVersions);

                packages.Add(entry);
            }

            // check if there are duplicate entries
            var duplicates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            PackageIdentity lastIdentity = null;
            var comparer = PackageIdentity.Comparer;

            // Sort the list of packages and check for duplicates
            foreach (var package in packages.OrderBy(p => p.PackageIdentity, comparer))
            {
                if (lastIdentity != null)
                {
                    if (allowDuplicatePackageIds)
                    {
                        // Full compare
                        if (comparer.Equals(package.PackageIdentity, lastIdentity))
                        {
                            duplicates.Add(lastIdentity.ToString());
                        }
                    }
                    else if (string.Equals(
                        package.PackageIdentity.Id,
                        lastIdentity.Id,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        // Id only compare
                        duplicates.Add(lastIdentity.Id);
                    }
                }

                lastIdentity = package.PackageIdentity;
            }

            if (duplicates.Count > 0)
            {
                throw new PackagesConfigReaderException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.ErrorDuplicatePackages,
                    string.Join(", ", duplicates)));
            }

            return packages;
        }
    }
}

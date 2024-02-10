// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.ProjectManagement;

namespace NuGet.Test.Utility
{
    /// <summary>
    /// Represents a central package management file.
    /// </summary>
    public class CentralPackageVersionsManagementFile
    {
        private const string DirectoryPackagesProps = "Directory.Packages.props";

        private readonly bool? _managePackageVersionsCentrally;

        private readonly Dictionary<string, string> _packageVersions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, string> _globalPackageReferences = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly FileInfo _path;

        private CentralPackageVersionsManagementFile(string directoryPath, bool? managePackageVersionsCentrally)
        {
            _path = new FileInfo(Path.Combine(directoryPath, DirectoryPackagesProps));
            _managePackageVersionsCentrally = managePackageVersionsCentrally;
        }

        /// <summary>
        /// Gets the full path of the central package management file.
        /// </summary>
        public string FullPath => _path.FullName;

        /// <summary>
        /// Gets a value indicating whether or not there are any unsaved changes to the current central package management file.
        /// </summary>
        public bool IsDirty { get; private set; }

        /// <summary>
        /// Creates a new central package management file (Directory.Packages.props) in the specified directory.
        /// </summary>
        /// <param name="directoryPath">The path to a directory to create the central package management in.</param>
        /// <param name="managePackageVersionsCentrally"><see langword="true" /> to enable central package management (default), or <see langword="false" /> to disable it.</param>
        /// <returns></returns>
        public static CentralPackageVersionsManagementFile Create(string directoryPath, bool? managePackageVersionsCentrally = null)
        {
            return new CentralPackageVersionsManagementFile(directoryPath, managePackageVersionsCentrally);
        }

        /// <summary>
        /// Removes a central package version, you must call <see cref="Save" /> after modifying the package versions.
        /// </summary>
        /// <param name="packageId">The ID of the package.</param>
        /// <returns>The current <see cref="CentralPackageVersionsManagementFile" />.</returns>
        public CentralPackageVersionsManagementFile RemovePackageVersion(string packageId)
        {
            if (_managePackageVersionsCentrally == false)
            {
                return this;
            }

            _packageVersions.Remove(packageId);

            IsDirty = true;

            return this;
        }

        /// <summary>
        /// Saves the current central package management file.
        /// </summary>
        public void Save()
        {
            XElement managePackageVersionsCentrallyProperty = _managePackageVersionsCentrally is null
                ? null
                : new XElement("PropertyGroup", new XElement(ProjectBuildProperties.ManagePackageVersionsCentrally, new XText(_managePackageVersionsCentrally.ToString())));

            XDocument directoryPackagesPropsXml = new XDocument(
                new XElement("Project",
                    managePackageVersionsCentrallyProperty,
                    new XElement("ItemGroup", _packageVersions.Select(i => new XElement("PackageVersion", new XAttribute("Include", i.Key), new XAttribute("Version", i.Value)))),
                    new XElement("ItemGroup", _globalPackageReferences.Select(i => new XElement("GlobalPackageReference", new XAttribute("Include", i.Key), new XAttribute("Version", i.Value))))));

            directoryPackagesPropsXml.Save(_path.FullName);

            IsDirty = false;
        }

        /// <summary>
        /// Adds or sets a central package version, you must call <see cref="Save" /> after modifying the package versions.
        /// </summary>
        /// <param name="packageId">The ID of the package.</param>
        /// <param name="packageVersion">The version of the package.</param>
        /// <returns>The current <see cref="CentralPackageVersionsManagementFile" />.</returns>
        public CentralPackageVersionsManagementFile SetPackageVersion(string packageId, string packageVersion)
        {
            if (_managePackageVersionsCentrally == false)
            {
                return this;
            }

            _packageVersions[packageId] = packageVersion;

            IsDirty = true;

            return this;
        }

        public CentralPackageVersionsManagementFile SetGlobalPackageReference(string packageId, string packageVersion)
        {
            if (_managePackageVersionsCentrally == false)
            {
                return this;
            }

            _globalPackageReferences[packageId] = packageVersion;

            IsDirty = true;

            return this;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Deployment.WindowsInstaller.Package;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Utility methods for Windows Installer (MSI) packages.
    /// </summary>
    public class MsiUtils
    {
        /// <summary>
        /// Query string to retrieve all the rows from the MSI File table.
        /// </summary>
        private const string _getFilesQuery = "SELECT `File`, `Component_`, `FileName`, `FileSize`, `Version`, `Language`, `Attributes`, `Sequence` FROM `File`";

        /// <summary>
        /// Query string to retrieve all the rows from the MSI Upgrade table.
        /// </summary>
        private const string _getUpgradeQuery = "SELECT `UpgradeCode`, `VersionMin`, `VersionMax`, `Language`, `Attributes` FROM `Upgrade`";

        /// <summary>
        /// Query string to retrieve the dependency provider key from the WixDependencyProvider table.
        /// </summary>
        private const string _getWixDependencyProviderQuery = "SELECT `ProviderKey` FROM `WixDependencyProvider`";

        /// <summary>
        /// Query string to retrieve all the rows from the MSI Directory table.
        /// </summary>
        private const string _getDirectoriesQuery = "SELECT `Directory`, `Directory_Parent`, `DefaultDir` FROM `Directory`";

        /// <summary>
        /// Query string to retrieve all rows from the MSI Registry table.
        /// </summary>
        private const string _getRegistryQuery = "SELECT `Root`, `Key`, `Name`, `Value` FROM `Registry`";

        /// <summary>
        /// Gets an enumeration of all the files inside an MSI.
        /// </summary>
        /// <param name="packagePath">The path of the MSI package to query.</param>
        /// <returns>An enumeration of all the files.</returns>
        public static IEnumerable<FileRow> GetAllFiles(string packagePath)
        {
            using InstallPackage ip = new(packagePath, DatabaseOpenMode.ReadOnly);
            using Database db = new(packagePath, DatabaseOpenMode.ReadOnly);
            using View fileView = db.OpenView(_getFilesQuery);
            List<FileRow> files = new();
            fileView.Execute();

            foreach (Record fileRecord in fileView)
            {
                files.Add(FileRow.Create(fileRecord));
            }

            return files;
        }

        /// <summary>
        /// Gets an enumeration of all the directories inside an MSI.
        /// </summary>
        /// <param name="packagePath">The path of the MSI package to query.</param>
        /// <returns>An enumeration of all the directories.</returns>
        public static IEnumerable<DirectoryRow> GetAllDirectories(string packagePath)
        {
            using InstallPackage ip = new(packagePath, DatabaseOpenMode.ReadOnly);
            using Database db = new(packagePath, DatabaseOpenMode.ReadOnly);
            using View directoryView = db.OpenView(_getDirectoriesQuery);
            List<DirectoryRow> directories = new();
            directoryView.Execute();

            foreach (Record directoryRecord in directoryView)
            {
                directories.Add(DirectoryRow.Create(directoryRecord));
            }

            return directories;
        }

        /// <summary>
        /// Gets an enumeration of all the registry keys inside an MSI.
        /// </summary>
        /// <param name="packagePath">The path of the MSI package to query.</param>
        /// <returns>An enumeration of all the registry keys.</returns>
        public static IEnumerable<RegistryRow> GetAllRegistryKeys(string packagePath)
        {
            using InstallPackage ip = new(packagePath, DatabaseOpenMode.ReadOnly);
            using Database db = new(packagePath, DatabaseOpenMode.ReadOnly);
            using View view = db.OpenView(_getRegistryQuery);
            List<RegistryRow> registryKeys = new();
            view.Execute();

            foreach (Record directoryRecord in view)
            {
                registryKeys.Add(RegistryRow.Create(directoryRecord));
            }

            return registryKeys;
        }

        /// <summary>
        /// Gets an enumeration describing related products defined in the Upgrade table of an MSI
        /// </summary>
        /// <param name="packagePath">The path of the MSI package to query.</param>
        /// <returns>An enumeration of upgrade related products.</returns>
        public static IEnumerable<RelatedProduct> GetRelatedProducts(string packagePath)
        {
            using InstallPackage ip = new(packagePath, DatabaseOpenMode.ReadOnly);
            using Database db = new(packagePath, DatabaseOpenMode.ReadOnly);

            if (db.Tables.Contains("Upgrade"))
            {
                using View upgradeView = db.OpenView(_getUpgradeQuery);
                List<RelatedProduct> relatedProducts = new();
                upgradeView.Execute();

                foreach (Record relatedProduct in upgradeView)
                {
                    relatedProducts.Add(RelatedProduct.Create(relatedProduct));
                }

                return relatedProducts;
            }

            return Enumerable.Empty<RelatedProduct>();
        }

        /// <summary>
        /// Gets the dependency provider key from the MSI package.
        /// </summary>
        /// <param name="packagePath">The path of the MSI package to query.</param>
        /// <returns>The name of the provider key or <see langword="null" /> if the WixDependencyProvider table does not exist.</returns>
        public static string GetProviderKeyName(string packagePath)
        {
            using InstallPackage ip = new(packagePath, DatabaseOpenMode.ReadOnly);
            using Database db = new(packagePath, DatabaseOpenMode.ReadOnly);

            if (db.Tables.Contains("WixDependencyProvider"))
            {
                using View depProviderView = db.OpenView(_getWixDependencyProviderQuery);
                depProviderView.Execute();

                Record providerKey = depProviderView.First();

                return providerKey != null ? (string)providerKey["ProviderKey"] : null;
            }

            return null;
        }

        /// <summary>
        /// Extracts the specified property from the MSI Property table.
        /// </summary>
        /// <param name="packagePath">The path to the MSI package.</param>
        /// <param name="property">The name of the property to extract.</param>
        /// <returns>The value of the property.</returns>
        public static string GetProperty(string packagePath, string property)
        {
            using InstallPackage ip = new(packagePath, DatabaseOpenMode.ReadOnly);
            return ip.Property[property];
        }

        /// <summary>
        /// Gets the ProductVersion property of the specified MSI.
        /// </summary>
        /// <param name="packagePath">The path to the MSI package.</param>
        /// <returns>The ProductVersion property.</returns>
        public static Version GetVersion(string packagePath) =>
            new Version(GetProperty(packagePath, MsiProperty.ProductVersion));

        /// <summary>
        /// Calculates the number of bytes a Windows Installer Package would consume on disk. The function assumes that all files will be installed.
        /// </summary>
        /// <param name="packagePath">The path to the MSI package.</param>
        /// <param name="factor">Multiplication factor to use to account for additional space requirements such as registry entries for components 
        /// in the installer database.</param>
        /// <returns>The number of bytes required to install the MSI.</returns>
        public static long GetInstallSize(string packagePath, double factor = 1.4) =>
            GetAllFiles(packagePath).Sum(f => Convert.ToInt64(f.FileSize * factor));

        /// <summary>
        /// Validates that a <see cref="Version"/> represents a valid MSI ProductVersion.
        /// </summary>
        /// <param name="version">The version to validate.</param>
        /// <exception cref="ArgumentOutOfRangeException" />
        public static void ValidateProductVersion(Version version)
        {
            // See to https://learn.microsoft.com/en-us/windows/win32/msi/productversion for additional information.

            if (version.Major > 255)
            {
                throw new ArgumentOutOfRangeException(string.Format(Strings.MsiProductVersionOutOfRange, nameof(version.Major), 255));
            }
                
            if (version.Minor > 255)
            {
                throw new ArgumentOutOfRangeException(string.Format(Strings.MsiProductVersionOutOfRange, nameof(version.Minor), 255));
            }

            if (version.Build > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(string.Format(Strings.MsiProductVersionOutOfRange, nameof(version.Build), ushort.MaxValue));
            }
        }

        /// <summary>
        /// Determines if the MSI contains a specific table.
        /// </summary>
        /// <param name="packagePath">The path to the MSI package.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <returns><see langword="true"/> if the table exists; <see langword="false"/> otherwise.</returns>
        public static bool HasTable(string packagePath, string tableName)
        {
            using InstallPackage ip = new(packagePath, DatabaseOpenMode.ReadOnly);
            using Database db = new(packagePath, DatabaseOpenMode.ReadOnly);

            return db.Tables.Contains(tableName);
        }
    }
}
